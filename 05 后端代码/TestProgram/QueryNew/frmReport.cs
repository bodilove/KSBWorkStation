using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace Test.Query
{
    public partial class frmReport : Form
    {

        #region 参数
        public ReportTableFormat myreport = null;

       

        public string S_PartNr = "0000";
        public string S_ProductName = "uuuu";
        public string S_User = "uuu";

        public string S_ExportTime = "";
        #endregion

        public frmReport()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            S_ExportTime = System.DateTime.Now.ToString();
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("S_PartNr", S_PartNr));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("S_ProductName", S_ProductName));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("S_ExportTime", S_ExportTime));
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter("S_User", S_User));
            this.SubTestStepBindingSource.DataSource = myreport.SubTestStep;

            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            this.TestsBindingSource.DataSource = myreport.Tests;




            this.reportViewer1.RefreshReport();
        }


        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", myreport.SubTestStep));
        }
    }
}
