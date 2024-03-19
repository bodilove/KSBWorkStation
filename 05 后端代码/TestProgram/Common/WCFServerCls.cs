using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using setDateTime;

namespace Common
{
    public class WCFServerCls
    {
        public WCFServer.Service1Client master = new WCFServer.Service1Client();
        public WCFServer.HeadBeat wcfhb = new WCFServer.HeadBeat();
        //发送心跳包
        public bool SendHeartBeat(int plcstatus, string SN)
        {
            bool yorn = true;
            try
            {
                
                switch (plcstatus)
                {
                    //case 0:
                    //    sss.SlaveStationState = WebServer1.SlaveStationState.FailProductLimitWaring;
                    //    break;
                    case 1:
                        wcfhb.PLCState = "急停";
                        wcfhb.SlaveStationID = "Station_02";
                        wcfhb.SN = SN;
                        wcfhb.SlaveStationState = "在线";
                        break;
                    case 2:
                        wcfhb.PLCState = "运行";
                        wcfhb.SlaveStationID = "Station_02";
                        wcfhb.SN = SN;
                        wcfhb.SlaveStationState = "在线";
                        break;
                    case 3:
                        wcfhb.PLCState = "维护";
                        wcfhb.SlaveStationID = "Station_02";
                        wcfhb.SN = SN;
                        wcfhb.SlaveStationState = "在线";
                        break;
                    default:
                        break;
                }
                master.sendHeadBeat(wcfhb);
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }
        public bool SendUUT(WCFServer.UUTTestResult uut)
        {
            bool yorn = true;
            try
            {
                master.UpUUT(uut);
            }
            catch (Exception)
            {
                yorn = false;
            }
            return yorn;
        }
        public bool SendCheckUUT(WCFServer.UUTTestResult uut)
        {
            bool yorn = true;
            try
            {
                master.UpCheckUUT(uut);
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
                WCFServer.ServerDateTime st = master.TimeSynchronization();
                SystemTime sdt = new SystemTime();
                sdt.wDay = st.wDay;
                sdt.wDayOfWeek = st.wDayOfWeek;
                sdt.wHour = st.wHour;
                sdt.wMiliseconds = st.wMiliseconds;
                sdt.wMinute = st.wMinute;
                sdt.wMonth = st.wMonth;
                sdt.wSecond = st.wSecond;
                sdt.wYear = st.wYear;
                SystemDateTime.SetLocalTime(ref sdt);
            }
            catch
            { }
        }


    }
}


