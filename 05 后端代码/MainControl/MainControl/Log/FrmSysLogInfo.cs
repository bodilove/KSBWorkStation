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
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//using YX.BLL;
//using YX.Entity;
namespace MainControl.Log
{
    public partial class FrmSysLogInfo : WindowParent
    {
        SysLogService bll = new SysLogService();

        //public FrmUserInfo(string ParentId)
        public FrmSysLogInfo(int ParentId)
        {
            InitializeComponent();
            SetButton(ParentId, this.toolStrip1);//设置按钮权限
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列自动填充
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//填充满

        }
   
        private void FrmSysLogInfo_Load(object sender, EventArgs e)
        {
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
            dr1["Name"] = "类型";
            dr1["Value"] = "Type";

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
                var entity = new SysLogModel();
                // 使用反射获取实体的属性
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    // 添加列，列名为中文字段名
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.HeaderText = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                    column.DataPropertyName = property.Name;
                    dataGridView1.Columns.Add(column);
                    if (property.Name == "LogID" )
                    {
                        column.Visible = false;
                    }
                }
                List<SysLogModel> list = await bll.QueryListAsync();

                this.dataGridView1.DataSource = list;
            }
            catch (Exception ex)
            {
                bll.AddLog(new SysLogModel
                {
                    //CreateUserID = FrmLogin.LoginUserID,
                    //CreateUserName = FrmLogin.loginUserName,
                    //LocalIP = FrmLogin.LocalIP,
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = typeof(FrmSysLogInfo).ToString()
                });
                MessageBox.Show(ex.Message);
            }
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

        /// <summary>
        /// 查询日志
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
        /// 清空日志数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                // 弹出确认对话框
                DialogResult dialogResult = MessageBox.Show("确定要删除所选数据吗？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // 检查用户的选择
                if (dialogResult == DialogResult.Yes)
                {
                    if (this.dataGridView1.RowCount > 0)
                    {
                        bool result = bll.DeleteAll();
                        if (result)
                        {
                            MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BindView();
                        }
                        else
                        {
                            MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    // 用户选择了“否”或关闭了对话框，取消删除操作
                    //MessageBox.Show("取消操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                bll.AddLog(new SysLogModel
                {
                    //CreateUserID = FrmLogin.LoginUserID,
                    //CreateUserName = FrmLogin.loginUserName,
                    //LocalIP = FrmLogin.LocalIP,
                    LogMessage = ex.Message,
                    Type = "系统错误！",
                    ClassName = typeof(FrmUserInfo).ToString()
                });
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 刷新日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            BindView();

        }

    }
}
