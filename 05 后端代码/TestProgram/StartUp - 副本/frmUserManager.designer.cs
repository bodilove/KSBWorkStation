namespace StartUp
{
    partial class frmUserManager
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("所有组");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserManager));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("记录查询", 2);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("构建测试", 3);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("用户管理", 1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("测试产品", 0);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("标签设置", 2);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Types = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.AddTypes = new System.Windows.Forms.ToolStripButton();
            this.Deltype = new System.Windows.Forms.ToolStripButton();
            this.RenameType = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.Adduser = new System.Windows.Forms.ToolStripButton();
            this.deluser = new System.Windows.Forms.ToolStripButton();
            this.SetType = new System.Windows.Forms.ToolStripSplitButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Rights = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Types);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(858, 383);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 0;
            // 
            // Types
            // 
            this.Types.AllowDrop = true;
            this.Types.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Types.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Types.HideSelection = false;
            this.Types.HotTracking = true;
            this.Types.Location = new System.Drawing.Point(0, 25);
            this.Types.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Types.Name = "Types";
            treeNode1.Name = "Types";
            treeNode1.Text = "所有组";
            this.Types.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.Types.Size = new System.Drawing.Size(210, 358);
            this.Types.TabIndex = 1;
            this.Types.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.Types_AfterLabelEdit);
            this.Types.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Types_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.AddTypes,
            this.Deltype,
            this.RenameType});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(210, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "组管理";
            // 
            // AddTypes
            // 
            this.AddTypes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddTypes.Image = ((System.Drawing.Image)(resources.GetObject("AddTypes.Image")));
            this.AddTypes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AddTypes.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.AddTypes.Name = "AddTypes";
            this.AddTypes.Size = new System.Drawing.Size(36, 22);
            this.AddTypes.Text = "添加";
            this.AddTypes.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Deltype
            // 
            this.Deltype.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Deltype.Image = ((System.Drawing.Image)(resources.GetObject("Deltype.Image")));
            this.Deltype.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.Deltype.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.Deltype.Name = "Deltype";
            this.Deltype.Size = new System.Drawing.Size(36, 22);
            this.Deltype.Text = "删除";
            this.Deltype.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // RenameType
            // 
            this.RenameType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RenameType.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.RenameType.Name = "RenameType";
            this.RenameType.Size = new System.Drawing.Size(48, 22);
            this.RenameType.Text = "重命名";
            this.RenameType.Click += new System.EventHandler(this.RenameType_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.userGrid);
            this.groupBox1.Controls.Add(this.toolStrip3);
            this.groupBox1.Controls.Add(this.Rights);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(644, 383);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "权限设置";
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.Adduser,
            this.deluser,
            this.SetType});
            this.toolStrip3.Location = new System.Drawing.Point(3, 100);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(638, 25);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel3.Text = "用户管理";
            // 
            // Adduser
            // 
            this.Adduser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Adduser.Image = ((System.Drawing.Image)(resources.GetObject("Adduser.Image")));
            this.Adduser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.Adduser.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.Adduser.Name = "Adduser";
            this.Adduser.Size = new System.Drawing.Size(36, 22);
            this.Adduser.Text = "添加";
            this.Adduser.ToolTipText = "添加用户";
            this.Adduser.Click += new System.EventHandler(this.Adduser_Click);
            // 
            // deluser
            // 
            this.deluser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deluser.Image = ((System.Drawing.Image)(resources.GetObject("deluser.Image")));
            this.deluser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deluser.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deluser.Name = "deluser";
            this.deluser.Size = new System.Drawing.Size(36, 22);
            this.deluser.Text = "删除";
            this.deluser.ToolTipText = "删除当前用户";
            this.deluser.Click += new System.EventHandler(this.deluser_Click);
            // 
            // SetType
            // 
            this.SetType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SetType.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.SetType.Name = "SetType";
            this.SetType.Size = new System.Drawing.Size(96, 22);
            this.SetType.Text = "指派到另一组";
            this.SetType.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.SetType.ButtonClick += new System.EventHandler(this.SetType_ButtonClick);
            this.SetType.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SetType_DropDownItemClicked);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "52design.com_kr_011.png");
            this.imageList2.Images.SetKeyName(1, "20071120134217968.png");
            this.imageList2.Images.SetKeyName(2, "png-0007.png");
            this.imageList2.Images.SetKeyName(3, "png-0014.png");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "52design.com_kr_012.png");
            this.imageList1.Images.SetKeyName(1, "20071120134217968.png");
            // 
            // Password
            // 
            this.Password.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Password.HeaderText = "登陆密码";
            this.Password.Name = "Password";
            this.Password.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Password.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // User
            // 
            this.User.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.User.HeaderText = "用户名";
            this.User.Name = "User";
            this.User.Width = 66;
            // 
            // userGrid
            // 
            this.userGrid.AllowDrop = true;
            this.userGrid.AllowUserToAddRows = false;
            this.userGrid.AllowUserToDeleteRows = false;
            this.userGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.userGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.userGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.User,
            this.Type,
            this.Password});
            this.userGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGrid.Location = new System.Drawing.Point(3, 125);
            this.userGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userGrid.Name = "userGrid";
            this.userGrid.RowHeadersVisible = false;
            this.userGrid.RowTemplate.Height = 23;
            this.userGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.userGrid.Size = new System.Drawing.Size(638, 256);
            this.userGrid.TabIndex = 2;
            this.userGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.userGrid_CellEndEdit);
            this.userGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.userGrid_CellLeave);
            this.userGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.userGrid_CellValueChanged);
            this.userGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.userGrid_EditingControlShowing);
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewComboBoxColumn1.HeaderText = "组";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Width = 23;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Type.HeaderText = "组";
            this.Type.Name = "Type";
            this.Type.Width = 23;
            // 
            // Rights
            // 
            this.Rights.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Rights.CheckBoxes = true;
            this.Rights.Dock = System.Windows.Forms.DockStyle.Top;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            this.Rights.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.Rights.LargeImageList = this.imageList2;
            this.Rights.Location = new System.Drawing.Point(3, 16);
            this.Rights.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Rights.Name = "Rights";
            this.Rights.Size = new System.Drawing.Size(638, 84);
            this.Rights.TabIndex = 0;
            this.Rights.UseCompatibleStateImageBehavior = false;
            this.Rights.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Rights_ItemCheck);
            // 
            // frmUserManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 383);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户和组管理";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserSettings_FormClosed);
            this.Load += new System.EventHandler(this.UserSettings_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton AddTypes;
        private System.Windows.Forms.ToolStripButton Deltype;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.TreeView Types;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton Adduser;
        private System.Windows.Forms.ToolStripButton deluser;
        private System.Windows.Forms.ToolStripButton RenameType;
        private System.Windows.Forms.ToolStripSplitButton SetType;
        private System.Windows.Forms.DataGridView userGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.ListView Rights;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
    }
}