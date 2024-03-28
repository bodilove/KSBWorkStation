using Common.SysConfig.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ORMSqlSugar;
using Common;
using System.Threading;

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
            bool ret;
            Mutex mutex = new Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                SysAppConfig.appCnfigDoc = new AppConfig();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);



                //Application.Run(new frmMain());
                Application.Run(new frmLoginNew());

                // Main 为你程序的主窗体，如果是控制台程序不用这句
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show(null, "有一个和本程序相同的应用程序已经在运行，请不要同时运行多个本程序。\n\n这个程序即将退出。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 提示信息，可以删除。
                Application.Exit();//退出程序

            }
        }
    }
}
