using MainControl.Entity;
using ORMSqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        /// 判断是否存在菜单名称
        /// </summary>
        /// <param name="MenuName"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public bool IsExistMenuName(string MenuName,int ParentId)
        {
            var result = db.Queryable<SysMenuModel>().Where(p => p.DeleteMark == 0 && p.Menu_Name == MenuName&&p.ParentId== ParentId).First();
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
        public   List<SysMenuModel> QueryList()
        {
             List<SysMenuModel> list =  db.Queryable<SysMenuModel>().Where(p => p.DeleteMark == 0).ToList();
             return list;
            
        }
        /// <summary>
        /// 查询所有下级列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysMenuModel>> QueryAllChildListAsync(int MenuID)
        {
            //List<SysMenuModel> list = await db.Queryable<SysMenuModel>().Where(p=>p.Menu_Id==MenuID).ToListAsync();
            var list = await db.Queryable<SysMenuModel>().Where(p=>p.DeleteMark==0).ToChildListAsync(it => it.ParentId, MenuID);//MenuID是主键，查主键为MenuID下面所有
            return list;

        }

        /// <summary>
        /// 查询一个下级列表 异步
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysMenuModel>> QueryListAsync(int MenuID)
        {
            //List<SysMenuModel> list = await db.Queryable<SysMenuModel>().Where(p=>p.Menu_Id==MenuID).ToListAsync();
           var list= await db.Queryable<SysMenuModel>().Where(p => p.ParentId== MenuID&& p.DeleteMark==0).ToListAsync();//MenuID是主键，查主键为MenuID下面第一个
            return list;
            
        }

        /// <summary>
        /// 角色权限查询
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public List<RoleRightModel> GetRoleRightByCondition(int RoleID)
        {
            try
            {

                return db.Queryable<RoleRightModel>().Where(p=>p.RoleID==RoleID).ToList();
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }

        //获取按钮
        public List<SysMenuModel> GetButtonList(int ParentId, int RoleID)
        {
            try
            {

                var list = db.Queryable<RoleRightModel>()
                 .InnerJoin<SysMenuModel>((r, m) => r.Menu_Id == m.Menu_Id)//多个条件用&&
                 //.Where(r => r.RoleID == RoleID)
                 .Where((r, m) => r.RoleID == RoleID && m.ParentId==ParentId) //如果用到m需要这么写
                 //.Select((r, m) => new ViewOrder { Id = o.Id, CustomName = cus.Name })  //ViewOrder是一个新建的类，更多Select用法看下面文档
                 .Select<SysMenuModel>().ToList(); 

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //获取角色菜单
        public List<SysMenuModel> GetButtonListAll( int RoleID)
        {
            try
            {

                var list = db.Queryable<RoleRightModel>()
                 .InnerJoin<SysMenuModel>((r, m) => r.Menu_Id == m.Menu_Id)//多个条件用&&
                                                                           //.Where(r => r.RoleID == RoleID)
                 .Where((r, m) => r.RoleID == RoleID) //如果用到m需要这么写
                                                                                //.Select((r, m) => new ViewOrder { Id = o.Id, CustomName = cus.Name })  //ViewOrder是一个新建的类，更多Select用法看下面文档
                 .Select<SysMenuModel>().ToList();

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加 同步
        /// </summary>
        /// <returns></returns>
        public int Add(SysMenuModel m)
        {
            int result = db.Insertable(m).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 更新 同步
        /// </summary>
        /// <returns></returns>
        public int Edit(SysMenuModel m)
        {
            int result = db.Updateable(m).ExecuteCommand();
            return result;
        }
        /// <summary>
        /// 删除 同步(逻辑删除)
        /// </summary>
        /// <returns></returns>
        public int DeleteIsLogic(int id)
        {
            int result = 0;
            //int result = db.Updateable<SysMenuModel>().In(m.Menu_Id).IsLogic()
            //    .ExecuteCommand("DeleteMark", 1, "ModifyDate", DateTime.Now);
            var m = db.Queryable<SysMenuModel>().Where(x => x.Menu_Id == id).First();
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

    }
}
