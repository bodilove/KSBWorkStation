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
    public partial class frmTestPin : Form
    {
        int currentPin;
        bool Loading = false;

        public frmTestPin()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Loading = true;
            int i = datPins.Rows.Add();
            string s = (i + 1).ToString();
            datPins.Rows[i].Cells[0].Value = s;
            datPins.Rows[i].Cells[1].Value = "TP" + s;
            datPins.Rows[i].Cells[2].Value = s;
            datPins.Rows[i].Cells[3].Value = "Test Pin" + s;
            datPins.Rows[i].Cells[0].Tag = new Rectangle(0, 0, 20, 20);
            Loading = false;

            datPins.Rows[i].Cells[1].Selected = true;
            datPins.BeginEdit(false);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = datPins.SelectedCells[0].RowIndex;
            if (index < 0) { return; }
            datPins.Rows.RemoveAt(index);
            for (int i = index; i < datPins.Rows.Count; i++)
            {
                datPins[0, i].Value = (i + 1).ToString();
            }
            if (datPins.Rows.Count == 0)
            {
            //    panError.Clear();
            }
        }

        private void datPins_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (datPins.Rows.Count == 0)
            {
                btnRemove.Enabled = false;
            }
        }

        private void datPins_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (datPins.SelectedCells.Count == 0) { return; }

            int insertpos = datPins.SelectedCells[0].RowIndex;
            int col = datPins.SelectedCells[0].ColumnIndex;

            DataGridViewRow row = (DataGridViewRow)datPins.Rows[insertpos].Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Value = datPins.Rows[insertpos].Cells[i].Value;
            }
            datPins.Rows.Insert(insertpos - 1, row);
            datPins.Rows.RemoveAt(insertpos + 1);
            datPins[col, insertpos - 1].Selected = true;
            for (int i = 0; i < datPins.Rows.Count; i++)
            {
                datPins[0, i].Value = (i + 1).ToString();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (datPins.SelectedCells.Count == 0) { return; }

            int insertpos = datPins.SelectedCells[0].RowIndex;
            int col = datPins.SelectedCells[0].ColumnIndex;

            DataGridViewRow row = (DataGridViewRow)datPins.Rows[insertpos].Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Value = datPins.Rows[insertpos].Cells[i].Value;
            }
            datPins.Rows.Insert(insertpos + 2, row);
            datPins.Rows.RemoveAt(insertpos);
            datPins[col, insertpos + 1].Selected = true;
            for (int i = 0; i < datPins.Rows.Count; i++)
            {
                datPins[0, i].Value = (i + 1).ToString();
            }
        }

        private void datPins_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (Loading) { return; }
            currentPin = e.RowIndex;

            btnUp.Enabled = true;
            btnDown.Enabled = true;
            if (e.RowIndex == 0)
            {
                btnUp.Enabled = false;
            }
            if (e.RowIndex == datPins.Rows.Count - 1)
            {
                btnDown.Enabled = false;
            }
        }

        private void frmTestPin_Load(object sender, EventArgs e)
        {
            Loading = true;
            datPins.Rows.Clear();
            foreach (KeyValuePair<string, TestPin> p in Global.Prj.TestPins)
            {
                int row = datPins.Rows.Add();
                datPins[0, row].Value = row + 1;
                datPins[1, row].Value = p.Value.Name;
                datPins[2, row].Value = p.Value.Channel.ToString();
                datPins[3, row].Value = p.Value.Description;
            }
            Loading = false;
            if (datPins.Rows.Count > 0)
            {
                datPins[1, 0].Selected = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.Prj.TestPins.Clear();
            for (int row = 0; row < datPins.Rows.Count; row++)
            {
                TestPin tp = new TestPin();
                tp.Name = datPins[1, row].Value.ToString();
                tp.Channel = Convert.ToInt32(datPins[2, row].Value);
                if (datPins[3, row].Value != null)
                {
                    tp.Description = datPins[3, row].Value.ToString();
                }
                Global.Prj.TestPins.Add(tp.Name, tp);
            }
            this.Close();
        }

        private void datPins_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    foreach (DataGridViewRow r in datPins.Rows)
                    {
                        if (r.Index == e.RowIndex) { continue; }
                        if (e.FormattedValue.ToString() == r.Cells[1].Value.ToString())
                        {
                            MessageBox.Show("[" + e.FormattedValue + "] 已存在！", "错误",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            e.Cancel = true;
                            datPins.BeginEdit(false);
                            return;
                        }
                    }
                    if (e.FormattedValue.ToString() == "")
                    {
                        MessageBox.Show("引脚名称不能为空！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        datPins.BeginEdit(false);
                        return;
                    }

                    char c = e.FormattedValue.ToString().ToCharArray()[0];
                    if (c >= '0' && c <= '9')
                    {
                        MessageBox.Show("引脚名称不能以数字开头！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        datPins.BeginEdit(false);
                        return;
                    }

                    string tmp = e.FormattedValue.ToString();
                    if (tmp.Contains(" ")||tmp.Contains("/"))
                    {
                        MessageBox.Show("引脚名称不能包含空格或者\"/\"字符！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        datPins.BeginEdit(false);
                        return;
                    }

                    string src=datPins[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string des=e.FormattedValue.ToString();
                    if (Global.Prj.TestPins.Keys.Contains(src))
                    {
                        if (src != des)
                        {
                            if (MessageBox.Show("确定要重命名该引脚吗？", "提示",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Global.Prj.RenameParameter("[TP/" + src + "]", "[TP/" + des + "]");
                                //Global.Prj.TestPins.
                            }
                            else
                            {
                                e.Cancel = true;
                                return;
                            }

                        }
                    }
                  
                    break;
                case 2:
                    try
                    {
                        object o = Convert.ToInt32(e.FormattedValue);
                    }
                    catch (System.Exception )
                    {
                        MessageBox.Show("请输入数字!");
                        datPins.BeginEdit(false);
                        e.Cancel = true;
                    }
                    break;
            }
        }

    }
}
