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

namespace MainControl
{
    public partial class FrmToolTestInfo : WindowParent
    {
        //SystemOrganization_Bll organization_bll = new SystemOrganization_Bll();
        //// SystemUserInfo_Dal user_dal = new SystemUserInfo_Dal();
        //SystemUserInfo_Bll user_bll = new SystemUserInfo_Bll();
        //SystemMenu_Bll menu_bll = new SystemMenu_Bll();
        //TSHandler TShandler = new TSHandler(Application.StartupPath + @"\TestSequence.xml");

        string filePath = Application.StartupPath + @"\TestSequence.xml";
        MyTest m = null;
        public FrmToolTestInfo()
        //public FrmToolTestInfo(string ParentId)
        {
            InitializeComponent();
            //SetButton(ParentId, this.toolStrip1);//设置按钮权限
            //Dictionary<int,string> lst= TShandler.GetTS_FilePathLst();
            //m = new MyTest();

            //加载xml数据文件
            XmlDocument xmlDoc = LoadXmlDoc(filePath);
            m = XmlHelper<MyTest>.DeserializeToObject(xmlDoc.InnerXml);
            string aa=string.Empty;
        }
   
        private void FrmToolTestInfo_Load(object sender, EventArgs e)
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
            dr1["Name"] = "项目名称";
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
                treeView1.Nodes.Clear();
                //list = organization_bll.GetOrganizations();
                if (Program.CurrentConfig != null)
                {
                    //dgvlocalStConfig.Rows.Clear();

                    //foreach (LocalStationConfig lst in )
                    //{
                    //    dgvlocalStConfig.Rows.Add(new object[] { lst.StationNum, lst.StationName });
                    //}

                    var parents = Program.CurrentConfig.localStlst;

                    //var parents = list.Where(o => o.ParentId == "0");
                    foreach (var item in parents)
                    {
                        TreeNode tn = new TreeNode();
                        tn.Text = $"[{item.StationNum}]{item.StationName}";
                        tn.Tag = item.StationNum;
                        tn.ImageIndex = 0;
                        //FillTree(tn, list);
                        treeView1.Nodes.Add(tn);
                    }
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
                //    ClassName = typeof(FrmUserInfo).ToString()
                //});
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
            //e.Node.BackColor = Color.Blue;

            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParameter> IList_param = new List<SqlParameter>();
            //if (!string.IsNullOrEmpty(Organization_ID))
            //{
            //    SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            //    IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID ));
            //}
            dataGridView1.DataSource = null;
            DataTable dt= new DataTable();
            if (m != null)
            {
                TestSequences item = m.TestSequences.Where(p => p.StationNum == Organization_ID).FirstOrDefault();
                if (item != null && item.TS != null)
                {
                    item.TS.ForEach(o => o.StationNum = Organization_ID);
                    this.dataGridView1.DataSource = item.TS;
                }
                
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
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            } 
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            //  StringBuilder SqlWhere = new StringBuilder();
            //  IList<SqlParameter> IList_param = new List<SqlParameter>();

            string Organization_ID=string.Empty;
            if (!string.IsNullOrEmpty(txt_Search.Text))
            {
                //SqlWhere.Append(" and U." + com_Searchwhere.ComboBox.SelectedValue.ToString() + " like @obj ");
                //IList_param.Add(new SqlParameter("@obj", '%' + txt_Search.Text.Trim() + '%'));
            }
            if (!string.IsNullOrEmpty(treeView1.SelectedNode.Tag.ToString()))
            {
                Organization_ID = treeView1.SelectedNode.Tag.ToString();
                //SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
                //IList_param.Add(new SqlParameter("@Organization_ID", treeView1.SelectedNode.Tag.ToString()));
            }
            if (m != null)
            {

                //TestSequences item = m.TestSequences.Where(p => p.StationNum .Contains(Organization_ID)).FirstOrDefault();

                List<TS> lstTS=new List<TS>();
                
                List<TestSequences> lstTestSequences =new List<TestSequences>();
                if (String.IsNullOrEmpty(Organization_ID))
                {
                    lstTestSequences = m.TestSequences;
                }
                else
                {
                    lstTestSequences = m.TestSequences.Where(p => p.StationNum.Contains(Organization_ID)).ToList();
                }

                foreach (TestSequences item in lstTestSequences)
                {
                    lstTS.AddRange( item.TS.Where(p=>p.Name.Contains(txt_Search.Text)).ToList());
                }

                if (lstTS != null && lstTS.Count>0)
                {
                    //item.TS.ForEach(o => o.StationNum = Organization_ID);
                    this.dataGridView1.DataSource = lstTS;
                }

            }
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
                //FrmUserInfoEdit edit = new FrmUserInfoEdit(this);
                //edit.ShowDialog();
                TreeNode treeNode= treeView1.SelectedNode;
                if (treeNode == null)
                {
                    MessageBox.Show("请选择工位节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string StationNum = treeView1.SelectedNode.Tag.ToString();
                if (String.IsNullOrEmpty(StationNum))
                {
                    MessageBox.Show("请选择工位节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Test.ProjectFileEditor.frmMain frm = new Test.ProjectFileEditor.frmMain();

                frm.StationNum = StationNum;
                frm.OptionType = 0;

                // 订阅弹出窗口的事件
                frm.CustomControlClicked += PopupForm_ObjectSelected;
                frm.ShowDialog();
               

                //if (lstTS.SelectedItems.Count == 0)
                //{
                //    frm.ShowDialog();
                //}
                //else
                //{
                //    frm.LoadFile(
                //        TShandler.GetTS_Name(lstTS.SelectedIndex + 1),
                //        TShandler.GetTS_FilePath(lstTS.SelectedIndex + 1)
                //        );
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
                //    ClassName = typeof(FrmUserInfo).ToString()
                //});
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 弹出窗口对象获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupForm_ObjectSelected(object sender, MyNewValueEventArgs e)
        {
            // 在主窗口中获取弹出窗口的对象值
            ProjectTS ts = e.NewValue;

            //添加项目
            if (ts != null)
            {

                //if (m.TestSequences == null)
                //{

                //    m.TestSequences.Add(new ts);
                //}
                TestSequences testSequences = m.TestSequences.Where(p=>p.StationNum==ts.StationNum).FirstOrDefault();
                if (testSequences == null)
                {
                    testSequences=new TestSequences() { 
                        StationNum = ts.StationNum,
                        TS = new List<TS>() { new TS() { ID = 1.ToString(),StationNum=ts.StationNum, Name = ts.Name, FilePath = ts.FilePath } }
                    };
                    m.TestSequences.Add(testSequences);
                }
                else
                {
                    if ((testSequences.TS == null))
                    {
                        testSequences.TS.Add(new TS() { ID = 1.ToString(), Name = ts.Name, StationNum = ts.StationNum, FilePath = ts.FilePath });
                    }
                    else
                    {
                        testSequences.TS.Add(new TS() { ID = (testSequences.TS.Count+1).ToString(), StationNum = ts.StationNum, Name = ts.Name, FilePath = ts.FilePath });
                    }
                    

                }
                int result = SaveXMLData(filePath, m);

                if (result == 1)
                {
                    //MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.DataSource = testSequences.TS;
                }
                else
                {
                    MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("添加项目信息为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 在这里处理获取到的对象值，例如显示在主窗体上
            //MessageBox.Show($"Selected Object: {selectedObject.Name}", "Object Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //判断是否选择编辑的行
                if (dataGridView1.DataSource == null)
                {
                    MessageBox.Show("请选择要编辑的行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    string StationNum = treeView1.SelectedNode.Tag.ToString();
                    if (String.IsNullOrEmpty(StationNum))
                    {
                        MessageBox.Show("请选择工位节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //DataGridViewRow dr = dataGridView1.SelectedRows[0];

                    //if (dr != null)
                    //{
                    //    FrmUserInfoEdit edit = new FrmUserInfoEdit(this,ref dr);
                    //    edit.ShowDialog();
                    //}
                    DataGridViewRow dr = null;
                    if (dataGridView1.SelectedRows == null)
                    {
                        MessageBox.Show("请选择要编辑的测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        dr = dataGridView1.SelectedRows[0];

                    }


                    TestSequences testItems = m.TestSequences.Where(p => p.StationNum == StationNum).FirstOrDefault();
                    if (testItems == null)
                    {
                        MessageBox.Show("没有配置测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    
                    if (dr != null)
                    {
                        Test.ProjectFileEditor.frmMain frm = new Test.ProjectFileEditor.frmMain();
                        //frm.OptionType = 1;
                        //frm.StationNum = StationNum;

                        string Name = dr.Cells["Name"].Value.ToString();
                        string FilePath= dr.Cells["FilePath"].Value.ToString();

                        frm.LoadFile( Name, FilePath );
                        
                        int result = SaveXMLData(filePath, m);

                        if (result == 1)
                        {
                            MessageBox.Show("编辑成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.dataGridView1.DataSource = null;
                            this.dataGridView1.DataSource = testItems.TS;
                        }
                        else
                        {
                            MessageBox.Show("编辑失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
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
                //    ClassName = typeof(FrmUserInfo).ToString()
                //});
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
               
                string StationNum = treeView1.SelectedNode.Tag.ToString();
                if (String.IsNullOrEmpty(StationNum))
                {
                    MessageBox.Show("请选择删除的工位节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                TestSequences testItems = m.TestSequences.Where(p => p.StationNum == StationNum).FirstOrDefault();
                if (testItems == null)
                {
                    MessageBox.Show("没有配置测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataGridViewRow dr = null;
                if (dataGridView1.SelectedRows == null)
                {
                    MessageBox.Show("请选择要删除的测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    dr = dataGridView1.SelectedRows[0];

                }
                if (dr != null)
                {

                    //MyTest m=new MyTest();
                    if (m.TestSequences != null && m.TestSequences.Count > 0)
                    {
                        
                        if (testItems != null)
                        {
                            string ID = dr.Cells["ID"].Value.ToString();
                            //移除删除项
                            if (testItems.TS != null && testItems.TS.Count > 0)
                            {
                                foreach (var item in testItems.TS)
                                {
                                    if (item.ID == ID)
                                    {
                                        testItems.TS.Remove(item);
                                        break;
                                    }

                                }
                            }
                            //将项目顺序重新怕列
                            if (testItems.TS != null && testItems.TS.Count > 0)
                            {
                                for (int i=0;i< testItems.TS.Count;i++)
                                {
                                    testItems.TS[i].ID = (i+1).ToString();

                                }
                            }

                        }
                        else {

                            MessageBox.Show("没有选择需要删除的测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有选择需要删除的测试项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //int result = bll.DeleteSysMenu(dr.Cells["Menu_Id"].Value.ToString());

                    int result = SaveXMLData(filePath,m);

                    if (result == 1)
                    {
                        this.dataGridView1.DataSource = null;
                        this.dataGridView1.DataSource = testItems.TS;

                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                    }
                    else
                    {
                        MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                //    ClassName = typeof(FrmUserInfo).ToString()
                //});
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode != null)
            //{
            //    string Organization_ID = treeView1.SelectedNode.Tag.ToString();
            //    //StringBuilder SqlWhere = new StringBuilder();
            //    //IList<SqlParameter> IList_param = new List<SqlParameter>();
            //    //if (!string.IsNullOrEmpty(Organization_ID))
            //    //{
            //    //    SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
            //    //    IList_param.Add(new SqlParameter("@Organization_ID", Organization_ID));
            //    //}
            //    if (m != null)
            //    {
            //        TestSequences item = m.TestSequences.Where(p => p.StationNum == Organization_ID).FirstOrDefault();
            //        if (item != null && item.TS != null)
            //        {
            //            item.TS.ForEach(o => o.StationNum = Organization_ID);
            //            this.dataGridView1.DataSource = item.TS;
            //        }

            //    }

            //}
            string Organization_ID = string.Empty;
            if (!string.IsNullOrEmpty(txt_Search.Text))
            {
                //SqlWhere.Append(" and U." + com_Searchwhere.ComboBox.SelectedValue.ToString() + " like @obj ");
                //IList_param.Add(new SqlParameter("@obj", '%' + txt_Search.Text.Trim() + '%'));
            }
            if (!string.IsNullOrEmpty(treeView1.SelectedNode.Tag.ToString()))
            {
                Organization_ID = treeView1.SelectedNode.Tag.ToString();
                //SqlWhere.Append(" AND S.Organization_ID =@Organization_ID");
                //IList_param.Add(new SqlParameter("@Organization_ID", treeView1.SelectedNode.Tag.ToString()));
            }
            if (m != null)
            {

                //TestSequences item = m.TestSequences.Where(p => p.StationNum .Contains(Organization_ID)).FirstOrDefault();

                List<TS> lstTS = new List<TS>();

                List<TestSequences> lstTestSequences = new List<TestSequences>();
                if (String.IsNullOrEmpty(Organization_ID))
                {
                    lstTestSequences = m.TestSequences;
                }
                else
                {
                    lstTestSequences = m.TestSequences.Where(p => p.StationNum.Contains(Organization_ID)).ToList();
                }

                foreach (TestSequences item in lstTestSequences)
                {
                    lstTS.AddRange(item.TS.Where(p => p.Name.Contains(txt_Search.Text)).ToList());
                }

                if (lstTS != null && lstTS.Count > 0)
                {
                    //item.TS.ForEach(o => o.StationNum = Organization_ID);
                    this.dataGridView1.DataSource = lstTS;
                }

            }

        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BindTreeView(organization_bll.GetOrganizations());
            //if (treeView1.Nodes.Count > 0)//展开一级节点
            //{
            //    treeView1.Nodes[0].Expand();
            //}
        }


        #region  读取xml
        public XmlDocument LoadXmlDoc(string filePath)
        {
            //加载xml文档格式
            XmlDocument doc = new XmlDocument();
            try
            {
                //读取文件内容xml数据（先定义编码）
                string str = string.Empty;
                //using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8")))
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("GB2312")))
                {
                    str = sr.ReadToEnd();
                    sr.DiscardBufferedData();
                    sr.Close();
                }


                //注册命名空间
                //XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
                //xnm.AddNamespace("x", "urn:http://www.w3.org/1999/xhtml");
                ////取值的时候一定要把x加进去
                //string xml = doc.SelectSingleNode("/x:Company/x:State", xnm).InnerText;
                doc.LoadXml(str);

            }
            catch (Exception ex)
            {
                //Logger.Error1(ex, $"读取追溯数据文件：{filePath}出错");
            }

            return doc;
        }
        /// <summary>
        /// 保存xml
        /// </summary>
        /// <returns></returns>
        public int SaveXMLData(string filePath, MyTest m)
        {
            int result = 0;
            try
            {
                // 创建路径
                //string xmlPath = SysConfig.XmlPath;
                //DirectoryUtil.CreateDirectory(xmlPath);//创建文件夹
                //将对象转换为xml
                string data = XmlHelper<MyTest>.SerializeToXml(m);
                XmlDocument xml = new XmlDocument();
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "", "no"));
                xml.LoadXml(data);

                //创建xml文件
                xml.Save($"{filePath}");
                result = 1;
            }
            catch (Exception ex)
            {
                //Logger.Error($"SaveXMLData：保存xml失败！{ex.Message.ToString()}");
            }
            return result;

        }
        #endregion
    }
}
