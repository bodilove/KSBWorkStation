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
    [SugarTable("U_SysMenu")] // 指定数据库中的表名
    public class SysMenuModel
    {
        //数据是自增需要加上IsIdentity 
        //数据库是主键需要加上IsPrimaryKey 
        //注意：要完全和数据库一致2个属性
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //登录ID
        [Description("序号")]
        public int Menu_Id { get; set; }
        //父ID
        [Description("父ID")]
        public int ParentId { get; set; }

        //菜单名称
        [Description("菜单名称")]
        public string Menu_Name { get; set; }

        //菜单标记
        [Description("菜单标记")]
        public string Menu_Tag { get; set; }

        /// <summary>
        /// 菜单图片
        /// </summary>
        [Description("菜单图片")]
        public int Menu_Img { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int SortCode { get; set; }

        ///// <summary>
        ///// 角色 0：操作工 1：工程师 2：主管
        ///// </summary>
        //public int UserRole { get; set; }

        ///// <summary>
        ///// 删除标记 0：否；1：是
        ///// </summary>
        //[Description("删除标记")]
        //public int DeleteMark { get; set; }
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
        /// 修改时间：当更新一个实体对象时，实体中包含time属性，即便没有设置新的时间，更新操作自动根据数据库时间更新该字段。
        /// </summary>
        [Description("修改时间")]
        //[SugarColumn(UpdateServerTime = true)]
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

        /// <summary>
        /// 菜单类型 0：否；1：是；2：
        /// </summary>
        [Description("菜单类型")]
        public int Menu_Type { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //[Description("备注")]
        //public string Remark { get; set; }

    }
}
