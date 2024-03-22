using Common;
using MainControl.BLL;
using MainControl.Entity;
using SqlSugar.Extensions;
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
//using YX.BLL;
//using YX.Entity;
namespace MainControl
{
    public partial class FrmSysMenuInfo : WindowParent
    {
        SysMenuService bll = new SysMenuService();

        SysLogService System_Bll = new SysLogService();
        string title = string.Empty;
        //public FrmUserInfo(string ParentId)

        FrmMenuEdit edit;//编辑窗口
        
        public FrmSysMenuInfo()
        {
            InitializeComponent();
            //SetButton(ParentId, this.toolStrip1);//设置按钮权限
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列自动填充
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//填充满

            title = this.Text;
        }
   
        private void FrmSysMenuInfo_Load(object sender, EventArgs e)
        {
            GetComboxList();
            BindTreeView();
            if (treeView1.Nodes.Count > 0)//展开一级节点
            {
                treeView1.Nodes[0].Expand();
                treeView1.Nodes[0].Checked=true;
            }

        }
        private void GetComboxList()
        {
            DataTable dt = new DataTable();
            DataColumn Name = new DataColumn("Name");
            DataColumn Value = new DataColumn("Value");
            dt.Columns.Add(Name);
            dt.Columns.Add(Value);

            DataRow dr1 = dt.NewRow();
            dr1["Name"] = "菜单名称";
            dr1["Value"] = "Menu_Name";
            dt.Rows.Add(dr1);
            
            this.com_Searchwhere.ComboBox.DisplayMember = "Name";
            this.com_Searchwhere.ComboBox.ValueMember = "Value";

            this.com_Searchwhere.ComboBox.DataSource = dt;
        }
        /// <summary>
        /// 绑定树形菜单
        /// </summary>
        private async void BindTreeView()
        {
            try
            {
                this.treeView1.Nodes.Clear();
                this.treeView1.ImageList = imageList1;
                var list = bll.QueryList();

                var parents = list.Where(o => o.ParentId == 0);
                foreach (var item in parents)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = item.Menu_Name;
                    tn.Tag = item.Menu_Id;
                    tn.ImageIndex = 0;
                    FillTree(tn, list);
                    treeView1.Nodes.Add(tn);
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
        /// 绑定节点菜单数据
        /// </summary>
        /// <param name="MenuID"></param>
        private async void BindView(int MenuID)
        {
            try
            {
                this.dataGridView1.DataSource = null;
                // 假设有一个实体类
                var entity = new SysMenuModel();
                // 使用反射获取实体的属性
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    // 添加列，列名为中文字段名
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                    column.DataPropertyName = property.Name;
                    dataGridView1.Columns.Add(column);
                    if (property.Name == "CreateUserId" || property.Name == "ModifyUserId" )
                    {
                        column.Visible = false;
                    }
                }
                List<SysMenuModel> list = await bll.QueryListAsync(MenuID);

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

        private void FillTree(TreeNode node, List<SysMenuModel> list)
        {

            var childs = list.Where(o => o.ParentId == node.Tag.ObjToInt());
            if (childs.Count() > 0)
            {
                foreach (var item in childs)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = item.Menu_Name.ToString();
                    tnn.Tag = item.Menu_Id;
                    tnn.ImageIndex = 0;
                    if (item.ParentId == node.Tag.ObjToInt())
                    {
                        FillTree(tnn, list);
                    }
                    node.Nodes.Add(tnn);
                }

            }
        }

        /// <summary>
        /// 选择节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int MeunID = e.Node.Tag.StrToInt(0);
            BindView(MeunID);

        }
        /// <summary>
        /// dataGridView1 样式
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
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
            int MeunID = treeView1.SelectedNode.Tag.StrToInt(0);
            BindView(MeunID);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                edit = new FrmMenuEdit(this);
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
        /// 修改
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
                        edit = new FrmMenuEdit(this, ref dr);
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
        /// 删除
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
                    SysMenuModel m = dr.DataBoundItem as SysMenuModel;
                    int result = bll.DeleteIsLogic(m.Menu_Id.StrToInt(-1));
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
                            LogMessage = $"{title}-菜单：{m.Menu_Name}删除成功！",
                            Type = "系统消息",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        int MeunID = treeView1.SelectedNode.Tag.StrToInt(0);
                        BindView(MeunID);
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
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int MenuID = treeView1.SelectedNode.Tag.StrToInt(-1);
                BindView(MenuID);
            }
           
        }
        /// <summary>
        /// 树形刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindTreeView();
            if (treeView1.Nodes.Count > 0)//展开一级节点
            {
                treeView1.Nodes[0].Expand();
            }
        }
    }
}
