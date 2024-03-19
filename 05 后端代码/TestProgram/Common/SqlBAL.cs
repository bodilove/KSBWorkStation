using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Test.Common
{
    public static class SQLBAL
    {
        public static string dbserver = ".";
        public static string dbname = "Utop_New";
        public static string userid = "sa";
        public static string password = "1234";
        //public static SqlConnection  _con= null;
     
        public static WriteLog WriteLog = new WriteLog(@"D:\Log");
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类对象</typeparam>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="record">每页显示条数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="AllLines">总数</param>        
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<T> GetPages<T>(string tableName, int record, int currentpage, ref long AllLines, string IdName, string condition) where T : new()
        {
            List<T> ts = new List<T>();
            DataTable dt = null;
            int n = 0;
            string sql = "select count(1) from " + tableName + " u1 where 1=1 " + condition;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                if (dt != null)
                {
                    AllLines = long.Parse(dt.Rows[0].ItemArray[0].ToString());
                    n = record * (currentpage - 1);
                }
                else
                {
                    AllLines = 0;
                    n =0;
                }
              
            }
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                sql = "SELECT TOP " + record + "* FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + IdName + ") AS RowNumber,* FROM " + tableName + " u1 where 1=1 " + condition + " ) AS t1 where t1.RowNumber>" + n;
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T t = new T();
                        foreach (System.Reflection.PropertyInfo p in t.GetType().GetProperties())
                        {
                            try
                            {
                                switch (p.PropertyType.Name)
                                {
                                    case "Int32":
                                        p.SetValue(t, int.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Int64":
                                        p.SetValue(t, long.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Long":
                                        p.SetValue(t, long.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Single":
                                        p.SetValue(t, float.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Object":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            using (XmlTextReader rdr = new XmlTextReader(dt.Rows[i][p.Name].ToString(), XmlNodeType.Document, null))
                                            {
                                                SqlXml sqlxml = new SqlXml(rdr);
                                                p.SetValue(t, sqlxml, null);
                                            }
                                        }
                                        break;
                                    case "String":
                                        p.SetValue(t, dt.Rows[i][p.Name].ToString(), null);
                                        break;
                                    case "DateTime":
                                        p.SetValue(t, DateTime.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Image":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            byte[] by1 = dt.Rows[i][p.Name] as byte[];
                                            if (by1 != null && by1.Length > 0)
                                            {
                                                MemoryStream ms = new MemoryStream(by1);
                                                Image image = System.Drawing.Image.FromStream(ms);
                                                p.SetValue(t, image, null);
                                            }
                                        }
                                        break;
                                    case "Byte[]":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            byte[] by1 = dt.Rows[i][p.Name] as byte[];
                                            if (by1 != null && by1.Length > 0)
                                            {
                                                p.SetValue(t, by1, null);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                            catch (Exception ex)
                            {

                                WriteLog.dbErrLog(" List<T> GetPages<T>(string tableName, int record, int currentpage, ref long AllLines, string IdName, string condition) where T : new()", ex.ToString());
                            }
                        }
                        ts.Add(t);
                        t = default(T);
                    }
                }
            }
            return ts;
        }


        /// <summary>
        /// 根据条件返回实体类对象集合
        /// </summary>
        /// <typeparam name="T">实体类对象</typeparam>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="conditionCol">条件列名</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<T> GetListInfo<T>(string tableName, string condition) where T : new()
        {
            List<T> ts = new List<T>();
            DataTable dt = null;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                string sql = "select * from " + tableName + " where 1=1 " + condition;
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T t = new T();
                        foreach (System.Reflection.PropertyInfo p in t.GetType().GetProperties())
                        {
                            try
                            {
                                switch (p.PropertyType.Name)
                                {
                                    case "Int32":
                                        p.SetValue(t, int.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Single":
                                        p.SetValue(t, float.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;

                                    case "Int64":
                                        p.SetValue(t, long.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Long":
                                        p.SetValue(t, long.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "SqlXml":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            using (XmlTextReader rdr = new XmlTextReader(dt.Rows[i][p.Name].ToString(), XmlNodeType.Document, null))
                                            {
                                                SqlXml sqlxml = new SqlXml(rdr);
                                                p.SetValue(t, sqlxml, null);
                                            }
                                        }
                                        break;
                                    case "String":
                                        p.SetValue(t, dt.Rows[i][p.Name].ToString(), null);
                                        break;
                                    case "DateTime":
                                        p.SetValue(t, DateTime.Parse(dt.Rows[i][p.Name].ToString()), null);
                                        break;
                                    case "Image":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            byte[] by1 = dt.Rows[i][p.Name] as byte[];
                                            if (by1 != null && by1.Length > 0)
                                            {
                                                MemoryStream ms = new MemoryStream(by1);
                                                Image image = System.Drawing.Image.FromStream(ms);
                                                p.SetValue(t, image, null);
                                            }
                                        }
                                        break;
                                    case "Byte[]":
                                        if (dt.Rows[i][p.Name].ToString() != null && dt.Rows[i][p.Name].ToString() != "")
                                        {
                                            byte[] by1 = dt.Rows[i][p.Name] as byte[];
                                            if (by1 != null && by1.Length > 0)
                                            {
                                                p.SetValue(t, by1, null);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                            catch (Exception ex)
                            {

                                WriteLog.dbErrLog("List<T> GetListInfo<T>(string tableName, string conditionCol, string condition) where T : new()", ex.ToString());
                            }
                        }
                        ts.Add(t);
                        t = default(T);
                    }
                }
            }
            return ts;
        }

        /// <summary>
        /// 根据ID返回对象
        /// </summary>
        /// <typeparam name="T">返回的实体类对象</typeparam>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="condition">条件列名</param>
        /// <param name="ID">ID</param>
        /// <returns></returns>
        public static T GetInfo<T>(string tableName, string condition, int ID) where T : new()
        {
            T t = new T();
            DataTable dt = null;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                string sql = "select * from " + tableName + " where " + condition + "=" + ID;
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                if (dt != null)
                {
                    foreach (System.Reflection.PropertyInfo p in t.GetType().GetProperties())
                    {
                        try
                        {
                            switch (p.PropertyType.Name)
                            {
                                case "Int32":
                                    p.SetValue(t, int.Parse(dt.Rows[0][p.Name].ToString()), null);
                                    break;
                                case "Int64":
                                    p.SetValue(t, long.Parse(dt.Rows[0][p.Name].ToString()), null);
                                    break;
                                case "Long":
                                    p.SetValue(t, long.Parse(dt.Rows[0][p.Name].ToString()), null);
                                    break;
                                case "Single":
                                    p.SetValue(t, float.Parse(dt.Rows[0][p.Name].ToString()), null);
                                    break;
                                case "Object":
                                    if (dt.Rows[0][p.Name].ToString() != null && dt.Rows[0][p.Name].ToString() != "")
                                    {
                                        using (XmlTextReader rdr = new XmlTextReader(dt.Rows[0][p.Name].ToString(), XmlNodeType.Document, null))
                                        {
                                            SqlXml sqlxml = new SqlXml(rdr);
                                            p.SetValue(t, sqlxml, null);
                                        }
                                    }
                                    break;
                                case "SqlXml":
                                    if (dt.Rows[0][p.Name].ToString() != null && dt.Rows[0][p.Name].ToString() != "")
                                    {
                                        using (XmlTextReader rdr = new XmlTextReader(dt.Rows[0][p.Name].ToString(), XmlNodeType.Document, null))
                                        {
                                            SqlXml sqlxml = new SqlXml(rdr);
                                            p.SetValue(t, sqlxml, null);
                                        }
                                    }
                                    break;
                                case "Byte[]":
                                    if (dt.Rows[0][p.Name] != null)
                                    {
                                        byte[] by1 = dt.Rows[0][p.Name] as byte[];
                                        if (by1 == null || by1.Length <= 0)
                                        {
                                            by1 = new byte[] { 0 };
                                        }
                                        p.SetValue(t, by1, null);
                                    }
                                    break;
                                case "String":
                                    p.SetValue(t, dt.Rows[0][p.Name].ToString(), null);
                                    break;
                                case "DateTime":
                                    p.SetValue(t, DateTime.Parse(dt.Rows[0][p.Name].ToString()), null);
                                    break;
                                case "Image":
                                    if (dt.Rows[0][p.Name].ToString() != null && dt.Rows[0][p.Name].ToString() != "")
                                    {
                                        byte[] by1 = dt.Rows[0][p.Name] as byte[];
                                        if (by1 != null && by1.Length > 0)
                                        {
                                            MemoryStream ms = new MemoryStream(by1);
                                            Image image = System.Drawing.Image.FromStream(ms);
                                            p.SetValue(t, image, null);
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }

                        }
                        catch (Exception ex)
                        {

                            WriteLog.dbErrLog("T GetInfo<T>(string tableName, string condition, int ID) where T : new()", ex.ToString());
                        }
                    }
                }
            }
            return t;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">实体类对象</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="tableName">要操作的表名</param>
        /// <returns></returns>
        public static bool Add<T>(T model, string tableName)
        {
            string columns = "";
            string content = "";
            bool yorn = false;
            int Scalar = 0;
            List<SqlParameter> _parames = new List<SqlParameter>();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                try
                {

                    foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
                    {
                        columns += "," + p.Name;
                        content += ",@" + p.Name;
                        if (p.PropertyType.Name == "Int32")
                        {
                            int value = int.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Int, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Int64")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Long")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Single")
                        {
                            float value = float.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Float, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);
                        }
                        else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                        {
                            DateTime value = DateTime.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.DateTime, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }

                        else if (p.PropertyType.Name == "SqlXml")
                        {
                            SqlXml sqlXml = (SqlXml)p.GetValue(model, null);
                            if (sqlXml != null)
                            {
                                if (!sqlXml.IsNull)
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, sqlXml.Value.Length);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                                else
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                            }
                            else
                            {
                                SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                parmContent.Value = new SqlXml();
                                _parames.Add(parmContent);
                            }
                            //content += ",'" + (SqlXml)p.GetValue(model, null) + "'";
                        }
                        else if (p.PropertyType.Name == "Image")
                        {
                            Image image = ((Image)p.GetValue(model, null));
                            if (image == null)
                            {
                                image = new Bitmap(1, 1);
                            }
                            ImageFormat format = image.RawFormat;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                image.Save(ms, ImageFormat.Jpeg);
                                byte[] buffer = new byte[ms.Length];
                                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                                ms.Seek(0, SeekOrigin.Begin);
                                ms.Read(buffer, 0, buffer.Length);
                                if (buffer != null)
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Image, 0);
                                    parmContent.Value = buffer;
                                    _parames.Add(parmContent);
                                }
                            }
                        }
                        else if (p.PropertyType.Name == "Byte[]")
                        {
                            byte[] buffer = p.GetValue(model, null) as byte[];
                            if (buffer != null)
                            {
                                SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Image, 0);
                                parmContent.Value = buffer;
                                _parames.Add(parmContent);
                            }
                        }
                        else
                        {
                            string value = p.GetValue(model, null).ToString();
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.VarChar, value.Length);
                            parmContent.Value = value;
                            _parames.Add(parmContent);
                            //content += ",'" + p.GetValue(model, null).ToString() + "'";
                        }
                    }
                }
                catch (Exception ex)
                {

                    WriteLog.dbErrLog("bool Add<T>(T model, string tableName)", ex.ToString());
                }
                if (content.Length == 0 || columns.Length == 0) return false;

                content = content.Remove(0, 1);
                content = content.Remove(0, content.IndexOf(',') + 1);
                columns = columns.Remove(0, 1);
                columns = columns.Remove(0, columns.IndexOf(',') + 1);
                string sql = "insert into " + tableName + "(" + columns + ") values(" + content + ")";

                Scalar = mydbUUTDB.ExecuteNonQuery(sql, _parames);
                if (Scalar > 0)
                {
                    yorn = true;
                }
            }
            return yorn;
        }
        public static int AddT<T>(T model, string tableName)
        {
            string columns = "";
            string content = "";
            List<SqlParameter> _parames = new List<SqlParameter>();
            int Scalar = 0;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                try
                {

                    foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
                    {
                        columns += "," + p.Name;
                        content += ",@" + p.Name;
                        if (p.PropertyType.Name == "Int32")
                        {
                            int value = int.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Int, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Int64")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Long")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }

                        else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                        {
                            DateTime value = DateTime.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.DateTime, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Object")
                        {
                            SqlXml sqlXml = (SqlXml)p.GetValue(model, null);
                            if (sqlXml != null)
                            {
                                if (!sqlXml.IsNull)
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, sqlXml.Value.Length);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                                else
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                            }
                            else
                            {
                                SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                parmContent.Value = new SqlXml();
                                _parames.Add(parmContent);
                            }
                        }
                        else
                        {
                            string value = p.GetValue(model, null).ToString();
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.VarChar, value.Length);
                            parmContent.Value = value;
                            _parames.Add(parmContent);
                            //content += ",'" + p.GetValue(model, null).ToString() + "'";
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                if (content.Length == 0 || columns.Length == 0) return 0;
                content = content.Remove(0, 1);
                content = content.Remove(0, content.IndexOf(',') + 1);
                columns = columns.Remove(0, 1);
               columns = columns.Remove(0, columns.IndexOf(',') + 1);
                string sql = "insert into " + tableName + "(" + columns + ") values(" + content + ") select id=@@identity";

                Scalar = int.Parse(mydbUUTDB.ExecuteScalar(sql, _parames));

            }
            return Scalar;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">实体类对象</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="condition">条件</param>       
        /// <returns></returns>
        public static bool Update<T>(T model, string tableName, string condition)
        {
            string content = "";
            bool yorn = false;
            int Scalar = 0;
            List<SqlParameter> _parames = new List<SqlParameter>();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                try
                {
                    foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
                    {
                        content += "," + p.Name + "=@" + p.Name;
                        if (p.PropertyType.Name == "Int32")
                        {
                            int value = int.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Int, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Int64")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Long")
                        {
                            long value = long.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.BigInt, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "Single")
                        {
                            float value = float.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Float, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);
                        }
                        else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                        {
                            DateTime value = DateTime.Parse(p.GetValue(model, null).ToString());
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.DateTime, 4);
                            parmContent.Value = value;
                            _parames.Add(parmContent);

                            //content += "," + p.GetValue(model, null).ToString();
                        }
                        else if (p.PropertyType.Name == "SqlXml")
                        {
                            SqlXml sqlXml = (SqlXml)p.GetValue(model, null);
                            if (sqlXml != null)
                            {
                                if (!sqlXml.IsNull)
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, sqlXml.Value.Length);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                                else
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                    parmContent.Value = sqlXml;
                                    _parames.Add(parmContent);
                                }
                            }
                            else
                            {
                                SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Xml, 0);
                                parmContent.Value = new SqlXml();
                                _parames.Add(parmContent);
                            }
                            //content += ",'" + (SqlXml)p.GetValue(model, null) + "'";
                        }
                        else if (p.PropertyType.Name == "Image")
                        {
                            Image image = ((Image)p.GetValue(model, null));
                            if (image == null)
                            {
                                image = new Bitmap(1, 1);
                            }
                            ImageFormat format = image.RawFormat;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                image.Save(ms, ImageFormat.Jpeg);
                                byte[] buffer = new byte[ms.Length];
                                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                                ms.Seek(0, SeekOrigin.Begin);
                                ms.Read(buffer, 0, buffer.Length);
                                if (buffer != null)
                                {
                                    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Image, 0);
                                    parmContent.Value = buffer;
                                    _parames.Add(parmContent);
                                }
                            }
                            //byte[] bys = (byte[])p.GetValue(model, null);
                            //if (bys != null)
                            //{
                            //    SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Image, 0);
                            //    parmContent.Value = bys;
                            //    mydbUUTDB._params.Add(parmContent);
                            //}
                        }
                        else if (p.PropertyType.Name == "Byte[]")
                        {
                            byte[] buffer = p.GetValue(model, null) as byte[];
                            if (buffer != null)
                            {
                                SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.Image, 0);
                                parmContent.Value = buffer;
                                _parames.Add(parmContent);
                            }
                        }
                        else
                        {
                            string value = p.GetValue(model, null).ToString();
                            SqlParameter parmContent = new SqlParameter("@" + p.Name, SqlDbType.VarChar, value.Length);
                            parmContent.Value = value;
                            _parames.Add(parmContent);
                            //content += ",'" + p.GetValue(model, null).ToString() + "'";
                        }
                    }
                }
                catch (Exception ex)
                {

                    WriteLog.dbErrLog("bool Update<T>(T model, string tableName, string condition, string IdCollect)", ex.ToString());
                }
                if (content.Length == 0) return false;
                content = content.Remove(0, 1);
                content = content.Remove(0, content.IndexOf(',') + 1);
                string sql = "update " + tableName + " set " + content + " where 1=1 " + condition;

                Scalar = mydbUUTDB.ExecuteNonQuery(sql, _parames);
                if (Scalar > -1)
                {
                    yorn = true;
                }
            }
            return yorn;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="condition">条件</param>        
        /// <returns></returns>
        public static bool Delete(string tableName, string condition)
        {
            bool yorn = false;
            int Scalar = 0;
            string sql = "delete " + tableName + " where 1=1 " + condition;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                Scalar = mydbUUTDB.ExecuteNonQuery(sql, new List<SqlParameter>());
                if (Scalar > -1)
                {
                    yorn = true;
                }
            }
            return yorn;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="condition">条件</param>        
        /// <returns></returns>
        public static bool UpdateBySql(string sql)
        {
            bool yorn = false;
            int Scalar = 0;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                Scalar = mydbUUTDB.ExecuteNonQuery(sql, new List<SqlParameter>());
                if (Scalar > -1)
                {
                    yorn = true;
                }
            }
            return yorn;
        }
        /// <summary>
        /// 判断表中是否存在
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <param name="columnName">要查询的字段名</param>
        /// <param name="content">条件</param>
        /// <returns></returns>
        public static bool IsExists(string tableName, string columnName, string content)
        {
            bool yorn = false;
            string sql = "";
            DataTable dt = null;
            string[] cols = columnName.Split(',');
            string[] cons = content.Split(',');
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                sql = "select 1 from dbo." + tableName + " where 1=1 ";
                for (int i = 0; i < cols.Length; i++)
                {
                    sql += " and " + cols[i] + "='" + cons[i] + "'";
                }
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                if (dt != null && dt.Rows.Count > 0)
                {
                    yorn = true;
                }
            }
            return yorn;
        }


        /// <summary>
        /// 根据条件返回DataTable
        /// </summary>
        /// <typeparam name="Sql">sql语句</typeparam>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql)
        {

            DataTable dt = null;
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
                {
                    dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                }
            }
            catch (Exception ex)
            {

                WriteLog.dbErrLog("GetDataTable(string sql)", ex.ToString());
            }
            return dt;
        }
    }
}
