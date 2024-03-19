using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Test.ProjectFileEditor
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmStartup());
            Application.Run(new frmMain());
        }
    }
}
