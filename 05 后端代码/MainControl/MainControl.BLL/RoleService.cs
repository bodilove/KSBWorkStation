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
    public class RoleService
    {
        SqlsugarMyClient client=new SqlsugarMyClient();
        ISqlSugarClient db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleService() {

            //方式1：通过Config获取数据库，比较灵活
            db = client.GetClient(); //非线程安全 (性能好)
  
        }
        /// <summary>
        /// 判断是否存在角色名称
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool IsExistRoleName(string RoleName)
        {
            var result = db.Queryable<RoleModel>().Where(p => p.DeleteMark == 0 && p.RoleName == RoleName).First();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取数据列表同步
        /// </summary>
        /// <returns></returns>
        public   List<RoleModel> QueryList()
        {
             List<RoleModel> list =  db.Queryable<RoleModel>().ToList();
             return list;
            
        }

        /// <summary>
        /// 查询用户列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleModel>> QueryListAsync()
        {
            List<RoleModel> list = await db.Queryable<RoleModel>().ToListAsync();
            return list;
            
        }
        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public int Add(RoleModel m)
        {
            int RoleID = db.Insertable(m).ExecuteReturnIdentity();
            //int result = db.Insertable(m).ExecuteCommand();
            if (m.RoleRightList != null && m.RoleRightList.Count > 0)
            {
                m.RoleRightList.ForEach(p => p.RoleID = RoleID);

                db.Deleteable<RoleModel>().Where(p => p.RoleID == RoleID);


                db.Insertable(m.RoleRightList).ExecuteCommand();
            }

            return RoleID;
        }

        /// <summary>
        /// 更新 同步
        /// </summary>
        /// <returns></returns>
        public int Edit(RoleModel m)
        {
            int result = db.Updateable(m).ExecuteCommand();

            if (m.RoleRightList != null && m.RoleRightList.Count > 0)
            {
                db.Deleteable<RoleModel>().Where(p => p.RoleID == m.RoleID);

                db.Insertable(m.RoleRightList).ExecuteCommand();
            }

            return result;
        }
        /// <summary>
        /// 删除 同步
        /// </summary>
        /// <returns></returns>
        public int Delete(RoleModel m)
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
            
            var m = db.Queryable<RoleModel>().Where(x => x.RoleID == id).First();
            if (m != null)
            {
                m.DeleteMark = 1;
                //m.ModifyUserId = 1;
                //m.ModifyUserName = "超级管理员";
                //m.ModifyDate = DateTime.Now;
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
            int result = db.Deleteable<RoleModel>().Where(p => ids.Contains(p.RoleID)).ExecuteCommand(); //批量删除
            return result;
        }

        
        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public async Task<int> AddAsync(RoleModel m)
        {
            int result = await db.Insertable(m).ExecuteCommandAsync();
            return result;
        }
        /// <summary>
        /// 更新 异步
        /// </summary>
        /// <returns></returns>
        public async Task<int> EditAsync(RoleModel m)
        {
            int result = await db.Updateable(m).ExecuteCommandAsync();
            return result;

        }
        /// <summary>
        /// 更新 异步
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteAsync(RoleModel m)
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
            int result = await db.Deleteable<RoleModel>().Where(p => ids.Contains(p.RoleID)).ExecuteCommandAsync(); //批量删除.ExecuteCommandAsync();
            return result;
        }


    }
}
