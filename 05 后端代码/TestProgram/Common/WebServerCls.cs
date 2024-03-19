using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;
using System.Runtime.Serialization;
namespace Common
{
    public class WebServerCls
    {
       public SVCForSlaveStationCallback callbackObject = null;
       InstanceContext clientContext = null;
       public    WebServer1.SVCForSlaveStationClient master = null;
       public WebServer1.SlaveStation sss = new WebServer1.SlaveStation();
       public static object ssslockobj = new object();


       public WebServerCls()
       {
           try
           {


               callbackObject = new SVCForSlaveStationCallback();
               callbackObject.sss = sss;
            clientContext = new InstanceContext(callbackObject);

            master = new WebServer1.SVCForSlaveStationClient(clientContext);

            sss.SlaveStationID = int.Parse(MdlClass.stationID);
            sss.SlaveStationName =MdlClass.stationName;
            sss.LineNo =int.Parse( MdlClass.LineNo);
               
            }
            catch (Exception ex)
            {

                Console.WriteLine("####WebServerCls：" + ex.Message + "####");
            }
        }

        //发送心跳包
        public bool SendHeartBeat(int plcstatus) 
        {
            bool yorn = true;
            try
            {
          
                switch (plcstatus)
                {
                    case 0:
                        sss.SlaveStationState = WebServer1.SlaveStationState.FailProductLimitWaring;
                        break;
                    case 1:
                        sss.SlaveStationState = WebServer1.SlaveStationState.EmergencyStop;
                        break;
                    case 2:
                        sss.SlaveStationState = WebServer1.SlaveStationState.Running;
                        break;
                    case 3:
                        sss.SlaveStationState = WebServer1.SlaveStationState.Repairing;
                        break;
                    default:
                        break;
                }
                master.HeartBeat(sss);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }

        public WebServer1.QueryProduct Query(string TrayNo)
        {
            WebServer1.QueryProduct qp = new WebServer1.QueryProduct();
            qp.TrayNo = TrayNo;
            WebServer1.QueryProduct qp1 =  master.QueryFOB(qp);
            return qp1;
        }

        public void SetSlaveStation(int passcount, int failcount)
        {
            lock (ssslockobj)
            {
                sss.Pass = passcount;
                sss.Fail = failcount;
                sss.AllCount = passcount + failcount;
            }
        }

        public void SetCurrentProductSN(string CurrentProductSN)
        {
            lock (ssslockobj)
            {
                sss.CurrentProductSN = CurrentProductSN;
            }
        }

        public void SetClientIsbusy(int ClientIsbusy)
        {
            lock (ssslockobj)
            {
                sss.ClientIsBusy = ClientIsbusy;
            }
        }
        //发送UUT
        public bool SendUUT(WebServer1.UUTTestResult uut)
        {
            bool yorn = true;
            try
            {
                yorn=master.UpUUT(uut);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }


        //发送测试结果
        public bool SendStationTestResult(WebServer1.StationTestResult sst)
        {
            bool yorn = true;
            try
            {
                yorn=master.UpStationTestResult(sst);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }

        //发送测试结果
        public bool SendTestOver(WebServer1.TestOver testover) 
        {
            bool yorn = true;
            try
            {
                yorn = master.UpTestover(testover);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }

        //登录
        public int Login(string id,string password) 
        {
            int usertype = -1; 
            try
            {
                WebServer1.User user= master.Login(id, password, sss.LineNo, sss.SlaveStationID);
                usertype  = (int)user.UserType;
                MdlClass.user = user;
            }
            catch (Exception)
            {

                usertype = -1;
            }
            return usertype;
        }


        //登出
        public bool Logout()
        {
            bool yorn = true;
            try
            {
                yorn = master.LogOut(sss.LineNo, sss.SlaveStationID, MdlClass.user);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }


        //断开连接
        public bool CloseConect()
        {
            bool yorn = true;
            try
            {
                yorn = master.SlaveStationDisConnect( sss.SlaveStationID, sss.LineNo);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }
        public void SetDateTimeFromServer()
        {
            try
            {
                WebServer1.ServerDateTime st = master.TimeSynchronization();
                setDateTime.SystemTime sdt = new setDateTime.SystemTime();
                sdt.wDay = st.wDay;
                sdt.wDayOfWeek = st.wDayOfWeek;
                sdt.wHour = st.wHour;
                sdt.wMiliseconds = st.wMiliseconds;
                sdt.wMinute = st.wMinute;
                sdt.wMonth = st.wMonth;
                sdt.wSecond = st.wSecond;
                sdt.wYear = st.wYear;
                setDateTime.SystemDateTime.SetLocalTime(ref sdt);
            }
            catch
            { }
        }
        public void SetAllTestMode(string Mode, string[] keys)
        {
            //string[] keys =new string[]{"2_2"};
            try
            {
                master.SetALLTestMode(Mode, keys);
            }
            catch
            {
 
            }
        }

        public bool upPCBSN(string TargetSN, string PCBSN)
        {
            return master.ModifyFOBPCBSN(TargetSN, PCBSN);
        }
    }

      


    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class SVCForSlaveStationCallback : WebServer1.ISVCForSlaveStationCallback
    {

        public static object lockobj = new object();

        public delegate void SetTestSeqcallbacked(int ID);
        public delegate void belogIncallbacked();
        public WebServer1.SlaveStation sss = null;
        public SetTestSeqcallbacked _setTestSeqcallbacked = null;
        public belogIncallbacked _belogIncallbacked = null;
        //public ReadPLC readPLC = null;
        public void HeartBeatCallback(string msg)
        {
            MessageBox.Show("回调");
        }
        public void SendSlaveStation(string msg)
        {
            MessageBox.Show("来自主控客户端的信息:" + msg);
        }

        /// <summary>
        /// 改变客户端是否为测试模式还是放板模式 //TestMode or EndMode
        /// </summary>
        /// <param name="msg"></param>
        public void SendSlaveTestMode(string msg)
        {
            //if (msg == "TestMode")
            //{
            //    MdlClass.readplc.ISTestMode = true;
            //    if (MdlClass.LabelTestModeState != null)
            //    {
            //        MdlClass.LabelTestModeState.Invoke(new Action(() => { MdlClass.LabelTestModeState.Text = "当前模式:自动模式"; }));
            //    }
            //}
            //else
            //{
            //    MdlClass.readplc.ISTestMode = false;
            //    if (MdlClass.LabelTestModeState != null)
            //    {
            //        MdlClass.LabelTestModeState.Invoke(new Action(() => { MdlClass.LabelTestModeState.Text = "当前模式:收料模式"; }));
            //    }
            //}
        }

        public void SetTestSquece(int seqID)
        {
            if (this._setTestSeqcallbacked != null)
            {
                this._setTestSeqcallbacked(seqID);
            }  
        }

        public int ClientIsBusy()
        {
            return sss.ClientIsBusy;
        }

        public bool BeLogIn()
        {
            try
            {
                if (this._belogIncallbacked !=null)
                {
                    _belogIncallbacked();
                    return true;
                }else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }


 
}
