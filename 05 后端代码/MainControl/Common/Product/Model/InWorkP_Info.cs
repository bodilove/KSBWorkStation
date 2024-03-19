using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.ComponentModel;
namespace Common.Product.Model
{
    //pd.PrdType = 1;//1正常测试，//2代表返工，3代表点检产品。
    //产品生产模式
   [DataContract]
    public enum ProductType
    {

        [Description("生产")]
        NomWork = 1,
        [Description("返工")]
        ReWork = 2,
        [Description("点检")]
        EquipmentCheck = 3,
    }
    
   [DataContract]
   public enum ProductResult
   {
       [Description("无记录")]
       NoDo = 0,
       [Description("合格")]
       Pass = 1,
       [Description("不合格")]
       Fail = 2,
   }
   // 1代表在制品，2代表正常下线，3代表下线(但是工艺没走完）。


   [DataContract]
   public enum ProductState
   {
       [Description("在制")]
       Working = 0,
       [Description("正常下线")]
       NomEnd = 1,
       [Description("异常下线")]
       AbnomEnd = 2,
   }

    [DataContract]
    public class InWorkP_Info
    {
        private Int64 uutestId;
        /// <summary>
        /// 产品的唯一识别号
        /// </summary>
        [DataMember]
        public Int64 UUTestId
        {
            get { return uutestId; }
            set { uutestId = value; }
        }


        private string userName;
        /// <summary>
        /// 产品SN号
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
      /// 产品SN号
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

        private int currentResult;
        /// <summary>
        /// 产品测试结果
        /// </summary>
        [DataMember]
        public int CurrentResult
        {
            get { return currentResult; }
            set { currentResult = value; }
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

        /// <summary>
        /// 当前测试站名称
        /// </summary>
        private string currentStation;
        [DataMember]
        public string CurrentStation
        {
            get { return currentStation; }
            set { currentStation = value; }
        }

        private Int64 currentSationTestID;
        /// <summary>
        /// 产品的唯一识别号
        /// </summary>
        [DataMember]
        public Int64 CurrentSationTestID
        {
            get { return currentSationTestID; }
            set { currentSationTestID = value; }
        }

    }
}
