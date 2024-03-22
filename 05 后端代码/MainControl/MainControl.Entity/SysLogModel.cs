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
    [SugarTable("Sys_Log")] // 指定数据库中的表名
    public class SysLogModel
    {
        //数据是自增需要加上IsIdentity 
        //数据库是主键需要加上IsPrimaryKey 
        //注意：要完全和数据库一致2个属性
        [SugarColumn(IsPrimaryKey = true)]
        //序号
        [Description("序号")]
        public Guid LogID { get; set; }

        //类型
        [Description("类型")]
        public string Type { get; set; }

        //模块
        [Description("模块")]
        public string Module { get; set; }

        //方法
        [Description("方法")]
        public string Method { get; set; }

        //内容
        [Description("内容")]
        public string LogMessage { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [Description("IP地址")]
        public string LocalIP { get; set; }
        /// <summary>
        /// 报错类名
        /// </summary>
        [Description("报错类名")]
        public string ClassName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        [Description("创建者ID")]
        public int CreateUserID { get; set; }
        /// <summary>
        /// 创建者用户名
        /// </summary>
        [Description("创建者")]
        public string CreateUserName { get; set; }
        
        
        
    }
}
