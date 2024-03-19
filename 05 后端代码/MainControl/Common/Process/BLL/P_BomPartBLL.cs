using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;

namespace Common.Process.BLL
{
    public class P_BomPartBLL
    {
        /// <summary>
        /// 根据岗位ID返回岗位实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public P_BomPart GetBomPartInfoById(int id)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    return GlobalResources.GetInfo<P_BomPart>("P_BomPart", "Id", id, mydbUUTDB);
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
        public List<P_BomPart> GetBomPartlList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    if (IsPage)
                    {
                        return GlobalResources.GetPages<P_BomPart>("P_BomPart", record, currentpage, ref allines, "Id", condion, mydbUUTDB);
                    }
                    else
                    {
                        return GlobalResources.GetListInfo<P_BomPart>("P_BomPart", condion, mydbUUTDB);
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
        public bool AddUpdateDeleteP_BomPart(P_BomPart P_bomPart, string condition, string operType)
        {
            bool yorn = false;
            try
            {
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
                {
                    switch (operType.ToUpper())
                    {
                        case "ADD":
                            yorn = GlobalResources.Add<P_BomPart>(P_bomPart, "P_BomPart", mydbUUTDB, false);
                            break;
                        case "UPDATE":
                            yorn = GlobalResources.Update<P_BomPart>(P_bomPart, "P_BomPart", condition, mydbUUTDB);
                            break;
                        case "DELETE":
                            yorn = GlobalResources.Delete("P_BomPart", condition, mydbUUTDB);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {

            }
            return yorn;
        }

    

    
    }
}
