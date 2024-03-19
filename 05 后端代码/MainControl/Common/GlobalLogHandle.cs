using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public static class GlobalLogHandle
    {
        public static Common.WriteLog StationLogHandle = new WriteLog(Application.StartupPath + @"\StationLog");
        public static Common.WriteLog dbErrLogHandle = new WriteLog(Application.StartupPath + @"\dbErro");
        public static Common.WriteLog PLCLogHandle = new WriteLog(Application.StartupPath + @"\PLClog");
        public static Common.WriteLog SysErroLogHandle = new WriteLog(Application.StartupPath + @"\SysErro");
        public static Common.WriteLog WCFLogHandle = new WriteLog(Application.StartupPath + @"\WCFHandle");




        ////每站的流程日志
        //public void StationLog
        ////数据库操作错误日志
        //public void dbErrLog

        ////PLC操作日志
        //public void PLCLog
        ////系统操作报错日志
        //public void SysErroLog

        ////WCF请求交互日志
        //public void WCFLog
    }
}
