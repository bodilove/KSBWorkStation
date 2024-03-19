using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Test.Common
{
    public class Variable
    {
        public string Name;
        public string Type;
        public string DefaultValue;
        public object Content;
        public string Description;

        public void Save(XmlNode Node)
        {
            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("Type")).InnerText = Type;
            Node.AppendChild(doc.CreateElement("DefaultValue")).InnerText = DefaultValue;
            Node.AppendChild(doc.CreateElement("Description")).InnerText = Description;
        }

        public void Load(XmlNode Node)
        {
            Name = Node["Name"].InnerText;
            Type = Node["Type"].InnerText;
            DefaultValue = Node["DefaultValue"].InnerText;
            Description = Node["Description"].InnerText;
        }    
    }
}
