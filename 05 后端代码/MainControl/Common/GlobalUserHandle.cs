using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class GlobalUserHandle
    {
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public static int LoginUserID { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public static string UserNum { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string loginUserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public static string Password { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public static int RoleID { get; set; }
       /// <summary>
       /// 登陆IP
       /// </summary>
        public static string LocalIP { get; set; }
    }
}
