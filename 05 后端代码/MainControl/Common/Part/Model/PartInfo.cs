using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Part.BLL
{
    [DataContract]
    public class PartInfo
    {
        private int partId;
        [DataMember]
        public int PartId
        {
            get { return partId; }
            set { partId = value; }
        }
        private string partNo;
        [DataMember]
        public string PartNo
        {
            get { return partNo; }
            set { partNo = value; }
        }
        private string partName;
        [DataMember]
        public string PartName
        {
            get { return partName; }
            set { partName = value; }
        }
        private int minCount;
        [DataMember]
        public int MinCount
        {
            get { return minCount; }
            set { minCount = value; }
        }
        private string unit;
        [DataMember]
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        private int warningNumber;
        [DataMember]
        public int WarningNumber
        {
            get { return warningNumber; }
            set { warningNumber = value; }
        }


        private int isPoint;//是否提示数量
        [DataMember]
        public int IsPoint
        {
            get { return isPoint; }
            set { isPoint = value; }
        }




        //零件类型//1零件，2半成品
        private int partType;
        [DataMember]
        public int PartType
        {
            get { return partType; }
            set { partType = value; }
        }
        //追溯方式
        private int traceMode;
        [DataMember]
        public int TraceMode
        {
            get { return traceMode; }
            set { traceMode = value; }
        }


        [DataMember]
        public int StartIndex { get; set; }

        [DataMember]
        public int PartNumLength { get; set; }

    }
}
