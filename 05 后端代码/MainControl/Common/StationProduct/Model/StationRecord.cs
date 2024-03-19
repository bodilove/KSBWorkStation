using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace Common.StationProduct.Model
{


    public class StationRecord
    {

        //    SELECT TOP 1000 [StationTestId]
        //    ,[UUTestId]
        //    ,[StationName]
        //    ,[StartTime]
        //    ,[EndTime]
        //    ,[PrdType]
        //    ,[Result]
        //FROM [LGDB].[dbo].[P_SationProduct]



        private long stationTestId;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public long StationTestId
        {
            get { return stationTestId; }
            set { stationTestId = value; }
        }

        private long uUTestId;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public long UUTestId
        {
            get { return uUTestId; }
            set { uUTestId = value; }
        }

        private string stationName;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public string StationName
        {
            get { return stationName; }
            set { stationName = value; }
        }

        private string stationNum;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public string StationNum
        {
            get { return stationNum; }
            set { stationNum = value; }
        }

        private DateTime startTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTime endTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private int STprdType;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public int STPrdType
        {
            get { return STprdType; }
            set { STprdType = value; }
        }

        private int result;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public int Result
        {
            get { return result; }
            set { result = value; }
        }
    }

}

