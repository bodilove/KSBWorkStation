using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common.StationProduct.Model;
using Common.Process.Model;


namespace MainControl
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract(SessionMode = SessionMode.Required,
    CallbackContract = typeof(ITestDataServerCallback))]
    public interface ITestDataServer
    {

        [OperationContract]
        void DoWork();
        [OperationContract(IsOneWay = false)]
        string DoWork2(string s);


        [OperationContract(IsOneWay = false)]
        P_Process ReturnCurrentprocess(ref string Erro);

        [OperationContract(IsOneWay = false)]
        long EnterLine_Normal(ref string newSN);

        [OperationContract(IsOneWay = false)]
        long EnterLine_ReWork(string SN, ref string ResultAndPath);

        [OperationContract(IsOneWay = false)]
        string LeaveLine(long uuid, EndType endType, Result res);
        [OperationContract(IsOneWay = false)]
        string EnterStation(string SationNum, long uuid, Result res);
        [OperationContract(IsOneWay = false)]
        string leaveStation(string SationNum, long uuid, Result res);
        [OperationContract(IsOneWay = false)]
        Common.Product.Model.InWorkP_Info GetInfoByUUTid(long uutid);
        [OperationContract(IsOneWay = false)]
        Common.Product.Model.InWorkP_Info GetInfoBySN(string SN);

        [OperationContract(IsOneWay = false)]
        string UpProductSN(string ProductSN, long uutid);

        [OperationContract(IsOneWay = false)]
        string SaveAssembleRecord(List<AssembleRecord> AssembleRecordlst, string SationNum);
        // string AddAssembleRecord(List<AssembleRecord> AssembleRecordlst);
        //   string AddAssembleRecord(string SationNum, string ItemName, string PartNum, int Result, string ScanCode, DateTime Scantime, long StationTestId, long UUTestId, string Description);

        [OperationContract(IsOneWay = false)]
        string SaveTestRecordlst(List<TestRecord> TestRecordlst, string SationNum);
        // string AddTestRecordlst(string SationNum, string ComPareMode, string Description, string Llimit, string Nom, string Ulimit, int Result, float SpanTime, DateTime TestTime, string TestName, string TestNum, string TestValue, long StationTestId, long UUTestId, string Unit);
    }

    public interface ITestDataServerCallback
    {

        [OperationContract(IsOneWay = false)]
        string GetUUT();
        [OperationContract(IsOneWay = true)]
        void FormClientood(string msg);

        [OperationContract(IsOneWay = false)]
        string GetUUT2(string s);



        //[OperationContract(IsOneWay = true)]
        //void SendSlaveStation(string msg);
        //[OperationContract(IsOneWay = false)]
        //UUT GetUUT();
        //[OperationContract(IsOneWay = true)]
        //void PutUUT(UUT uut);

        //[OperationContract(IsOneWay = false)]
        //StationStateForRobot GetStationState();

        //[OperationContract(IsOneWay = false)]
        //bool GetStationIsReady();

        //[OperationContract(IsOneWay = true)]
        //void StartPowerOncheck(int funcIndex);//0，1 start, 2,break.


    }

}
