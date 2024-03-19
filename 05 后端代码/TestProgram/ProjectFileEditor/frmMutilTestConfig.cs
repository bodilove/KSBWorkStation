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
    public partial class frmMutilTestConfig : Form
    {

        #region 变量
       
        
        #endregion


        #region 方法


        void Init()
        {
            //载入所有测试大步
            mTsTestStepList.Nodes.Clear();
            this.chkIsMutiEnable.Checked = Global.Prj.IsMutiTest;
            foreach (TestStep ts in Global.Prj.TestSteps)
            {
                bool b = true;
                foreach (TestThreadConfig tsc in Global.Prj.MyTestThreadConfigList)
                {
                    if (tsc.TestStepNameList.Contains(ts.Name))
                    {
                        b = false;
                        break;
                    }
                }
                if (b)
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = ts.Name;
                    tn.Text = ts.Name;
                    mTsTestStepList.Nodes.Add(tn);
                }
            }

            //载入所有测试线程
            mTsThreadList.Nodes.Clear();
            foreach (TestThreadConfig tsc in Global.Prj.MyTestThreadConfigList)
            {
                TreeNode tn = new TreeNode();
                tn.Name =tsc.Name;
                tn.Text = tsc.Name;
                foreach (string TestStepName in tsc.TestStepNameList)
                {
                    TreeNode tnn = new TreeNode();
                    tnn.Name = TestStepName;
                    tnn.Text = TestStepName;
                    tn.Nodes.Add(tnn);
                }
                mTsThreadList.Nodes.Add(tn);
            }
        }


        void Save()
        {
            Global.Prj.IsMutiTest = this.chkIsMutiEnable.Checked;
            Global.Prj.MyTestThreadConfigList.Clear();
            foreach (TreeNode tn in mTsThreadList.Nodes)
            {
                TestThreadConfig tsc = new TestThreadConfig();
                tsc.Name = tn.Text;
                tsc.ID = tn.Index + 1;
                foreach (TreeNode tnn in tn.Nodes)
                {
                    tsc.TestStepNameList.Add(tnn.Text);
                }
                Global.Prj.MyTestThreadConfigList.Add(tsc);
            }
        }
        #endregion


        public frmMutilTestConfig()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TreeNode tn = mTsThreadList.Nodes.Add("线程" + (mTsThreadList.Nodes.Count+1).ToString());
            tn.Name = tn.Text;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (mTsThreadList.SelectedNode != null)
            {
                TreeNode tnn = mTsThreadList.SelectedNode;
                mTsThreadList.Nodes.Remove(mTsThreadList.SelectedNode);
                foreach (TreeNode tn in tnn.Nodes)
                {
                    mTsTestStepList.Nodes.Add(tn);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void frmMutilTestConfig_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void mTsThreadList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (e.Node.Level == 0)
            //{
                
            //    e.Node.BeginEdit();
            //}
        }

        private void mTsThreadList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {

                mTsThreadList.LabelEdit = true;
            }
            else
            {
                mTsThreadList.LabelEdit = false;
            }
        }

        private void mTsTestStepList_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
         
        }

        private void mTsTestStepList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string s = "";
            foreach (TestStep st in Global.Prj.TestSteps)
            {
                if (e.Node.Text == st.Name)
                {
                    s = st.Description;
                    break;
                }
            }
            toolTip1.Show(s, mTsTestStepList, mTsTestStepList.PointToClient(Control.MousePosition).X + 20, mTsTestStepList.PointToClient(Control.MousePosition).Y);
        }

        private void mTsTestStepList_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(mTsTestStepList);
        }
    }
}
