using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Product.BLL
{
    public class InWorkPBLL
    {
        /// <summary>
        /// 增加一个产品
        /// </summary>
        /// <param name="pdif"></param>
        /// <returns></returns>
        public long AddProduct(Common.Product.Model.ProductInfo pdif)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    long res = GlobalResources.AddT<Common.Product.Model.ProductInfo>(pdif, "P_InWork", mydbUUTDB,true);
                    return res;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 更新一个产品
        /// </summary>
        /// <param name="pdif"></param>
        public bool UpdateByUUTestId(Common.Product.Model.InWorkP_Info pdif)
        {
            bool res = false;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                res = GlobalResources.Update<Common.Product.Model.InWorkP_Info>(pdif, "P_InWork", "AND [UUTestId]=" + pdif.UUTestId, mydbUUTDB);
            }
            //bool res = SqlHandle.Add<ProductInfo>(pdif, "P_Product");

            return res;
        }



        public void DeleteByUUTestId(int UUTestId)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                GlobalResources.Delete("P_InWork", "AND [UUTestId]=" + UUTestId, mydbUUTDB);
            }
        }

        public Common.Product.Model.InWorkP_Info SelectInWorkProductInfo(long uutId)
        {
            Common.Product.Model.InWorkP_Info pd = null;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    pd = GlobalResources.GetInfo<Common.Product.Model.InWorkP_Info>("P_InWork", "[UUTestId]", uutId, mydbUUTDB);

                }
                catch
                {
                    return null;
                }
            }
            return pd;
        }

        public List<Common.Product.Model.InWorkP_Info> SelectInWorkProductInfo(string SN)
        {
            List<Common.Product.Model.InWorkP_Info> pdlst = new List<Model.InWorkP_Info>();
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    //"P_SationProduct", " [StationNum]='" + CurrentStationNum + "' AND [StationTestId]", StationTestId, mydbUUTDB
                    pdlst = GlobalResources.GetListInfo<Common.Product.Model.InWorkP_Info>("P_InWork", "AND [SN]='" + SN+"'", mydbUUTDB);

                }
                catch
                {
                
                }
            }
            return pdlst;
        }

        /// <summary>
        /// 清空产品列表，id从清空前的id开始
        /// </summary>
        /// <returns></returns>
        public bool ClearProductTable()//清空，id从清空前的id开始
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                return GlobalResources.ClearTable("P_InWork", mydbUUTDB);
            }
        }

        /// <summary>
        /// 清空产品列表，id从1开始
        /// </summary>
        /// <returns></returns>
        public bool TruncateProductTable()//清空，id从1开始
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                return GlobalResources.TruncateTable("P_InWork", mydbUUTDB);
            }
        }
    }
}
