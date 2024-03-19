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

namespace MainControl
{
    public partial class FrmUserEdit : Form
    {
        //FrmUserInfo _frmUserInfo;
        //SystemAppendProperty_Bll app_bll = new SystemAppendProperty_Bll();
        //SystemMenu_Bll menu_bll = new SystemMenu_Bll();
        //SystemUserInfo_Bll user_bll = new SystemUserInfo_Bll();
        //SystemOrganization_Bll org_bll = new SystemOrganization_Bll();
        //SystemRole_Bll role_bll = new SystemRole_Bll();
        string User_ID = "";
        //OperationType type;
        public FrmUserEdit()
        {
           
            //this._frmUserInfo = frmUserInfo;
            //InitializeComponent();
            this.Text = "用户信息-添加";
            //type = OperationType.Add;
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
                //Base_UserInfo userinfo = new Base_UserInfo();
                //if (!string.IsNullOrEmpty(User_ID))
                //{
                //    userinfo.ModifyDate = DateTime.Now;
                //    userinfo.ModifyUserId = FrmLogin.LoginUserID;
                //    userinfo.ModifyUserName = FrmLogin.loginUserName;

                //}
                //else
                //{
                //    userinfo.CreateDate = DateTime.Now;
                //    userinfo.CreateUserId = FrmLogin.LoginUserID;
                //    userinfo.CreateUserName = FrmLogin.loginUserName;

                //    User_ID = Guid.NewGuid().ToString();
                //}
                //userinfo.DeleteMark = 1;
                //userinfo.Email = txt_Email.Text;
                //userinfo.Title = txt_Title.Text;
                //userinfo.User_Account = txt_User_Account.Text;
                //userinfo.User_Code = txt_User_Code.Text;
                //userinfo.User_ID = User_ID;
                //userinfo.User_Name = txt_User_Name.Text;
                ////userinfo.User_Pwd =Md5Helper.Md5( txt_User_Pwd.Text);
                //userinfo.User_Remark = richTextBox1.Text;
                //userinfo.User_Sex = com_User_Sex.Text;
                //StaffOrgList.Clear();
                //UserRightList.Clear();
                //RoleList.Clear();
                //CheckRoleTreeViewNode(tree_UserRole.Nodes);
                //CheckUsertRightTreeViewNode(this.tree_UserRight.Nodes);//获取勾选值
                //CheckUsertDeptTreeViewNode(this.tree_Dept.Nodes);//获取勾选值
                //GetAllAppendValue();//获取附加属性值
                //userinfo.Base_UserRight = UserRightList;
                //userinfo.Base_StaffOrganize = StaffOrgList;
                //userinfo.Base_UserRole = RoleList;
                //userinfo.Base_AppendPropertyInstance = appendList;
                //int isOk = 0;
                //if (type == OperationType.Add)
                //{
                //    isOk = user_bll.AddUserInfo(userinfo);
                //}
                //else
                //{
                //    isOk = user_bll.EditUserInfo(userinfo);
                //}

                //if (isOk > 0)
                //{
                //    MessageBox.Show("保存成功！");
                //    this.Close();
                //}
                //else
                //{
                //    MessageBox.Show("保存失败！");
                //}
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
