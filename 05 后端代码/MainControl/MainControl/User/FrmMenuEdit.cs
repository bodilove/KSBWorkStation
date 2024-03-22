using Common;
using MainControl.BLL;
using MainControl.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace MainControl
{
    public partial class FrmMenuEdit : Form
    {
        SysMenuService bll = new SysMenuService();
        SysLogService System_Bll = new SysLogService();

        FrmSysMenuInfo _frm;
        OperationType type;
        int MenuID = -1;

        SysMenuModel m;
        
        string title = string.Empty;

        #region 订阅窗口关闭事件
        // 声明一个事件委托
        public delegate void DataUpdatedEventHandler(object sender, EventArgs e);
        // 声明一个事件
        public event DataUpdatedEventHandler DataUpdated;
        // 在关闭窗口前触发事件
        private void FrmEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 触发事件
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        public FrmMenuEdit(FrmSysMenuInfo frm)
        {
         
            type = OperationType.Add;
            //m = new SysMenuModel();
            this._frm = frm;
            InitializeComponent();

            this.Text = "菜单信息-添加";
            this.txt_parent.Text = _frm.treeView1.SelectedNode.Text;

            title = this.Text;
        }
        public FrmMenuEdit(FrmSysMenuInfo frm, ref DataGridViewRow dvr)
        {
            try
            {
                type = OperationType.Edit;
                this._frm = frm;
                InitializeComponent();
                

                m = dvr.DataBoundItem as SysMenuModel;
                //  this.com_Parent.Text=dvr.Cells["节点位置"].Value==null?"": dvr.Cells["节点位置"].Value.ToString();
                this.txt_parent.Text = _frm.treeView1.SelectedNode.Text;
                this.txt_MenuName.Text = m.Menu_Name.ToStringExt();
                txt_MenuTag.Text = m.Menu_Tag.ToStringExt();
                txt_Sort.Text = m.SortCode.ToStringExt();
                pic_menu.Image = new FrmListImages(this).imageList1.Images[m.Menu_Img.StrToInt(0)];
                pic_menu.Tag = m.Menu_Img;
                MenuID = m.Menu_Id.StrToInt(-1);


                this.Text = "菜单信息-编辑";
                title = this.Text;
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
                    Type = "系统错误",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
        }

    
   
        private void FrmMenuEdit_Load(object sender, EventArgs e)
        {

            //var ParentID= _frmMenu.treeView1.SelectedNode==null?"": _frmMenu.treeView1.SelectedNode.Tag;
            //com_Parent.DataSource = bll.GetSysMenusParent(FrmLogin.LoginUserID);
            //com_Parent.DisplayMember = "Menu_Name";
            //com_Parent.ValueMember = "Menu_Id";
            //com_Parent.SelectedValue = ParentID;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                int isOk = 0;
                if (type == OperationType.Add)
                {
                    SysMenuModel m = new SysMenuModel
                    {
                        CreateDate = DateTime.Now,
                        CreateUserId = GlobalUserHandle.LoginUserID,
                        CreateUserName = GlobalUserHandle.loginUserName,
                        Menu_Img = int.Parse(pic_menu.Tag.ToString()),
                        Menu_Name = txt_MenuName.Text,
                        Menu_Tag = txt_MenuTag.Text,
                        ParentId = _frm.treeView1.SelectedNode.Tag.StrToInt(-1),
                        SortCode = int.Parse(txt_Sort.Text),
                        //Menu_Id = Guid.NewGuid().ToString()

                    };

                    if (bll.IsExistMenuName(m.Menu_Name,m.ParentId))
                    {
                        MessageBox.Show($"菜单：{m.Menu_Name}，已经存在，无法重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = $"{title}-菜单：{m.Menu_Name} 已经存在，无法重复添加！",
                            Type = "添加",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        return;
                    }
                    else
                    {
                        isOk = bll.Add(m);
                    }
                    
                  if (isOk == 1)
                  {
                      MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // _frm.dataGridView1.DataSource = bll.GetSysMenuChilds(_frm.treeView1.SelectedNode.Tag.ToString(), GlobalUserHandle.LoginUserID);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = $"{title}-菜单：{m.Menu_Name} 添加成功！",
                            Type = "添加",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        
                        this.Close();
                        return;
                    }
                  else
                  {
                      MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                }
                else
                {
                    SysMenuModel m = new SysMenuModel
                    {

                        Menu_Img = int.Parse(pic_menu.Tag.ToString()),
                        Menu_Name = txt_MenuName.Text,
                        Menu_Tag = txt_MenuTag.Text,
                        ParentId = _frm.treeView1.SelectedNode.Tag.StrToInt(-1),
                        SortCode = int.Parse(txt_Sort.Text),
                        ModifyUserName = GlobalUserHandle.loginUserName,
                        ModifyDate = DateTime.Now,
                        ModifyUserId = GlobalUserHandle.LoginUserID,
                        Menu_Id = MenuID
                    };
                    isOk =  bll.Edit(m);
                    if (isOk == 1)
                    {
                        MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // _frm.dataGridView1.DataSource = bll.GetSysMenuChilds(_frm.treeView1.SelectedNode.Tag.ToString(),FrmLogin.LoginUserID);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = $"{title}-菜单：{m.Menu_Name} 修改成功！",
                            Type = "修改",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });


                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                    Type = "系统错误",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
           
        }

        private void pic_menu_Click(object sender, EventArgs e)
        {
            FrmListImages image = new FrmListImages(this);
            image.ShowDialog();
        }

        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
