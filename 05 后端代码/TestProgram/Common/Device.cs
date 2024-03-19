using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Test.Common
{
    public class Device
    {
        public string Name = null;
        public string Class = null;
        object obj = null;

        public object Instance
        {
            set { obj = value; }
            get{return obj;}
        }

        //public MethodInfo[] MethodList()
        //{
        //    Type typ =obj.GetType();
        //    MethodInfo[] method = null;
        //    MethodInfo[] totalm = typ.GetMethods();
        //    List<MethodInfo> lm = new List<MethodInfo>();
        //    foreach (MethodInfo m in totalm)
        //    {
        //        if (m.IsPublic && !m.IsVirtual && m.Name != "GetType")
        //        {
        //            lm.Add(m);
        //        }
        //    }
        //    method = lm.ToArray();

        //    return method;

        //}

        public void Save(XmlNode Node)
        {
            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("Class")).InnerText = Class;
        }

        public void Load(XmlNode Node)
        {
            Name = Node["Name"].InnerText;
            Class = Node["Class"].InnerText;
        }

        public bool CreateObject(Assembly asm)
        {
            ////get current directory
            //String strFilePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            //strFilePath += "\\Library\\" + this.Library;

            //if (!File.Exists(strFilePath)) return false;

            //create a instance for this device
            //Assembly asm = Assembly.LoadFrom(strFilePath);
            if (null == asm) return false;

            obj = asm.CreateInstance(this.Class);
            return obj != null;
        }
    }
}
