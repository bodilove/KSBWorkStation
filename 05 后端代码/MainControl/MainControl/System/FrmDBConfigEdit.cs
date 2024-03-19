using Common.SysConfig.Model;
using MainControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainControl
{
    public partial class FrmDBConfigEdit : Form
    {
        public FrmDBConfigEdit()
        {
            InitializeComponent();
            this.Text = "数据库信息-编辑";
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
                    //DialogResult dr = MessageBox.Show("保存成功！", "提示：", MessageBoxButtons.YesNo);
                    //if (dr == DialogResult.Yes)
                    //{
                    //  //Application.Exit();

                    //}
                    //else
                    //{
                    //    this.Close();
                    //}
                    MessageBox.Show("保存成功！");
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                //System_Bll.WriteLogToDB(new Entity.Base_Log
                //{
                //    CreateUserID = FrmLogin.LoginUserID,
                //    CreateUserName = FrmLogin.loginUserName,
                //    LocalIP = FrmLogin.LocalIP,
                //    LogMessage = ex.Message,
                //    Type = "系统错误！",
                //    ClassName = typeof(FrmDBConfigEdit).ToString()
                //});
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 保存数据库文件
        /// </summary>
        /// <returns></returns>
        public bool SaveConfig()
        {
            
            Program.CurrentConfig.DataSource = txtServerName.Text;
            Program.CurrentConfig.InitialCatalog = txtDBName.Text;
            Program.CurrentConfig.UserID = txtUserID.Text;
            Program.CurrentConfig.Password = txtPassWord.Text;
            //   CurrentConfig.localStlst.Clear();
            //foreach (DataGridViewRow dr in dgvlocalStConfig.Rows)
            //{
            //    LocalStationConfig lst = new LocalStationConfig();
            //    lst.StationNum = dr.Cells[0].Value.ToString();
            //    lst.StationName = dr.Cells[1].Value.ToString();
            //    CurrentConfig.localStlst.Add(lst);
            //}
            return Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            
        }

        //public SystemConfig CurrentConfig = null;
        //DataGridView CurrentDgv = null;
        //LocalStationConfig CurrenLocalStationConfig = null;

        //public void NewConfig()
        //{
        //    SystemConfig gf = new SystemConfig();
        //    CurrentConfig = gf;

        //}


        public bool LoadConfig()
        {
            //CurrentConfig = CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            if (Program.CurrentConfig != null)
            {
                txtServerName.Text = Program.CurrentConfig.DataSource;
                txtDBName.Text = Program.CurrentConfig.InitialCatalog;
                txtUserID.Text = Program.CurrentConfig.UserID;
                txtPassWord.Text = Program.CurrentConfig.Password;
                //dgvlocalStConfig.Rows.Clear();

                //foreach (LocalStationConfig lst in CurrentConfig.localStlst)
                //{
                //    dgvlocalStConfig.Rows.Add(new object[] { lst.StationNum, lst.StationName });
                //}
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDBConfigEdit_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(MdlClass.SysConfigPath))
            {
                //如果不包含
                DialogResult dr = MessageBox.Show("总体配置的文件夹不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Directory.CreateDirectory(MdlClass.SysConfigPath);
                    //创建文件
                    //NewConfig();
                    Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                   
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                //载该文件

                if (File.Exists(MdlClass.SysConfigPath + @"\SysConfig.cfg"))
                {
                    //CurrentConfig = new SystemConfig();
                    Program.CurrentConfig = Program.CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");

                    if (Program.CurrentConfig == null)
                    {
                        DialogResult dr = MessageBox.Show("总体配置的文件载入失败，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            //NewConfig();

                            Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                            
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("总体配置的文件不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        //NewConfig();

                        Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                       
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
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
