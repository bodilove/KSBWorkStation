using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;

namespace Common.Process.BLL
{
    public class P_DetailBLL
    {
        /// <summary>
        /// 根据岗位ID返回岗位实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public P_Detail GetDetailsInfoById(int id)
        {
            return GlobalResources.GetInfo<P_Detail>("P_Detail", "Id", id);
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
        public List<P_Detail> GetDetailList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            if (IsPage)
            {
                return GlobalResources.GetPages<P_Detail>("P_Detail", record, currentpage, ref allines, "Id", condion);
            }
            else
            {
                return GlobalResources.GetListInfo<P_Detail>("P_Detail", condion);
            }
        }


        /// <summary>
        /// 根据工号返回用户实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public List<P_Detail> GetDetailList(string productnum, string stationnum)
        {
            string sql = "";
            if (productnum != "")
            {
                sql = sql + string.Format(" and ProductNum='{0}'", productnum);
            }
            if (stationnum != "")
            {
                sql = sql + string.Format(" and StationNum='{0}'", stationnum);
            }
            sql = sql + " order by OrderNum";
            return GlobalResources.GetListInfo<P_Detail>("P_Detail", sql);
        }

        ///// <summary>
        ///// 查询是否最后一个工站
        ///// </summary>
        ///// <returns></returns>
        //public P_Process GetLastStation(string productname) 
        //{
        //    string sql = string.Format(" and OrderNum in (select Max(OrderNum) from T_ProductToStation where ProductNum='{0}') and ProductNum='{1}'", productname, productname);

        //    List<P_Process> tlist = GlobalResources.GetListInfo<P_Process>("P_Process", sql);
        //    if (tlist != null && tlist.Count > 0)
        //    {
        //        return tlist[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        /// <summary>
        /// 添加、修改、删除岗位
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="IdCollect"></param>
        /// <param name="operType"></param>
        /// <returns></returns>
        public bool AddUpdateDeleteP_Detail(P_Detail P_Detail, string condition, string operType)
        {
            bool yorn = false;
            switch (operType.ToUpper())
            {
                case "ADD":
                    yorn = GlobalResources.Add<P_Detail>(P_Detail, "P_Detail");
                    break;
                case "UPDATE":
                    yorn = GlobalResources.Update<P_Detail>(P_Detail, "P_Detail", condition);
                    break;
                case "DELETE":
                    yorn = GlobalResources.Delete("P_Detail", condition);
                    break;
                default:
                    break;
            }
            return yorn;
        }

        public bool AddP_DetailList(string product, List<P_Detail> hlist)
        {
            bool yorn = false;
            if (AddUpdateDeleteP_Detail(null, string.Format(" and ProductNum='{0}'", product), "DELETE"))
            {
                for (int i = 0; i < hlist.Count; i++)
                {
                    AddUpdateDeleteP_Detail(hlist[i], "", "ADD");
                }

                yorn = true;

            }
            return yorn;
        }


        public bool updateOrderNum(int id, int OrderNum)
        {
            return GlobalResources.excsql("P_Detail", "[OrderNum]=" + OrderNum, "AND [Id]=" + id);
        }
    
    }
}
