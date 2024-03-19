using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Test.Common;

namespace Test.ProjectFileEditor
{
    public partial class frmPowerOnTestConfigure : Form
    {
        public frmPowerOnTestConfigure()
        {
            InitializeComponent();
        }
        #region
        Project _prj = null;
        string _path = "";
        TestPowerOnManage MyTestManage = null;

        TestPowerOn CurrentTPO = null;
        #endregion
        public frmPowerOnTestConfigure(Project prj, string Path)
        {
            _prj = prj;
            _path = Path;
            InitializeComponent();
        }
        void MyTestManageLoad()
        {
            try
            {
                MyTestManage = new TestPowerOnManage();
                MyTestManage.Load(_path);
                for (int i = 0; i < MyTestManage.PowerOnLst.Count; i++)
                {
                    CheckAllSubTestStep(MyTestManage.PowerOnLst[i]);
                }
                RefreshTestPowerOnNode();
            }
            catch
            {
                DialogResult dr = MessageBox.Show("导入文件出错，可能是文件格式不对或者路径出错,是否重新生成配置文件？", "报警", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    MyTestManage = new TestPowerOnManage();
                }
                else
                {
                    this.Close();
                }
            }
        }


        void RefreshTestPowerOnNode()
        {
            TreePowerOnTest.Nodes.Clear();
            for (int i = 0; i < MyTestManage.PowerOnLst.Count; i++)
            {
                //CheckAllSubTestStep(MyTestManage.PowerOnLst[i]);
                TreeNode tn = new TreeNode();
                tn.Name = i.ToString();
                tn.Text = MyTestManage.PowerOnLst[i].Name;
                TreePowerOnTest.Nodes.Add(tn);
            }
        }

        void RefreshSubTestStep(TestPowerOn tpo)
        {
            tpo.TestPowerSubTestSteplist.Sort();
            this.GridView.Rows.Clear();

            foreach (TestPowerSubTestStep tpss in tpo.TestPowerSubTestSteplist)
            {
                int x = this.GridView.Rows.Add(new object[] { tpss.Name, tpss.Ischecked, tpss.Enable, null, tpss.ULimit, tpss.LLimit, tpss.NomValue, tpss.Discription });
                GridView.Rows[x].Cells[3] = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell cmbC = (DataGridViewComboBoxCell)GridView.Rows[x].Cells[3];
                foreach (string s in Enum.GetNames(typeof(CompareMode)))
                {
                    cmbC.Items.Add(s);
                }
                cmbC.Value = tpss.CmpMode.ToString();
                GridView.Rows[x].ReadOnly = true; ;
                GridView.Rows[x].Cells[0].ReadOnly = false;
                if (tpss.Ischecked)
                {
                    GridView.Rows[x].Cells[0].ReadOnly = true;
                    GridView.Rows[x].Cells[1].ReadOnly = false;
                    GridView.Rows[x].Cells[2].ReadOnly = false;
                    GridView.Rows[x].Cells[3].ReadOnly = false;
                    GridView.Rows[x].Cells[4].ReadOnly = false;
                    GridView.Rows[x].Cells[5].ReadOnly = false;
                    GridView.Rows[x].Cells[6].ReadOnly = false;
                    GridView.Rows[x].Cells[7].ReadOnly = true;
                }
                else
                {
                    GridView.Rows[x].Cells[0].ReadOnly = true;
                    GridView.Rows[x].Cells[1].ReadOnly = false;
                    GridView.Rows[x].Cells[2].ReadOnly = true;
                    GridView.Rows[x].Cells[3].ReadOnly = true;
                    GridView.Rows[x].Cells[4].ReadOnly = true;
                    GridView.Rows[x].Cells[5].ReadOnly = true;
                    GridView.Rows[x].Cells[6].ReadOnly = true;
                    GridView.Rows[x].Cells[7].ReadOnly = true;
                }
            }
        }

        void CheckAllSubTestStep(TestPowerOn tp)
        {
            //遍历开机测试的所有小步，如果在序列中不包含就删除;
            for (int i = 0; i < tp.TestPowerSubTestSteplist.Count; i++)
            {
                if (!subTestSteppContains(tp.TestPowerSubTestSteplist[i].Name))
                {
                    tp.TestPowerSubTestSteplist.RemoveAt(i);
                }
            }




            foreach (TestStep ts in _prj.TestSteps)
            {
                foreach (SubTestStep st in ts.SubTest)
                {
                    TestPowerSubTestStep tpsts1 = TestPowerSubTestStepContains(st.Name, tp);
                    if (tpsts1 != null)
                    {
                        tpsts1.OrgSubTestStep = st.Clone() as SubTestStep;
                    }
                    else
                    {
                        //不包含就添加
                        TestPowerSubTestStep tpsts = new TestPowerSubTestStep();
                        tpsts.OrgSubTestStep = st.Clone() as SubTestStep;
                        tpsts.CmpMode = st.CmpMode;
                        tpsts.Discription = st.Description;
                        tpsts.Enable = st.Enable;
                        tpsts.Ischecked = false;
                        tpsts.LLimit = st.LLimit;
                        tpsts.Name = st.Name;
                        tpsts.NomValue = st.NomValue;
                        tpsts.ULimit = st.ULimit;
                        tp.TestPowerSubTestSteplist.Add(tpsts);
                    }
                }
            }
            tp.TestPowerSubTestSteplist.Sort();
        }



        TestPowerSubTestStep TestPowerSubTestStepContains(string SubStepName, TestPowerOn tp)
        {
            TestPowerSubTestStep st = null;
            foreach (TestPowerSubTestStep tsst in tp.TestPowerSubTestSteplist)
            {
                if (tsst.Name == SubStepName)
                {
                    st = tsst;
                }
            }
            return st;
        }

        bool subTestSteppContains(string StepName)
        {
            foreach (TestStep ts in _prj.TestSteps)
            {
                foreach (SubTestStep st in ts.SubTest)
                {
                    if (StepName == st.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        private void frmPowerOnTestConfigure_Load(object sender, EventArgs e)
        {
            MyTestManageLoad();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            diagSaveFile.InitialDirectory = Application.StartupPath + "\\TestPoweOnConfig";

            int startIndex = Global.Prj.Title.Length - 4;
            diagSaveFile.FileName = Global.Prj.Title.Remove(startIndex, 4) + "_" + "TesPowerON.xml";
            string path = "";
            if (diagSaveFile.ShowDialog(this) == DialogResult.OK)
            {
                path = diagSaveFile.FileName;
                MyTestManage.Save(path);
                MessageBox.Show("已保存！");
            }
        }

        private void TreePowerOnTest_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int i = e.Node.Index;
            CurrentTPO = MyTestManage.PowerOnLst[i];
            RefreshSubTestStep(MyTestManage.PowerOnLst[i]);
            txtName.Text = CurrentTPO.Name;
            txtSN.Text = CurrentTPO.ProductSN;
        }

        private void GridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                if (GridView.Rows[i].Cells["ColChecked"].Value == null)
                {
                    GridView.Rows[i].Cells["ColChecked"].Value = false;
                }
                CurrentTPO.TestPowerSubTestSteplist[i].Ischecked = (bool)GridView.Rows[i].Cells["ColChecked"].Value;

                if (GridView.Rows[i].Cells["ColName"].Value == null)
                {
                    GridView.Rows[i].Cells["ColName"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].Name = GridView.Rows[i].Cells["ColName"].Value.ToString();
                //ColCpm


                if (GridView.Rows[i].Cells["ColCpm"].Value == null)
                {
                    GridView.Rows[i].Cells["ColCpm"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].CmpMode = (CompareMode)System.Enum.Parse(typeof(CompareMode), GridView.Rows[i].Cells["ColCpm"].Value.ToString(), true);


                if (GridView.Rows[i].Cells["ColULimit"].Value == null)
                {
                    GridView.Rows[i].Cells["ColULimit"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].ULimit = GridView.Rows[i].Cells["ColULimit"].Value.ToString();

                if (GridView.Rows[i].Cells["ColLLimit"].Value == null)
                {
                    GridView.Rows[i].Cells["ColLLimit"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].LLimit = GridView.Rows[i].Cells["ColLLimit"].Value.ToString();

                if (GridView.Rows[i].Cells["ColNom"].Value == null)
                {
                    GridView.Rows[i].Cells["ColNom"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].NomValue = GridView.Rows[i].Cells["ColNom"].Value.ToString();

                if (GridView.Rows[i].Cells["ColDescription"].Value == null)
                {
                    GridView.Rows[i].Cells["ColDescription"].Value = "";
                }
                CurrentTPO.TestPowerSubTestSteplist[i].Discription = GridView.Rows[i].Cells["ColDescription"].Value.ToString();


                if (GridView.Rows[i].Cells["ColIsTest"].Value == null)
                {
                    GridView.Rows[i].Cells["ColIsTest"].Value = false;
                }
                CurrentTPO.TestPowerSubTestSteplist[i].Enable = (bool)GridView.Rows[i].Cells["ColIsTest"].Value;
            }



        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            CurrentTPO.Name = txtName.Text;
        }

        private void txtSN_Leave(object sender, EventArgs e)
        {
            CurrentTPO.ProductSN = txtSN.Text;
        }

        private void btnRemovePowerOnTest_Click(object sender, EventArgs e)
        {
            if (TreePowerOnTest.SelectedNode == null) return;
            int i = int.Parse(TreePowerOnTest.SelectedNode.Name);
            MyTestManage.PowerOnLst.RemoveAt(i);
            RefreshTestPowerOnNode();
            i = i - 1;
            if (i >= 0)
            {
                TreePowerOnTest.SelectedNode = TreePowerOnTest.Nodes[i]; ;
            }
            else
            {
                GridView.Rows.Clear();
            }
        }

        private void btnAddPowerOnTest_Click(object sender, EventArgs e)
        {
            frmAddOneTestPowerOn fm = new frmAddOneTestPowerOn();
            DialogResult dr = fm.ShowDialog();

            if (dr == DialogResult.OK)
            {
                TestPowerOn tpo = new TestPowerOn();
                tpo.Name = fm.Name;
                tpo.ProductSN = fm.SN;
                MyTestManage.PowerOnLst.Add(tpo);
                CheckAllSubTestStep(tpo);
            }
            else
            {

            }
            RefreshTestPowerOnNode();
        }

        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            if (CurrentTPO.TestPowerSubTestSteplist[x].Ischecked)
            {
                GridView.Rows[x].Cells[0].ReadOnly = true;
                GridView.Rows[x].Cells[1].ReadOnly = false;
                GridView.Rows[x].Cells[2].ReadOnly = false;

                GridView.Rows[x].Cells[3].ReadOnly = false;
                GridView.Rows[x].Cells[4].ReadOnly = false;
                GridView.Rows[x].Cells[5].ReadOnly = false;
                GridView.Rows[x].Cells[6].ReadOnly = false;
                GridView.Rows[x].Cells[7].ReadOnly = true;
                //GridView.Rows[x].Cells[4].Value = CurrentTPO.TestPowerSubTestSteplist[x].ULimit.ToString();
                //GridView.Rows[x].Cells[5].Value = CurrentTPO.TestPowerSubTestSteplist[x].LLimit.ToString();
                //GridView.Rows[x].Cells[6].Value = CurrentTPO.TestPowerSubTestSteplist[x].NomValue.ToString();
            }
            else
            {
                GridView.Rows[x].Cells[0].ReadOnly = true;
                GridView.Rows[x].Cells[1].ReadOnly = false;
                GridView.Rows[x].Cells[2].ReadOnly = true;

                GridView.Rows[x].Cells[3].ReadOnly = true;
                GridView.Rows[x].Cells[4].ReadOnly = true;
                GridView.Rows[x].Cells[5].ReadOnly = true;
                GridView.Rows[x].Cells[6].ReadOnly = true;
                GridView.Rows[x].Cells[7].ReadOnly = true;
                //GridView.Rows[x].Cells[4].Value = CurrentTPO.TestPowerSubTestSteplist[x].OrgSubTestStep.ULimit.ToString();
                //GridView.Rows[x].Cells[5].Value = CurrentTPO.TestPowerSubTestSteplist[x].OrgSubTestStep.LLimit.ToString();
                //GridView.Rows[x].Cells[6].Value = CurrentTPO.TestPowerSubTestSteplist[x].OrgSubTestStep.NomValue.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        bool IsChecked = false;
        bool IsTest = false;
        private void GridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (!IsChecked)
                {
                    IsChecked = true;
                    for (int i = 0; i < GridView.Rows.Count; i++)
                    {
                        GridView.Rows[i].Cells[e.ColumnIndex].Value = true;
                    }
                }
                else
                {
                    IsChecked = false;
                    for (int i = 0; i < GridView.Rows.Count; i++)
                    {
                        GridView.Rows[i].Cells[e.ColumnIndex].Value = false;
                    }
                }
            }
            if (e.ColumnIndex == 2)
            {
                if (!IsTest)
                {
                    IsTest = true;
                    for (int i = 0; i < GridView.Rows.Count; i++)
                    {
                        if (GridView.Rows[i].Cells[1].Value.ToString() == "True")
                            GridView.Rows[i].Cells[e.ColumnIndex].Value = true;
                    }
                }
                else
                {
                    IsTest = false;
                    for (int i = 0; i < GridView.Rows.Count; i++)
                    {
                        if (GridView.Rows[i].Cells[1].Value.ToString() == "True")
                            GridView.Rows[i].Cells[e.ColumnIndex].Value = false;
                    }
                }
            }
        }
    }
}
