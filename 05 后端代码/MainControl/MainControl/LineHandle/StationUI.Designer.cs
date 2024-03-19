namespace MainControl
{
    partial class StationUI
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUUTId_SN = new System.Windows.Forms.Label();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.dgvBom = new System.Windows.Forms.DataGridView();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.lblIsByPass = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblStationName = new System.Windows.Forms.Label();
            this.lblStationNum = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.dgvTest = new System.Windows.Forms.DataGridView();
            this.ColTestNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExceptValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTestValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.ColItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColScannerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBom)).BeginInit();
            this.groupBox27.SuspendLayout();
            this.groupBox28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).BeginInit();
            this.groupBox30.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 456F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblUUTId_SN, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox29, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox27, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox28, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox30, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1012, 381);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblUUTId_SN
            // 
            this.lblUUTId_SN.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblUUTId_SN, 3);
            this.lblUUTId_SN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUUTId_SN.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUUTId_SN.Location = new System.Drawing.Point(2, 0);
            this.lblUUTId_SN.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUUTId_SN.Name = "lblUUTId_SN";
            this.lblUUTId_SN.Size = new System.Drawing.Size(1008, 47);
            this.lblUUTId_SN.TabIndex = 11;
            this.lblUUTId_SN.Text = "UUTId：????????    SN:?????????????????????????????";
            this.lblUUTId_SN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox29
            // 
            this.groupBox29.Controls.Add(this.dgvBom);
            this.groupBox29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox29.Location = new System.Drawing.Point(177, 49);
            this.groupBox29.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox29.Size = new System.Drawing.Size(452, 163);
            this.groupBox29.TabIndex = 3;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "BOM列表：";
            // 
            // dgvBom
            // 
            this.dgvBom.AllowUserToAddRows = false;
            this.dgvBom.AllowUserToDeleteRows = false;
            this.dgvBom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBom.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColItemName,
            this.colPartNo,
            this.ColScannerCode});
            this.dgvBom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBom.Location = new System.Drawing.Point(2, 16);
            this.dgvBom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvBom.Name = "dgvBom";
            this.dgvBom.ReadOnly = true;
            this.dgvBom.RowHeadersVisible = false;
            this.dgvBom.RowTemplate.Height = 30;
            this.dgvBom.Size = new System.Drawing.Size(448, 145);
            this.dgvBom.TabIndex = 0;
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.lblIsByPass);
            this.groupBox27.Controls.Add(this.label14);
            this.groupBox27.Controls.Add(this.lblStationName);
            this.groupBox27.Controls.Add(this.lblStationNum);
            this.groupBox27.Controls.Add(this.label12);
            this.groupBox27.Controls.Add(this.label9);
            this.groupBox27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox27.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox27.Location = new System.Drawing.Point(2, 49);
            this.groupBox27.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.SetRowSpan(this.groupBox27, 2);
            this.groupBox27.Size = new System.Drawing.Size(171, 330);
            this.groupBox27.TabIndex = 0;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "工站状态：";
            // 
            // lblIsByPass
            // 
            this.lblIsByPass.AutoSize = true;
            this.lblIsByPass.Location = new System.Drawing.Point(78, 73);
            this.lblIsByPass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIsByPass.Name = "lblIsByPass";
            this.lblIsByPass.Size = new System.Drawing.Size(51, 20);
            this.lblIsByPass.TabIndex = 9;
            this.lblIsByPass.Text = "？？？";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 73);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(79, 20);
            this.label14.TabIndex = 8;
            this.label14.Text = "启用状态：";
            // 
            // lblStationName
            // 
            this.lblStationName.AutoSize = true;
            this.lblStationName.Location = new System.Drawing.Point(78, 51);
            this.lblStationName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStationName.Name = "lblStationName";
            this.lblStationName.Size = new System.Drawing.Size(51, 20);
            this.lblStationName.TabIndex = 7;
            this.lblStationName.Text = "？？？";
            // 
            // lblStationNum
            // 
            this.lblStationNum.AutoSize = true;
            this.lblStationNum.Location = new System.Drawing.Point(78, 28);
            this.lblStationNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStationNum.Name = "lblStationNum";
            this.lblStationNum.Size = new System.Drawing.Size(51, 20);
            this.lblStationNum.TabIndex = 6;
            this.lblStationNum.Text = "？？？";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 51);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 20);
            this.label12.TabIndex = 5;
            this.label12.Text = "工站名称：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 28);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 20);
            this.label9.TabIndex = 4;
            this.label9.Text = "工站号：";
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.dgvTest);
            this.groupBox28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox28.Location = new System.Drawing.Point(633, 49);
            this.groupBox28.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox28.Size = new System.Drawing.Size(377, 163);
            this.groupBox28.TabIndex = 2;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "测试列表：";
            // 
            // dgvTest
            // 
            this.dgvTest.AllowUserToAddRows = false;
            this.dgvTest.AllowUserToDeleteRows = false;
            this.dgvTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTestNum,
            this.ColTestName,
            this.ColExceptValue,
            this.ColTestValue,
            this.ColResult,
            this.ColDescription});
            this.dgvTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTest.Location = new System.Drawing.Point(2, 16);
            this.dgvTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvTest.Name = "dgvTest";
            this.dgvTest.ReadOnly = true;
            this.dgvTest.RowHeadersVisible = false;
            this.dgvTest.RowTemplate.Height = 30;
            this.dgvTest.Size = new System.Drawing.Size(373, 145);
            this.dgvTest.TabIndex = 0;
            // 
            // ColTestNum
            // 
            this.ColTestNum.HeaderText = "标号";
            this.ColTestNum.Name = "ColTestNum";
            this.ColTestNum.ReadOnly = true;
            this.ColTestNum.Width = 80;
            // 
            // ColTestName
            // 
            this.ColTestName.HeaderText = "测试名称";
            this.ColTestName.Name = "ColTestName";
            this.ColTestName.ReadOnly = true;
            // 
            // ColExceptValue
            // 
            this.ColExceptValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColExceptValue.HeaderText = "设计值";
            this.ColExceptValue.Name = "ColExceptValue";
            this.ColExceptValue.ReadOnly = true;
            // 
            // ColTestValue
            // 
            this.ColTestValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColTestValue.HeaderText = "实测值";
            this.ColTestValue.Name = "ColTestValue";
            this.ColTestValue.ReadOnly = true;
            // 
            // ColResult
            // 
            this.ColResult.HeaderText = "结果";
            this.ColResult.Name = "ColResult";
            this.ColResult.ReadOnly = true;
            this.ColResult.Width = 80;
            // 
            // ColDescription
            // 
            this.ColDescription.HeaderText = "描述";
            this.ColDescription.Name = "ColDescription";
            this.ColDescription.ReadOnly = true;
            this.ColDescription.Width = 130;
            // 
            // groupBox30
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox30, 2);
            this.groupBox30.Controls.Add(this.txtLog);
            this.groupBox30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox30.Location = new System.Drawing.Point(177, 216);
            this.groupBox30.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox30.Size = new System.Drawing.Size(833, 163);
            this.groupBox30.TabIndex = 5;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "日志：";
            // 
            // txtLog
            // 
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(2, 16);
            this.txtLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(829, 145);
            this.txtLog.TabIndex = 1;
            this.txtLog.Text = "";
            // 
            // ColItemName
            // 
            this.ColItemName.HeaderText = "名称";
            this.ColItemName.Name = "ColItemName";
            this.ColItemName.ReadOnly = true;
            this.ColItemName.Width = 120;
            // 
            // colPartNo
            // 
            this.colPartNo.HeaderText = "零件号";
            this.colPartNo.Name = "colPartNo";
            this.colPartNo.ReadOnly = true;
            this.colPartNo.Width = 150;
            // 
            // ColScannerCode
            // 
            this.ColScannerCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColScannerCode.HeaderText = "条码值";
            this.ColScannerCode.Name = "ColScannerCode";
            this.ColScannerCode.ReadOnly = true;
            // 
            // StationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "StationUI";
            this.Size = new System.Drawing.Size(1012, 381);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox29.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBom)).EndInit();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            this.groupBox28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).EndInit();
            this.groupBox30.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.DataGridView dgvBom;
        private System.Windows.Forms.GroupBox groupBox27;
        public System.Windows.Forms.Label lblIsByPass;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.Label lblStationName;
        public System.Windows.Forms.Label lblStationNum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.DataGridView dgvTest;
        private System.Windows.Forms.GroupBox groupBox30;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExceptValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTestValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.Label lblUUTId_SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColScannerCode;
    }
}
