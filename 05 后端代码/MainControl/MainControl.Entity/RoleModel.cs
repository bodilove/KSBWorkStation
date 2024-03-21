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
    [SugarTable("U_Role")] // 指定数据库中的表名
    public class RoleModel
    {
        //数据是自增需要加上IsIdentity 
        //数据库是主键需要加上IsPrimaryKey 
        //注意：要完全和数据库一致2个属性
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //登录ID
        [Description("序号")]
        public int RoleID { get; set; }

        //登录账号
        [Description("角色名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 删除标记 0：否；1：是
        /// </summary>
        [Description("删除标记")]
        public int DeleteMark { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //[Description("创建时间")]
        //public DateTime CreateDate { get; set; }
        ///// <summary>
        ///// 创建者ID
        ///// </summary>
        //[Description("创建者ID")]
        //public int CreateUserId { get; set; }
        ///// <summary>
        ///// 创建者用户名
        ///// </summary>
        //[Description("创建者")]
        //public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }

    }
}
