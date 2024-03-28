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
    [SugarTable("Z_ProcessSteps")] // 指定数据库中的表名
    public class ProcessSteps
    {
        //数据是自增需要加上IsIdentity 
        //数据库是主键需要加上IsPrimaryKey 
        //注意：要完全和数据库一致2个属性
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //登录ID
        [Description("序号")]
        public int ProcessID { get; set; }

        //工位
        [Description("工位")]
        public string StationNum { get; set; }

        //父ID
        [Description("父ID")]
        public int ParentId { get; set; }

        //工艺名称
        [Description("工艺名称")]
        public string ProcessName { get; set; }

        //条件
        [Description("条件")]
        public string Conditional { get; set; }

        //零件码格式
        [Description("零件码格式")]
        public string KeyCodeFormat { get; set; }
        //是否零件
        [Description("是否零件")]
        public int IsKeyCode { get; set; }

        //上限值
        [Description("上限值")]
        public string Ulimit { get; set; }
        //下限值
        [Description("下限值")]
        public string Llimit { get; set; }
        //单位
        [Description("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        [SugarColumn(DefaultValue = "100")]
        public int SortCode { get; set; }

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
        /// 工艺类型 0：主；1：子
        /// </summary>
        [Description("工艺类型")]
        public int ProcessType { get; set; }
        

    }
}
