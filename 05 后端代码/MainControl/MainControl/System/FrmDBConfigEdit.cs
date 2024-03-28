using Common;
using Common.SysConfig.Model;
using MainControl;
using MainControl.BLL;
using MainControl.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MainControl
{
    public partial class FrmDBConfigEdit : WindowParent
    {
        SysLogService System_Bll = new SysLogService();
        string title = string.Empty;

        string connectionString= SysAppConfig.appCnfigDoc.AppConfigGet("MySqlConnection");
        public FrmDBConfigEdit()
        {
            InitializeComponent();
            InitView(this);//初始化标题放大缩小
            this.Text = "数据库信息-编辑";
            title = this.Text;
        }
       
        

        #region 保存事件
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!SaveConfig())
                {
                    MessageBox.Show("保存失败！");
                }
                else
                {
                    
                    System_Bll.AddLog(new SysLogModel
                    {
                        CreateUserID = GlobalUserHandle.LoginUserID,
                        CreateUserName = GlobalUserHandle.loginUserName,
                        LocalIP = GlobalUserHandle.LocalIP,
                        Module = title,
                        Method = MethodBase.GetCurrentMethod().Name,
                        LogMessage = $"{title}-数据库链接字符串！",
                        Type = "编辑！",
                        ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                    });

                    MessageBox.Show("保存成功！");
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                System_Bll.AddLog(new SysLogModel
                {
                    CreateUserID = GlobalUserHandle.LoginUserID,
                    CreateUserName = GlobalUserHandle.loginUserName,
                    LocalIP = GlobalUserHandle.LocalIP,
                    Module = title,
                    Method = MethodBase.GetCurrentMethod().Name,
                    LogMessage = $"{title}-{ex.Message}！",
                    Type = "系统错误！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 保存数据库文件
        /// </summary>
        /// <returns></returns>
        public bool SaveConfig()
        {
            connectionString = $"server={txtServerName.Text};database={txtDBName.Text};uid={txtUserID.Text};pwd={txtPassWord.Text};min pool size=50; Max Pool Size=512;Integrated Security=false";
            SysAppConfig.appCnfigDoc.AppConfigSet("MySqlConnection", connectionString);
            //Program.CurrentConfig.DataSource = txtServerName.Text;
            //Program.CurrentConfig.InitialCatalog = txtDBName.Text;
            //Program.CurrentConfig.UserID = txtUserID.Text;
            //Program.CurrentConfig.Password = txtPassWord.Text;
            ////   CurrentConfig.localStlst.Clear();
            ////foreach (DataGridViewRow dr in dgvlocalStConfig.Rows)
            ////{
            ////    LocalStationConfig lst = new LocalStationConfig();
            ////    lst.StationNum = dr.Cells[0].Value.ToString();
            ////    lst.StationName = dr.Cells[1].Value.ToString();
            ////    CurrentConfig.localStlst.Add(lst);
            ////}
            //return Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            return true;

        }

        
        public bool LoadConfig()
        {
            var parts = connectionString.Split(';');

            string server = null, database = null, uid = null, pwd = null;
            if(parts!=null&&parts.Length>0)
            { 
                foreach (var part in parts)
                {
                    var keyValue = part.Trim().Split('=');
                    if (keyValue.Length == 2)
                    {
                        switch (keyValue[0].ToLower())
                        {
                            case "server":
                                server = keyValue[1];
                                break;
                            case "database":
                                database = keyValue[1];
                                break;
                            case "uid":
                                uid = keyValue[1];
                                break;
                            case "pwd":
                                pwd = keyValue[1];
                                break;
                                // 可以添加其他可能的键来处理  
                        }
                    }
                }
            
                txtServerName.Text = server;
                txtDBName.Text = database;
                txtUserID.Text = uid;
                txtPassWord.Text = pwd;
               return true;
            }
            else
            {
                return false;
            }
            
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDBConfigEdit_Load(object sender, EventArgs e)
        {
            //if (!Directory.Exists(MdlClass.SysConfigPath))
            //{
            //    //如果不包含
            //    DialogResult dr = MessageBox.Show("总体配置的文件夹不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
            //    if (dr == DialogResult.Yes)
            //    {
            //        Directory.CreateDirectory(MdlClass.SysConfigPath);
            //        //创建文件
            //        //NewConfig();
            //        Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                   
            //    }
            //    else
            //    {
            //        this.Close();
            //    }
            //}
            //else
            //{
            //    //载该文件

            //    if (File.Exists(MdlClass.SysConfigPath + @"\SysConfig.cfg"))
            //    {
            //        //CurrentConfig = new SystemConfig();
            //        Program.CurrentConfig = Program.CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");

            //        if (Program.CurrentConfig == null)
            //        {
            //            DialogResult dr = MessageBox.Show("总体配置的文件载入失败，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
            //            if (dr == DialogResult.Yes)
            //            {
            //                //NewConfig();

            //                Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                            
            //            }
            //            else
            //            {
            //                this.Close();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        DialogResult dr = MessageBox.Show("总体配置的文件不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
            //        if (dr == DialogResult.Yes)
            //        {
            //            //NewConfig();

            //            Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                       
            //        }
            //        else
            //        {
            //            this.Close();
            //        }
            //    }
            //}
            LoadConfig();
        }
        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
