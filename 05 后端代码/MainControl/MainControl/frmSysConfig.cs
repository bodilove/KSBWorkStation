using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.SysConfig.Model;
using System.IO;
using System.Net;
namespace MainControl
{
    public partial class frmSysConfig : Form
    {



        public SystemConfig CurrentConfig = null;
        DataGridView CurrentDgv = null;
        LocalStationConfig CurrenLocalStationConfig = null;

        public void NewConfig()
        {
            SystemConfig gf = new SystemConfig();

            CurrentConfig = gf;
            //CurrentConfig.Save(@"C:\ww\123.cfg");
        }


        public bool LoadConfig()
        {
            CurrentConfig = CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            if (CurrentConfig != null)
            {
                txtServerName.Text = CurrentConfig.DataSource;
                txtDBName.Text = CurrentConfig.InitialCatalog;
                txtUserID.Text = CurrentConfig.UserID;
                txtPassWord.Text = CurrentConfig.Password;
                dgvlocalStConfig.Rows.Clear();

                foreach (LocalStationConfig lst in CurrentConfig.localStlst)
                {
                    dgvlocalStConfig.Rows.Add(new object[] { lst.StationNum, lst.StationName });
                }
            }
            else
            {
                return false;
            }
            return true;
        }


        public bool SaveConfig()
        {
            CurrentConfig.DataSource = txtServerName.Text;
            CurrentConfig.InitialCatalog = txtDBName.Text;
            CurrentConfig.UserID = txtUserID.Text;
            CurrentConfig.Password = txtPassWord.Text;
            //   CurrentConfig.localStlst.Clear();
            //foreach (DataGridViewRow dr in dgvlocalStConfig.Rows)
            //{
            //    LocalStationConfig lst = new LocalStationConfig();
            //    lst.StationNum = dr.Cells[0].Value.ToString();
            //    lst.StationName = dr.Cells[1].Value.ToString();
            //    CurrentConfig.localStlst.Add(lst);
            //}
            return CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
        }


        public frmSysConfig()
        {
            InitializeComponent();
        }




        private void frmSysConfig_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(MdlClass.SysConfigPath))
            {
                //如果不包含
                DialogResult dr = MessageBox.Show("总体配置的文件夹不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Directory.CreateDirectory(MdlClass.SysConfigPath);
                    //创建文件
                    NewConfig();
                    CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                //载该文件

                if (File.Exists(MdlClass.SysConfigPath + @"\SysConfig.cfg"))
                {
                    CurrentConfig = new SystemConfig();
                    CurrentConfig = CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");

                    if (CurrentConfig == null)
                    {
                        DialogResult dr = MessageBox.Show("总体配置的文件载入失败，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            NewConfig();

                            CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    DialogResult dr = MessageBox.Show("总体配置的文件不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        NewConfig();

                        CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            LoadConfig();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!SaveConfig())
            {
                MessageBox.Show("保存失败！");
            }
            else
            {
                MessageBox.Show("保存成功，需要重启软件才能生效，请处理产线剩余事务，重启软件！");
                this.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Add()
        {
            if (CurrentDgv == dgvlocalStConfig)
            {

                LocalStationConfig lsc = new LocalStationConfig();
                lsc.StationName = "NULL";
                lsc.StationNum = "NULL";

                CurrentConfig.localStlst.Add(lsc);
                dgvlocalStConfig.Rows.Add(new object[] { "NULL", "NULL" });
            }
            if (CurrentDgv == dgvDevice)
            {
                if (CurrenLocalStationConfig != null)
                {
                    DeviceAddress dssc = new DeviceAddress();
                    dssc.Name = "NULL";
                    dssc.Address = "127.0.0.1";
                    dssc.Port = 5000;
                    string AddressAddPort = dssc.Address + ":" + dssc.Port;

                    DataGridViewComboBoxCell drCombox = new DataGridViewComboBoxCell();
                    drCombox.DataSource = Enum.GetNames(typeof(ConnectType));
                    drCombox.Value = ConnectType.TcpIp.ToString();
                    //  int x = dgvDevice.Rows.Add(new object[] { dar.Name, "", AddressAddPort });




                    if (dssc.ConnectType == ConnectType.TcpIp)
                    {
                        AddressAddPort = dssc.Address + ":" + dssc.Port;
                    }
                    CurrenLocalStationConfig.DeviceAddresslst.Add(dssc);
                    int x = dgvDevice.Rows.Add(new object[] { dssc.Name, "", AddressAddPort });
                    dgvDevice.Rows[x].Cells[1] = drCombox;
                }
            }
        }

        public void Delete()
        {
            if (CurrentDgv == dgvlocalStConfig)
            {
                if (dgvlocalStConfig.Rows.Count > 0 && dgvlocalStConfig.SelectedCells.Count > 0)
                {
                    dgvlocalStConfig.Rows.RemoveAt(dgvlocalStConfig.SelectedCells[0].RowIndex);
                    CurrentConfig.localStlst.RemoveAt(dgvlocalStConfig.SelectedCells[0].RowIndex);
                }
            }
            if (CurrentDgv == dgvDevice)
            {
                if (CurrenLocalStationConfig != null)
                {
                    if (dgvDevice.Rows.Count > 0 && dgvDevice.SelectedCells.Count > 0)
                    {
                        CurrenLocalStationConfig.DeviceAddresslst.RemoveAt(dgvDevice.SelectedCells[0].RowIndex);
                        dgvDevice.Rows.RemoveAt(dgvDevice.SelectedCells[0].RowIndex);
                    }
                }
            }
        }




        private void dgvlocalStConfig_Enter(object sender, EventArgs e)
        {
            CurrentDgv = dgvlocalStConfig;
        }

        private void dgvDevice_Enter(object sender, EventArgs e)
        {
            CurrentDgv = dgvDevice;
        }


        public void LoadDeviceAddress()
        {
            dgvDevice.Rows.Clear();
            foreach (DeviceAddress dar in CurrenLocalStationConfig.DeviceAddresslst)
            {
                string AddressAddPort = dar.Address + ":" + dar.Port;
                if (dar.ConnectType == ConnectType.TcpIp)
                {
                    AddressAddPort = dar.Address + ":" + dar.Port;
                }
                if (dar.ConnectType == ConnectType.Com)
                {
                    AddressAddPort = dar.Address;
                }
                // CurrenLocalStationConfig.DeviceAddresslst.Add(dar);




                DataGridViewComboBoxCell drCombox = new DataGridViewComboBoxCell();



                drCombox.DataSource = Enum.GetNames(typeof(ConnectType));
                drCombox.Value = dar.ConnectType.ToString();
                int x = dgvDevice.Rows.Add(new object[] { dar.Name, "", AddressAddPort });
                dgvDevice.Rows[x].Cells[1] = drCombox;
            }
        }

        private void dgvlocalStConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            CurrenLocalStationConfig = CurrentConfig.localStlst[rowIndex];
            LoadDeviceAddress();
        }

        private void dgvlocalStConfig_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (columnIndex == 0)
            {
                CurrentConfig.localStlst[rowIndex].StationNum = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            }
            else if (columnIndex == 1)
            {
                CurrentConfig.localStlst[rowIndex].StationName = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            }

        }

        private void dgvlocalStConfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            CurrenLocalStationConfig = CurrentConfig.localStlst[rowIndex];
            LoadDeviceAddress();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }



        private void dgvlocalStConfig_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CurrentDgv = dgvlocalStConfig;

                this.MenuStrip1.Show(Cursor.Position);

            }

        }

        private void dgvDevice_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CurrentDgv = dgvDevice;
                this.MenuStrip1.Show(Cursor.Position);
            }
        }

        private void dgvDevice_CellValidated(object sender, DataGridViewCellEventArgs e)
        {

            //  dgvDevice.Rows[rowIndex].Cells[columnIndex].Selected = true;
        }
        string OldValue = "";
        private void dgvDevice_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            try
            {
                if (columnIndex == 0)
                {
                    CurrenLocalStationConfig.DeviceAddresslst[rowIndex].Name = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                    //  CurrentConfig.localStlst[rowIndex].StationNum = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                }
                else if (columnIndex == 1)
                {
                    CurrenLocalStationConfig.DeviceAddresslst[rowIndex].ConnectType = (ConnectType)Enum.Parse(typeof(ConnectType), dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString());
                    // CurrentConfig.localStlst[rowIndex].StationName = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                }
                else if (columnIndex == 2)
                {
                    if (CurrenLocalStationConfig.DeviceAddresslst[rowIndex].ConnectType == ConnectType.Com)
                    {
                        if (dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString().ToUpper().StartsWith("COM"))
                        {
                            string[] s = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString().ToUpper().Split('M');
                            int comport = 0;
                            if (int.TryParse(s[1].Trim(), out comport) && comport > 0)
                            {
                                CurrenLocalStationConfig.DeviceAddresslst[rowIndex].Address = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString().ToUpper();
                            }
                            else
                            {
                                throw new Exception(@"输入COM口格式错误，正确格式:" + "COM1");
                            }
                        }
                        else
                        {
                            throw new Exception(@"输入COM口格式错误，正确格式:" + "COM1");
                        }

                    }
                    if (CurrenLocalStationConfig.DeviceAddresslst[rowIndex].ConnectType == ConnectType.TcpIp)
                    {

                        string[] s = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString().Split(':');
                        IPAddress ip = null;
                        int port = 0;
                        if (IPAddress.TryParse(s[0].Trim(), out ip))
                        {
                            if (int.TryParse(s[1].Trim(), out port) && port > 0)
                            {
                                CurrenLocalStationConfig.DeviceAddresslst[rowIndex].Address = s[0].Trim();
                                CurrenLocalStationConfig.DeviceAddresslst[rowIndex].Port = port;
                            }
                            else
                            {
                                throw new Exception(@"输入的IP有误，正确例子：" + @"192.168.0.1:5000");
                                // MessageBox.Show(@"输入的端口号有误，正确例子：" + @"192.168.0.1:5000");
                            }
                        }
                        else
                        {
                            throw new Exception(@"输入的IP有误，正确例子：" + @"192.168.0.1:5000");
                            // MessageBox.Show(@"输入的IP有误，正确例子：" + @"192.168.0.1:5000");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                dgvDevice.Rows[rowIndex].Cells[columnIndex].Value = OldValue;
            }
        }

        private void dgvDevice_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            OldValue = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString();
        }
    }
}
