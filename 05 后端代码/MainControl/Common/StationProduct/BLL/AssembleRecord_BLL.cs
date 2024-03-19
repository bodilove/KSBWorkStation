using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Product.Model
{
    public class AssembleRecord_BLL
    {
        //单条测试记录写入
        public long AssembleRecordWrite(Common.StationProduct.Model.AssembleRecord Assres)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    long res = GlobalResources.AddT<Common.StationProduct.Model.AssembleRecord>(Assres,"[P_AssembleRes]", mydbUUTDB, true);
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

        //多条测试记录批量写入
        public bool MultiAssembleRecordWrite(List<Common.StationProduct.Model.AssembleRecord> lsAsstres)
        {
            bool res1 = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    long res = 0;
                    foreach (Common.StationProduct.Model.AssembleRecord subst in lsAsstres)
                    {
                        res = GlobalResources.AddT<Common.StationProduct.Model.AssembleRecord>(subst, "[P_AssembleRes]", mydbUUTDB, true);
                        if (res < 0)
                        {
                            break;
                        }
                    }
                    if (res > 0)
                    {
                        mydbUUTDB.Commit();
                        res1 = true;
                    }
                    else
                    {
                        mydbUUTDB.Rollback();
                        res1 = false;
                    }
                }
                catch
                {
                    mydbUUTDB.Rollback();
                    res1 = false;
                }
            }
            return res1;
        }



        //通过ScanCode查询UUTID数量
        public List<Common.StationProduct.Model.AssembleRecord> SelectAssembleRecordByScanCode(string ScanCode)
        {
            List<Common.StationProduct.Model.AssembleRecord> pdlst = new List<Common.StationProduct.Model.AssembleRecord>();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    //"P_SationProduct", " [StationNum]='" + CurrentStationNum + "' AND [StationTestId]", StationTestId, mydbUUTDB
                    pdlst = GlobalResources.GetListInfo<Common.StationProduct.Model.AssembleRecord>("[P_AssembleRes]", "AND [ScanCode]='" + ScanCode + "'" + " ORDER BY [Id] ASC", mydbUUTDB);

                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            return pdlst;
        }
    }
}
