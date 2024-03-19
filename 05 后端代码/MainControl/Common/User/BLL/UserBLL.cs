using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.User.Model;

namespace Common.User.BLL
{
    public class UserBLL
    {
        /// <summary>
        /// 根据工号返回用户实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserNum(string userNum, string password)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                string sql = "";
                if (userNum != "")
                {
                    sql = sql + string.Format(" and UserNum='{0}'", userNum);
                }
                if (password != "")
                {
                    sql = sql + string.Format(" and PassWord='{0}'", password);
                }
                return GlobalResources.GetListInfo<UserInfo>("T_User", sql, mydbUUTDB).Count > 0 ? GlobalResources.GetListInfo<UserInfo>("T_User", " and UserNum='" + userNum + "'", mydbUUTDB)[0] : null;
            }
        }


        /// <summary>
        /// 根据工号返回用户实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserNumAndPassword(string userNum, string password)
        {

            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                string sql = string.Format(" and UserNum='{0}' and PassWord='{1}' ", userNum, password);


                return GlobalResources.GetListInfo<UserInfo>("T_User", sql, mydbUUTDB).Count > 0 ? GlobalResources.GetListInfo<UserInfo>("T_User", " and UserNum='" + userNum + "'", mydbUUTDB)[0] : null;
            }
        }

        /// <summary>
        /// 根据工号返回相同的用户实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public UserInfo GetSameUserInfoByUserNum(string userName, string userNum, int userId, string operType)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                string sql = "";
                if (operType == "Add")
                {
                    if (userName != "")
                    {
                        sql = sql + string.Format(" and UserName='{0}'", userName);
                    }
                    if (userNum != "")
                    {
                        sql = sql + string.Format(" and UserNum='{0}'", userNum);
                    }

                }
                else
                {
                    if (userName != "")
                    {
                        sql = sql + string.Format(" and UserName='{0}'", userName);
                    }
                    if (userNum != "")
                    {
                        sql = sql + string.Format(" and UserNum='{0}'", userNum);
                    }
                    sql = sql + string.Format(" and UserId<>{0}", userId);
                }
                return GlobalResources.GetListInfo<UserInfo>("T_User", sql, mydbUUTDB).Count > 0 ? GlobalResources.GetListInfo<UserInfo>("T_User", " and UserNum='" + userNum + "'", mydbUUTDB)[0] : null;
            }
        }

        /// <summary>
        /// 返回用户List
        /// </summary>
        /// <param name="record"></param>
        /// <param name="currentpage"></param>
        /// <param name="allines"></param>
        /// <param name="condion"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                if (IsPage)
                {

                    return GlobalResources.GetPages<UserInfo>("T_User", record, currentpage, ref allines, "UserNum", condion, mydbUUTDB);
                }
                else
                {
                    return GlobalResources.GetListInfo<UserInfo>("T_User", condion, mydbUUTDB);
                }
            }
        }
        /// <summary>
        /// 添加修改删除用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="IdCollect"></param>
        /// <param name="operType"></param>
        /// <returns></returns>
        public bool AddUpdateDeleteUser(UserInfo userInfo, string condition, string operType)
        {
            bool yorn = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                switch (operType.ToUpper())
                {
                    case "ADD":
                        yorn = GlobalResources.Add<UserInfo>(userInfo, "T_User", mydbUUTDB,false);
                        break;
                    case "UPDATE":
                        yorn = GlobalResources.Update<UserInfo>(userInfo, "T_User", condition, mydbUUTDB);
                        break;
                    case "DELETE":
                        yorn = GlobalResources.Delete("T_User", condition, mydbUUTDB);
                        break;
                    default:
                        break;
                }
            }
            return yorn;
        }



    }
}
