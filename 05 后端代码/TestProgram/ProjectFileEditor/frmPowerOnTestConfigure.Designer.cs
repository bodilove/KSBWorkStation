namespace Test.ProjectFileEditor
{
    partial class frmPowerOnTestConfigure
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.GridView = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColIsTest = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColCpm = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColULimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TreePowerOnTest = new System.Windows.Forms.TreeView();
            this.btnAddPowerOnTest = new System.Windows.Forms.Button();
            this.btnRemovePowerOnTest = new System.Windows.Forms.Button();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.diagSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtSN);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.GridView);
            this.groupBox2.Location = new System.Drawing.Point(203, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(791, 397);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "开机检测：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(314, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "标准件SN号：";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(397, 26);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(227, 21);
            this.txtSN.TabIndex = 3;
            this.txtSN.Leave += new System.EventHandler(this.txtSN_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "开机测试序列名称：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(142, 26);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(144, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
            // 
            // GridView
            // 
            this.GridView.AllowUserToAddRows = false;
            this.GridView.AllowUserToDeleteRows = false;
            this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColChecked,
            this.ColIsTest,
            this.ColCpm,
            this.ColULimit,
            this.ColLLimit,
            this.ColNom,
            this.ColDescription});
            this.GridView.Location = new System.Drawing.Point(6, 75);
            this.GridView.Name = "GridView";
            this.GridView.RowTemplate.Height = 23;
            this.GridView.Size = new System.Drawing.Size(779, 305);
            this.GridView.TabIndex = 0;
            this.GridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellClick);
            this.GridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellDoubleClick);
            this.GridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridView_CellEndEdit);
            this.GridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridView_CellMouseDoubleClick);
            // 
            // ColName
            // 
            this.ColName.HeaderText = "测试步名称";
            this.ColName.Name = "ColName";
            // 
            // ColChecked
            // 
            this.ColChecked.HeaderText = "关注项";
            this.ColChecked.Name = "ColChecked";
            this.ColChecked.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColChecked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColIsTest
            // 
            this.ColIsTest.HeaderText = "是否测试";
            this.ColIsTest.Name = "ColIsTest";
            this.ColIsTest.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColIsTest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColCpm
            // 
            this.ColCpm.HeaderText = "比较模式";
            this.ColCpm.Name = "ColCpm";
            this.ColCpm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColCpm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColULimit
            // 
            this.ColULimit.HeaderText = "上限";
            this.ColULimit.Name = "ColULimit";
            // 
            // ColLLimit
            // 
            this.ColLLimit.HeaderText = "下限";
            this.ColLLimit.Name = "ColLLimit";
            // 
            // ColNom
            // 
            this.ColNom.HeaderText = "标准值";
            this.ColNom.Name = "ColNom";
            // 
            // ColDescription
            // 
            this.ColDescription.HeaderText = "描述";
            this.ColDescription.Name = "ColDescription";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(919, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(838, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TreePowerOnTest);
            this.groupBox1.Controls.Add(this.btnAddPowerOnTest);
            this.groupBox1.Controls.Add(this.btnRemovePowerOnTest);
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 397);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "序列选择：";
            // 
            // TreePowerOnTest
            // 
            this.TreePowerOnTest.Location = new System.Drawing.Point(15, 20);
            this.TreePowerOnTest.Name = "TreePowerOnTest";
            this.TreePowerOnTest.Size = new System.Drawing.Size(157, 329);
            this.TreePowerOnTest.TabIndex = 3;
            this.TreePowerOnTest.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreePowerOnTest_AfterSelect);
            // 
            // btnAddPowerOnTest
            // 
            this.btnAddPowerOnTest.Location = new System.Drawing.Point(38, 355);
            this.btnAddPowerOnTest.Name = "btnAddPowerOnTest";
            this.btnAddPowerOnTest.Size = new System.Drawing.Size(64, 25);
            this.btnAddPowerOnTest.TabIndex = 1;
            this.btnAddPowerOnTest.Text = "增加";
            this.btnAddPowerOnTest.UseVisualStyleBackColor = true;
            this.btnAddPowerOnTest.Click += new System.EventHandler(this.btnAddPowerOnTest_Click);
            // 
            // btnRemovePowerOnTest
            // 
            this.btnRemovePowerOnTest.Location = new System.Drawing.Point(108, 355);
            this.btnRemovePowerOnTest.Name = "btnRemovePowerOnTest";
            this.btnRemovePowerOnTest.Size = new System.Drawing.Size(64, 25);
            this.btnRemovePowerOnTest.TabIndex = 2;
            this.btnRemovePowerOnTest.Text = "删除";
            this.btnRemovePowerOnTest.UseVisualStyleBackColor = true;
            this.btnRemovePowerOnTest.Click += new System.EventHandler(this.btnRemovePowerOnTest_Click);
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.HeaderText = "比较模式";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // frmPowerOnTestConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 461);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPowerOnTestConfigure";
            this.Load += new System.EventHandler(this.frmPowerOnTestConfigure_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridView GridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColChecked;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsTest;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColCpm;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColULimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView TreePowerOnTest;
        private System.Windows.Forms.Button btnAddPowerOnTest;
        private System.Windows.Forms.Button btnRemovePowerOnTest;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.SaveFileDialog diagSaveFile;
    }
}