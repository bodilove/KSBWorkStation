using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORMSqlSugar
{
    /// <summary>
    /// SqlSugar的辅助方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarHelper<T> where T : class, new()
    {
        //注意：不能写成静态的
        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
        //public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来操作当前表的数据

        public SqlSugarHelper()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString,//数据库链接字符串
                //ConnectionString = AppSettingFun.GetConnectionStr(),//数据库链接字符串
                DbType = DbType.SqlServer,//指定数据库类型
                IsAutoCloseConnection = true,//链接使用完后是否自动释放
                InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息
            });

            //调式代码 用来打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
    }
    public class SqlSugarFun<T> : SqlSugarHelper<T> where T : class, new()
    {
        public SqlSugarClient db;
        public SqlSugarFun()
        {
            db = Db;
        }

        #region 保存(新增或更新)
        /// <summary>
        /// 保存(新增或更新)
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns></returns>
        public async Task<bool> Save(T model)
        {
            return await Task.Run(() => db.Storageable(model).ExecuteCommandAsync()) > 0;
        }

        /// <summary>
        /// 保存数据集(新增或更新)
        /// </summary>
        /// <param name="model">实体数据集</param>
        /// <returns></returns>
        public async Task<int> Save(List<T> model)
        {
            return await Task.Run(() => db.Storageable(model).ExecuteCommandAsync());
        }
        #endregion

        #region 新增
        #region 新增

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns>是否成功</returns>
        public async Task<bool> AddModel(T model)
        {
            return await Task.Run(() => db.Insertable(model).ExecuteCommandAsync()) > 0;
        }

        /// <summary>
        ///多数据插入
        /// </summary>
        /// <param name="lst">实体集合数据</param>
        /// <returns>影响行数</returns>
        public async Task<int> AddModel(List<T> lst)
        {
            return await Task.Run(() => db.Insertable(lst).ExecuteCommandAsync());
        }

        /// <summary>
        /// 写入实体数据并返回最新实体
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns>最新实体</returns>
        public async Task<T> AddbackEntity(T model)
        {
            return await Task.Run(() => db.Insertable(model).ExecuteReturnEntityAsync());
        }

        /// <summary>
        /// 写入实体数据并返回自增列
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns>主键</returns>
        public async Task<int> AddbackIdentity(T model)
        {
            return await Task.Run(() => db.Insertable(model).ExecuteReturnIdentityAsync());
        }

        /// <summary>
        /// 单条插入返回雪花ID
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns>雪花ID</returns>
        public async Task<long> AddbackSnowflakeid(T model)
        {
            return await Task.Run(() => db.Insertable(model).ExecuteReturnSnowflakeIdAsync());
        }

        /// <summary>
        /// 多条插入批量返回,比自增好用
        /// </summary>
        /// <param name="lst">实体集合数据</param>
        /// <returns>雪花ID集合</returns>
        public async Task<List<long>> AddbackSnowflakeid(List<T> lst)
        {
            return await Task.Run(() => db.Insertable(lst).ExecuteReturnSnowflakeIdListAsync());
        }
        #endregion

        #region 大数据新增
        /// <summary>
        /// 参数化内部分页插入（建议500行以下）
        /// </summary>
        /// <param name="model_lst">实体数据集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> AddListParam(List<T> model_lst)
        {
            return await Task.Run(() => db.Insertable(model_lst).UseParameter().ExecuteCommandAsync());
        }

        /// <summary>
        /// 大数据写入（特色功能：大数据处理上比所有框架都要快30%）
        /// </summary>
        /// <param name="model_lst">实体数据集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> AddListMax(List<T> model_lst)
        {
            return await Task.Run(() => db.Fastest<T>().BulkCopyAsync(model_lst));
        }
        #endregion
        #endregion

        #region 删除
        #region 删除
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>是否成功</returns>
        public async Task<bool> Delete(object id)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).ExecuteCommandHasChangeAsync());
        }

        /// <summary>
        /// 根据主键数组批量删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public async Task<int> Delete(object[] ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).ExecuteCommandAsync());
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="model">实体数据,有主键就行</param>
        /// <returns>是否成功</returns>
        public async Task<bool> Delete(T model)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(model).ExecuteCommandHasChangeAsync());
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="lst">实体集合数据,有主键就行</param>
        /// <returns>影响行数</returns>
        public async Task<int> Delete(List<T> lst)
        {
            return await Task.Run(() => db.Deleteable<T>(lst).ExecuteCommandAsync());
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns>是否成功</returns>
        public async Task<bool> Delete(Expression<Func<T, bool>> where)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(where).ExecuteCommandHasChangeAsync());
        }
        #endregion

        #region 假删除
        /// <summary>
        /// 根据主键假删除, 要求实体属性中必须有isdelete或者isdeleted
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteLogic(object id)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).IsLogic().ExecuteCommandAsync()) > 0;
        }

        /// <summary>
        /// 根据主键指定属性假删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="name">属性名称</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteLogic(object id, string name)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).IsLogic().ExecuteCommandAsync(name)) > 0;
        }

        /// <summary>
        /// 根据主键数组批量假删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteLogic(object[] ids)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).IsLogic().ExecuteCommandAsync());
        }

        /// <summary>
        /// 根据主键数组指定属性批量假删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteLogic(object[] ids, string name)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).IsLogic().ExecuteCommandAsync(name));
        }

        /// <summary>
        /// 根据条件指定属性假删除
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <param name="name">删除字段名称</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteLogic(Expression<Func<T, bool>> where, string delcol)
        {
            return await Task.Run(() => db.Deleteable<T>().Where(where).IsLogic().ExecuteCommandAsync(delcol));
        }

        /// <summary>
        /// 根据主键删除并更新操作时间
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="delcol">删除标记字段</param>
        /// <param name="datecol">时间标记字段</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteLogic(object id, string delcol, string datecol)
        {
            return await Task.Run(() => db.Deleteable<T>().In(id).IsLogic().ExecuteCommand(delcol, DateTime.Now, datecol)) > 0;
        }

        /// <summary>
        /// 根据主键批量删除并更新操作时间
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="delcol">删除标记字段</param>
        /// <param name="datecol">时间标记字段</param>
        /// <returns>影响行数</returns>
        public async Task<int> DeleteLogic(object[] ids, string delcol, string datecol)
        {
            return await Task.Run(() => db.Deleteable<T>().In(ids).IsLogic().ExecuteCommand(delcol, DateTime.Now, datecol));
        }
        #endregion
        #endregion

        #region 更新
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="model">实体数据</param>
        /// <returns>是否成功</returns>
        public async Task<bool> Update(T model)
        {
            //这种方式会以主键为条件
            return await Task.Run(() => db.Updateable(model).ExecuteCommandHasChange());
        }

        /// <summary>
        /// 批量更新实体数据
        /// 数据超过50条时启用大数据更新
        /// </summary>
        /// <param name="lst">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Update(List<T> lst)
        {
            if (lst.Count > 50)
            {
                return await Task.Run(() => db.Fastest<T>().BulkUpdateAsync(lst));
            }
            //这种方式会以主键为条件
            return await Task.Run(() => db.Updateable(lst).ExecuteCommandAsync());
        }

        /// <summary>
        /// 按条件指定更新列
        /// </summary>
        /// <param name="newObject">t=>t.更新列==值 表达式</param>
        /// <param name="where">条件表达式</param>
        /// <returns>影响行数</returns>
        public async Task<int> Update(Expression<Func<T, bool>> newObject, Expression<Func<T, bool>> where)
        {
            return await Task.Run(() => db.Updateable<T>().SetColumns(newObject).Where(where).ExecuteCommandAsync());
        }

        /// <summary>
        /// 按条件指定更新列
        /// </summary>
        /// <param name="newObject">t=> new T(){更新列=值} 表达式</param>
        /// <param name="where">条件表达式</param>
        /// <returns>影响行数</returns>
        public async Task<int> Update(Expression<Func<T, T>> newObject, Expression<Func<T, bool>> where)
        {
            return await Task.Run(() => db.Updateable<T>().SetColumns(newObject).Where(where).ExecuteCommandAsync());
        }

        /// <summary>
        /// 更新并启用启用验证
        /// 需要有Timestamp字段呼应
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateVer(T model)
        {
            return await Task.Run(() => db.Updateable(model).IsEnableUpdateVersionValidation().ExecuteCommandHasChange());
        }
        #endregion

        #region 查询

        #region 基础查询
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAll()
        {
            return await Task.Run(() => db.Queryable<T>().ToList());
        }

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public async Task<T> QueryByID(object objId)
        {
            return await Task.Run(() => db.Queryable<T>().InSingleAsync(objId));
        }

        /// <summary>
        /// 根据条件查询数据
        /// var exp= Expressionable.Create<Student>();
        /// exp.OrIF(条件,it=>it.Id==1);//.OrIf 是条件成立才会拼接OR
        /// exp.Or(it =>it.Name.Contains("jack"));//拼接OR
        /// var list =db.Queryable<Student>().Where(exp.ToExpression()).ToList();
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<T> QueryByWhere(Expression<Func<T, bool>> where)
        {
            return await Task.Run(() => db.Queryable<T>().Where(where).First());
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> Any(Expression<Func<T, bool>> where)
        {
            return await Task.Run(() => db.Queryable<T>().AnyAsync(where));
        }

        /// <summary>
        /// 获取正序数据集合
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListByAsc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order)
        {
            return await Task.Run(() => db.Queryable<T>().Where(where).OrderBy(order).ToList());
        }

        /// <summary>
        /// 获取反序数据集合
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListByDesc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order)
        {
            return await Task.Run(() => db.Queryable<T>().Where(where).OrderBy(order, OrderByType.Desc).ToList());
        }
        #endregion

        #region 分页查询
        /// <summary>
        /// 分页正序查询
        /// SqlSever2012分页  把  ToPageList 换成  ToOffsetPage   //offest分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagerow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> GetPageListByAsc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, int pageindex, int pagerow, ref int count)
        {
            return db.Queryable<T>().Where(where).OrderBy(order).ToPageList(pageindex, pagerow, ref count);
        }

        /// <summary>
        /// 分页反序查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagerow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> GetPageListByDesc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, int pageindex, int pagerow, ref int count)
        {
            return db.Queryable<T>().Where(where).OrderBy(order, OrderByType.Desc).ToPageList(pageindex, pagerow, ref count);
        }

        /// <summary>
        /// 分页正序查询
        /// SqlSever2012分页  把  ToPageList 换成  ToOffsetPage   //offest分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="select"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagerow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<object> GetPageListByAsc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, Expression<Func<T, object>> select, int pageindex, int pagerow, ref int count)
        {
            return db.Queryable<T>().Where(where).OrderBy(order).Select(select).ToPageList(pageindex, pagerow, ref count);
        }

        /// <summary>
        /// 分页反序查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="select"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagerow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<object> GetPageListByDesc(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, Expression<Func<T, object>> select, int pageindex, int pagerow, ref int count)
        {
            return db.Queryable<T>().Where(where).OrderBy(order, OrderByType.Desc).Select(select).ToPageList(pageindex, pagerow, ref count);
        }

        /// <summary>
        /// 获取数据总页数
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetPageTotle(int count, int pagerow)
        {
            int total = count;
            if (total % pagerow > 0)
                total = total / pagerow + 1;
            else
                total /= pagerow;
            return total;
        }
        #endregion

        #endregion

        #region 其它方法
        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public async Task<DateTime> GetdbDate()
        {
            return await Task.Run(() => db.GetDate());
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<dynamic> SqlQuery(string sql)
        {
            return await Task.Run(() => db.Ado.SqlQuery<dynamic>(sql));
        }

        /// <summary>
        /// 执行sql语句（查询除外）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<bool> SqlExec(string sql)
        {
            return await Task.Run(() => db.Ado.ExecuteCommand(sql)) > 0;
        }

        /// <summary>
        /// 获取新的雪花ID
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetSnowFlakeID()
        {
            return await Task.Run(() => SnowFlakeSingle.Instance.NextId());
        }

        /// <summary>
        /// 初始化表
        /// 表中数据全部清空，清除，自增初始化
        /// </summary>
        /// <returns>是否成功</returns>
        public async Task<bool> Truncate()
        {
            return await Task.Run(() => db.DbMaintenance.TruncateTable<T>());
        }
        #endregion
    }
}
