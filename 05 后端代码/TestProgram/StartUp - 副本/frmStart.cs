using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;

using System.IO;
using Test.Common;
using StartUp.Enity;
using Test.Query;

namespace StartUp
{
    public partial class frmStart : Form
    {
        string userName;
        UserManager m_UserManage = new UserManager();
        TSHandler TShandler = new TSHandler(Application.StartupPath + @"\TestSequence.xml");
        bool plcstatus = false;
        ReadPLC rp = new ReadPLC();

        Test.ProjectTest.frmTest_Wide frm = null;
        public frmStart()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        void TestMode_Change(object sender)
        {
            ReadPLC RPe = sender as ReadPLC;

            //  string[] keys =new string[]{"1_2","2_2","3_2"};
            //////  MessageBox.Show(RPe.Test_Mode.ToString());
            ////  //button1.Text = RPe.Test_Mode.ToString();
            ////  //发送到网络服务器
            //  if (RPe.ENdMode)
            //  {
            //      MdlClass.webserver.SetAllTestMode("TestMode", keys);
            //  }
            //  else
            //  {
            //      MdlClass.webserver.SetAllTestMode("EndMode", keys);
            //  }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //if (rp.ISTestMode)
            //{
            //    MdlClass.webserver.SetAllTestMode("EndMode");
            //}
            //else
            //{
            //    MdlClass.webserver.SetAllTestMode("TestMode");
            //}
        }

        public void LoginRun()
        {
            this.BeginInvoke(new Action(() =>
            {
                Login();
                btnRunTest_Click(null, null);
            }));
        }

        private void frmStart_Load(object sender, EventArgs e)
        {



            txtUserName.TextBox.Select();
            txtPassword.TextBox.PasswordChar = '*';
            LogOut();
            //if (rp.Init(13) != null)
            //{
            //    MdlClass.readplc = rp;
            //    plcstatus = true;
            //}

            // this.timer1.Start();
            //MdlClass.readplc.StartReadth();
        }

        /// <summary>
        /// 登出后相关控件处理
        /// </summary>
        private void LogOut()
        {
            btnLogin.Enabled = true;
            //btnLogOut.Enabled = false;

            btnEditTest.Enabled = false;
            btnRunTest.Enabled = false;
            btnUserManage.Enabled = false;
            btnChangePSW.Enabled = false;
            btnHWDebug.Enabled = false;

            lstTS.Enabled = false;
            btnAddTestSeq.Enabled = false;
            btnDelTestSeq.Enabled = false;
            btnSeqDown.Enabled = false;
            btnSeqUp.Enabled = false;

            lblUserName.Text = "用户名：";
            lblPassword.Visible = true;
            txtUserName.Visible = true;
            txtPassword.Visible = true;
            separator.Visible = true;

            lstTS.Items.Clear();
            txtUserName.Text = "";
            txtPassword.Text = "";
            menuChangePwd.Enabled = false;
            menuTest.Enabled = false;
        }





        /// <summary>
        /// 登录后相关控件处理
        /// </summary>
        private void Login()
        {
            lblInfo.Text = "";
            userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();


            if (userName == "" || userName == null)
            {
                lblInfo.Text = "请输入用户名！";
                return;
            }


            string sql = string.Format("SELECT [Name],[Type],[Number],[Password],[Description]  FROM [Utop_New].[dbo].[Users] where Number ='{0}'", userName, password);

            DataTable StepNamedt = SQLBAL.GetDataTable(sql);

            if (StepNamedt.Rows.Count > 0)
            {
                if (StepNamedt.Rows[0]["Password"].ToString() == password)
                {
                    frmUserManager frm = new frmUserManager();
                    List<GroupsEnity> groupsList = new List<GroupsEnity>();
                    groupsList = frm.SelectGroups(" and  Type='" + StepNamedt.Rows[0]["Type"].ToString() + "'");
                    if (groupsList.Count < 0 || groupsList == null)
                    {
                        lblInfo.Text = "未查询到权限，请重新输入！";
                        return;
                    }
                    else
                    {

                        //运行测试
                        btnRunTest.Enabled = groupsList[0].Test_Bool;
                        //测试结构
                        btnEditTest.Enabled = menuTest.Enabled = groupsList[0].Build_Bool;
                        //用户管理
                        btnUserManage.Enabled = groupsList[0].Manage_Bool;
                        //更改密码
                        btnChangePSW.Enabled = groupsList[0].Manage_Bool;
                        //记录查询
                        btnHWDebug.Enabled = groupsList[0].Query_Bool;

                        menuUserManage.Enabled = btnUserManage.Enabled;
                        menuHardware.Enabled = btnHWDebug.Enabled;
                        menuEditTest.Enabled = btnEditTest.Enabled;
                        menuRunTest.Enabled = btnRunTest.Enabled;
                    }



                    lblUserName.Text = "欢迎你，" + StepNamedt.Rows[0]["Name"].ToString();
                    lblPassword.Visible = false;
                    txtUserName.Visible = false;
                    txtPassword.Visible = false;
                    separator.Visible = false;
                    lstTS.Enabled = true;

                    if (btnEditTest.Enabled)
                    {
                        btnAddTestSeq.Enabled = true;
                        btnDelTestSeq.Enabled = true;
                        btnSeqDown.Enabled = true;
                        btnSeqUp.Enabled = true;

                    }




                }
                else
                {
                    lblInfo.Text = "用户名密码错误,请重新输入！";
                    return;
                }

            }
            else
            {
                lblInfo.Text = "用户名密码错误,请重新输入！";
                return;
            }










            btnLogin.Enabled = false;
            InitTestSeqList();

        }


        /// <summary>
        /// 初始测试序列列表
        /// </summary>
        private void InitTestSeqList()
        {
            lstTS.Items.Clear();
            for (int i = 0; i < TShandler.TSCount; i++)
            {
                lstTS.Items.Add(TShandler.GetTS_Name(i + 1));
            }
            if (lstTS.Items.Count > 0)
                lstTS.SelectedIndex = 0;
            //btnHWDebug.Enabled = btnEditTest.Enabled = btnRunTest.Enabled = (lstTS.Items.Count > 0);
            btnHWDebug.Enabled = btnRunTest.Enabled = (lstTS.Items.Count > 0);
            menuRunTest.Enabled = btnRunTest.Enabled;

            lstTS.Focus();
        }


        private void menuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Login();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Login();
            }
        }

        private void btnUserManage_Click(object sender, EventArgs e)
        {
            frmUserManager frm = new frmUserManager();
            // frm.m_xmlusers = m_UserManage;
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btnEditTest_Click(object sender, EventArgs e)
        {
            ProjectFileEditor.frmMain frm = new ProjectFileEditor.frmMain();

            this.Hide();
            if (lstTS.SelectedItems.Count == 0)
            {
                frm.ShowDialog();
            }
            else
            {
                frm.LoadFile(
                    TShandler.GetTS_Name(lstTS.SelectedIndex + 1),
                    TShandler.GetTS_FilePath(lstTS.SelectedIndex + 1)
                    );
            }

            this.Show();

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Enabled = (txtUserName.Text.Length > 0);
        }

        private void frmStart_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                m_UserManage.Close();
                System.Environment.Exit(0);
            }
            catch (System.Exception)
            {
                Application.Exit();
                System.Environment.Exit(0);
            }
        }

        private void btnSeqUp_Click(object sender, EventArgs e)
        {
            int Index = lstTS.SelectedIndex;

            if (Index <= 0) return;

            lstTS.Tag = "Wait";
            TShandler.MoveUp(Index + 1);
            lstTS.Items.Insert(Index - 1, lstTS.SelectedItem);
            lstTS.Items.RemoveAt(Index + 1);
            lstTS.Tag = "";
            lstTS.SelectedIndex = Index - 1;
        }

        private void btnAddTestSeq_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "*.prj";
            dialog.Filter = "项目文件(*.prj)|*.prj";
            //dialog.InitialDirectory = "";
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            dialog.InitialDirectory = Application.StartupPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string FileName = dialog.FileName;
                string shortName = dialog.SafeFileName;
                shortName = shortName.Remove(shortName.Length - 4);

                TShandler.AddTS(shortName, @FileName);
                lstTS.Items.Add(shortName);
                lstTS.SelectedIndex = lstTS.Items.Count - 1;
            }
            btnHWDebug.Enabled = btnEditTest.Enabled = btnRunTest.Enabled = (lstTS.Items.Count > 0);
            menuRunTest.Enabled = btnRunTest.Enabled;

        }

        private void btnDelTestSeq_Click(object sender, EventArgs e)
        {
            if (lstTS.SelectedIndex == -1) return;
            int Index = lstTS.SelectedIndex;

            lstTS.Tag = "Wait";
            TShandler.RemoveTS(Index + 1);
            lstTS.Items.RemoveAt(Index);

            if (Index > lstTS.Items.Count - 1)
            {
                lstTS.SelectedIndex = lstTS.Items.Count - 1;
            }
            else
            {
                lstTS.SelectedIndex = Index;
            }
            lstTS.Tag = "";

            btnRunTest.Enabled = (lstTS.Items.Count > 0);
            btnHWDebug.Enabled = btnEditTest.Enabled = menuRunTest.Enabled = btnRunTest.Enabled;
        }

        private void btnSeqDown_Click(object sender, EventArgs e)
        {
            int Index = lstTS.SelectedIndex;

            if (Index == -1 || Index == lstTS.Items.Count - 1) return;

            lstTS.Tag = "Wait";
            TShandler.MoveDown(Index + 1);
            lstTS.Items.Insert(Index + 2, lstTS.SelectedItem);
            lstTS.Items.RemoveAt(Index);
            lstTS.Tag = "";

            lstTS.SelectedIndex = Index + 1;
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {


            this.Hide();
            Dictionary<int, string> dicPath = TShandler.GetTS_FilePathLst();
            frm = new Test.ProjectTest.frmTest_Wide();
            frm.mUser.Name = userName;
            frm.mUser.Description = m_UserManage.Description(userName);
            frm.m_dbConnection = m_UserManage.DbConnection;

            frm.ProjectFile = TShandler.GetTS_FilePath(lstTS.SelectedIndex + 1);
            frm.ShowDialog();
            this.Show();
        }

        private void lstTS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((lstTS.Tag != null) && (lstTS.Tag.ToString() == "Wait")) return;
            if (lstTS.Items.Count == 0) return;

            try
            {
                statusPath.Text = TShandler.GetTS_FilePath(lstTS.SelectedIndex + 1);
                //toolStripStatusLabel1.Text = "文件路径：" + statusPath.Text;
                Project prj = new Project();
                prj.File = statusPath.Text;
                prj.Load();
                picTestPrj.Image = prj.mImage;
                lblDUTNumber.Text = "产品号： " + prj.PartNumber;
                lblDUTName.Text = "产品名称：" + prj.Description;
                lblDUTVer.Text = "版本：" + prj.Version;
                //statusPath.Text = @"E:\UTOP-FOB标定异步取放点检功能\UTOP_V5.1 151020\Release\Projects\SGM358_FOB_6SW_PCB标定.";
                int a = statusPath.Text.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmStart_Shown(object sender, EventArgs e)
        {
            if (!m_UserManage.Connect())
            {


                MessageBox.Show("数据库连接失败，程序将退出！");
                this.Close();


                //    dbserver = ".";
                //public static string dbname = "Utop_New";
                //public static string userid = "sa";
                //public static string password = "1234";
            }
            string a = m_UserManage.DbConnection.ConnectionString;
            Console.WriteLine(a);
            //do
            //{
            //    Application.DoEvents();
            //    System.Threading.Thread.Sleep(50);
            //    if (MdlClass.IsStartTesting)
            //    {
            //        break;
            //    }
            //} while (true);

            //System.Threading.Thread.Sleep(2000);
            //btnRunTest_Click(null, null);
        }

        private void btnHWDebug_Click(object sender, EventArgs e)
        {
            frmQueryNew frm = new frmQueryNew();
            this.Hide();
            frm.RunTest(TShandler.GetTS_FilePath(lstTS.SelectedIndex + 1));
            frm.UserName = userName;
            frm.ShowDialog(this);
            this.Show();
        }

        private void btnChangePSW_Click(object sender, EventArgs e)
        {
            frmSetPassword frm = new frmSetPassword();
            frm.m_xmlusers = m_UserManage;
            frm.UserName = userName;
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {

        }

        //int Slavestationsate = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {



        }



        bool HeartRes = false;
        //void HeartBeat()
        //{

        //    MdlClass.TcpipServer.MyStationState.IsInTestForm = false;
        //    do
        //    {
        //       HeartRes=    MdlClass.TcpipServer.SendHeartBeat(0);
        //       if (!HeartRes)
        //       {

        //           timer1.Stop();
        //           SuspendRun();
        //           //MessageBox.Show("服务器已断开", "提示");
        //           System.Environment.Exit(0);
        //       }
        //       System.Threading.Thread.Sleep(800);

        //       Console.WriteLine("Beat");
        //    }while(true);
        //}

        private void frmStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            //MdlClass.webserver.CloseConect();
        }
        System.Threading.Thread th = null;
        public void SuspendRun()
        {

            if (frm != null)
            {
                if (th == null)
                {
                    th = new System.Threading.Thread(frm.SuspendThread);
                    th.Start();
                    frm = null;
                }

                else
                {
                    if (!th.IsAlive)
                    {
                        th = new System.Threading.Thread(frm.SuspendThread);
                        th.Start();
                        frm = null;
                    }
                }
            }
        }
        public void StopRun()
        {

            if (frm != null)
            {
                if (th == null)
                {
                    th = new System.Threading.Thread(frm.EndThread);
                    th.Start();
                    frm = null;
                }

                else
                {
                    if (!th.IsAlive)
                    {
                        th = new System.Threading.Thread(frm.EndThread);
                        th.Start();
                        frm = null;
                    }
                }
            }
        }



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.Hide();
        //    Dictionary<int, string> dicPath = TShandler.GetTS_FilePathLst();
        //    frm = new ProjectTest.frmTest_Wide();
        //    frm.m_dbConnection = m_UserManage.DbConnection;
        //    frm.RunTest_WideInit(dicPath);
        //    this.Show();
        //}


    }
}
