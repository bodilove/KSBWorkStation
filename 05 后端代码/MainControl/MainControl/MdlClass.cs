using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.User;
using Common.User.BLL;
using Common.User.Model;
using Common.SysConfig;
using Common.PLCData.Model;
using Common.PLCData.BLL;
using Common.Process.BLL;
using Common.Process.Model;
using Common.Product.Model;
using Common.StationProduct.BLL;
using Common.Product.BLL;
using Common.StationProduct.Model;
using System.Runtime.Serialization;
using Common.Part.BLL;
namespace MainControl
{

    public enum WorkType
    {
        Normal = 1,
        ReWork = 2,
        NULL = 3,
        ClearLine = 4,
        PowerOnCheck = 5,
        OutBuffer40 = 6,
        OutBuffer50 = 7,
    }

    public enum StationEndType
    {
        InWork = 1,
        NormalEndStation = 2,
        AbnormalEndStation = 3,
    }

    [DataContract]
    public enum EndType
    {
        [EnumMember]
        InWork = 1,
        [EnumMember]
        NormalEndline =2,
         [EnumMember]
        AbnormalEndLine =3,

      //  1,在制，2，下线，3异常下线
    }
     [DataContract]
    public enum Result
    {
             [EnumMember]
        NoResult =0,//未制作
             [EnumMember]
        Pass=2,
             [EnumMember]
        Fail =3,
    }

    public static class MdlClass
    {
   
        public static WorkType WorkType = 0;//工作模式
        public static byte PartType = 0;//PLC变种实例
        public static bool Working =false;

        public static string Oliling30Path = @"D:\picture\IV3_Greasing_OP30";
        public static string Oliling40_2Path = @"D:\picture\IV3_Greasing_OP40_2";

        public static string CamerPathOK = @"D:\picture\Temp\CAM1\OK\picture_OK.bmp";
        public static string CamerPathNG = @"D:\picture\Temp\CAM1\NG\picture_NG.bmp";
        public static Library.DK_DC mydc62 = new Library.DK_DC();
        public static Library.DK_DC mydc63 = new Library.DK_DC();
        public static Library.LT_Link_Protocol80 myLT_Link98room2 = new Library.LT_Link_Protocol80();
        public static Library.LT_Link_Protocol80 myLT_Link99room1 = new Library.LT_Link_Protocol80();
        public static Library.SccanerHoneyWell myScanner = new Library.SccanerHoneyWell();
        public static Library.NetScanner Scanner70 = new Library.NetScanner();

        public static string SysConfigPath = Application.StartupPath + @"\SysConfig";
        public static string PLCConfigPath = Application.StartupPath + @"\GNConfig";
        public static string ShowPicturePath = Application.StartupPath + @"\ShowPicture";
        public static UserInfo userInfo = new UserInfo();
        public static UserBLL userbll = new UserBLL();
       

        public static Common.SysConfig.Model.SystemConfig sysSet = new Common.SysConfig.Model.SystemConfig();

        public static Common.PLCData.BLL.PLCDataCommonBLL PlcdataBll = new Common.PLCData.BLL.PLCDataCommonBLL();

        public static List<P_Process> ProcessList = new List<P_Process>();
         public static P_ProcessBLL p_processbll = new P_ProcessBLL();
         public static P_DetailBLL p_Detailsbll = new P_DetailBLL();
         public static ProcessConfig CurrentProcessConfig = new ProcessConfig();
         public static P_BomPartBLL p_BomPartBLL = new P_BomPartBLL();
         public static PartBLL partbll = new PartBLL();
         public static TestRecord_BLL p_TestRecordbll = new TestRecord_BLL();
         public static   StationRecord_BLL stationRecordbll= new StationRecord_BLL();
         public static StationRecord_BLL stresbll = new StationRecord_BLL();
         public static InWorkPBLL inWorkBll = new InWorkPBLL();
         public static LineService lineSever = new LineService();
         public static AssembleRecord_BLL assembleRecord_bll = new AssembleRecord_BLL();
         public static P_TestItemConfigBLL p_TestItemConfigbll = new P_TestItemConfigBLL();
         public static TestRecord_BLL testRecord_bll = new TestRecord_BLL();
         public static ProductConfigInfoBLL productConfigInfoBbLL = new ProductConfigInfoBLL();
         public static ProductBLL ProductInfobll = new ProductBLL();
        
         public static Common.CreatSN.LongGanSNCreaterAndCheck sncreat = new Common.CreatSN.LongGanSNCreaterAndCheck();

         public static void AddAssembleRecord(string SationNum, string ItemName, string PartNum, int Result, string ScanCode, DateTime Scantime, long StationTestId, long UUTestId, string Description)
         {
             AssembleRecord ar = new AssembleRecord();
             ar.ItemName = ItemName;
             ar.PartNum = PartNum;
             ar.Result = Result;
             ar.ScanCode = ScanCode;
             ar.Scantime = Scantime;
             ar.StationTestId = StationTestId;
             ar.UUTestId = UUTestId;
             ar.Description = Description;
             MdlClass.lineSever.DicStationControl[SationNum].AssembleRecordlst.Add(ar);
         }


         public static void AddTestRecordlst(string SationNum, string ComPareMode, string Description, string Llimit, string Nom, string Ulimit, int Result, float SpanTime, DateTime TestTime, string TestName, string TestNum, string TestValue, long StationTestId, long UUTestId, string Unit)
         {
             TestRecord tr = new TestRecord();
             tr.ComPareMode = ComPareMode;
             tr.Description = Description;
             tr.Llimit = Llimit;
             tr.Nom = Nom;
             tr.Ulimit = Ulimit;
             tr.Result = Result;
             tr.SpanTime = SpanTime;
             tr.StationTestId = StationTestId;
             tr.TestName = TestName;
             tr.TestNum = TestNum;
             tr.TestTime = TestTime;
             tr.TestValue = TestValue;
             tr.Unit = Unit;
             tr.UUTestId = UUTestId;
             MdlClass.lineSever.DicStationControl[SationNum].TestRecordlst.Add(tr);
         }



         public static string IsCanReWork(string ScanCode, ref Common.Product.Model.ProductInfo pn)
         {
             string NOReWork = "NG,无法返工,原因:";


             List<Common.StationProduct.Model.AssembleRecord> lst = MdlClass.assembleRecord_bll.SelectAssembleRecordByScanCode(ScanCode);
             if (lst == null)
             {
                 return NOReWork + "数据库查询出错。";
             }

             if (lst.Count > 0)
             {
                 //判断正常生产的那次

                 string NGStation = "";

                 foreach (Common.StationProduct.Model.AssembleRecord smr in lst)
                 {
                     Common.Product.Model.ProductInfo pn1 = MdlClass.ProductInfobll.GetProductInfo(smr.UUTestId);
                     if (pn1.PrdType == 1)
                     {
                         pn = pn1;
                         break;
                     }
                 }
                 if (pn == null)
                 {
                     return NOReWork + "未找到该产品。UUTestId：" + lst[lst.Count - 1].UUTestId;
                 }


                 List<Common.StationProduct.Model.StationRecord> Stationreslst = MdlClass.stationRecordbll.GetStationRecordByUUTidList(pn.UUTestId);

                 foreach (Common.StationProduct.Model.StationRecord sr in Stationreslst)
                 {
                     if (sr.Result != 2)
                     {
                         NGStation = sr.StationNum;
                         break;
                     }
                 }
                 if (NGStation == "")
                 {
                     return NOReWork + "未查询到。";
                 }

                 if (pn.PartNum == MdlClass.CurrentProcessConfig.Currentprocess.ProductNum)
                 {
                     string NewPath = "";
                     switch (NGStation)
                     {
                         case "OPH30":
                             return NOReWork + "该站无法返工。";
                         case "OPH40_1":
                             return "OK,OPH30," + NewPath + ExpectedPath("OPH40_1", pn);//得到路径
                         case "OPH40_2":
                             return NOReWork + "该站无法返工。";
                         case "OPH50":
                             return "OK,OPH30," + NewPath + ExpectedPath("OPH50", pn);//得到路径//得到路径判断是否为扫码NG？
                         case "OPH60":
                             return NOReWork + "该站无法返工。";
                         case "OPH70":
                             return NOReWork + "该站无法返工。";//可以在80返工
                         case "OPH80":
                             return "OK," + NewPath + ExpectedPath("OPH80", pn);//得到路径
                         case "OPH90":
                             return "OK," + NewPath + ExpectedPath("OPH90", pn);//得到路径
                         case "":
                             return "NG,NG站输入为空。";//得到路径
                         default:
                             return "NG，未知输入";
                     }
                 }
                 else
                 {
                     return NOReWork + "返工产品:" + pn.PartNum + "与现阶段：" + MdlClass.CurrentProcessConfig.Currentprocess.ProductNum + "不符合。";
                 }
             }
             else
             {
                 return NOReWork + "未找到该条码。";
             }
         }

         public static string ExpectedPath(string NGStationName, Common.Product.Model.ProductInfo pn)
         {
             string[] Stations = pn.ExpectedPathNames.Split(',');
             string NewPath = "";
             if (Stations.Length > 0)
             {
                 int i = 0;
                 bool res = false;
                 for (i = 0; i < Stations.Length; i++)
                 {
                     if (NGStationName == Stations[i])
                     {
                         res = true;
                         break;
                     }
                 }
                 if (res)
                 {
                     for (int j = i; j < Stations.Length; j++)
                     {

                         if (j > i)
                         {
                             NewPath += "," + Stations[j];
                         }
                         else
                         {
                             NewPath = Stations[j];
                         }
                     }
                 }
                 else
                 {

                 }
             }
             else
             {

             }
             return NewPath;
         }
    }
}
