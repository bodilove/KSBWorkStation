using Common;
using MainControl.BLL;
using MainControl.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using YX.BLL;
//using YX.Common.DotNetEncrypt;
//using YX.Entity;

namespace MainControl.User
{
    public partial class FrmUserInfoEdit : Form
    {
        UserService bll=new UserService();
        RoleService role_bll = new RoleService();
        FrmUserInfo _frmUserInfo;
        //SystemAppendProperty_Bll app_bll = new SystemAppendProperty_Bll();
        //SystemMenu_Bll menu_bll = new SystemMenu_Bll();
        //SystemUserInfo_Bll user_bll = new SystemUserInfo_Bll();
        //SystemOrganization_Bll org_bll = new SystemOrganization_Bll();
        //SystemRole_Bll role_bll = new SystemRole_Bll();

        string UserID = "";
        OperationType type;

        UserModel m ;
        #region 订阅窗口关闭事件
        // 声明一个事件委托
        public delegate void DataUpdatedEventHandler(object sender, EventArgs e);
        // 声明一个事件
        public event DataUpdatedEventHandler DataUpdated;
        // 在关闭窗口前触发事件
        private void FrmUserInfoEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 触发事件
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        public FrmUserInfoEdit(FrmUserInfo frmUserInfo)
        {
            InitializeComponent();
            Init();

            type = OperationType.Add;

            m = new UserModel();
            this._frmUserInfo = frmUserInfo;

            this.Text = "用户信息-添加";
        }
        public FrmUserInfoEdit(FrmUserInfo frmUserInfo, ref DataGridViewRow dvr)
        {
            InitializeComponent();
            Init();

            type = OperationType.Edit;
            m = dvr.DataBoundItem as UserModel;
            this._frmUserInfo = frmUserInfo;
            UserID = m.UserID.ToStringExt();
            txt_Email.Text = m.Email.ToStringExt();
            txt_User_Account.Text = m.UserNum.ToStringExt();
            txt_User_Name.Text = m.UserName.ToStringExt();
            //Md5Helper.Md5( txt_Password.Text)
            txt_Password.Text = m.Password.ToStringExt();  
            com_User_Sex.Text = m.Gender.ToStringExt();
            richTextBox1.Text = m.Remark.ToStringExt();
            com_RoleId.SelectedValue = m.RoleID == null ? -1 : m.RoleID;


            this.Text = "用户信息-编辑";
        }
        void Init()
        {
            com_RoleId.DataSource = role_bll.QueryList();
            com_RoleId.DisplayMember = "RoleName";
            com_RoleId.ValueMember = "RoleID";
        }

        private void FrmUserInfoEdit_Load(object sender, EventArgs e)
        {
           

            //GetAppendProperty();
            //BindUsertRightTreeView();
            //BindUserDeptTreeView();
            //BindRoleTreeView();
            //if (tree_Dept.Nodes.Count > 0)//展开一级节点
            //{
            //    tree_Dept.Nodes[0].Expand();
            //}
            //if (tree_UserRight.Nodes.Count > 0)//展开一级节点
            //{
            //    tree_UserRight.Nodes[0].Expand();
            //}
            //if (tree_UserRight.Nodes.Count > 0)//展开一级节点
            //{
            //    tree_UserRole.Nodes[0].Expand();
            //}
        }

      

        #region 用户权限



        #endregion



        #region 保存事件
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(UserID))
                {
                   
                    m.ModifyDate = DateTime.Now;
                    //m.ModifyUserId = FrmLogin.LoginUserID;
                    //m.ModifyUserName = FrmLogin.loginUserName;
                }
                else
                {
                    
                    
                    m.CreateDate = DateTime.Now;
                    m.CreateUserId = 1;
                    m.CreateUserName = "超级管理员";
                    //m.CreateUserId = frmLoginNew.LoginUserID;
                    //m.CreateUserName = frmLoginNew.loginUserName;

                }
                m.UserNum = txt_User_Account.Text.ToStringExt();
                m.UserName = txt_User_Name.Text.ToStringExt();
                m.Password = txt_Password.Text.ToStringExt("123456");//Md5Helper.Md5( txt_User_Pwd.Text);
                m.Email = txt_Email.Text.ToStringExt();
                m.Gender = com_User_Sex.Text.ToStringExt();
                m.RoleID = com_RoleId.SelectedValue.StrToInt(-1);
                m.Remark = richTextBox1.Text;
                
                int isOk = 0;
                if (type == OperationType.Add)
                {
                    isOk = bll.Add(m);
                }
                else
                {
                    isOk = bll.Edit(m);
                }

                if (isOk > 0)
                {
                    MessageBox.Show("保存成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
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
        //List<Base_UserRight> UserRightList = new List<Base_UserRight>();
        //List<Base_StaffOrganize> StaffOrgList = new List<Base_StaffOrganize>();
        //List<Base_AppendPropertyInstance> appendList = new List<Base_AppendPropertyInstance>();
        //List<Base_UserRole> RoleList = new List<Base_UserRole>();
        /// <summary>
        /// 遍历用户权限选中节点
        /// </summary>
        /// <param name="node"></param>
        public void CheckUsertRightTreeViewNode(TreeNodeCollection node)
        {
       
            foreach (TreeNode n in node)
            {
                if (n.Checked)
                {
                    //UserRightList.Add(new Base_UserRight { UserRight_ID = Guid.NewGuid().ToString(), Menu_Id = n.Tag.ToString(),
                    //    CreateUserName=FrmLogin.loginUserName, CreateUserId=FrmLogin.LoginUserID, CreateDate=DateTime .Now });
                }
                CheckUsertRightTreeViewNode(n.Nodes);
            }
        }
        /// <summary>
        /// 遍历所属部门选中节点
        /// </summary>
        /// <param name="node"></param>
        public void CheckUsertDeptTreeViewNode(TreeNodeCollection node)
        {
        
            foreach (TreeNode n in node)
            {
                if (n.Checked)
                {
                    //StaffOrgList.Add(new Base_StaffOrganize
                    //{
                    //    StaffOrganize_Id = Guid.NewGuid().ToString(),
                    //    Organization_ID = n.Tag.ToString(),
                    //    CreateUserName = FrmLogin.loginUserName,
                    //    CreateUserId = FrmLogin.LoginUserID,
                    //    CreateDate = DateTime.Now 
                      
                    //});
                }
                CheckUsertDeptTreeViewNode(n.Nodes);
            }
        } 
        /// <summary>
        /// 遍历附加属性
        /// </summary>
        public void GetAllAppendValue()
        {
            //foreach(var control in this.flowLayoutPanel1.Controls)
            //{
            //    //遍历所有TextBox...
            //   // if (control is TextBox)
            //   // {
            //   //     TextBox t = (TextBox)control;
            //   //     if(!string.IsNullOrEmpty(t.Text))
            //   //     {
            //   //         appendList.Add(new Base_AppendPropertyInstance { PropertyInstance_ID = Guid.NewGuid().ToString(), Property_Control_ID = t.Name, PropertyInstance_Value = t.Text });
            //   //     }
              
            //   // }
            //   // if(control is RichTextBox)
            //   // {
            //   //     RichTextBox r = (RichTextBox)control;
            //   //     if (!string.IsNullOrEmpty(r.Text))
            //   //     {
            //   //         appendList.Add(new Base_AppendPropertyInstance { PropertyInstance_ID = Guid.NewGuid().ToString(), Property_Control_ID = r.Name, PropertyInstance_Value = r.Text });
            //   //     }
                  
            //   // }
            //   // if(control is ComboBox)
            //   // {
            //   //     ComboBox c = (ComboBox)control;
            //   //     if(!string.IsNullOrEmpty(c.Text)&& c.Text!= "==请选择==")
            //   //     {
            //   //         appendList.Add(new Base_AppendPropertyInstance { PropertyInstance_ID = Guid.NewGuid().ToString(), Property_Control_ID = c.Name, PropertyInstance_Value = c.Text });
            //   //     }
                    
            //   // }
            //   //// 遍历所有DateTimePicker...
            //   // if (control is DateTimePicker)
            //   // {
            //   //     DateTimePicker d = (DateTimePicker)control;
            //   //     if (!string.IsNullOrEmpty(d.Text))
            //   //     {
            //   //         appendList.Add(new Base_AppendPropertyInstance { PropertyInstance_ID = Guid.NewGuid().ToString(), Property_Control_ID = d.Name, PropertyInstance_Value = d.Text });
            //   //     }
            //   // }
            //}
        }
        /// <summary>
        /// 遍历角色权限选中节点
        /// </summary>
        /// <param name="node"></param>
        public void CheckRoleTreeViewNode(TreeNodeCollection node)
        {

            foreach (TreeNode n in node)
            {
                if (n.Checked)
                {
                    //RoleList.Add(new Base_UserRole
                    //{                        
                    //    UserRole_ID = Guid.NewGuid().ToString(),
                         
                    //    //User_ID = User_ID,
                    //    Roles_ID= n.Tag.ToString(),
                    //    CreateUserName = FrmLogin.loginUserName,
                    //    CreateUserId = FrmLogin.LoginUserID,
                    //    CreateDate = DateTime.Now
                    //});
                }
                CheckRoleTreeViewNode(n.Nodes);
            }
        }
        #endregion

        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
