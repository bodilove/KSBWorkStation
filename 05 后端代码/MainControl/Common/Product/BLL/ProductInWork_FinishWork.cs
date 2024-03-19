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
    //public enum LineMode
    //{
    //    Normal =1,//正常生产
    //    ReWork =2,//返工模式
    //    PowerOnCheck=3,//开机点检
    //    Manual=-1,//手动模式
    //}

    public class ProductInWork_FinishWork
    {
        private long ProductStart(Common.Product.Model.ProductInfo pdif, MyDB_Sql mydbUUTDB, string CurrentStation)//进线,返回UUTID
        {
            long Scalar = -1;
            //using (MyDB_Sql mydbUUTDB = new MyDB_Sql(SqlHandle.dbserver, SqlHandle.dbname, SqlHandle.userid, SqlHandle.password))
            //{
                try
                {

                    List<SqlParameter> _parames;
                    //开启事务
                    mydbUUTDB.BeginTransaction();
                 //   string sql = SqlHandleNew.SqlCommand<Common.Product.Model.ProductInfo>(pdif, "ADD", "P_Product", "", out _parames, true) + " select id=@@identity"; 
                 ////   Scalar = long.Parse(mydbUUTDB.ExecuteScalar(sql, _parames));
                 //   Scalar = int.Parse(mydbUUTDB.ExecuteScalar(sql, _parames));
                    Scalar = GlobalResources.AddT<Common.Product.Model.ProductInfo>(pdif, "P_Product", mydbUUTDB,true);

                    if (Scalar > 0)
                    {
                        Common.Product.Model.InWorkP_Info InPro = new Common.Product.Model.InWorkP_Info();
                        InPro.UUTestId = Scalar;
                        InPro.UserName = pdif.UserName;
                        InPro.SN = pdif.SN;
                        InPro.StartTime = pdif.StartTime;
                        InPro.PrdType = pdif.PrdType;
                        InPro.PartNum = pdif.PartNum;
                        InPro.CurrentResult = pdif.Result;
                        InPro.ExpectedPathNames =pdif.ExpectedPathNames;
                        InPro.ExpectedPath = pdif.ExpectedPath;
                        InPro.ActualPath = pdif.ActualPath;
                        InPro.ActualResult = pdif.ActualResult;
                        InPro.CurrentStation = CurrentStation;
                        InPro.CurrentSationTestID = -1;
                        InPro.ProductSN = pdif.ProductSN;
                        bool res = GlobalResources.Add<Common.Product.Model.InWorkP_Info>(InPro, "P_InWork", mydbUUTDB,false);
                        //if (res < 0)
                        //{
                        //    Scalar = -1;
                        //}
                        //sql = SqlHandleNew.SqlCommand<Common.Product.Model.InWorkP_Info>(InPro, "ADD", "P_InWork", "", out _parames, false);
                        //int id = mydbUUTDB.ExecuteNonQuery(sql, _parames);
                        //if (Scalar > 0)
                        //{
                        //    yorn = true;
                        //}
                        //long id = long.Parse(mydbUUTDB.ExecuteScalar(sql, _parames));
                        if (res)
                        {
                            mydbUUTDB.Commit();
                        }
                        else
                        {
                            Scalar = -1;
                            mydbUUTDB.Rollback();
                        }
                    }
                    else
                    {
                        Scalar = -1;
                        mydbUUTDB.Rollback();
                    }
                }
                catch
                {
                    Scalar = -1;
                    mydbUUTDB.Rollback();
                }
            //}
            return Scalar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UUTestId"></param>
        /// <param name="EndType">1,在制，2，下线，3异常下线</param>
        /// <returns></returns>
        public bool ProductEnd(long UUTestId, int EndType, int result)
        {
            bool res = false;

            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                //查询出该产品的信息
                Common.Product.Model.ProductInfo pd = GlobalResources.GetInfo<Common.Product.Model.ProductInfo>("P_Product", "[UUTestId]", UUTestId, mydbUUTDB);

                if (pd == null||pd.UUTestId<1)
                {
                    throw new Exception("产品列表未查询到产品");
                }



                Common.Product.Model.InWorkP_Info pinw = GlobalResources.GetInfo<Common.Product.Model.InWorkP_Info>("P_InWork", "[UUTestId]", UUTestId, mydbUUTDB);
                if (pinw == null || pinw.UUTestId < 1)
                {
                    //未查询到产品

                    throw new Exception("在制品列表未查询到产品");
                }

       


                try
                {
               
                    mydbUUTDB.BeginTransaction();
                    pd.EndTime = System.DateTime.Now;
                    pd.State = EndType;//正常下线

                    pd.Result = result;

                    //string sql = SqlHandleNew.SqlCommand<Common.Product.Model.ProductInfo>(pd, "UPDATE", "P_Product", "AND [UUTestId]=" + UUTestId, out _parames, true);
                    //int id = mydbUUTDB.ExecuteNonQuery(sql, _parames);

                   res = GlobalResources.Update<Common.Product.Model.ProductInfo>(pd, "P_Product", "AND [UUTestId]=" + UUTestId, mydbUUTDB);

                   if (res)
                    {
                        //sql = SqlHandleNew.SqlCommand<Common.Product.Model.InWorkP_Info>(null, "DELETE", "P_InWork", "AND [UUTestId]=" + UUTestId, out _parames, true);
                        //id = mydbUUTDB.ExecuteNonQuery(sql, _parames);
                        res = GlobalResources.Delete("P_InWork", "AND [UUTestId]=" + UUTestId, mydbUUTDB);


                        if (res)
                        {
                            mydbUUTDB.Commit();
                            res = true;
                        }
                        else
                        {
                            mydbUUTDB.Rollback();
                        }
                    }
                    else
                    {
                        mydbUUTDB.Rollback();
                    }
                }
                catch
                {
                    mydbUUTDB.Rollback();
                }
            }
            return res;
        }



        public long ReWorkEnterLine(Common.Product.Model.ProductInfo pdif,string CurrentStation)
        {
            long id = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    int res = GlobalResources.IsExists("P_Product", "SN", pdif.SN , mydbUUTDB);
                    if (res != 1)
                    {
                        return -2;
                    }
                    res = GlobalResources.IsExists("P_InWork", "SN", pdif.SN, mydbUUTDB);
                    if (res != 0)
                    {
                        return -3;
                    }




                    pdif.Result = 1;
                    pdif.StartTime = System.DateTime.Now;
                    pdif.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
                    pdif.PrdType = 2;//1正常测试，//2代表返工，3代表点检产品。
                    id = ProductStart(pdif, mydbUUTDB, CurrentStation);





                    ////查询产品完成记录中没有该产品SN;
                    //string sql = "SELECT  count(UUTestId) FROM [P_Product] where [SN] =" + "'" + pdif.SN + "'";
                    //DataTable dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                    //if (dt == null) return -1;
                    //if ((int)dt.Rows[0][0] < 1) return -1;//返回值为false
                    //sql = "SELECT  count(UUTestId) FROM [P_InWork] where [SN] =" + "'" + pdif.SN + "'";
                    //dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                    //if (dt == null) return -1;
                    //if ((int)dt.Rows[0][0] > 0) return -1;//返回值为false

                    //pdif.Result = "ReWorking";
                    //pdif.StartTime = System.DateTime.Now;
                    //pdif.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
                    //pdif.PrdType = 2;//1正常测试，//2代表返工，3代表点检产品。
                    //id = ProductStart(pdif, mydbUUTDB);
                }
                catch
                {
                    return id;
                }
            }
            return id;
        }

        public long NormalEnterLine(Common.Product.Model.ProductInfo pdif, string CurrentStation)
        {
            long id = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {
                    int res = GlobalResources.IsExists("P_Product", "SN", pdif.SN, mydbUUTDB);
                    if (res!= 0)
                    {
                        return -2;
                    }
                    res = GlobalResources.IsExists("P_InWork", "SN", pdif.SN, mydbUUTDB);
                    if (res != 0)
                    {
                        return -3;
                    }
              


                   
                 
                    //pdif.StartTime = System.DateTime.Now;
                    //pdif.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
                    //pdif.PrdType = 1;//1正常测试，//2代表返工，3代表点检产品。
                    id = ProductStart(pdif, mydbUUTDB, CurrentStation);
                }
                catch
                {
                    return id;
                }
            }
            return id;
        }

        public long PowerOnCheckEnterLine(Common.Product.Model.ProductInfo pdif, string CurrentStation)
        {
            long id = -1;
            using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
            {
                try
                {

                    //int res = SqlHandleNew.IsExists("P_Product", "SN", "= '" + pdif.SN + "'", mydbUUTDB);
                    //if (res != 1)
                    //{
                    //    return -2;
                    //}
                    int res = GlobalResources.IsExists("P_InWork", "SN", pdif.SN, mydbUUTDB);
                    if (res == 1)
                    {
                        return -3;
                    }




                    pdif.Result = 1;
                    pdif.StartTime = System.DateTime.Now;
                    pdif.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
                    pdif.PrdType = 3;//1正常测试，//2代表返工，3代表点检产品。
                    id = ProductStart(pdif, mydbUUTDB, CurrentStation);






                    ////查询产品完成记录中没有该产品SN;
                    //string sql = "SELECT  count(UUTestId) FROM [P_Product] where [SN] =" + "'" + pdif.SN + "'";
                    //DataTable dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                    //if (dt == null) return -1;
                    //if ((int)dt.Rows[0][0] > 0) return -1;//返回值为false
                    //sql = "SELECT  count(UUTestId) FROM [P_InWork] where [SN]  =" + "'" + pdif.SN + "'";
                    //dt = mydbUUTDB.DataTableExecuteReader(sql, new List<SqlParameter>());
                    //if (dt == null) return -1;
                    //if ((int)dt.Rows[0][0] > 0) return -1;//返回值为false
                    //pdif.Result = "StartChecking";
                    //pdif.StartTime = System.DateTime.Now;
                    //pdif.State = 1;//1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
                    //pdif.PrdType = 3;//1正常测试，//2代表返工，3代表点检产品。
                    //id = ProductStart(pdif, mydbUUTDB);
                }
                catch
                {
                    return id;
                }
            }
            return id;
        }

        //public Common.Product.Model.InWorkP_Info SelectProductInfo(long uutId)
        //{
        //    Common.Product.Model.InWorkP_Info pd = null;
        //    using (MyDB_Sql mydbUUTDB = new MyDB_Sql(GlobalResources.dbserver, GlobalResources.dbname, GlobalResources.userid, GlobalResources.password))
        //    {
        //        try
        //        {
        //            pd = GlobalResources.GetInfo<Common.Product.Model.InWorkP_Info>("P_InWork", "[UUTestId]", uutId, mydbUUTDB);
                  
        //        }
        //        catch
        //        {
        //           return null;
        //        }
        //    }
        //    return pd;
        //}
      
    }
}
