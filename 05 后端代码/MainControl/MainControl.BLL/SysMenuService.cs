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
    public class SysMenuService
    {
       SqlsugarMyClient client=new SqlsugarMyClient();
        ISqlSugarClient db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysMenuService() {

            //方式1：通过Config获取数据库，比较灵活
            db = client.GetClient(); //非线程安全 (性能好)
  
        }
        /// <summary>
        /// 获取数据列表同步
        /// </summary>
        /// <returns></returns>
        public   List<SysMenuModel> QueryList()
        {
             List<SysMenuModel> list =  db.Queryable<SysMenuModel>().ToList();
             return list;
            
        }
        /// <summary>
        /// 查询用户列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysMenuModel>> QueryListAsync()
        {
            //var db = client.Queryable<List<UserInfo>>(null, "dbo.U_User").ToList();

            List<SysMenuModel> list = await db.Queryable<SysMenuModel>().ToListAsync();
            return list;
            
        }
        ///// <summary>
        ///// 多删
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="accountID"></param>
        ///// <param name="businessUserID"></param>
        ///// <returns></returns>
        //public async Task<string> DelOrder(object[] id)
        //{

        //    var od = await Task.Run(() => GetListByAsc(t => id.Contains(t.FID), t => t.FID));
        //    if (od == null)
        //    {
        //        return "找不到";
        //    }
        //    var b = await Task.Run(() => Delete(od)) > 0;
        //    if (b)
        //        return result.Success("操作成功");
        //    return result.Error("操作失败");
        //}

        /// <summary>
        /// 数据库联查
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="accountID"></param>
        /// <param name="private_key"></param>
        /// <returns></returns>
        //public async Task<Object> GroupPerformanceRanking(int year, int month, long accountID)
        //{
        //    DateTime start = Convert.ToDateTime($@"{year}-{month}-01 00:00:00");
        //    DateTime end = start.AddMonths(1);

        //    var lst = await Task.Run(() => db.Queryable<tOrder, tGroup>((o, g) => new JoinQueryInfos(
        //        JoinType.Left, o.FGroupID.Equals(g.FID)
        //        ))
        //    .Where((o, g) => o.FAccounID.Equals(accountID) && o.FOrderDate > start && o.FOrderDate < end)
        //    .Select((o, g) => new
        //    {
        //        g.FGroupName,
        //        g.FGroupLogo,
        //        o.FDollar,
        //        o.FRMB,
        //    }).MergeTable().GroupBy(g => new
        //    {
        //        g.FGroupName,
        //        g.FGroupLogo
        //    }).Select(t => new
        //    {
        //        t.FGroupName,
        //        FDollar = SqlFunc.AggregateSum(t.FDollar),
        //        FRMB = SqlFunc.AggregateSum(t.FRMB)
        //    }).OrderBy(t => t.FRMB, OrderByType.Desc).ToList());

        //    return result.Success(lst);
        //}




    }
}
