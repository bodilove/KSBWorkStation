using Common;
using MainControl.BLL;
using MainControl.Entity;
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
using WeifenLuo.WinFormsUI.Docking;
//using YX.BLL;
//using YX.Entity;
namespace MainControl.User
{
    public partial class FrmRoleInfo : WindowParent
    {
        RoleService bll = new RoleService();
        SysLogService System_Bll = new SysLogService();

        FrmRoleInfoEdit edit;
       
        string title = string.Empty;
        public FrmRoleInfo(int ParentId)
        {
            InitializeComponent();
            SetButton(ParentId, this.toolStrip1);//设置按钮权限
            InitView(this);//初始化标题放大缩小
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列自动填充
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//填充满

        }
   
        private void FrmRoleInfo_Load(object sender, EventArgs e)
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
            dr1["Name"] = "角色名称";
            dr1["Value"] = "RoleName";

            dt.Rows.Add(dr1);
           
            this.com_Searchwhere.ComboBox.DisplayMember = "Name";
            this.com_Searchwhere.ComboBox.ValueMember = "Value";

            this.com_Searchwhere.ComboBox.DataSource = dt;
        }
        private async void BindView()
        {
            try
            {
                this.dataGridView1.DataSource = null;
                // 假设有一个实体类
                var entity = new RoleModel();
                // 使用反射获取实体的属性
                PropertyInfo[] properties = entity.GetType().GetProperties();


                foreach (PropertyInfo property in properties)
                {
                    // 添加列，列名为中文字段名
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                    column.DataPropertyName = property.Name;
                    dataGridView1.Columns.Add(column);
                }

                List<RoleModel> list = await bll.QueryListAsync();

              
                this.dataGridView1.DataSource = list;
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

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                edit = new FrmRoleInfoEdit(this);
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
                        edit = new FrmRoleInfoEdit(this, ref dr);
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

        private void btn_delete_Click(object sender, EventArgs e)
        {

            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                if (dr != null)
                {
                    RoleModel m = dr.DataBoundItem as RoleModel;
                    int result = bll.DeleteIsLogic(m.RoleID.StrToInt(-1));
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
                            LogMessage = $"{title}-删除角色ID：{m.RoleID}，角色名：{m.RoleName}",
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

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ////if (treeView1.SelectedNode != null)
            ////{
            ////    string Organization_ID = treeView1.SelectedNode.Tag.ToString();
            ////    StringBuilder SqlWhere = new StringBuilder();
            ////    IList<SqlParameter> IList_param = new List<SqlParameter>();
            ////    if (!string.IsNullOrEmpty(Organization_ID))
            ////    {
            ////        SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            ////        IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID));
            ////    }
            ////    this.dataGridView1.DataSource = user_bll.GetUserInfoByOrganization_Id(SqlWhere, IList_param);
            ////}
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
