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
//using YX.BLL;
//using YX.Common.DotNetEncrypt;
//using YX.Entity;

namespace MainControl.User
{
    public partial class FrmRoleInfoEdit : Form
    {
       
        RoleService bll = new RoleService();
        SysMenuService menu_bll = new SysMenuService();
        FrmRoleInfo _frm;
        //SystemAppendProperty_Bll app_bll = new SystemAppendProperty_Bll();
        
        //SystemUserInfo_Bll user_bll = new SystemUserInfo_Bll();
        //SystemOrganization_Bll org_bll = new SystemOrganization_Bll();
        //SystemRole_Bll role_bll = new SystemRole_Bll();

        string ID = "";
        OperationType type;

        RoleModel m ;
        SysLogService System_Bll = new SysLogService();
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
        public FrmRoleInfoEdit(FrmRoleInfo frm)
        {
            InitializeComponent();
            Init();

            type = OperationType.Add;

            m = new RoleModel();
            this._frm = frm;

            this.Text = "角色信息-添加";
            title = this.Text;
        }
        public FrmRoleInfoEdit(FrmRoleInfo frm, ref DataGridViewRow dvr)
        {
            InitializeComponent();
            //Init();

            type = OperationType.Edit;
            m = dvr.DataBoundItem as RoleModel;
            this._frm = frm;
            ID = m.RoleID.ToStringExt();
            txt_RoleName.Text = m.RoleName.ToStringExt();
            richTextBox1.Text = m.Remark.ToStringExt();
          
            this.Text = "角色信息-编辑";
            title = this.Text;
        }
        void Init()
        {
            //com_RoleId.DataSource = role_bll.QueryList();
            //com_RoleId.DisplayMember = "RoleName";
            //com_RoleId.ValueMember = "RoleID";
        }

        private void FrmRoleInfoEdit_Load(object sender, EventArgs e)
        {

            BindRoleRightTreeView();
            if (tree_RoleRight.Nodes.Count > 0)
            {
                tree_RoleRight.Nodes[0].Expand();
            }
        }



        #region 用户权限
        /// <summary>
        /// 用户权限菜单树列表
        /// </summary>
        public void BindRoleRightTreeView()
        {
            try
            {
                this.tree_RoleRight.ImageList = imageList1;
                this.tree_RoleRight.Nodes.Clear();
                var list = menu_bll.QueryList();
                var role_right_list = menu_bll.GetRoleRightByCondition(ID.StrToInt(-1));
                var parents = list.Where(o => o.ParentId == 0);
                foreach (var item in parents)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = item.Menu_Name;
                    tn.Tag = item.Menu_Id;
                    tn.ImageIndex = item.Menu_Img == null ? 0 : item.Menu_Img;
                    foreach (var role_right in role_right_list)
                    {
                        if (item.Menu_Id == role_right.Menu_Id)
                        {
                            tn.Checked = true;
                        }
                    }
                    RoleRightFillTree(tn, list, role_right_list);
                    tree_RoleRight.Nodes.Add(tn);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RoleRightFillTree(TreeNode node, List<SysMenuModel> list, List<RoleRightModel> role_right_list)
        {

            var childs = list.Where(o => o.ParentId == node.Tag.StrToInt(-1));
            if (childs.Count() > 0)
            {
                foreach (var item in childs)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = item.Menu_Name;
                    tnn.Tag = item.Menu_Id;
                    tnn.ImageIndex = item.Menu_Img == null ? 0 : item.Menu_Img;
                    foreach (var role_right in role_right_list)
                    {
                        if (item.Menu_Id == role_right.Menu_Id)
                        {
                            tnn.Checked = true;
                        }
                    }
                    if (item.ParentId == node.Tag.StrToInt(-1))
                    {
                        RoleRightFillTree(tnn, list, role_right_list);
                    }
                    node.Nodes.Add(tnn);
                }

            }
        }


        #endregion



        #region 保存事件
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (!string.IsNullOrEmpty(ID))
                {
                   
                    //m.ModifyDate = DateTime.Now;
                    //m.ModifyUserId = FrmLogin.LoginUserID;
                    //m.ModifyUserName = FrmLogin.loginUserName;
                }
                else
                {
                    
                    
                    //m.CreateDate = DateTime.Now;
                    //m.CreateUserId = 1;
                    //m.CreateUserName = "超级管理员";
                    //m.CreateUserId = frmLoginNew.LoginUserID;
                    //m.CreateUserName = frmLoginNew.loginUserName;

                }
                m.RoleName = txt_RoleName.Text.ToStringExt();
                m.Remark = richTextBox1.Text;

                RoleRightList.Clear();
                CheckRoleRightTreeViewNode(tree_RoleRight.Nodes);
                m.RoleRightList = RoleRightList;
                //roles.ParentId = com_ParentId.SelectedValue.ToString();
                //roles.Roles_Name = txt_RoleName.Text;
                //roles.Roles_Remark = txt_RoleRemark.Text;
                //roles.SortCode = int.Parse(txt_SortCode.Text);

                if (String.IsNullOrEmpty(m.RoleName))
                {
                    MessageBox.Show($"角色不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
              

                int isOk = 0;
                if (type == OperationType.Add)
                {
                    if (bll.IsExistRoleName(m.RoleName))
                    {
                        MessageBox.Show($"角色：{m.RoleName}，已经存在，无法重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = $"{title}-角色：{m.RoleName}已经存在，无法重复添加！",
                            Type = "系统消息",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        return;
                    }
                    else
                    {
                        isOk = bll.Add(m);
                    }
                }
                else
                {
                    isOk = bll.Edit(m);
                }

                if (isOk > 0)
                {
                    MessageBox.Show("保存成功！");
                    System_Bll.AddLog(new SysLogModel
                    {
                        CreateUserID = GlobalUserHandle.LoginUserID,
                        CreateUserName = GlobalUserHandle.loginUserName,
                        LocalIP = GlobalUserHandle.LocalIP,
                        Module = title,
                        Method = MethodBase.GetCurrentMethod().Name,
                         LogMessage = $"{title}-角色ID：{m.RoleID}，角色名：{m.RoleName} 成功！",
                        Type = "系统消息",
                        ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                    });
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
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
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
        }

        List<RoleRightModel> RoleRightList = new List<RoleRightModel>();
        /// <summary>
        /// 遍历角色权限选中节点
        /// </summary>
        /// <param name="node"></param>
        public void CheckRoleRightTreeViewNode(TreeNodeCollection node)
        {

            foreach (TreeNode n in node)
            {
                if (n.Checked)
                {
                    RoleRightList.Add(new RoleRightModel
                    {
                        //RoleRightID = Guid.NewGuid().ToString(),
                        RoleID=ID.StrToInt(-1),
                        Menu_Id = n.Tag.StrToInt(-1),
                        CreateUserName = GlobalUserHandle.loginUserName,
                        CreateUserId = GlobalUserHandle.LoginUserID,
                        CreateDate = DateTime.Now
                    });
                }
                CheckRoleRightTreeViewNode(n.Nodes);
            }
        }
        #endregion

        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
