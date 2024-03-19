using System;
using System.Collections.Generic;
using System.Xml;

namespace Test.Common
{
    [Serializable]
    public enum TestStatus
    {
        Pass = 0,
        Fail = 1,
        NotTested = 2
    }

    [Serializable]
    public class TestStep : ICloneable
    {
        public bool Enable = true;
        public string Nr = null;
        public string Name = null;
        public string Description = null;
        public int RetryCount = 0;
        public List<Command> PreTest;
        public List<Command> PostTest;
        public List<SubTestStep> SubTest;
        public TestStatus status;
        //public SubTestStep FailureStep = null;


        //------------testtime------------
        public string StartTime = "";
        public string TestTime = "";

        public TestStep()
        {
            status = TestStatus.NotTested;
            PreTest = new List<Command>();
            PostTest = new List<Command>();
            SubTest = new List<SubTestStep>();
        }

        public void RenameParameter(string Src, string Des)
        {
            foreach (Command c in PreTest)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }

            foreach (Command c in PostTest)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }

            foreach (SubTestStep st in SubTest)
            {
                st.RenameParameter(Src, Des);
            }

        }

        public void RenameDevice(string Src, string Des)
        {
            foreach (Command c in PreTest)
            {
                c.RenameDevice(Src, Des);
            }

            foreach (Command c in PostTest)
            {
                c.RenameDevice(Src, Des);
            }

            foreach (SubTestStep st in SubTest)
            {
                st.RenameDevice(Src, Des);
            }
        }

        public void Save(XmlNode Node)
        {
            XmlAttribute attr;
            int ID;
            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Enable")).InnerText = Convert.ToInt32(Enable).ToString();
            Node.AppendChild(doc.CreateElement("Nr")).InnerText = Nr;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("RetryCount")).InnerText = RetryCount.ToString();
            Node.AppendChild(doc.CreateElement("Description")).InnerText = Description;

            //----------------Save Pretest---------------
            ID = 0;
            Node.AppendChild(doc.CreateElement("PreTest"));
            foreach (Command c in PreTest)
            {
                XmlNode sNode = Node["PreTest"].AppendChild(doc.CreateElement("Command"));
                attr = doc.CreateAttribute("ID");
                attr.Value = (++ID).ToString();
                sNode.Attributes.Append(attr);
                c.Save(sNode);
            }

            //----------------Save PostTest---------------
            ID = 0;
            Node.AppendChild(doc.CreateElement("PostTest"));
            foreach (Command c in PostTest)
            {
                XmlNode sNode = Node["PostTest"].AppendChild(doc.CreateElement("Command"));
                attr = doc.CreateAttribute("ID");
                attr.Value = (++ID).ToString();
                sNode.Attributes.Append(attr);
                c.Save(sNode);
            }

            //----------------Save SubTest---------------
            ID = 0;
            foreach (SubTestStep sT in SubTest)
            {
                XmlNode sNode = Node.AppendChild(doc.CreateElement("SubTest"));
                attr = doc.CreateAttribute("ID");
                attr.Value = (++ID).ToString();
                sNode.Attributes.Append(attr);
                sT.Save(sNode);
            }

        }
        public void Load(XmlNode Node)
        {
            int cnt;
            Enable = Convert.ToBoolean(Convert.ToInt32(Node["Enable"].InnerText));
            Nr = Node["Nr"].InnerText;
            Name = Node["Name"].InnerText;
            RetryCount = Convert.ToInt32(Node["RetryCount"].InnerText);
            Description = Node["Description"].InnerText;


            //----------------Load PreTest---------------
            PreTest.Clear();
            cnt = Node["PreTest"].SelectNodes("Command").Count;
            for (int i = 1; i <= cnt; i++)
            {
                XmlNode sNode = Node["PreTest"].SelectSingleNode("Command[@ID='" + i.ToString() + "']");
                Command c = new Command();
                c.Load(sNode);
                PreTest.Add(c);
            }

            //----------------Load PostTest---------------
            PostTest.Clear();
            cnt = Node["PostTest"].SelectNodes("Command").Count;
            for (int i = 1; i <= cnt; i++)
            {
                XmlNode sNode = Node["PostTest"].SelectSingleNode("Command[@ID='" + i.ToString() + "']");
                Command c = new Command();
                c.Load(sNode);
                PostTest.Add(c);
            }
            //----------------Load SubTest---------------
            SubTest.Clear();
            cnt = Node.SelectNodes("SubTest").Count;
            for (int i = 1; i <= cnt; i++)
            {
                XmlNode sNode = Node.SelectSingleNode("SubTest[@ID='" + i.ToString() + "']");
                SubTestStep sT = new SubTestStep();
                sT.Load(sNode);
                SubTest.Add(sT);
            }

        }

        public object Clone()
        {
            TestStep ts = new TestStep();
            ts.Enable = this.Enable;
            ts.Nr=this.Nr;
            ts.Name = this.Name;
            ts.RetryCount = this.RetryCount;
            ts.Description=this.Description;

            foreach (Command c in this.PreTest)
            {
                ts.PreTest.Add((Command)c.Clone());
            }

            foreach (Command c in this.PostTest)
            {
                ts.PostTest.Add((Command)c.Clone());
            }

            foreach (SubTestStep st in this.SubTest)
            {
                ts.SubTest.Add((SubTestStep)st.Clone());
            }
            return ts;
        }
    }
}
