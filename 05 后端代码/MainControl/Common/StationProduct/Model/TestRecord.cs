using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace Common.StationProduct.Model
{

  //  SELECT TOP 1000 [ID]
  //    ,[UUTestId]
  //    ,[StationTestId]
  //    ,[TestName]
  //    ,[StepName]
  //    ,[ComPareMode]
  //    ,[Ulimit]
  //    ,[Llimit]
  //    ,[Nom]
  //    ,[TestValue]
  //    ,[Unit]
  //    ,[Result]
  //    ,[Description]
  //    ,[SpanTime]
  //    ,[TestTime]
  //FROM [LGDB].[dbo].[P_TestRes]

       [DataContract]
   public  class TestRecord
   {
       private Int64 id;
       /// <summary>
       /// 产品类型批号
       /// </summary>
       [DataMember]
       public long ID
       {
           get { return id; }
           set { id = value; }
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




       private string testName;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string TestName
       {
           get { return testName; }
           set { testName = value; }
       }



       private string testNum;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string TestNum
       {
           get { return testNum; }
           set { testNum = value; }
       }

       private string comPareMode;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string ComPareMode
       {
           get { return comPareMode; }
           set { comPareMode = value; }
       }

       private string ulimit;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string Ulimit
       {
           get { return ulimit; }
           set { ulimit = value; }
       }

       private string llimit;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string Llimit
       {
           get { return llimit; }
           set { llimit = value; }
       }


       private string nom;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string Nom
       {
           get { return nom; }
           set { nom = value; }
       }


       private string testValue;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string TestValue
       {
           get { return testValue; }
           set { testValue = value; }
       }


       private string unit;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string Unit
       {
           get { return unit; }
           set { unit = value; }
       }


       private int result;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public int Result
       {
           get { return result; }
           set { result = value; }
       }

       private string description;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public string Description
       {
           get { return description; }
           set { description = value; }
       }

       private float spanTime;
       /// <summary>
       /// 产品SN号
       /// </summary>
       [DataMember]
       public float SpanTime
       {
           get { return spanTime; }
           set { spanTime = value; }
       }



       private DateTime testTime;
       /// <summary>
       /// 开始时间
       /// </summary>
       [DataMember]
       public DateTime TestTime
       {
           get { return testTime; }
           set { testTime = value; }
       }
    }
}
