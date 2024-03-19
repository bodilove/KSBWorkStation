namespace Test.StartUp
{
    partial class frmStart
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDUTVer = new System.Windows.Forms.Label();
            this.lblDUTNumber = new System.Windows.Forms.Label();
            this.lblDUTName = new System.Windows.Forms.Label();
            this.lstTS = new System.Windows.Forms.ListBox();
            this.picTestPrj = new System.Windows.Forms.PictureBox();
            this.btnSeqDown = new System.Windows.Forms.Button();
            this.btnSeqUp = new System.Windows.Forms.Button();
            this.btnDelTestSeq = new System.Windows.Forms.Button();
            this.btnAddTestSeq = new System.Windows.Forms.Button();
            this.toolUser = new System.Windows.Forms.ToolStrip();
            this.lblUserName = new System.Windows.Forms.ToolStripLabel();
            this.txtUserName = new System.Windows.Forms.ToolStripTextBox();
            this.separator = new System.Windows.Forms.ToolStripSeparator();
            this.lblPassword = new System.Windows.Forms.ToolStripLabel();
            this.txtPassword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLogOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblInfo = new System.Windows.Forms.ToolStripLabel();
            this.menuUser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChangePwd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHardware = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUserManage = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnRunTest = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHWDebug = new System.Windows.Forms.Button();
            this.btnChangePSW = new System.Windows.Forms.Button();
            this.btnUserManage = new System.Windows.Forms.Button();
            this.btnEditTest = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lb_serverstate = new System.Windows.Forms.Label();
            this.lb_plcstatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTestPrj)).BeginInit();
            this.toolUser.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDUTVer);
            this.groupBox1.Controls.Add(this.lblDUTNumber);
            this.groupBox1.Controls.Add(this.lblDUTName);
            this.groupBox1.Controls.Add(this.lstTS);
            this.groupBox1.Controls.Add(this.picTestPrj);
            this.groupBox1.Controls.Add(this.btnSeqDown);
            this.groupBox1.Controls.Add(this.btnSeqUp);
            this.groupBox1.Controls.Add(this.btnDelTestSeq);
            this.groupBox1.Controls.Add(this.btnAddTestSeq);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(15, 75);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(890, 393);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试序列";
            // 
            // lblDUTVer
            // 
            this.lblDUTVer.AutoSize = true;
            this.lblDUTVer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDUTVer.Location = new System.Drawing.Point(692, 33);
            this.lblDUTVer.Name = "lblDUTVer";
            this.lblDUTVer.Size = new System.Drawing.Size(62, 18);
            this.lblDUTVer.TabIndex = 4;
            this.lblDUTVer.Text = "版本：";
            // 
            // lblDUTNumber
            // 
            this.lblDUTNumber.AutoSize = true;
            this.lblDUTNumber.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDUTNumber.Location = new System.Drawing.Point(464, 33);
            this.lblDUTNumber.Name = "lblDUTNumber";
            this.lblDUTNumber.Size = new System.Drawing.Size(80, 18);
            this.lblDUTNumber.TabIndex = 4;
            this.lblDUTNumber.Text = "产品号：";
            // 
            // lblDUTName
            // 
            this.lblDUTName.AutoSize = true;
            this.lblDUTName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDUTName.Location = new System.Drawing.Point(464, 63);
            this.lblDUTName.Name = "lblDUTName";
            this.lblDUTName.Size = new System.Drawing.Size(98, 18);
            this.lblDUTName.TabIndex = 4;
            this.lblDUTName.Text = "产品名称：";
            // 
            // lstTS
            // 
            this.lstTS.FormattingEnabled = true;
            this.lstTS.ItemHeight = 20;
            this.lstTS.Location = new System.Drawing.Point(14, 33);
            this.lstTS.Name = "lstTS";
            this.lstTS.Size = new System.Drawing.Size(415, 264);
            this.lstTS.TabIndex = 3;
            this.lstTS.SelectedIndexChanged += new System.EventHandler(this.lstTS_SelectedIndexChanged);
            // 
            // picTestPrj
            // 
            this.picTestPrj.Location = new System.Drawing.Point(466, 99);
            this.picTestPrj.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picTestPrj.Name = "picTestPrj";
            this.picTestPrj.Size = new System.Drawing.Size(414, 284);
            this.picTestPrj.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTestPrj.TabIndex = 1;
            this.picTestPrj.TabStop = false;
            // 
            // btnSeqDown
            // 
            this.btnSeqDown.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSeqDown.Location = new System.Drawing.Point(339, 352);
            this.btnSeqDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeqDown.Name = "btnSeqDown";
            this.btnSeqDown.Size = new System.Drawing.Size(90, 33);
            this.btnSeqDown.TabIndex = 1;
            this.btnSeqDown.Text = "下移";
            this.btnSeqDown.UseVisualStyleBackColor = true;
            this.btnSeqDown.Click += new System.EventHandler(this.btnSeqDown_Click);
            // 
            // btnSeqUp
            // 
            this.btnSeqUp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSeqUp.Location = new System.Drawing.Point(234, 352);
            this.btnSeqUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeqUp.Name = "btnSeqUp";
            this.btnSeqUp.Size = new System.Drawing.Size(90, 33);
            this.btnSeqUp.TabIndex = 1;
            this.btnSeqUp.Text = "上移";
            this.btnSeqUp.UseVisualStyleBackColor = true;
            this.btnSeqUp.Click += new System.EventHandler(this.btnSeqUp_Click);
            // 
            // btnDelTestSeq
            // 
            this.btnDelTestSeq.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelTestSeq.Location = new System.Drawing.Point(120, 352);
            this.btnDelTestSeq.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDelTestSeq.Name = "btnDelTestSeq";
            this.btnDelTestSeq.Size = new System.Drawing.Size(90, 33);
            this.btnDelTestSeq.TabIndex = 1;
            this.btnDelTestSeq.Text = "删除";
            this.btnDelTestSeq.UseVisualStyleBackColor = true;
            this.btnDelTestSeq.Click += new System.EventHandler(this.btnDelTestSeq_Click);
            // 
            // btnAddTestSeq
            // 
            this.btnAddTestSeq.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddTestSeq.Location = new System.Drawing.Point(15, 352);
            this.btnAddTestSeq.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddTestSeq.Name = "btnAddTestSeq";
            this.btnAddTestSeq.Size = new System.Drawing.Size(90, 33);
            this.btnAddTestSeq.TabIndex = 1;
            this.btnAddTestSeq.Text = "添加";
            this.btnAddTestSeq.UseVisualStyleBackColor = true;
            this.btnAddTestSeq.Click += new System.EventHandler(this.btnAddTestSeq_Click);
            // 
            // toolUser
            // 
            this.toolUser.AllowDrop = true;
            this.toolUser.AutoSize = false;
            this.toolUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUserName,
            this.txtUserName,
            this.separator,
            this.lblPassword,
            this.txtPassword,
            this.toolStripSeparator2,
            this.btnLogin,
            this.toolStripSeparator3,
            this.btnLogOut,
            this.toolStripSeparator4,
            this.lblInfo});
            this.toolUser.Location = new System.Drawing.Point(0, 34);
            this.toolUser.Name = "toolUser";
            this.toolUser.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolUser.Size = new System.Drawing.Size(916, 38);
            this.toolUser.TabIndex = 5;
            this.toolUser.Text = "toolStrip1";
            // 
            // lblUserName
            // 
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(82, 35);
            this.lblUserName.Text = "用户名：";
            // 
            // txtUserName
            // 
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(148, 38);
            this.txtUserName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserName_KeyPress);
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // separator
            // 
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(6, 38);
            // 
            // lblPassword
            // 
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 35);
            this.lblPassword.Text = "密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(148, 38);
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // btnLogin
            // 
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(66, 35);
            this.btnLogin.Text = "登录";
            this.btnLogin.ToolTipText = "Log in";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // btnLogOut
            // 
            this.btnLogOut.Image = ((System.Drawing.Image)(resources.GetObject("btnLogOut.Image")));
            this.btnLogOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(66, 35);
            this.btnLogOut.Text = "注销";
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 38);
            // 
            // lblInfo
            // 
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(64, 35);
            this.lblInfo.Text = "请登录";
            this.lblInfo.Click += new System.EventHandler(this.lblInfo_Click);
            // 
            // menuUser
            // 
            this.menuUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogin,
            this.menuLogout,
            this.menuChangePwd,
            this.toolStripSeparator8,
            this.menuQuit});
            this.menuUser.Name = "menuUser";
            this.menuUser.Size = new System.Drawing.Size(83, 28);
            this.menuUser.Text = "用户(&U)";
            // 
            // menuLogin
            // 
            this.menuLogin.Name = "menuLogin";
            this.menuLogin.Size = new System.Drawing.Size(176, 28);
            this.menuLogin.Text = "登录(&L)";
            this.menuLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(176, 28);
            this.menuLogout.Text = "注销(&O)";
            this.menuLogout.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // menuChangePwd
            // 
            this.menuChangePwd.Name = "menuChangePwd";
            this.menuChangePwd.Size = new System.Drawing.Size(176, 28);
            this.menuChangePwd.Text = "修改密码(&C)";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(173, 6);
            // 
            // menuQuit
            // 
            this.menuQuit.Name = "menuQuit";
            this.menuQuit.Size = new System.Drawing.Size(176, 28);
            this.menuQuit.Text = "退出(&Q)";
            this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
            // 
            // menuTest
            // 
            this.menuTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditTest,
            this.menuRunTest,
            this.menuHardware,
            this.menuUserManage});
            this.menuTest.Name = "menuTest";
            this.menuTest.Size = new System.Drawing.Size(80, 28);
            this.menuTest.Text = "操作(&T)";
            // 
            // menuEditTest
            // 
            this.menuEditTest.Name = "menuEditTest";
            this.menuEditTest.Size = new System.Drawing.Size(212, 28);
            this.menuEditTest.Text = "编辑测试项目(&E)";
            // 
            // menuRunTest
            // 
            this.menuRunTest.Name = "menuRunTest";
            this.menuRunTest.Size = new System.Drawing.Size(212, 28);
            this.menuRunTest.Text = "运行测试项目(&R)";
            // 
            // menuHardware
            // 
            this.menuHardware.Name = "menuHardware";
            this.menuHardware.Size = new System.Drawing.Size(212, 28);
            this.menuHardware.Text = "硬件调试(&D)";
            // 
            // menuUserManage
            // 
            this.menuUserManage.Name = "menuUserManage";
            this.menuUserManage.Size = new System.Drawing.Size(212, 28);
            this.menuUserManage.Text = "用户管理(&M)";
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(84, 28);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(141, 28);
            this.关于ToolStripMenuItem.Text = "关于(&A)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuUser,
            this.menuTest,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(916, 34);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "52design.com_kr_011.png");
            this.imageList1.Images.SetKeyName(1, "20071120134217968.png");
            this.imageList1.Images.SetKeyName(2, "png-0007.png");
            this.imageList1.Images.SetKeyName(3, "png-0014.png");
            this.imageList1.Images.SetKeyName(4, "AVx02.jpg");
            // 
            // btnRunTest
            // 
            this.btnRunTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRunTest.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRunTest.ImageIndex = 0;
            this.btnRunTest.ImageList = this.imageList1;
            this.btnRunTest.Location = new System.Drawing.Point(4, 4);
            this.btnRunTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(170, 168);
            this.btnRunTest.TabIndex = 10;
            this.btnRunTest.Text = "运行测试";
            this.btnRunTest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRunTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnHWDebug, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnChangePSW, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnUserManage, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnEditTest, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRunTest, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 477);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(890, 176);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // btnHWDebug
            // 
            this.btnHWDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHWDebug.Enabled = false;
            this.btnHWDebug.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHWDebug.ImageIndex = 2;
            this.btnHWDebug.ImageList = this.imageList1;
            this.btnHWDebug.Location = new System.Drawing.Point(716, 4);
            this.btnHWDebug.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHWDebug.Name = "btnHWDebug";
            this.btnHWDebug.Size = new System.Drawing.Size(170, 168);
            this.btnHWDebug.TabIndex = 14;
            this.btnHWDebug.Text = "记录查询";
            this.btnHWDebug.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHWDebug.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHWDebug.UseVisualStyleBackColor = true;
            this.btnHWDebug.Click += new System.EventHandler(this.btnHWDebug_Click);
            // 
            // btnChangePSW
            // 
            this.btnChangePSW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChangePSW.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChangePSW.ImageIndex = 4;
            this.btnChangePSW.ImageList = this.imageList1;
            this.btnChangePSW.Location = new System.Drawing.Point(538, 4);
            this.btnChangePSW.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangePSW.Name = "btnChangePSW";
            this.btnChangePSW.Size = new System.Drawing.Size(170, 168);
            this.btnChangePSW.TabIndex = 13;
            this.btnChangePSW.Text = "更改密码";
            this.btnChangePSW.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangePSW.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnChangePSW.UseVisualStyleBackColor = true;
            this.btnChangePSW.Click += new System.EventHandler(this.btnChangePSW_Click);
            // 
            // btnUserManage
            // 
            this.btnUserManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUserManage.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUserManage.ImageIndex = 1;
            this.btnUserManage.ImageList = this.imageList1;
            this.btnUserManage.Location = new System.Drawing.Point(360, 4);
            this.btnUserManage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Size = new System.Drawing.Size(170, 168);
            this.btnUserManage.TabIndex = 12;
            this.btnUserManage.Text = "用户管理";
            this.btnUserManage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUserManage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUserManage.UseVisualStyleBackColor = true;
            this.btnUserManage.Click += new System.EventHandler(this.btnUserManage_Click);
            // 
            // btnEditTest
            // 
            this.btnEditTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditTest.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditTest.ImageIndex = 3;
            this.btnEditTest.ImageList = this.imageList1;
            this.btnEditTest.Location = new System.Drawing.Point(182, 4);
            this.btnEditTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditTest.Name = "btnEditTest";
            this.btnEditTest.Size = new System.Drawing.Size(170, 168);
            this.btnEditTest.TabIndex = 11;
            this.btnEditTest.Text = "测试构建";
            this.btnEditTest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEditTest.UseVisualStyleBackColor = true;
            this.btnEditTest.Click += new System.EventHandler(this.btnEditTest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.statusPath});
            this.statusStrip1.Location = new System.Drawing.Point(0, 664);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(916, 29);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(100, 24);
            this.toolStripStatusLabel1.Text = "文件路径：";
            // 
            // statusPath
            // 
            this.statusPath.Name = "statusPath";
            this.statusPath.Size = new System.Drawing.Size(28, 24);
            this.statusPath.Text = "无";
            // 
            // timer1
            // 
            this.timer1.Interval = 800;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(518, 666);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 26;
            this.label3.Text = "网络状态：";
            // 
            // lb_serverstate
            // 
            this.lb_serverstate.AutoSize = true;
            this.lb_serverstate.ForeColor = System.Drawing.Color.Red;
            this.lb_serverstate.Location = new System.Drawing.Point(626, 666);
            this.lb_serverstate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_serverstate.Name = "lb_serverstate";
            this.lb_serverstate.Size = new System.Drawing.Size(62, 18);
            this.lb_serverstate.TabIndex = 27;
            this.lb_serverstate.Text = "未连接";
            // 
            // lb_plcstatus
            // 
            this.lb_plcstatus.AutoSize = true;
            this.lb_plcstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lb_plcstatus.Location = new System.Drawing.Point(816, 666);
            this.lb_plcstatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_plcstatus.Name = "lb_plcstatus";
            this.lb_plcstatus.Size = new System.Drawing.Size(62, 18);
            this.lb_plcstatus.TabIndex = 29;
            this.lb_plcstatus.Text = "未连接";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(722, 666);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 28;
            this.label2.Text = "PLC状态：";
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 693);
            this.Controls.Add(this.lb_plcstatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_serverstate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolUser);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStart_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStart_FormClosed);
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.Shown += new System.EventHandler(this.frmStart_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTestPrj)).EndInit();
            this.toolUser.ResumeLayout(false);
            this.toolUser.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picTestPrj;
        private System.Windows.Forms.Button btnSeqDown;
        private System.Windows.Forms.Button btnSeqUp;
        private System.Windows.Forms.Button btnDelTestSeq;
        private System.Windows.Forms.Button btnAddTestSeq;
        private System.Windows.Forms.ToolStrip toolUser;
        private System.Windows.Forms.ToolStripLabel lblUserName;
        private System.Windows.Forms.ToolStripTextBox txtUserName;
        private System.Windows.Forms.ToolStripSeparator separator;
        private System.Windows.Forms.ToolStripLabel lblPassword;
        private System.Windows.Forms.ToolStripTextBox txtPassword;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnLogOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ListBox lstTS;
        private System.Windows.Forms.ToolStripMenuItem menuUser;
        private System.Windows.Forms.ToolStripMenuItem menuLogin;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripMenuItem menuChangePwd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem menuQuit;
        private System.Windows.Forms.ToolStripMenuItem menuTest;
        private System.Windows.Forms.ToolStripMenuItem menuEditTest;
        private System.Windows.Forms.ToolStripMenuItem menuRunTest;
        private System.Windows.Forms.ToolStripMenuItem menuHardware;
        private System.Windows.Forms.ToolStripMenuItem menuUserManage;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnRunTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnHWDebug;
        private System.Windows.Forms.Button btnChangePSW;
        private System.Windows.Forms.Button btnUserManage;
        private System.Windows.Forms.Button btnEditTest;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusPath;
        private System.Windows.Forms.ToolStripLabel lblInfo;
        private System.Windows.Forms.Label lblDUTName;
        private System.Windows.Forms.Label lblDUTVer;
        private System.Windows.Forms.Label lblDUTNumber;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_serverstate;
        private System.Windows.Forms.Label lb_plcstatus;
        private System.Windows.Forms.Label label2;



    }
}

