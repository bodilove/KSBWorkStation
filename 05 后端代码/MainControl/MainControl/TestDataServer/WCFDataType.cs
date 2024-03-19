using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MainControl
{
    [DataContract]
    public class WCFAssembleRecord
    {

        //string SationNum, string ItemName, string PartNum, int Result, string ScanCode, DateTime Scantime, long StationTestId, long UUTestId, string Description
        [DataMember]
        public string SationNum;
        [DataMember]
        public string ItemName;
        [DataMember]
        public string PartNum;
        [DataMember]
        public int Result;
        [DataMember]
        public string ScanCode;
        [DataMember]
        public DateTime Scantime;
        [DataMember]
        public long StationTestId;
        [DataMember]
        public long UUTestId;
        [DataMember]
        public string Description;

    }



    [DataContract]
    public class WCFTestRecord
    {
        //   string SationNum, string ComPareMode, string Description, string Llimit, string Nom, string Ulimit, int Result, float SpanTime, DateTime TestTime, string TestName, string TestNum, string TestValue, long StationTestId, long UUTestId, string Unit

        [DataMember]
        string SationNum;

        [DataMember]
        string ComPareMode;

        [DataMember]
        string Description;

        [DataMember]
        string Llimit;

        [DataMember]
        string Nom;

        [DataMember]
        string Ulimit;

        [DataMember]
        int Result;

        [DataMember]
        float SpanTime;

        [DataMember]
        DateTime TestTime;

        [DataMember]
        string TestName;

        [DataMember]
        string TestNum;

        [DataMember]
        string TestValue;

        [DataMember]
        long StationTestId;

        [DataMember]
        long UUTestId;

        [DataMember]
        string Unit;
    }
}

