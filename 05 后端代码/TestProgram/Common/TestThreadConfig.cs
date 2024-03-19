using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Test.Common
{
    public class TestThreadConfig
    {

        public string Name = "";
        public int ID = 0;
        public List<string> TestStepNameList = null;


        public TestThreadConfig()
        {
            Name = "";
            ID = 0;
            TestStepNameList = new List<string>();
        }

        public void Save(XmlNode Node)
        {
            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("ID")).InnerText = ID.ToString();
            XmlNode TestStepsNode = Node.AppendChild(doc.CreateElement("TestSteps"));
            foreach (string s in TestStepNameList)
            {
                TestStepsNode.AppendChild(doc.CreateElement("TestStep")).InnerText = s;
            }
        }

        public void Load(XmlNode Node)
        {
            TestStepNameList = new List<string>();
            Name = Node["Name"].InnerText;
            ID = int.Parse(Node["ID"].InnerText);
            XmlNode TestStepsNode = Node["TestSteps"];
            foreach (XmlNode xn in TestStepsNode.ChildNodes)
            {
                XmlElement xe = (XmlElement)xn;
                TestStepNameList.Add(xe.InnerText);
            }
        }
    }
}
