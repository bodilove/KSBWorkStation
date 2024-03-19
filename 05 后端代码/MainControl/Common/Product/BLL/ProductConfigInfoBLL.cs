using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Product.BLL
{
    public class ProductConfigInfoBLL
    {
      

        /// <summary>
        /// 更新一个产品
        /// </summary>
        /// <param name="pdif"></param>
        public Common.Product.Model.ProductConfigInfo SelectProductConfigInfoByProductNum(string ProductNum)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {

                    string sql = string.Format(" and [ProductNum]='{0}'", ProductNum);


                    List<Common.Product.Model.ProductConfigInfo> pd = GlobalResources.GetListInfo<Common.Product.Model.ProductConfigInfo>("T_Product", sql, mydbUUTDB);
                    if (pd.Count == 1)
                    {
                        return pd[0];
                    }
                    else
                    {
                        throw new Exception("查询产品配置出错。");
                    }
                }

            }
            catch
            {
                return null;
            }
            //bool res = SqlHandle.Add<ProductInfo>(pdif, "P_Product");
        }

       
    }
}
