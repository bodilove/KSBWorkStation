using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;
namespace MainControl
{
    public class ProcessConfig
    {
        long allcount = 0;
        public P_Process Currentprocess = null;
        public Common.Product.Model.ProductConfigInfo CurrentProductConfigInfo = null;
        public Dictionary<string, DetailStation> DicStationHandle = new Dictionary<string, DetailStation>();//StationNum
        public List<P_Detail> P_Detaillst = new List<P_Detail>();

        public bool Load(P_Process P)
        {
            try
            {
                
                Currentprocess = P;
                P_Detaillst = MdlClass.p_Detailsbll.GetDetailList(10, 1, ref allcount, "AND [ProsseId]=" + Currentprocess.Id + "  ORDER BY [OrderNum] ASC", false).ToList();
                foreach (P_Detail pd in P_Detaillst)
                {
                    DetailStation st = new DetailStation();
                    st.Init(this, pd);
                    DicStationHandle.Add(st.CurrentDetail.StationNum, st);
                }
                CurrentProductConfigInfo = MdlClass.productConfigInfoBbLL.SelectProductConfigInfoByProductNum(Currentprocess.ProductNum);
                if (CurrentProductConfigInfo == null)
                {
                    throw new Exception("当前工艺产品无法读取客户料号");
                }
              
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
    public class DetailStation
    {
        long allcount = 0;
        public P_Detail CurrentDetail = null;
        public   List<P_BomPart> bomparts = new List<P_BomPart>();
        public List<P_TestItemConfig> TestItemConfiglst = new List<P_TestItemConfig>();
        public ProcessConfig Parent = null;
        public void Init(ProcessConfig CurrentParent, P_Detail dl)
        {
            try
            {
                bomparts.Clear();
                //还需读取本站配置
                Parent = CurrentParent;
                CurrentDetail = dl;
              

                bomparts = MdlClass.p_BomPartBLL.GetBomPartlList(10, 1, ref allcount, "AND [ProcessId]=" + Parent.Currentprocess.Id + "AND [DetailsId]=" + CurrentDetail.Id + "  ORDER BY [OrderNum] ASC", false).ToList();
                TestItemConfiglst = MdlClass.p_TestItemConfigbll.GetTestItemConfigList(10, 1, ref allcount, "AND [ProcessId]=" + Parent.Currentprocess.Id + "AND [DetailsId]=" + CurrentDetail.Id + "  ORDER BY [OrderNum] ASC", false).ToList();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
