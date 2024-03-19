namespace Test.Query
{
    partial class frmReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.TestsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataSetSours = new Query.DataSetSours();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SubTestStepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TestsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetSours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubTestStepBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TestsBindingSource
            // 
            this.TestsBindingSource.DataMember = "Tests";
            this.TestsBindingSource.DataSource = this.DataSetSours;
            // 
            // DataSetSours
            // 
            this.DataSetSours.DataSetName = "DataSetSours";
            this.DataSetSours.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.TestsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Query.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(742, 326);
            this.reportViewer1.TabIndex = 0;
            // 
            // SubTestStepBindingSource
            // 
            this.SubTestStepBindingSource.DataMember = "SubTestStep";
            this.SubTestStepBindingSource.DataSource = this.DataSetSours;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 326);
            this.Controls.Add(this.reportViewer1);
            this.Name = "frmReport";
            this.Text = "报表打印";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TestsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetSours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubTestStepBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource SubTestStepBindingSource;
        private DataSetSours DataSetSours;
        private System.Windows.Forms.BindingSource TestsBindingSource;

    }
}