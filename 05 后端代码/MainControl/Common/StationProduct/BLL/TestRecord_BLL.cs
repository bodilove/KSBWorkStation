using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Product.Model
{
    public class TestRecord_BLL
    {

        //单条测试记录写入
        public long SingleTestResWrite(Common.StationProduct.Model.TestRecord stres)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    long res = GlobalResources.AddT<Common.StationProduct.Model.TestRecord>(stres, "[P_TestRes]", mydbUUTDB, true);
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
        public bool MultiTestResWrite(List<Common.StationProduct.Model.TestRecord> lstres)
        {
            bool res1 = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    long res = 0;
                    foreach (Common.StationProduct.Model.TestRecord subst in lstres)
                    {
                        res = GlobalResources.AddT<Common.StationProduct.Model.TestRecord>(subst, "[P_TestRes]", mydbUUTDB, true);
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
 

    }
}
