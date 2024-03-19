using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace Common.StationProduct.Model
{
    [DataContract]
    public class AssembleRecord
    {

//****** Script for SelectTopNRows command from SSMS  ******/
//SELECT TOP 1000 [Id]
//      ,[UUTestId]
//      ,[StationTestId]
//      ,[ScanCode]
//      ,[Scantime]
//      ,[Result]
//      ,[PartNum]
//      ,[ItemName]
//      ,[Description]
//  FROM [LGDB].[dbo].[P_AssembleRes]

        private Int64 id;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public Int64 Id
        {
            get { return id; }
            set { id = value; }
        }

        private Int64 uUTestId;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public Int64 UUTestId
        {
            get { return uUTestId; }
            set { uUTestId = value; }
        }

        private Int64 stationTestId;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public Int64 StationTestId
        {
            get { return stationTestId; }
            set { stationTestId = value; }
        }



        private string scanCode;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public string ScanCode
        {
            get { return scanCode; }
            set { scanCode = value; }
        }

        private DateTime scantime;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public DateTime Scantime
        {
            get { return scantime; }
            set { scantime = value; }
        }


        private string partNum;
        /// <summary>
        /// 零件的批号
        /// </summary>
        [DataMember]
        public string PartNum
        {
            get { return partNum; }
            set { partNum = value; }
        }
        

        private string itemName;
        /// <summary>
        /// 零件的批号
        /// </summary>
        [DataMember]
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        private string description;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        private int result;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public int Result
        {
            get { return result; }
            set { result = value; }
        }


        [DataMember]
        public int StartIndex { get; set; }

        [DataMember]
        public int PartNumLength { get; set; }
    }
}
