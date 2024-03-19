using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
namespace Common.Product.Model
{

    [DataContract]
    public class ProductInfo
    {
        private Int64 uutestId;
        /// <summary>
        /// 产品的唯一识别码
        /// </summary>
        [DataMember]
        public Int64 UUTestId
        {
            get { return uutestId; }
            set { uutestId = value; }
        }

        private string userid;
        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }


        private string userName;
        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string Productsn;
        /// <summary>
        /// SN号
        /// </summary>
        [DataMember]
        public string ProductSN
        {
            get { return Productsn; }
            set { Productsn = value; }
        }


        private string sn;
        /// <summary>
        /// SN号
        /// </summary>
        [DataMember]
        public string SN
        {
            get { return sn; }
            set { sn = value; }
        }


        private DateTime starttime;
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime
        {
            get { return starttime; }
            set { starttime = value; }
        }

        private DateTime endTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }


        private int prdType;
        /// <summary>
        /// 产品的类型//1正常生产，//2代表返工，3代表点检产品。
        /// </summary>
        [DataMember]
        public int PrdType
        {
            get { return prdType; }
            set { prdType = value; }
        }

        private string partNum;

        /// <summary>
        /// 产品类型号
        /// </summary>
        [DataMember]
        public string PartNum
        {
            get { return partNum; }
            set { partNum = value; }
        }

       
        private int result;
        /// <summary>
        /// 当前的测试结果
        /// </summary>
        [DataMember]
        public int Result
        {
            get { return result; }
            set { result = value; }
        }



        private int state;
        /// <summary>
        /// 产品状态 //1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。
        /// </summary>
        [DataMember]
        public int State
        {
            get { return state; }
            set { state = value; }
        }



        private Int64 expectedPath;
        /// <summary>
        /// 产品的生产路径
        /// </summary>
        [DataMember]
        public Int64 ExpectedPath
        {
            get
            {
                return expectedPath;
            }
            set { expectedPath = value; }
        }

        private string expectedPathNames;
        /// <summary>
        /// 产品的生产路径
        /// </summary>
        [DataMember]
        public string ExpectedPathNames
        {
            get
            {
                return expectedPathNames;
            }
            set { expectedPathNames = value; }
        }

        /// <summary>
        /// 产品实际过站的路径
        /// </summary>
        private Int64 actualPath;
        [DataMember]
        public Int64 ActualPath
        {
            get
            {
                return actualPath;
            }
            set { actualPath = value; }
        }

        /// <summary>
        /// 产品实际过站的结果
        /// </summary>
        private Int64 actualResult;
        [DataMember]
        public Int64 ActualResult
        {
            get
            {
                return actualResult;
            }
            set { actualResult = value; }
        }
    }
}
