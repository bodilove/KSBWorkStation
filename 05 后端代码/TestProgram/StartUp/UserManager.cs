using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using Test.Common;

namespace Test.StartUp
{
    public class UserManager 
   {
        public SqlConnection m_dbConnection = null;
        private DataContext m_dc = null;

        public SqlConnection DbConnection
        {
            get { return this.m_dbConnection; }
        }

        public bool SubmitChanges()
        {
            bool bSucc = true;
            try
            {
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bSucc = false;
            }

            return bSucc;
            
        }

        public void Close()
        {
            if (m_dbConnection.State != ConnectionState.Closed)
                m_dbConnection.Close();
        }

        public bool Connect()
        {
            bool bRet = true;

            m_dbConnection = new SqlConnection();
            string con = ConfigurationManager.ConnectionStrings["UTopConnectionString"].ToString();
            
            m_dbConnection.ConnectionString = con;

            try
            {
                m_dbConnection.Open();
                m_dc = new DataContext(m_dbConnection);

                string[] s = con.Split(';');
                SQLBAL.dbserver = s[0].Split('=')[1];
                SQLBAL.dbname = s[1].Split('=')[1];
                SQLBAL.userid = s[2].Split('=')[1];
                SQLBAL.password = s[3].Split('=')[1];
            }
            catch (System.Exception)
            {
                bRet = false;
                
                m_dbConnection = null;
            }


            //Common.SQLBAL.dbserver = m_dbConnection.DataSource;
            //Common.SQLBAL.dbname = m_dbConnection.Database;

          


       //Common.SQLBAL. userid =m_dbConnection.
       //Common.SQLBAL.password = "1234";

            return bRet;

        }


        public string[] USERS
        {
            get
            {
                if(m_dbConnection.State != ConnectionState.Open) return new string[0];

                List<string> liRet = new List<string>();
                try
                {

                    Table<Users> us = m_dc.GetTable<Users>();
                    var query =
                        from u in us
                        select u;
                    foreach(var u in query)
                    {
                        liRet.Add(u.Name);
                    }
                   
                }
                catch (System.Exception )
                {
                	
                }
                finally
                {

                }

                return liRet.ToArray();
            }
        }

        public string[] Types
        {
            get
            {
             if (m_dbConnection.State != ConnectionState.Open) return new string[0];
         
                List<string> liRet = new List<string>();

                try
                {
                    Table<Groups> us = m_dc.GetTable<Groups>();
                    var query =
                        from u in us
                        select u;
                    foreach (var u in query)
                    {
                        liRet.Add(u.Type);
                    }
                   
                }
                catch (Exception )
                {

                }
                finally
                {
                   
                }
                return liRet.ToArray();
            }
        }

        public string Description(string user)
        {
             string lpszText = "";

             if (!USERS.Contains(user)) return "";

             if (m_dbConnection.State != ConnectionState.Open) return "";

             try
             {
                 Table<Users> us = m_dc.GetTable<Users>();
                 var query =
                     from u in us
                     where u.Name == user
                     select u.Description;

                 foreach(var ds in query)
                 {
                     lpszText = (ds == null ? "" : ds.Trim()); 
                     break; //only check one 
                 }
             }
             catch (System.Exception )
             {

             }
             finally
             {
             }

             return lpszText;
        }

        public string Type(string user)
        {
            string lpszText = "";

            if (!USERS.Contains(user)) return "";

            if (m_dbConnection.State != ConnectionState.Open) return "";

            //get data from table users
          

            try
            {
                Table<Users> us = m_dc.GetTable<Users>();
                var query =
                    from u in us
                    where u.Name == user
                    select u.Type;

                foreach (var ds in query)
                {
                    lpszText = ds;
                    break; //only check one 
                }
            }
            catch (System.Exception )
            {

            }
            finally
            {
            }

            return lpszText;
        }

        public string Password(string user)
        {
            if (!USERS.Contains(user))
                    throw new Exception("Unauthorized user");

            string lpszText = "";

            try
            {
                Table<Users> us = m_dc.GetTable<Users>();
                var query =
                    from u in us
                    where u.Name == user
                    select u.Password;

                foreach (var ds in query)
                {
                    lpszText = (ds == null? "" : ds.Trim());
                    break; //only check one 
                }
            }
            catch (System.Exception )
            {

            }
            finally
            {
              
            }

               return lpszText;
        }

        public bool RightsOfUser(string user,string key)
        {
                if (!USERS.Contains(user)) return false;

                //get user type
                string strType = Type(user);
                if(!Types.Contains(strType)) return false; //un-register user

                //get right of this type
                return RightsOfType(strType, key);
         }

        public bool RightsOfType(string strType, string key)
        {
            if (!Types.Contains(strType)) return false;

            bool bAuthorized = false;
            if (m_dbConnection.State != ConnectionState.Open) return false;

            try
            {
                Table<Groups> us = m_dc.GetTable<Groups>();
                var query =
                    from u in us
                    where u.Type == strType
                    select u;

                foreach (var ds in query)
                {
                    if (key.Trim() == "Query")
                        bAuthorized = ds.Query;
                    else if (key.Trim() == "BuildProject")
                        bAuthorized = ds.Build;
                    else if (key.Trim() == "SystemManage")
                        bAuthorized = ds.Manage;
                    else if (key.Trim() == "Test")
                        bAuthorized = ds.Test;
                    else
                        bAuthorized = false;

                    break; //only check one 
                }
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {

            }
            finally
            {
            
            }

            return bAuthorized;
        }

        public bool AddType(string name)
        {
            if (Types.Contains(name)) return false;

            bool bRet = true;
               
            try
            {
                //create a new type
                Groups tp = new Groups();
                tp.Type = name;
                tp.Build = false;
                tp.Manage = false;
                tp.Query = false;
                tp.Test = false;
         
                //insert to data context
                m_dc.GetTable<Groups>().InsertOnSubmit(tp);
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {
                
            }

            return bRet;
        }

        public bool ModifyUserName(string oldUser, string newUser)
        {
            if (newUser == string.Empty) return false;
            if (oldUser == newUser) return true;

            bool bRet = true;

            try
            {
                //get old user 
                Table<Users> usrTable = m_dc.GetTable<Users>();
                var query =
                    from u in usrTable
                    where u.Name == oldUser
                    select u;
                foreach (var cu in query)
                {
                    Users usr = new Users();
                    usr.Type = cu.Type;
                    usr.Name = newUser;
                    usr.Password = cu.Password;
                    usr.Description = cu.Description;

                    //insert to data context
                    usrTable.DeleteOnSubmit(cu);
                    m_dc.SubmitChanges();
                    usrTable.InsertOnSubmit(usr);
                    m_dc.SubmitChanges();

                }
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;
        }
        public bool SetUserDescription(string user, string Description)
        {
            if (!this.USERS.Contains(user)) return false;
            bool bRet = true;
            try
            {
                Table<Users> us = m_dc.GetTable<Users>();
                var query =
                    from u in us
                    where u.Name == user
                    select u;

                foreach (var ds in query)
                {
                    ds.Description = Description;
                }
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;
        }
        public bool ModifyTypeName(string oldType, string newType)
        {
            if (newType == string.Empty) return false;
            if (oldType == newType) return true;

            bool bRet = true;

            try
            {
                //get old user 
                Table<Groups> gpGroup = m_dc.GetTable<Groups>();

                var query =
                    from u in gpGroup
                    where u.Type == oldType
                    select u;
                foreach (var cu in query)
                {
                    cu.Type = newType;
                }
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;

        }

        public bool IsRootUser(string name)
        {
            if (m_dbConnection.State != ConnectionState.Open) return false;

            //get data from table users
            bool bIsUser = false;
            try
            {
                Table<root> us = m_dc.GetTable<root>();
                var query =
                    from u in us
                    where u.Name == name
                    select u.Name;

                 foreach(var ds in query)
                 {
                     bIsUser = true;
                     break;
                 }
              
            }
            catch (System.Exception )
            {

            }
            finally
            {
            }

           return bIsUser;
        }
       
        public bool SetRightOfType(string type,string key, bool Enable = true)
        {
            bool bRet = true;
            if (!Types.Contains(type)) return false;

            try
            {
                Table<Groups> us = m_dc.GetTable<Groups>();
                var query =
                    from u in us
                    where u.Type == type
                    select u;

                foreach (var ds in query)
                {
                    if (key.Trim() == "Query")
                        ds.Query = Enable;
                    else if (key.Trim() == "BuildProject")
                        ds.Build = Enable;
                    else if (key.Trim() == "SystemManage")
                        ds.Manage = Enable;
                    else if (key.Trim() == "Test")
                        ds.Test = Enable;
                    else
                        bRet = false;
                    break; //only check one 
                }
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;
        }  
 
        public bool DeleteType(string type)
        {
            bool bRet = true;
            try
            {
                Table<Groups> us = m_dc.GetTable<Groups>();
                
                //get target type
                var query =
                    from t in us
                    where t.Type == type
                    select t;

                //delete type
                foreach( var t in query)
                {
                    us.DeleteOnSubmit(t);
                }
                m_dc.SubmitChanges();

            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;

        }

        public bool DeleteUser(string usr)
        {
            bool bRet = true;
            try
            {
                Table<Users> us = m_dc.GetTable<Users>();

                //get target type
                var query =
                    from t in us
                    where t.Name == usr
                    select t;

                //delete type
                foreach (var t in query)
                {
                    us.DeleteOnSubmit(t);
                }
                m_dc.SubmitChanges();
            }
            catch (System.Exception )
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;
        }

        public bool SetPasswordOfUser(string usr,string newPwr)
        {
            if (!this.USERS.Contains(usr)) return false;

            bool bRet = true;

            try
            {
                //get old user 
                Table<Users> usrTable = m_dc.GetTable<Users>();

                var query =
                    from u in usrTable
                    where u.Name == usr
                    select u;
                foreach (var cu in query)
                {
                    cu.Password = newPwr;
                }
                m_dc.SubmitChanges();

            }
            catch (System.Exception)
            {
                bRet = false;
            }
            finally
            {

            }

            return bRet;
        }

         public bool SetTypeOfUser(string usr,string newType)
        {
            if (!this.USERS.Contains(usr)) return false;
            if (!this.Types.Contains(newType)) return false;

            bool bRet = true;

            try
            {
                //get old user 
                Table<Users> usrTable = m_dc.GetTable<Users>();

                var query =
                    from u in usrTable
                    where u.Name == usr
                    select u;
                foreach (var cu in query)
                {
                    cu.Type = newType;
                }
                m_dc.SubmitChanges();

            }
            catch (System.Exception)
            {
                bRet = false;
            }

            return bRet;
        }

        public bool AddUser(string name)
        {
            if (this.USERS.Contains(name)) return false;

            bool bRet = true;

            try
            {
                //create a new type
                Users usr = new Users();
                usr.Type = "Operator";
                usr.Name = name;
                usr.Password = "";
                usr.Description = "";
      
                //insert to data context
                m_dc.GetTable<Users>().InsertOnSubmit(usr);
                m_dc.SubmitChanges();

            }
            catch (System.Exception)
            {
                bRet = false;
            }

            return bRet;

        }

    }
}
