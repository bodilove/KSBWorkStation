using Common.SysConfig.Model;
using MainControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MainControl
{
    public partial class FrmStationConfig : Form
    {
        //public SystemConfig CurrentConfig = null;
        DataGridView CurrentDgv = null;
        LocalStationConfig CurrenLocalStationConfig = null;

        public void NewConfig()
        {
            //SystemConfig gf = new SystemConfig();

            //CurrentConfig = gf;
          
        }

        /// <summary>
        /// 加载工位配置文件
        /// </summary>
        /// <returns></returns>
        public bool LoadConfig()
        {
            //CurrentConfig = CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");
            if (Program.CurrentConfig != null)
            {
                dgvlocalStConfig.Rows.Clear();

                foreach (LocalStationConfig lst in Program.CurrentConfig.localStlst)
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
            //CurrentConfig.DataSource = txtServerName.Text;
            //CurrentConfig.InitialCatalog = txtDBName.Text;
            //CurrentConfig.UserID = txtUserID.Text;
            //CurrentConfig.Password = txtPassWord.Text;
            ////Program.CurrentConfig.localStlst.Clear();
            ////foreach (DataGridViewRow dr in dgvlocalStConfig.Rows)
            ////{
            ////    LocalStationConfig lst = new LocalStationConfig();
            ////    lst.StationNum = dr.Cells[0].Value.ToString();
            ////    lst.StationName = dr.Cells[1].Value.ToString();
            ////    Program.CurrentConfig.localStlst.Add(lst);
            ////}
            ///
           
            return Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
        }

        public FrmStationConfig()
        {
           
            //this._frmUserInfo = frmUserInfo;
            InitializeComponent();
            this.Text = "工位管理";
            //type = OperationType.Add;
        }
       
        private void FrmStationConfig_Load(object sender, EventArgs e)
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
                    Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
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
                    SystemConfig CurrentConfig = new SystemConfig();
                    Program.CurrentConfig = CurrentConfig.Load(MdlClass.SysConfigPath + @"\SysConfig.cfg");

                    if (Program.CurrentConfig == null)
                    {
                        DialogResult dr = MessageBox.Show("总体配置的文件载入失败，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            NewConfig();

                            Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
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

                        Program.CurrentConfig.Save(MdlClass.SysConfigPath + @"\SysConfig.cfg");
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            LoadConfig();
        }
        #region 工位配置

        #region 右键功能
        /// <summary>
        /// 右键添加功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add();
        }
        /// <summary>
        /// 右键删除功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }
        /// <summary>
        /// 添加
        /// </summary>
        public void Add()
        {
            if (CurrentDgv == dgvlocalStConfig)
            {

                LocalStationConfig lsc = new LocalStationConfig();
                lsc.StationName = "NULL";
                lsc.StationNum = "NULL";
               
                Program.CurrentConfig.localStlst.Add(lsc);
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
        /// <summary>
        /// 删除
        /// </summary>

        public void Delete()
        {
            if (CurrentDgv == dgvlocalStConfig)
            {
                if (dgvlocalStConfig.Rows.Count > 0 && dgvlocalStConfig.SelectedCells.Count > 0)
                {
                    dgvlocalStConfig.Rows.RemoveAt(dgvlocalStConfig.SelectedCells[0].RowIndex);
                    Program.CurrentConfig.localStlst.RemoveAt(dgvlocalStConfig.SelectedCells[0].RowIndex);
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

        #endregion

        /// <summary>
        /// 工位-Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlocalStConfig_Enter(object sender, EventArgs e)
        {
            CurrentDgv = dgvlocalStConfig;
        }
        /// <summary>
        /// 设备-Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDevice_Enter(object sender, EventArgs e)
        {
            CurrentDgv = dgvDevice;
        }

        /// <summary>
        /// 加载设备列表
        /// </summary>
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

                //设备下拉选择
                DataGridViewComboBoxCell drCombox = new DataGridViewComboBoxCell();
                drCombox.DataSource = Enum.GetNames(typeof(ConnectType));
                drCombox.Value = dar.ConnectType.ToString();
                int x = dgvDevice.Rows.Add(new object[] { dar.Name, "", AddressAddPort });
                dgvDevice.Rows[x].Cells[1] = drCombox;
            }
        }

        /// <summary>
        /// 工位-点击内容单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlocalStConfig_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            CurrenLocalStationConfig = Program.CurrentConfig.localStlst[rowIndex];
            LoadDeviceAddress();
        }
        /// <summary>
        /// 工位-单元格改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlocalStConfig_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewCell cell = (DataGridViewCell)sender;
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (columnIndex == 0)
            {
                Program.CurrentConfig.localStlst[rowIndex].StationNum = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            }
            else if (columnIndex == 1)
            {
                Program.CurrentConfig.localStlst[rowIndex].StationName = dgvlocalStConfig.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            }

        }
        /// <summary>
        /// 工位-点击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlocalStConfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            CurrenLocalStationConfig = Program.CurrentConfig.localStlst[rowIndex];
            LoadDeviceAddress();
        }

        
        /// <summary>
        /// 工位鼠标-右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvlocalStConfig_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                CurrentDgv = dgvlocalStConfig;

                this.MenuStrip1.Show(Cursor.Position);

            }

        }

        /// <summary>
        /// 设备-鼠标右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 设备-单元格编辑结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 设备-单元格编辑开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDevice_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            OldValue = dgvDevice.Rows[rowIndex].Cells[columnIndex].Value.ToString();
        }


        #endregion



        #region 保存事件
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> vs = Program.CurrentConfig.localStlst.GroupBy(g => g.StationNum).Where(w => w.Count() > 1).Select(s => s.Key).ToList();
                if (Program.CurrentConfig.localStlst.GroupBy(o => o.StationNum).Any(g => g.Count() > 1))
                {
                    vs = Program.CurrentConfig.localStlst.GroupBy(g => g.StationNum).Where(w => w.Count() > 1).Select(s => s.Key).ToList();
                    MessageBox.Show($"保存失:工位序号:[{(string.Join(",", vs))}]有重复记录！");
                    return;
                }
               

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
            catch (Exception ex)
            {

                //System_Bll.WriteLogToDB(new Entity.Base_Log
                //{
                //    CreateUserID = FrmLogin.LoginUserID,
                //    CreateUserName = FrmLogin.loginUserName,
                //    LocalIP = FrmLogin.LocalIP,
                //    LogMessage = ex.Message,
                //    Type = "系统错误！",
                //    ClassName = typeof(FrmDBConfigEdit).ToString()
                //});
                MessageBox.Show(ex.Message);
            }
        }
       
        
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_concel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
