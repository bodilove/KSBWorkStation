using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using Test.Common;


namespace Test.ProjectFileEditor
{
    public partial class frmLibrary : Form
    {
        private bool hasShown=false;

        public frmLibrary()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Global.Prj.Devices.Clear();
            for (int row = 0; row < DatDevice.Rows.Count; row++)
            {
                Device d = new Device();
                d.Name = DatDevice[1, row].Value.ToString();
                d.Class = DatDevice[2, row].Value.ToString();
                Global.Prj.Devices.Add(d.Name, d);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DatDevice.Rows.Add();
            DataGridViewRow r = DatDevice.Rows[DatDevice.Rows.Count - 1];
            r.Cells[0].Value = DatDevice.Rows.Count.ToString();
            r.Cells[1].Value ="Device" + DatDevice.Rows.Count.ToString();

            DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)r.Cells[2];
            foreach (string s in Global.Classes.Keys)
            {
                c2.Items.Add(s);
            }
            c2.Value = c2.Items[0];
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (DatDevice.Rows.Count==0) return;
            //Save selected row and col
            int row= DatDevice.SelectedCells[0].RowIndex;
            int col=DatDevice.SelectedCells[0].ColumnIndex;
            DatDevice.Rows.RemoveAt(row);

            for (int i = row; i < DatDevice.Rows.Count; i++)
            {
                DatDevice[0, i].Value = (i+1).ToString();
            }

            if (row >= 0 && row < DatDevice.Rows.Count)
            {
                DatDevice[col,row].Selected = true;
            }
        }

        private void frmLibrary_Shown(object sender, EventArgs e)
        {
            hasShown = true;
        }

        //private void DatDevice_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (!hasShown) { return; }
        //    DataGridViewCell C = ((DataGridView)sender).CurrentCell;
        //    int row = C.RowIndex;
        //    int col = C.ColumnIndex;
        //    switch (col)
        //    {
        //        case 2:
        //            DataGridViewComboBoxCell CobCell = (DataGridViewComboBoxCell)DatDevice[3, row];
        //            CobCell.Items.Clear();
        //            foreach (Type t in Global.Classes[C.Value.ToString()])
        //            {
        //                CobCell.Items.Add(t.FullName);
        //            }
        //            CobCell.Value = CobCell.Items[0];
        //            break;
        //        case 3:
        //            break;

        //    }
        //}

        private void frmLibrary_Load(object sender, EventArgs e)
        {
            int row = 0;
            DatDevice.Rows.Clear();
            foreach (KeyValuePair<string,Device> p in Global.Prj.Devices)
            {
                DataGridViewRow r ;
                DatDevice.Rows.Add();
                r = DatDevice.Rows[row];
                r.Cells[0].Value = (++row).ToString();
                r.Cells[1].Value = p.Key;

                DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)r.Cells[2];
                foreach (string s in Global.Classes.Keys)
                {
                    c2.Items.Add(s);
                }
                r.Cells[2].Value = p.Value.Class;
            }
        }

        private void DatDevice_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    foreach (DataGridViewRow r in DatDevice.Rows)
                    {
                        if (r.Index == e.RowIndex) { continue; }
                        if (e.FormattedValue.ToString() == r.Cells[1].Value.ToString())
                        {
                            MessageBox.Show("[" + e.FormattedValue + "] 已存在！", "错误",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            e.Cancel = true;
                            return;
                        }
                    }
                    if (e.FormattedValue.ToString() == "")
                    {
                        MessageBox.Show("设备名称不能为空！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        return;
                    }

                    char c = e.FormattedValue.ToString().ToCharArray()[0];
                    if (c >= '0' && c <= '9')
                    {
                        MessageBox.Show("设备名称不能以数字开头！", "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        return;
                    }
                    string src = DatDevice[e.ColumnIndex, e.RowIndex].Value.ToString();
                    string des = e.FormattedValue.ToString();
                    if (Global.Prj.Devices.ContainsKey(src))
                    {
                        if (src != des)
                        {
                            if (MessageBox.Show("确定要重命名该设备吗？", "提示",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Global.Prj.RenameDevice(src, des);
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
            }
        }
    }
}
