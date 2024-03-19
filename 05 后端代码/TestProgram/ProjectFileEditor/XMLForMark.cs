using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace ProjectFileEditor
{
    [Serializable]
    public class XMLForMark
    {
        public struct member
        {
            public string memberName;
            public string mark;
            public List<string> paramName;
            public List<string> paramDescrition;
            public string returns;
        }
        public string _xmlName;
        public Dictionary<string, member> Members = null;
        public XMLForMark( string xmlName)
        {
           
            _xmlName = xmlName;
            Members = new Dictionary<string, member>();
        }
        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <returns></returns>
        public string Init()
        {
            int index = 0;
            if (_xmlName == "") return "";
            string filename = Application.StartupPath + @"\" + _xmlName.Split('.')[0] + ".xml";
            if (!File.Exists(filename)) return "";
            try
            {
                member m = new member();
                XmlNodeList members;               
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNode RootNode, Node, sNode;
                RootNode = doc.DocumentElement;
                Node = RootNode["members"];
                members = Node.ChildNodes;
                for (int i = 0; i < members.Count; i++)
                {
                    index = i;
                    string mName = members[i].Attributes["name"].Value;
                    m.memberName = mName;
                    m.paramDescrition = new List<string>();
                    m.paramName = new List<string>();
                    sNode = Node.SelectSingleNode("member[@name='" + mName + "']");
                    try
                    {
                        foreach (XmlElement x in sNode.ChildNodes)
                        {
                            switch (x.Name)
                            {
                                case "summary":
                                    m.mark = x.InnerText.Replace("\r\n", "").Trim();
                                    break;
                                case "param":
                                    m.paramName.Add(x.Attributes["name"].Value);
                                    m.paramDescrition.Add(x.InnerText.Trim());
                                    break;
                                case "returns":
                                    m.returns = x.InnerText.Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    Members.Add(mName, m);
                }
            }
            catch (Exception ex)
            {
                return index.ToString();
            }
            return "";
        }
        /// <summary>
        /// 显示注释
        /// </summary>
        /// <param name="cmbText">方法名称</param>
        /// <returns>注释+参数名+参数描述</returns>
        public string ShowMark(string cmbText)
        {            
            try
            {               
                string MethodName = "";
                string[] paras = null;
                string mark = "";
                string key = "";
                paras = cmbText.Split(' ')[1].Replace("[", "").Replace("]", "").Trim().Split(',');
                MethodName = cmbText.Split(' ')[0] + "(";
                for (int i = 0; i < paras.Length; i++)
                {
                    if (cmbText.Split(' ')[1].Replace("[", "").Replace("]", "").Trim() != "")
                        MethodName += "System." + paras[i] + ",";
                }
                MethodName = MethodName.Remove(MethodName.Length - 1, 1);
                if (cmbText.Split(' ')[1].Replace("[", "").Replace("]", "").Trim() != "")
                    MethodName += ")";

                key = "M:" + _xmlName + "." + MethodName;
                if (!Members.ContainsKey(key)) return "";
                member value = Members[key];                
                mark = value.mark + Environment.NewLine;
                for (int i = 0; i < value.paramName.Count; i++)
                {
                    mark += "参数"+
                        value.paramName[i] + ":" + value.paramDescrition[i] + Environment.NewLine;
                }
                mark += value.returns;
                return mark;
            }
            catch (Exception)
            {
                return "";
            }
            return ""; ;
            
        }
    }
}
