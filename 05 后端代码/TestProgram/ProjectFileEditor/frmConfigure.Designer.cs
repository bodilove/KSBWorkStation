namespace ProjectFileEditor
{
    partial class frmConfigure
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox_F = new System.Windows.Forms.ListBox();
            this.listBox_N = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_select_F = new System.Windows.Forms.Button();
            this.btn_remove_F = new System.Windows.Forms.Button();
            this.btn_remove_N = new System.Windows.Forms.Button();
            this.btn_select_N = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkselect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.D_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_Name = new System.Windows.Forms.ComboBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txt_addname = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(443, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "FailureSteps:";
            // 
            // listBox_F
            // 
            this.listBox_F.FormattingEnabled = true;
            this.listBox_F.ItemHeight = 12;
            this.listBox_F.Location = new System.Drawing.Point(445, 47);
            this.listBox_F.Name = "listBox_F";
            this.listBox_F.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_F.Size = new System.Drawing.Size(179, 124);
            this.listBox_F.TabIndex = 3;
            // 
            // listBox_N
            // 
            this.listBox_N.FormattingEnabled = true;
            this.listBox_N.ItemHeight = 12;
            this.listBox_N.Location = new System.Drawing.Point(447, 215);
            this.listBox_N.Name = "listBox_N";
            this.listBox_N.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_N.Size = new System.Drawing.Size(179, 124);
            this.listBox_N.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "NotTestSteps:";
            // 
            // btn_select_F
            // 
            this.btn_select_F.Enabled = false;
            this.btn_select_F.Location = new System.Drawing.Point(359, 47);
            this.btn_select_F.Name = "btn_select_F";
            this.btn_select_F.Size = new System.Drawing.Size(75, 23);
            this.btn_select_F.TabIndex = 6;
            this.btn_select_F.Text = "添加>>";
            this.btn_select_F.UseVisualStyleBackColor = true;
            this.btn_select_F.Click += new System.EventHandler(this.btn_select_F_Click);
            // 
            // btn_remove_F
            // 
            this.btn_remove_F.Enabled = false;
            this.btn_remove_F.Location = new System.Drawing.Point(359, 81);
            this.btn_remove_F.Name = "btn_remove_F";
            this.btn_remove_F.Size = new System.Drawing.Size(75, 23);
            this.btn_remove_F.TabIndex = 7;
            this.btn_remove_F.Text = "<<移除";
            this.btn_remove_F.UseVisualStyleBackColor = true;
            this.btn_remove_F.Click += new System.EventHandler(this.btn_remove_F_Click);
            // 
            // btn_remove_N
            // 
            this.btn_remove_N.Enabled = false;
            this.btn_remove_N.Location = new System.Drawing.Point(360, 246);
            this.btn_remove_N.Name = "btn_remove_N";
            this.btn_remove_N.Size = new System.Drawing.Size(75, 23);
            this.btn_remove_N.TabIndex = 9;
            this.btn_remove_N.Text = "<<移除";
            this.btn_remove_N.UseVisualStyleBackColor = true;
            this.btn_remove_N.Click += new System.EventHandler(this.btn_remove_N_Click);
            // 
            // btn_select_N
            // 
            this.btn_select_N.Enabled = false;
            this.btn_select_N.Location = new System.Drawing.Point(360, 214);
            this.btn_select_N.Name = "btn_select_N";
            this.btn_select_N.Size = new System.Drawing.Size(75, 23);
            this.btn_select_N.TabIndex = 8;
            this.btn_select_N.Text = "添加>>";
            this.btn_select_N.UseVisualStyleBackColor = true;
            this.btn_select_N.Click += new System.EventHandler(this.btn_select_N_Click);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(641, 258);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 35);
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "保 存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(641, 304);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(77, 35);
            this.btn_close.TabIndex = 11;
            this.btn_close.Text = "退 出";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Step:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkselect,
            this.D_Name,
            this.D_Description});
            this.dataGridView1.Location = new System.Drawing.Point(64, 49);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(284, 290);
            this.dataGridView1.TabIndex = 14;
            // 
            // checkselect
            // 
            this.checkselect.HeaderText = "选择";
            this.checkselect.Name = "checkselect";
            this.checkselect.Width = 50;
            // 
            // D_Name
            // 
            this.D_Name.FillWeight = 108.8435F;
            this.D_Name.HeaderText = "Name";
            this.D_Name.Name = "D_Name";
            this.D_Name.ReadOnly = true;
            this.D_Name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.D_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // D_Description
            // 
            this.D_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.D_Description.FillWeight = 91.15646F;
            this.D_Description.HeaderText = "Description";
            this.D_Description.Name = "D_Description";
            this.D_Description.ReadOnly = true;
            this.D_Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.D_Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // txt_Name
            // 
            this.txt_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txt_Name.FormattingEnabled = true;
            this.txt_Name.Location = new System.Drawing.Point(64, 17);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(116, 20);
            this.txt_Name.TabIndex = 15;
            this.txt_Name.SelectedIndexChanged += new System.EventHandler(this.txt_Name_SelectedIndexChanged);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(186, 15);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 16;
            this.btn_add.Text = "Add Name";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.txt_addname);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(64, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 75);
            this.panel1.TabIndex = 17;
            this.panel1.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(40, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 12);
            this.label6.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_addname
            // 
            this.txt_addname.Location = new System.Drawing.Point(43, 12);
            this.txt_addname.Name = "txt_addname";
            this.txt_addname.Size = new System.Drawing.Size(177, 21);
            this.txt_addname.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Name:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(267, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Remove Name";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(179, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(41, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 353);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.txt_Name);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_remove_N);
            this.Controls.Add(this.btn_select_N);
            this.Controls.Add(this.btn_remove_F);
            this.Controls.Add(this.btn_select_F);
            this.Controls.Add(this.listBox_N);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox_F);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置";
            this.Load += new System.EventHandler(this.frmConfigure_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_F;
        private System.Windows.Forms.ListBox listBox_N;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_select_F;
        private System.Windows.Forms.Button btn_remove_F;
        private System.Windows.Forms.Button btn_remove_N;
        private System.Windows.Forms.Button btn_select_N;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkselect;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_Description;
        private System.Windows.Forms.ComboBox txt_Name;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_addname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
    }
}