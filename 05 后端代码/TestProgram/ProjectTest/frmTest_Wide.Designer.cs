namespace Test.ProjectTest
{
    partial class frmTest_Wide
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Band 1");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Test Result", 0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StepNr", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 1", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StepNr", 0);
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Test Result", 1);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Expected Value", 2);
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Test Value", 3);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Unit", 4);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ErrorCode", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 6);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataBand ultraDataBand1 = new Infragistics.Win.UltraWinDataSource.UltraDataBand("Band 1");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusInfo = new System.Windows.Forms.StatusStrip();
            this.statusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTestTime = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnClearCount = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnClearCounter = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFail = new System.Windows.Forms.Label();
            this.lblPPM = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkBreakOnError = new System.Windows.Forms.CheckBox();
            this.btnTogglePPM = new System.Windows.Forms.Button();
            this.tblChooseKind = new System.Windows.Forms.TableLayoutPanel();
            this.ckbNormalTest = new System.Windows.Forms.RadioButton();
            this.ckbAnalysis = new System.Windows.Forms.RadioButton();
            this.LabelTestModeState = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DataGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.dgvTestPowerOn = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SubStepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.btnClearCounter.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tblChooseKind.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestPowerOn)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubStepBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // statusInfo
            // 
            this.statusInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusUser,
            this.lblStatus,
            this.toolStripStatusLabel2,
            this.Progress});
            this.statusInfo.Location = new System.Drawing.Point(0, 674);
            this.statusInfo.Name = "statusInfo";
            this.statusInfo.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusInfo.Size = new System.Drawing.Size(862, 22);
            this.statusInfo.TabIndex = 0;
            this.statusInfo.Text = "statusInfo";
            // 
            // statusUser
            // 
            this.statusUser.Name = "statusUser";
            this.statusUser.Size = new System.Drawing.Size(87, 17);
            this.statusUser.Text = "用户：Zhao01";
            this.statusUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(544, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "测试项目";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel2.Text = "测试进度：";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Progress
            // 
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(150, 16);
            this.Progress.Value = 40;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.68213F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.31786F));
            this.tableLayoutPanel1.Controls.Add(this.lblMessage, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvTestPowerOn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(862, 674);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.Cyan;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblMessage.Location = new System.Drawing.Point(233, 604);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(625, 62);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "请放入产品";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(231, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(629, 92);
            this.panel2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblTitle);
            this.groupBox1.Location = new System.Drawing.Point(10, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 2, 6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(605, 77);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(2, 17);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(602, 49);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Huf PEPS Test";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(231, 98);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 498F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(629, 498);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.lblTestTime, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblResult, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnClearCount, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.btnQuit, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.btnClearCounter, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnTogglePPM, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.tblChooseKind, 0, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(406, 2);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(221, 494);
            this.tableLayoutPanel4.TabIndex = 76;
            // 
            // lblTestTime
            // 
            this.lblTestTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTestTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTestTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTestTime.Location = new System.Drawing.Point(2, 80);
            this.lblTestTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTestTime.Name = "lblTestTime";
            this.lblTestTime.Size = new System.Drawing.Size(217, 40);
            this.lblTestTime.TabIndex = 77;
            this.lblTestTime.Text = "测试时间：";
            this.lblTestTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.Color.LawnGreen;
            this.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Font = new System.Drawing.Font("宋体", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.Location = new System.Drawing.Point(2, 0);
            this.lblResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(217, 80);
            this.lblResult.TabIndex = 76;
            this.lblResult.Text = "PASS";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearCount
            // 
            this.btnClearCount.BackgroundImage = global::Test.ProjectTest.Properties.Resources.button;
            this.btnClearCount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearCount.ForeColor = System.Drawing.SystemColors.Info;
            this.btnClearCount.Location = new System.Drawing.Point(2, 376);
            this.btnClearCount.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearCount.Name = "btnClearCount";
            this.btnClearCount.Size = new System.Drawing.Size(217, 56);
            this.btnClearCount.TabIndex = 4;
            this.btnClearCount.Text = "清除计数";
            this.btnClearCount.UseVisualStyleBackColor = true;
            this.btnClearCount.Click += new System.EventHandler(this.btnClearCount_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackgroundImage = global::Test.ProjectTest.Properties.Resources.button;
            this.btnQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnQuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuit.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuit.ForeColor = System.Drawing.SystemColors.Info;
            this.btnQuit.Location = new System.Drawing.Point(2, 436);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(2);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(217, 56);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "退出";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnClearCounter
            // 
            this.btnClearCounter.Controls.Add(this.tableLayoutPanel6);
            this.btnClearCounter.Controls.Add(this.chkBreakOnError);
            this.btnClearCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearCounter.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearCounter.Location = new System.Drawing.Point(2, 122);
            this.btnClearCounter.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearCounter.Name = "btnClearCounter";
            this.btnClearCounter.Padding = new System.Windows.Forms.Padding(2);
            this.btnClearCounter.Size = new System.Drawing.Size(217, 107);
            this.btnClearCounter.TabIndex = 77;
            this.btnClearCounter.TabStop = false;
            this.btnClearCounter.Text = "统计";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.lblFail, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.lblPPM, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.lblPass, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(4, 21);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(211, 54);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // lblFail
            // 
            this.lblFail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFail.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFail.Location = new System.Drawing.Point(107, 17);
            this.lblFail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFail.Name = "lblFail";
            this.lblFail.Size = new System.Drawing.Size(102, 17);
            this.lblFail.TabIndex = 6;
            this.lblFail.Text = "100";
            this.lblFail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPPM
            // 
            this.lblPPM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPPM.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPPM.Location = new System.Drawing.Point(107, 34);
            this.lblPPM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPPM.Name = "lblPPM";
            this.lblPPM.Size = new System.Drawing.Size(102, 20);
            this.lblPPM.TabIndex = 5;
            this.lblPPM.Text = "100";
            this.lblPPM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPass
            // 
            this.lblPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPass.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPass.Location = new System.Drawing.Point(107, 0);
            this.lblPass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(102, 17);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "100";
            this.lblPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(2, 17);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "不合格：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(2, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "不合格率：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "合格：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkBreakOnError
            // 
            this.chkBreakOnError.AutoSize = true;
            this.chkBreakOnError.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkBreakOnError.Checked = true;
            this.chkBreakOnError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBreakOnError.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkBreakOnError.ForeColor = System.Drawing.SystemColors.InfoText;
            this.chkBreakOnError.Location = new System.Drawing.Point(4, 83);
            this.chkBreakOnError.Margin = new System.Windows.Forms.Padding(2);
            this.chkBreakOnError.Name = "chkBreakOnError";
            this.chkBreakOnError.Size = new System.Drawing.Size(131, 19);
            this.chkBreakOnError.TabIndex = 6;
            this.chkBreakOnError.Text = "出错后终止测试";
            this.chkBreakOnError.UseVisualStyleBackColor = true;
            this.chkBreakOnError.Visible = false;
            this.chkBreakOnError.CheckedChanged += new System.EventHandler(this.chkBreakOnError_CheckedChanged);
            // 
            // btnTogglePPM
            // 
            this.btnTogglePPM.BackgroundImage = global::Test.ProjectTest.Properties.Resources.button;
            this.btnTogglePPM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTogglePPM.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTogglePPM.ForeColor = System.Drawing.SystemColors.Info;
            this.btnTogglePPM.Location = new System.Drawing.Point(2, 316);
            this.btnTogglePPM.Margin = new System.Windows.Forms.Padding(2);
            this.btnTogglePPM.Name = "btnTogglePPM";
            this.btnTogglePPM.Size = new System.Drawing.Size(216, 56);
            this.btnTogglePPM.TabIndex = 5;
            this.btnTogglePPM.Text = "切换PPM/%";
            this.btnTogglePPM.UseVisualStyleBackColor = true;
            this.btnTogglePPM.Click += new System.EventHandler(this.btnTogglePPM_Click);
            // 
            // tblChooseKind
            // 
            this.tblChooseKind.ColumnCount = 3;
            this.tblChooseKind.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tblChooseKind.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblChooseKind.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tblChooseKind.Controls.Add(this.ckbNormalTest, 1, 0);
            this.tblChooseKind.Controls.Add(this.ckbAnalysis, 1, 1);
            this.tblChooseKind.Controls.Add(this.LabelTestModeState, 1, 2);
            this.tblChooseKind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblChooseKind.Location = new System.Drawing.Point(3, 234);
            this.tblChooseKind.Name = "tblChooseKind";
            this.tblChooseKind.RowCount = 3;
            this.tblChooseKind.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblChooseKind.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblChooseKind.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblChooseKind.Size = new System.Drawing.Size(215, 81);
            this.tblChooseKind.TabIndex = 78;
            // 
            // ckbNormalTest
            // 
            this.ckbNormalTest.AutoSize = true;
            this.ckbNormalTest.Checked = true;
            this.ckbNormalTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckbNormalTest.Font = new System.Drawing.Font("宋体", 10.8F);
            this.ckbNormalTest.Location = new System.Drawing.Point(36, 3);
            this.ckbNormalTest.Name = "ckbNormalTest";
            this.ckbNormalTest.Size = new System.Drawing.Size(143, 22);
            this.ckbNormalTest.TabIndex = 0;
            this.ckbNormalTest.TabStop = true;
            this.ckbNormalTest.Text = "正常测试";
            this.ckbNormalTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbNormalTest.UseVisualStyleBackColor = true;
            this.ckbNormalTest.Visible = false;
            this.ckbNormalTest.CheckedChanged += new System.EventHandler(this.ckbNormalTest_CheckedChanged);
            // 
            // ckbAnalysis
            // 
            this.ckbAnalysis.AutoSize = true;
            this.ckbAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ckbAnalysis.Font = new System.Drawing.Font("宋体", 10.8F);
            this.ckbAnalysis.Location = new System.Drawing.Point(36, 31);
            this.ckbAnalysis.Name = "ckbAnalysis";
            this.ckbAnalysis.Size = new System.Drawing.Size(143, 22);
            this.ckbAnalysis.TabIndex = 0;
            this.ckbAnalysis.TabStop = true;
            this.ckbAnalysis.Text = "质量分析";
            this.ckbAnalysis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckbAnalysis.UseVisualStyleBackColor = true;
            this.ckbAnalysis.Visible = false;
            this.ckbAnalysis.CheckedChanged += new System.EventHandler(this.ckbAnalysis_CheckedChanged);
            // 
            // LabelTestModeState
            // 
            this.LabelTestModeState.AutoSize = true;
            this.LabelTestModeState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelTestModeState.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabelTestModeState.Location = new System.Drawing.Point(36, 56);
            this.LabelTestModeState.Name = "LabelTestModeState";
            this.LabelTestModeState.Size = new System.Drawing.Size(143, 28);
            this.LabelTestModeState.TabIndex = 3;
            this.LabelTestModeState.Text = "当前模式:";
            this.LabelTestModeState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelTestModeState.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.DataGrid, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(400, 494);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // DataGrid
            // 
            this.DataGrid.DataSource = this.ultraDataSource1;
            this.DataGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.Header.VisiblePosition = 3;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            ultraGridColumn2.Header.VisiblePosition = 0;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(117, 0);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Width = 72;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance2;
            ultraGridColumn3.Header.VisiblePosition = 1;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(123, 0);
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.Width = 85;
            ultraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn4.Header.VisiblePosition = 2;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(504, 0);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 355;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            ultraGridBand1.Override.HeaderAppearance = appearance3;
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            ultraGridColumn5.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn5.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            ultraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn5.CellAppearance = appearance4;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Width = 103;
            ultraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance5.TextHAlignAsString = "Center";
            ultraGridColumn6.CellAppearance = appearance5;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Width = 118;
            ultraGridColumn7.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
            ultraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance6.TextHAlignAsString = "Right";
            ultraGridColumn7.CellAppearance = appearance6;
            ultraGridColumn7.Header.VisiblePosition = 2;
            ultraGridColumn7.Width = 129;
            ultraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance7.TextHAlignAsString = "Right";
            ultraGridColumn8.CellAppearance = appearance7;
            ultraGridColumn8.Header.VisiblePosition = 3;
            ultraGridColumn8.Width = 121;
            ultraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance8.TextHAlignAsString = "Center";
            ultraGridColumn9.CellAppearance = appearance8;
            ultraGridColumn9.Header.VisiblePosition = 4;
            ultraGridColumn9.Width = 100;
            ultraGridColumn10.Header.VisiblePosition = 5;
            ultraGridColumn11.Header.VisiblePosition = 6;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11});
            appearance9.BackColor = System.Drawing.Color.LemonChiffon;
            ultraGridBand2.Override.HeaderAppearance = appearance9;
            ultraGridBand2.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            this.DataGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.DataGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            appearance10.FontData.SizeInPoints = 16F;
            appearance10.ForeColor = System.Drawing.Color.Blue;
            this.DataGrid.DisplayLayout.CaptionAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.DataGrid.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.DataGrid.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ExtendFirstColumn;
            this.DataGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.DataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGrid.Location = new System.Drawing.Point(2, 2);
            this.DataGrid.Margin = new System.Windows.Forms.Padding(2);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(376, 489);
            this.DataGrid.TabIndex = 0;
            this.DataGrid.Text = "测试结果";
            this.DataGrid.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.ChildBands.AddRange(new object[] {
            ultraDataBand1});
            // 
            // dgvTestPowerOn
            // 
            this.dgvTestPowerOn.AllowUserToAddRows = false;
            this.dgvTestPowerOn.AllowUserToDeleteRows = false;
            this.dgvTestPowerOn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTestPowerOn.BackgroundColor = System.Drawing.Color.OldLace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestPowerOn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTestPowerOn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTestPowerOn.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColRes});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTestPowerOn.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTestPowerOn.Location = new System.Drawing.Point(3, 99);
            this.dgvTestPowerOn.Name = "dgvTestPowerOn";
            this.dgvTestPowerOn.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestPowerOn.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTestPowerOn.RowHeadersVisible = false;
            this.tableLayoutPanel1.SetRowSpan(this.dgvTestPowerOn, 2);
            this.dgvTestPowerOn.RowTemplate.Height = 23;
            this.dgvTestPowerOn.Size = new System.Drawing.Size(223, 572);
            this.dgvTestPowerOn.TabIndex = 12;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColName.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColRes
            // 
            this.ColRes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColRes.HeaderText = "点检结果";
            this.ColRes.Name = "ColRes";
            this.ColRes.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 90);
            this.panel1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.LawnGreen;
            this.label1.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(19, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 77);
            this.label1.TabIndex = 4;
            this.label1.Text = "点检";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SubStepBindingSource
            // 
            this.SubStepBindingSource.DataSource = typeof(Test.ProjectTest.SubStep);
            // 
            // frmTest_Wide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(862, 696);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmTest_Wide";
            this.Text = "Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTest_Wide_FormClosing);
            this.Load += new System.EventHandler(this.frmTestNew_Load);
            this.Shown += new System.EventHandler(this.frmTest_Wide_Shown);
            this.statusInfo.ResumeLayout(false);
            this.statusInfo.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.btnClearCounter.ResumeLayout(false);
            this.btnClearCounter.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tblChooseKind.ResumeLayout(false);
            this.tblChooseKind.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestPowerOn)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SubStepBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusInfo;
        private System.Windows.Forms.ToolStripStatusLabel statusUser;
        private System.Windows.Forms.ToolStripProgressBar Progress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private System.Windows.Forms.BindingSource SubStepBindingSource;
        private Infragistics.Win.UltraWinGrid.UltraGrid DataGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblTestTime;
        private System.Windows.Forms.Button btnTogglePPM;
        private System.Windows.Forms.GroupBox btnClearCounter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label lblFail;
        private System.Windows.Forms.Label lblPPM;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnClearCount;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.CheckBox chkBreakOnError;
        private System.Windows.Forms.TableLayoutPanel tblChooseKind;
        private System.Windows.Forms.RadioButton ckbNormalTest;
        private System.Windows.Forms.RadioButton ckbAnalysis;
        //private System.Windows.Forms.Button button2;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LabelTestModeState;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvTestPowerOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRes;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label1;
    }
}