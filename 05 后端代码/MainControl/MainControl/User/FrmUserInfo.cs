using Common;
using Common.User.Model;
using MainControl.BLL;
using MainControl.Entity;
using MainControl.User;
using ORMSqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;
namespace MainControl
{
    public partial class FrmUserInfo : WindowParent
    {

        UserService bll=new UserService();
        SysLogService System_Bll = new SysLogService();
        //SystemOrganization_Bll organization_bll = new SystemOrganization_Bll();
        //// SystemUserInfo_Dal user_dal = new SystemUserInfo_Dal();
        //SystemUserInfo_Bll user_bll = new SystemUserInfo_Bll();
        //SystemMenu_Bll menu_bll = new SystemMenu_Bll();

        //public FrmUserInfo(string ParentId)
        
        FrmUserInfoEdit edit;//编辑窗口

       
        string title =string.Empty;
        public FrmUserInfo(int ParentId)
        {
            InitializeComponent();
            SetButton(ParentId, this.toolStrip1);//设置按钮权限
            InitView(this);//初始化标题放大缩小
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列自动填充
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//填充满
           
        }
   
        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            title = this.Text;
            GetComboxList();
            BindView();
        }
        private void GetComboxList()
        {
            DataTable dt = new DataTable();
            DataColumn Name = new DataColumn("Name");
            DataColumn Value = new DataColumn("Value");
            dt.Columns.Add(Name);
            dt.Columns.Add(Value);

            DataRow dr1 = dt.NewRow();
            dr1["Name"] = "账户";
            dr1["Value"] = "UserNum";

            DataRow dr2 = dt.NewRow();
            dr2["Name"] = "用户名";
            dr2["Value"] = "UserName";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            this.com_Searchwhere.ComboBox.DisplayMember = "Name";
            this.com_Searchwhere.ComboBox.ValueMember = "Value";

            this.com_Searchwhere.ComboBox.DataSource = dt;
        }
        private  async void BindView()
        {
            try
            {
                this.dataGridView1.DataSource =null;
                // 假设有一个实体类
                var entity = new UserModel();
                // 使用反射获取实体的属性
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    // 添加列，列名为中文字段名
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                    column.DataPropertyName = property.Name;
                    dataGridView1.Columns.Add(column);
                    if (property.Name == "CreateUserId" || property.Name == "ModifyUserId"|| property.Name == "Password"
                        || property.Name == "RoleID")
                    {
                        column.Visible = false;
                    }
                }
                List<UserModel> list = await bll.QueryListAsync();
               
                this.dataGridView1.DataSource = list;
            }
            catch (Exception ex)
            {
                System_Bll.AddLog(new SysLogModel
                {
                    CreateUserID = GlobalUserHandle.LoginUserID,
                    CreateUserName = GlobalUserHandle.loginUserName,
                    LocalIP = GlobalUserHandle.LocalIP,
                    Module= title,
                    Method= MethodBase.GetCurrentMethod().Name,
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
        }

        //private void FillTree(TreeNode node, List<Base_Organization> list)
        //{

        //    var childs = list.Where(o => o.ParentId == node.Tag.ToString());
        //    if (childs.Count() > 0)
        //    {
        //        foreach (var item in childs)
        //        {
        //            TreeNode tnn = new TreeNode();
        //            tnn.Text = item.Organization_Name;
        //            tnn.Tag = item.Organization_ID;
        //            tnn.ImageIndex = 0;
        //            if (item.ParentId == node.Tag.ToString())
        //            {
        //                FillTree(tnn, list);
        //            }
        //            node.Nodes.Add(tnn);
        //        }

        //    }
        //}

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string Organization_ID = e.Node.Tag.ToString();
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParameter> IList_param = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Organization_ID))
            {
                SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
                IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID ));
            }
            //this.dataGridView1.DataSource = user_bll.GetUserInfoByOrganization_Id(SqlWhere, IList_param);
     
        }
        /// <summary>
        /// data gridview 数据绑定完成执行（颜色）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dataGridView1.Rows.Count != 0)
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; )
                {
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.BlanchedAlmond;
                    i += 2;
                }
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
          //  StringBuilder SqlWhere = new StringBuilder();
          //  IList<SqlParameter> IList_param = new List<SqlParameter>();
          //  if (!string.IsNullOrEmpty(txt_Search.Text))
          //  {
          //      SqlWhere.Append(" and U." + com_Searchwhere.ComboBox.SelectedValue.ToString()+ " like @obj ");
          //      IList_param.Add(new SqlParameter("@obj", '%' + txt_Search.Text.Trim() + '%'));
          //  }
          //  if (!string.IsNullOrEmpty(treeView1.SelectedNode.Tag.ToString()))
          //  {
          //      SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
          //      IList_param.Add(new SqlParameter("@Organization_ID", treeView1.SelectedNode.Tag.ToString()));
          //  }
          //this.dataGridView1.DataSource= user_bll.GetUserInfoByOrganization_Id(SqlWhere, IList_param);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                edit = new FrmUserInfoEdit(this);
                // 订阅子窗体的事件
                edit.DataUpdated += btn_refresh_Click;
                edit.ShowDialog();
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

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource == null)
                {
                    MessageBox.Show("请选择要编辑的行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];

                    if (dr != null)
                    {
                        edit = new FrmUserInfoEdit(this, ref dr);
                        // 订阅子窗体的事件
                        edit.DataUpdated += btn_refresh_Click;
                        edit.ShowDialog();
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
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {

            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                if (dr != null)
                {
                    UserModel m = dr.DataBoundItem as UserModel;
                    if (m != null&&m.UserNum.ToUpper()== "SuperAdmin")
                    {
                        MessageBox.Show($"{m.UserNum}为超级账号，不可以删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int result = bll.DeleteIsLogic(m.UserID.StrToInt(-1));
                    if (result == 1)
                    {
                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage =$"{title}-删除账号：{m.UserNum}，用户名：{m.UserName}" ,
                            Type = "系统消息",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        BindView();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                });
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新主窗口，（添加、修改也可以订阅）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode != null)
            //{
            //    string Organization_ID = treeView1.SelectedNode.Tag.ToString();
            //    StringBuilder SqlWhere = new StringBuilder();
            //    IList<SqlParameter> IList_param = new List<SqlParameter>();
            //    if (!string.IsNullOrEmpty(Organization_ID))
            //    {
            //        SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            //        IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID));
            //    }
            //    this.dataGridView1.DataSource = user_bll.GetUserInfoByOrganization_Id(SqlWhere, IList_param);
            //}
            BindView();

        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BindTreeView(organization_bll.GetOrganizations());
            //if (treeView1.Nodes.Count > 0)//展开一级节点
            //{
            //    treeView1.Nodes[0].Expand();
            //}
        }
    }
}
