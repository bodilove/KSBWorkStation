
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace Common
{
    public class SysConfig2
    {
        public static AppConfig appCnfigDoc { get; set; }
        /// <summary>
        /// 曲线类型
        /// </summary>
        public static int CurveType
        {
            get
            {
                return ConfigurationManager.AppSettings["CurveType"].StrToInt(0);
            }
        }
       
    }
}