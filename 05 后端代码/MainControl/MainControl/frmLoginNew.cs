using Common;
using Common.SysConfig.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MainControl
{
    public partial class frmLoginNew : Form
    {
        SystemConfig sysConfig = new SystemConfig();
        public frmLoginNew()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            MdlClass.sysSet = MdlClass.sysSet.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            if (MdlClass.sysSet == null)
            {
                lblMessage.Text = "缺少配置文件:SysConfig.cfg！";
                return;
            }
            Common.GlobalResources.dbserver = MdlClass.sysSet.DataSource;
            Common.GlobalResources.dbname = MdlClass.sysSet.InitialCatalog;
            Common.GlobalResources.userid = MdlClass.sysSet.UserID;
            Common.GlobalResources.password = MdlClass.sysSet.Password;
            lblMessage.Text = "正在连接数据库。。。";
            //Application.DoEvents();
            Thread.Sleep(100);
            if (!GlobalResources.SqlConnectTest())
            {
                MessageBox.Show("连接数据库失败");
                lblMessage.Text = "连接数据库失败！";
            }
            else
            {
                lblMessage.Text = "连接数据库成功！";
            }
        }
        /// <summary>
        /// 退出系统按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("确定要退出系统么？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
        }
        /// <summary>
        /// 缩小界面按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        
        /// <summary>
        /// 按下Enter 按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
        /// <summary>
        /// 登录方方法
        /// </summary>
        public void Login()
        {
            string UserName = txt_UserName.Text.Trim();
            string Password = this.txt_Password.Text.Trim();

            if (String.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("用户名不能为空，请输入用户名！", "");
                txt_UserName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(Password))
            {
                MessageBox.Show("密码不能为空，请输入密码！", "");
                txt_Password.Focus();
                return;
            }

            try
            {

                //if (txt_UserName.Text.Trim() != "")
                //{
                    MdlClass.userInfo = MdlClass.userbll.GetUserInfoByUserNum(UserName, Password);
                    if (MdlClass.userInfo != null)
                    {
                        if (MdlClass.userInfo.PassWord.ToUpper() != Password.ToUpper())
                        {
                            MessageBox.Show("用户名密码错误，请重新输入！", "");
                            return;
                        }
                        else
                        {
                            //this.DialogResult = DialogResult.OK;
                            //this.Close();
                            Program.CurrentConfig = sysConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                            this.Visible = false;
                            frmMainNew fm = new frmMainNew();
                            //fm.userName = this.txt_UserName.Text.ToUpper();
                            fm.ShowDialog();
                            this.Close();
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("用户名密码错误，请重新输入！", "");
                        txt_UserName.Focus();
                        txt_UserName.SelectAll();
                        return;
                    }
                //}
            }
            catch (Exception ex){

                MessageBox.Show($"系统错误：{ex.Message}！", "错误");
                txt_UserName.Focus();
                //txt_UserName.SelectAll();
                return;

            }

            //if (this.txt_UserName.Text.Trim() != "")
            //{
       

            //        this.Visible = false;

            //        frmMain fm = new frmMain();
            //        //fm.userName = this.txt_UserName.Text.ToUpper();
            //        fm.ShowDialog();
            //        this.Close();

            //}
            //else 
            //{
            //    MessageBox.Show("请输入用户名密码！", "");
            //    this.txt_UserName.Focus();
            //    this.txt_UserName.SelectAll();
            //    return;
            //}

        }
        /// <summary>
        /// 点击登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_login_Click(object sender, EventArgs e)
        {
            Login();
        }

        
    }
}
