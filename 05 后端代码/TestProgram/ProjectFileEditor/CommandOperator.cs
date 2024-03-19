using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Test.Common;
using System.Reflection;

namespace Test.ProjectFileEditor
{
    [Serializable]
    class CommandOperator
    {
        [NonSerialized]
        DataGridView _view;

        bool cutStatus = false;
        Command _cmd;
        DataGridViewRow _row;
        frmMain _frm;

        List<Command> _cmds = new List<Command>();
        List<DataGridViewRow> _rows = new List<DataGridViewRow>();
        List<int> OldNo = new List<int>();


        public List<Command> _CommandList = null;
        public int _CommandIndex = 0;

        public CommandOperator(frmMain frm, DataGridView view)
        {
            _frm = frm;
            _view = view;
        }

        public void AddCommand(List<Command> CurCmdList)
        {
            if (CurCmdList == null) { return; }

            Command c = new Command();
            c.Parameter = new Command.parameter[0];

            int insertPos = 0;
            if (_view.Rows.Count == 0)
            {
                insertPos = 0;
            }
            else
            {
                if (_view.SelectedCells.Count == 0)
                {
                    insertPos = _view.Rows.Count;
                }
                else
                {
                    insertPos = _view.SelectedCells[0].RowIndex + 1;
                }
            }
            CurCmdList.Insert(insertPos, c);
            _view.Rows.Insert(insertPos, 1);
            DataGridViewRow r = _view.Rows[insertPos];
            r.Cells[0].Value = (insertPos + 1).ToString();

            DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)r.Cells[1];
            foreach (KeyValuePair<string, Device> p in Global.Prj.Devices)
            {
                c1.Items.Add(p.Key);
            }

            if (c1.Items.Count > 0) { c1.Value = c1.Items[0].ToString(); }

            c1.Selected = true;
            //Change Nr. for the rest rows
            for (int i = insertPos; i < _view.Rows.Count; i++)
            {
                _view[0, i].Value = (i + 1).ToString();
            }
        }

        public void DeleteCommand(List<Command> CurCmdList)
        {
            try
            {
                int index = _view.SelectedCells[0].RowIndex;
                _view.Rows.RemoveAt(index);
                CurCmdList.RemoveAt(index);
                for (int i = index; i < _view.Rows.Count; i++)
                {
                    _view[0, i].Value = (i + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void MoveUp(List<Command> CurCmdList,int insertpos)
        {
            int col = _view.SelectedCells[0].ColumnIndex;

            _frm.Loading = true;
            Command c = (Command)CurCmdList[insertpos].Clone();
            if (insertpos == 0) return;//防止第一行继续上移报错
            CurCmdList.Insert(insertpos - 1, c);

            DataGridViewRow row = (DataGridViewRow)_view.Rows[insertpos].Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Value = _view.Rows[insertpos].Cells[i].Value;
            }
         
            _view.Rows.Insert( insertpos - 1, row);
            _view.Rows.RemoveAt(insertpos + 1);
            CurCmdList.RemoveAt(insertpos + 1);

            _frm.Loading = false;

            for (int i = 0; i < _view.Rows.Count; i++)
            {
                _view[0, i].Value = (i + 1).ToString();
                _view.Rows[i].Selected = false;
            }

            _view[col, insertpos - 1].Selected = true;
         

        }

        public void MoveDown(List<Command> CurCmdList, int insertpos)
        {

            int col = _view.SelectedCells[0].ColumnIndex;

            _frm.Loading = true;

            Command c = (Command)CurCmdList[insertpos].Clone();
            if (insertpos == _view.Rows.Count - 1) return;//防止最后一行继续下移报错
            CurCmdList.Insert(insertpos + 2, c);
            CurCmdList.RemoveAt(insertpos);

            DataGridViewRow row = (DataGridViewRow)_view.Rows[insertpos].Clone();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Value = _view.Rows[insertpos].Cells[i].Value;
            }
            _view.Rows.Insert(insertpos + 2, row);
            _view.Rows.RemoveAt(insertpos);

            _frm.Loading = false;
            
            for (int i = 0; i < _view.Rows.Count; i++)
            {
                _view[0, i].Value = (i + 1).ToString();
                _view.Rows[i].Selected = false;
            }

            _view[col, insertpos + 1].Selected = true;
        }

        public void CopyCommand(bool cut, List<Command> CurCmdList, int pos)
        {
            cutStatus = cut;

            Clipboard.Clear();
            _cmd = (Command)CurCmdList[pos].Clone();
            _CommandList = CurCmdList;
            _row = (DataGridViewRow)_view.Rows[pos].Clone();
            for (int i = 0; i < _row.Cells.Count; i++)
            {
                _row.Cells[i].Value = _view.Rows[pos].Cells[i].Value;
            }
            Clipboard.SetData("Command", _cmd);
        }

        public void PasteCommand(List<Command> CurCmdList)
        {
            Command cmd = (Command)Clipboard.GetData("Command");
            if (cmd == null)
            {
                MessageBox.Show("焦点错误");
                return;
            }//防止粘贴错误
            //CurCmdList.Add(cmd);
            int insertPos = 0;
            if (_view.Rows.Count == 0)
            {
                insertPos = 0;
            }
            else
            {
                if (_view.SelectedCells.Count == 0)
                {
                    insertPos = _view.Rows.Count;
                }
                else
                {
                    insertPos = _view.SelectedCells[0].RowIndex + 1;
                }
            }

            CurCmdList.Insert(insertPos, cmd);
            //int n = insertPos;
            //DataGridViewRow r = _view.Rows[n];

            //DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)r.Cells[1];
            //foreach (KeyValuePair<string, Device> p in Global.Prj.Devices)
            //{
            //    c1.Items.Add(p.Key);
            //}

            //Command.parameter[] par =(Command.parameter[]) cmd.Parameter.Clone();
            //string methodname = cmd.MethodName;

            //r.Cells[0].Value = (n + 1).ToString();
            //r.Cells[1].Value = cmd.DeviceName;
            //r.Cells[2].Value = methodname;

            //cmd.Parameter = par;
            //r.Cells[3].Value = cmd.ParameterName;
            //r.Cells[1].Selected = true;

            if (cutStatus)
            {
                _CommandList.Remove(_cmd);
                cutStatus = false;
            }

        }

        public void CopyCommands(bool cut, List<Command> CurCmdList, List<int> POSs)
        {
            OldNo = POSs;
            cutStatus = cut;

            Clipboard.Clear();
            _cmds.Clear();
            _rows.Clear();
            //List<Command> _cmds = new List<Command>();
            POSs.Reverse();
            foreach (int pos in POSs)
            {
                _cmds.Add((Command)CurCmdList[pos].Clone());
                _rows.Add((DataGridViewRow)_view.Rows[pos].Clone());
            }
            for (int k = 0; k < _rows.Count; k++)
            {
                for (int i = 0; i < _rows[k].Cells.Count; i++)
                {
                    _rows[k].Cells[i].Value = _view.Rows[k].Cells[i].Value;
                }
            }
            //_cmd = (Command)CurCmdList[pos].Clone();
            _CommandList = CurCmdList;
            //_row = (DataGridViewRow)_view.Rows[pos].Clone();

            //for (int i = 0; i < _row.Cells.Count; i++)
            //{
            //    _row.Cells[i].Value = _view.Rows[pos].Cells[i].Value;
            //}
            Clipboard.SetData("Command", _cmds);

        }


        public void PasteCommands(List<Command> CurCmdList)
        {
            List<Command> cmds = (List<Command>)Clipboard.GetData("Command");
            object t = (List<Command>)Clipboard.GetData("Command");
            if (cmds == null)
            {
                MessageBox.Show("焦点错误");
                return;
            }//防止粘贴错误
            //CurCmdList.Add(cmd);
            int insertPos = 0;
            if (_view.Rows.Count == 0)
            {
                insertPos = 0;
            }
            else
            {
                if (_view.SelectedCells.Count == 0)
                {
                    insertPos = _view.Rows.Count;
                }
                else
                {
                    insertPos = _view.SelectedCells[0].RowIndex + 1;
                }
            }

            //  CurCmdList.Insert(insertPos, cmd);
            for (int i = 0; i < _cmds.Count; i++)
            {
                CurCmdList.Insert(insertPos + i, cmds[i]);
            }


            if (cutStatus)
            {
                foreach (int j in OldNo)
                {
                    _CommandList.RemoveAt(j);
                }
                cutStatus = false;
            }

        }
    }
}
