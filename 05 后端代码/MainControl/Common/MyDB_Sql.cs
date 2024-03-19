using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Common
{
    public class MyDB_Sql : IDisposable
    {

        public bool IsConnect = false;
        #region 定义变量
        /// <summary></summary>
        public SqlConnection _connection = null;

        /// <summary></summary>
        public SqlCommand _command = null;

        /// <summary></summary>
        private SqlDataReader _reader = null;

        /// <summary></summary>
        //public List<SqlParameter> _params = new List<SqlParameter>();

        public string dbconnectionstring = "";
        #endregion

        #region

        /// <summary></summary>
        public SqlDataReader DataReader
        {
            get { return _reader; }
        }

        public void Dispose()
        {
            Close();
            IsConnect = false;
        }
        #endregion

        #region 数据库操作

        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="serverName">服务器名称</param>
        /// <param name="dataName">数据库名称</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        public MyDB_Sql(string serverName, string dataName, string userName, string passWord)
        {

            dbconnectionstring = string.Format("Data Source={0}" +
                    ";Initial Catalog=" + "{1}" +
                    ";Persist Security Info=True;User ID=" + "{2}" +
                    ";Password=" + @"{3};MultipleActiveResultSets=true;", serverName, dataName, userName, passWord);
            IsConnect = Open();
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns>true或者false</returns>
        public bool Open()
        {
            if (_reader != null)
                _reader.Dispose();

            if (_command != null)
                _command.Dispose();

            if (_connection != null)
                _connection.Dispose();

            //_params.Clear();
            _connection = new SqlConnection();
            _command = new SqlCommand();
            if (_connection.State == System.Data.ConnectionState.Open)
                return true;
            try
            {
                _connection.ConnectionString = dbconnectionstring;

                _connection.Open();
                _command.CommandTimeout = 5;
                _command.Connection = _connection;
            }
            catch (Exception)
            {
               
                return false;
            }
          
            return true;
        }

        /// <summary>
        /// 释放数据库连接
        /// </summary>
        public void Close()
        {
            if (_reader != null)
                _reader.Dispose();

            if (_command != null)
                _command.Dispose();

            if (_connection != null)
                _connection.Dispose();
            //_params.Clear();
        }


        /// <summary>SELECT查询</summary>
        /// <param name="sql">SQL</param>
        /// <returns>true或者false</returns>
        public bool ExecuteReader(string sql, List<SqlParameter> _params)
        {
            try
            {
                if (_reader != null)
                    _reader.Dispose();

                _command.Parameters.Clear();
                if (_params != null && _params.Count != 0)
                    _command.Parameters.AddRange(_params.ToArray());

                _command.CommandText = sql;

                _reader = _command.ExecuteReader();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        /// <summary>SELECT查询</summary>
        /// <param name="sql">SQL</param>
        /// <returns>返回一个datatable</returns>
        public DataTable DataTableExecuteReader(string sql, List<SqlParameter> _params)
        {
            DataTable dt = null;
            try
            {
                //Open();
                if (_reader != null)
                    _reader.Dispose();

                _command.Parameters.Clear();
                if (_params != null && _params.Count != 0)
                    _command.Parameters.AddRange(_params.ToArray());

                _command.CommandText = sql;
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(_command);
                sda.Fill(ds);
                dt = ds.Tables[0];
                //Close();
            }
            catch (Exception ex)
            {

                return null;
            }

            return dt;
        }

        /// <summary>INSERT丄UPDATE丄DELETE语句</summary>
        /// <param name="sql">sql</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string sql, List<SqlParameter> _params)
        {
            int result;

            try
            {
              //  Open();
                _command.CommandText = sql;

                _command.Parameters.Clear();
                if (_params != null && _params.Count != 0)
                    _command.Parameters.AddRange(_params.ToArray());

                result = _command.ExecuteNonQuery();
              //  Close();
            }
            catch (Exception ex)
            {
                GlobalResources.WriteLog.dbErrLog("int ExecuteNonQuery(string sql, List<SqlParameter> _params)", ex.ToString());
                result = -1;
            }

            return result;
        }

        /// <summary>INSERT丄UPDATE丄DELETE语句</summary>
        /// <param name="sql">sql</param>
        /// <returns>bool</returns>
        public bool ExecuteNonQuery_Create(string sql, List<SqlParameter> _params)
        {
            int result;
            bool yorn = false;
            try
            {
               // Open();
                _command.CommandText = sql;

                _command.Parameters.Clear();
                if (_params != null && _params.Count != 0)
                    _command.Parameters.AddRange(_params.ToArray());

                result = _command.ExecuteNonQuery();
           //     Close();
                yorn = true;
            }
            catch (Exception)
            {
                result = -1;
                yorn = false;
            }

            return yorn;
        }

        /// <summary>
        /// 插入时可返回自增长ID
        /// </summary>
        /// <param name="sql">sql格式："INSERT INTO tablename (name) VALUES (@name);SELECT @@Identity"</param>
        /// <returns>返回自增长ID</returns>
        public string ExecuteScalar(string sql, List<SqlParameter> _params)
        {
            string result;

            try
            {
             //   Open();
                _command.CommandText = sql;

                _command.Parameters.Clear();
                if (_params != null && _params.Count != 0)
                    _command.Parameters.AddRange(_params.ToArray());

                result = Convert.ToString(_command.ExecuteScalar());
             //   Close();
            }
            catch (Exception)
            {
                result = "-1";
            }

            return result;
        }





        /// <summary>开始事务</summary>
        /// <remarks>
        /// </remarks>
        public void BeginTransaction()
        {
            _command.Transaction = _connection.BeginTransaction();
        }

        /// <summary>提交事务</summary>
        public void Commit()
        {
            _command.Transaction.Commit();
        }

        /// <summary>回滚事务</summary>
        public void Rollback()
        {
            _command.Transaction.Rollback();
        }


        /// <summary>
        /// 调用存储过程(增删改)
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameterlist">参数List</param>
        /// <param name="parameterValuelist">值List</param>
        public bool ExecuteProcedure_update(string procedureName, List<string> parameterlist, List<string> parameterValuelist)
        {
            bool yorn = true;
            try
            {

              //  Open();
                SqlCommand command = new SqlCommand(procedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parameterlist.Count; i++)
                {
                    command.Parameters.AddWithValue(string.Format("@{0}", parameterlist[i]), parameterValuelist[i]);
                }
                command.ExecuteNonQuery();
            //    Close();
            }
            catch (Exception)
            {

                yorn = false;
            }
            return yorn;
        }


        /// <summary>
        /// 调用存储过程(查询)
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameterlist">参数List</param>
        /// <param name="parameterValuelist">值List</param>
        public DataTable ExecuteProcedure_select(string procedureName, List<string> parameterlist, List<string> parameterValuelist)
        {
            DataTable dt = null;
            //bool yorn = true;
            try
            {

              //  Open();
                SqlCommand command = new SqlCommand(procedureName, _connection);
                command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parameterlist.Count; i++)
                {
                    command.Parameters.AddWithValue(string.Format("@{0}", parameterlist[i]), parameterValuelist[i]);
                }
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(ds);
                dt = ds.Tables[0];
              //  Close();
            }
            catch (Exception)
            {

                //yorn = false;
            }
            return dt;
        }


        public int ExecuteNonQuery_Image(string sql, byte[] by1, byte[] by2)
        {
            int result;

            try
            {

                _command.Parameters.Clear();
                _command.Parameters.Add("@image_product", SqlDbType.Image).Value = by1;
                _command.Parameters.Add("@image_label", SqlDbType.Image).Value = by2;
                _command.CommandText = sql;
                result = _command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        #endregion

        #region 批量插入

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dt">需要插入的表</param>
        /// <returns></returns>
        public bool BulkCopy(DataTable dt)
        {
            try
            {
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(_connection))
                {
                    sqlBC.DestinationTableName = dt.TableName;

                    foreach (DataColumn dr in dt.Columns)
                    {
                        sqlBC.ColumnMappings.Add(dr.ColumnName, dr.ColumnName);
                    }

                    //sqlBC.DestinationTableName = "dbo.table";
                    //sqlBC.ColumnMappings.Add("StockNo", "StockNo");
                    //sqlBC.ColumnMappings.Add("Angel", "Angel");
                    //sqlBC.ColumnMappings.Add("YesterDayAmountIn", "YesterDayAmountIn");
                    //sqlBC.ColumnMappings.Add("TwoDayAmountInTest", "TwoDayAmountInTest");
                    //sqlBC.ColumnMappings.Add("YesterDay10AmountIn", "YesterDay10AmountIn");
                    //sqlBC.ColumnMappings.Add("TenDayAmountInTest", "TenDayAmountInTest");
                    //sqlBC.ColumnMappings.Add("CreatedDate", "CreatedDate");
                    sqlBC.WriteToServer(dt);
                    //sqlBC.Close();
                }

             
                return true;
            }
            catch
            {
                try
                {
                
                }
                catch
                {
                }
                return false;
            }
        }



        #endregion
    }
}
