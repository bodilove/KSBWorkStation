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
    public class UserService
    {
        SqlsugarMyClient client=new SqlsugarMyClient();
        ISqlSugarClient db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserService() {

            //方式1：通过Config获取数据库，比较灵活
            db = client.GetClient(); //非线程安全 (性能好)
  
        }
        /// <summary>
        /// 获取数据列表同步
        /// </summary>
        /// <returns></returns>
        public List<UserModel> QueryUserList()
        {
          var list=  db.Queryable<UserModel, RoleModel>((u, r) => new JoinQueryInfos(
                JoinType.Left, u.RoleID == r.RoleID //左连接 左链接 左联 
            )).Where(u => u.DeleteMark == 0)
           .Select<UserModel>().ToList();


            //List<UserModel> list =  db.Queryable<UserModel>().Where(p=>p.DeleteMark==0).ToList();
            return list;
        }
        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public int Add(UserModel m)
        {
            int result = db.Insertable(m).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 更新 同步
        /// </summary>
        /// <returns></returns>
        public int Edit(UserModel m)
        {
            int result =  db.Updateable(m).ExecuteCommand();
            return result;
        }
        /// <summary>
        /// 删除 同步
        /// </summary>
        /// <returns></returns>
        public int Delete(UserModel m)
        {
            int result = db.Deleteable(m).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 删除 同步(逻辑删除)
        /// </summary>
        /// <returns></returns>
        public int DeleteIsLogic(int id)
        {
            int result = 0;
            //int result = db.Updateable<UserModel>().In(m.UserID).IsLogic()
            //    .ExecuteCommand("DeleteMark", 1, "ModifyDate", DateTime.Now);
            var m= db.Queryable<UserModel>().Where(x => x.UserID == id).First();
            if (m != null)
            {
                m.DeleteMark = 1;
                m.ModifyUserId = 1;
                m.ModifyUserName = "超级管理员";
                m.ModifyDate = DateTime.Now;
                result = db.Updateable(m).ExecuteCommand();
                
            }
            return result;
        }
        /// <summary>
        /// 多删
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountID"></param>
        /// <param name="businessUserID"></param>
        /// <returns></returns>
        public int DelAll(object[] ids)
        {
            int result = db.Deleteable<UserModel>().Where(p => ids.Contains(p.UserID)).ExecuteCommand(); //批量删除
            return result;
        }

        /// <summary>
        /// 查询用户列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserModel>> QueryListAsync()
        {
            var list =await db.Queryable<UserModel, RoleModel>((u, r) => new JoinQueryInfos(
               JoinType.Left, u.RoleID == r.RoleID //左连接 左链接 左联 
           ))
           .Where(u=>u.DeleteMark==0)
           .Select<UserModel>().ToListAsync();
            //List<UserModel> list = await db.Queryable<UserModel>().Where(p=>p.DeleteMark==0).ToListAsync();
            return list;
        }
        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public async Task<int> AddAsync(UserModel m)
        {
            int result = await db.Insertable(m).ExecuteCommandAsync();
            return result;
        }
        /// <summary>
        /// 更新 异步
        /// </summary>
        /// <returns></returns>
        public async Task<int> EditAsync(UserModel m)
        {
            int result=await db.Updateable(m).ExecuteCommandAsync();
            return result;

        }
        /// <summary>
        /// 更新 异步
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteAsync(UserModel m)
        {
            int result = await db.Deleteable(m).ExecuteCommandAsync();
            return result;

        }

        /// <summary>
        /// 多删
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountID"></param>
        /// <param name="businessUserID"></param>
        /// <returns></returns>
        public async Task<int> DelAll2(object[] ids)
        {
            int result = await db.Deleteable<UserModel>().Where(p=>ids.Contains( p.UserID)).ExecuteCommandAsync(); //批量删除.ExecuteCommandAsync();
            return result;
        }

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
