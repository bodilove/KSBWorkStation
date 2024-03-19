using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace MainControl
{
  
// 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
    /// <remarks/>
    [Serializable]
    [XmlRoot("MyTest")]
    public partial class MyTest
    {

        /// <remarks/>
        [XmlElement("TestSequences")]
        public List<TestSequences> TestSequences { get; set; }
    }

    
    public partial class TestSequences
    {
        /// <summary>
        /// 工位序列号
        /// </summary>
        [XmlAttribute("StationNum")]
        public string StationNum { get; set; }

        [XmlElement("TS", IsNullable = true)]
        public List<TS> TS { get; set; }
        
    }

    [Serializable]
    public partial class TS
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        [XmlElement("StationNum")]
        public string StationNum { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [XmlElement("Name")]
        public string Name { get ; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("FilePath")]
        public string FilePath { get; set; }
       
       
    }


}
