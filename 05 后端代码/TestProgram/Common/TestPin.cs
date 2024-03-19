using System;
using System.Drawing;
using System.Xml;

namespace Test.Common
{
    public class TestPin
    {
        public string Name;
        public int Channel;
        public string Description;

        public TestPin()
        {
            Name = "";
            Channel = 0;
            Description = "";
        }

        public void Save(XmlNode Node)
        {
            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("Channel")).InnerText = Channel.ToString();
            Node.AppendChild(doc.CreateElement("Description")).InnerText = Description;
            XmlNode sNode = Node.AppendChild(doc.CreateElement("Location"));
        }

        public void Load(XmlNode Node)
        {
            try
            {
                Name = Node["Name"].InnerText;
                Channel = Convert.ToInt32(Node["Channel"].InnerText);
                Description = Node["Description"].InnerText;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
         
        }
    }
}
