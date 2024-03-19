using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Runtime.Serialization;
using Common.StationProduct.Model;
using Common.Process.Model;
namespace MainControl
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TestDataServer : ITestDataServer
    {

        ITestDataServerCallback slaveStationCallback = null;
        OperationContext oc = null;


        //EOL扫码扫到SN，通过SN找到UUTID
        //进站
        //EOL开始测试
        //通过UUTID找到STATIONID
        //上传装配记录和测试记录
        //出站

        //返回当前工艺
        public P_Process ReturnCurrentprocess(ref string Erro)
        {
            try
            {
                if (MdlClass.CurrentProcessConfig != null&& MdlClass.Working)
                {
                    if (MdlClass.CurrentProcessConfig.Currentprocess != null)
                    {
                        return MdlClass.CurrentProcessConfig.Currentprocess;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception err)
            {
                Erro = err.ToString();
                return null;
            }
        }



        /// <summary>
        ///正常生产
        /// </summary>
        /// <returns></returns>
        public long EnterLine_Normal(ref string newSN)
        {
            long uuid = MdlClass.lineSever.Enter(WorkType.Normal, "", "", ref newSN);
            return uuid;
        }


        /// <summary>
        /// 返工进线 返回-1 返工进线失败
        /// </summary>
        /// <returns></returns>
        public long EnterLine_ReWork(string SN,ref string ResultAndPath)
        {
            string newSN = "";
           
            long uuid = -1;
            Common.Product.Model.ProductInfo pn = null;
            ResultAndPath=   MdlClass.IsCanReWork(SN,ref pn);
            if (ResultAndPath.StartsWith("OK"))
            {
                uuid = MdlClass.lineSever.Enter(WorkType.ReWork, ResultAndPath, SN, ref newSN);
            }
            return uuid;
        }



        public string LeaveLine(long uuid,EndType endType, Result res)
        {
            try
            {
                MdlClass.lineSever.Leave(uuid, endType, res);
                return "OK";
            }
            catch(Exception ex)
            {
                return "NG:" + ex.ToString();
            }
        }

        public string EnterStation(string SationNum, long uuid, Result res)
        {
            try
            {
                if (MdlClass.lineSever.DicStationControl[SationNum].EnterStation(uuid, res))
                {
                    return "OK";
                }
                else
                {
                    return "NG_Add";
                }
            }
            catch (Exception ex)
            {
                return "Erro:" + ex.ToString();
            }
  
        }

        public string leaveStation(string SationNum, long uuid, Result res)
        {
            try
            {

                Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl[SationNum].GetSationInfo(uuid);
                MdlClass.lineSever.DicStationControl[SationNum].LeaveStation(pd, res);

                return "OK";

            }
            catch (Exception ex)
            {
                return "Erro:"+ex.ToString();
            }
        }

        public Common.Product.Model.InWorkP_Info GetInfoByUUTid(long uutid)
        {
             Common.Product.Model.InWorkP_Info pd = MdlClass.inWorkBll.SelectInWorkProductInfo(uutid);
             if (pd != null)
             {
                 return pd;
             }
             else
             {
                 return null;
             }
        }

        public Common.Product.Model.InWorkP_Info GetInfoBySN(string SN)
        {
            List<Common.Product.Model.InWorkP_Info> pdlst = MdlClass.inWorkBll.SelectInWorkProductInfo(SN);
            if (pdlst.Count < 1) return null;
            if (pdlst != null || pdlst.Count == 1)
            {
                return pdlst[0];
            }
            else
            {
                return null;
            }
        }


        public string SaveAssembleRecord(List<AssembleRecord> AssembleRecordlst, string SationNum)
        {
           //Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl[SationNum].GetSationInfo((long)uutid);

            try
            {
                MdlClass.lineSever.DicStationControl[SationNum].AssembleRecordlst.Clear();

                foreach (AssembleRecord ard in AssembleRecordlst)
                {
                    MdlClass.lineSever.DicStationControl[SationNum].AssembleRecordlst.Add(ard);
                }
                MdlClass.lineSever.DicStationControl[SationNum].SaveAssembleRecord();
                return "OK_AssembleRecord";
            }
            catch (Exception ex)
            {
                return "NG_AssembleRecord:" + ex.ToString();
            }
        }

        public string SaveTestRecordlst(List<TestRecord> TestRecordlst, string SationNum)
        {
            try
            {
                MdlClass.lineSever.DicStationControl[SationNum].TestRecordlst.Clear();

                foreach (TestRecord ard in TestRecordlst)
                {
                    MdlClass.lineSever.DicStationControl[SationNum].TestRecordlst.Add(ard);
                }
                MdlClass.lineSever.DicStationControl[SationNum].SaveTestRecord();
                return "OK_TestRecord";
            }
            catch (Exception ex)
            {
                return "NG_TestRecord:" + ex.ToString();
            }
        }


        public string  UpProductSN(string ProductSN, long uutid)
        {
            string res = "";
            Common.Product.Model.InWorkP_Info pd = MdlClass.inWorkBll.SelectInWorkProductInfo(uutid);
            if (pd != null)
            {
                pd.ProductSN = ProductSN;

                if (MdlClass.inWorkBll.UpdateByUUTestId(pd))
                {
                    res = "OK";
                }
                else
                {
                    res = "NG,更新的产品失败。uutid:" + uutid + "ProductSN:" + ProductSN;
                }
            }
            else
            {
                res = "NG,未找到uutid:" + uutid + ",的产品。";
            }
            return res;
        }
        

        public void DoWork()
        {
            oc = OperationContext.Current;
            slaveStationCallback = oc.GetCallbackChannel<ITestDataServerCallback>();
            Console.WriteLine("sssssss");
        }

        public string DoWork2(string s)
        {
            //oc = OperationContext.Current;
            //slaveStationCallback = oc.GetCallbackChannel<ITestDataServerCallback>();
            Console.WriteLine("sssssss");
            return "Input: "+s+": "+System.DateTime.Now.ToString();
        }


        public string ControlClient(string s)
        {

            if (oc != null)
            {
                return slaveStationCallback.GetUUT2(s);
            }
            return "NULL";
        }



        
    }

  

}


