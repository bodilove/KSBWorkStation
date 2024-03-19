using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;

namespace Common.Process.BLL
{
    public class P_TestItemConfigBLL
    {
        /// <summary>
        /// 根据岗位ID返回岗位实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public P_TestItemConfig GetTestItemInfoById(int id)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    return GlobalResources.GetInfo<P_TestItemConfig>("P_TestItemConfig", "Id", id, mydbUUTDB);
                }
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 返回岗位List
        /// </summary>
        /// <param name="record"></param>
        /// <param name="currentpage"></param>
        /// <param name="allines"></param>
        /// <param name="condion"></param>
        /// <param name="IsPage"></param>
        /// <returns></returns>
        public List<P_TestItemConfig> GetTestItemConfigList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    if (IsPage)
                    {
                        return GlobalResources.GetPages<P_TestItemConfig>("P_TestItemConfig", record, currentpage, ref allines, "Id", condion, mydbUUTDB);
                    }
                    else
                    {
                        return GlobalResources.GetListInfo<P_TestItemConfig>("P_TestItemConfig", condion, mydbUUTDB);
                    }
                }
            }
            catch
            {
                return null;
            }
        }







        /// <summary>
        /// 添加、修改、删除岗位
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="IdCollect"></param>
        /// <param name="operType"></param>
        /// <returns></returns>
        public bool AddUpdateDeleteP_TestItemConfig(P_TestItemConfig TestItem, string condition, string operType)
        {
            bool yorn = false;
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    switch (operType.ToUpper())
                    {
                        case "ADD":
                            yorn = GlobalResources.Add<P_TestItemConfig>(TestItem, "P_TestItemConfig", mydbUUTDB, true);
                            break;
                        case "UPDATE":
                            yorn = GlobalResources.Update<P_TestItemConfig>(TestItem, "P_TestItemConfig", condition, mydbUUTDB);
                            break;
                        case "DELETE":
                            yorn = GlobalResources.Delete("P_TestItemConfig", condition, mydbUUTDB);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                return false;
            }
            return yorn;
        }

        //public bool AddP_DetailList(string product, List<P_BomPart> hlist)
        //{
        //    bool yorn = false;
        //    if (AddUpdateDeleteP_BomPart(null, string.Format(" and ProductNum='{0}'", product), "DELETE"))
        //    {
        //        for (int i = 0; i < hlist.Count; i++)
        //        {
        //            AddUpdateDeleteP_BomPart(hlist[i], "", "ADD");
        //        }
        //        yorn = true;
        //    }
        //    return yorn;
        //}
        //public bool updateOrderNum(int id, int OrderNum, int ForeWarningNum, int WarningNum, int Enable)
        //{
        //    return GlobalResources.excsql("P_TestItemConfig", "[OrderNum]=" + OrderNum + ",[ForeWarning]=" + ForeWarningNum + ",[Warning]=" + WarningNum + ",[Enable]=" + Enable, "AND [Id]=" + id);
        //}

    }
}
