namespace Test.ProjectFileEditor
{
    partial class frmMutilTestConfig
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Step1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Step2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Step3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Step4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Step5");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Step6");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("节点0");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("节点2");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("节点4");
            this.btnOK = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mTsTestStepList = new MultiSelectTreeView.MultiSelectTree3(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mTsThreadList = new MultiSelectTreeView.MultiSelectTree2(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkIsMutiEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(268, 300);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(339, 300);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(65, 23);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.Text = "返回";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(126, 300);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加线程";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(197, 300);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(65, 23);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mTsTestStepList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 279);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试大步：";
            // 
            // mTsTestStepList
            // 
            this.mTsTestStepList.AllowDrop = true;
            this.mTsTestStepList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTsTestStepList.Location = new System.Drawing.Point(3, 17);
            this.mTsTestStepList.MultiSelect = false;
            this.mTsTestStepList.Name = "mTsTestStepList";
            treeNode1.Name = "节点0";
            treeNode1.Text = "Step1";
            treeNode2.Name = "节点1";
            treeNode2.Text = "Step2";
            treeNode3.Name = "节点2";
            treeNode3.Text = "Step3";
            treeNode4.Name = "节点3";
            treeNode4.Text = "Step4";
            treeNode5.Name = "节点4";
            treeNode5.Text = "Step5";
            treeNode6.Name = "节点5";
            treeNode6.Text = "Step6";
            this.mTsTestStepList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            this.mTsTestStepList.Size = new System.Drawing.Size(185, 259);
            this.mTsTestStepList.TabIndex = 9;
            this.mTsTestStepList.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.mTsTestStepList_NodeMouseHover);
            this.mTsTestStepList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mTsTestStepList_NodeMouseClick);
            this.mTsTestStepList.MouseLeave += new System.EventHandler(this.mTsTestStepList_MouseLeave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mTsThreadList);
            this.groupBox2.Location = new System.Drawing.Point(209, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 276);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "线程：";
            // 
            // mTsThreadList
            // 
            this.mTsThreadList.AllowDrop = true;
            this.mTsThreadList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTsThreadList.LabelEdit = true;
            this.mTsThreadList.Location = new System.Drawing.Point(3, 17);
            this.mTsThreadList.MultiSelect = false;
            this.mTsThreadList.Name = "mTsThreadList";
            treeNode7.Name = "节点0";
            treeNode7.Text = "节点0";
            treeNode8.Name = "节点1";
            treeNode8.Text = "节点1";
            treeNode9.Name = "节点2";
            treeNode9.Text = "节点2";
            treeNode10.Name = "节点3";
            treeNode10.Text = "节点3";
            treeNode11.Name = "节点4";
            treeNode11.Text = "节点4";
            this.mTsThreadList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            this.mTsThreadList.Size = new System.Drawing.Size(189, 256);
            this.mTsThreadList.TabIndex = 0;
            this.mTsThreadList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mTsThreadList_NodeMouseClick);
            this.mTsThreadList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mTsThreadList_NodeMouseDoubleClick);
            // 
            // chkIsMutiEnable
            // 
            this.chkIsMutiEnable.AutoSize = true;
            this.chkIsMutiEnable.Location = new System.Drawing.Point(12, 304);
            this.chkIsMutiEnable.Name = "chkIsMutiEnable";
            this.chkIsMutiEnable.Size = new System.Drawing.Size(108, 16);
            this.chkIsMutiEnable.TabIndex = 9;
            this.chkIsMutiEnable.Text = "是否启用多线程";
            this.chkIsMutiEnable.UseVisualStyleBackColor = true;
            // 
            // frmMutilTestConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 327);
            this.Controls.Add(this.chkIsMutiEnable);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnOK);
            this.Name = "frmMutilTestConfig";
            this.Text = "多线程配置界面";
            this.Load += new System.EventHandler(this.frmMutilTestConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private MultiSelectTreeView.MultiSelectTree2 mTsThreadList;
        private MultiSelectTreeView.MultiSelectTree3 mTsTestStepList;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkIsMutiEnable;
    }
}