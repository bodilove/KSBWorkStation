using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.StationProduct.BLL
{
    public class StationRecord_BLL
    {
        /// <summary>
        /// 每站产品开始
        /// </summary>
        /// <param name="pdif"></param>
        /// <param name="StationIndex"></param>
        /// <returns></returns>
        public long StationStart(Common.StationProduct.Model.StationRecord  pdif)
        {

            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    long res = GlobalResources.AddT<Common.StationProduct.Model.StationRecord>(pdif, "P_SationProduct", mydbUUTDB, true);
                    if (res > 0)
                    {
                        return res;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch
                {
                    return -1;
                }
            }
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="UUTestId"></param>
        /// <param name="EndType">1,在制，2，下线，3异常下线</param>
        /// <returns></returns>
        public bool StationEnd(long StationTestId, int EndType, int result, string CurrentStationNum)
        {
            bool res = false;

            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                //查询出该产品的信息
                Common.StationProduct.Model.StationRecord pd = GlobalResources.GetInfo<Common.StationProduct.Model.StationRecord>("P_SationProduct", " [StationNum]='" + CurrentStationNum + "' AND [StationTestId]", StationTestId, mydbUUTDB);

                if (pd == null || pd.StationTestId < 1)
                {

                    return false;
                }

                try
                {
                    pd.EndTime = System.DateTime.Now;

                    pd.Result = result;

                    res = GlobalResources.Update<Common.StationProduct.Model.StationRecord>(pd, "P_SationProduct", "AND [StationTestId]=" + StationTestId, mydbUUTDB);

                    if (res)
                    {
                        res = true;
                    }
                    else
                    {
                    }
                }
                catch
                {
                }
            }
            return res;
        }



        public Common.StationProduct.Model.StationRecord SelectStationRecord(long StationTestId)
        {
            Common.StationProduct.Model.StationRecord pd = null;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    pd = GlobalResources.GetInfo<Common.StationProduct.Model.StationRecord>("P_SationProduct", "[StationTestId]", StationTestId, mydbUUTDB);

                }
                catch
                {
                    return null;
                }
            }
            return pd;
        }


        public List<Common.StationProduct.Model.StationRecord> GetStationRecordByUUTidList(long UUTiD)
        {
            string sql = string.Format(" and [UUTestId]='{0}'  ORDER BY [StationTestId] ASC", UUTiD);
            List<Common.StationProduct.Model.StationRecord> pdlst = null;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    pdlst = GlobalResources.GetListInfo<Common.StationProduct.Model.StationRecord>("P_SationProduct", sql, mydbUUTDB);

                }
                catch
                {
                    return null;
                }
            }
            return pdlst;
        }
    }
}
