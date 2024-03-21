using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MainControl.Entity
{
    /// <summary>
    /// 返回用户
    /// </summary>
    public class UserViewModelOutput:UserModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string RoleName { get; set; }
    }
}
