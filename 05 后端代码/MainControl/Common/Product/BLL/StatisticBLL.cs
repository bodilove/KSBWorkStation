using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common.Product.BLL
{
    public class StatisticBLL
    {
     
        public bool CheckRollingNumTable(string PartNum)
        {
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    int res = GlobalResources.IsExists("Statistic", "PartNr", PartNum, mydbUUTDB);
                    if (res == 0)
                    {
                        Common.Product.Model.Statistic_Info sc = new Model.Statistic_Info();
                        sc.PartNr = PartNum;
                        sc.Date = System.DateTime.Now;
                        if (!GlobalResources.Add<Common.Product.Model.Statistic_Info>(sc, "Statistic", mydbUUTDB, false))
                        {
                            throw new Exception("SqlHandleNew.Add<Common.Product.Model.Statistic_Info>失败。");
                        }
                    }
                    if (res <0)
                    {
                        throw new Exception("连接数据库失败。");
                    }
                    List<Common.Product.Model.Statistic_Info> lst = GlobalResources.GetListInfo<Common.Product.Model.Statistic_Info>("Statistic", "AND [PartNr] = " + "'" + PartNum + "'", mydbUUTDB);
                  

                    DateTime CurrentDate = System.DateTime.Now;
                    if (lst[0].Date.Year != CurrentDate.Year || lst[0].Date.Month != CurrentDate.Month || lst[0].Date.Day != CurrentDate.Day)
                    {
                        lst[0].DayRollingNum = 0;
                    }
                    lst[0].Date = CurrentDate;
                    if (GlobalResources.Update<Common.Product.Model.Statistic_Info>(lst[0], "Statistic", "AND [PartNr]  = " + "'" + PartNum + "'", mydbUUTDB))
                    {
                        mydbUUTDB.Commit();
                        return true;
                    }
                    else
                    {
                        
                        mydbUUTDB.Rollback();
                        return false;
                    }

                }
                catch
                {
                    mydbUUTDB.Rollback();
                    return false;
                }
            }
      
        }

        public int requestDayRollingNum(string PartNum)
        {
            int Dayrolling = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    int res = GlobalResources.IsExists("Statistic", "PartNr", PartNum, mydbUUTDB);
                    if (res == 0)
                    {
                        Common.Product.Model.Statistic_Info sc = new Model.Statistic_Info();
                        sc.PartNr = PartNum;
                        sc.Date = System.DateTime.Now;
                        if (!GlobalResources.Add<Common.Product.Model.Statistic_Info>(sc, "Statistic", mydbUUTDB, false))
                        {
                            throw new Exception("SqlHandleNew.Add<Common.Product.Model.Statistic_Info>失败。");
                        }
                    }
                    if (res < 0)
                    {
                        throw new Exception("连接数据库失败。");
                    }
                    List<Common.Product.Model.Statistic_Info> lst = GlobalResources.GetListInfo<Common.Product.Model.Statistic_Info>("Statistic", "AND [PartNr] = " + "'" + PartNum + "'", mydbUUTDB);


                    DateTime CurrentDate = System.DateTime.Now;
                    if (lst[0].Date.Year != CurrentDate.Year || lst[0].Date.Month != CurrentDate.Month || lst[0].Date.Day != CurrentDate.Day)
                    {
                        lst[0].DayRollingNum = 0;
                    }
                    lst[0].Date = CurrentDate;
                    lst[0].DayRollingNum ++;
                    lst[0].RollingNum++;
                    if (GlobalResources.Update<Common.Product.Model.Statistic_Info>(lst[0], "Statistic", "AND [PartNr]  = " + "'" + PartNum + "'", mydbUUTDB))
                    {
                        mydbUUTDB.Commit();
                        Dayrolling = lst[0].DayRollingNum;
                    }
                    else
                    {
                        Dayrolling = -2;
                        mydbUUTDB.Rollback();
                    }

                }
                catch
                {
                    mydbUUTDB.Rollback();
                    return -2;
                }
            }
            return Dayrolling;
        }

        public int requestTotalRollingNum(string PartNum)
        {
            int Totalrolling = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    int res = GlobalResources.IsExists("Statistic", "PartNr", PartNum, mydbUUTDB);
                    if (res == 0)
                    {
                        Common.Product.Model.Statistic_Info sc = new Model.Statistic_Info();
                        sc.PartNr = PartNum;
                        if (!GlobalResources.Add<Common.Product.Model.Statistic_Info>(sc, "Statistic", mydbUUTDB, false))
                        {
                            throw new Exception("SqlHandleNew.Add<Common.Product.Model.Statistic_Info>失败。");
                        }
                    }
                    if (res < 0)
                    {
                        throw new Exception("连接数据库失败。");
                    }
                    List<Common.Product.Model.Statistic_Info> lst = GlobalResources.GetListInfo<Common.Product.Model.Statistic_Info>("Statistic", "AND [PartNr] = " + "'" + PartNum + "'", mydbUUTDB);


                    DateTime CurrentDate = System.DateTime.Now;
                    if (lst[0].Date.Year != CurrentDate.Year || lst[0].Date.Month != CurrentDate.Month || lst[0].Date.Day != CurrentDate.Day)
                    {
                        lst[0].DayRollingNum = 0;
                    }
                    lst[0].Date = CurrentDate;
                    lst[0].DayRollingNum++;
                    lst[0].RollingNum++;
                    if (GlobalResources.Update<Common.Product.Model.Statistic_Info>(lst[0], "Statistic", "AND [PartNr]  = " + "'" + PartNum + "'", mydbUUTDB))
                    {
                        mydbUUTDB.Commit();
                        Totalrolling = lst[0].RollingNum;
                    }
                    else
                    {
                        Totalrolling = -2;
                        mydbUUTDB.Rollback();
                    }

                }
                catch
                {
                    mydbUUTDB.Rollback();
                    return -2;
                }
            }
            return Totalrolling;

        }


        public int UpdateCounter(string PartNum,bool AddPassOrFail)
        {
            int restol = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {

                mydbUUTDB.BeginTransaction();
                try
                {
                    int res = GlobalResources.IsExists("Statistic", "PartNr", PartNum, mydbUUTDB);
                    if (res == 0)
                    {
                        Common.Product.Model.Statistic_Info sc = new Model.Statistic_Info();
                        sc.PartNr = PartNum;
                        if (!GlobalResources.Add<Common.Product.Model.Statistic_Info>(sc, "Statistic", mydbUUTDB, false))
                        {
                            throw new Exception("SqlHandleNew.Add<Common.Product.Model.Statistic_Info>失败。");
                        }
                    }
                    if (res < 0)
                    {
                        throw new Exception("连接数据库失败。");
                    }
                    List<Common.Product.Model.Statistic_Info> lst = GlobalResources.GetListInfo<Common.Product.Model.Statistic_Info>("Statistic", "AND [PartNr] = " + "'" + PartNum + "'", mydbUUTDB);


                    //DateTime CurrentDate = System.DateTime.Now;
                    //if (lst[0].Date.Year != CurrentDate.Year || lst[0].Date.Month != CurrentDate.Month || lst[0].Date.Day != CurrentDate.Day)
                    //{
                    //    lst[0].DayRollingNum = 0;
                    //}
                    //lst[0].Date = CurrentDate;
                    //lst[0].DayRollingNum++;
                    //lst[0].RollingNum++;
                    if (AddPassOrFail)
                    {
                        lst[0].PassCount++;
                    }
                    else
                    {
                        lst[0].FailCount++;
                    }
                    if (GlobalResources.Update<Common.Product.Model.Statistic_Info>(lst[0], "Statistic", "AND [PartNr]  = " + "'" + PartNum + "'", mydbUUTDB))
                    {
                        mydbUUTDB.Commit();
                        restol = 0;
                    }
                    else
                    {
                        restol = -1;
                        mydbUUTDB.Rollback();
                    }

                }
                catch
                {
                    mydbUUTDB.Rollback();
                    return -1;
                }
            }
            return restol;

        }

   
    }
}
