using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MainControl.Entity
{
    /// <summary>
    /// //方式1：通过Config获取数据库，比较灵活
    ///var db1 = db.GetConnection(configId) //非线程安全 (性能好)
    ///var db1 = db.GetConnectionScope(configId); //线程安全 (解决大部线程安全，性能有损耗)
    /// </summary>
    [TenantAttribute("1")]//对应ConfigId=1
    [Serializable]
    [SugarTable("U_User")] // 指定数据库中的表名
    public class UserModel
    {
        //数据是自增需要加上IsIdentity 
        //数据库是主键需要加上IsPrimaryKey 
        //注意：要完全和数据库一致2个属性
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //登录ID
        [Description("序号")]
        public int UserID { get; set; }

        //登录账号
        [Description("账号")]
        public string UserNum { get; set; }

        //用户名
        [Description("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string Password { get; set; }

        ///// <summary>
        ///// 角色 1：超级管理员2：管理员 3：工程师 4：操作工
        ///// </summary>
        [Description("角色ID")]
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Description("角色名")]
        public string RoleName { get; set; }

        ///// <summary>
        ///// 角色 1：男2：女  （默认是1）
        ///// </summary>
        [Description("性别")]
        public string Gender { get; set; }

        [Description("邮件")]
        public string Email { get; set; }

        /// <summary>
        /// 删除标记 0：否；1：是
        /// </summary>
        [Description("删除标记")]
        public int DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [SugarColumn(UpdateServerTime = true)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        [Description("创建者ID")]
        public int CreateUserId { get; set; }
        /// <summary>
        /// 创建者用户名
        /// </summary>
        [Description("创建者")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [SugarColumn(UpdateServerTime = true)]
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 修改者ID
        /// </summary>
        [Description("修改者ID")]
        public int ModifyUserId { get; set; }
        /// <summary>
        /// 修改者用户名
        /// </summary>
        [Description("修改者")]
        public string ModifyUserName { get; set; }

    }
}
