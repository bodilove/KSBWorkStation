using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;

namespace Test.StartUp
{
    public class TSHandler
    {
        XmlDocument doc = new XmlDocument();
        XmlNode RootNode;
        string File = null;

        public TSHandler(string Filename)
        {
            File = Filename;
            doc.Load(Filename);
            RootNode = doc.DocumentElement;
        }

        public int TSCount
        {
            get
            {
                return RootNode.SelectNodes("TS").Count;
            }
        }

        public string GetTS_Name(int Index)
        {
            XmlNode Node = RootNode.SelectSingleNode("TS[@ID='" + Index.ToString() + "']");
            return Node["Name"].InnerText;
        }

        public string GetTS_FilePath(int Index)
        {
            XmlNode Node = RootNode.SelectSingleNode("TS[@ID='" + Index.ToString() + "']");
            return Node["FilePath"].InnerText;
        }

        public Dictionary<int, string> GetTS_FilePathLst()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            XmlNodeList xdl = RootNode.SelectNodes("TS");
            foreach (XmlNode xn in xdl)
            {
                dic.Add(int.Parse(xn.Attributes["ID"].InnerText), xn["FilePath"].InnerText);
            }
            return dic;
        }

        public void AddTS(string Name, string FilePath)
        {
            int count = this.TSCount;
            XmlNode Node = RootNode.AppendChild(doc.CreateElement("TS"));
            XmlAttribute attr = Node.Attributes.Append(doc.CreateAttribute("ID"));
            attr.Value = (count+1).ToString();
            Node.AppendChild(doc.CreateElement("Name")).InnerText=Name;
            Node.AppendChild(doc.CreateElement("FilePath")).InnerText = FilePath;
            doc.Save(File);
        }

        public void RemoveTS(int Index)
        {
            XmlNode Node = RootNode.SelectSingleNode("TS[@ID='" + Index.ToString() + "']");
            XmlNode Parent=Node.ParentNode;
            int count=this.TSCount;

            if (Node != null)
            {
                Parent.RemoveChild(Node);
                for (int i = Index + 1; i <= count; i++)
                {
                    Node = RootNode.SelectSingleNode("TS[@ID='" + i.ToString() + "']");
                    Node.Attributes["ID"].Value = (i - 1).ToString();
                }
            }
            doc.Save(File);
        }

        public void MoveUp(int CurrentIndex)
        {
            if (CurrentIndex == 1) return;

            string tName = null;
            string tPath = null;

            XmlNode Node1 = RootNode.SelectSingleNode("TS[@ID='" + CurrentIndex.ToString() + "']");
            XmlNode Node2 = RootNode.SelectSingleNode("TS[@ID='" + (CurrentIndex-1).ToString() + "']");
            tName = Node2["Name"].InnerText;
            tPath = Node2["FilePath"].InnerText;

            Node2["Name"].InnerText = Node1["Name"].InnerText;
            Node2["FilePath"].InnerText = Node1["FilePath"].InnerText;

            Node1["Name"].InnerText = tName;
            Node1["FilePath"].InnerText = tPath;

            doc.Save(File);
        }

        public void MoveDown(int CurrentIndex)
        {
            if (CurrentIndex == this.TSCount) return;

            string tName = null;
            string tPath = null;

            XmlNode Node1 = RootNode.SelectSingleNode("TS[@ID='" + CurrentIndex.ToString() + "']");
            XmlNode Node2 = RootNode.SelectSingleNode("TS[@ID='" + (CurrentIndex + 1).ToString() + "']");
            tName = Node2["Name"].InnerText;
            tPath = Node2["FilePath"].InnerText;

            Node2["Name"].InnerText = Node1["Name"].InnerText;
            Node2["FilePath"].InnerText = Node1["FilePath"].InnerText;

            Node1["Name"].InnerText = tName;
            Node1["FilePath"].InnerText = tPath;

            doc.Save(File);
        }
    }
}
