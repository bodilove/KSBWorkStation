using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Test.Common;
using System.IO;
using System.Threading;
using Test.ProjectTest;
namespace Test.Query
{
    public partial class frmQueryNew : Form
    {
        DataTable CurrentTable = null;
        DataTable MasterTable = null;
        DataTable MasterTableForReport = null;
        public string UserName = "未知";
        TestDataSqlManger tm = new TestDataSqlManger();

        bool PartNrChecked = true;

        int CurrentAllLines = 0;

        //int nudDisPlayNo = 10;

        int PagesCount = 0;

        int CurrentPageNo = 1;

        string TableName = "SingleSteps";

        string PartNr = "00000";

     
        Project Prj = null;

        // List<string> Command = new List<string>();



        public void RunTest(string ProjectFile)
        {
            Prj = new Project();
            Prj.File = ProjectFile;
            Prj.Load();
        }


        Dictionary<string, string> Command = null;

        string CommandPart(string colName, string R1, string R2)
        {
            string CommandPart = " AND " + colName + " BETWEEN " + R1 + "AND " + R2;
            return CommandPart;
        }

        string CommandPart(string colName, string value)
        {
            string CommandPart = " AND " + colName + "= " + "'" + value + "'";
            return CommandPart;
        }

        string QCommand()
        {
            List<string> BUFSTR = new List<string>();
            string cmd = "select * from SingleSteps where PartNr = '" + PartNr + "' and ( ";
            //---更新 BaseCommand
            int n = (int)nudDisPlayNo.Value * (this.CurrentPageNo - 1) + 1;
            int m = n + (int)nudDisPlayNo.Value;
            for (int i = n; i < m; i++)
            {
                if (i <= MasterTable.Rows.Count)
                {
                    cmd = cmd + " id = " + MasterTable.Rows[i - 1][0].ToString() + " or ";
                }
                else
                {
                    break;
                }
            }
            //cmd = cmd.Remove(cmd.Length - 4) + " order by Convert(int,ID) asc";
            cmd = cmd.Remove(cmd.Length - 4) + " )" + " order by Convert(int,StepIndex) asc";
            return cmd;
        }

        string BaseCCommandALL()
        {
            // string BUFSTR = "SELECT COUNT(*) AS Expr1 FROM " + TableName;
            string BUFSTR = "SELECT Convert(int,ID) as ID FROM " + TableName;
            return BUFSTR;
        }

        string UPCommand()
        {
            string Cm = "";
            Command = new Dictionary<string, string>();
            //---更新TestKind
            if (chbTestKind.Checked == true)
            {
                if (!this.Command.ContainsKey("TestKind"))
                {
                    this.Command.Add("TestKind", this.CommandPart("TestKind", this.cmbTestKind.Text));
                }
            }
            else
            {
                if (this.Command.ContainsKey("TestKind"))
                {
                    this.Command.Remove("TestKind");
                }
            }

            //---更新PartNr
            if (PartNrChecked == true)
                if (true)
                {
                    if (!this.Command.ContainsKey("PartNr"))
                    {
                        //this.Command.Add("PartNr", this.CommandPart("PartNr", this.txtPartNrB.Text));

                        this.Command.Add("PartNr", this.CommandPart("PartNr", PartNr));
                    }
                }
                else
                {
                    if (this.Command.ContainsKey("PartNr"))
                    {
                        this.Command.Remove("PartNr");
                    }
                }
            //---更新SN
            if (chbSN.Checked == true)
            {
                if (!this.Command.ContainsKey("SN"))
                {
                    string tx = this.txtSNB.Text.ToUpper();
                    if (tx[0] == '*')
                    {
                        tx = tx.Remove(0, 1);
                    }

                    if (tx[tx.Length - 1] == '*')
                    {
                        tx = tx.Remove(tx.Length - 1, 1);
                    }
                    this.Command.Add("SN", this.CommandPart("SN", tx.Trim()));
                }
            }
            else
            {
                if (this.Command.ContainsKey("SN"))
                {
                    this.Command.Remove("SN");
                }
            }
            //---更新TestTime
            //string BeginDate = "'" + this.txtyearB.Text + "/" + this.txtMonthB.Text + "/" + this.txtDayB.Text + " " + this.txtHourB.Text + ":" + this.txtMB.Text + ":" + this.txtSB.Text + "'";
            //string StopDate = "'" + this.txtyearS.Text + "/" + this.txtMonthS.Text + "/" + this.txtDayS.Text + " " + this.txtHourS.Text + ":" + this.txtMS.Text + ":" + this.txtSS.Text + "'";
            string BeginDate = "'" + dtpStart.Value.Year.ToString() + "/" + dtpStart.Value.Month.ToString() + "/" + dtpStart.Value.Day.ToString() + " " + "10" + ":" + "40" + ":" + "00" + "'";
            string StopDate = "'" + dtpEND.Value.Year.ToString() + "/" + dtpEND.Value.Month.ToString() + "/" + dtpEND.Value.Day.ToString() + " " + "23" + ":" + "59" + ":" + "59" + "'";
            if (chbTestTime.Checked == true)
            {
                if (!this.Command.ContainsKey("TestTime"))
                {
                    this.Command.Add("TestTime", this.CommandPart("TestTime", BeginDate, StopDate));
                }
            }
            else
            {
                if (this.Command.ContainsKey("TestTime"))
                {
                    this.Command.Remove("TestTime");
                }
            }
            //---更新Result
            //string Result = "";

            //if (rbtnFail.Checked == true)
            //{
            //    Result = "Fail";
            //}
            //else if (rbtnPass.Checked == true)
            //{
            //    Result = "PASS";
            //}
            //if (!this.Command.ContainsKey("Result"))
            //{
            //    this.Command.Add("Result", this.CommandPart("Result", Result));
            //}
            //else
            //{
            //    if (this.Command.ContainsKey("Result"))
            //    {
            //        this.Command.Remove("Result");
            //    }
            //}
            //if (rbtnALL.Checked == true)
            //{
            //    this.Command.Remove("Result");
            //}
            foreach (KeyValuePair<string, string> d in Command)
            {
                Cm = Cm + d.Value;
            }
            return Cm;

        }

        void Qurey()
        {
            try
            {
                if (this.MasterTable == null || this.MasterTable.Rows.Count == 0)
                {
                    this.CurrentTable = new DataTable();
                }
                else
                {
                    this.CurrentTable = tm.GetTable(QCommand());
                }
                if (this.CurrentTable == null)
                {
                    //  CurrentPageNo = 1;
                    throw new Exception("翻页设回第一页");
                }
                RefreshDataGrid1();
            }
            catch
            {
                CurrentPageNo = 1;
                if (this.MasterTable == null || this.MasterTable.Rows.Count == 0)
                {
                    this.CurrentTable = new DataTable();
                }
                else
                {
                    this.CurrentTable = tm.GetTable(QCommand());
                }
                //if (this.CurrentTable == null)
                //{
                //    throw new Exception("翻页设回第一页");
                //}
                RefreshDataGrid1();

            }
        }

        void GetMasterTable()
        {
            try
            {




                string Cmmd = "";
                if (PartNrChecked || !this.rbtnALL.Checked || this.chbSN.Checked || this.chbTestTime.Checked || chbTestKind.Checked == true)
                {
                    Cmmd = BaseCCommandALL() + " where " + UPCommand().Remove(0, 4) + " group by ID" + " ORDER BY ID";
                }
                else
                {
                    Cmmd = BaseCCommandALL();
                }
                DataTable BufData = tm.GetTable(Cmmd);
                MasterTable = BufData;
                CurrentAllLines = MasterTable.Rows.Count;
                PagesCount = (int)CurrentAllLines / (int)nudDisPlayNo.Value;
                if (CurrentAllLines % (int)nudDisPlayNo.Value > 0)
                {
                    PagesCount = PagesCount + 1;
                }
                this.lblnPagesNo.Text = "第" + CurrentPageNo.ToString() + "页" + "/" + "共" + PagesCount.ToString() + "页";
            }
            catch (Exception e)
            {
                MessageBox.Show("发生错误，输入值有错误！");
            }
        }



        void RefreshDataGrid1()
        {
            this.btnGo.Enabled = false;
            this.btnLast.Enabled = false;
            this.btnNext.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnQuery.Enabled = false;
            this.btnExport.Enabled = false;
            this.btnChart.Enabled = false;
            //  this.DataGrid.DataSource = null;
            UltraGridLayout Layout = DataGrid.DisplayLayout;


            //    Layout.CopyFrom(DataGrid.DisplayLayout, PropertyCategories.All);


            this.dataSetSours1.Tests.Rows.Clear();
            this.dataSetSours1.SubTestResult.Rows.Clear();

            string CurId = "";
            string OldId = "";
            foreach (DataRow dr in this.CurrentTable.Rows)
            {
                CurId = dr["ID"].ToString();
                if (CurId.Length > 0 && CurId != OldId)
                {
                    this.dataSetSours1.Tests.Rows.Add(new object[] { dr["ID"], dr["TestTime"], dr["SN"], dr["PartNr"], "Pass" });
                    //加入主表
                }
                string sRet = "";
                switch (dr["CompareMode"].ToString())
                {
                    case "Between":
                    case "NotBetween":
                    case "HexStringBetween":

                        sRet = "{" + dr["LLimit"].ToString() + "," + dr["ULimit"].ToString() + "}";
                        break;

                    case "Equal":

                        sRet = "= " + dr["Nom"].ToString();
                        break;
                    case "More":

                        sRet = "> " + dr["Nom"].ToString();
                        break;

                    case "CaseEqual":
                        string[] switchs = dr["Nom"].ToString().Split(',');
                        for (int i = 0; i < switchs.Length; i++)
                        {
                            sRet = sRet + "Cs(" + i.ToString() + ") = " + switchs[i] + " ";
                        }
                        //sRet = "= " + NomValue_OBJ.ToString();
                        break;
                    case "Contains":
                        sRet = "⊇" + dr["Nom"].ToString();
                        break;
                    case "HexStringMatch":

                        sRet = "Match " + dr["Nom"].ToString();

                        break;
                }
                this.dataSetSours1.SubTestResult.Rows.Add(new object[] { dr["ID"], dr["SubStepName"], dr["CompareMode"], sRet, dr["Value"] + " " + dr["Unit"].ToString(), dr["Result"], dr["TestTime"], dr["Description"] });
                OldId = CurId;
            }

            this.DataGrid.DataSource = dataSetSours1;


            for (int i = 0; i < DataGrid.Rows.Count; i++)
            {
                for (int j = 0; j < DataGrid.Rows[i].ChildBands[0].Rows.Count; j++)
                {
                    string a = DataGrid.Rows[i].ChildBands[0].Rows[j].Cells["Result"].Value.ToString();
                    if (a == "Fail")
                    {
                        DataGrid.Rows[i].Cells["Result"].Value = a;
                    }
                }
                if (rbtnFail.Checked == true)
                {
                    if ("Pass" == DataGrid.Rows[i].Cells["Result"].Value.ToString())
                    {
                        DataGrid.Rows[i].Hidden = true;
                    }
                }
                else if (rbtnPass.Checked == true)
                {
                    if ("Fail" == DataGrid.Rows[i].Cells["Result"].Value.ToString())
                    {
                        DataGrid.Rows[i].Hidden = true;
                    }
                }
                //DataGrid.Rows[i].CollapseAll();
            }



            Layout.Bands[0].Columns["ID"].Header.Caption = "Index";
            Layout.Bands[1].Columns["ID"].Hidden = true;
            DataGrid.Show();
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.1));
            this.btnGo.Enabled = true;
            this.btnLast.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnCancel.Enabled = true;
            this.btnQuery.Enabled = true;
            this.btnExport.Enabled = true;
            this.btnChart.Enabled = true;

        }


        public frmQueryNew()
        {
            InitializeComponent();
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //   Infragistics.Win.UltraWinDataSource.UltraDataRow dr = ultraDataSource2.Rows.Add();

            try
            {
               
                AddStringBuilder();
                long num = 0;
                switch (comType.Text)
                {
                    case "测试记录":
                        num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 2);
                        break;
                    case "点检记录":
                        num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 1);
                        break;
                    case "中转记录":
                        num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 3);
                        break;
                    default:
                        break;
                }


                GetCount(num);
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.CurrentPageNo = this.CurrentPageNo - 1;
            if (CurrentPageNo < 1)
            {
                CurrentPageNo = 1;
            }
            AddStringBuilder();
            long num = 0;
            switch (comType.Text)
            {
                case "测试记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 2);
                    break;
                case "点检记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 1);
                    break;
                case "中转记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 3);
                    break;
                default:
                    break;
            }
            GetCount(num);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.CurrentPageNo = this.CurrentPageNo + 1;
            if (CurrentPageNo > this.PagesCount)
            {
                CurrentPageNo = PagesCount;
            }
            AddStringBuilder();
            long num = 0;
            switch (comType.Text)
            {
                case "测试记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 2);
                    break;
                case "点检记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 1);
                    break;
                case "中转记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 3);
                    break;
                default:
                    break;
            }

            GetCount(num);
        }

        private void rbtnFail_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPageNo = 1;
            for (int i = 0; i < DataGrid.Rows.Count; i++)
            {
                if (DataGrid.Rows[i].Cells["Result"].Value.ToString() == "Pass")
                {
                    DataGrid.Rows[i].Hidden = true;
                }
                else
                {
                    DataGrid.Rows[i].Hidden = false;
                }
            }
        }

        private void rbtnPass_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPageNo = 1;
            for (int i = 0; i < DataGrid.Rows.Count; i++)
            {
                if (DataGrid.Rows[i].Cells["Result"].Value.ToString() == "Fail")
                {
                    DataGrid.Rows[i].Hidden = true;
                }
                else
                {
                    DataGrid.Rows[i].Hidden = false;
                }
            }
        }

        private void rbtnALL_CheckedChanged(object sender, EventArgs e)
        {
            CurrentPageNo = 1;
            for (int i = 0; i < DataGrid.Rows.Count; i++)
            {
                DataGrid.Rows[i].Hidden = false;
            }
        }

        private void chbTestTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTestTime.Checked == true)
            {
                CurrentPageNo = 1;
                groupBox3.Enabled = true;
            }
            else
            {
                CurrentPageNo = 1;
                groupBox3.Enabled = false;
            }
        }
        string sn = "";
        string stringtime = "";
        string endtime = "";
        string result = "";
        int type = 1;
        private void frmQueryNew_Load(object sender, EventArgs e)
        {

            chbTestKind.Hide();
            cmbTestKind.Hide();
            cmbTestKind.SelectedIndex = 0;
            this.PartNr = this.Prj.PartNumber;
            this.DataGrid.Text = "产品批号： " + this.PartNr + "     产品名称:" + this.Prj.Description;
            comType.Text = "测试记录";
            long num = 0;
            AddStringBuilder();
            switch (comType.Text)
            {
                case "测试记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 2);
                    break;
                case "点检记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 1);
                    break;
                case "中转记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 3);
                    break;
                default:
                    break;
            }


            GetCount(num);


            groupBox3.Enabled = false;
        }


        public long getdata(string sn, string startTime1, string startTime2, string endTime1, string endTime2, string result, int pagevalue, int pagesize, int type)
        {
            string sql1 = "";
            long num = 0;
            dataSetSours1.Tests.Rows.Clear();
            dataSetSours1.SubTestResult.Rows.Clear();
            dataSetSours1.assembleResultTableExample.Rows.Clear();
            
                sql1 = string.Format(" and PartNum ='{0}' and PrdType='{1}'", PartNr, type);//"and PartNum='" + PartNr + "'";
            
           
        
            if (sn != "")
            {
                sql1 = sql1 + string.Format(" and SN='{0}'", sn);
            }
            if (result != "")
            {
                sql1 = sql1 + string.Format(" and Result='{0}'", result);
            }

            if (startTime1 != "")
            {
                sql1 = sql1 + string.Format(" and StartTime between '{0}' and '{1}'", startTime1, startTime2);
            }
            if (endTime1 != "")
            {
                sql1 = sql1 + string.Format(" and EndTime between '{0}' and '{1}'", endTime1, endTime2);
            }

            List<Station_ProductInfo> station_ProductInfo = Common.SQLBAL.GetPages<Station_ProductInfo>("ST_StationProductResExample", pagesize, pagevalue, ref num, "StartTime", sql1);

            sql1 = "";
            for (int i = 0; i < station_ProductInfo.Count; i++)
            {
                string prdType = "";
                switch (station_ProductInfo[i].PrdType)
                {
                    case 0:
                        prdType = "";
                        break;
                    case 1:
                        prdType = "点检";
                        break;
                    case 2:
                        prdType = "测试";
                        break;
                    default:
                        prdType = "";
                        break;
                }
                if (i == 0)
                {
                    sql1 += string.Format("UUTestId='{0}'", station_ProductInfo[i].UUTestId);
                }
                else
                {
                    sql1 += string.Format("or UUTestId='{0}'", station_ProductInfo[i].UUTestId);
                }


                dataSetSours1.Tests.Rows.Add(new object[] { station_ProductInfo[i].UUTestId, station_ProductInfo[i].StartTime, station_ProductInfo[i].SN
                , station_ProductInfo[i].EndTime, station_ProductInfo[i].Result, prdType
                , station_ProductInfo[i].PartNum, station_ProductInfo[i].StationID, station_ProductInfo[i].UserID}
                                                                    );
            }

            if (type == 3)
            {
                sql1 = "and  (" + sql1 + ")";
                //0=进料条码 ，1=出料条码
                string TypeNum = "";
                List<AssembleResultInfo> assembleResultInfo = Common.SQLBAL.GetListInfo<AssembleResultInfo>("ST_assembleResultTableExample", sql1);
                for (int i = 0; i < assembleResultInfo.Count; i++)
                {
                    if (assembleResultInfo[i].Type == 0)
                    {
                        TypeNum = "进料条码";
                    }
                    else if (assembleResultInfo[i].Type == 1)
                    {
                        TypeNum = "出料条码";
                    }

                    dataSetSours1.assembleResultTableExample.Rows.Add(new object[] {assembleResultInfo[i].UUTestId,assembleResultInfo[i].PartSNNum,assembleResultInfo[i].Result,
                        assembleResultInfo[i].Scantime,TypeNum,assembleResultInfo[i].Id,
                     });
                }
            }
            else
            {
                sql1 = "and  (" + sql1 + ")";
                List<Station_TestInfo> station_TestInfo = Common.SQLBAL.GetListInfo<Station_TestInfo>("ST_StationTestResultTableExample", sql1);
                for (int i = 0; i < station_TestInfo.Count; i++)
                {
                    string sRet = "";
                    switch (station_TestInfo[i].ComPareMode)
                    {
                        case "Between":
                        case "NotBetween":
                        case "HexStringBetween":

                            sRet = "{" + station_TestInfo[i].Llimit + "," + station_TestInfo[i].Ulimit + "}";

                            break;

                        case "Equal":

                            sRet = "= " + station_TestInfo[i].TestValue;
                            break;
                        case "More":

                            sRet = "> " + station_TestInfo[i].Nom;
                            break;
                        default:
                            sRet = "= " + station_TestInfo[i].TestValue;
                            break;
                    }


                    dataSetSours1.SubTestResult.Rows.Add(new object[] {
                    station_TestInfo[i].UUTestId,station_TestInfo[i].ID,station_TestInfo[i].StepName,station_TestInfo[i].TestName,station_TestInfo[i].ComPareMode
                    ,station_TestInfo[i].TestTime,sRet ,station_TestInfo[i].TestValue ,station_TestInfo[i].Unit ,station_TestInfo[i].Result ,station_TestInfo[i].Description
                });
                }

            }




            this.DataGrid.DataSource = dataSetSours1;
            UUTTestTableLayOutRefresh();
            DataGrid.Show();

            return num;
        }

        public DataSet getdatatocsv(string sn, string startTime1, string startTime2, string endTime1, string endTime2, string result, int pagevalue, int pagesize,int NumType)
        {

            string sql1 = "";
            sql1 = string.Format(" and PartNum ='{0}' and PrdType={1}", PartNr, NumType);//"and PartNum='" + PartNr + "'";
            if (sn != "")
            {
                sql1 = sql1 + string.Format(" and SN='{0}'", sn);
            }
            if (result != "")
            {
                sql1 = sql1 + string.Format(" and Result='{0}'", result);
            }

            if (startTime1 != "")
            {
                sql1 = sql1 + string.Format(" and StartTime between '{0}' and '{1}'", startTime1, startTime2);
            }
            if (endTime1 != "")
            {
                sql1 = sql1 + string.Format(" and EndTime between '{0}' and '{1}'", endTime1, endTime2);
            }


            long num = 0;
            List<Station_ProductInfo> station_ProductInfo = Common.SQLBAL.GetPages<Station_ProductInfo>("ST_StationProductResExample", pagesize, pagevalue, ref num, "StartTime", sql1);

            dataSetSours2.Tests.Rows.Clear();
            dataSetSours2.SubTestResult.Rows.Clear();
            dataSetSours2.assembleResultTableExample.Rows.Clear();
            sql1 = "";
            for (int i = 0; i < station_ProductInfo.Count; i++)
            {
                string prdType = "";
                switch (station_ProductInfo[i].PrdType)
                {
                    case 0:
                        prdType = "";
                        break;
                    case 1:
                        prdType = "点检";
                        break;
                    case 2:
                        prdType = "测试";
                        break;
                    default:
                        prdType = "";
                        break;
                }
                if (i == 0)
                {
                    sql1 += string.Format("UUTestId='{0}'", station_ProductInfo[i].UUTestId);
                }
                else
                {
                    sql1 += string.Format("or UUTestId='{0}'", station_ProductInfo[i].UUTestId);
                }


                dataSetSours2.Tests.Rows.Add(new object[] { station_ProductInfo[i].UUTestId, station_ProductInfo[i].StartTime, station_ProductInfo[i].SN
                , station_ProductInfo[i].EndTime,  station_ProductInfo[i].Result, station_ProductInfo[i].PrdType
                , station_ProductInfo[i].PartNum, station_ProductInfo[i].StationID, station_ProductInfo[i].UserID}
                                                                    );
            }
            long num1 = 0;
            sql1 = "and  (" + sql1 + ")";

            List<Station_TestInfo> station_TestInfo = Common.SQLBAL.GetListInfo<Station_TestInfo>("ST_StationTestResultTableExample", sql1);
            for (int i = 0; i < station_TestInfo.Count; i++)
            {
                string sRet = "";
                switch (station_TestInfo[i].ComPareMode)
                {
                    case "Between":
                    case "NotBetween":
                    case "HexStringBetween":

                        sRet = "{" + station_TestInfo[i].Llimit + "," + station_TestInfo[i].Ulimit + "}";

                        break;

                    case "Equal":

                        sRet = "= " + station_TestInfo[i].TestValue;
                        break;
                    case "More":

                        sRet = "> " + station_TestInfo[i].Nom;
                        break;
                    default:
                        sRet = "= " + station_TestInfo[i].TestValue;
                        break;
                }


                dataSetSours2.SubTestResult.Rows.Add(new object[] {
                    station_TestInfo[i].UUTestId,station_TestInfo[i].ID,station_TestInfo[i].StepName,station_TestInfo[i].TestName,station_TestInfo[i].ComPareMode
                    ,station_TestInfo[i].TestTime,sRet ,station_TestInfo[i].TestValue ,station_TestInfo[i].Unit ,station_TestInfo[i].Result ,station_TestInfo[i].Description
                });
            }



            return dataSetSours2;


        }


        void GetCount(long num)
        {
            CurrentAllLines = (int)num;
            PagesCount = (int)CurrentAllLines / (int)nudDisPlayNo.Value;
            if (CurrentAllLines % (int)nudDisPlayNo.Value > 0)
            {
                PagesCount = PagesCount + 1;
            }
            this.lblnPagesNo.Text = "第" + CurrentPageNo.ToString() + "页" + "/" + "共" + PagesCount.ToString() + "页";
        }


        void AddStringBuilder()
        {
            if (this.chbSN.Checked)
            {
                sn = this.txtSNB.Text.Trim().ToUpper();
            }
            else
            {
                sn = "";
            }
            if (this.chbTestTime.Checked)
            {
                stringtime = this.dtpStart.Value.ToString();
                endtime = this.dtpEND.Value.AddDays(+1).ToString();
            }
            else
            {
                stringtime = "";
                endtime = "";
            }
            if (this.rbtnFail.Checked)
            {
                result = "Fail";
            }
            else if (this.rbtnPass.Checked)
            {
                result = "Pass";
            }
            else
            {
                result = "";
            }
        }

        /// <summary>
        /// 产品表布局更新
        /// </summary>
        public void UUTTestTableLayOutRefresh()
        {
            UltraGridLayout Layout = DataGrid.DisplayLayout;
            foreach (UltraGridColumn c in Layout.Bands[0].Columns)
            {
                c.Header.Appearance.BackColor = Color.FromArgb(192, 192, 255);
            }
            foreach (UltraGridColumn c in Layout.Bands[1].Columns)
            {
                c.Header.Appearance.BackColor = Color.FromArgb(255, 160, 128);
            }

            Layout.Bands[0].RowLayoutStyle = RowLayoutStyle.ColumnLayout;
            Layout.Bands[1].RowLayoutStyle = RowLayoutStyle.ColumnLayout;



            Layout.Bands[0].Columns["UUTestId"].Header.Caption = "编号";
            Layout.Bands[0].Columns["SN"].Header.Caption = "产品SN";
            Layout.Bands[0].Columns["SN"].Width = 250;

            Layout.Bands[0].Columns["PartNum"].Header.Caption = "产品批号";
            Layout.Bands[0].Columns["PartNum"].Width = 250;
            Layout.Bands[0].Columns["Result"].Header.Caption = "产品测试结果";
            Layout.Bands[0].Columns["StartTime"].Header.Caption = "进线时间";
            Layout.Bands[0].Columns["StartTime"].Width = 190;
            Layout.Bands[0].Columns["EndTime"].Header.Caption = "出线时间";
            Layout.Bands[0].Columns["EndTime"].Width = 190;
            Layout.Bands[0].Columns["PrdType"].Header.Caption = "测试记录类型";
            //Layout.Bands[0].Columns["PrdType"].Hidden = true;
            Layout.Bands[0].Columns["StationID"].Header.Caption = "线号";
            Layout.Bands[0].Columns["StationID"].Hidden = true;
            Layout.Bands[0].Columns["UserID"].Header.Caption = "工号";
            //Layout.Bands[0].Columns["BoxNum"].Hidden = true;





            Layout.Bands[1].Columns["UUTestId"].Hidden = true;
            Layout.Bands[1].Columns["SubTestId"].Hidden = true;
            Layout.Bands[1].Columns["SubStepName"].Hidden = false;
            Layout.Bands[1].Columns["SlaveStationNoName"].Hidden = true;

            Layout.Bands[1].Columns["SubStepName"].Header.Caption = "测试步名";
            Layout.Bands[1].Columns["ComPareMode"].Header.Caption = "比较模式";
            Layout.Bands[1].Columns["TestTime"].Header.Caption = "测试日期";
            Layout.Bands[1].Columns["ExceptValue"].Header.Caption = "预设值";
            Layout.Bands[1].Columns["ExceptValue"].Width = 250;
            Layout.Bands[1].Columns["Value"].Header.Caption = "测试值";
            Layout.Bands[1].Columns["Value"].Width = 250;
            Layout.Bands[1].Columns["Unit"].Header.Caption = "单位";
            Layout.Bands[1].Columns["Result"].Header.Caption = "测试结果";

            Layout.Bands[1].Columns["Description"].Header.Caption = "测试描述";

            DataGrid.Show();
        }



        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                this.CurrentPageNo = int.Parse(this.txtGoalNo.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入的值非法！");
            }
            if (CurrentPageNo > PagesCount)
            {
                CurrentPageNo = PagesCount;
                this.txtGoalNo.Text = CurrentPageNo.ToString();
            }
            if (CurrentPageNo < 1)
            {
                CurrentPageNo = 1;
                this.txtGoalNo.Text = "1";
            }
            AddStringBuilder();
            long num = 0;
            switch (comType.Text)
            {
                case "测试记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 2);
                    break;
                case "点检记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 1);
                    break;
                case "中转记录":
                    num = getdata(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), 3);
                    break;
                default:
                    break;
            }

            GetCount(num);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnExport_Click(object sender, EventArgs e)
        {
            int NumType = 0;
            if (comType.Text == "测试记录")
            {
                NumType = 2;
            }
            else if (comType.Text == "点检记录")
            {
                NumType = 1;
                
            }
          
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "保存路径(*.csv)|*.csv";
            sfd.FileName = DateTime.Now.ToString("yyyyMMdd");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fullPath = sfd.FileName;

                ThreadPool.QueueUserWorkItem(state =>
              {
                  bool ret = SaveCsv(fullPath, NumType);
                  if (ret)
                  {
                      //完成
                      MessShwo("导出成功！");
                  }
                  else
                  {
                      //未完成

                      MessShwo("导出失败！");
                  }
                
              });
            }
        }

        void MessShwo(string str)
        {
            if (this.InvokeRequired)
            {
                Invoke(new System.Action(() =>
                {
                    MessageBox.Show(str, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }));
            }
            else
            {
                MessageBox.Show(str, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }


        private bool SaveCsv(string fullPath,int TypeNum)
        {
            string UUtestid = "";
            string TestDate = string.Empty;
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {

                var fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw = new StreamWriter(fs, Encoding.UTF8);
                var data = "";
                data = string.Format("时间：,{0},型号：,{1}", DateTime.Now.ToString(), this.PartNr);
                sw.WriteLine(data);

                data = "UUTestId,SN,进站时间,出站时间,测试结果,测试记录,产品类型,工站名称,操作人";

                //查询单步名称
                string sql = @"select  StepName ,Description from ST_StationTestResultTableExample t right join ST_StationProductResExample p on
                        t.UUTestId=p.UUTestId and p.PartNum='" + PartNr + "' where StepName is not NULL group by StepName ,Description order by StepName asc;";
                DataTable StepNamedt = Common.SQLBAL.GetDataTable(sql);

                if (StepNamedt.Rows.Count > 0 && StepNamedt != null)
                {
                    for (int i = 0; i < StepNamedt.Rows.Count; i++)
                    {
                        data += string.Format(",{0}({1})", StepNamedt.Rows[i][0], StepNamedt.Rows[i][1]);
                    }
                }
                else
                {

                    return false;
                }

                sw.WriteLine(data);


                List<string> step = new List<string>();

                for (int stepnum = 0; stepnum < StepNamedt.Rows.Count; stepnum++)
                {
                    step.Add("");
                    // stepbak.Add("");
                }
                List<string> stepbak = new List<string>(step);

                //分页导出
                //查询总表 uutest拿出
                PagesCount = (int)CurrentAllLines / 1000;
                if (CurrentAllLines % (int)nudDisPlayNo.Value > 0)
                {
                    PagesCount = PagesCount + 1;
                }
                data = "";
                #region 对齐数据
                //在子表查询
                //for (int i = 1; i <= PagesCount; i++)
                //{
                DataSet DataCsv = getdatatocsv(sn, stringtime, endtime, "", "", result, CurrentPageNo, Convert.ToInt32(this.nudDisPlayNo.Value), TypeNum);
                    for (int rownum = 0; rownum < DataCsv.Tables[0].Rows.Count; rownum++)
                    {
                        data = "";
                        data += DataCsv.Tables[0].Rows[rownum]["UUTestId"].ToString();            //UUTestId
                        data += "," + DataCsv.Tables[0].Rows[rownum]["SN"].ToString();            //SN
                        data += "," + DataCsv.Tables[0].Rows[rownum]["StartTime"].ToString();     //进站时间
                        data += "," + DataCsv.Tables[0].Rows[rownum]["EndTime"].ToString();       //出站时间
                        data += "," + DataCsv.Tables[0].Rows[rownum]["Result"].ToString();        //测试结果
                        //data += "," + DataCsv.Tables[0].Rows[rownum]["PrdType"].ToString();       //测试记录
                        int PrdType = Convert.ToInt32(DataCsv.Tables[0].Rows[rownum]["PrdType"]);//0:默认；1：点检；2：测试；3：返工
                        switch (PrdType)
                        {
                            case 0: data += "," + "Default"; break;
                            case 1: data += "," + "点检产品"; break;
                            case 2: data += "," + "测试产品"; break;
                            case 3: data += "," + "返工产品"; break;
                            default: break;
                        }
                        data += "," + DataCsv.Tables[0].Rows[rownum]["PartNum"].ToString();       //产品类型
                        data += "," + DataCsv.Tables[0].Rows[rownum]["StationID"].ToString();     //工站名称
                        data += "," + DataCsv.Tables[0].Rows[rownum]["UserID"].ToString();        //操作人

                        //获取测试数据
                        //step = stepbak;

                        UUtestid = DataCsv.Tables[0].Rows[rownum]["UUTestId"].ToString();

                        for (int testNum = 0; testNum < DataCsv.Tables[1].Rows.Count; testNum++)
                        {
                            //查找相同uutestid
                            if (UUtestid == DataCsv.Tables[1].Rows[testNum]["UUTestId"].ToString())
                            {
                                for (int stepnum = 0; stepnum < StepNamedt.Rows.Count; stepnum++)
                                {

                                    //查找相同测试项
                                    if (StepNamedt.Rows[stepnum]["StepName"].ToString() == DataCsv.Tables[1].Rows[testNum]["SubStepName"].ToString()
                                        && StepNamedt.Rows[stepnum]["Description"].ToString() == DataCsv.Tables[1].Rows[testNum]["Description"].ToString()
                                        )
                                    {
                                        step[stepnum] = DataCsv.Tables[1].Rows[testNum]["Value"].ToString();
                                        break;
                                    }
                                }

                            }
                        }
                        for (int s = 0; s < step.Count; s++)
                        {
                            data += "," + step[s];
                        }

                        sw.WriteLine(data);
                        step = new List<string>(stepbak);
                    }
                #endregion

                //}

            }
            catch (Exception ex)
            {
                MessShwo(ex.ToString());
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return true;
        }


        private void btnChart_Click(object sender, EventArgs e)
        {
            frmChart frm = new frmChart();
            frm.tm = this.tm;
            frm.MasterTable = this.MasterTable;
            frm.ShowDialog(this);
        }

        public void ChartAnalysis()
        {
            //得到该产品一年的测试步
            string cmd = "select SubStepName from SingleSteps where PartNr = " + PartNr + " AND TestTime BETWEEN '2014/1/1 00:00:00'AND '2014/12/31 23:59:59'" + " Group by SubStepName";
            DataTable dt = tm.GetTable(cmd);
            List<ChartStruct> ChartStructList = new List<ChartStruct>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartStruct CS = new ChartStruct();
                CS.StepName = dr["SubStepName"].ToString();
                ChartStructList.Add(CS);

                cmd = "select * from SingleSteps where SubStepName = '" + dr["SubStepName"] + "'";
                DataTable dtt = tm.GetTable(cmd);
                foreach (DataRow dr1 in dtt.Rows)
                {
                    //if (!TestStepList.Keys.Contains(dr["SubStepName"].ToString()))
                    //{
                    //    TestStepList.Add(dr1["SubStepName"].ToString(), dr1["Description"].ToString());
                    //}
                }
            }
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnChartAy_Click(object sender, EventArgs e)
        {
            string cmd = "select SubStepName from SingleSteps where PartNr = " + "'" + PartNr + "'" + " Group by SubStepName";
            DataTable dt = tm.GetTable(cmd);
            List<ChartStruct> ChartStructList = new List<ChartStruct>();
            foreach (DataRow dr in dt.Rows)
            {
                ChartStruct CS = new ChartStruct();
                CS.StepName = dr["SubStepName"].ToString();
                cmd = "select * from SingleSteps where SubStepName = '" + dr["SubStepName"] + "'";
                DataTable dtt = tm.GetTable(cmd);
                foreach (DataRow dr1 in dtt.Rows)
                {
                    if (CS.StepName == dr["SubStepName"].ToString())
                    {
                        if (CS.Discrption != dr1["Description"].ToString())
                        {
                            CS.Discrption = dr1["Description"].ToString();
                        }
                    }
                }
                ChartStructList.Add(CS);
            }
            frmChart_Month frm = new frmChart_Month();
            frm.MyChartStructList = ChartStructList;
            frm.tm = tm;
            frm.ShowDialog();
        }

        private void comType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comType.Text == "中转记录")
            {
                btnExport.Enabled = false;
            }
            else
            {
                btnExport.Enabled = true;
            }
        }

        private void chbSN_CheckedChanged(object sender, EventArgs e)
        {
            txtSNB.ReadOnly = !chbSN.Checked;
        }

      
    }
}
