using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Process.Model
{

//    Id
//ProductNum
//ProductName
//LineId
//LineName

    [DataContract]
    public class P_Process
    {
        private int id;
       [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }


       private string name;
       [DataMember]
       public string Name
       {
           get { return name; }
           set { name = value; }
       }

       private string productNum;
       [DataMember]
       public string ProductNum
       {
           get { return productNum; }
           set { productNum = value; }
       }
        [DataMember]
       public string SAPNum { get; set; }

       private string productName;
       [DataMember]
       public string ProductName
       {
           get { return productName; }
           set { productName = value; }
       }

        private int lineId;
        [DataMember]
        public int LineId
        {
            get { return lineId; }
            set { lineId = value; }
        }
        //LineId
        private string lineName;
        [DataMember]
        public string LineName
        {
            get { return lineName; }
            set { lineName = value; }
        }

        private int plcpnum;
        [DataMember]
        public int PLCPNum
        {
            get { return plcpnum; }
            set { plcpnum = value; }
        }
    }
}
