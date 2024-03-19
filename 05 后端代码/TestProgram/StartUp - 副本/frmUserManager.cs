using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StartUp.Enity;
using Test.Common;

namespace StartUp
{
    public partial class frmUserManager : Form
    {
        Control pEditControl = null;
        string m_strOldName = "";
        List<UsersEnity> usersEnityList = new List<UsersEnity>();
        UsersEnity usersEnity = new UsersEnity();
        List<GroupsEnity> GroupsList = new List<GroupsEnity>();
        GroupsEnity GroupEntity = new GroupsEnity();


        public frmUserManager()
        {
            InitializeComponent();
        }

        // public UserManager m_xmlusers;
        //public UserManager.UserManage m_xmlusersOrig;

        private void UserSettings_Load(object sender, EventArgs e)
        {
            //refresh data
            tabControl1_SelectedIndexChanged(sender, e);

            Application.Idle += new EventHandler(Application_Idle);
        }


        private void Application_Idle(object sender, EventArgs e)
        {

            //types
            if (Types.SelectedNode == null)
            {
                this.Deltype.Enabled = false;
                this.RenameType.Enabled = false;
            }
            else
            {
                this.Deltype.Enabled = (Types.SelectedNode.Level == 1);
                this.RenameType.Enabled = (Types.SelectedNode.Level == 1);
            }

            //users
            if (userGrid.SelectedCells.Count <= 0)
            {
                this.deluser.Enabled = false;
                this.SetType.Enabled = false;
            }
            else
            {
                this.deluser.Enabled = true;
                this.SetType.Enabled = true;
            }



        }
        private void UserSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.m_xmlusers.Save();
            //this.Owner.Show();
        }

        private void InsertUsers(string type)
        {
            int i = 0;
            this.userGrid.Columns.Clear();
            this.userGrid.Rows.Clear();
            this.userGrid.Columns.Add("No", "编号");
            this.userGrid.Columns.Add("Name", "姓名");
            this.userGrid.Columns["No"].ReadOnly = true;
            this.userGrid.Columns["No"].DefaultCellStyle.BackColor = Color.LightGray;
            this.userGrid.Columns.Add("Num", "工号");
            usersEnityList = SelectUser("");
            if (type == string.Empty)
            {
                //find column
                DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
                col.Name = "Type";
                col.HeaderText = "组";
                ////col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.userGrid.Columns.Add(col);
                this.userGrid.Columns.Add("Description", "描述");

                //fill all users in user grid
                foreach (UsersEnity s in usersEnityList)
                {
                    i = this.userGrid.Rows.Add();
                    this.userGrid.Rows[i].Cells["No"].Value = (i + 1).ToString();
                    this.userGrid.Rows[i].Cells["Name"].Value = s.Name; //user

                    foreach (GroupsEnity t in GroupsList)
                    {
                        if (t.Type!=s.Type)
                        {
                            ((DataGridViewComboBoxCell)(this.userGrid.Rows[i].Cells["Type"])).
                   Items.AddRange(t.Type); //types
                        }
                   
                    }

                    ((DataGridViewComboBoxCell)(this.userGrid.Rows[i].Cells["Type"])).
                        Items.AddRange(s.Type); //types
                    this.userGrid.Rows[i].Cells["Type"].Value = s.Type;
                    this.userGrid.Rows[i].Cells["Description"].Value = s.Description;
                    this.userGrid.Rows[i].Cells["Num"].Value = s.Number;
                }
            }
            else
            {
                this.userGrid.Columns.Add("Description", "描述");
                foreach (UsersEnity s in usersEnityList)
                {
                    if (s.Type == type)
                    {
                        i = this.userGrid.Rows.Add();
                        this.userGrid.Rows[i].Cells["No"].Value = (i + 1).ToString();
                        this.userGrid.Rows[i].Cells["Name"].Value = s.Name; //user
                        this.userGrid.Rows[i].Cells["Description"].Value = s.Description;
                        this.userGrid.Rows[i].Cells["Num"].Value = s.Number;
                    }
                }
            }


            this.userGrid.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;



        }
        private void Types_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //更改选定内容后发生
            this.SetType.Visible = (e.Node.Level == 1);
            this.Rights.Visible = (e.Node.Level == 1);


            if (e.Node.Level != 1)
            {

                InsertUsers(string.Empty);
                this.userGrid.Columns["No"].ReadOnly = true;
                this.userGrid.Columns["No"].DefaultCellStyle.BackColor = Color.LightGray;
                return;
            }
            else
            {
                //更新权限

                GroupsList = SelectGroups(" and  Type='" + e.Node.Text + "'");
                if (GroupsList.Count < 1)
                {
                    MessageBox.Show("未查询到权限表");
                    return;
                }


                this.Rights.Items[0].Checked = GroupsList[0].Query_Bool;
                this.Rights.Items[1].Checked = GroupsList[0].Build_Bool;
                this.Rights.Items[2].Checked = GroupsList[0].Manage_Bool;
                this.Rights.Items[3].Checked = GroupsList[0].Test_Bool;
                this.Rights.Items[4].Checked = GroupsList[0].SetLabel_Bool;

                //Insert users
                this.InsertUsers(e.Node.Text);

                //Insert set to other type menu
                SetType.DropDownItems.Clear();
                ToolStripItem item = null;
                GroupsList = SelectGroups("");
                foreach (GroupsEnity t in GroupsList)
                {
                    if (t.Type == e.Node.Text)
                    {
                        continue;
                    }
                    item = SetType.DropDownItems.Add(t.Type);
                }
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {


            GroupsList = SelectGroups("");

            //添加到窗体上

            if (GroupsList == null) return;
            try
            {
                //load all typs(group2)
                this.Types.Nodes[0].Nodes.Clear();
                foreach (GroupsEnity s in GroupsList)
                {
                    this.Types.Nodes[0].Nodes.Add(s.Type);
                }
                this.Types.Nodes[0].ExpandAll();

                if (this.Types.Nodes[0].Nodes.Count > 0)
                {
                    this.Types.SelectedNode = this.Types.Nodes[0].Nodes[0];
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string strName = "NewType";
            //查看是否存在
            if (SelectGroups(string.Format(" type ='0'", strName)).Count > 0)
            {
                MessageBox.Show(this, "组创建失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                GroupsIntEnity gr = new GroupsIntEnity();
                gr.Type = strName;
                gr.Test = 0;
                gr.SetLabel = 0;
                gr.Manage = 0;
                gr.Build = 0;
                gr.Query = 0;
                AddGroupsr("", gr);
                this.Types.Nodes[0].Nodes.Add(strName);
            }

        }

        private void Rights_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //某项的选中状态将要更改。直到事件发生后，该值才会更新。
            GroupsList = SelectGroups(string.Format(" and [Type]='{0}'", Types.SelectedNode.Text));






            if (GroupsList == null || GroupsList.Count < 1)
            {
                return;
            }
            switch (e.Index)
            {
                case 0: //查询
                    GroupsList[0].Query = Convert.ToInt32(e.NewValue == CheckState.Checked);

                    break;
                case 1: //构建测试
                    GroupsList[0].Build = Convert.ToInt32(e.NewValue == CheckState.Checked);
                    break;

                case 2: //用户管理
                    GroupsList[0].Manage = Convert.ToInt32(e.NewValue == CheckState.Checked);
                    break;
                case 3: //测试
                    GroupsList[0].Test = Convert.ToInt32(e.NewValue == CheckState.Checked);
                    break;
                case 4: //标签
                    GroupsList[0].SetLabel = Convert.ToInt32(e.NewValue == CheckState.Checked);
                    break;

            }
            GroupsIntEnity GroupsInt = new GroupsIntEnity();
            GroupsInt.Type = GroupsList[0].Type;
            GroupsInt.Query = GroupsList[0].Query;
            GroupsInt.Build = GroupsList[0].Build;
            GroupsInt.Test = GroupsList[0].Test;
            GroupsInt.Manage = GroupsList[0].Manage;
            GroupsInt.SetLabel = GroupsList[0].SetLabel;

            UpGroups(GroupsInt, Types.SelectedNode.Text);


        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (DelGroups(Types.SelectedNode.Text))
            {
                MessageBox.Show("更新成功！");
            }
            else
            {
                MessageBox.Show("更新失败！");
            }


        }

        private void Types_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (SelectGroups(string.Format(" type ='0'", e.Label)).Count > 0)
            {
                e.CancelEdit = true;
                MessageBox.Show("组名已经存在，请更换另一个名字", "组重复");
                return;
            }
            else
            {
                GroupsIntEnity UPusersEnity = new GroupsIntEnity();
                UPusersEnity.Type = e.Label;
                e.CancelEdit = !UpGroups(UPusersEnity, e.Node.Text);
            }

            ////在用户编辑节点以后发生
            ////Change type name
            //if (m_xmlusers.Types.Contains(e.Label))
            //{
            //    e.CancelEdit = true;
            //    MessageBox.Show("组名已经存在，请更换另一个名字", "组重复");
            //    return;
            //}
            //e.CancelEdit = !m_xmlusers.ModifyTypeName(e.Node.Text, e.Label);

        }

        private void Adduser_Click(object sender, EventArgs e)
        {
            //查看是否存在

            int i = 0;
            i = this.userGrid.Rows.Add();
            this.userGrid.Rows[i].Cells["No"].Value = (i + 1).ToString();
            this.userGrid.Rows[i].Cells["Name"].Value = "用户" + (i + 1); //user




            UsersEnity ADDUser = new UsersEnity();
            ADDUser.Name = "用户" + (i + 1); //user
            ADDUser.Number = "";
            ADDUser.Password = "";
            ADDUser.Description = "";
            ADDUser.Type = Types.SelectedNode.Text;
            AddUser("", ADDUser);



            // ((DataGridViewComboBoxCell)(this.userGrid.Rows[i].Cells["Type"])).Items.AddRange(this.m_xmlusers.Types); //types

            //添加用户

            //if (m_xmlusers == null) return;

            ////create new user name
            //int i = m_xmlusers.USERS.Count() + 1;
            //string usr = "";
            //do
            //{
            //    usr = "用户" + i.ToString();
            //    i++;
            //} while (m_xmlusers.USERS.Contains(usr));


            ////add user
            //if (m_xmlusers.AddUser(usr))
            //{
            //    i = this.userGrid.Rows.Add();
            //    this.userGrid.Rows[i].Cells["No"].Value = (i + 1).ToString();
            //    this.userGrid.Rows[i].Cells["Name"].Value = usr; //user
            //    if (Types.SelectedNode.Level == 0)
            //    {
            //        //set to unknow type
            //        ((DataGridViewComboBoxCell)(this.userGrid.Rows[i].Cells["Type"])).
            //            Items.AddRange(this.m_xmlusers.Types); //types
            //        this.userGrid.Rows[i].Cells["Type"].Value = m_xmlusers.Type(usr);
            //    }
            //    else
            //    {
            //        //selected user type
            //        if (!m_xmlusers.SetTypeOfUser(usr, Types.SelectedNode.Text))
            //        {
            //            MessageBox.Show("设置用户类型失败");
            //        }
            //    }
            //    //this.userGrid.Rows[i].Cells[2].Value = m_xmlusers.Password(usr);
            //}
            //else
            //{
            //    MessageBox.Show("添加用户失败");
            //}
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {


        }

        private void userGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //当前选定单元格的编辑模式停止时发生
            if (userGrid.CurrentRow == null) return;
            object obj = pEditControl.Text;
            if (obj == null) return;
            switch (userGrid.Columns[e.ColumnIndex].Name)
            {
                case "Name":
                    if (obj.ToString() == m_strOldName) return;
                    for (int i = 0; i < usersEnityList.Count; i++)
                    {
                        if (m_strOldName == usersEnityList[i].Name)
                        {
                            usersEnityList[i].Name = obj.ToString();
                            if (UpUser(usersEnityList[i]))
                            {
                                break;
                            }
                            else
                            {
                                MessageBox.Show("更改名字失败");
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        private void userGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //单元格的值更改时发生
            usersEnityList = SelectUser("");

            if (userGrid.CurrentRow == null) return;
            object obj = userGrid.CurrentRow.Cells[e.ColumnIndex].Value;
            if (obj == null) return;
            switch (userGrid.Columns[e.ColumnIndex].Name)
            {
                //case "Name":
                //    if (!m_xmlusers.ModifyUserName(userGrid.CurrentRow.Cells["Name"].Value.ToString()
                //           , obj.ToString()))
                //    {
                //        MessageBox.Show("用户名更改失败");
                //       }
                //     break;
                case "Type": //types

                    for (int i = 0; i < usersEnityList.Count; i++)
                    {
                        if (userGrid.CurrentRow.Cells["Name"].Value.ToString() == usersEnityList[i].Name)
                        {
                            usersEnityList[i].Type = obj.ToString();
                            if (UpUser(usersEnityList[i]))
                            {
                                break;
                            }
                            else
                            {
                                MessageBox.Show("设置用户类型失败");
                                break;
                            }
                        }
                    }
                    break;

                case "Description":
                    for (int i = 0; i < usersEnityList.Count; i++)
                    {
                        if (userGrid.CurrentRow.Cells["Name"].Value.ToString() == usersEnityList[i].Name)
                        {
                            usersEnityList[i].Description = obj.ToString();
                            if (UpUser(usersEnityList[i]))
                            {
                                break;
                            }
                            else
                            {
                                MessageBox.Show("设置用户类型失败");
                                break;
                            }
                        }
                    }
                    break;

                case "Num":
                    for (int i = 0; i < usersEnityList.Count; i++)
                    {
                        if (userGrid.CurrentRow.Cells["Name"].Value.ToString() == usersEnityList[i].Name)
                        {
                            usersEnityList[i].Number = obj.ToString();
                            if (UpUser(usersEnityList[i]))
                            {
                                break;
                            }
                            else
                            {
                                MessageBox.Show("设置用户类型失败");
                                break;
                            }
                        }
                    }
                    break;

            }
        }

        private void userGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //显示用于编辑单元格的控件时发生
            this.pEditControl = e.Control;
            m_strOldName = this.pEditControl.Text;
            this.pEditControl.TextChanged += new EventHandler(EditControl_TextChanged);
        }

        private void EditControl_TextChanged(object sender, EventArgs e)
        {
            bool bExist = false;
            if (userGrid.CurrentRow == null) return;

            if (userGrid.Columns[userGrid.CurrentCell.ColumnIndex].Name == "Name") //user name
            {
                //Is this user exist in other row?
                foreach (DataGridViewRow r in userGrid.Rows)
                {
                    if (r.Cells[0].Value.ToString() == pEditControl.Text
                        && r.Index != userGrid.CurrentRow.Index)
                    {
                        bExist = true;
                    }
                }

                if (bExist)
                {
                    MessageBox.Show("该用户[" + pEditControl.Text + "]已经存在,请更换一个用户名");
                    pEditControl.Text = userGrid.CurrentRow.Cells[0].Value.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //m_xmlusersOrig.LoadXml(m_xmlusers.OuterXml);
            //m_xmlusersOrig.Save();
            this.Close();
        }

        private void RenameType_Click(object sender, EventArgs e)
        {
            Types.LabelEdit = true;
            Types.SelectedNode.BeginEdit();
        }

        private void userGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void SetType_ButtonClick(object sender, EventArgs e)
        {
            //
            if (this.SetType.DropDownItems != null)
                this.SetType.ShowDropDown();

        }

        private void SetType_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //get current user
            string s = "";
            //get selected rows
            List<DataGridViewRow> li = new List<DataGridViewRow>();
            DataGridViewRow row = null;
            foreach (DataGridViewCell c in userGrid.SelectedCells)
            {
                row = userGrid.Rows[c.RowIndex];
                if (!li.Contains(row))
                    li.Add(row);
            }


            foreach (DataGridViewRow r in li)
            {
                s = r.Cells["Name"].Value.ToString();
                for (int i = 0; i < usersEnityList.Count; i++)
                {
                    if (usersEnityList[i].Name == s)
                    {
                        if (UpUser(usersEnityList[i]))
                        {
                            usersEnityList[i].Type = e.ClickedItem.Text;
                            if (UpUser(usersEnityList[i]))
                            {
                                userGrid.Rows.Remove(userGrid.CurrentRow);
                            }
                        }
                    }
                }

            }
        }

        private void deluser_Click(object sender, EventArgs e)
        {
            //Delete all selected user
            if (userGrid.SelectedCells == null) return;
            if (SelectUser("") == null) return;

            //get selected rows
            List<DataGridViewRow> li = new List<DataGridViewRow>();
            DataGridViewRow row = null;
            foreach (DataGridViewCell c in userGrid.SelectedCells)
            {
                row = userGrid.Rows[c.RowIndex];
                if (!li.Contains(row))
                    li.Add(row);
            }

            foreach (DataGridViewRow r in li)
            {
                if (DelUser(r.Cells["Name"].Value.ToString()))
                    userGrid.Rows.Remove(r);
            }

            //reset Number for all rows
            foreach (DataGridViewRow r in userGrid.Rows)
            {
                r.Cells["No"].Value = (r.Index + 1).ToString();
            }
        }

        private List<UsersEnity> SelectUser(string sql)
        {

            return SQLBAL.GetListInfo<UsersEnity>("Users", sql);
        }
        private bool DelUser(string User)
        {
            string sql = string.Format("and Name ='{0}'", User);
            return SQLBAL.Delete("Users", sql);
        }
        private bool UpUser(UsersEnity UPusersEnity)
        {
            string sql = string.Format("and ID ='{0}'", UPusersEnity.ID);
            return SQLBAL.Update<UsersEnity>(UPusersEnity, "Users", sql);
        }
        private int AddUser(string sql, UsersEnity AddusersEnity)
        {

            return SQLBAL.AddT<UsersEnity>(AddusersEnity, "Users");
        }

        #region 权限
        public List<GroupsEnity> SelectGroups(string sql)
        {

            return SQLBAL.GetListInfo<GroupsEnity>("Groups", sql);
        }
        private bool DelGroups(string Type)
        {
            string sql = string.Format("and Type ='{0}'", Type);
            return SQLBAL.Delete("Groups", sql);
        }
        private bool UpGroups(GroupsIntEnity UPusersEnity, string OldType)
        {
            string sql = string.Format("and Type ='{0}'", OldType);
            return SQLBAL.Update<GroupsIntEnity>(UPusersEnity, "Groups", sql);
        }
        private int AddGroupsr(string sql, GroupsIntEnity AddGroupEntiey)
        {
            return SQLBAL.AddT<GroupsIntEnity>(AddGroupEntiey, "Groups");
        }
        #endregion
    }
}
