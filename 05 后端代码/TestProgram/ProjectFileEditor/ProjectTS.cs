using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test.ProjectFileEditor
{
    /// <summary>
    /// 添加项目 项目名称，工位，路径
    /// </summary>
    [Serializable]
    public  class ProjectTS
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
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("FilePath")]
        public string FilePath { get; set; }

    }
}
