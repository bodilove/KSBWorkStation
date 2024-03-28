using Common;
using MainControl.BLL;
using MainControl.Entity;
using SqlSugar;
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
    public partial class FrmToolHmiEdit : Form
    {
        ProcessStepsService bll = new ProcessStepsService();
        SysLogService System_Bll = new SysLogService();

        FrmToolHmiInfo _frm;
        OperationType type;
        int ProcessID = -1;
        int ProcessType = 0;

        //int ParentId = -1;//父ID

        ProcessSteps m;
        
        string title = string.Empty;

        List<EnumEntity> lst;

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

        /// <summary>
        /// 添加构造
        /// </summary>
        /// <param name="frm"></param>
        public FrmToolHmiEdit(FrmToolHmiInfo frm)
        {
         
            type = OperationType.Add;
            this._frm = frm;
            InitializeComponent();
            Init();
            m = new ProcessSteps() { SortCode=100};
            this.Text = "工艺信息-添加";
            if (_frm.treeView1.SelectedNode != null)
            {
                ProcessID = _frm.treeView1.SelectedNode.Tag.StrToInt(-1);
                this.txt_parent.Text = _frm.treeView1.SelectedNode.Text;
                this.txt_StationNum.Text = _frm.treeView1.SelectedNode.Name;
                this.txt_Sort.Text = m.SortCode.ToStringExt("100");
               
                if (ProcessID <= 0)
                {
                    this.com_Conditional.DataSource=null;

                    chk_IsKeyCode.Enabled = false;
                    this.com_Conditional.Enabled = false;
                    this.txt_Ulimit.Enabled = false;
                    this.txt_Llimit.Enabled = false;
                    this.txt_Unit.Enabled = false;
                    ProcessType = 0;
                   
                }
                else
                {
                    //com_Conditional.SelectedValue = -1;

                    chk_IsKeyCode.Enabled = true;
                    this.com_Conditional.Enabled = true;  
                    this.txt_Ulimit.Enabled = true;
                    this.txt_Llimit.Enabled = true;
                    this.txt_Unit.Enabled = true;
                    ProcessType = 1;
                }


            }
            else {
                MessageBox.Show("请选择节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            title = this.Text;
        }
        /// <summary>
        /// 编辑构造
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="dvr"></param>
        public FrmToolHmiEdit(FrmToolHmiInfo frm, ref DataGridViewRow dvr)
        {
            try
            {
                type = OperationType.Edit;
                this._frm = frm;
                InitializeComponent();
                Init();

                m = dvr.DataBoundItem as ProcessSteps;
                ProcessID = _frm.treeView1.SelectedNode.Tag.StrToInt(-1);
                this.txt_parent.Text = _frm.treeView1.SelectedNode.Text;
                this.txt_StationNum.Text = _frm.treeView1.SelectedNode.Name;
                chk_IsKeyCode.Checked = m.IsKeyCode==0?false :true;
                if (ProcessID <= 0)
                {
                    this.com_Conditional.DataSource = null;
                    com_Conditional.Enabled=false;
                   
                    chk_IsKeyCode .Enabled = false;
                    this.txt_KeyCodeFormat.Enabled = false;

                    this.txt_Ulimit.Enabled = false;
                    this.txt_Llimit.Enabled = false;
                    this.txt_Unit.Enabled = false;
                }
                else
                {
                    if (chk_IsKeyCode.Checked)
                    {
                        chk_IsKeyCode.Enabled = true;
                        this.txt_KeyCodeFormat.Enabled = true;

                        this.txt_Ulimit.Enabled = false;
                        this.txt_Llimit.Enabled = false;
                        this.txt_Unit.Enabled = false;
                    }
                    else
                    {
                        chk_IsKeyCode.Enabled = false;
                        this.txt_KeyCodeFormat.Enabled = false;

                        this.txt_Ulimit.Enabled = true;
                        this.txt_Llimit.Enabled = true;
                        this.txt_Unit.Enabled = true;

                    }

                    com_Conditional.Enabled = true;
                   // EnumConditional enumcon=(EnumConditional)Enum.Parse(typeof(EnumConditional), m.Conditional.ToStringExt("Between"));
                    var item =lst.Where(p => p.Name == m.Conditional.ToStringExt("Between")).FirstOrDefault();
                    com_Conditional.SelectedValue = item.Value; //;Enum.Parse(typeof(EnumConditional), m.Conditional.ToStringExt("Between"));
                }
                


                //  this.com_Parent.Text=dvr.Cells["节点位置"].Value==null?"": dvr.Cells["节点位置"].Value.ToString();
                this.txt_StationNum.Text = m.StationNum;
                this.txt_parent.Text = _frm.treeView1.SelectedNode.Text;
                this.txt_ProcessName.Text = m.ProcessName.ToStringExt();
                this.txt_Ulimit.Text = m.Ulimit.ToStringExt();
                this.txt_Llimit.Text = m.Llimit.ToStringExt();
                this.txt_Unit.Text = m.Unit.ToStringExt();
                txt_Sort.Text = m.SortCode.ToStringExt("100");
              
                richTextBox1.Text = m.Remark;
                ProcessID = m.ProcessID.StrToInt(-1);
                //ParentId = _frm.treeView1.SelectedNode.Tag.StrToInt(-1);

                this.Text = "工艺信息-编辑";
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

        void Init()
        {
            lst = EnumUtil.EnumToList(typeof(EnumConditional));
            com_Conditional.DataSource =  lst;
            com_Conditional.DisplayMember = "Name";
            com_Conditional.ValueMember = "Value";
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMenuEdit_Load(object sender, EventArgs e)
        {

            //var ParentID= _frmMenu.treeView1.SelectedNode==null?"": _frmMenu.treeView1.SelectedNode.Tag;
            //com_Parent.DataSource = bll.GetSysMenusParent(FrmLogin.LoginUserID);
            //com_Parent.DisplayMember = "Menu_Name";
            //com_Parent.ValueMember = "Menu_Id";
            //com_Parent.SelectedValue = ParentID;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                int isOk = 0;
                if (type == OperationType.Add)
                {

                    m.CreateDate = DateTime.Now;
                    m.CreateUserId = GlobalUserHandle.LoginUserID;
                    m.CreateUserName = GlobalUserHandle.loginUserName;
                    m.StationNum = _frm.treeView1.SelectedNode.Name;
                    m.ProcessName = txt_ProcessName.Text;
                    m.ParentId = _frm.treeView1.SelectedNode.Tag.StrToInt(-1);
                    m.SortCode = int.Parse(txt_Sort.Text);
                    m.Remark = richTextBox1.Text.ToString();
                   

                    if (ProcessType >= 1)
                    {
                        m.Conditional = this.com_Conditional.Text.ToStringExt();
                        if (chk_IsKeyCode.Checked)
                        {
                            m.IsKeyCode = 0;
                            m.Ulimit = this.txt_Ulimit.Text.ToStringExt();
                            m.Llimit = this.txt_Llimit.Text.ToStringExt();
                            m.Unit = this.txt_Unit.Text.ToStringExt();
                        }
                        else
                        {
                            m.IsKeyCode = 1;
                            m.KeyCodeFormat = this.txt_KeyCodeFormat.Text.ToStringExt();
                        }
                    }

                    if (bll.IsExistMenuName(m.ProcessName,m.ParentId))
                    {
                        MessageBox.Show($"工位：{m.StationNum}，工艺：{m.ProcessName}，已经存在，无法重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System_Bll.AddLog(new SysLogModel
                        {
                            CreateUserID = GlobalUserHandle.LoginUserID,
                            CreateUserName = GlobalUserHandle.loginUserName,
                            LocalIP = GlobalUserHandle.LocalIP,
                            Module = title,
                            Method = MethodBase.GetCurrentMethod().Name,
                            LogMessage = $"{title}-工位：{m.StationNum}，工艺：{m.ProcessName} 已经存在，无法重复添加！",
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
                            LogMessage = $"{title}-工位：{m.StationNum}，工艺：{m.ProcessName} 添加成功！",
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


                    m.StationNum = _frm.treeView1.SelectedNode.Name;
                    m.ProcessName = txt_ProcessName.Text;
                    m.ParentId = _frm.treeView1.SelectedNode.Tag.StrToInt(-1);
                   

                    if (m.ProcessType >= 1)
                    {
                        m.Conditional = this.com_Conditional.Text.ToStringExt();
                        if (chk_IsKeyCode.Checked)
                        {
                            m.IsKeyCode = 1;
                            m.Ulimit = this.txt_Ulimit.Text.ToStringExt();
                            m.Llimit = this.txt_Llimit.Text.ToStringExt();
                            m.Unit = this.txt_Unit.Text.ToStringExt();

                        }
                        else
                        {
                            m.IsKeyCode = 0;
                            m.KeyCodeFormat = this.txt_KeyCodeFormat.Text.ToStringExt();
                        }
                    }
                   
                    
                    m.SortCode = int.Parse(txt_Sort.Text);
                    m.Remark = richTextBox1.Text.ToString();
                    
                    m.ModifyUserName = GlobalUserHandle.loginUserName;
                    m.ModifyDate = DateTime.Now;
                    m.ModifyUserId = GlobalUserHandle.LoginUserID;
                   
                    
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
                            LogMessage = $"{title}-工位：{m.StationNum}，工艺：{m.ProcessName} 修改成功！",
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

       /// <summary>
       /// 取消按钮
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region  输入框限制 数字，小数

        private void txt_Ulimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            // 允许输入数字（0-9）、退格键和小数点
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                // 如果输入的不是数字或者小数点，则取消输入
                e.Handled = true;
            }

            // 只允许输入一个小数点
            if (txt.Text.Contains(".") && e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }

        private void txt_Llimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            // 允许输入数字（0-9）、退格键和小数点
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                // 如果输入的不是数字或者小数点，则取消输入
                e.Handled = true;
            }

            // 只允许输入一个小数点
            if (txt.Text.Contains(".") && e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }

        private void txt_Sort_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            // 允许输入数字（0-9）、退格键和小数点
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // 如果输入的不是数字或者小数点，则取消输入
                e.Handled = true;
            }

        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_IsKeyCode.Checked)
            {
                txt_KeyCodeFormat.Enabled= true;
                this.txt_Ulimit.Enabled = false;
                this.txt_Llimit.Enabled = false;
                this.txt_Unit.Enabled = false;

                this.txt_Ulimit.Text = null;
                this.txt_Llimit.Text = null;
                this.txt_Unit.Text = null;
            }
            else
            {
                txt_KeyCodeFormat.Enabled = false;
                this.txt_Ulimit.Enabled = true;
                this.txt_Llimit.Enabled = true;
                this.txt_Unit.Enabled = true;

                this.txt_KeyCodeFormat.Text = null;
               

            }

        }
        
    }
}
