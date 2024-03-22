using MainControl.Entity;
using ORMSqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MainControl.BLL
{
    public class SysLogService
    {
        SqlsugarMyClient client=new SqlsugarMyClient();
        ISqlSugarClient db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysLogService() {

            //方式1：通过Config获取数据库，比较灵活
            db = client.GetClient(); //非线程安全 (性能好)
  
        }
        /// <summary>
        /// 获取数据列表同步
        /// </summary>
        /// <returns></returns>
        public List<SysLogModel> QueryList()
        {
            var list =  db.Queryable<SysLogModel>().ToList();
            return list;
        }
        /// <summary>
        /// 查询用户列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysLogModel>> QueryListAsync()
        {
           
            var list = await db.Queryable<SysLogModel>().OrderByDescending(p=>p.CreateDate).Take(10000).ToListAsync();
            return list;
        }
        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public int AddLog(SysLogModel m)
        {
            int result = db.Insertable(m).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 删除 同步
        /// </summary>
        /// <returns></returns>
        public int Delete(SysLogModel m)
        {
            int result = db.Deleteable(m).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 删除 所有,TruncateTable
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
           bool result= db.DbMaintenance.TruncateTable<SysLogModel>();
            return result;
        }


    }
}
