using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Test.Common
{
    public class Project : ICloneable
    {
        public string Title = null;
        public string File = null;
        //private string tmpFile = Application.StartupPath + "\\temp.prj";

        XmlDocument Doc = null;

        public Image mImage = null;
        //public int Index = 0;

        public string Version = "";
        public string PartNumber = null;
        public string Description = null;
        public bool IsMutiTest = false;
        public bool IsAnalyse = false;
        public bool IsStartTest = false;
        public string InitFileName = "";
        public bool IsErrorBreak = false;
        public string LotNumber = null;
        public Dictionary<string, Device> Devices = null;
        public Dictionary<string, TestPin> TestPins = null;
        public Dictionary<string, Variable> Variables = null;
        public List<Command> TestInits = null;
        public List<Command> TestExits = null;
        public List<Command> PreTest = null;
        public List<Command> PostTest = null;
        public List<Command> SucTest = null;
        public List<Command> FailTest = null;
        public List<TestStep> TestSteps = null;

        public SubTestStep FailureStep = null;

        //2015_03_10洪伟刚修改
      //  public TestThreadConfig MyTestThreadConfig = null;
        public List<TestThreadConfig> MyTestThreadConfigList = null;

        //for test log
        public DateTime m_StartTime;
        public DateTime m_EndTime;

        public Project()
        {
            Doc = new XmlDocument();
            Devices = new Dictionary<string, Device>();
            TestPins = new Dictionary<string, TestPin>();
            Variables = new Dictionary<string, Variable>();
            TestInits = new List<Command>();
            TestExits = new List<Command>();
            PreTest = new List<Command>();
            PostTest = new List<Command>();
            SucTest = new List<Command>();
            FailTest = new List<Command>();
            TestSteps = new List<TestStep>();
            MyTestThreadConfigList = new List<TestThreadConfig>();
            Command.prj = this;
            SubTestStep.prj = this;
        }

        //private bool CompareTo(Project prj)
        //{

        //    if (prj.Description != this.Description) return false;
        //    if (prj.Version != this.Version) return false;
        //    if (prj.LotNumber != this.LotNumber) return false;
        //    if (prj.PCBNumber != this.PCBNumber) return false;

        //    MemoryStream stream1 = new MemoryStream();
        //    prj.mImage.Save(stream1, ImageFormat.Jpeg);
        //    byte[] bytImage1 = stream1.ToArray();

        //    MemoryStream stream2 = new MemoryStream();
        //    this.mImage.Save(stream2, ImageFormat.Jpeg);
        //    byte[] bytImage2 = stream2.ToArray();

        //    if (bytImage1.Length != bytImage2.Length) return false;
        //    for (long i = 0; i < bytImage1.Length; i++)
        //    {
        //        if (bytImage1[i] != bytImage2[i]) return false;
        //    }

        //    if (this.Devices.Count != prj.Devices.Count) return false;
        //    foreach (KeyValuePair<string, Device> p in this.Devices)
        //    {
        //        if (!prj.Devices.ContainsKey(p.Key)) return false;
        //        if(prj.Devices[p.Key].
        //    }
        //    for (long i = 0; i < Devices.Count; i++)
        //    {
        //        this.Devices.Keys[i] = prj.Devices.Keys[i];
        //    }

            

        //    //public Dictionary<string, Device> Devices = null;
        //    //public Dictionary<string, TestPin> TestPins = null;
        //    //public Dictionary<string, Variable> Variables = null;
        //    //public List<Command> TestInits = null;
        //    //public List<Command> TestExits = null;
        //    //public List<Command> PreTest = null;
        //    //public List<Command> PostTest = null;
        //    //public List<Command> SucTest = null;
        //    //public List<Command> FailTest = null;
        //    //public List<TestStep> TestSteps = null;

        //    //public SubTestStep FailureStep = null;
        //    return true;

        //}

        public bool CheckChange()
        {
            //bool res = false;
            if (this.File == null) return true;

            Project tmpPrj = new Project();
            tmpPrj.File = this.File;
            tmpPrj.Load();

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            tmpPrj.Doc.Save(stream1);
            XmlDocument doc = new XmlDocument();
            this.SaveDoc(doc);
            doc.Save(stream2);

            if(stream1.Length!=stream2.Length) return true;

            for (long i = 0; i < stream1.Length; i++)
            {
                if (stream1.ReadByte() != stream2.ReadByte())
                {
                    return true;
                }
            }

            return false;
            //return this.CompareTo(tmpPrj);

            //this.Save(tmpFile);

            //FileStream f1 = new FileStream(tmpFile, FileMode.Open);
            //FileStream f2 = new FileStream(File, FileMode.Open);
            //if (f1.Length != f2.Length)
            //{
            //    res = true;
            //}
            //else
            //{
            //    for (int i = 0; i < f1.Length; i++)
            //    {
            //        int a = f1.ReadByte();
            //        int b = f2.ReadByte();
            //        if (f1.ReadByte() != f2.ReadByte())
            //        {
            //            res = true;
            //            break;
            //        }
            //    }
            //}
            //f1.Close();
            //f2.Close();
            //System.IO.File.Delete(tmpFile);
            //return res;
        }

        public void InitTest()
        {
            try
            {
                foreach (Command c in TestInits)
                {
                    c.Execute();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ;
        }

        public void ExitTest()
        {
            foreach (Command c in TestExits)
            {
                try
                {
                    c.Execute();
                }
                catch (Exception)
                {
                }
            }
        }

        public void RenameDevice(string Src, string Des)
        {
            foreach (Command c in TestInits)
            {
                c.RenameDevice(Src, Des);
            }

            foreach (Command c in TestExits)
            {
                c.RenameDevice(Src, Des);
            }

            foreach (Command c in PreTest)
            {
                c.RenameDevice(Src, Des);
            }

            foreach (Command c in PostTest)
            {
                c.RenameDevice(Src, Des);
            }
            foreach (Command c in SucTest)
            {
                c.RenameDevice(Src, Des);
            }
            foreach (Command c in FailTest)
            {
                c.RenameDevice(Src, Des);
            }
            foreach (TestStep ts in TestSteps)
            {
                ts.RenameDevice(Src, Des);
            }
        }

        public void RenameParameter(string Src, string Des)
        {
            foreach (Command c in TestInits)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }

            foreach (Command c in TestExits)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }

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

            foreach (Command c in SucTest)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }
            foreach (Command c in FailTest)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }
            foreach (TestStep ts in TestSteps)
            {
                ts.RenameParameter(Src, Des);
            }
        }

        public void SaveDoc(XmlDocument Doc)
        {
            int ID = 0;
            XmlNode RootNode, Node, sNode;

            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "utf-8", ""));
            RootNode = Doc.AppendChild(Doc.CreateElement("Project"));

            //XmlAttribute attr = Doc.CreateAttribute("Index");
            //attr.Value = Index.ToString();
            //RootNode.Attributes.Append(attr);

            XmlAttribute attr = Doc.CreateAttribute("Version");
            attr.Value = Version;
            RootNode.Attributes.Append(attr);

            //RootNode.AppendChild(Doc.CreateElement("LotNumber")).InnerText = LotNumber;
            RootNode.AppendChild(Doc.CreateElement("PCBNumber")).InnerText = PartNumber;
            RootNode.AppendChild(Doc.CreateElement("Description")).InnerText = Description;
            RootNode.AppendChild(Doc.CreateElement("IsMutiTest")).InnerText = IsMutiTest.ToString();
            RootNode.AppendChild(Doc.CreateElement("IsAnalyse")).InnerText = IsAnalyse.ToString();
            RootNode.AppendChild(Doc.CreateElement("IsStartTest")).InnerText = IsStartTest.ToString();
            RootNode.AppendChild(Doc.CreateElement("InitFileName")).InnerText = InitFileName.ToString();
            RootNode.AppendChild(Doc.CreateElement("IsErrorBreak")).InnerText = IsErrorBreak.ToString();
            //Save Image
            //Jacky 2011.08.03
            if (mImage != null)
            {
                MemoryStream stream = new MemoryStream();
                mImage.Save(stream, ImageFormat.Jpeg);
                byte[] bytImage = stream.ToArray();
                RootNode.AppendChild(Doc.CreateElement("Image")).InnerText = Convert.ToBase64String(bytImage);
            }
            //-----------------Save Test Pins-------------------
            Node = RootNode.AppendChild(Doc.CreateElement("TestPins"));
            if (TestPins != null)
            {
                ID = 0;
                foreach (KeyValuePair<string, TestPin> p in TestPins)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Pin"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    p.Value.Save(sNode);
                }
            }
            //-----------------Save TestThreadConfig------------
            //
            Node = RootNode.AppendChild(Doc.CreateElement("TestThreadConfigList"));

            if (MyTestThreadConfigList != null)
            {
                foreach (TestThreadConfig tsc in MyTestThreadConfigList)
                {
                    XmlNode xNode = Node.AppendChild(Doc.CreateElement("TestThreadConfig"));
                    tsc.Save(xNode);
                }
            }


            //-----------------Save Variables-------------------
            Node = RootNode.AppendChild(Doc.CreateElement("Variables"));
            if (Variables != null)
            {
                foreach (KeyValuePair<string, Variable> p in Variables)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Variable"));
                    p.Value.Save(sNode);
                }
            }

            //-----------------Save Devices-------------------
            Node = RootNode.AppendChild(Doc.CreateElement("Devices"));
            if (Devices != null)
            {
                ID = 0;
                foreach (KeyValuePair<string, Device> p in Devices)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Device"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    p.Value.Save(sNode);
                }
            }

            //-----------------Save TestInits-------------------
            Node = RootNode.AppendChild(Doc.CreateElement("TestInit"));
            if (TestInits != null)
            {
                ID = 0;
                foreach (Command c in TestInits)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }

            //-----------------Save TestExits-------------------
            Node = RootNode.AppendChild(Doc.CreateElement("TestExit"));
            if (TestExits != null)
            {
                ID = 0;
                foreach (Command c in TestExits)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }

            //-----------------Save TestSequences-------------------
            //Node:TestSequences
            RootNode.AppendChild(Doc.CreateElement("TestSequences"));

            //-----------------Save PreTest-------------------
            //Node:PreTest
            Node = RootNode["TestSequences"].AppendChild(Doc.CreateElement("PreTest"));
            if (PreTest != null)
            {
                ID = 0;
                foreach (Command c in PreTest)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }

            //-----------------Save PostTest-------------------
            //Node:PostTest
            Node = RootNode["TestSequences"].AppendChild(Doc.CreateElement("PostTest"));
            if (PostTest != null)
            {
                ID = 0;
                foreach (Command c in PostTest)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }

            //-----------------Save SucTest-------------------
            //Node:SucTest
            Node = RootNode["TestSequences"].AppendChild(Doc.CreateElement("SucTest"));
            if (SucTest != null)
            {
                ID = 0;
                foreach (Command c in SucTest)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }
            //-----------------Save FailTest-------------------
            //Node:FailTest
            Node = RootNode["TestSequences"].AppendChild(Doc.CreateElement("FailTest"));
            if (FailTest != null)
            {
                ID = 0;
                foreach (Command c in FailTest)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Command"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    c.Save(sNode);
                }
            }

            //-----------------Save Test-------------------
            //Node:TestSequences
            Node = RootNode["TestSequences"];
            if (TestSteps != null)
            {
                ID = 0;
                foreach (TestStep t in TestSteps)
                {
                    sNode = Node.AppendChild(Doc.CreateElement("Test"));
                    attr = sNode.Attributes.Append(Doc.CreateAttribute("ID"));
                    attr.Value = (++ID).ToString();
                    t.Save(sNode);
                }
            }
        }

        public void Save()
        {
            Doc = new XmlDocument();
            SaveDoc(Doc);
            Doc.Save(this.File);
        }

        public void Save(string Filename)
        {
            Doc = new XmlDocument();
            SaveDoc(Doc);
            Doc.Save(Filename);
        }

        private void LoadDoc(XmlDocument Doc)
        {
            int cnt = 0;
            XmlNode RootNode, Node, sNode;

            RootNode = Doc.DocumentElement;
            //Index = Convert.ToInt32(RootNode.Attributes["Index"].Value);
            Version = RootNode.Attributes["Version"].Value;
            Description = RootNode["Description"].InnerText;
            //LotNumber = RootNode["LotNumber"].InnerText;
            PartNumber = RootNode["PCBNumber"].InnerText;
            InitFileName = RootNode["InitFileName"].InnerText;
            if (RootNode["InitFileName"] != null)
            {
                IsErrorBreak = bool.Parse(RootNode["IsErrorBreak"].InnerText);
            }
            
            if (RootNode["IsMutiTest"] != null)
            {
                IsMutiTest = bool.Parse(RootNode["IsMutiTest"].InnerText);
            }
            if (RootNode["IsAnalyse"] != null)
            {
                IsAnalyse = bool.Parse(RootNode["IsAnalyse"].InnerText);
            }
            if (RootNode["IsStartTest"] != null)
            {
                IsStartTest = bool.Parse(RootNode["IsStartTest"].InnerText);
            }
            //Load Image
            //Jacky 2011.08.03
            if (RootNode["Image"] != null)
            {
                byte[] bytImage = Convert.FromBase64String(RootNode["Image"].InnerText);
                MemoryStream stream = new MemoryStream(bytImage);
                mImage = new Bitmap(stream, true);
            }
            //-----------------Load Test Pins-------------------
            Node = RootNode["TestPins"];
            if (Node != null)
            {
                TestPins.Clear();
                cnt = Node.SelectNodes("Pin").Count;
                for (int i = 1; i <= cnt; i++)
                {

                  
                        sNode = Node.SelectSingleNode("Pin[@ID='" + i.ToString() + "']");
                
                        Console.WriteLine();
                    
                    TestPin tp = new TestPin();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("Pin")[i-1];
                        sNode.Attributes["ID"].InnerText = (i ).ToString();

                        tp.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {
                        //if (sNode.Name == Node.SelectNodes("Pin")[i].Name)
                        //{
                            tp.Load(sNode);
                        //}
                        //else
                        //{
                        //    sNode = Node.SelectNodes("Pin")[i];
                        //    sNode.Attributes["ID"].InnerText = (i).ToString();

                        //    tp.Load(sNode);
                        //    tp.Channel = i;
                        //}
                    }
               
                    TestPins.Add(tp.Name, tp);
                }
            }
            //-----------------Load TestThreadConfig------------

            Node = RootNode["TestThreadConfigList"];
            if (Node != null)
            {
                MyTestThreadConfigList.Clear();
                foreach (XmlNode xn in Node.ChildNodes)
                {
                    TestThreadConfig tsc = new TestThreadConfig();
                    if (xn.Name == "TestThreadConfig")
                    {
                        tsc.Load(xn);
                        MyTestThreadConfigList.Add(tsc);
                    }
                }
            }

            //-----------------Load Variables-------------------
            Node = RootNode["Variables"];
            if (Node != null)
            {
                Variables.Clear();
                foreach (XmlNode node in Node.SelectNodes("Variable"))
                {
                    Variable v = new Variable();
                    v.Load(node);
                    Variables.Add(v.Name, v);
                }
            }

            //-----------------Load Devices-------------------
            Node = RootNode["Devices"];
            if (Node != null)
            {
                Devices.Clear();
                cnt = Node.SelectNodes("Device").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    sNode = Node.SelectSingleNode("Device[@ID='" + i.ToString() + "']");
                    Device d = new Device();
                    d.Load(sNode);
                    Devices.Add(d.Name, d);
                }
            }

            //-----------------Load TestInit-------------------
            Node = RootNode["TestInit"];
            if (Node != null)
            {
                TestInits.Clear();
                cnt = Node.SelectNodes("Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    sNode = Node.SelectSingleNode("Command[@ID='" + i.ToString() + "']");
                      Command c = new Command();
                      if (sNode == null)
                      {
                          //if (i == cnt)
                          //{
                          //    break;
                          //}
                          sNode = Node.SelectNodes("Command")[i-1];
                          sNode.Attributes["ID"].InnerText = (i).ToString();

                          c.Load(sNode);
                          //tp.Channel = i;
                      }
                      else
                      {

                          c.Load(sNode);
                      }
                    TestInits.Add(c);
                }
            }

            //-----------------Load TestExit-------------------
            Node = RootNode["TestExit"];
            if (Node != null)
            {
                TestExits.Clear();
                cnt = Node.SelectNodes("Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    sNode = Node.SelectSingleNode("Command[@ID='" + i.ToString() + "']");
                    Command c = new Command();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("Command")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        c.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        c.Load(sNode);
                    }
                    TestExits.Add(c);
                }
            }

            //-----------------Load TestSequences-------------------
            Node = RootNode["TestSequences"];
            if (Node != null)
            {
                //-----------------Load PreTest-------------------
                PreTest.Clear();
                cnt = Node.SelectNodes("PreTest/Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    //sNode = Node.SelectSingleNode("PreTest/Command[@ID='" + i.ToString() + "']");
                    //Command c = new Command();
                    //c.Load(sNode);
                    //PreTest.Add(c);
                    sNode = Node.SelectSingleNode("PreTest/Command[@ID='" + i.ToString() + "']");
                    Command c = new Command();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("PreTest/Command")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        c.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        c.Load(sNode);
                    }
                    PreTest.Add(c);
                }

                //-----------------Load PostTest-------------------
                PostTest.Clear();
                cnt = Node.SelectNodes("PostTest/Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                //    sNode = Node.SelectSingleNode("PostTest/Command[@ID='" + i.ToString() + "']");
                //    Command c = new Command();
                //    c.Load(sNode);
                //    c.Device = Devices[c.DeviceName];

                    sNode = Node.SelectSingleNode("PostTest/Command[@ID='" + i.ToString() + "']");
                    Command c = new Command();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("PostTest/Command")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        c.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        c.Load(sNode);
                    }
                    PostTest.Add(c);
                }

                //-----------------Load SucTest-------------------
                SucTest.Clear();
                cnt = Node.SelectNodes("SucTest/Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    //sNode = Node.SelectSingleNode("SucTest/Command[@ID='" + i.ToString() + "']");
                    //Command c = new Command();
                    //c.Load(sNode);

                    sNode = Node.SelectSingleNode("SucTest/Command[@ID='" + i.ToString() + "']");
                    Command c = new Command();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("SucTest/Command")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        c.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        c.Load(sNode);
                    }
                    SucTest.Add(c);

                }
                //-----------------Load FailTest-------------------
                FailTest.Clear();
                cnt = Node.SelectNodes("FailTest/Command").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    //sNode = Node.SelectSingleNode("FailTest/Command[@ID='" + i.ToString() + "']");
                    //Command c = new Command();
                    //c.Load(sNode);
                    sNode = Node.SelectSingleNode("FailTest/Command[@ID='" + i.ToString() + "']");
                    Command c = new Command();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("FailTest/Command")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        c.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        c.Load(sNode);
                    }
                    FailTest.Add(c);
                }

                //-----------------Load Test-------------------
                TestSteps.Clear();
                cnt = Node.SelectNodes("Test").Count;
                for (int i = 1; i <= cnt; i++)
                {
                    //sNode = Node.SelectSingleNode("Test[@ID='" + i.ToString() + "']");
                    //TestStep t = new TestStep();
                    //t.Load(sNode);

                    sNode = Node.SelectSingleNode("Test[@ID='" + i.ToString() + "']");
                    TestStep t = new TestStep();
                    if (sNode == null)
                    {
                        //if (i == cnt)
                        //{
                        //    break;
                        //}
                        sNode = Node.SelectNodes("Test")[i - 1];
                        sNode.Attributes["ID"].InnerText = (i).ToString();

                        t.Load(sNode);
                        //tp.Channel = i;
                    }
                    else
                    {

                        t.Load(sNode);
                    }
                    TestSteps.Add(t);
                }
            }
        }
     
        public void Load()
        {
            try
            {
                Doc = new XmlDocument();
                Doc.Load(File);
                this.LoadDoc(Doc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
