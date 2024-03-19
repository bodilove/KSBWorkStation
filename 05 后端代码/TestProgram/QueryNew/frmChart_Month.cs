using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace Test.Query
{
    public partial class frmChart_Month : Form
    {
        #region【变量】

        public List<ChartStruct> MyChartStructList = null;
        public TestDataSqlManger tm = null;
        Dictionary<int, string> Month12 = null;
        public int year = 0;
        ChartStruct CurrentChartStruct = null;
        List<int> YearsList = new List<int>();
        #endregion

        void InitMonth12()
        {
      
                Month12 = new Dictionary<int, string>();
          
           // 
            //  year = 2000;

            Month12.Add(1, "'" + year.ToString() + "/1/1 00:00:00'AND '" + year.ToString() + "/1/31 23:59:59'");
            if (year % 4 == 0)
            {
                Month12.Add(2, "'" + year.ToString() + "/2/1 00:00:00'AND '" + year.ToString() + "/2/29 23:59:59'");
            }
            else
            {
                Month12.Add(2, "'" + year.ToString() + "/2/1 00:00:00'AND '" + year.ToString() + "/2/28 23:59:59'");
            }
            Month12.Add(3, "'" + year.ToString() + "/3/1 00:00:00'AND '" + year.ToString() + "/3/31 23:59:59'");
            Month12.Add(4, "'" + year.ToString() + "/4/1 00:00:00'AND '" + year.ToString() + "/4/30 23:59:59'");
            Month12.Add(5, "'" + year.ToString() + "/5/1 00:00:00'AND '" + year.ToString() + "/5/31 23:59:59'");
            Month12.Add(6, "'" + year.ToString() + "/6/1 00:00:00'AND '" + year.ToString() + "/6/30 23:59:59'");
            Month12.Add(7, "'" + year.ToString() + "/7/1 00:00:00'AND '" + year.ToString() + "/7/31 23:59:59'");
            Month12.Add(8, "'" + year.ToString() + "/8/1 00:00:00'AND '" + year.ToString() + "/8/31 23:59:59'");
            Month12.Add(9, "'" + year.ToString() + "/9/1 00:00:00'AND '" + year.ToString() + "/9/30 23:59:59'");
            Month12.Add(10, "'" + year.ToString() + "/10/1 00:00:00'AND '" + year.ToString() + "/10/31 23:59:59'");
            Month12.Add(11, "'" + year.ToString() + "/11/1 00:00:00'AND '" + year.ToString() + "/11/30 23:59:59'");
            Month12.Add(12, "'" + year.ToString() + "/12/1 00:00:00'AND '" + year.ToString() + "/12/31 23:59:59'");

        }

        void InitYearsList()
        {
            int Thisyear = System.DateTime.Now.Year;
            for (int i = 2010; i <= Thisyear; i++)
            {
                YearsList.Add(i);
            }
            this.cmbDropList.DataSource = YearsList;
            this.cmbDropList.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDropList.SelectedIndex = YearsList.Count - 1;
        }

        public frmChart_Month()
        {
            InitializeComponent();
       
        }

        void ChartShow()
        {
            foreach (ChartStruct cs in MyChartStructList)
            {
                this.DataGrid.Rows.Add(new object[] { cs.StepName, cs.Discrption });
            }
        }

        private void frmChart_Month_Load(object sender, EventArgs e)
        {
            this.chkFailTimes.Checked = true;
            this.chkPassTime.Checked = true;
            ChartShow();
            InitYearsList();
            year = System.DateTime.Now.Year;
            InitMonth12();
           
    
            string key = DataGrid.Rows[DataGrid.CurrentCell.RowIndex].Cells[0].Value.ToString();
            CurrentChartStruct = null;
            foreach (ChartStruct cs in MyChartStructList)
            {
                if (cs.StepName == key)
                {
                    CurrentChartStruct = cs;
                }
            }
            if (CurrentChartStruct == null) return;

            for (int i = 0; i < 12; i++)
            {
                CurrentChartStruct.PassTime[i] = 0;
                CurrentChartStruct.FailTime[i] = 0;
                CurrentChartStruct.TestTime[i] = 0;
            }
            foreach (KeyValuePair<int, string> kp in Month12)
            {

                string cmd = "select * from SingleSteps where SubStepName = '" + key + "'" + " AND TestTime BETWEEN " + kp.Value;
                DataTable dt = tm.GetTable(cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    CurrentChartStruct.TestTime[kp.Key - 1] = CurrentChartStruct.TestTime[kp.Key - 1] + 1;
                    if (dr["Result"].ToString() == "Pass")
                    {
                        CurrentChartStruct.PassTime[kp.Key - 1] = CurrentChartStruct.PassTime[kp.Key - 1] + 1;
                    }
                    if (dr["Result"].ToString() == "Fail")
                    {
                        CurrentChartStruct.FailTime[kp.Key - 1] = CurrentChartStruct.FailTime[kp.Key - 1] + 1;
                    }
                }
            }
            RefreshChart();
        }
        void RefreshChart()
        {
            this.MyChart.Series["TestTimes"].Points.Clear();
            this.MyChart.Series["PassTimes"].Points.Clear();
            this.MyChart.Series["FailTimes"].Points.Clear();
            for (int i = 0; i < 12; i++)
            {
                DataPoint p = new DataPoint();
                p.AxisLabel = (i+1).ToString()+"月份";
                p.XValue = i+1;
                p.YValues[0] = this.CurrentChartStruct.TestTime[i];

                this.MyChart.Series["TestTimes"].Points.Add(p);
                this.MyChart.Series["PassTimes"].Points.AddXY(i + 1, this.CurrentChartStruct.PassTime[i]);
                this.MyChart.Series["FailTimes"].Points.AddXY(i + 1, this.CurrentChartStruct.FailTime[i]);
            }
        }

        private void DataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex < 0)
            {
                return;
            }
            string key = DataGrid.Rows[rowIndex].Cells[0].Value.ToString();
            CurrentChartStruct = null;
            foreach (ChartStruct cs in MyChartStructList)
            {
                if (cs.StepName == key)
                {
                    CurrentChartStruct = cs;
                }
            }
            if (CurrentChartStruct == null) return;

            for (int i = 0; i < 12; i++)
            {
                CurrentChartStruct.PassTime[i] = 0;
                CurrentChartStruct.FailTime[i] = 0;
                CurrentChartStruct.TestTime[i] = 0;
            }
            foreach (KeyValuePair<int, string> kp in Month12)
            {
                string cmd = "select * from SingleSteps where SubStepName = '" + key + "'" + " AND TestTime BETWEEN " + kp.Value;
                DataTable dt = tm.GetTable(cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    CurrentChartStruct.TestTime[kp.Key - 1] = CurrentChartStruct.TestTime[kp.Key - 1] + 1;
                    if (dr["Result"].ToString() == "Pass")
                    {
                        CurrentChartStruct.PassTime[kp.Key - 1] = CurrentChartStruct.PassTime[kp.Key - 1] + 1;
                    }
                    if (dr["Result"].ToString() == "Fail")
                    {
                        CurrentChartStruct.FailTime[kp.Key - 1] = CurrentChartStruct.FailTime[kp.Key - 1] + 1;
                    }
                }
            }
            RefreshChart();
        }

        private void chkPassTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassTime.Checked == true)
            {
                MyChart.Series["PassTimes"].Enabled = true;
            }
            else
            {
                MyChart.Series["PassTimes"].Enabled = false;
            }
        }

        private void chkFailTimes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFailTimes.Checked == true)
            {
                MyChart.Series["FailTimes"].Enabled = true;
            }
            else
            {
                MyChart.Series["FailTimes"].Enabled = false;
            }
        }

        private void cmbDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            year = (int)this.cmbDropList.SelectedItem;
            InitMonth12();
            //InitYearsList();
            string key = DataGrid.Rows[DataGrid.CurrentCell.RowIndex].Cells[0].Value.ToString();
            CurrentChartStruct = null;
            foreach (ChartStruct cs in MyChartStructList)
            {
                if (cs.StepName == key)
                {
                    CurrentChartStruct = cs;
                }
            }
            if (CurrentChartStruct == null) return;

            for (int i = 0; i < 12; i++)
            {
                CurrentChartStruct.PassTime[i] = 0;
                CurrentChartStruct.FailTime[i] = 0;
                CurrentChartStruct.TestTime[i] = 0;
            }
            foreach (KeyValuePair<int, string> kp in Month12)
            {

                string cmd = "select * from SingleSteps where SubStepName = '" + key + "'" + " AND TestTime BETWEEN " + kp.Value;
                DataTable dt = tm.GetTable(cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    CurrentChartStruct.TestTime[kp.Key - 1] = CurrentChartStruct.TestTime[kp.Key - 1] + 1;
                    if (dr["Result"].ToString() == "Pass")
                    {
                        CurrentChartStruct.PassTime[kp.Key - 1] = CurrentChartStruct.PassTime[kp.Key - 1] + 1;
                    }
                    if (dr["Result"].ToString() == "Fail")
                    {
                        CurrentChartStruct.FailTime[kp.Key - 1] = CurrentChartStruct.FailTime[kp.Key - 1] + 1;
                    }
                }
            }
            RefreshChart();
        }
    }
}
