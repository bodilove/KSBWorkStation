using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Product.Model
{
    [DataContract]
    public class ProductConfigInfo
    {
        //  [ProductId]
        //,[ProjectName]
        //,[CusotmerName]
        //,[CustomPartNum]
        //,[ProductNum]
        //,[ProductName] 



        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string CusotmerName { get; set; }

        [DataMember]
        public string SAPPartNum { get; set; }

        [DataMember]
        public string CustomPartNum { get; set; }

        [DataMember]
        public string ProductNum { get; set; }

        [DataMember]
        public string ProductName { get; set; }

    }
}
