using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test.Common
{
    public static class MdlClass
    {
        
        public static string LineNo = "2";
        public static string stationID = "2";
        public static int CurrentstationID = 2;
        public static string LineName = "FOB";
        public static string stationName = "FOB标定";
     
        //public static WebServerCls webserver=new WebServerCls ();
        //public static TcpIpServerCls TcpipServer = new TcpIpServerCls();

      
        public static int Plcstatus = 0;//当前plc状态


        public static string PCBSN = "";//当前烧录的PCBSN

        public static string PCBSM = "";//当前扫描的PCBSN

        public static bool IsTestPowerON = false;

        public static ReadPLC readplc = null;//读取plc
        public static bool IscanTest = false;

        public static bool PowerOnIsOK = false;

        /// <summary>
        /// NG次数
        /// </summary>
        public static int NGProduct = 0;
        /// <summary>
        /// NG测试项
        /// </summary>
        public static string NGTestName = "";


        public static Label labelMessage = null;
        public static Label LabelTestModeState = null;

        public static bool IsStartTesting = false;
    }
}
