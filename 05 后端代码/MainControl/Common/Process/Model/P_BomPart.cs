using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Process.Model
{

//        Id
//ProcessId
//DetailsId
//PartNo
//CurrentCount
//ForeWarning
//Warning
//Enable
//OrderNum
      [DataContract]
      public class P_BomPart
      {
          private int id;
          [DataMember]
          public int Id
          {
              get { return id; }
              set { id = value; }
          }


          private int processId;
          [DataMember]
          public int ProcessId
          {
              get { return processId; }
              set { processId = value; }
          }


          private int detailsId;
          [DataMember]
          public int DetailsId
          {
              get { return detailsId; }
              set { detailsId = value; }
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

          private string partNo;
          [DataMember]
          public string PartNo
          {
              get { return partNo; }
              set { partNo = value; }
          }




          private int currentCount;
          [DataMember]
          public int CurrentCount
          {
              get { return currentCount; }
              set { currentCount = value; }
          }

//Enable
          private int foreWarning;
          [DataMember]
          public int ForeWarning
          {
              get { return foreWarning; }
              set { foreWarning = value; }
          }



          private int warning;
          [DataMember]
          public int Warning
          {
              get { return warning; }
              set { warning = value; }
          }


          private int enable;
          [DataMember]
          public int Enable
          {
              get { return enable; }
              set { enable = value; }
          }

          private int orderNum;
          [DataMember]
          public int OrderNum
          {
              get { return orderNum; }
              set { orderNum = value; }
          }
      }

    }

