using System;
using System.Reflection;
using System.Xml;

namespace Test.Common
{
    [Serializable]
    public class Command:ICloneable
    {
        [Serializable]
        public struct parameter
        {
            public string Type;
            public string Value;
        }
        public string DeviceName = null;
        public string MethodName = null;
        public parameter[] Parameter = null;
        public string LinkedVariable = "";
        public static Project prj;

        public bool IsBreak;

        private object oDevice;
        public MethodInfo mMethod;
        private object[] oParameters;

        enum ParProperty
        {
            Normal = 0,
            TestPin = 1,
            Variable = 2,
            TestStep = 3,
            SubTestStep = 4,
            FailureStep = 5
        }

        public Command()
        {
            DeviceName = "";
            MethodName = "";
        }

        public string ParameterName
        {
            get 
            {
                string s = "";
                if (Parameter!=null && Parameter.Length > 0)
                {
                    for (int i = 0; i < Parameter.Length; i++)
                    {
                        s += Parameter[i].Value + ",";
                    }
                    s = s.Remove(s.Length - 1);
                }
                return s;
            }
        }

        //add by ydw 2011-08-05
        public Device Device
        {
            get { return (Device)oDevice; }
            set { oDevice = value; }
        }

        public void Save(XmlNode Node)
        {
            XmlNode sNode;
            XmlAttribute attr;
            XmlDocument doc=Node.OwnerDocument;

            Node.AppendChild(doc.CreateElement("Device")).InnerText = DeviceName;
            Node.AppendChild(doc.CreateElement("Method")).InnerText = MethodName;

            if (Parameter != null)
            {
                for (int i = 1; i <= Parameter.Length; i++)
                {
                    sNode = Node.AppendChild(doc.CreateElement("Parameter"));
                    attr = doc.CreateAttribute("ID");
                    attr.Value = i.ToString();
                    sNode.Attributes.Append(attr);
                    attr = doc.CreateAttribute("Type");
                    attr.Value = Parameter[i - 1].Type;
                    sNode.Attributes.Append(attr);
                    sNode.InnerText = Parameter[i - 1].Value;
                }
            }
            Node.AppendChild(doc.CreateElement("LinkedVariable")).InnerText = LinkedVariable;

        }

        private ParProperty DeterminePar(string v)
        {
            if (v == string.Empty) return ParProperty.Normal;

            if (v.Length <= 1) return ParProperty.Normal;

            // it will be treat as a variable , if this string starts with a char "[_"
            if (v.StartsWith("[Var/") && v.EndsWith("]")) return ParProperty.Variable;

            // it will be treat as a test pin , if this string starts with a char "["
            if (v.StartsWith("[TP/") && v.EndsWith("]")) return ParProperty.TestPin;

            if (v.StartsWith("[TS/") && v.EndsWith("]")) return ParProperty.TestStep;

            if (v.StartsWith("[SubTS/") && v.EndsWith("]")) return ParProperty.SubTestStep;

            if (v.StartsWith("[FailureStep") && v.EndsWith("]")) return ParProperty.FailureStep;

            return ParProperty.Normal;

        }

        private object getFailureStep(string FailureStep)
        {
            object res = null;
            string tmp = FailureStep.Replace("[", "").Replace("]", "");
            string[] sec = tmp.Split('/');
            int len = sec.Length;
            if (len == 1)
            {
                res = prj.FailureStep;
            }
            else if(len==2)
            {
                if (prj.FailureStep == null) return "";
                switch (sec[1])
                {
                    case "Name":
                        res = prj.FailureStep.Name;
                        break;
                    case "Value":
                        res = prj.FailureStep.Value;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                throw new Exception("参数创建错误,参数名:" + FailureStep);
            }
            return res;
        }


        private object getSubTS(string SubTS)
        {
            object res = null;
            string tmp = SubTS.Replace("[", "").Replace("]", "");
            string[] sec = tmp.Split('/');
            int len = sec.Length;
            if (len < 3 || len > 4) throw new Exception("参数创建错误,参数名:" + SubTS);

            foreach (TestStep ts in prj.TestSteps)
            {
                if (ts.Name == sec[1])
                {
                    foreach (SubTestStep subts in ts.SubTest)
                    {
                        if (subts.Name == sec[2])
                        {
                            if (len == 3)
                            {
                                res = subts;
                            }
                            if (len == 4)
                            {
                                switch (sec[3])
                                {
                                    case "Result":
                                        res = subts.Status;
                                        break;
                                    case "Value":
                                        res = subts.Value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            return res;

        }

        private object getTS(string TS)
        {
            object res = null;
            //string tmp = TS.Replace("[TS/", "").Replace("]", "");
            string tmp = TS.Replace("[", "").Replace("]", "");
            string[] sec = tmp.Split('/');
            int len = sec.Length;
            if (len < 2 || len > 3) throw new Exception("参数创建错误,参数名:" + TS);

            foreach (TestStep ts in prj.TestSteps)
            {
                if (ts.Name == sec[1])
                {
                    if (len == 2)
                    {
                        res = ts;
                    }
                    if (len == 3)
                    {
                        switch (sec[2])
                        {
                            case "Result":
                                res = ts.status;
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                }
            }
            return res;

        }

        private int getTP(string TP)
        {
            string tmp = TP.Replace("[TP/", "").Replace("]", "");
            if (prj.TestPins.ContainsKey(tmp))
            {
                return prj.TestPins[tmp].Channel;
            }
            else
            {
                throw new Exception("参数创建错误,参数名:" + TP);
            }
        }

       //get Variable ,Add by ydw at 2011-09-22
        private object getVariable(string v)
        {
            object obj = null;

            string var = v.Replace("[Var/", "").Replace("]", "");

            if (prj.Variables.ContainsKey(var))
            {
                obj = prj.Variables[var].Content;
            }
            else
            {
                throw new Exception("参数创建错误,参数名:" + v);
            }
            return obj;
        }

        //Create Objects add by ydw at 2011-08-05
        protected bool CreateObject()
        {
            Type tp = null;
            string[] SubStrings = null;

            try
            {

                if (Device == null) return false;
                if (Device.Instance == null) return false;

                //get parameters
                SubStrings = MethodName.Split(new Char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (SubStrings.Length < 1)
                    throw new Exception("函数定义错误,函数名:" + MethodName);

                //Create method
                if (this.mMethod == null)
                {
                    Type[] tpDef = new Type[Parameter.Length];
                    for (int i = 0; i < tpDef.Length; i++)
                    {
                        int p1 = Parameter[i].Type.IndexOf("(");
                        int p2 = Parameter[i].Type.IndexOf(")");
                        string typ = Parameter[i].Type.Substring(p1 + 1, p2 - p1 - 1);
                        tpDef[i] = Type.GetType(typ);
                    }
                    mMethod = Device.Instance.GetType().GetMethod(SubStrings[0].Trim(), tpDef);
                }

                //Create parameters
                for (int i = 0; i < Parameter.Length; i++)
                {
                    //          if(oParameters[i] == null)
                    {
                        int p1 = Parameter[i].Type.IndexOf("(");
                        int p2 = Parameter[i].Type.IndexOf(")");
                        string typ = Parameter[i].Type.Substring(p1 + 1, p2 - p1 - 1);

                        tp = Type.GetType(typ);
                        if (tp == null) throw new Exception("参数类型错误,参数类型:" + Parameter[i].Type);

                        switch (DeterminePar(Parameter[i].Value))
                        {
                            case ParProperty.TestPin:
                                oParameters[i] = getTP(Parameter[i].Value);
                                break;

                            case ParProperty.Variable:
                                oParameters[i] = getVariable(Parameter[i].Value);
                                break;

                            case ParProperty.TestStep:
                                oParameters[i] = getTS(Parameter[i].Value);
                                break;

                            case ParProperty.SubTestStep:
                                oParameters[i] = getSubTS(Parameter[i].Value);
                                break;

                            case ParProperty.FailureStep:
                                oParameters[i] = getFailureStep(Parameter[i].Value);
                                break;

                            default:
                                oParameters[i] = System.Convert.ChangeType(Parameter[i].Value, tp);
                                break;
                        }

                        if (oParameters[i] == null)
                            throw new Exception("参数创建错误,参数名:" + Parameter[i].Value);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public void Load(XmlNode Node)
        {
            XmlNode sNode;
            int cnt;

            DeviceName = Node["Device"].InnerText;
            MethodName = Node["Method"].InnerText;
            cnt = Node.SelectNodes("Parameter").Count;
            Parameter = new parameter[cnt];

            oParameters = new object[cnt]; //add by ydw 2011-08-05
            for (int i = 1; i <= cnt; i++)
            {
                sNode = Node.SelectSingleNode("Parameter[@ID='" + i.ToString() + "']");
                Parameter[i - 1].Type = sNode.Attributes["Type"].Value;
                Parameter[i - 1].Value = sNode.InnerText;
            }
            if (Node["LinkedVariable"] != null)
            {
                LinkedVariable = Node["LinkedVariable"].InnerText;
            }
        }
        public object Execute()
        {
            object obj = null;
            try
            {
                CreateObject();
                obj = mMethod.Invoke(Device.Instance, oParameters); //modify by ydw 2011-08-06
                if (LinkedVariable != "")
                {
                    prj.Variables[LinkedVariable].Content = obj;
                }
            }
            catch (Exception e)
            {

                string s = ", Command=" + this.DeviceName + "." + this.MethodName +  ParameterName;

                if (e.InnerException != null)
                {
                    s += ", Info=" + e.InnerException.Message;
                }
                else
                {
                    s += ", Info=" + e.Message;
                }
                throw new Exception(s);
            }
            return obj;
        }

        public void RenameDevice(string src, string des)
        {
            if (DeviceName == src) DeviceName = des;
        }

        public void RenameParameter(string src, string des)
        {
            if (Parameter == null) return;
            for (int i = 0; i < Parameter.Length; i++)
            {
                if (Parameter[i].Value == src) Parameter[i].Value = des;
            }
        }

        public object Clone()
        {
            Command c = new Command();
            c.DeviceName = this.DeviceName;
            c.MethodName = this.MethodName;
            c.oDevice = this.oDevice; //add by ydw 2011-0806
            c.LinkedVariable = this.LinkedVariable;

            if (this.Parameter != null)
            {
                c.Parameter = new parameter[this.Parameter.Length];
                for (int i = 0; i < this.Parameter.Length; i++)
                {
                    c.Parameter[i] = this.Parameter[i];
                }
            }

            c.CreateObject();
            return c;
        }
        
    }
}
