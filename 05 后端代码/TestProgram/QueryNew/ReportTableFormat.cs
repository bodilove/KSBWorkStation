using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Test.Query
{
    public class ReportTableFormat
    {

        public DataTable Tests = null;
        public DataTable SubTestStep = null;

        #region 各表初始化

        public void InitTests()
        {
            Tests = new DataTable();
            Tests.Columns.Add("UUTestId");
            Tests.Columns.Add("StartTime");
            Tests.Columns.Add("SN");
            Tests.Columns.Add("EndTime");
            Tests.Columns.Add("Result");
            Tests.Columns.Add("PrdType");
            Tests.Columns.Add("PartNum");
            Tests.Columns.Add("StationID");
            Tests.Columns.Add("UserID");



        }

        public void InitSubTestStep()
        {
            SubTestStep = new DataTable();
            SubTestStep.Columns.Add("UUTestId");
            SubTestStep.Columns.Add("SubTestId");
            SubTestStep.Columns.Add("SubStepName");
            SubTestStep.Columns.Add("SlaveStationNoName");
            SubTestStep.Columns.Add("ComPareMode");
            SubTestStep.Columns.Add("TestTime");
            SubTestStep.Columns.Add("ExceptValue");
            SubTestStep.Columns.Add("Value");
            SubTestStep.Columns.Add("Unit");
            SubTestStep.Columns.Add("Result");
            SubTestStep.Columns.Add("Description");
            SubTestStep.Columns.Add("LLimit");
        }

        #endregion

        #region 插入数据

        public  void InsertTests(DataTable MasterTable)
        {
            Tests = MasterTable;
        }

        public  void InsertSubTestStep(DataTable PTable)
        {
            foreach (DataRow dr in PTable.Rows)
            {
                string ExpectValue = "";
     
                switch (dr["CompareMode"].ToString())
                {

                    case "Between":
                    case "NotBetween":
                    case "HexStringBetween":

                        ExpectValue = "{" + dr["LLimit"].ToString() + "," + dr["ULimit"].ToString() + "}";
                        break;

                    case "Equal":

                        ExpectValue = "= " + dr["Nom"].ToString();
                        break;
                    case "More":

                        ExpectValue = "> " + dr["Nom"].ToString();
                        break;

                }
                SubTestStep.Rows.Add(new object[] { dr["ID"], dr["SubStepName"], dr["ComPareMode"], ExpectValue, dr["Value"],dr["Result"], dr["TestTime"], dr["Description"] });
            }
        }

        #endregion

    }
}
