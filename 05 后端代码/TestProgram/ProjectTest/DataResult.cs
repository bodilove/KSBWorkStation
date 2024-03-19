using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ProjectTest
{
    public class SubStep
    {
        string m_strName = "";
        //string m_Step = "";
        string m_Time = "";
        string m_strStartTime="";
        string m_Status = "";
        string m_Value = "";
        string m_NomValue = "";
        string m_Unit = "";
        string m_ErrorCode = "";

        public string ErrorCode
        {
            get { return m_ErrorCode; }
            set { m_ErrorCode = value; }
        }

        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        public string NomValue
        {
            get { return m_NomValue; }
            set { m_NomValue = value; }
        }

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public string Unit
        {
            get { return m_Unit; }
            set { m_Unit = value; }
        }

        //public string Step
        //{
        //    get { return m_Step; }
        //    set { m_Step = value; }
        //}
        public string Name 
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public string Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        public string StartTime
        {
            get { return m_strStartTime; }
            set { m_strStartTime = value; }
        }


    }

    public class Step
    {
        string m_strName = "";
        string m_TestTime = "";
        string m_StartTime = "";
        string m_TestStatus = "";

        static public string NS
        {
            get { return "1"; }
        }

        List<SubStep> m_Subs = new List<SubStep>();
        public string Name
        {
            get
            {
                return m_strName;
             }
            set
            {
                m_strName = value;     
            }
            
        }

        public string TestTime
        {
            get
            {
                return m_TestTime;
            }
            set
            {
                m_TestTime = value;
            }
        }
        public string TestStatus
        {
            get { return m_TestStatus; }
            set { m_TestStatus = value; }
        }
        public string StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }

        public List<SubStep> GetSubSteps() 
        {
            return m_Subs;
        }

    }

    /// <summary>
    /// 数据储存结构
    /// </summary>
    public class DataResult
    {
        List<Step> m_Steps = new List<Step>();
        private string m_strPcbNo = "";
        private string m_strOpName = "";
        private string m_strPCName = "";
        private string m_strProgID = "";

        public string PCBNumber 
        {
            get
            {
                return m_strPcbNo;
            }   
            set
            {
                m_strPcbNo = value;
            }
        }

        public string OpName
        {
            get { return m_strOpName; }
            set {m_strOpName = value;}
        }
        public string PCName
        {
            get{return m_strPCName;}
            set { m_strPCName = value; }
        }
        public string ProgID
        {
            get { return m_strProgID; }
            set { m_strProgID = value; }
        }

        public void Clear()
        {
            m_strPcbNo = "";
            m_Steps.Clear();
        }


        //////////////////////////////////////////////////////////////////////////
        //TestSteps
        //private IList<SubResult>
        public List<Step> GetSteps
        {
            get{return m_Steps;}
        }

        //TestSteps
        //private IList<SubResult>
        public List<Step> GetFailedSteps
        {
            get{
                List<Step> err = new List<Step>();
                foreach(Step ts in m_Steps)
                {
                    if(ts.TestStatus == Common.TestStatus.Fail.ToString())
                    {
                        err.Add(ts);
                    }
                }
                return err;
            }  
        }
    }
}
