using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Query
{
    public partial class frmQuery : Form
    {
        DataTable CurrentTable = null;

        TestDataSqlManger tm = new TestDataSqlManger();

        int CurrentAllLines = 0;

        int DisLineNo = 30;

        int PagesCount = 0;

        int CurrentPageNo = 1;

        string TableName = "SingleSteps";

       // List<string> Command = new List<string>();

        Dictionary<string, string> Command = null;

        string CommandPart(string colName, string R1, string R2)
        {
            string CommandPart = " AND " + colName + " BETWEEN " +R1 + "AND " + R2;
            return CommandPart;
        }

        string CommandPart(string colName, string value)
        {
            string CommandPart = " AND " + colName + "= " +"'"+ value+"'";
            return CommandPart;
        }

        public frmQuery()
        {
            InitializeComponent();
        }

        List<string> BaseQCommand()
        {
            List<string> BUFSTR = new List<string>();
            //---更新 BaseCommand
            int n = DisLineNo * (this.CurrentPageNo - 1);
            int m = n + DisLineNo;
             BUFSTR.Add("with cte as(  select id0=row_number() over(order by TestTime),* from " + TableName);
              BUFSTR.Add(") select * from cte where id0 BETWEEN " + n.ToString() + " AND " + m.ToString());
            return BUFSTR;
        }

        string BaseCCommand()
        {
            string BUFSTR = "SELECT COUNT(*) AS Expr1 FROM " + TableName;
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
            if (chbPartNr.Checked == true)
            {
                if (!this.Command.ContainsKey("PartNr"))
                {
                    this.Command.Add("PartNr", this.CommandPart("PartNr", this.txtPartNrB.Text));
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
                    this.Command.Add("SN", this.CommandPart("SN", this.txtSNB.Text));
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
            string BeginDate = "'" + this.txtyearB.Text + "/" + this.txtMonthB.Text + "/" + this.txtDayB.Text + " " + this.txtHourB.Text + ":" + this.txtMB.Text + ":" + this.txtSB.Text + "'";
            string StopDate = "'" + this.txtyearS.Text + "/" + this.txtMonthS.Text + "/" + this.txtDayS.Text + " " + this.txtHourS.Text + ":" + this.txtMS.Text + ":" + this.txtSS.Text + "'";
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
            string Result = "";

            if (rbtnFail.Checked == true)
            {
                Result = "Fail";
            }
            else if (rbtnPass.Checked == true)
            {
                Result = "PASS";
            }
            if (!this.Command.ContainsKey("Result"))
            {
                this.Command.Add("Result", this.CommandPart("Result", Result));
            }
            else
            {
                if (this.Command.ContainsKey("Result"))
                {
                    this.Command.Remove("Result");
                }
            }
            if (rbtnALL.Checked == true)
            {
                this.Command.Remove("Result");
            }
            foreach (KeyValuePair<string, string> d in Command)
            {
                Cm = Cm + d.Value;
            }
            return Cm;

        }

        void CCount()
        {
            
            string Cmmd = "";
            if (this.chbPartNr.Checked || !this.rbtnALL.Checked || this.chbSN.Checked || this.chbTestTime.Checked || chbTestKind.Checked == true)
            {
                Cmmd = BaseCCommand() + " where " + UPCommand().Remove(0, 4);
            }
            else
            {
                Cmmd = BaseCCommand();
            }
            DataTable BufData = tm.GetTable(Cmmd);
            CurrentAllLines = (int)BufData.Rows[0][0];
            PagesCount = (int)CurrentAllLines / DisLineNo;
            if (CurrentAllLines % DisLineNo > 0)
            {
                PagesCount = PagesCount + 1;
            }
            this.lblnPagesNo.Text = "第" + CurrentPageNo.ToString() + "页" + "/" + "共" + PagesCount.ToString() + "页";

        }

        void Qurey()
        {
            string Cmmd = "";
            this.dataGridView1.DataBindings.Clear();
            if (this.chbPartNr.Checked == true || this.rbtnALL.Checked == false || this.chbSN.Checked == true || this.chbTestTime.Checked == true || chbTestKind.Checked ==true)
            {
                Cmmd = BaseQCommand()[0] + " where " + UPCommand().Remove(0, 4) + BaseQCommand()[1];
            }
            else
            {
                Cmmd = BaseQCommand()[0] + BaseQCommand()[1];
            }

            this.CurrentTable = tm.GetTable(Cmmd);
            CurrentTable.Columns[0].ColumnName = "Nr";
            this.dataGridView1.DataSource = this.CurrentTable;
            this.dataGridView1.Columns[0].Width = 50;
            dataGridView1.Refresh();
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            cmbTestKind.SelectedIndex = 0;
            CCount();
            Qurey();
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();

            txtyearB.Text = year;
            txtyearS.Text = year;
            txtMonthB.Text = month;
            txtMonthS.Text = month;
            txtDayB.Text = day;
            txtDayS.Text = day;
            this.txtHourB.Text = "00";
            this.txtMB.Text = "00";
            this.txtSB.Text = "00";
            this.txtHourS.Text = "23";
            this.txtMS.Text = "59";
            this.txtSS.Text = "59";
            groupBox3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            CurrentPageNo = 1;
            CCount();
            Qurey();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
           
            this.CurrentPageNo = this.CurrentPageNo - 1;
            if (CurrentPageNo <1)
            {
                CurrentPageNo = 1;
            } 
            CCount();
            Qurey();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.CurrentPageNo = this.CurrentPageNo + 1;  
            if (CurrentPageNo > this.PagesCount)
            {
                CurrentPageNo = PagesCount;
            }
            CCount(); 
            Qurey();
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
            CCount();
            Qurey();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void chbTestTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTestTime.Checked == true)
            {
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
            }
        }

        private void chbTestKind_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chbSN_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
