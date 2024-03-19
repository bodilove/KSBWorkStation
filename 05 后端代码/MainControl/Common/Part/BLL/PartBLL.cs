using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Product.Model;

namespace Common.Part.BLL
{
    public class PartBLL
    {
        /// <summary>
        /// 根据PartID返回零件实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public PartInfo GetPartInfoByPartId(int partid)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    return GlobalResources.GetInfo<PartInfo>("T_Part", "PartId", partid, mydbUUTDB);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据PartNo返回零件实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public PartInfo GetPartInfoByPartNo(string partNo)
        {

            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    string sql = string.Format(" and PartNo='{0}' ", partNo);
                    return GlobalResources.GetListInfo<PartInfo>("T_Part", sql, mydbUUTDB).Count > 0 ? GlobalResources.GetListInfo<PartInfo>("T_Part", " and PartNo='" + partNo + "'", mydbUUTDB)[0] : null;
                }
            }
            catch
            {
                return null;
            }
        }

       
    }
}
