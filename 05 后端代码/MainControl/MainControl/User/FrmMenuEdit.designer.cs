namespace MainControl
{
    partial class FrmMenuEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenuEdit));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_parent = new System.Windows.Forms.TextBox();
            this.btn_concel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_Sort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pic_menu = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_MenuTag = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_MenuName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menu)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_parent);
            this.panel1.Controls.Add(this.btn_concel);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.txt_Sort);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.pic_menu);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_MenuTag);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txt_MenuName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 282);
            this.panel1.TabIndex = 0;
            // 
            // txt_parent
            // 
            this.txt_parent.Enabled = false;
            this.txt_parent.Location = new System.Drawing.Point(110, 119);
            this.txt_parent.Name = "txt_parent";
            this.txt_parent.Size = new System.Drawing.Size(159, 21);
            this.txt_parent.TabIndex = 28;
            // 
            // btn_concel
            // 
            this.btn_concel.Image = global::MainControl.Properties.Resources.delete;
            this.btn_concel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_concel.Location = new System.Drawing.Point(156, 238);
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
            this.btn_save.Location = new System.Drawing.Point(67, 238);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(83, 32);
            this.btn_save.TabIndex = 26;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_Sort
            // 
            this.txt_Sort.Location = new System.Drawing.Point(113, 204);
            this.txt_Sort.Name = "txt_Sort";
            this.txt_Sort.Size = new System.Drawing.Size(159, 21);
            this.txt_Sort.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "排序顺序：";
            // 
            // pic_menu
            // 
            this.pic_menu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pic_menu.Image = ((System.Drawing.Image)(resources.GetObject("pic_menu.Image")));
            this.pic_menu.Location = new System.Drawing.Point(113, 156);
            this.pic_menu.Name = "pic_menu";
            this.pic_menu.Size = new System.Drawing.Size(42, 39);
            this.pic_menu.TabIndex = 7;
            this.pic_menu.TabStop = false;
            this.pic_menu.Tag = "1";
            this.pic_menu.Click += new System.EventHandler(this.pic_menu_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "菜单图标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "节点位置：";
            // 
            // txt_MenuTag
            // 
            this.txt_MenuTag.Location = new System.Drawing.Point(110, 77);
            this.txt_MenuTag.Name = "txt_MenuTag";
            this.txt_MenuTag.Size = new System.Drawing.Size(159, 21);
            this.txt_MenuTag.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "菜单标记：";
            // 
            // txt_MenuName
            // 
            this.txt_MenuName.Location = new System.Drawing.Point(110, 33);
            this.txt_MenuName.Name = "txt_MenuName";
            this.txt_MenuName.Size = new System.Drawing.Size(159, 21);
            this.txt_MenuName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "菜单名称：";
            // 
            // FrmMenuEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 282);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMenuEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "菜单编辑";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEdit_FormClosed);
            this.Load += new System.EventHandler(this.FrmMenuEdit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_MenuName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_MenuTag;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Sort;
        public System.Windows.Forms.PictureBox pic_menu;
        private System.Windows.Forms.Button btn_concel;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_parent;
    }
}