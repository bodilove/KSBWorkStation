using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.Xml;

namespace ProjectFileEditor
{
    public partial class frmConfigure : Form
    {
        private string Xmlpath = "";

        public frmConfigure(string xmlpath)
        {
            InitializeComponent();
            if (xmlpath != "")
            {
                Xmlpath = xmlpath;
                this.Text = "修改配置文件";
            }
            else 
            {
                this.Text = "添加配置文件";
           
            }
        }
        private List<TestStep> tlist = null;
        private Dictionary<string, string> tdict = new Dictionary<string, string>();

        private Dictionary<string, List<string>> failureStepsList = new Dictionary<string, List<string>>();//报错测试步骤
        private Dictionary<string, List<string>> notTestStepsList = new Dictionary<string, List<string>>();//无线测试步骤
        private List<string> nameList = new List<string>();//所有测试名
        private string filenames = "";
        private void frmConfigure_Load(object sender, EventArgs e)
        {
            //获取所有步骤
            filenames = Common.Command.prj.File.Substring(Common.Command.prj.File.LastIndexOf("\\") + 1);
            GetStep();
            if (Xmlpath!="")
            {
                loadxml();
            }
        }

        //加载xml
        public void loadxml() 
        {
            try
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Xmlpath);
                XmlNode root = xmlDoc.SelectSingleNode("Samples");
                XmlElement xroot = (XmlElement)root;
             
                if (xroot.GetAttribute("Name") != filenames)
                {
                    if (MessageBox.Show("你修改的配置与项目不匹配！确定要修改吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        this.Close();
                    }
                    else 
                    {
                        filenames = xroot.GetAttribute("Name");
                    }
                }
                XmlNodeList rootchild = root.ChildNodes;
                foreach (XmlNode xn in rootchild)
                {

                    XmlNode roots= xn.SelectSingleNode("Name");
                    this.txt_Name.Items.Add(roots.InnerText);
                    this.nameList.Add(roots.InnerText);

                    XmlNode rootFailureSteps = xn.SelectSingleNode("FailureSteps");
                    //获取到所有<users>的子节点
                    XmlNodeList rootFailureStep = rootFailureSteps.ChildNodes;
                    //遍历所有子节点
                    foreach (XmlNode xn1 in rootFailureStep)
                    {
                        XmlElement xe = (XmlElement)xn1;
                        //listBox_F.Items.Add(xe.GetAttribute("name"));

                        //selectDataGridview(xe.GetAttribute("name"));
                        if (!failureStepsList.ContainsKey(roots.InnerText))
                        {
                            List<string> flist = new List<string>();
                            flist.Add(xe.GetAttribute("name"));
                            failureStepsList.Add(roots.InnerText, flist);
                        }
                        else
                        {
                            failureStepsList[roots.InnerText].Add(xe.GetAttribute("name"));
                        }
                    }


                    XmlNode NotTestSteps = xn.SelectSingleNode("NotTestSteps");
                    //获取到所有<users>的子节点
                    XmlNodeList NotTestStep = NotTestSteps.ChildNodes;
                    //遍历所有子节点
                    foreach (XmlNode xn2 in NotTestStep)
                    {
                        XmlElement xe = (XmlElement)xn2;
                        if (!notTestStepsList.ContainsKey(roots.InnerText))
                        {
                            List<string> flist = new List<string>();
                            flist.Add(xe.GetAttribute("name"));
                            notTestStepsList.Add(roots.InnerText, flist);
                        }
                        else
                        {
                            notTestStepsList[roots.InnerText].Add(xe.GetAttribute("name"));
                        }
                    }

                }
                if(this.txt_Name.Items.Count>0)
                {
                    this.txt_Name.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"错误提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }


        public void selectDataGridview(string name1) 
        {
            for (int i = 0; i < dataGridView1.Rows.Count;i++ )
            {
                if (dataGridView1.Rows[i].Cells["D_Name"].Value.ToString() == name1)
                {
                    dataGridView1.Rows.RemoveAt(i);
                    i = i - 1;
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //获取所有的Step
        private void GetStep() 
        {
            tlist = Command.prj.TestSteps;
            if (tlist != null && tlist.Count > 0 && this.txt_Name.Items.Count>0)
            {
                for (int i = 0; i < tlist.Count; i++)
                {
                    for (int j = 0; j < tlist[i].SubTest.Count; j++)
                    {

                        this.dataGridView1.Rows.Add(false,tlist[i].SubTest[j].Name,tlist[i].SubTest[j].Description);
                        if (!tdict.ContainsKey(tlist[i].SubTest[j].Name))
                        {
                            tdict.Add(tlist[i].SubTest[j].Name, tlist[i].SubTest[j].Description);
                        }
                    }
                }
                this.btn_remove_F.Enabled = true;
                this.btn_remove_N.Enabled = true;
                this.btn_select_F.Enabled = true;
                this.btn_select_N.Enabled = true;
                this.btn_save.Enabled = true;
            }
            else 
            {
                this.btn_remove_F.Enabled = false;
                this.btn_remove_N.Enabled = false;
                this.btn_select_F.Enabled = false;
                this.btn_select_N.Enabled = false;
                this.btn_save.Enabled = false;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //判断是否符合保存的要求，符合要求再进行保存
            if (Judge())
            {
                SaveXML(); 
            }
        }

        //判断是否符合保存的要求
        public bool Judge() 
        {
            bool yorn = true;
            if(this.txt_Name.Text.Trim()=="")
            {
                yorn = false;
                MessageBox.Show("Name不能为空！","提示");
            }
            //else if (listBox_F.Items.Count == 0 && listBox_N.Items.Count == 0)
            //{
            //    yorn = false;
            //    MessageBox.Show("FailureSteps和NotTestSteps至少有一个不能为空！", "提示");
            //}
            return yorn;
        }

        private void btn_select_F_Click(object sender, EventArgs e)
        {
            AddSelect(this.listBox_F);
        }

        //添加选择项
        private void AddSelect(ListBox listbox) 
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count;i++ )
            {
       
                if((bool)dataGridView1.Rows[i].Cells[0].EditedFormattedValue==true)
                {
                    listbox.Items.Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                    
                 
                    if (listbox == this.listBox_F)
                    {
                        if (!failureStepsList.ContainsKey(this.txt_Name.Text))
                        {
                            List<string> flist = new List<string>();
                            flist.Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                            failureStepsList.Add(this.txt_Name.Text, flist);
                        }
                        else 
                        {
                            failureStepsList[this.txt_Name.Text].Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                        }
                    }
                    else
                    {
                        if (!notTestStepsList.ContainsKey(this.txt_Name.Text))
                        {
                            List<string> nlist = new List<string>();
                            nlist.Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                            notTestStepsList.Add(this.txt_Name.Text, nlist);
                        }
                        else
                        {
                            notTestStepsList[this.txt_Name.Text].Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                        }
                    }

                    dataGridView1.Rows.RemoveAt(i);
                    i = i - 1;

                }
            }

  
        }
        //删除选择项
        private void DelSelect(ListBox listbox)
        {
            for (int i = 0; i < listbox.Items.Count; i++)
            {

                if (listbox.GetSelected(i))
                {
                    if (tdict.ContainsKey(listbox.Items[i].ToString()))
                    {
                        this.dataGridView1.Rows.Add(false, listbox.Items[i].ToString(), tdict[listbox.Items[i].ToString()]);
                    }
                  
                

                    if (listbox == this.listBox_F)
                    {
                        if (failureStepsList.ContainsKey(this.txt_Name.Text))
                        {
                            failureStepsList[this.txt_Name.Text].Remove(listbox.Items[i].ToString());
                        }
                 
                    }
                    else
                    {
                        if (notTestStepsList.ContainsKey(this.txt_Name.Text))
                        {
                            notTestStepsList[this.txt_Name.Text].Remove(listbox.Items[i].ToString());
                        }
                    }
                    listbox.Items.RemoveAt(i);
                    i = i - 1;
                }
            }

        }

        private void btn_remove_F_Click(object sender, EventArgs e)
        {
            DelSelect(this.listBox_F);
        }

        private void btn_select_N_Click(object sender, EventArgs e)
        {
            AddSelect(this.listBox_N);

        }

        private void btn_remove_N_Click(object sender, EventArgs e)
        {
            DelSelect(this.listBox_N);
        }

        //保存xml
        private void SaveXML() 
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            XmlNode node1 = doc.CreateElement("Samples");
            XmlElement xenode1 = (XmlElement)node1;
            xenode1.SetAttribute("Name", filenames);
            doc.AppendChild(node1);

            for (int m = 0; m < nameList.Count; m++)
            {
                //创建节点（二级）
                XmlNode node2 = doc.CreateElement("Sample");
                node2.Attributes.Append(doc.CreateAttribute("ID")).Value = (m + 1).ToString(); ;
                node1.AppendChild(node2);

                //创建节点（三级）
                //Name节点
                XmlNode node3_1 = doc.CreateElement("Name");
                node3_1.InnerText = nameList[m];
                node2.AppendChild(node3_1);

                //FailureSteps节点
                XmlNode node3_2 = doc.CreateElement("FailureSteps");
                node2.AppendChild(node3_2);

                if (failureStepsList.ContainsKey(nameList[m]))
                {
                    for (int i = 0; i < failureStepsList[nameList[m]].Count; i++)
                    {
                        XmlNode node3_2_1 = doc.CreateElement("FailureStep");
                        node3_2_1.Attributes.Append(doc.CreateAttribute("ID")).Value = (i + 1).ToString();
                        node3_2_1.Attributes.Append(doc.CreateAttribute("name")).Value = failureStepsList[nameList[m]][i].ToString();
                        node3_2.AppendChild(node3_2_1);
                    }
                }



                //NotTestSteps节点
                XmlNode node3_3 = doc.CreateElement("NotTestSteps");
                node2.AppendChild(node3_3);
                if (notTestStepsList.ContainsKey(nameList[m]))
                {
                    for (int j = 0; j < notTestStepsList[nameList[m]].Count; j++)
                    {
                        XmlNode node3_3_1 = doc.CreateElement("NotTestStep");
                        node3_3_1.Attributes.Append(doc.CreateAttribute("ID")).Value = (j + 1).ToString();
                        node3_3_1.Attributes.Append(doc.CreateAttribute("name")).Value = notTestStepsList[nameList[m]][j].ToString();
                        node3_3.AppendChild(node3_3_1);
                    }
                }
            }

            if (Xmlpath == "")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "XML文件|*.XML";
                dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    //Prj.SaveAs(dlg.FileName);
                    doc.Save(dlg.FileName);
                }
            }
            else
            {
                try
                {
                    doc.Save(Xmlpath);
                    MessageBox.Show("保存成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (this.panel1.Visible == false)
            {
                this.panel1.Visible = true;
                this.txt_addname.Text = "";
                this.label6.Text = "";
            }
            else 
            {
                this.panel1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.label6.Text = "";
            if (this.txt_addname.Text.Trim()== "")
            {
                this.label6.Text = "Name不能为空！";
            }
            else 
            {
                bool yorn = true;
                for (int i = 0; i < txt_Name.Items.Count;i++ )
                {
                    if (this.txt_addname.Text.Trim() == txt_Name.Items[i].ToString())
                    {
                        yorn = false;
                        break;
                    }
                }
                if (yorn)
                {
                    this.txt_Name.Items.Add(this.txt_addname.Text.Trim());
                    this.txt_Name.Text = this.txt_addname.Text.Trim();
                    
                    this.panel1.Visible = false;
                    nameList.Add(this.txt_Name.Text);
                }
                else 
                {
                    this.label6.Text = "Name已存在！";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nameList.Contains(this.txt_Name.Text.Trim()))
            {
                nameList.Remove(this.txt_Name.Text.Trim());
            }
            if (failureStepsList.ContainsKey(this.txt_Name.Text.Trim()))
            {
                failureStepsList.Remove(this.txt_Name.Text.Trim());
            }
            if (notTestStepsList.ContainsKey(this.txt_Name.Text.Trim()))
            {
                notTestStepsList.Remove(this.txt_Name.Text.Trim());
            }
            this.txt_Name.Items.Remove(this.txt_Name.Text.Trim());
            if (this.txt_Name.Items.Count > 0)
            {
                this.txt_Name.SelectedIndex = 0;
            }
            else 
            {
                this.btn_remove_F.Enabled = false;
                this.btn_remove_N.Enabled = false;
                this.btn_select_F.Enabled = false;
                this.btn_select_N.Enabled = false;
                this.btn_save.Enabled = false;
            }
     
        }

        private void txt_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            GetStep();
         
            this.listBox_F.Items.Clear();
            this.listBox_N.Items.Clear();
            if (this.txt_Name.Items.Count == 0)
            {
                this.btn_remove_F.Enabled = false;
                this.btn_remove_N.Enabled = false;
                this.btn_select_F.Enabled = false;
                this.btn_select_N.Enabled = false;
                //this.btn_save.Enabled = false;
            }
            else 
            {
                this.btn_remove_F.Enabled = true;
                this.btn_remove_N.Enabled = true;
                this.btn_select_F.Enabled = true;
                this.btn_select_N.Enabled = true;
            }
            if (failureStepsList.ContainsKey(this.txt_Name.Text.Trim()))
            {


                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < failureStepsList[this.txt_Name.Text.Trim()].Count;j++ )
                    {
                        if (this.dataGridView1.Rows[i].Cells["D_Name"].Value.ToString() == failureStepsList[this.txt_Name.Text.Trim()][j])
                        {
                            listBox_F.Items.Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                            dataGridView1.Rows.RemoveAt(i);
                          
                        }
                    }
                }
            }
            if (notTestStepsList.ContainsKey(this.txt_Name.Text.Trim()))
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < notTestStepsList[this.txt_Name.Text.Trim()].Count; j++)
                    {
                        if (this.dataGridView1.Rows[i].Cells["D_Name"].Value.ToString() == notTestStepsList[this.txt_Name.Text.Trim()][j])
                        {
                            listBox_N.Items.Add(dataGridView1.Rows[i].Cells["D_Name"].Value.ToString());
                            dataGridView1.Rows.RemoveAt(i);
                      
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }
    }
}
