using Common.SysConfig.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MainControl
{
    static class Program
    {
        public static SystemConfig CurrentConfig = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            
            //Application.Run(new frmMain());
            Application.Run(new frmLoginNew());
        }
    }
}
