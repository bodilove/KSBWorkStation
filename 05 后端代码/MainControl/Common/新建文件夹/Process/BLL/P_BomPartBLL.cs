using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;

namespace Common.Process.BLL
{
    public class P_BomPartBLL
    {
        /// <summary>
        /// 根据岗位ID返回岗位实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public P_BomPart GetBomPartInfoById(int id)
        {
            return GlobalResources.GetInfo<P_BomPart>("P_BomPart", "Id", id);
        }


        /// <summary>
        /// 返回岗位List
        /// </summary>
        /// <param name="record"></param>
        /// <param name="currentpage"></param>
        /// <param name="allines"></param>
        /// <param name="condion"></param>
        /// <param name="IsPage"></param>
        /// <returns></returns>
        public List<P_BomPart> GetBomPartlList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            if (IsPage)
            {
                return GlobalResources.GetPages<P_BomPart>("P_BomPart", record, currentpage, ref allines, "Id", condion);
            }
            else
            {
                return GlobalResources.GetListInfo<P_BomPart>("P_BomPart", condion);
            }
        }


       

    


        /// <summary>
        /// 添加、修改、删除岗位
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="IdCollect"></param>
        /// <param name="operType"></param>
        /// <returns></returns>
        public bool AddUpdateDeleteP_BomPart(P_BomPart P_bomPart, string condition, string operType)
        {
            bool yorn = false;
            switch (operType.ToUpper())
            {
                case "ADD":
                    yorn = GlobalResources.Add<P_BomPart>(P_bomPart, "P_BomPart");
                    break;
                case "UPDATE":
                    yorn = GlobalResources.Update<P_BomPart>(P_bomPart, "P_BomPart", condition);
                    break;
                case "DELETE":
                    yorn = GlobalResources.Delete("P_BomPart", condition);
                    break;
                default:
                    break;
            }
            return yorn;
        }

        //public bool AddP_DetailList(string product, List<P_BomPart> hlist)
        //{
        //    bool yorn = false;
        //    if (AddUpdateDeleteP_BomPart(null, string.Format(" and ProductNum='{0}'", product), "DELETE"))
        //    {
        //        for (int i = 0; i < hlist.Count; i++)
        //        {
        //            AddUpdateDeleteP_BomPart(hlist[i], "", "ADD");
        //        }
        //        yorn = true;
        //    }
        //    return yorn;
        //}
        public bool updateOrderNum(int id, int OrderNum,int ForeWarningNum,int WarningNum,int Enable )
        {
            return GlobalResources.excsql("P_BomPart", "[OrderNum]=" + OrderNum + ",[ForeWarning]=" + ForeWarningNum + ",[Warning]=" + WarningNum + ",[Enable]=" + Enable, "AND [Id]=" + id);
        }
    
    }
}
