using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Product.BLL
{
    public class ProductBLL
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
                    long res = GlobalResources.AddT<Common.Product.Model.ProductInfo>(pdif, "P_Product", mydbUUTDB,true);
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
        public bool UpdateByUUTestId(Common.Product.Model.ProductInfo pdif)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                bool res = GlobalResources.Update<Common.Product.Model.ProductInfo>(pdif, "P_Product", "AND [UUTestId]=" + pdif.UUTestId, mydbUUTDB);
                return res;
            }
            //bool res = SqlHandle.Add<ProductInfo>(pdif, "P_Product");
        }

        /// <summary>
        /// 根据UUTestId 获取产品
        /// </summary>
        /// <param name="UUTestId"></param>
        /// <returns></returns>
        public Common.Product.Model.ProductInfo GetProductInfo(long UUTestId)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    Common.Product.Model.ProductInfo pd = GlobalResources.GetInfo<Common.Product.Model.ProductInfo>("P_Product", "[UUTestId]", UUTestId, mydbUUTDB);

                    return pd;
                }

            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 清空产品列表，id从清空前的id开始
        /// </summary>
        /// <returns></returns>
        public bool ClearProductTable()//清空，id从清空前的id开始
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                return GlobalResources.ClearTable("P_Product", mydbUUTDB);
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
                return GlobalResources.TruncateTable("P_Product", mydbUUTDB);
            }
        }
    }
}
