using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
namespace Common.Product.Model
{

    [DataContract]
    public class Statistic_Info
    {

        private string partNr;
        /// <summary>
        /// 产品类型批号
        /// </summary>
        [DataMember]
        public string PartNr
        {
            get { return partNr; }
            set { partNr = value; }
        }




        private int passCount;
      /// <summary>
      /// 产品SN号
      /// </summary>
        [DataMember]
        public int PassCount
        {
            get { return passCount; }
            set { passCount = value; }
        }

        private int failCount;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public int FailCount
        {
            get { return failCount; }
            set { failCount = value; }
        }


        private int rollingNum;
     /// <summary>
     /// 开始时间
     /// </summary>
        [DataMember]
        public int RollingNum
        {
            get { return rollingNum; }
            set { rollingNum = value; }
        }



        private int dayRollingNum;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public int DayRollingNum
        {
            get { return dayRollingNum; }
            set { dayRollingNum = value; }
        }




        private DateTime datetime;

        /// <summary>
        /// 产品类型号
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get { return datetime; }
            set { datetime = value; }
        }
    }
}
