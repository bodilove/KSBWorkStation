
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace Common
{
    public class SysAppConfig
    {
        /// <summary>
        /// APP Config 文件
        /// </summary>
        public static AppConfig appCnfigDoc { get; set; }

        /// <summary>
        ///  数据服务器连接字符串
        /// </summary>
        public static string MySqlConnection
        {
            get
            {
                return ConfigurationManager.AppSettings["MySqlConnection"].ToStringExt("");
            }
        }

        /// <summary>
        /// Log时间
        /// </summary>
        public static int LogRetainDays
        {
            get
            {
                return ConfigurationManager.AppSettings["LogRetainDays"].StrToInt(8);
            }
        }


    }
}