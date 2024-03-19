using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Process.Model
{

    ///****** Script for SelectTopNRows command from SSMS  ******/
    //SELECT TOP 1000 [Id]
    //      ,[ProcessId]
    //      ,[DetailsId]
    //      ,[TestName]
    //      ,[TestNum]
    //      ,[ComPareMode]
    //      ,[Ulimit]
    //      ,[Llimit]
    //      ,[Nom]
    //      ,[Unit]
    //      ,[Description]
    //  FROM [LGDB].[dbo].[P_TestItemConfig]

    [DataContract]
    public class P_TestItemConfig
    {
        private int id;
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        private int processId;
        [DataMember]
        public int ProcessId
        {
            get { return processId; }
            set { processId = value; }
        }


        private int detailsId;
        [DataMember]
        public int DetailsId
        {
            get { return detailsId; }
            set { detailsId = value; }
        }



        private string testName;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        private string testNum;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string TestNum
        {
            get { return testNum; }
            set { testNum = value; }
        }


        private string comPareMode;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string ComPareMode
        {
            get { return comPareMode; }
            set { comPareMode = value; }
        }

        private string ulimit;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string Ulimit
        {
            get { return ulimit; }
            set { ulimit = value; }
        }

        private string llimit;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string Llimit
        {
            get { return llimit; }
            set { llimit = value; }
        }


        private string nom;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }


        //private string testValue;
        ///// <summary>
        ///// 产品SN号
        ///// </summary>
        //[DataMember]
        //public string TestValue
        //{
        //    get { return testValue; }
        //    set { testValue = value; }
        //}


        private string unit;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }




        private string description;
        /// <summary>
        /// 产品SN号
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        private int orderNum;
        [DataMember]
        public int OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }
    }

    [Serializable]
    public enum CompareMode
    {
        Between = 1,
        Equal = 2,
        HexStringBetween = 3,
        CaseEqual = 4,
        Contains = 5,
        HexStringMatch = 6,
        NotBetween = 7,
        More = 8,
        Show = 9,
        NotEqual = 10
    }

}

