using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Common.Process.Model;
using Common.SysConfig.Model;
using System.Windows.Forms;
namespace MainControl
{
    public class LineService
    {
        long allcount = 0;
        public ProcessConfig CurrentprocessConfig = null;
       
        public ConcurrentDictionary<string, StationService> DicStationControl = new ConcurrentDictionary<string, StationService>();//StationNum
        TabControl CurrentTabControl = null;

        public void Init(TabControl tabSation)
        {
            CurrentTabControl = tabSation;
            if (CurrentTabControl != null)
            {
                CurrentTabControl.TabPages.Clear();
            }
            DicStationControl.Clear();
            //CurrentprocessConfig = P;
            //DicStationControl.Clear();
            foreach (LocalStationConfig lcost in MdlClass.sysSet.localStlst)
            {
                StationService stclt = new StationService();
                stclt.Init(this, lcost);
                if (CurrentTabControl != null)
                {
                    CurrentTabControl.TabPages.Add(stclt.tabPage);
                }
                DicStationControl.TryAdd(lcost.StationNum, stclt);
            }
        }

        public void Dispose()
        {
            DicStationControl.Clear();
            if (CurrentTabControl != null)
            {
                CurrentTabControl.TabPages.Clear();
            }
        }


        public bool SelectProcess(ProcessConfig P)
        {
            CurrentprocessConfig = P;
            bool res = true;
            string ErroStaton = "";
            foreach (KeyValuePair<string, StationService> kp in DicStationControl)
            {
                kp.Value.Selcet(false,null,null);
            }

      
            //检查总站工艺路径的站是否都包含
            foreach(KeyValuePair<string, DetailStation> kp in  CurrentprocessConfig.DicStationHandle)
            {
                if (!DicStationControl.ContainsKey(kp.Key))
                {
                    res = false;
                    ErroStaton = kp.Key;
                    break;
                }
                else
                {
                    DicStationControl[kp.Key].Selcet(true,kp.Value.bomparts,kp.Value.TestItemConfiglst);
                }
            }

            return res;
        }


        public void Start()
        {
            //开始流程
            //1.上位机询问PLC内部是否有产品


            //2.如果是PLC内部是否有产品，则获取PLC变种号，继续需生产。
            //3.如果PLC是未生产状态，则发给PLC当前上位机选择的变种号。
            //4.开始生产


            //类型，有无


            //结束流程
            //1.如果PLC内部有产品，需要用户切到清线模式清线
            //2.如果PLC内部无产品，则结束生产

            //通过工艺路径拿到客户料号。
          
            foreach (KeyValuePair<string, StationService> kp in DicStationControl)
            {
                kp.Value.Start();
            }
        }



        public void Stop()
        {

            foreach (KeyValuePair<string, StationService> kp in DicStationControl)
            {
                kp.Value.Stop();
            }
            CurrentprocessConfig = null;
        }


        public Common.Product.BLL.ProductInWork_FinishWork ProductInWork_FinishWorkbll = new Common.Product.BLL.ProductInWork_FinishWork();
        Common.Product.BLL.StatisticBLL statisticbll = new Common.Product.BLL.StatisticBLL();


        private long NormalEnterLine(string ExpectedPathNames,ref string NewSN)
        {
            long uutid = -1;
            //1.申请RollingNum

            // 2.生成SN号

            Common.Product.Model.ProductInfo pd = new Common.Product.Model.ProductInfo();

            pd.UserID = MdlClass.userInfo.UserNum;
            pd.UserName = MdlClass.userInfo.UserName;


            pd.StartTime = System.DateTime.Now;
            pd.EndTime = System.DateTime.Now;
            pd.PrdType = (int)WorkType.Normal;//1正常测试，//2代表返工，3代表点检产品。
            pd.PartNum = CurrentprocessConfig.Currentprocess.ProductNum;
            pd.ProductSN = "";
            pd.Result = (int)MainControl.Result.NoResult;//1代表产品pass，2代表fail
            pd.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。

            pd.ExpectedPath = 0;
            //获取当前的工艺路径

     
            int RollingNum = statisticbll.requestDayRollingNum(CurrentprocessConfig.Currentprocess.ProductNum);
            //之后多态实现动态加载SN生成器
            NewSN = MdlClass.sncreat.CreatSN(RollingNum, CurrentprocessConfig.CurrentProductConfigInfo);
            pd.SN = NewSN;

            pd.ExpectedPathNames = ExpectedPathNames;
            pd.ActualPath = 0;
            pd.ActualResult = 0;
            pd.StartTime = System.DateTime.Now;


            string FirstStation = GetStartStation(pd.ExpectedPathNames);
           
            uutid = ProductInWork_FinishWorkbll.NormalEnterLine(pd, FirstStation);
            return uutid;
        }

        private long ReWorkEnterLine(string ExpectedPathNames, string SN)//成品返工
        {
            long uutid = -1;
            ////1.申请RollingNum
            //int RollingNum = statisticbll.requestDayRollingNum(Currentprocess.ProductNum);
            //// 2.生成SN号
            //string SN = "PartNum,ee" + RollingNum;
            Common.Product.Model.ProductInfo pd = new Common.Product.Model.ProductInfo();

            pd.UserID = MdlClass.userInfo.UserNum;
            pd.UserName = MdlClass.userInfo.UserName;
            pd.SN = SN;
            pd.ProductSN = "";
            pd.StartTime = System.DateTime.Now;
            pd.EndTime = System.DateTime.Now;
            pd.PrdType = (int)WorkType.ReWork;//1正常测试，//2代表返工，3代表点检产品。
            pd.PartNum = CurrentprocessConfig.Currentprocess.ProductNum;
            pd.Result = 1;//1代表产品pass，2代表fail
            pd.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。

            pd.ExpectedPath = 0;
            pd.ExpectedPathNames = ExpectedPathNames;
            pd.ActualPath = 0;
            pd.ActualResult = 0;
            string FirstStation = GetStartStation(pd.ExpectedPathNames);
            uutid = ProductInWork_FinishWorkbll.ReWorkEnterLine(pd, FirstStation);
            return uutid;
        }

        private long PowerOnCheckEnterLine( string ExpectedPathNames,string SN)
        {
            long uutid = -1;
            Common.Product.Model.ProductInfo pd = new Common.Product.Model.ProductInfo();


            pd.UserID = MdlClass.userInfo.UserNum;
            pd.UserName = MdlClass.userInfo.UserName;
            pd.SN = SN;


            pd.StartTime = System.DateTime.Now;
            pd.EndTime = System.DateTime.Now;
            pd.PrdType = (int)WorkType.PowerOnCheck; ;//1正常测试，//2代表返工，3代表点检产品。
            pd.PartNum =CurrentprocessConfig. Currentprocess.ProductNum;
            pd.Result = 1;//1代表Pass,2代表NG
            pd.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。

            pd.ExpectedPath = 0;
            pd.ExpectedPathNames = ExpectedPathNames;
            pd.ActualPath = 0;
            pd.ActualResult = 0;
            string FirstStation = GetStartStation(pd.ExpectedPathNames);
            uutid = ProductInWork_FinishWorkbll.PowerOnCheckEnterLine(pd, FirstStation);
            return uutid;
        }

        public long Enter(WorkType workType, string ExpectedPathNames, string SN,ref string NewSN)
        {
            try
            {
                long uutid = -1;
                switch (workType)
                {
                    case WorkType.Normal:
                        string ExpectedPathNums = "";
                        //foreach (P_Detail pddd in CurrentprocessConfig.P_Detaillst)
                        for (int i = 0; i < CurrentprocessConfig.P_Detaillst.Count; i++)
                        {
                            if (i > 0)
                            {
                                ExpectedPathNums += "," + CurrentprocessConfig.P_Detaillst[i].StationNum;
                            }
                            else
                            {
                                ExpectedPathNums = CurrentprocessConfig.P_Detaillst[i].StationNum;
                            }
                        }


                        uutid = NormalEnterLine(ExpectedPathNums, ref NewSN);
                        break;
                    case WorkType.ReWork:
                        uutid = ReWorkEnterLine(ExpectedPathNames, SN);
                        break;
                    case WorkType.PowerOnCheck:
                        uutid = PowerOnCheckEnterLine(ExpectedPathNames, SN);
                        break;
                }
                if (uutid< 0)
                {
                    throw new Exception("向服务器数据库申请uutId失败。");
                }
                return uutid;
            }
            catch(Exception ex)
            {
                throw new Exception("进线失败。生产模式：" + workType.ToString() + ",异常原因:" + ex.ToString());
            }
        }

        public void Leave(long uutid, EndType endType ,Result res)
        {
            try
            {
                //判断每个站点是否都为PASS
                if (!ProductInWork_FinishWorkbll.ProductEnd(uutid, (int)endType, (int)res))
                {
                    throw new Exception("出站失败。");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("离站失败。产品UUTID：" + uutid +",产品离线状态:"+endType.ToString()+ ",异常原因:" + ex.ToString());
            }
        }

        public string GetStartStation(string ExpectedPathNames)
        {
            string[] SationNums = ExpectedPathNames.Split(',');
            if (SationNums.Length > 0)
            {
                return SationNums[0];
            }
            else
            {
                throw new Exception("工艺路径有错误!");
            }
        }
    }
}
