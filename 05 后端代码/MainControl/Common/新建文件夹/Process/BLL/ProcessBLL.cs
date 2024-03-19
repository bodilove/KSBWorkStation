using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;

namespace Common.Process.BLL
{
    public class P_ProcessBLL
    {
        /// <summary>
        /// 根据岗位ID返回岗位实例
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public P_Process GetProcessInfoById(int id)
        {
            return GlobalResources.GetInfo<P_Process>("P_Process", "Id", id);
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
        public List<P_Process> GetProcessList(int record, int currentpage, ref long allines, string condion, bool IsPage)
        {
            if (IsPage)
            {
                return GlobalResources.GetPages<P_Process>("P_Process", record, currentpage, ref allines, "Id", condion);
            }
            else
            {
                return GlobalResources.GetListInfo<P_Process>("P_Process", condion);
            }
        }


        /// <summary>
        /// 根据工号返回用户实例
        /// </summary>
        /// <param name="userNum"></param>
        /// <returns></returns>
        public List<P_Process> GetProcessInfoList(string productnum, string stationnum)
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
            return GlobalResources.GetListInfo<P_Process>("P_Process", sql);
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
        public bool AddUpdateDeleteP_Process(P_Process Process, string condition, string operType)
        {
            bool yorn = false;
            switch (operType.ToUpper())
            {
                case "ADD":
                    yorn = GlobalResources.Add<P_Process>(Process, "P_Process");
                    break;
                case "UPDATE":
                    yorn = GlobalResources.Update<P_Process>(Process, "P_Process", condition);
                    break;
                case "DELETE":
                    yorn = GlobalResources.Delete("P_Process", condition);
                    break;
                default:
                    break;
            }
            return yorn;
        }

        public bool AddProcessList(string product, List<P_Process> hlist)
        {
            bool yorn = false;
            if (AddUpdateDeleteP_Process(null, string.Format(" and ProductNum='{0}'", product), "DELETE"))
            {
                for (int i = 0; i < hlist.Count; i++)
                {
                    AddUpdateDeleteP_Process(hlist[i], "", "ADD");
                }

                yorn = true;

            }
            return yorn;
        }


        /// <summary>
        /// 根据工艺名称查询工艺
        /// </summary>
        /// <param name="halfnum">查询工艺</param>
        /// <returns></returns>
        public P_Process GetProcessInfoByName(string ProcessName)
        {


            string sql = "";
            if (ProcessName != "")
            {
                sql = sql + string.Format(" and Name='{0}'", ProcessName);
            }

            //if (partid > 0)
            //{
            //    sql = sql + string.Format(" and PartId<>{0}", partid);
            //}
            List<P_Process> tlist = GlobalResources.GetListInfo<P_Process>("P_Process", sql);
            if (tlist != null && tlist.Count > 0)
            {
                return tlist[0];
            }
            else
            {
                return null;
            }
        }
    
    }
}
