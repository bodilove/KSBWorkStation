using Common.SysConfig.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//using YX.BLL;
//using YX.Entity;
using Test.StartUp;
using Common;
using System.Xml;
using System.IO;
using Test.ProjectFileEditor;
using Test;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using static Test.ProjectFileEditor.frmMain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Test.ProjectTest;
using MainControl.Entity;
using System.Reflection;
using MainControl.BLL;
using System.Diagnostics;

namespace MainControl
{
    public partial class FrmToolHmiInfo : WindowParent
    {
       ProcessStepsService bll=new ProcessStepsService();

        //string filePath = Application.StartupPath + @"\TestSequence.xml";
        //MyTest m = null;

        SysLogService System_Bll = new SysLogService();
        string title = string.Empty;

        FrmToolHmiEdit edit;//编辑窗口
        public FrmToolHmiInfo(int ParentId)
        {
            InitializeComponent();
            SetButton(ParentId, this.toolStrip1);//设置按钮权限
            InitView(this);//初始化标题放大缩小
            ////加载xml数据文件
            //XmlDocument xmlDoc = LoadXmlDoc(filePath);
            //m = XmlHelper<MyTest>.DeserializeToObject(xmlDoc.InnerXml);

            title = this.Text;
        }
   
        private void FrmToolHmiInfo_Load(object sender, EventArgs e)
        {
            GetComboxList();

            //查询工位
            BindTreeView();
            if (treeView1.Nodes.Count > 0)//展开一级节点
            {
                treeView1.Nodes[0].Expand();
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
            dr1["Name"] = "测试步骤";
            dr1["Value"] = "Name";

            //DataRow dr2 = dt.NewRow();
            //dr2["Name"] = "文件路径";
            //dr2["Value"] = "FilePath";

            //DataRow dr3 = dt.NewRow();
            //dr3["Name"] = "时间";
            //dr3["Value"] = "CreateTime";
            dt.Rows.Add(dr1);
            //dt.Rows.Add(dr2);
            //dt.Rows.Add(dr3);
            this.com_Searchwhere.ComboBox.DisplayMember = "Name";
            this.com_Searchwhere.ComboBox.ValueMember = "Value";

            this.com_Searchwhere.ComboBox.DataSource = dt;
        }

        /// <summary>
        /// 查询工位
        /// </summary>
        /// <param name="list"></param>
        private void BindTreeView()
        {
            try
            {
                int SelectNodeIndex = 0;
                if (treeView1.SelectedNode != null)
                {
                    SelectNodeIndex = treeView1.SelectedNode.Index;//选中节点下标
                }

                treeView1.Nodes.Clear();
                List<ProcessSteps> list = bll.QueryList();
                if (Program.CurrentConfig != null)
                {
                    var parents = Program.CurrentConfig.localStlst;

                    //var parents = list.Where(o => o.ParentId == "0");
                    foreach (var item in parents)
                    {
                        TreeNode tn = new TreeNode();
                        tn.Text = $"[{item.StationNum}]{item.StationName}";
                        tn.Tag = "0";
                        tn.Name = item.StationNum;
                        tn.ImageIndex = 0;
                        FillTree(tn, list);
                        treeView1.Nodes.Add(tn);
                    }

                    if (treeView1.Nodes.Count > 0)//展开一级节点
                    {
                        treeView1.Nodes[SelectNodeIndex].Expand();
                        treeView1.Nodes[SelectNodeIndex].Checked = true; ;
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

        private void FillTree(TreeNode node, List<ProcessSteps> list)
        {

            var childs = list.Where(o =>o.StationNum==node.Name.ToString()&& o.ParentId == node.Tag.StrToInt(-1));
            if (childs.Count() > 0)
            {
                foreach (var item in childs)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Text = item.ProcessName;
                    tnn.Tag = item.ProcessID;
                    tnn.Name = item.StationNum.ToString();
                    tnn.ImageIndex = 0;
                    if (item.ParentId == node.Tag.StrToInt(-1))
                    {
                        FillTree(tnn, list);
                    }
                    node.Nodes.Add(tnn);
                }

            }
        }

        /// <summary>
        /// 绑定节点菜单数据
        /// </summary>
        /// <param name="MenuID"></param>
        private async void BindView()
        {
            try
            {
                int ProcessID = treeView1.SelectedNode.Tag.StrToInt(-1);
                string StationNum = treeView1.SelectedNode.Name.ToString();
                this.dataGridView1.DataSource = null;
                // 假设有一个实体类
                var entity = new ProcessSteps();
                // 使用反射获取实体的属性
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    // 添加列，列名为中文字段名
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                    column.DataPropertyName = property.Name;
                    dataGridView1.Columns.Add(column);
                    if (
                        property.Name == "ParentId" ||
                        property.Name == "CreateUserId" ||
                        property.Name == "ModifyUserId" ||
                        property.Name == "DeleteMark" ||
                        property.Name == "ProcessType")
                    {
                        column.Visible = false;
                    }
                    if (ProcessID <= 0)
                    {
                        if (

                             property.Name == "Conditional" ||
                           property.Name == "IsKeyCode" ||
                            property.Name == "KeyCodeFormat" ||
                           property.Name == "Ulimit" ||
                           property.Name == "Llimit" ||
                           property.Name == "Unit")
                        {
                            column.Visible = false;
                        }

                    }
                }
               
                List<ProcessSteps> list = await bll.QueryListAsync(StationNum,ProcessID);

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


        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //int ProcessID = e.Node.Tag.StrToInt(-1);
            //string StationNum=e.Node.Name.ToString();
            //e.Node.BackColor = Color.Blue;
            //var list = await bll.QueryListAsync(StationNum, ProcessID);

            //if (list != null && list.Count>0)
            //{
            //    this.dataGridView1.DataSource =list;
            //}
            BindView();

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
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            } 
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            //  StringBuilder SqlWhere = new StringBuilder();
            //  IList<SqlParameter> IList_param = new List<SqlParameter>();

            //string Organization_ID=string.Empty;
            //if (!string.IsNullOrEmpty(txt_Search.Text))
            //{
            //    //SqlWhere.Append(" and U." + com_Searchwhere.ComboBox.SelectedValue.ToString() + " like @obj ");
            //    //IList_param.Add(new SqlParameter("@obj", '%' + txt_Search.Text.Trim() + '%'));
            //}
            //if (!string.IsNullOrEmpty(treeView1.SelectedNode.Tag.ToString()))
            //{
            //    Organization_ID = treeView1.SelectedNode.Tag.ToString();
            //    //SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            //    //IList_param.Add(new SqlParameter("@Organization_ID", treeView1.SelectedNode.Tag.ToString()));
            //}
            BindView();
        }

        /// <summary>
        /// 添加测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView1.SelectedNode == null)
                {
                   
                    MessageBox.Show("请选择节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                edit = new FrmToolHmiEdit(this);
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
        /// 编辑项目
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
                        edit = new FrmToolHmiEdit(this, ref dr);
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
                if (dataGridView1.DataSource == null)
                {
                    MessageBox.Show("没有选择任何数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                if (dr != null)
                {
                    ProcessSteps m = dr.DataBoundItem as ProcessSteps;
                    int result = bll.DeleteIsLogic(m.ProcessID.StrToInt(-1));
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
                            LogMessage = $"{title}-工位：{m.StationNum}，工艺：{m.ProcessName}删除成功！",
                            Type = "系统消息",
                            ClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName
                        });
                        //int MeunID = treeView1.SelectedNode.Tag.StrToInt(0);
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
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode != null)
            //{
            //    int ProcessID = treeView1.SelectedNode.Tag.StrToInt(-1);
            //    string StationNum = treeView1.SelectedNode.Name.ToString();
            //    //StringBuilder SqlWhere = new StringBuilder();
            //    //IList<SqlParameter> IList_param = new List<SqlParameter>();
            //    //if (!string.IsNullOrEmpty(Organization_ID))
            //    //{
            //    //    SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            //    //    IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID));
            //    //}
            //    if (!string.IsNullOrEmpty(StationNum))
            //    {
            //        //BindTreeView();


            //    }
            //    else
            //    {

            //    }


            //}
            BindView();


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
