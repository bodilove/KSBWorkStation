namespace MainControl
{
    partial class FrmToolHmiEdit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_StationNum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_parent = new System.Windows.Forms.TextBox();
            this.btn_concel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_Sort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_ProcessName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Unit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Llimit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Ulimit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_KeyCodeFormat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chk_IsKeyCode = new System.Windows.Forms.CheckBox();
            this.com_Conditional = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.com_Conditional);
            this.panel1.Controls.Add(this.chk_IsKeyCode);
            this.panel1.Controls.Add(this.txt_KeyCodeFormat);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txt_Ulimit);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txt_Llimit);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txt_Unit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.txt_StationNum);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_parent);
            this.panel1.Controls.Add(this.btn_concel);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.txt_Sort);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_ProcessName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 452);
            this.panel1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(103, 318);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(166, 71);
            this.richTextBox1.TabIndex = 32;
            this.richTextBox1.Text = "";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 335);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 31;
            this.label12.Text = "备注：";
            // 
            // txt_StationNum
            // 
            this.txt_StationNum.Enabled = false;
            this.txt_StationNum.Location = new System.Drawing.Point(110, 18);
            this.txt_StationNum.Name = "txt_StationNum";
            this.txt_StationNum.Size = new System.Drawing.Size(159, 21);
            this.txt_StationNum.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "工位工序：";
            // 
            // txt_parent
            // 
            this.txt_parent.Enabled = false;
            this.txt_parent.Location = new System.Drawing.Point(110, 85);
            this.txt_parent.Name = "txt_parent";
            this.txt_parent.Size = new System.Drawing.Size(159, 21);
            this.txt_parent.TabIndex = 28;
            // 
            // btn_concel
            // 
            this.btn_concel.Image = global::MainControl.Properties.Resources.delete;
            this.btn_concel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_concel.Location = new System.Drawing.Point(162, 409);
            this.btn_concel.Name = "btn_concel";
            this.btn_concel.Size = new System.Drawing.Size(83, 32);
            this.btn_concel.TabIndex = 27;
            this.btn_concel.Text = "关闭";
            this.btn_concel.UseVisualStyleBackColor = true;
            this.btn_concel.Click += new System.EventHandler(this.btn_concel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Image = global::MainControl.Properties.Resources.save;
            this.btn_save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_save.Location = new System.Drawing.Point(73, 409);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(83, 32);
            this.btn_save.TabIndex = 26;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_Sort
            // 
            this.txt_Sort.Location = new System.Drawing.Point(110, 284);
            this.txt_Sort.Name = "txt_Sort";
            this.txt_Sort.Size = new System.Drawing.Size(159, 21);
            this.txt_Sort.TabIndex = 9;
            this.txt_Sort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Sort_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "排序顺序：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "节点位置：";
            // 
            // txt_ProcessName
            // 
            this.txt_ProcessName.Location = new System.Drawing.Point(110, 51);
            this.txt_ProcessName.Name = "txt_ProcessName";
            this.txt_ProcessName.Size = new System.Drawing.Size(159, 21);
            this.txt_ProcessName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工艺名称：";
            // 
            // txt_Unit
            // 
            this.txt_Unit.Location = new System.Drawing.Point(110, 248);
            this.txt_Unit.Name = "txt_Unit";
            this.txt_Unit.Size = new System.Drawing.Size(159, 21);
            this.txt_Unit.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "单位：";
            // 
            // txt_Llimit
            // 
            this.txt_Llimit.Location = new System.Drawing.Point(110, 214);
            this.txt_Llimit.Name = "txt_Llimit";
            this.txt_Llimit.Size = new System.Drawing.Size(159, 21);
            this.txt_Llimit.TabIndex = 36;
            this.txt_Llimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Llimit_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "下限值：";
            // 
            // txt_Ulimit
            // 
            this.txt_Ulimit.Location = new System.Drawing.Point(110, 181);
            this.txt_Ulimit.Name = "txt_Ulimit";
            this.txt_Ulimit.Size = new System.Drawing.Size(159, 21);
            this.txt_Ulimit.TabIndex = 38;
            this.txt_Ulimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Ulimit_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "下限值：";
            // 
            // txt_KeyCodeFormat
            // 
            this.txt_KeyCodeFormat.Enabled = false;
            this.txt_KeyCodeFormat.Location = new System.Drawing.Point(110, 152);
            this.txt_KeyCodeFormat.Name = "txt_KeyCodeFormat";
            this.txt_KeyCodeFormat.Size = new System.Drawing.Size(159, 21);
            this.txt_KeyCodeFormat.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 41;
            this.label8.Text = "零件码格式：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(39, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "条件选择：";
            // 
            // chk_IsKeyCode
            // 
            this.chk_IsKeyCode.AutoSize = true;
            this.chk_IsKeyCode.Location = new System.Drawing.Point(275, 156);
            this.chk_IsKeyCode.Name = "chk_IsKeyCode";
            this.chk_IsKeyCode.Size = new System.Drawing.Size(15, 14);
            this.chk_IsKeyCode.TabIndex = 43;
            this.chk_IsKeyCode.UseVisualStyleBackColor = true;
            this.chk_IsKeyCode.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // com_Conditional
            // 
            this.com_Conditional.FormattingEnabled = true;
            this.com_Conditional.Location = new System.Drawing.Point(110, 117);
            this.com_Conditional.Name = "com_Conditional";
            this.com_Conditional.Size = new System.Drawing.Size(159, 20);
            this.com_Conditional.TabIndex = 44;
            // 
            // FrmToolHmiEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 452);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmToolHmiEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "菜单编辑";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEdit_FormClosed);
            this.Load += new System.EventHandler(this.FrmMenuEdit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_ProcessName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Sort;
        private System.Windows.Forms.Button btn_concel;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_parent;
        private System.Windows.Forms.TextBox txt_StationNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_Ulimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_Llimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Unit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_KeyCodeFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chk_IsKeyCode;
        private System.Windows.Forms.ComboBox com_Conditional;
    }
}