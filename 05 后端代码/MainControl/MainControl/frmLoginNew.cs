﻿using Common;
using Common.SysConfig.Model;
using MainControl.BLL;
using MainControl.Entity;
using ORMSqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace MainControl
{
    public partial class frmLoginNew : Form
    {
        SqlsugarMyClient helper=new SqlsugarMyClient();

        UserService bll= new UserService();
        SysLogService System_Bll = new SysLogService();
        SystemConfig sysConfig = new SystemConfig();
        string title = string.Empty;
        string LocalIP=string.Empty;

        string connectionString = SysAppConfig.appCnfigDoc.AppConfigGet("MySqlConnection");
        public frmLoginNew()
        {
            InitializeComponent();
            title = this.Text;

            // 获取本机的主机名
            string hostname = Dns.GetHostName();

            // 获取本机的IP地址列表
            IPAddress[] addresses = Dns.GetHostAddresses(hostname);

            // 获取第一个IPv4地址
            IPAddress ipv4Address = Array.Find(addresses, address => address.AddressFamily == AddressFamily.InterNetwork);
            LocalIP=ipv4Address.ToString();
        }
        /// <summary>
        /// 加载数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                MdlClass.sysSet = MdlClass.sysSet.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                if (String.IsNullOrEmpty(connectionString))
                {
                    lblMessage.Text = "缺少配置链接配置:MainControl.exe.config！";
                    return;
                }
                //Common.GlobalResources.dbserver = MdlClass.sysSet.DataSource;
                //Common.GlobalResources.dbname = MdlClass.sysSet.InitialCatalog;
                //Common.GlobalResources.userid = MdlClass.sysSet.UserID;
                //Common.GlobalResources.password = MdlClass.sysSet.Password;
                lblMessage.Text = "正在连接数据库。。。";
                Application.DoEvents();
                Thread.Sleep(50);
                //GlobalResources.SqlConnectTest();

                bool IsValidConnection = helper.GetClient().Ado.IsValidConnection();
                //if (!GlobalResources.SqlConnectTest())
                if (!IsValidConnection)
                {
                    lblMessage.Text = "连接数据库失败！";

                    DialogResult dr = MessageBox.Show("数据库链接失败，是否配置数据库链接字符串.", "提示：", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        FrmDBConfigEdit fm = new FrmDBConfigEdit();
                        fm.StartPosition = FormStartPosition.CenterParent;
                        fm.ShowDialog();

                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    lblMessage.Text = "连接数据库成功！";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "连接数据库失败！";
                helper = new SqlsugarMyClient();
                bool IsValidConnection = helper.GetClient().Ado.IsValidConnection();
                if (IsValidConnection)
                {
                    lblMessage.Text = "连接数成功！";
                    bll = new UserService();
                    System_Bll = new SysLogService();
                    System_Bll.AddLog(new SysLogModel
                    {
                        CreateUserID = 0,
                        CreateUserName = "",
                        LocalIP = LocalIP,
                        Module = title,
                        Method = MethodBase.GetCurrentMethod().Name,
                        LogMessage = ex.Message,
                        Type = "系统错误！",
                        ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                    });
                }
                else
                {
                    
                }
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
                bool IsValidConnection = helper.GetClient().Ado.IsValidConnection();
                if (IsValidConnection)
                {
                    System_Bll.AddLog(new SysLogModel
                    {
                        CreateUserID = GlobalUserHandle.LoginUserID,
                        CreateUserName = GlobalUserHandle.loginUserName,
                        LocalIP = LocalIP,
                        Module = title,
                        Method = MethodBase.GetCurrentMethod().Name,
                        LogMessage = "关闭登录",
                        Type = "登录！",
                        ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                    });
                }
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
                lblMessage.Text = "用户名不能为空，请输入用户名！";
                txt_UserName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(Password))
            {
                lblMessage.Text = "密码不能为空，请输入密码！";
                txt_Password.Focus();
                return;
            }

            try
            {

                //helper = new SqlsugarMyClient();
                bool IsValidConnection = helper.GetClient().Ado.IsValidConnection();
                if (IsValidConnection)
                {
                    lblMessage.Text = "连接数成功！";
                    bll = new UserService();
                    System_Bll = new SysLogService();
                }
                else
                {
                    DialogResult dr = MessageBox.Show("数据库链接失败，是否配置数据库链接字符串.", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        FrmDBConfigEdit fm = new FrmDBConfigEdit();
                        fm.StartPosition = FormStartPosition.CenterParent;
                        fm.ShowDialog();

                    }
                    else
                    {
                        this.Close();
                    }
                }

                UserModel m= bll.GetUserLoginInfo(UserName, DESEncrypt.Encrypt(Password, "1"));

                if (m != null)
                {
                    GlobalUserHandle.LoginUserID= m.UserID;
                    GlobalUserHandle.UserNum = m.UserNum;
                    GlobalUserHandle.loginUserName = m.UserName;
                    GlobalUserHandle.Password = m.Password;
                    GlobalUserHandle.LocalIP = LocalIP;
                    GlobalUserHandle.RoleID = m.RoleID;

                    if (m.Password.ToUpper() != DESEncrypt.Encrypt(Password, "1").ToUpper())
                    {
                        lblMessage.Text = "用户名密码错误，请重新输入！";
                        return;
                    }
                    else
                    {
                        Program.CurrentConfig = sysConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                        this.Visible = false;
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = "登录成功",
                            Type = "登录！",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });

                        //进入主窗口
                        frmMainNew fm = new frmMainNew();
                        fm.ShowDialog();

                        //关闭登录框
                        this.Close();
                        

                    }
                }
                else
                {
                    lblMessage.Text="用户名或密码错误，请重新输入！";
                    txt_UserName.Focus();
                    txt_UserName.SelectAll();
                    return;
                }
                
            }
            catch (Exception ex){

                lblMessage.Text=$"系统错误：{ex.Message}！";
                txt_UserName.Focus();
                //txt_UserName.SelectAll();
                System_Bll.AddLog(new SysLogModel
                {
                    CreateUserID = GlobalUserHandle.LoginUserID,
                    CreateUserName = GlobalUserHandle.loginUserName,
                    LocalIP = LocalIP,
                    Module = title,
                    Method = MethodBase.GetCurrentMethod().Name,
                    LogMessage = $"系统错误:{ex.Message}",
                    Type = "登录！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });


            }
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
