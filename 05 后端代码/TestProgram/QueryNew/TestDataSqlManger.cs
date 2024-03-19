using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace Test.Query
{
    public class TestDataSqlManger
    {
        SqlConnection _conn;
        SqlCommand _cmd;
        string _strConn = "Data Source=LocalHost;Initial Catalog=UTop;User ID=sa;Password=1234";

        public DataTable GetData(string _tableName)
        {
            DataTable dt = GetTable("select * from" + " " + _tableName);
            return dt;
        }

        #region 数据库操作
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConn()
        {
            if (_conn == null) _conn = new SqlConnection(_strConn);
            if (_conn.State == ConnectionState.Closed) _conn.Open();
            else if (_conn.State == ConnectionState.Broken)
            {
                _conn.Close();
                _conn.Open();
            }
            return _conn;
        }

        /// <summary>
        /// 执行增删改查操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                _cmd = new SqlCommand(sql, GetConn());
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql)
        {
            SqlDataReader dr = null;
            try
            {
                _cmd = new SqlCommand(sql, GetConn());
                dr = _cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch
            {
                return null;
            }
            finally
            {
                dr.Close();
            }
        }

        public SqlCommand SqlSmd(string sql)
        {
            SqlCommand _cmd = new SqlCommand(sql, GetConn());
            return _cmd;
        }

        /// <summary>
        /// 得到该表数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, GetConn());
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _conn.Close();
            }
        }



        #endregion



    }
}
