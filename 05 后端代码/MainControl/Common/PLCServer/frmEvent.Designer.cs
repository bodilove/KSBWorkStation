namespace Common.PLCServer
{
    partial class frmEvent
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
            this.dgveventTask = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCanncel = new System.Windows.Forms.Button();
            this.ColStationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgveventTask)).BeginInit();
            this.SuspendLayout();
            // 
            // dgveventTask
            // 
            this.dgveventTask.AllowUserToAddRows = false;
            this.dgveventTask.AllowUserToDeleteRows = false;
            this.dgveventTask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgveventTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgveventTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColStationID,
            this.colStationName,
            this.colEventId,
            this.colEventName});
            this.dgveventTask.Location = new System.Drawing.Point(2, 2);
            this.dgveventTask.Name = "dgveventTask";
            this.dgveventTask.ReadOnly = true;
            this.dgveventTask.RowHeadersVisible = false;
            this.dgveventTask.RowTemplate.Height = 30;
            this.dgveventTask.Size = new System.Drawing.Size(625, 329);
            this.dgveventTask.TabIndex = 23;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(440, 361);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 36);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCanncel
            // 
            this.btnCanncel.Location = new System.Drawing.Point(536, 361);
            this.btnCanncel.Name = "btnCanncel";
            this.btnCanncel.Size = new System.Drawing.Size(80, 36);
            this.btnCanncel.TabIndex = 25;
            this.btnCanncel.Text = "返回";
            this.btnCanncel.UseVisualStyleBackColor = true;
            this.btnCanncel.Click += new System.EventHandler(this.btnCanncel_Click);
            // 
            // ColStationID
            // 
            this.ColStationID.HeaderText = "站ID";
            this.ColStationID.Name = "ColStationID";
            this.ColStationID.ReadOnly = true;
            this.ColStationID.Width = 80;
            // 
            // colStationName
            // 
            this.colStationName.HeaderText = "站名";
            this.colStationName.Name = "colStationName";
            this.colStationName.ReadOnly = true;
            // 
            // colEventId
            // 
            this.colEventId.HeaderText = "事件ID";
            this.colEventId.Name = "colEventId";
            this.colEventId.ReadOnly = true;
            // 
            // colEventName
            // 
            this.colEventName.HeaderText = "名称";
            this.colEventName.Name = "colEventName";
            this.colEventName.ReadOnly = true;
            this.colEventName.Width = 150;
            // 
            // frmEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 409);
            this.Controls.Add(this.btnCanncel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgveventTask);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(650, 465);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 465);
            this.Name = "frmEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正在运行的事件";
            this.Load += new System.EventHandler(this.frmEvent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgveventTask)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgveventTask;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCanncel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventName;
    }
}