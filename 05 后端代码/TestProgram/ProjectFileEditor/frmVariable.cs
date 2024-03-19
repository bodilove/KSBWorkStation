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
    public partial class frmVariable : Form
    {
        public frmVariable()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int i = datVariables.Rows.Add();
            string s = (i + 1).ToString();
            datVariables.Rows[i].Cells[0].Value = s;
            datVariables.Rows[i].Cells[1].Value = "variable" + s;
            DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)datVariables[2, i];
            foreach (string t in Enum.GetNames(typeof(TypeCode)))
            {
                c2.Items.Add(t);
            }
            datVariables.Rows[i].Cells[2].Value = "Int32";
            datVariables.Rows[i].Cells[3].Value = 0;
            datVariables.Rows[i].Cells[4].Value = "Variable";

        }

        private void frmVariable_Load(object sender, EventArgs e)
        {
            datVariables.Rows.Clear();
            foreach (KeyValuePair<string, Variable> p in Global.Prj.Variables)
            {
                int row = datVariables.Rows.Add();
                datVariables[0, row].Value = row + 1;
                datVariables[1, row].Value = p.Value.Name;
                DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)datVariables[2, row];
                foreach (string t in Enum.GetNames(typeof(TypeCode)))
                {
                    c2.Items.Add(t);
                }

                datVariables[2, row].Value = p.Value.Type;
                datVariables[3, row].Value = p.Value.DefaultValue;
                datVariables[4, row].Value = p.Value.Description;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = datVariables.SelectedCells[0].RowIndex;
            if (index < 0) { return; }
            datVariables.Rows.RemoveAt(index);
            for (int i = index; i < datVariables.Rows.Count; i++)
            {
                datVariables[0, i].Value = (i + 1).ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.Prj.Variables.Clear();
            for (int row = 0; row < datVariables.Rows.Count; row++)
            {
                Variable v = new Variable();
                v.Name = datVariables[1, row].Value.ToString();
                v.Type = datVariables[2, row].Value.ToString();
                v.DefaultValue = datVariables[3, row].Value.ToString();
                v.Description = datVariables[4, row].Value.ToString();
                Global.Prj.Variables.Add(v.Name, v);
            }
            this.Close();


        }

        private void datVariables_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                foreach (DataGridViewRow r in datVariables.Rows)
                {
                    if (r.Index == e.RowIndex) { continue; }
                    if (e.FormattedValue.ToString() == r.Cells[1].Value.ToString())
                    {
                        MessageBox.Show("[" + e.FormattedValue + "] 已存在！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        datVariables.BeginEdit(false);
                        return;
                    }
                }
                if (e.FormattedValue.ToString() == "")
                {
                    MessageBox.Show("变量名称不能为空！", "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                    datVariables.BeginEdit(false);
                    return;
                }

                string tmp = e.FormattedValue.ToString();
                if (tmp.Contains(" ") || tmp.Contains("/"))
                {
                    MessageBox.Show("变量名称不能包含空格或者\"/\"字符！", "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                    datVariables.BeginEdit(false);
                    return;
                }

                string src = datVariables[e.ColumnIndex, e.RowIndex].Value.ToString();
                string des = e.FormattedValue.ToString();
                if (Global.Prj.Variables.Keys.Contains(src))
                {
                    if (src != des)
                    {
                        if (MessageBox.Show("确定要重命名该变量吗？", "提示",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Global.Prj.RenameParameter("[Var/" + src + "]", "[Var/" + des + "]");
                            //Global.Prj.TestPins.
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }

                    }
                }

            }
        }
    }
}
