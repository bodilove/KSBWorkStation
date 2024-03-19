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
    public partial class frmSelectVariable : Form
    {
        private string VarName = "";
        public frmSelectVariable()
        {
            InitializeComponent();
        }

        private void frmSelectTP_Load(object sender, EventArgs e)
        {
            tree.Nodes.Clear();

            TreeNode tp = tree.Nodes.Add("Test Pins");
            foreach (KeyValuePair<string, TestPin> p in Global.Prj.TestPins)
            {
                tp.Nodes.Add(p.Key).Tag = "TP/" + p.Key;
            }

            TreeNode var = tree.Nodes.Add("Variables");
            foreach (KeyValuePair<string, Variable> p in Global.Prj.Variables)
            {
                var.Nodes.Add(p.Key).Tag = "Var/" + p.Key;
            }

            TreeNode nodeTS = tree.Nodes.Add("Test Steps");
            foreach (TestStep ts in Global.Prj.TestSteps)
            {
                TreeNode nodeSubTS = nodeTS.Nodes.Add(ts.Name);
                nodeSubTS.Tag = "TS/" + ts.Name;
                nodeSubTS.Nodes.Add("(Result)").Tag = nodeSubTS.Tag + "/Result";

                foreach (SubTestStep subTS in ts.SubTest)
                {
                    TreeNode nodeSSTS = nodeSubTS.Nodes.Add(subTS.Name);
                    nodeSSTS.Tag = "SubTS/" + ts.Name + "/" + subTS.Name;
                    nodeSSTS.Nodes.Add("Value").Tag = nodeSSTS.Tag + "/Value";
                    nodeSSTS.Nodes.Add("Result").Tag = nodeSSTS.Tag + "/Result";
                }
            }

            TreeNode nodeFailure = tree.Nodes.Add("FailureStep");
            nodeFailure.Tag = "FailureStep";
            nodeFailure.Nodes.Add("Name").Tag = nodeFailure.Tag + "/Name";
            nodeFailure.Nodes.Add("Value").Tag = nodeFailure.Tag + "/Value";


            if (Global.Prj.TestPins.Count == 0 && Global.Prj.Variables.Count == 0)
            {
                btnOK.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TreeNode node = tree.SelectedNode;
            VarName = "[" + node.Tag + "]";
            this.Close();
        }

        public string GetTP(IWin32Window owern)
        {
            this.ShowDialog(owern);
            return VarName;
        }

        private void lstTP_DoubleClick(object sender, EventArgs e)
        {
            //if (lstTP.SelectedIndex >= 0)
            //{
            //    btnOK_Click(sender, e);
            //}
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnOK.Enabled = (e.Node.Tag!=null);
        }

    }
}
