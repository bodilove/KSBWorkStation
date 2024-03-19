using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;


namespace Test.Common
{
    [Serializable]
    public enum CompareMode
    {
        Between = 1,
        Equal = 2,
        HexStringBetween = 3,
        CaseEqual =4,
        Contains =5,
        HexStringMatch = 6,
        NotBetween=7,
        More=8,
    }

    public enum enumMeasureType
    {
        AutoTest = 1,
        ManualTest = 2
    }

    [Serializable]
    public class SubTestStep:ICloneable
    {
        public static Project prj;
        public string Nr = null;
        public string Name = null;
        public string Description = null;
        //public string ErrorNum = null;
        public bool Enable = true;
        public bool SaveResult = false;
        public int RetryCount = 0;
        public enumMeasureType MeasureType = enumMeasureType.AutoTest;
        //public TestKind TestKind = TestKind.NormalTest;
        public int Timeout = 0;

        public List<Command> Commands = null;
        //---------------Measure------------
        public string Unit = null;
        public CompareMode CmpMode;
        public Command MeasureCmd;
        public string LLimit = null;
        public string ULimit = null;
        public string NomValue = null;
        public object Value = null;

        //---------------Error------------
        public string ErrorCode = null;
        public string ErrorDescription = null;
        //public List<Rectangle> ErrorLocation = null;

        //------------Teststatus-----------
        public TestStatus Status ;

        //-----------Test time------------
        public string StartTime = "";
        public string TestTime = "";
        public string FinishTime = "";

        public object LLimit_OBJ
        {
            get
            {
                object obj_LL;

                if (LLimit.StartsWith("[Var"))
                {
                    try
                    {
                        obj_LL = prj.Variables[LLimit.Replace("[Var/", "").Replace("]", "")].Content;
                    }
                    catch (Exception)
                    {
                        obj_LL = null;
                    }

                }
                else
                {
                    obj_LL = LLimit;
                }

                if (obj_LL == null)
                    return "LLimit_Null";
                else
                    return obj_LL;
            }
        }

        public object ULimit_OBJ
        {
            get
            {
                object obj_UL;

                if (ULimit.StartsWith("[Var"))
                {
                    try
                    {
                        obj_UL = prj.Variables[ULimit.Replace("[Var/", "").Replace("]", "")].Content;
                    }
                    catch (Exception)
                    {
                        obj_UL = null;
                    }
                }
                else
                {
                    obj_UL = ULimit;
                }

                if (obj_UL == null)
                    return "ULimit_Null";
                else
                    return obj_UL;
            }
        }

        public object NomValue_OBJ
        {
            get
            {
                object obj_Nom;

                if (NomValue.StartsWith("[Var"))
                {
                    try
                    {
                        obj_Nom = prj.Variables[NomValue.Replace("[Var/", "").Replace("]", "")].Content;
                    }
                    catch (Exception)
                    {
                        obj_Nom = null;
                    }

                }
                else
                {
                    obj_Nom = NomValue;
                }

                if (obj_Nom == null)
                    return "NomValue_Null";
                else
                    return obj_Nom;
            }
        }


        public string DesignedValueString()
        {
            string sRet = "";

            object obj_Nom = null;
            object obj_LL = null;
            object obj_UL = null;


            switch (CmpMode)
            {

                case CompareMode.Between:
                case CompareMode.NotBetween:
                case CompareMode.HexStringBetween:

                    sRet = "{" + LLimit_OBJ.ToString() + "," + ULimit_OBJ.ToString() + "}";
                    break;

                case CompareMode.Equal:
                    sRet = "= " + NomValue_OBJ.ToString();
                    break;
                case CompareMode.More:
                    sRet = "> " + NomValue_OBJ.ToString();
                    break;
                case CompareMode.CaseEqual:
                    string[] switchs = NomValue_OBJ.ToString().Split(',');
                    for (int i = 0; i < switchs.Length; i++)
                    {
                        sRet = sRet+"Cs(" + i.ToString() + ") = " + switchs[i]+" ";
                    }
                    //sRet = "= " + NomValue_OBJ.ToString();
                    break;
                case CompareMode.Contains:
                    sRet = "⊇" + NomValue_OBJ.ToString();
                    break;
                case CompareMode.HexStringMatch:

                    sRet = "Match " + NomValue_OBJ.ToString();

                    break;
            }


            return sRet;
        }


        public SubTestStep()
        {
            Nr = "";
            Name = "";
            Description = "";

            Status = TestStatus.NotTested;
            Commands = new List<Command>();
            MeasureCmd = new Command();
            Unit = "";
            CmpMode = CompareMode.Between;
            LLimit = "";
            ULimit = "";
            NomValue = "";
            ErrorCode = "";
            ErrorDescription = "";

        }

        public bool Test()
        {
            bool res = false;
            Stopwatch watch = new Stopwatch();
            try
            {

                for (int i = 0; i <= this.RetryCount; i++)
                {
                    //Perform Command list
                    foreach (Command c in Commands)
                    {
                        c.Execute();
                    }
                    //System.Threading.Thread.Sleep(1000);
                    //Perform Measure
                    watch.Reset();
                    watch.Start();
                    do
                    {
                        Value = MeasureCmd.Execute();
                        res = Evaluate();
                    } while (watch.ElapsedMilliseconds < this.Timeout && !res);
                    if (res) break;
                }
                if (res)
                {
                    Status = TestStatus.Pass;
                }
                else
                {
                    Status = TestStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                string s = "StepNr=" + this.Name + ", " + ex.Message;
                throw new Exception(s);

            }
            return res;
        }

        public void RenameParameter(string Src, string Des)
        {
            foreach (Command c in  Commands)
            {
                if (("[Var/" + c.LinkedVariable + "]") == Src)
                    c.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
                c.RenameParameter(Src, Des);
            }

            if (("[Var/" + MeasureCmd.LinkedVariable + "]") == Src)
                MeasureCmd.LinkedVariable = Des.Replace("[Var/", "").Replace("]", "");
            MeasureCmd.RenameParameter(Src, Des);

        }

        public void RenameDevice(string Src, string Des)
        {
            foreach (Command c in Commands)
            {
                c.RenameDevice(Src, Des);
            }

            MeasureCmd.RenameDevice(Src, Des);

        }

        private bool Evaluate()
        {
            double LL, UL, v;
 
            v = -9.9e37;

           
            switch (CmpMode)
            {

                case CompareMode.CaseEqual:
                     string nom1 = NomValue_OBJ.ToString();
                     string val1 = Value.ToString();
                     string[] switchs = nom1.Split(',');
                     return switchs.Contains(val1);

                case CompareMode.Between:
                    if (LLimit_OBJ == "LLimit_Null")
                        LL = -9.9e37;
                    else
                        LL = Convert.ToDouble(LLimit_OBJ);

                    if (ULimit == "ULimit_Null")
                        UL = 9.9e37;
                    else
                        UL = Convert.ToDouble(ULimit_OBJ);

                    v = Convert.ToDouble(Value);

                    return (v >= LL && v <= UL);
                case CompareMode.NotBetween:
                    if (LLimit_OBJ == "LLimit_Null")
                        LL = -9.9e37;
                    else
                        LL = Convert.ToDouble(LLimit_OBJ);

                    if (ULimit == "ULimit_Null")
                        UL = 9.9e37;
                    else
                        UL = Convert.ToDouble(ULimit_OBJ);

                    v = Convert.ToDouble(Value);

                    return (v <= LL && v >= UL);

                case CompareMode.Equal:

                    string nom = NomValue_OBJ.ToString();
                    string val = Value.ToString();
                    if (nom == val)
                        return true;
                    else
                        return (nom == val);

                case CompareMode.More:
                    double num = Convert.ToDouble(NomValue_OBJ);
                    double valu = Convert.ToDouble(Value);
                    return (valu >= num);
                case CompareMode.HexStringBetween:

                    string[] Lstr = LLimit_OBJ.ToString().Split(' ');
                    string[] Ustr = ULimit_OBJ.ToString().Split(' ');
                    string[] Vstr = Value.ToString().Split(' ');

                    bool res = true;


                    if (Lstr.Length == Ustr.Length && Vstr.Length == Lstr.Length)
                    {

                    }
                    else
                    {
                        return false;
                    }


                    for (int i = 0; i < Lstr.Length; i++)
                    {
                        byte l = Convert.ToByte(Lstr[i], 16);
                        byte u = Convert.ToByte(Ustr[i], 16);
                        byte vv = Convert.ToByte(Vstr[i], 16);
                        bool bok = (vv >= l && vv <= u);
                        res &= bok;
                        if (!res) break;
                    }

                    return res;
                case CompareMode.Contains:
                    string nom2 = NomValue_OBJ.ToString();
                    string val2 = Value.ToString();
                    if (val2.Contains(nom2))
                        return true;
                    else
                        return false;
                case CompareMode.HexStringMatch:
                    string MatchStr = NomValue_OBJ.ToString();
                    string BeMatchValue = Value.ToString();
                    bool Res = true;
                    if (MatchStr.Length != BeMatchValue.Length)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < MatchStr.Length; i++)
                        {
                            if (MatchStr[i] != 'X' && MatchStr[i] != 'x')
                            {
                                //比较
                                if (MatchStr[i] == BeMatchValue[i])
                                {
                                    Res = true;
                                }
                                else
                                {
                                    Res = false;
                                    break;
                                }
                            }
                        }
                        return Res;
                    }

            }
            return false;
        }

        public void Save(XmlNode Node)
        {
            XmlAttribute attr;
            XmlNode rNode, sNode;
            int ID = 0;

            XmlDocument doc = Node.OwnerDocument;
            Node.AppendChild(doc.CreateElement("Enable")).InnerText = Convert.ToInt32(Enable).ToString();
            Node.AppendChild(doc.CreateElement("Nr")).InnerText = Nr;
            Node.AppendChild(doc.CreateElement("Name")).InnerText = Name;
            Node.AppendChild(doc.CreateElement("Description")).InnerText = Description;
            Node.AppendChild(doc.CreateElement("RetryCount")).InnerText = RetryCount.ToString();
            Node.AppendChild(doc.CreateElement("MeasureType")).InnerText = MeasureType.ToString();
            Node.AppendChild(doc.CreateElement("Timeout")).InnerText = Timeout.ToString();
            Node.AppendChild(doc.CreateElement("SaveResult")).InnerText = Convert.ToInt32(SaveResult).ToString();

            //--------------Save Command----------
            ID = 0;
            Node.AppendChild(doc.CreateElement("Commands"));
            if (Commands != null)
            {
                foreach (Command c in Commands)
                {
                    sNode = Node["Commands"].AppendChild(doc.CreateElement("Command"));
                    attr = doc.CreateAttribute("ID");
                    attr.Value = (++ID).ToString();
                    sNode.Attributes.Append(attr);
                    c.Save(sNode);
                }
            }

            //---------------Save Measure----------
            rNode = Node.AppendChild(doc.CreateElement("Measurement"));
            rNode.AppendChild(doc.CreateElement("Unit")).InnerText = Unit;
            //rNode.AppendChild(doc.CreateElement("CompareMode")).InnerText = Enum.GetName(typeof(CompareMode), CmpMode);
            rNode.AppendChild(doc.CreateElement("CompareMode")).InnerText = ((int) CmpMode).ToString();
            sNode = rNode.AppendChild(doc.CreateElement("Command"));
            MeasureCmd.Save(sNode);
            sNode = rNode.AppendChild(doc.CreateElement("Limit"));
            sNode.AppendChild(doc.CreateElement("LLimit")).InnerText = LLimit;
            sNode.AppendChild(doc.CreateElement("ULimit")).InnerText = ULimit;
            sNode.AppendChild(doc.CreateElement("NomValue")).InnerText = NomValue;

            //---------------Save Error----------
            rNode = Node.AppendChild(doc.CreateElement("Error"));
            rNode.AppendChild(doc.CreateElement("Code")).InnerText = ErrorCode;
            rNode.AppendChild(doc.CreateElement("Description")).InnerText = ErrorDescription;
            ID = 0;

        }
        public void Load(XmlNode Node)
        {
            int cnt = 0;
            XmlNode rNode, sNode;

            Enable = Convert.ToBoolean(Convert.ToInt32(Node["Enable"].InnerText));
            Nr = Node["Nr"].InnerText;
            Name = Node["Name"].InnerText;
            Description = Node["Description"].InnerText;
            RetryCount = Convert.ToInt32(Node["RetryCount"].InnerText);
            MeasureType = (enumMeasureType)Enum.Parse(typeof(enumMeasureType), Node["MeasureType"].InnerText);
            Timeout = Convert.ToInt32(Node["Timeout"].InnerText);
            SaveResult = Convert.ToBoolean(Convert.ToInt32(Node["SaveResult"].InnerText));

            Commands.Clear();
            cnt = Node["Commands"].SelectNodes("Command").Count;
            for (int i = 1; i <= cnt; i++)
            {
                sNode = Node["Commands"].SelectSingleNode("Command[@ID='" + i.ToString() + "']");
                Command c = new Command();
                c.Load(sNode);
                Commands.Add(c);
            }

            rNode = Node["Measurement"];
            Unit = rNode["Unit"].InnerText;
            //CmpMode = (CompareMode)Enum.Parse(typeof(CompareMode), rNode["CompareMode"].InnerText);
            CmpMode = (CompareMode) Convert.ToInt32( rNode["CompareMode"].InnerText);
            MeasureCmd = new Command();
            MeasureCmd.Load(rNode["Command"]);
            LLimit = rNode["Limit"]["LLimit"].InnerText;
            ULimit = rNode["Limit"]["ULimit"].InnerText;
            NomValue = rNode["Limit"]["NomValue"].InnerText;

            //---------------Load Error----------
            rNode = Node["Error"];
            ErrorCode = rNode["Code"].InnerText;
            ErrorDescription = rNode["Description"].InnerText;

        }

        public object Clone()
        {
            SubTestStep subTS = new SubTestStep();
            subTS.Nr=this.Nr;
            subTS.Name = this.Name;
            subTS.Description=this.Description;
            subTS.Enable=this.Enable;
            subTS.RetryCount = this.RetryCount;
            subTS.MeasureType = this.MeasureType;
            subTS.Timeout = this.Timeout;
            subTS.SaveResult = this.SaveResult;
            foreach (Command c in this.Commands)
            {
                subTS.Commands.Add((Command)c.Clone());
            }
            subTS.Unit = this.Unit;
            subTS.CmpMode = this.CmpMode;
            subTS.MeasureCmd = (Command)this.MeasureCmd.Clone();
            subTS.LLimit = this.LLimit;
            subTS.ULimit = this.ULimit;
            subTS.NomValue = this.NomValue;
            subTS.Value = this.Value;

            subTS.ErrorCode = this.ErrorCode;
            subTS.ErrorDescription = this.ErrorDescription;
            subTS.Status = this.Status;

            return subTS;
        }
    }
}
