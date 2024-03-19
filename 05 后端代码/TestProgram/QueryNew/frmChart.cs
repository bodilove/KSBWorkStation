using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
namespace Test.Query
{
    public partial class frmChart : Form
    {
        public frmChart()
        {
            InitializeComponent();
        }

        int[] PassProducts = new int[12];
        int[] FailProducts = new int[12];
        int[] Products = new int[12];
        public TestDataSqlManger tm = null;
        public DataTable MasterTable = null;
        Dictionary<int, string> Month12 = null;
        List<int> YearsList = new List<int>();
        public int year = 0;
        bool b = false;
        Thread th = null;
        
        void InitMonth12()
        {

                Month12 = new Dictionary<int, string>();

            //year = System.DateTime.Now.Year;
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

        private void frmChart_Load(object sender, EventArgs e)
        {
        
        }

        void Init()
        {

            for (int i = 0; i < 12; i++)
            {
                PassProducts[i] = 0;
                FailProducts[i] = 0;
                Products[i] = 0;
            }
            foreach (KeyValuePair<int, string> kp in Month12)
            {
                foreach (DataRow dr in MasterTable.Rows)
                {
                    string cmd = "select Result from SingleSteps where id = " + dr[0].ToString() + " AND TestTime BETWEEN " + kp.Value;
                    DataTable dt = tm.GetTable(cmd);
                    bool res = true;
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        if (dr1[0].ToString() == "Fail")
                        {
                            res = false;
                            break;
                        }
                       
                    }
                    if (res&&dt.Rows.Count>0)
                    {
                        PassProducts[kp.Key - 1] = PassProducts[kp.Key - 1] + 1;
                    }
                    if (!res && dt.Rows.Count > 0)
                    {
                        FailProducts[kp.Key - 1] = FailProducts[kp.Key - 1] + 1;
                    }
                    if ( dt.Rows.Count > 0)
                    {
                        Products[kp.Key - 1] = Products[kp.Key - 1] + 1;
                    }
                }
            }
        }

        void RefreshChart()
        {
            this.MyChart.Series[0].Points.Clear();
            this.MyChart.Series[1].Points.Clear();
            this.MyChart.Series[2].Points.Clear();
            for (int i = 0; i < 12; i++)
            {
                DataPoint p = new DataPoint();
                p.AxisLabel = (i + 1).ToString() + "月份";
                p.XValue = i + 1;
                p.YValues[0] = Products[i];

                this.MyChart.Series["TestProducts"].Points.Add(p);
                this.MyChart.Series["PassProducts"].Points.AddXY(i + 1, PassProducts[i]);
                this.MyChart.Series["FailProducts"].Points.AddXY(i + 1, FailProducts[i]);
            }
        
        }

        private void cmbDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (b == false)
            {
        
                this.year = (int)this.cmbDropList.SelectedItem;
                frmMessage f = new frmMessage();
                f.label1.Text = "正在查询分析...";
                f.Show();
                Application.DoEvents();
                InitMonth12();
                thInitStart();
                do
                {
                    if (th != null && th.IsAlive == false)
                    {
                        break;
                    }
                } while (true);
                RefreshChart();
                f.Close();
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void thInitStart()
        {
            th = new Thread(Init);
            th.Start();
        }

        public void thInitStop()
        {
          if(th!=null&&th.IsAlive)
            {
                th.Abort();
            }
        }

        private void frmChart_Shown(object sender, EventArgs e)
        {
            
            frmMessage f = new frmMessage();
            f.label1.Text = "正在查询分析...";
            f.Show();
            Application.DoEvents();
            b = true;
            this.year = DateTime.Now.Year;
            InitYearsList();
            InitMonth12();
            thInitStart();
            do
            {
                if (th != null && th.IsAlive==false)
                {
                    break;
                }
            }while(true);
            RefreshChart();
            b = false;
            f.Close();
        }
    }
}

