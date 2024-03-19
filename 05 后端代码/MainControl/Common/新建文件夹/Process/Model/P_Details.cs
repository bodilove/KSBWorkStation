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
    public class P_Detail
    {
        private int id;
       [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }


       private int prosseId;
       [DataMember]
       public int ProsseId
       {
           get { return prosseId; }
           set { prosseId = value; }
       }
    


        private string stationNum;
        [DataMember]
        public string StationNum
        {
            get { return stationNum; }
            set { stationNum = value; }
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
