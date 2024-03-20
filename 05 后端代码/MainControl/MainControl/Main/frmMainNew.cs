using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.PLCServer;
using Common.PLCData.Model;
using Common.PLCData.BLL;
using System.Threading;
using Common.Process.BLL;
using Common.Process.Model;
using System.IO;
using Common.StationProduct.Model;
using Library;
using System.ServiceModel;
using System.IO.Ports;
using System.Reflection;
using System.Timers;
using Test.StartUp;

namespace MainControl
{
    public partial class frmMainNew : Form
    {




        #region 公共变量
        public long allcount = 0;
        public int CurrentPageNum = 1;
        int FlowId30 = -1;
        public GeneralConfig CurrentConfig = new GeneralConfig();
        string Path = MdlClass.PLCConfigPath + @"\GNConfig.cfg";
        //public PLCS7 PLC = null;
        OverallServiceNew OVserver = null;


        public Thread thUIUPdate = null;
        FileStream Oilfs = null;
        #endregion


        #region 公共方法
        public void InitPLCServer()
        {
            CurrentConfig = CurrentConfig.Load(Path);
            OVserver = new OverallServiceNew(CurrentConfig, this);
        }

        public void GetProcesslst()
        {
            cmbProcessChoose.Items.Clear();
            MdlClass.ProcessList.Clear();
            MdlClass.ProcessList = MdlClass.p_processbll.GetProcessList(100, CurrentPageNum, ref allcount, "", false).ToList();
            //List<string> productlist = new List<string>();
            //int dcount = 1;
            for (int i = 0; i < MdlClass.ProcessList.Count; i++)
            {
                this.cmbProcessChoose.Items.Add(MdlClass.ProcessList[i].Name);
                //  dcount++;
                //}
            }
            if (cmbProcessChoose.Items.Count > 0)
            {
                cmbProcessChoose.SelectedIndex = 0;
            }
        }


        public void ClearUI()
        {
            dgvEvent30.Columns.Clear();
        }

        public void KeepUpUI()
        {
            do
            {



                Thread.Sleep(1000);
            } while (true);
        }

        #endregion

        public void GetSysSet()
        {

        }



        public frmMainNew()
        {
            InitializeComponent();

            #region 定时刷新时间
            timer4.Start(); // 启动定时器
            timer4.Enabled = true;
            #endregion


        }

        //private void 自定义CToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    frmSysConfig fm = new frmSysConfig();
        //    fm.ShowDialog();
        //}
        /// <summary>
        /// 菜单-工具-PLC 引擎
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选项OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPLCconfig fm = new frmPLCconfig();
            fm.ShowDialog();
        }

        /// <summary>
        /// 菜单-用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmLoginNew fm = new frmLoginNew();

            if (fm.ShowDialog() == DialogResult.OK)
            {

         
                PLCOToolStripMenuItem.Enabled = true;
                lblUserId.Text = $"用户ID：{ MdlClass.userInfo.UserNum}   用户名称: { MdlClass.userInfo.UserName}";
                lblMessage.Text = "登录成功。";

                OVserver.ConnectAllPlc(tabConrolMonitor);
                if (tabConrolMonitor.TabPages.Count > 0)
                {
                    tabConrolMonitor.SelectedIndex = 1;
                    tabConrolMonitor.SelectedIndex = 0;
                }
                toolStripMenuItem1.Enabled = false;
                退出XToolStripMenuItem.Enabled = true;
                MessageBox.Show(MdlClass.userInfo.UserName + ",欢迎登录.");
                GetProcesslst();
                MdlClass.lineSever.Init(tabSation);
                timer1.Enabled = true;
                timer1.Start();
                timer2.Enabled = true;
                timer2.Start();
                timer3.Enabled = true;
                timer3.Start();
                tlayPanlAll.Enabled = true;

               
            }
            else
            {

            }

        }

        /// <summary>
        /// 登录后初始化登录界面
        /// </summary>
        private void InitFrmMain()
        {

            PLCOToolStripMenuItem.Enabled = true;
            lblUserId.Text = $"用户ID：{MdlClass.userInfo.UserNum}   用户名称: {MdlClass.userInfo.UserName}";
            lblMessage.Text = "登录成功。";

            OVserver.ConnectAllPlc(tabConrolMonitor);
            if (tabConrolMonitor.TabPages.Count > 0)
            {
                tabConrolMonitor.SelectedIndex = 1;
                tabConrolMonitor.SelectedIndex = 0;
            }
            toolStripMenuItem1.Enabled = false;
            退出XToolStripMenuItem.Enabled = true;
            //MessageBox.Show(MdlClass.userInfo.UserName + ",欢迎登录.");
            GetProcesslst();
            MdlClass.lineSever.Init(tabSation);
            timer1.Enabled = true;
            timer1.Start();
            timer2.Enabled = true;
            timer2.Start();
            timer3.Enabled = true;
            timer3.Start();
            tlayPanlAll.Enabled = true;
        }
        


        private void frmMain_Load(object sender, EventArgs e)
        {

            worker = new BackgroundWorker();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.WorkerSupportsCancellation = true;
            StartServer();
            //MdlClass.Scanner70.Init_TCP(502, "192.168.0.65");

            //Int32 c = 0xE188;

            //Int32 b = 0xFFFF-c;
            InitPLCServer();

            btnStart.Enabled = false;
            btnStop.Enabled = false;

            cmbWorkType.SelectedIndex = 0;
            MdlClass.WorkType = (WorkType)(cmbWorkType.SelectedIndex + 1);
            //PLCDataCommon pp = new PLCDataCommon();
            //pp.TrayId = 12;
            //pp.UUTid = 12349;
            //pp.WorkEnable = 0;
            //pp.WorkType = 1;
            //pp.PartType = 2;
            //pp.WorkEnable = 3;
            //pp.CurrentResult = 4;
            //pp.ProductSN = "ABCSSEFRRG";
            //byte[] bt = MdlClass.PlcdataBll.PLCDataCommonToBytes(pp);
            //pp = MdlClass.PlcdataBll.ByteToPLCDataCommon(bt);


            toolStripMenuItem1.Enabled = true;
            退出XToolStripMenuItem.Enabled = false;
            tlayPanlAll.Enabled = false;

            lblUserId.Text = "用户ID：  " + "   用户名称:   ";
            MdlClass.sysSet = MdlClass.sysSet.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");

            Common.GlobalResources.dbserver = MdlClass.sysSet.DataSource;
            Common.GlobalResources.dbname = MdlClass.sysSet.InitialCatalog;
            Common.GlobalResources.userid = MdlClass.sysSet.UserID;
            Common.GlobalResources.password = MdlClass.sysSet.Password;
            lblMessage.Text = "正在连接数据库。。。";
            Application.DoEvents();
            Thread.Sleep(300);
            if (!GlobalResources.SqlConnectTest())
            {
                MessageBox.Show("连接数据库失败");
                lblMessage.Text = "连接数据库失败！";
            }
            else
            {
                lblMessage.Text = "连接数据库成功！";
            }
            timer1.Enabled = true;
            timer1.Start();
            timer2.Enabled = true;
            timer2.Start();
            timer3.Enabled = true;
            timer3.Start();

            //登录后初始化主界面
            InitFrmMain();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("退出用户登录，PLC连接将会断开，请确认是否继续断开？", "通知", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {

                //tlayPanlAll.Enabled = false;//禁用控件
                //  PLCOToolStripMenuItem.Enabled = false;

                lblUserId.Text = "用户ID：  " + "   用户名称:   ";
                lblMessage.Text = "未登录。";
                OVserver.DisConnectAllPlc();
                toolStripMenuItem1.Enabled = true;
                退出XToolStripMenuItem.Enabled = false;
                GC.Collect();

                timer1.Stop();
                timer1.Enabled = false;
                timer2.Stop();
                timer2.Enabled = false;
                timer3.Stop();
                timer3.Enabled = false;
                MessageBox.Show("注销成功, " + MdlClass.userInfo.UserName + ",感谢您的使用.");
                MdlClass.userInfo = null;
                MdlClass.lineSever.Dispose();
                cmbProcessChoose.Items.Clear();

            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                timer1.Stop();
                timer1.Enabled = false;
                timer2.Stop();
                timer2.Enabled = false;
                timer3.Stop();
                timer3.Enabled = false;
                OVserver.DisConnectAllPlc();
                StopSever();
            }
            catch
            {

            }
            MdlClass.Scanner70.CloseTCP();
            System.Environment.Exit(0);
        }


        public void UplblConnect(Label lb, bool res)
        {
            lb.Text = res ? "在线" : "离线";
            lb.BackColor = res ? Color.Green : Color.Red;
        }





        private void 文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cmbWorkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MdlClass.WorkType = (WorkType)(cmbWorkType.SelectedIndex + 1);
            OVserver.SetCurrentWorkType((byte)MdlClass.WorkType);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MdlClass.lineSever.DicStationControl["OPH30"].EnterStation(int.Parse(textBox1.Text), MainControl.Result.Pass);



            //MdlClass.lineSever.DicStationControl[""].EnterStation();
            //Common.StationProduct.BLL.StationRecord_BLL stbll = new Common.StationProduct.BLL.StationRecord_BLL();
            //Common.StationProduct.Model.StationRecord sres = new Common.StationProduct.Model.StationRecord();
            //sres.UUTestId = 1023;
            //sres.StationName = "11223";
            //sres.StationNum = "OPH30";
            //sres.STPrdType = (int)WorkType.Normal;
            //sres.StartTime = System.DateTime.Now;
            //sres.EndTime = sres.StartTime;
            //stbll.StationStart(sres);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MdlClass.lineSever.DicStationControl["OPH30"].LeaveStation(int.Parse(textBox1.Text), MainControl.Result.Pass);
            //  Common.StationProduct.BLL.StationRecord_BLL stbll = new Common.StationProduct.BLL.StationRecord_BLL();
            //    stbll.StationEnd(1023,(int)EndType.NormalEndline,(int)Result.Pass);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<Common.StationProduct.Model.AssembleRecord> lst = new List<Common.StationProduct.Model.AssembleRecord>();
            Common.Product.Model.AssembleRecord_BLL assbll = new Common.Product.Model.AssembleRecord_BLL();
            Common.StationProduct.Model.AssembleRecord ass = new Common.StationProduct.Model.AssembleRecord();
            ass.ItemName = "注油组件压装";
            ass.Description = "注油组件的压装";
            ass.PartNum = "0723221";
            ass.Result = (int)Result.Pass;
            ass.ScanCode = "asassdsd343434";
            ass.Scantime = System.DateTime.Now;
            ass.StationTestId = 1;
            ass.UUTestId = 1023;
            lst.Add(ass);
            ass = new Common.StationProduct.Model.AssembleRecord();
            ass.ItemName = "注油组件压装";
            ass.Description = "注油组件的压装";
            ass.PartNum = "0723221";
            ass.Result = (int)Result.Pass;
            ass.ScanCode = "asassdsd343434";
            ass.Scantime = System.DateTime.Now;
            ass.StationTestId = 2;
            ass.UUTestId = 1024;
            lst.Add(ass);
            assbll.MultiAssembleRecordWrite(lst);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Common.Product.Model.AssembleRecord_BLL assbll = new Common.Product.Model.AssembleRecord_BLL();
            Common.StationProduct.Model.AssembleRecord ass = new Common.StationProduct.Model.AssembleRecord();
            ass.ItemName = "注油组件压装";
            ass.Description = "注油组件的压装";
            ass.PartNum = "0723221";
            ass.Result = (int)Result.Pass;
            ass.ScanCode = "asassdsd343434";
            ass.Scantime = System.DateTime.Now;
            ass.StationTestId = 1;
            ass.UUTestId = 1023;
            assbll.AssembleRecordWrite(ass);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Common.Product.Model.TestRecord_BLL assbll = new Common.Product.Model.TestRecord_BLL();
            Common.StationProduct.Model.TestRecord tes = new Common.StationProduct.Model.TestRecord();
            tes.ComPareMode = "对比";
            tes.Description = "测试记录";
            tes.Llimit = "100";
            tes.Nom = "200";
            tes.Result = (int)Result.Pass;
            tes.SpanTime = 1000;
            tes.StationTestId = 1;
            tes.TestNum = "注油测试";
            tes.TestName = "测试时间";
            tes.TestTime = System.DateTime.Now;
            tes.TestValue = "10";
            tes.Ulimit = "300";
            tes.Unit = "克";

            tes.StationTestId = 1;
            tes.UUTestId = 1023;
            assbll.SingleTestResWrite(tes);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<Common.StationProduct.Model.TestRecord> lst = new List<Common.StationProduct.Model.TestRecord>();
            Common.Product.Model.TestRecord_BLL assbll = new Common.Product.Model.TestRecord_BLL();
            Common.StationProduct.Model.TestRecord tes = new Common.StationProduct.Model.TestRecord();
            tes.ComPareMode = "对比";
            tes.Description = "测试记录";
            tes.Llimit = "100";
            tes.Nom = "200";
            tes.Result = (int)Result.Pass;
            tes.SpanTime = 1000;
            tes.StationTestId = 1;
            tes.TestNum = "注油测试";
            tes.TestName = "测试时间";
            tes.TestTime = System.DateTime.Now;
            tes.TestValue = "10";
            tes.Ulimit = "300";
            tes.Unit = "克";

            tes.StationTestId = 1;
            tes.UUTestId = 1023;


            lst.Add(tes);



            tes = new Common.StationProduct.Model.TestRecord();
            tes.ComPareMode = "不对比";
            tes.Description = "测试记录";
            tes.Llimit = "100";
            tes.Nom = "200";
            tes.Result = (int)Result.Pass;
            tes.SpanTime = 1000;
            tes.StationTestId = 1;
            tes.TestNum = "注油测试";
            tes.TestName = "测试时间";
            tes.TestTime = System.DateTime.Now;
            tes.TestValue = "10";
            tes.Ulimit = "300";
            tes.Unit = "斤";

            tes.StationTestId = 1;
            tes.UUTestId = 1024;
            lst.Add(tes);

            assbll.MultiTestResWrite(lst);
        }

        private void cmbProcessChoose_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbProcessChoose.SelectedIndex >= 0)
            {
                MdlClass.CurrentProcessConfig = null;
                MdlClass.CurrentProcessConfig = new ProcessConfig();
                if (MdlClass.CurrentProcessConfig.Load(MdlClass.ProcessList[cmbProcessChoose.SelectedIndex]))
                {
                    lbltitle.Text = "产品号: " + MdlClass.CurrentProcessConfig.Currentprocess.ProductNum + "   SAP号: " + MdlClass.CurrentProcessConfig.Currentprocess.SAPNum + "   名称: " + MdlClass.CurrentProcessConfig.Currentprocess.ProductName + "  PLC程序号: " + MdlClass.CurrentProcessConfig.Currentprocess.PLCPNum;
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    MdlClass.lineSever.SelectProcess(MdlClass.CurrentProcessConfig);
                }
                else
                {
                    MessageBox.Show("选取产品失败！");
                }
            }
        }

        string PCBSN = "";
        public void SacnPCBSN(object sender, SerialDataReceivedEventArgs e)
        {

            if (PCBSN.Length > 0)
            {
                if (PCBSN[PCBSN.Length - 1] == 0x0D)
                {
                    PCBSN = "";
                    Console.WriteLine("清空");
                }
                else
                {

                }
            }
            PCBSN += MdlClass.myScanner.MyPort.ReadExisting();
            //
            if (PCBSN.Length > 0)
            {
                if (PCBSN[PCBSN.Length - 1] == 0x0D)
                {
                    this.Invoke(new Action(() => { lblPCBSN.Text = PCBSN; }));
                    Console.WriteLine("PCBSN:" + PCBSN);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            cmbProcessChoose_SelectedIndexChanged(null,null);
            if (MdlClass.CurrentProcessConfig == null)
            {
               // Console.WriteLine();
                MessageBox.Show("当前工艺配置为空。");
                return;
            }

            PLCDataCommon plcdatacommon = new PLCDataCommon();
            //plcdatacommon.PartType_PLC =(byte) cmbProcessChoose.SelectedIndex;//设置当前PLC的变种
            plcdatacommon.PartType_PLC = (byte)MdlClass.CurrentProcessConfig.Currentprocess.PLCPNum;//设置当前PLC的变种
            //询问PLC是否无产品 PLC_OPH30   请求切换变种
            ParaAndResultStrut res = OVserver.ExcutPC2PLCEvent(MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdatacommon), "PLC_OPH30", "上下料工位", "请求切换变种") as ParaAndResultStrut;
            if (res.Result)
            {
                MdlClass.lineSever.Start();
                MdlClass.mydc62.Init_TCP(5000, "192.168.0.62");
                MdlClass.mydc63.Init_TCP(5000, "192.168.0.63");
                MdlClass.mydc62.StartACQ();
                MdlClass.mydc63.StartACQ();
                MdlClass.myLT_Link99room1.Init_TCP(5000, "192.168.0.99");
                MdlClass.myLT_Link98room2.Init_TCP(5000, "192.168.0.98");
                MdlClass.myLT_Link99room1.StartACQ();
                MdlClass.myLT_Link98room2.StartACQ();
                lblPCBSN.Text = "";
                MdlClass.myScanner.Open("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, new SerialDataReceivedEventHandler(SacnPCBSN));

                btnStop.Enabled = true;
                btnStart.Enabled = false;
                cmbProcessChoose.Enabled = false;
                MdlClass.Working = true;
                OVserver.SetCanWork();
                MdlClass.WorkType = (WorkType)(cmbWorkType.SelectedIndex + 1);
                OVserver.SetCurrentWorkType((byte)MdlClass.WorkType);
             
                MessageBox.Show("切换成功。");
            }
            else
            {
                MessageBox.Show("无法切换变种，PLC正在生产变种:" + MdlClass.PlcdataBll.ByteToPLCDataCommon(res.Data).PartType_PLC.ToString());
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cmbProcessChoose.Enabled = true;

            btnStop.Enabled = false;
            btnStart.Enabled = true;
            MdlClass.lineSever.Stop();
            MdlClass.Working = false;
            MdlClass.mydc62.StopACQ();
            MdlClass.mydc62.CloseTCP();
            MdlClass.mydc63.StopACQ();
            MdlClass.mydc63.CloseTCP();

            MdlClass.myLT_Link99room1.StopACQ();
            MdlClass.myLT_Link99room1.CloseTCP();
            MdlClass.myLT_Link98room2.StopACQ();
            MdlClass.myLT_Link98room2.CloseTCP();

            MdlClass.myScanner.Close();
            OVserver.ResetNotWork();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        Random r = new Random();
        private void timer1_Tick(object sender, EventArgs e)
        {

            return;
            #region 当前唯一号
            Dictionary<string, byte> dicworktype = OVserver.GetCurrentWorkType();
            if (dicworktype != null && dicworktype.ContainsKey("PLC_OPH30"))
            {
                lblCurrentWorkMode.Text = "当前工作模式:" + ((WorkType)OVserver.GetCurrentWorkType()["PLC_OPH30"]).ToString();
            }
            #endregion


            #region  随机生成SN号

            //   lblMainPart.Text = "PCBSN"+System.DateTime.Now.ToString("yyyyMMddHHmmss");
            #endregion

            int selectRowIndex = 0;
            try
            {
                int y = dgvInWork.VerticalScrollingOffset;
                DataTable dt = null;
                using (MyDB_Sql mydbUUTDB = new MyDB_Sql(Common.GlobalResources.dbserver, Common.GlobalResources.dbname, Common.GlobalResources.userid, Common.GlobalResources.password))
                {
                    dt = GlobalResources.GetDataTable("SELECT * FROM [P_InWork]", mydbUUTDB);
                }
                if (dt == null) return;
                dt.Columns.Add("WrokType");
                dt.Columns.Add("Result");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["WrokType"] = ((WorkType)dt.Rows[i]["PrdType"]).ToString();
                    dt.Rows[i]["Result"] = ((Result)dt.Rows[i]["CurrentResult"]).ToString();
                }
                if (dgvInWork.SelectedCells != null && dgvInWork.SelectedCells.Count > 0)
                {
                    selectRowIndex = dgvInWork.SelectedCells[0].RowIndex;

                }
                dgvInWork.DataSource = dt;
                dgvInWork.Columns["StartTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvInWork.Columns["ExpectedPathNames"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvInWork.Columns["ActualPath"].Visible = false;
                dgvInWork.Columns["ActualResult"].Visible = false;
                dgvInWork.Columns["ExpectedPath"].Visible = false;
                // dgvInWork.Columns["CurrentSationTestID"].Visible = false;
                dgvInWork.Columns["PrdType"].Visible = false;
                dgvInWork.Columns["CurrentResult"].Visible = false;
                dgvInWork.Sort(dgvInWork.Columns[0], ListSortDirection.Ascending);
                dgvInWork.ClearSelection();
                if (selectRowIndex < dgvInWork.Rows.Count)
                {
                    dgvInWork.Rows[selectRowIndex].Selected = true;
                    dgvInWork.FirstDisplayedScrollingRowIndex = dgvInWork.SelectedRows[0].Index;
                }
                else
                {
                    dgvInWork.Rows[dgvInWork.Rows.Count - 1].Selected = true;
                    dgvInWork.FirstDisplayedScrollingRowIndex = dgvInWork.SelectedRows[0].Index;
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // MdlClass.lineSever.Enter(WorkType.Normal,"","");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //MdlClass.lineSever.Leave(int.Parse(textBox1.Text), EndType.NormalEndline, Result.Pass);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }




        //string Current30OlilingPath = "";
        DateTime CurrentDatetTime30;
        DateTime CurrentDatetTime40_2;
        public void upOPH30Oiling()
        {
            try
            {
                FileInfo[] files = new DirectoryInfo(MdlClass.Oliling30Path).GetFiles();


                //1分类
                List<FileInfo> Oliling30files = new List<FileInfo>();


                foreach (FileInfo f in files)
                {
                    //1检查文件名合法性
                    if (System.IO.Path.GetExtension(f.FullName).ToUpper() == ".JPEG")
                    {
                        string[] s = System.IO.Path.GetFileNameWithoutExtension(f.FullName).Split('_');
                        Oliling30files.Add(f);
                    }
                    else
                    {
                        System.IO.File.Delete(f.FullName);
                    }
                }


                //2排序。



                Oliling30files.Sort((x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));//从小到大排列
                //MainPartfiles.Reverse();
                //Pcbfiles.Reverse();
                //3显示

                if (Oliling30files.Count > 0)
                {

                    if (CurrentDatetTime30 < Oliling30files[Oliling30files.Count - 1].LastWriteTime)
                    {
                        try
                        {
                            //显示PCBA
                            CurrentDatetTime30 = Oliling30files[Oliling30files.Count - 1].LastWriteTime;
                            //string[] s = Current30OlilingPath.Split('_');
                            //if (s[1] == "2")
                            //{
                            //    gbboxPCBA.ForeColor = Color.Red;
                            //    //NG
                            //}
                            //else
                            //{
                            //    gbboxPCBA.ForeColor = Color.Green;
                            //    //Pass
                            //}

                         //   this.Invoke(new Action(() => { this.pci30Oiling.Image.Dispose(); }));
                            if (Oliling30files[Oliling30files.Count - 1].Name.ToUpper().StartsWith("OK"))
                            {
                                this.Invoke(new Action(() =>
                                {

                                    lbl30Oiling.Text = "检测结果:Pass";
                                    lbl30Oiling.BackColor = Color.Green;
                                    lbl30Oiling.ForeColor = Color.White;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {

                                    lbl30Oiling.Text = "检测结果:Fail";
                                    lbl30Oiling.BackColor = Color.Red;
                                    lbl30Oiling.ForeColor = Color.White;
                                }));
                            }

                            //Bitmap mImage = new Bitmap(CurrentPCBAPath, false);
                            //picPCBA.Image = mImage;
                            Thread.Sleep(300);

                            try
                            {

                               Oilfs = new FileStream(Oliling30files[Oliling30files.Count - 1].FullName, FileMode.Open, FileAccess.Read,
                                //     Oilfs = new FileStream("C:\\rw.txt", FileMode.Open, FileAccess.Read,
                                FileShare.None);

                                if (Oilfs != null)
                                {
                                  
                                    Bitmap mImage = new Bitmap(Oilfs,true);
                                  //  mImage = null;
                                    //Bitmap mImage = new Bitmap(Oliling40_2files[Oliling40_2files.Count - 1].FullName, true);
                                    this.Invoke(new Action(() => {
                                        if (this.pci30Oiling.Image != null)
                                        {
                                            this.pci30Oiling.Image.Dispose();
                                            this.pci30Oiling.Image = null;
                                        }
                                        this.pci30Oiling.Image = mImage; 
                                    }));
                                }
                            }
                            catch
                            {
                               
                            }
                            finally
                            {
                                if (Oilfs != null)
                                {
                                    Oilfs.Close();
                                }
                            }
                            //using (FileStream fs = File.OpenRead(Oliling30files[Oliling30files.Count - 1].FullName))
                            //{
                            //    Bitmap mImage = new Bitmap(fs);
                            //    this.Invoke(new Action(() => { this.pci30Oiling.Image = mImage; }));
                            //}
                        }
                        catch
                        {
                            Console.WriteLine();
                        }
                        //显示本体
                    }

                }



                //4删除时间最早的。

                if (Oliling30files.Count > 5)
                {
                    int Removetime = Oliling30files.Count - 5;
                    for (int i = 0; i < Removetime; i++)
                    {
                        File.Delete(Oliling30files[0].FullName);
                        Oliling30files.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,

                FileShare.None);

                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            return;

            try
            {
                // Show40_2_Oliling();

              //  upOPH30Oiling();


                if (OVserver != null)
                {

                    DataTable dt = OVserver.GetSelectPLCFlowState(0);
                    if (dt == null) return;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["StationId"].ToString() == "0")
                        {
                            if (dr["FlowMessage"].ToString() != "NA")
                            {
                                lblMessage.Text = dr["FlowMessage"].ToString();

                                if (FlowId30 != int.Parse(dr["FlowId"].ToString()))
                                {
                                    FlowId30 = int.Parse(dr["FlowId"].ToString());
                                    if (FlowId30 == 52)
                                    {
                                        this.Invoke(new Action(() => { picNotice.Image.Dispose(); }));
                                        Bitmap mImage = new Bitmap(MdlClass.ShowPicturePath + @"\取出连杆.png", true);

                                        this.Invoke(new Action(() => { picNotice.Image = mImage; }));
                                    }
                                    if (FlowId30 == 55)
                                    {
                                        this.Invoke(new Action(() => { picNotice.Image.Dispose(); }));
                                        Bitmap mImage = new Bitmap(MdlClass.ShowPicturePath + @"\ProductIn.png", true);

                                        picNotice.Image = mImage;
                                    }
                                    if (FlowId30 == 67)
                                    {
                                        this.Invoke(new Action(() => { picNotice.Image.Dispose(); }));
                                        Bitmap mImage = new Bitmap(MdlClass.ShowPicturePath + @"\ProductOut.png", true);

                                        picNotice.Image = mImage;
                                    }
                                    if (FlowId30 == 5)
                                    {
                                        this.Invoke(new Action(() => { picNotice.Image.Dispose(); }));
                                        Bitmap mImage = new Bitmap(MdlClass.ShowPicturePath + @"\Waiting.png", true);

                                        this.Invoke(new Action(() => { picNotice.Image = mImage; }));
                                    }
                                    if (FlowId30 == 105)
                                    {
                                        this.Invoke(new Action(() => { picNotice.Image.Dispose(); }));
                                        Bitmap mImage = new Bitmap(MdlClass.ShowPicturePath + @"\TrayLeave.png", true);

                                        this.Invoke(new Action(() => { picNotice.Image = mImage; }));
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        DK_DC mydkdc = null;
        LT_Link_Protocol80 myltlink = new LT_Link_Protocol80();
        private void button15_Click_1(object sender, EventArgs e)
        {
            //mydkdc = new DK_DC();
            //mydkdc.Init_TCP(5000, "192.168.0.62");
            //mydkdc.StartACQ();
            myltlink.Init_TCP(5000, "192.168.0.98");
            myltlink.StartACQ();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            myltlink.StopACQ();
            myltlink.CloseTCP();
            //mydkdc.StopACQ();
            //mydkdc.CloseTCP();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //mydkdc.SendHeart();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "确定要清空所有产品数据吗？", "警告", MessageBoxButtons.YesNo);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                Common.GlobalResources.ClearTable("LGDB", "P_AssembleRes");
                Common.GlobalResources.ClearTable("LGDB", "P_TestRes");
                Common.GlobalResources.ClearTable("LGDB", "P_InWork");
                Common.GlobalResources.ClearTable("LGDB", "P_Product");
                Common.GlobalResources.ClearTable("LGDB", "P_SationProduct");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Common.GlobalResources.ClearTable("LGDB", "P_InWork");
        }

        private void button20_Click(object sender, EventArgs e)
        {


            MdlClass.Scanner70.StartScan();
          //  MdlClass.LogHandle = new WriteLog(Application.StartupPath + @"\Log");
          //  MdlClass.LogHandle.dbErrLog
            Common.GlobalLogHandle.PLCLogHandle.PLCLog( MethodBase.GetCurrentMethod().Name,"测试1111111");
            Common.GlobalLogHandle.PLCLogHandle.SizeCheck(15);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            double B = GlobalResources.SelectLogBufferSize("LGDB");
            if (B > 1)//如果日志大于1.5G
            {
                if (GlobalResources.BackDB("LGDB", @"D:\MESDB\DBAutoBack\LGDBbak" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak"))
                {
                    if (GlobalResources.CutLog("LGDB"))
                    {
                        MessageBox.Show("日志截断成功。");
                    }
                    else { MessageBox.Show("日志截断失败。"); }
                }
                else 
                {
                    MessageBox.Show("数据库备份失败。");
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
         //   Common.Part.BLL.PartInfo p = MdlClass.partbll.GetPartInfoByPartNo("71130030");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            return;
            upOPH30Oiling();
        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        #region 洪修改print服务
        TestDataServer BC = null;
        ServiceHost serviceHost = null;




        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BC = new TestDataServer();
                //  BC.myPrint = new TestDataServer.PrintEvent(Pinritlabel);

                //BC._hbManager = new HeartBeatManager(DiccallbackList, DicSlaveStationList, DichbRecordList);
                //BC._hbManager.LossHeartHandle = new HeartBeatManager.LossHeartHandledelegate(LossHandle);
                //BC.Channel_ClosingHandler = new EventHandler(Channel_Closing);
                //BC.frmMaster = this;
                serviceHost = new ServiceHost(BC);

                if (serviceHost.State != CommunicationState.Opened)
                {
                    serviceHost.Open();
                }

                e.Result = "正常";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.ToString() == "正常")
                {

                    lblDataServerState.Text = "数据服务状态:  " + "正在运行...";
                }
                else
                {
                    lblDataServerState.Text = "数据服务状态:  " + string.Format("错误：{0}", e.Result);
                }
                this.Text = this.Text;
            }
        }

        void StartServer()
        {
            if (!worker.IsBusy)
            {
                lblDataServerState.Text = "数据服务状态:  " + "正在启动......";


                worker.RunWorkerAsync();
            }
        }



        void StopSever()
        {
            serviceHost.Close();
            worker.CancelAsync();
            this.Text = "数据服务状态:  " + "停止";
        }
        #endregion

       
        #region 窗体头部
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 退出关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要退出系统么？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
        }
        /// <summary>
        /// 定时刷新时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer4_Tick(object sender, EventArgs e)
        {
            
            labtime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }


        #endregion

        private void 自定义CToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据库配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDBConfigEdit fm = new FrmDBConfigEdit();
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
        }
        /// <summary>
        /// 站位配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 站位配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStationConfig fm = new FrmStationConfig();
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
        }

        /// <summary>
        /// 测试工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 测试配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmToolTestInfo fm = new FrmToolTestInfo();

            //frmStart fm=new frmStart();
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            FrmUserInfo fm = new FrmUserInfo();
            fm.StartPosition = FormStartPosition.CenterParent;
            fm.ShowDialog();
        }
    }
}
