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

namespace Common
{
    public static class GlobalResources
    {
        public static string dbserver = "LAPTOP-1ST4SV69";
        public static string dbname = "LGDB";
        public static string userid = "sa";
        public static string password = "1234";
        //public static MyDB_Sql mydbUUTDB;
        public static WriteLog WriteLog = new WriteLog(@"D:\ZJ_MES_Log");

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
        public static List<T> GetPages<T>(string tableName, int record, int currentpage, ref long AllLines, string IdName, string condition, MyDB_Sql mydbUUTDB) where T : new()
        {
            List<T> ts = new List<T>();
            DataTable dt = null;
            int n = 0;
            string sql = "select count(1) from " + tableName + " where 1=1 " + condition;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            // {
            dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
            AllLines = long.Parse(dt.Rows[0].ItemArray[0].ToString());
            n = record * (currentpage - 1);
            // }
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            //}
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
        public static List<T> GetListInfo<T>(string tableName, string condition, MyDB_Sql mydbUUTDB) where T : new()
        {
            List<T> ts = new List<T>();
            DataTable dt = null;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            //}
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
        public static T GetInfo<T>(string tableName, string condition, long ID, MyDB_Sql mydbUUTDB) where T : new()
        {
            T t = new T();
            DataTable dt = null;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            //}
            return t;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">实体类对象</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="tableName">要操作的表名</param>
        /// <returns></returns>
        public static bool Add<T>(T model, string tableName, MyDB_Sql mydbUUTDB, bool IsPrimary)
        {
            string columns = "";
            string content = "";
            bool yorn = false;
            int Scalar = 0;
            List<SqlParameter> _parames = new List<SqlParameter>();
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            if (IsPrimary)
            {
                content = content.Remove(0, content.IndexOf(',') + 1);
            }
            columns = columns.Remove(0, 1);
            if (IsPrimary)
            {
                columns = columns.Remove(0, columns.IndexOf(',') + 1);
            }
            string sql = "insert into " + tableName + "(" + columns + ") values(" + content + ")";

            Scalar = mydbUUTDB.ExecuteNonQuery(sql, _parames);
            if (Scalar > 0)
            {
                yorn = true;
            }
            //}
            return yorn;
        }
        public static long AddT<T>(T model, string tableName, MyDB_Sql mydbUUTDB, bool IsPrimary)
        {
            string columns = "";
            string content = "";
            List<SqlParameter> _parames = new List<SqlParameter>();
            long Scalar = 0;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            if (IsPrimary)
            {
                content = content.Remove(0, content.IndexOf(',') + 1);
            }
            columns = columns.Remove(0, 1);
            if (IsPrimary)
            {
                columns = columns.Remove(0, columns.IndexOf(',') + 1);
            }
            string sql = "insert into " + tableName + "(" + columns + ") values(" + content + ") select id=@@identity";

            Scalar = int.Parse(mydbUUTDB.ExecuteScalar(sql, _parames));

            //}
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
        public static bool Update<T>(T model, string tableName, string condition, MyDB_Sql mydbUUTDB)
        {
            string content = "";
            bool yorn = false;
            int Scalar = 0;
            List<SqlParameter> _parames = new List<SqlParameter>();
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
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
            //}
            return yorn;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName">要操作的表名</param>
        /// <param name="condition">条件</param>        
        /// <returns></returns>
        public static bool Delete(string tableName, string condition, MyDB_Sql mydbUUTDB)
        {
            bool yorn = false;
            int Scalar = 0;
            string sql = "delete " + tableName + " where 1=1 " + condition;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
            Scalar = mydbUUTDB.ExecuteNonQuery(sql, new List<SqlParameter>());
            if (Scalar > -1)
            {
                yorn = true;
            }
            //}
            return yorn;
        }

        /// <summary>
        /// 判断表中是否存在
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <param name="columnName">要查询的字段名</param>
        /// <param name="content">条件</param>
        /// <returns></returns>
        public static int IsExists(string tableName, string columnName, string content, MyDB_Sql mydbUUTDB)
        {
            bool yorn = false;
            string sql = "";
            DataTable dt = null;
            string[] cols = columnName.Split(',');
            string[] cons = content.Split(',');
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            //{
            sql = "select 1 from dbo." + tableName + " where 1=1 ";
            for (int i = 0; i < cols.Length; i++)
            {
                sql += " and " + cols[i] + "='" + cons[i] + "'";
            }
            dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
            if (dt == null) return -1;
            if (dt.Rows.Count > 0)
            {
                return 1;
            }
            return 0;
        }


        /// <summary>
        /// 根据条件返回DataTable
        /// </summary>
        /// <typeparam name="Sql">sql语句</typeparam>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, MyDB_Sql mydbUUTDB)
        {

            DataTable dt = null;
            try
            {
                //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
                //{
                dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                //}
            }
            catch (Exception ex)
            {

                WriteLog.dbErrLog("GetDataTable(string sql)", ex.ToString());
            }
            return dt;
        }


        public static bool ClearTable(string TableName, MyDB_Sql mydbUUTDB)//清空，id从清空前的id开始
        {
            bool res = false;
            try
            {
                //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
                //{
                string sql = "delete from " + TableName;
                res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                //}
            }
            catch (Exception ex)
            {

                WriteLog.dbErrLog("ClearTable(string sql)", ex.ToString());
            }
            return res;
        }

        public static bool TruncateTable(string TableName, MyDB_Sql mydbUUTDB)//清空，id从1开始
        {
            bool res = false;
            try
            {
                //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
                //{
                string sql = "truncate Table " + TableName;
                res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                //}
            }
            catch (Exception ex)
            {

                WriteLog.dbErrLog("TruncateTable(string sql)", ex.ToString());
            }
            return res;
        }

        public static bool BulkCopy(DataTable dt)
        {
            System.Diagnostics.Stopwatch sp1 = new System.Diagnostics.Stopwatch();
            bool res = false;
            sp1.Start();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
            {
                mydbUUTDB.BeginTransaction();
                try
                {
                    if (mydbUUTDB.BulkCopy(dt))
                    {
                        //  dt.Rows.Clear();
                        mydbUUTDB.Commit();
                        res = true;

                    }
                    else
                    {
                        mydbUUTDB.Rollback();
                        //throw new Exception("插入数据失败");
                        res = false;
                    }
                }
                catch
                {
                    mydbUUTDB.Rollback();
                    res = false;
                }
            }
            sp1.Stop();
            Console.WriteLine("消耗:" + sp1.ElapsedMilliseconds);
            return res;
        }



        public static bool SqlConnectTest()
        {
            bool res = false;
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(dbserver, dbname, userid, password))
                {
                    res = mydbUUTDB.IsConnect;
                }
               
            }
            catch (Exception ex)
            {
                res = false;
                //  MdlClass.WriteLog.dbErrLog("DbConnectTestErro", ex.ToString());
            }
            return res;
        }



        public static void CreateDataBase(string dbName, string Path)
        {

            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, "master", GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE DATABASE " + dbName
                                        + " CONTAINMENT = NONE"
                                        + " ON  PRIMARY "
                                        + "( NAME = N'" + dbName + "', FILENAME = N'" + Path + @"\" + dbName + ".mdf" + "' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )"
                                        + " LOG ON "
                                        + "( NAME = N'" + dbName + "_log" + "', FILENAME = N'" + Path + @"\" + dbName + "_log.ldf" + "' , SIZE = 2048KB , MAXSIZE = 102400KB , FILEGROWTH = 10%)" + "\r\n"
                 + "IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))" + "\r\n"
       + "begin" + "\r\n"
       + "EXEC [" + dbName + "].[dbo].[sp_fulltext_database] @action = 'enable'" + "\r\n"
       + "end" + "\r\n"
                        //  + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ANSI_NULL_DEFAULT OFF " + "\r\n"
                        //  + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ANSI_NULLS OFF " + "\r\n"
                        //   + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ANSI_PADDING OFF " + "\r\n"
                        //  + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ANSI_WARNINGS OFF " + "\r\n"
                        //  + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ARITHABORT OFF " + "\r\n"
                        //    + "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET AUTO_CLOSE OFF " + "\r\n"
                        // //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET AUTO_CREATE_STATISTICS ON " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET AUTO_SHRINK OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET AUTO_UPDATE_STATISTICS ON " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET CURSOR_CLOSE_ON_COMMIT OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET CURSOR_DEFAULT  GLOBAL " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET CONCAT_NULL_YIELDS_NULL OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET NUMERIC_ROUNDABORT OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET QUOTED_IDENTIFIER OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET RECURSIVE_TRIGGERS OFF" + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET  DISABLE_BROKER " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF" + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET DATE_CORRELATION_OPTIMIZATION OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET TRUSTWORTHY OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET ALLOW_SNAPSHOT_ISOLATION OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET PARAMETERIZATION SIMPLE " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET READ_COMMITTED_SNAPSHOT OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET HONOR_BROKER_PRIORITY OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET RECOVERY FULL " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET  MULTI_USER " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET PAGE_VERIFY CHECKSUM  " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET DB_CHAINING OFF " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET TARGET_RECOVERY_TIME = 0 SECONDS " + "\r\n"
                        //+ "GO" +"\r\n"

       + "ALTER DATABASE [" + dbName + "] SET  READ_WRITE " + "\r\n";
                    //+ "GO" +"\r\n";

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void DropDataBase(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, "master", GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "drop database " + dbName;

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            // drop database 数据库名
        }


        public static List<string> GetAllDataBase()
        {
            //  SELECT NAME FROM MASTER.DBO.SYSDATABASES ORDER BY NAME
            List<string> Namelst = new List<string>();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, "master", GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "SELECT NAME FROM MASTER.DBO.SYSDATABASES ORDER BY NAME";

                    DataTable dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());

                    foreach (DataRow dr in dt.Rows)
                    {
                        Namelst.Add(dr[0].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Namelst;
        }



        public static void CreateStatistic(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[Statistic](" + "\r\n"
    + "[PartNr] [varchar](50) NOT NULL," + "\r\n"
    + "[PassCount] [int] NULL," + "\r\n"
    + "[FailCount] [int] NULL," + "\r\n"
    + "[RollingNum] [int] NULL," + "\r\n"
    + "[Date] [datetime] NULL," + "\r\n"
    + "[DayRollingNum] [int] NULL," + "\r\n"
+ " CONSTRAINT [PK_Statistic_1] PRIMARY KEY CLUSTERED " + "\r\n"
+ "(" + "\r\n" + "\r\n"
    + "[PartNr] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
+ ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建Statistic表失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateP_Product(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "     CREATE TABLE [dbo].[P_Product](" + "\r\n"
                  + "[UUTestId] [bigint] IDENTITY(1,1) NOT NULL," + "\r\n"
                  + "[UserID] [int] NULL," + "\r\n"
                  + "[UserName] [varchar](50) NOT NULL," + "\r\n"
                  + "[SN] [varchar](100) NOT NULL," + "\r\n"
                  + "[StartTime] [datetime] NOT NULL," + "\r\n"
                  + "[EndTime] [datetime] NULL," + "\r\n"
                  + "[PrdType] [int] NOT NULL," + "\r\n"
                  + "[PartNum] [varchar](50) NULL," + "\r\n"
                  + "[Result] [varchar](15) NULL," + "\r\n"
                  + "[State] [int] NOT NULL," + "\r\n"
                  + "[ExpectedPath] [bigint] NOT NULL," + "\r\n"
                  + "[ExpectedPathNames] [varchar](300) NOT NULL," + "\r\n"
                  + "[ActualPath] [bigint] NOT NULL," + "\r\n"
                  + "[ActualResult] [bigint] NOT NULL," + "\r\n"
               + "CONSTRAINT [PK_P_Product] PRIMARY KEY CLUSTERED " + "\r\n"
              + "(" + "\r\n"
              + "	[UUTestId] ASC" + "\r\n"
              + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
              + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建P_Product表失败。");
                    }



                    sql = "CREATE NONCLUSTERED INDEX [NonClusteredIndex-P_Product-001] ON [dbo].[P_Product]" + "\r\n"
+ "(" + "\r\n"
+ "	[SN] ASC," + "\r\n"
 + "[UserID] ASC," + "\r\n"
   + "[StartTime] ASC," + "\r\n"
+ "	[EndTime] ASC," + "\r\n"
   + "[PrdType] ASC," + "\r\n"
   + "[PartNum] ASC," + "\r\n"
   + "[Result] ASC," + "\r\n"
   + "[State] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建P_Product索引失败。");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateP_InWork(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "      CREATE TABLE [dbo].[P_InWork](" + "\r\n"
          + "[UUTestId] [bigint] NOT NULL," + "\r\n"
                        //+ "[UserID] [int] NULL," + "\r\n"
          + "[UserName] [varchar](50) NULL," + "\r\n"
          + "[SN] [varchar](100) NULL," + "\r\n"
          + "[StartTime] [datetime] NULL," + "\r\n"
          + "[PrdType] [int] NULL," + "\r\n"
          + "[PartNum] [varchar](50) NULL," + "\r\n"
          + "[CurrentResult] [varchar](15) NULL," + "\r\n"
          + "[ExpectedPath] [bigint] NULL," + "\r\n"
          + "[ActualPath] [bigint] NULL," + "\r\n"
          + "[ActualResult] [bigint] NULL," + "\r\n"
          + "[CurrentStationName] [varchar](30) NULL" + "\r\n"
      + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建P_InWork表失败。");
                    }


                    sql = "CREATE NONCLUSTERED INDEX [NonClusteredIndex-P_InWork-001] ON [dbo].[P_InWork]" + "\r\n"
+ "(" + "\r\n"
    + "[SN] ASC," + "\r\n"
                        //+ "[UserID] ASC," + "\r\n"
    + "[StartTime] ASC," + "\r\n"
    + "[PrdType] ASC," + "\r\n"
    + "[PartNum] ASC," + "\r\n"
    + "[CurrentResult] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n";

                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建P_InWork索引失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateST_Product(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[ST_" + Index + "_Product](" + "\r\n"
     + "[StationTestId] [bigint] IDENTITY(1,1) NOT NULL," + "\r\n"
     + "[UUTestId] [bigint] NOT NULL," + "\r\n"
     + "[StationName] [varchar](30) NULL," + "\r\n"
     + "[StartTime] [datetime] NOT NULL," + "\r\n"
     + "[EndTime] [datetime] NULL," + "\r\n"
     + "[PrdType] [int] NOT NULL," + "\r\n"
     + "[Result] [varchar](15) NOT NULL," + "\r\n"
 + " CONSTRAINT [PK_" + "ST_" + Index + "_Product" + "] PRIMARY KEY CLUSTERED " + "\r\n"
 + "(" + "\r\n"
     + "[StationTestId] ASC" + "\r\n"
 + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
 + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_Product" + Index + "表失败。");
                    }

                    sql = "CREATE NONCLUSTERED INDEX [NonClusteredIndex-ST_Product-" + Index + "] ON [dbo].[ST_" + Index + "_Product]" + "\r\n"
+ "(" + "\r\n"
    + "[UUTestId] ASC," + "\r\n"
    + "[StationName] ASC," + "\r\n"
    + "[StartTime] ASC," + "\r\n"
    + "[EndTime] ASC," + "\r\n"
    + "[PrdType] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_Product" + Index + "索引失败。");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //ST_AssembleRes
        public static void CreateST_AssembleRes(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[ST_" + Index + "_AssembleRes](" + "\r\n"
                    + "[Id] [bigint] IDENTITY(1,1) NOT NULL," + "\r\n"
                    + "[StationTestId] [bigint] NOT NULL," + "\r\n"
                    + "[ScanCode] [varchar](200) NULL," + "\r\n"
                    + "[Scantime] [datetime] NULL," + "\r\n"
                    + "[Result] [varchar](15) NULL," + "\r\n"
                    + "[Description] [varchar](200) NULL," + "\r\n"
                    + " CONSTRAINT [PK_" + "ST_" + Index + "_AssembleRes] PRIMARY KEY CLUSTERED " + "\r\n"
                    + "(" + "\r\n"
                    + "[Id] ASC" + "\r\n"
                    + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
                    + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_AssembleRes" + Index + "表失败。");
                    }

                    sql = "CREATE NONCLUSTERED INDEX [NonClusteredIndex-ST_AssembleRes-" + Index + "] ON [dbo].[ST_" + Index + "_AssembleRes]" + "\r\n"
 + "(" + "\r\n"
     + "[StationTestId] ASC," + "\r\n"
     + "[ScanCode] ASC," + "\r\n"
     + "[Scantime] ASC," + "\r\n"
     + "[Result] ASC" + "\r\n"
 + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_AssembleRes" + Index + "索引失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateST_TestRes(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[ST_" + Index + "_TestRes](" + "\r\n"
      + "[ID] [bigint] IDENTITY(1,1) NOT NULL," + "\r\n"
      + "[StationTestId] [bigint] NULL," + "\r\n"
      + "[TestName] [varchar](50) NULL," + "\r\n"
      + "[StepName] [varchar](50) NULL," + "\r\n"
      + "[ComPareMode] [varchar](20) NULL," + "\r\n"
      + "[Ulimit] [varchar](200) NULL," + "\r\n"
      + "[Llimit] [varchar](200) NULL," + "\r\n"
      + "[Nom] [varchar](200) NULL," + "\r\n"
      + "[TestValue] [varchar](200) NULL," + "\r\n"
      + "[Unit] [varchar](20) NULL," + "\r\n"
      + "[Result] [varchar](20) NULL," + "\r\n"
      + "[Description] [varchar](200) NULL," + "\r\n"
      + "[SpanTime] [float] NULL," + "\r\n"
      + "[TestTime] [datetime] NULL," + "\r\n"
   + "CONSTRAINT [PK_" + "ST_" + Index + "_TestRes] PRIMARY KEY CLUSTERED " + "\r\n"
  + "(" + "\r\n"
      + "[ID] ASC" + "\r\n"
  + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
  + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_TestRes" + Index + "表失败。");
                    }



                    sql = "CREATE NONCLUSTERED INDEX [NonClusteredIndex-ST_TestRes-" + Index + "] ON [dbo].[ST_" + Index + "_TestRes]" + "\r\n"
   + "(" + "\r\n"
       + "[StationTestId] ASC," + "\r\n"
       + "[TestName] ASC," + "\r\n"
       + "[StepName] ASC," + "\r\n"
       + "[Result] ASC," + "\r\n"
       + "[TestTime] ASC" + "\r\n"
   + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建ST_TestRes" + Index + "索引失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateUser(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[Users](" + "\r\n"
    + "[ID] [int] IDENTITY(1,1) NOT NULL," + "\r\n"
    + "[Name] [nvarchar](25) NOT NULL," + "\r\n"
    + "[Type] [nvarchar](25) NOT NULL," + "\r\n"
    + "[Number] [nvarchar](25) NULL," + "\r\n"
    + "[Password] [nvarchar](25) NOT NULL," + "\r\n"
    + "[Description] [nvarchar](25) NOT NULL," + "\r\n"
+ " CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED " + "\r\n"
+ "(" + "\r\n"
    + "[ID] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
+ ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建User表失败。");
                    }

                    sql = "      INSERT INTO [Users] ([Name], [Type],[Number],[Password],[Description]) VALUES ('admin','管理员', 'admin','','管理员')";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("User表插入admin初始账户失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateGroups(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[Groups](" + "\r\n"
               + "[id] [int] IDENTITY(1,1) NOT NULL," + "\r\n"
               + "[Type] [varchar](50) NOT NULL," + "\r\n"
               + "[Test] [int] NOT NULL," + "\r\n"
               + "[Build] [int] NOT NULL," + "\r\n"
               + "[Manage] [int] NOT NULL," + "\r\n"
               + "[Query] [int] NOT NULL," + "\r\n"
               + "[SetLabel] [int] NULL," + "\r\n"
            + "CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED " + "\r\n"
           + "(" + "\r\n"
               + "[id] ASC" + "\r\n"
           + ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
           + ") ON [PRIMARY]" + "\r\n";
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建Groups表失败。");
                    }

                    sql = "  INSERT INTO [Groups] ([Type], [Test],[Build],[Manage],[Query],[SetLabel]) VALUES ('管理员', 1,1,1,1,1)";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("Groups表插入管理员组失败。");
                    }
                    sql = "INSERT INTO [Groups] ([Type], [Test],[Build],[Manage],[Query],[SetLabel]) VALUES ('操作员', 1,0,0,0,0)";
                    res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("Groups表插入操作员组失败。。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static void CreateStationConfig(string dbName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [StationConfig](" + "\r\n"
     + "[StationID] [int] NOT NULL," + "\r\n"
     + "[PassCount] [bigint] NOT NULL," + "\r\n"
     + "[FailCount] [bigint] NOT NULL," + "\r\n"
     + "[CurrentCount] [int] NOT NULL," + "\r\n"
     + "[ForeWarning] [int] NOT NULL," + "\r\n"
     + "[Warning] [int] NOT NULL" + "\r\n"
 + ") ON [PRIMARY]" + "\r\n";

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建StationConfig表失败。");
                    }

                    for (int i = 0; i < 64; i++)
                    {
                        sql = "      INSERT INTO [StationConfig] ([StationID],[PassCount], [FailCount],[CurrentCount],[ForeWarning],[Warning]) VALUES (" + i + ",0,0, 0,0,0)";
                        res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                        if (!res)
                        {
                            throw new Exception("StationConfig表插入表初始站位失败。");
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 增加工位易损件清单
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="Index"></param>
        public static void CreateSparePart(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "        CREATE TABLE [dbo].[ST_" + Index + "_SparePart](" + "\r\n"
    + "[ID] [int] IDENTITY(1,1) NOT NULL," + "\r\n"
    + "[SparePartName] [varchar](50) NOT NULL," + "\r\n"
    + "[CurrentCount] [int] NOT NULL," + "\r\n"
    + "[ForeWarning] [int] NOT NULL," + "\r\n"
    + "[Warning] [int] NOT NULL," + "\r\n"
 + "CONSTRAINT [PK_ST_" + Index + "_SparePart] PRIMARY KEY CLUSTERED " + "\r\n"
+ "(" + "\r\n"
    + "[ID] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
+ ") ON [PRIMARY]" + "\r\n";

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建SparePart表失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public static void CreateBOMConfig(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[ST_" + Index + "_BOMConfig](" + "\r\n"
    + "[BomId] [int] IDENTITY(1,1) NOT NULL," + "\r\n"
    + "[Name] [varchar](50) NULL," + "\r\n"
    + "[Description] [varchar](200) NULL," + "\r\n"
+ " CONSTRAINT [PK_ST_" + Index + "_BOMConfig] PRIMARY KEY CLUSTERED " + "\r\n"
+ "(" + "\r\n"
    + "[BomId] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
+ ") ON [PRIMARY]" + "\r\n";

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建BOMConfig表失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public static void CreateBOMPart(string dbName, int Index)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "CREATE TABLE [dbo].[ST_" + Index + "_BOMPart](" + "\r\n"
    + "[id] [int] IDENTITY(1,1) NOT NULL," + "\r\n"
    + "[BomId] [int] NOT NULL," + "\r\n"
    + "[PartName] [varchar](50) NOT NULL," + "\r\n"
    + "[PartNum] [varchar](50) NOT NULL," + "\r\n"
    + "[SNLength] [int] NOT NULL," + "\r\n"
    + "[CurrentCount] [int] NOT NULL," + "\r\n"
    + "[Warning] [int] NOT NULL," + "\r\n"
    + "[ForeWarning] [int] NOT NULL," + "\r\n"
+ " CONSTRAINT [PK_ST_" + Index + "_BOMPart] PRIMARY KEY CLUSTERED " + "\r\n"
+ "(" + "\r\n"
    + "[id] ASC" + "\r\n"
+ ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" + "\r\n"
+ ") ON [PRIMARY]" + "\r\n";

                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建BOMPart表失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static void ClearTable(string dbName, string  TableName)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "truncate table" +"[dbo].["+TableName+"]";
    
                    bool res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("新建BOMPart表失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static  double SelectLogBufferSize(string dbName)
        {
            double res =0;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "SELECT DB_NAME(database_id) AS DatabaseName, Name AS Logical_Name, Physical_Name, (size*8.0)/1024/1024 SizeGB FROM sys.master_files WHERE DB_NAME(database_id) = '" + dbName + "'";

                    DataTable dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[2].ToString().Contains(".ldf"))
                        {
                            res = double.Parse(dr[3].ToString());
                        }
                    }
              
                }
                catch (Exception ex)
                {
                   //
                    res = 0;
                }
            }
            return res;
        }



        public static bool BackDB(string dbName,string BackPath)
        {
            bool res = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = " BACKUP DATABASE LGDB TO DISK='" + BackPath + "' with format;";

                     res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("备份数据库失败。");
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                    return false;
                }
            }
            return res;
        }


        public static bool CutLog(string dbName)
        {
            bool res = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, dbName, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    string sql = "USE[master]" + "\r\n"

+ "ALTER DATABASE LGDB SET RECOVERY SIMPLE WITH NO_WAIT" + "\r\n"

+ "ALTER DATABASE LGDB SET RECOVERY SIMPLE  " + "\r\n"

+ "USE LGDB" + "\r\n"

+ "DBCC SHRINKFILE (ZJ_MES_log , 2, TRUNCATEONLY) " + "\r\n"

+ "USE[master]" + "\r\n"

+ "ALTER DATABASE LGDB SET RECOVERY FULL WITH NO_WAIT" + "\r\n"

+ "ALTER DATABASE LGDB SET RECOVERY FULL  " + "\r\n";


                     res = mydbUUTDB.ExecuteNonQuery_Create(sql, new List<SqlParameter>());
                    if (!res)
                    {
                        throw new Exception("截断日志失败。");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return res;
        }

    }
}
