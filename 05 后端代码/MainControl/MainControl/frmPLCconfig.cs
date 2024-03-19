using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.IO;
using Common.PLCServer;
namespace MainControl
{
    public partial class frmPLCconfig : Form
    {
        public frmPLCconfig()
        {
            InitializeComponent();
        }
        public static readonly object lockCurrentEvent = new object();

       

        public GeneralConfig CurrentConfig = null;
        bool Islinkage = true;
        Dictionary<string, Type> DicClasses = new Dictionary<string, Type>();

        public PLCStation CurrentStation = null;
        public PLCHandle CurrentPLC = null;
        public PLCEvent CurrentEvent = null;
        List<TabPage> tabpagelst = new List<TabPage>();
        bool isEdit = false;
        /// <summary>
        /// 当前选择节点LEVEL
        /// </summary>
        public int SelectNodeLevel = -2;

        /// <summary>
        /// 当前选择节点下标
        /// </summary>
        public int SelectNodeIndex = -2;
        //public TreeNode SelectNode = null;


        #region 【公用方法】


        public string GetFullMethodName(MethodInfo method)
        {
            string sM = method.Name;
            string sPT = null;

            if (method.GetParameters().Length > 0)
            {
                sPT = " [";
                foreach (ParameterInfo p in method.GetParameters())
                {
                    sPT += p.ParameterType.Name + ",";
                }
                sPT = sPT.Remove(sPT.Length - 1);
                sPT += "]";
            }
            else
            {
                sPT = " []";
            }
            return sM + sPT;
        }


        public void GetClass(string AssemblyName, string ClassName)
        {
            DicClasses.Clear();
            string[] AsmFiles = Directory.GetFiles(Application.StartupPath);


            List<string> filenames = new List<string>();
            for (int i = 0; i < AsmFiles.Length; i++)
            {
                string filename = System.IO.Path.GetFileName(AsmFiles[i]);
                //if (filename == "ProjectTest.dll")
                if (filename == AssemblyName)
                {
                    filenames.Add(AsmFiles[i]);
                }
            }


            cmbDeviceName.Items.Clear();
            cmbDeviceName.Items.Add("");

            for (int i = 0; i < filenames.Count; i++)
            {
                Assembly A = Assembly.LoadFrom(filenames[i]);
                //Global.Libraries[i] = A;

                foreach (Type tp in A.GetTypes())
                {
                    if (tp.Name == ClassName )
                    {
                        DicClasses.Add(tp.FullName, tp);
                        cmbDeviceName.Items.Add(tp.FullName);
                    }
                }
            }
        }


        public void RefreshUI()
        {
            PLCStationtree.Nodes.Clear();
            TreeNode t = PLCStationtree.Nodes.Add(CurrentConfig.Name);
            foreach (PLCHandle p in CurrentConfig.plclst)
            {
                TreeNode t0 = new TreeNode();
                t0.Name = p.id.ToString();
                t0.Text = p.Name; ;
                t.Nodes.Add(t0);

                foreach (PLCStation pl in p.PLCStationlst)
                {
                    TreeNode t1 = new TreeNode();
                    t1.Text = pl.Name;
                    t1.Name = pl.id.ToString();
                    t0.Nodes.Add(t1);
                }
            }
        }




        /// <summary>
        /// 整理ID号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <returns></returns>
        public bool TidyID<T>(List<T> t1) where T : AGeneralCf
        {

            try
            {
                for (int i = 0; i < t1.Count; i++)
                {
                    t1[i].id = i;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到最后一个ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <returns></returns>
        public int GetLastID<T>(List<T> t1) where T : AGeneralCf
        {
            return t1.Count;
        }

        /// <summary>
        /// 全名查重，返回bool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <returns></returns>
        public bool CheckSameName<T>(List<T> t1) where T : AGeneralCf
        {
            bool b = t1.GroupBy(l => l.Name).Where(g => g.Count() > 1).Count() > 0;
            return b;
        }

        /// <summary>
        /// 新建的名称查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool NameCheckNewName<T>(List<T> t1, string Name) where T : AGeneralCf
        {
            bool res = false;
            T t = t1.Find(x => x.Name == Name);

            if (t != null)
            {
                res = false;
            }
            else
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 修改的名称查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool NameCheckModify<T>(List<T> t1, string Name, int currentIndex) where T : AGeneralCf
        {
            bool res = false;

            // List<T> aa = null;
            // int ddd = 0;
            // T t = null;
            //  aa = t1.FindAll(x => x.Name == Name);
            //ddd = t1.FindIndex(x => x.Name == Name);
            //t = t1.Find(x => x.Name == Name);

            //aa = new List<T>();
            //ddd = 0;
            //string s = t.Name;
            List<T> tttt = t1.FindAll(x => x.Name == Name);
            if (tttt.Count > 1)
            {
                res = false;
            }
            else if (tttt.Count == 1)
            {
                int intd = t1.FindIndex(x => x.Name == Name);
                if (intd == currentIndex)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
            else
            {
                res = true;
            }
            return res;


        }




        /// <summary>
        /// 新建一个初始配置
        /// </summary>
        public void NewConfig()
        {
            GeneralConfig gf = new GeneralConfig();
            PLCHandle p = new PLCHandle();
            p.Name = "PLC_1";
            p.id = 0;
            PLCStation pt = new PLCStation();
            pt.Name = "PLC_1_ST1";
            pt.id = 0;
            PLCEvent pe = new PLCEvent();
            pe.Name = "PLC_event1";
            pe.id = 0;
            //pt.ArrPLCEvent.Add(pe);
            //p.PLCStationlst.Add(pt);
            //gf.plclst.Add(p);
            CurrentConfig = gf;
            //CurrentConfig.Save(@"C:\ww\123.cfg");
        }

        public bool SaveConfig()
        {
            if (CurrentConfig != null)
            {
                return CurrentConfig.Save(MdlClass.PLCConfigPath + @"\GNConfig.cfg");
            }
            else
            {
                return false;
            }
        }

        public bool SaveAsConfig(string path)
        {
            if (CurrentConfig != null)
            {
                return CurrentConfig.Save(path);
            }
            else
            {
                return false;
            }
        }

        public void LoadConfig(string path)
        {
            //if (CurrentConfig != null)
            //{
            //    DialogResult dr = MessageBox.Show(".", "提示：", MessageBoxButtons.YesNo);
            //    CurrentConfig.Save(Path + @"\GNConfig.cfg");
            //}
            CurrentConfig.Load(path);
        }
        #endregion


        public MethodInfo[] MethodList(string ClassName)
        {
            Type typ = DicClasses[ClassName];
            MethodInfo[] method = null;
            MethodInfo[] totalm = typ.GetMethods();
            List<MethodInfo> lm = new List<MethodInfo>();
            foreach (MethodInfo m in totalm)
            {
                if (m.IsPublic && !m.IsVirtual && m.Name != "GetType")
                {
                    lm.Add(m);
                }
            }
            //对方法集合排序
            lm = (from l in lm orderby l.Name ascending select l).ToList();
            method = lm.ToArray();

            return method;
        }
        public void ShowPage(int index, string Name)
        {
            for (int i = 0; i < tabpagelst.Count; i++)
            {
                if (i != index)
                {
                    tabpagelst[i].Parent = null;
                }
                else
                {
                    tabpagelst[i].Parent = tabConfig;
                    //tabpagelst[i].Text = Name;
                }
            }
        }

        public void LoadCurrentNodeInfo(TreeNode node)
        {

            CurrentStation = null;
            CurrentPLC = null;
            CurrentEvent = null;

            switch (node.Level)
            {
                case 0:
                    RefreshConfig();

                    break;
                case 1:
                    RefreshPLCHandle(node);

                    break;
                case 2:
                    RefreshStation(node);
                    break;
                case 3:
                    //RefreshEvent(node);
                    break;
            }
        }


        public void RefreshConfig()
        {
            this.txtConfigName.Text = CurrentConfig.Name;
            this.chbIsConnectDB.Checked = CurrentConfig.IsConnectDB;
            this.chbIsConnectHeadStation.Checked = CurrentConfig.IsConnectHeadStation;
            this.chbIsConnectMES.Checked = CurrentConfig.IsConnectMESService;
            this.chbIsConnectPLC.Checked = CurrentConfig.IsConnectPLC;
        }

        public void RefreshPLCHandle(TreeNode node)
        {
            PLCHandle PLC = null;
            PLC = CurrentConfig.plclst[node.Index];
            CurrentPLC = PLC;
            this.txtPLCName.Text = PLC.Name;
            txtIP.Text = PLC.ip;
            txtPort.Text = PLC.port.ToString();
            chbPLCIsEnable.Checked = PLC.IsEnbale;
            cmbThreadNum.Text = PLC.ThreadNum.ToString();
            txtHeadBeatAdr.Text = PLC.HeatBeatAdr.ToString();
            this.cmbSlot.SelectedIndex = PLC.Slot;
            this.cmbRack.SelectedIndex = PLC.Rack;
            this.nupDBIndex.Value = PLC.DBIndex;
        }


        public void RefreshStation(TreeNode node)
        {
            PLCStation st = null;

            st = CurrentConfig.plclst[node.Parent.Index].PLCStationlst[node.Index];
            CurrentStation = st;
            #region [显示基础信息]

            this.txtStationName.Text = st.Name;
            this.chbStationIsEnable.Checked = st.IsEnbale;

            //this.txtPlcParaTypeAdr.Text = st.PlcParamType_StartAddress.ToString();
            //this.txtPlcParaLengthAdr.Text = st.PlcParamLength_StartAddress.ToString();
            this.txtPlcParaAdr.Text = st.PlcParamAndRes_StartAddress.ToString();
            //this.cmbPlcParaTypeByteCount.Text = st.PlcParamType_ByteCount.ToString();
            //this.cmbPlcParaLengthByteCount.Text = st.PlcParamLength_ByteCount.ToString();
            this.txtPlcParaByteCount.Text = st.PlcParamAndRes_ByteCount.ToString();

            //this.txtPcParaTypeAdr.Text = st.PcParamType_StartAddress.ToString();
            //this.txtPcParaLengthAdr.Text = st.PcParamLength_StartAddress.ToString();
            this.txtPcParaAdr.Text = st.PcParamAndRes_StartAddress.ToString();
            //this.cmbPcParaTypeByteCount.Text = st.PcParamType_ByteCount.ToString();
            //this.cmbPcParaLengthByteCount.Text = st.PcParamLength_ByteCount.ToString();
            this.txtPcParaByteCount.Text = st.PcParamAndRes_ByteCount.ToString();

            #endregion

            #region [显示报警信息]
            //this.txtWarnStartAdr.Text = st.WarningConfig.Warning_StartAddress.ToString();
            this.cmbWarnCount.Text = st.WarningConfig.WMessagelst.Count.ToString();
            this.dgWarningMessage.Rows.Clear();
            this.dgEMGWarningMessage.Rows.Clear();
            for (int i = 0; i < st.WarningConfig.WMessagelst.Count; i++)
            {
                WarningMessage wm = st.WarningConfig.WMessagelst[i];
                this.dgWarningMessage.Rows.Add(new object[] { i, wm.Name, wm.IsEnbale, wm.ShowMessage });
            }

            for (int i = 0; i < st.WarningConfig.EMGMessagelst.Count; i++)
            {
                WarningMessage wm = st.WarningConfig.EMGMessagelst[i];
                this.dgEMGWarningMessage.Rows.Add(new object[] { i, wm.Name, wm.IsEnbale, wm.ShowMessage });
            }

            #endregion

            #region [显示流程信息]
            //this.txtFlowStartAdr.Text = st.FlowConfig.Flow_StartAddress.ToString();
            this.cmbFlowCount.Text = st.FlowConfig.Processlst.Count.ToString();
            this.dgFlow.Rows.Clear();
            for (int i = 0; i < st.FlowConfig.Processlst.Count; i++)
            {
                SubProcess sp = st.FlowConfig.Processlst[i];
                this.dgFlow.Rows.Add(i, sp.Name, sp.IsEnbale, sp.ShowMessage);
            }

            #endregion



            #region [显示PLC网络事件]
            //this.txtEventStartAdr.Text = st.EventConfig.PLCEvent_StartAddress.ToString();
            //this.txtEventByteCount.Text = st.EventConfig.PLCEvent_ByteCount.ToString();
            int vadress = st.PLC2PCEventConfig.PLCEvent_StartAddress;


            treeEvent.Nodes.Clear();
            //this.cmbEventAdr.Items.Clear();

            for (int i = 0; i < st.PLC2PCEventConfig.Eventlst.Count; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Name = st.PLC2PCEventConfig.Eventlst[i].Name;
                tn.Text = st.PLC2PCEventConfig.Eventlst[i].Name;
                tn.Checked = st.PLC2PCEventConfig.Eventlst[i].IsEnbale;
                treeEvent.Nodes.Add(tn);
                //this.cmbEventAdr.Items.Add(vadress);
                //vadress++;
            }

            treeEvent.SelectedNode = treeEvent.Nodes[0];
            #endregion


            #region [显示PC网络事件]
            //this.txtEventStartAdr.Text = st.EventConfig.PLCEvent_StartAddress.ToString();
            //this.txtEventByteCount.Text = st.EventConfig.PLCEvent_ByteCount.ToString();
             vadress = st.PC2PLCEventConfig.PLCEvent_StartAddress;


            treeEventPC.Nodes.Clear();
            //this.cmbEventAdr.Items.Clear();

            for (int i = 0; i < st.PC2PLCEventConfig.Eventlst.Count; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Name = st.PC2PLCEventConfig.Eventlst[i].Name;
                tn.Text = st.PC2PLCEventConfig.Eventlst[i].Name;
                tn.Checked = st.PC2PLCEventConfig.Eventlst[i].IsEnbale;
                treeEventPC.Nodes.Add(tn);
                //this.cmbEventAdr.Items.Add(vadress);
                //vadress++;
            }

            treeEventPC.SelectedNode = treeEventPC.Nodes[0];
            #endregion

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            cmbRack.Items.Clear();
            cmbSlot.Items.Clear();
            for (int i = 0; i < 101; i++)
            {
                cmbRack.Items.Add(i);
                cmbSlot.Items.Add(i);
            }

            ///插入方法库的名称

            GetClass("MainControl.exe", "frmMain");

            dgWarningMessage.ReadOnly = false;
            dgWarningMessage.Columns[0].ReadOnly = true;
            dgWarningMessage.Columns[1].ReadOnly = false;
            dgWarningMessage.Columns[2].ReadOnly = false;
            dgWarningMessage.Columns[3].ReadOnly = false;


            dgEMGWarningMessage.ReadOnly = false;
            dgEMGWarningMessage.Columns[0].ReadOnly = true;
            dgEMGWarningMessage.Columns[1].ReadOnly = false;
            dgEMGWarningMessage.Columns[2].ReadOnly = false;
            dgEMGWarningMessage.Columns[3].ReadOnly = false;


            dgFlow.ReadOnly = false;
            dgFlow.Columns[0].ReadOnly = true;
            dgFlow.Columns[1].ReadOnly = false;
            dgFlow.Columns[2].ReadOnly = false;
            dgFlow.Columns[3].ReadOnly = false;
            //dgPara.ReadOnly = false;
            //dgFlow.Columns[0].ReadOnly = true;
            //dgFlow.Columns[1].ReadOnly = true;
            //dgFlow.Columns[2].ReadOnly = false;

            //demoTest();
            //检查是否包含该文件夹
            if (!Directory.Exists(MdlClass.PLCConfigPath))
            {
                //如果不包含
                DialogResult dr = MessageBox.Show("总体配置的文件夹不存在，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Directory.CreateDirectory(MdlClass.PLCConfigPath);
                    //创建文件
                    NewConfig();
                    CurrentConfig.Save(MdlClass.PLCConfigPath + @"\GNConfig.cfg");
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                //载该文件

                if (File.Exists(MdlClass.PLCConfigPath + @"\GNConfig.cfg"))
                {
                    CurrentConfig = new GeneralConfig();
                    CurrentConfig = CurrentConfig.Load(MdlClass.PLCConfigPath + @"\GNConfig.cfg");

                    if (CurrentConfig == null)
                    {
                        DialogResult dr = MessageBox.Show("总体配置的文件载入失败，点击确定后自动创建该文件夹和初始化的配置,点击否退出程序.", "提示：", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            NewConfig();

                            CurrentConfig.Save(MdlClass.PLCConfigPath + @"\GNConfig.cfg");
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

                        CurrentConfig.Save(MdlClass.PLCConfigPath + @"\GNConfig.cfg");
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            //CheckSameName();
            //demoTest();
            tabpagelst.Clear();
            for (int i = 0; i < tabConfig.TabPages.Count; i++)
            {
                tabpagelst.Add(tabConfig.TabPages[i]);
            }
            RefreshUI();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("配置文件将被保存，会覆盖路径"
                + MdlClass.PLCConfigPath + @"\GNConfig.cfg，如果确定覆盖，点击“是”按钮，如果不覆盖，点击取消。", "提示：", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {

                if (SaveConfig())
                {
                    MessageBox.Show("保存成功。");
                }
                else
                {
                    MessageBox.Show("保存失败。");
                }
            }
        }

        private void PLCStationtree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentStation = null;
            ShowPage(e.Node.Level, e.Node.Text);
            LoadCurrentNodeInfo(e.Node);
        }

        private void PLCStationtree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.PLCStationtree.SelectedNode = e.Node;

            Application.DoEvents();
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                switch (e.Node.Level)
                {
                    case 0:
                        NodecontextMenuStrip.Items[0].Visible = true;
                        NodecontextMenuStrip.Items[0].Text = "添加PLC";
                        NodecontextMenuStrip.Items[1].Visible = false;

                        NodecontextMenuStrip.Items[2].Visible = false;

                        NodecontextMenuStrip.Items[3].Visible = false;
                        NodecontextMenuStrip.Items[4].Visible = false;

                        NodecontextMenuStrip.Items[5].Visible = false;


                        NodecontextMenuStrip.Items[6].Visible = false;
                        NodecontextMenuStrip.Items[7].Visible = false;
                        NodecontextMenuStrip.Show(PLCStationtree, e.X, e.Y);
                        break;
                    case 1:
                        NodecontextMenuStrip.Items[0].Visible = true;
                        NodecontextMenuStrip.Items[0].Text = "插入PLC";
                        NodecontextMenuStrip.Items[1].Visible = true;

                        NodecontextMenuStrip.Items[2].Visible = true;

                        NodecontextMenuStrip.Items[3].Visible = true;
                        NodecontextMenuStrip.Items[3].Text = "添加工作站";
                        NodecontextMenuStrip.Items[4].Visible = false;

                        NodecontextMenuStrip.Items[5].Visible = false;

                        NodecontextMenuStrip.Items[6].Visible = false;
                        NodecontextMenuStrip.Items[7].Visible = false;

                        NodecontextMenuStrip.Show(PLCStationtree, e.X, e.Y);
                        break;
                    case 2:
                        NodecontextMenuStrip.Items[0].Visible = false;
                        NodecontextMenuStrip.Items[1].Visible = false;

                        NodecontextMenuStrip.Items[2].Visible = false;

                        NodecontextMenuStrip.Items[3].Visible = true;
                        NodecontextMenuStrip.Items[3].Text = "插入工作站";
                        NodecontextMenuStrip.Items[4].Visible = true;

                        NodecontextMenuStrip.Items[5].Visible = true;

                        NodecontextMenuStrip.Items[6].Visible = false;
                        //NodecontextMenuStrip.Items[6].Text = "添加触发事件";
                        NodecontextMenuStrip.Items[7].Visible = false;
                        NodecontextMenuStrip.Show(PLCStationtree, e.X, e.Y);
                        break;
                    case 3:
                        //NodecontextMenuStrip.Items[0].Visible = false;
                        //NodecontextMenuStrip.Items[1].Visible = false;

                        //NodecontextMenuStrip.Items[2].Visible = false;

                        //NodecontextMenuStrip.Items[3].Visible = false;
                        //NodecontextMenuStrip.Items[4].Visible = false;

                        //NodecontextMenuStrip.Items[5].Visible = false;

                        //NodecontextMenuStrip.Items[6].Visible = true;
                        //NodecontextMenuStrip.Items[6].Text = "插入触发事件";
                        //NodecontextMenuStrip.Items[7].Visible = true;
                        //NodecontextMenuStrip.Show(PLCStationtree, e.X, e.Y);
                        break;
                }
            }
        }



        /// <summary>
        /// 添加一个PLC
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PLCHandle AddPLC(int index)
        {
            int id = GetLastID(CurrentConfig.plclst);
            PLCHandle PLC = new PLCHandle();
            PLC.Name = "PLC_" + id; ;
            do
            {
                if (NameCheckNewName(CurrentConfig.plclst, PLC.Name))
                {
                    break;
                }
                else
                {
                    id = id + 1;
                    PLC.Name = "PLC_" + id;
                }
            } while (true);
            PLC.ThreadNum = 0;




            CurrentConfig.plclst.Insert(index, PLC);
            TidyID(CurrentConfig.plclst);
            return PLC;
        }

        /// 删除一个PLC
        /// </summary>
        /// <param name="index"></param>
        public void DeletePLCHandle(int index)
        {
            CurrentConfig.plclst.RemoveAt(index);
            TidyID(CurrentConfig.plclst);
        }

        /// <summary>
        /// 增加一个工作站
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public PLCStation AddStation(PLCHandle plc, int index)
        {
            int id = GetLastID(plc.PLCStationlst);
            PLCStation st = new PLCStation();
            st.Name = "ST_" + id;
            do
            {
                if (NameCheckNewName(plc.PLCStationlst, st.Name))
                {
                    break;
                }
                else
                {
                    id = id + 1;
                    st.Name = "ST_" + id;
                }
            } while (true);

            #region[添加报警信息]
            //一个Byte代表8个报警
            //int MAXwarningCount = st.WarningConfig.Warning_ByteCount * 8;

            //for (int i = 0; i < 256; i++)
            //{
            //    WarningMessage wm = new WarningMessage();
            //    wm.id = i;
            //    wm.IsEnbale = false;
            //    wm.ShowMessage = "未知报警" + wm.id;
            //    wm.Name = "0.0";
            //    st.WarningConfig.WMessagelst.Add(wm);
            //}

            for (int i = 0; i < 16; i++)
            {
                WarningMessage wm = new WarningMessage();
                wm.id = i;
                wm.IsEnbale = false;
                wm.ShowMessage = "未知急停报警" + wm.id;
                wm.Name = "0.0";
                st.WarningConfig.EMGMessagelst.Add(wm);
            }


            for (int i = 0; i < 32; i++)
            {
                WarningMessage wm = new WarningMessage();
                wm.id = i;
                wm.IsEnbale = false;
                wm.ShowMessage = "未知报警" + wm.id;
                wm.Name = "0.0";
                st.WarningConfig.WMessagelst.Add(wm);
            }
            #endregion



            #region[添加流程显示信息]

            for (int i = 0; i < 1024; i++)
            {
                SubProcess sp = new SubProcess();
                sp.id = i;
                sp.Name = "未知流程_" + sp.id;
                sp.ShowMessage = "未知流程的显示信息。。";
                sp.IsEnbale = false;
                st.FlowConfig.Processlst.Add(sp);
            }
            #endregion



            #region[添加PLC网络事件信息]
            //int MAXEventCount = st.EventConfig.PLCEvent_ByteCount;

            for (int i = 0; i < 255; i++)
            {
                PLCEvent et = new PLCEvent();
                et.id = i;
                et.Name = "PLC事件_" + et.id;
                et.Mycommand = new PLCCommand();
                et.Mycommand.id = 0;
                et.Mycommand.MethodName = "";
                et.IsEnbale = false;
                et.Sender = SenderType.PLC;

                et.PrType = PLCPCPrType.NULL;
                et.ResultValueType = PLCPCPrType.NULL;
                et.PrLength = 0;
                et.ResultValueLength = 0;
                et.EventDescription = "PLC懒货，写个描述吧，要不大家都不清楚呢。。。";

                st.PLC2PCEventConfig.Eventlst.Add(et);
            }

            #endregion


            #region[添加PC网络事件信息]
            //int MAXEventCount = st.EventConfig.PLCEvent_ByteCount;

            for (int i = 0; i < 255; i++)
            {
                PLCEvent et = new PLCEvent();
                et.id = i;
                et.Name = "PC事件_" + et.id;
                et.Mycommand = new PLCCommand();
                et.Mycommand.id = 0;
                et.Mycommand.MethodName = "";
                et.IsEnbale = false;
                et.Sender = SenderType.PC;

                et.PrType = PLCPCPrType.NULL;
                et.ResultValueType = PLCPCPrType.NULL;
                et.PrLength = 0;
                et.ResultValueLength = 0;
                et.EventDescription = "PC懒货，写个描述吧，要不大家都不清楚呢。。。";

                st.PC2PLCEventConfig.Eventlst.Add(et);
            }

            #endregion

            plc.PLCStationlst.Insert(index, st);
            TidyID(plc.PLCStationlst);
            return st;
        }
        /// <summary>
        /// 删除一个工作站
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="index"></param>
        public void DeleteStation(PLCHandle plc, int index)
        {
            plc.PLCStationlst.RemoveAt(index);
            TidyID(plc.PLCStationlst);
        }


        private void 添加PLCToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.PLCStationtree.SelectedNode.Level == 0)
            {
                PLCHandle p = AddPLC(this.PLCStationtree.SelectedNode.Nodes.Count);
                TreeNode n = new TreeNode();
                n.Name = p.Name;
                n.Text = p.Name;
                this.PLCStationtree.SelectedNode.Nodes.Add(n);
                this.PLCStationtree.SelectedNode = n;
                return;
            }
            if (this.PLCStationtree.SelectedNode.Level == 1)
            {
                PLCHandle p = AddPLC(this.PLCStationtree.SelectedNode.Index + 1);

                TreeNode n = new TreeNode();
                n.Name = p.Name;
                n.Text = p.Name;
                this.PLCStationtree.SelectedNode.Parent.Nodes.Insert(this.PLCStationtree.SelectedNode.Index + 1, n);
                this.PLCStationtree.SelectedNode = n;
                return;
            }
        }

        private void 删除PLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletePLCHandle(this.PLCStationtree.SelectedNode.Index);
            this.PLCStationtree.SelectedNode.Parent.Nodes.RemoveAt(this.PLCStationtree.SelectedNode.Index);
        }

        private void 添加工作站ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.PLCStationtree.SelectedNode.Level == 1)//添加到末尾
            {
                PLCStation st = AddStation(CurrentConfig.plclst[this.PLCStationtree.SelectedNode.Index], this.PLCStationtree.SelectedNode.Nodes.Count);


                TreeNode n = new TreeNode();
                n.Name = st.Name;
                n.Text = st.Name;
                this.PLCStationtree.SelectedNode.Nodes.Add(n);
                this.PLCStationtree.SelectedNode = n;
                return;
            }
            if (this.PLCStationtree.SelectedNode.Level == 2)//zai
            {
                PLCStation st = AddStation(CurrentConfig.plclst[this.PLCStationtree.SelectedNode.Parent.Index], this.PLCStationtree.SelectedNode.Index + 1);


                TreeNode n = new TreeNode();
                n.Name = st.Name;
                n.Text = st.Name;
                this.PLCStationtree.SelectedNode.Parent.Nodes.Insert(this.PLCStationtree.SelectedNode.Index + 1, n);
                this.PLCStationtree.SelectedNode = n;
                return;
            }
        }

        private void 删除工作站ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteStation(CurrentConfig.plclst[this.PLCStationtree.SelectedNode.Parent.Index], this.PLCStationtree.SelectedNode.Index);
            this.PLCStationtree.SelectedNode.Parent.Nodes.RemoveAt(this.PLCStationtree.SelectedNode.Index);
        }

        private void rtxtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 添加触发事件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cmbThreadNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PLCStationtree.SelectedNode != null && PLCStationtree.SelectedNode.Level == 1)
            {

                //TreeNode tnnn = this.PLCStationtree.SelectedNode;
                int stnum = int.Parse(cmbThreadNum.Text);
                //if (stnum > CurrentPLC.ThreadNum)
                //{
                //    for (int i = CurrentPLC.ThreadNum; i < stnum; i++)
                //    {
                //        PLCStation st = AddStation(CurrentConfig.plclst[tnnn.Index], tnnn.Nodes.Count);
                //        TreeNode n = new TreeNode();
                //        n.Name = st.Name;
                //        n.Text = st.Name;
                //        tnnn.Nodes.Add(n);
                //        //this.PLCStationtree.SelectedNode = n;
                //    }
                    CurrentPLC.ThreadNum = stnum;
                //    return;
                //}
                //else
                //{
                //    for (int i = CurrentPLC.ThreadNum; i > stnum; i--)
                //    {
                //        DeleteStation(CurrentPLC, (CurrentPLC.PLCStationlst.Count - 1));
                //        tnnn.Nodes.RemoveAt(tnnn.Nodes.Count - 1);
                //    }
                //    CurrentPLC.ThreadNum = stnum;
                //    return;
                //}

            }
        }

        private void txtConfigName_Validated(object sender, EventArgs e)
        {

        }



        //把修改的基础信息写入
        private void chbIsConnectDB_Validated(object sender, EventArgs e)
        {
            if (CurrentConfig != null)
            {
                CurrentConfig.IsConnectDB = this.chbIsConnectDB.Checked;
                CurrentConfig.IsConnectHeadStation = this.chbIsConnectHeadStation.Checked;
                CurrentConfig.IsConnectMESService = this.chbIsConnectMES.Checked;
                CurrentConfig.IsConnectPLC = this.chbIsConnectPLC.Checked;
            }
        }

        private void txtIP_Validated(object sender, EventArgs e)
        {
            try
            {
                if (CurrentPLC != null)
                {
                    if (PLCStationtree.SelectedNode.Level == 1)
                    {
                        CurrentPLC.ip = txtIP.Text;
                        CurrentPLC.port = int.Parse(txtPort.Text);
                        CurrentPLC.IsEnbale = chbPLCIsEnable.Checked;
                        CurrentPLC.ThreadNum = int.Parse(cmbThreadNum.Text);
                        CurrentPLC.HeatBeatAdr = int.Parse(txtHeadBeatAdr.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPLCName_Validated(object sender, EventArgs e)
        {
            if (CurrentPLC != null)
            {
                if (PLCStationtree.SelectedNode.Level == 1)
                {
                    //查重
                    if (NameCheckModify(this.CurrentConfig.plclst, txtPLCName.Text, PLCStationtree.SelectedNode.Index))
                    {
                        this.CurrentPLC.Name = txtPLCName.Text;
                        PLCStationtree.SelectedNode.Name = this.CurrentPLC.Name;
                        PLCStationtree.SelectedNode.Text = this.CurrentPLC.Name;
                    }
                    else
                    {
                        txtPLCName.Text = this.CurrentPLC.Name;
                        MessageBox.Show("'" + txtPLCName.Text + "'" + "已存在。");
                    }
                }


            }
        }

        private void cmbWarnByteCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CurrentStation != null)
                {
                    if (PLCStationtree.SelectedNode.Level == 2)
                    {
                        TreeNode tnnn = this.PLCStationtree.SelectedNode;
                        int stnum = int.Parse(cmbWarnCount.Text);
                        int ConfigNum = CurrentStation.WarningConfig.WMessagelst.Count;
                        if (stnum > ConfigNum)
                        {
                            for (int i = ConfigNum; i < stnum; i++)
                            {
                                WarningMessage wm = new WarningMessage();
                                wm.id = i;
                                wm.IsEnbale = false;
                                wm.ShowMessage = "未知报警" + wm.id;
                                wm.Name = "0.0";
                                CurrentStation.WarningConfig.WMessagelst.Add(wm);
                                this.dgWarningMessage.Rows.Add(new object[] { i, wm.Name, wm.IsEnbale, wm.ShowMessage });
                            }
                            return;
                        }
                        else
                        {
                            for (int i = ConfigNum; i > stnum; i--)
                            {
                                CurrentStation.WarningConfig.WMessagelst.RemoveAt(CurrentStation.WarningConfig.WMessagelst.Count - 1);
                                this.dgWarningMessage.Rows.RemoveAt(this.dgWarningMessage.Rows.Count - 1);
                            }
                            return;
                        }
                    }
                }
            }
            catch
            { }
        }

        private void cmbFlowByteCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CurrentStation != null)
                {
                    //if (PLCStationtree.SelectedNode.Level == 2)
                    //{
                    //    CurrentStation.FlowConfig.FlowNCounts = int.Parse(this.cmbFlowCount.Text);
                    //}

                    TreeNode tnnn = this.PLCStationtree.SelectedNode;
                    int stnum = int.Parse(cmbFlowCount.Text);
                    int ConfigNum = CurrentStation.FlowConfig.Processlst.Count;
                    if (stnum > ConfigNum)
                    {
                        for (int i = ConfigNum; i < stnum; i++)
                        {
                            SubProcess sp = new SubProcess();
                            sp.id = i;
                            sp.Name = "未知流程_" + sp.id;
                            sp.ShowMessage = "未知流程的显示信息。。";
                            sp.IsEnbale = false;
                            CurrentStation.FlowConfig.Processlst.Add(sp); ;
                            this.dgFlow.Rows.Add(i, sp.Name, sp.IsEnbale, sp.ShowMessage);
                        }
                        return;
                    }
                    else
                    {
                        for (int i = ConfigNum; i > stnum; i--)
                        {
                            CurrentStation.FlowConfig.Processlst.RemoveAt(CurrentStation.FlowConfig.Processlst.Count - 1);
                            this.dgFlow.Rows.RemoveAt(this.dgFlow.Rows.Count - 1);
                        }

                        return;
                    }
                }
            }
            catch
            { }
        }

        private void dgWarningMessage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentStation != null)
            {
                if (PLCStationtree.SelectedNode.Level == 2)
                {
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            //点位的名称
                            CurrentStation.WarningConfig.WMessagelst[e.RowIndex].Name = dgWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                            break;
                        case 2:
                            //该报警是否启用
                            CurrentStation.WarningConfig.WMessagelst[e.RowIndex].IsEnbale = (bool)dgWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            break;
                        case 3:
                            //报警信息
                            CurrentStation.WarningConfig.WMessagelst[e.RowIndex].ShowMessage = dgWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                            break;
                    }
                }
            }
        }

        private void dgFlow_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentStation != null)
            {
                if (PLCStationtree.SelectedNode.Level == 2)
                {
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            //点位的名称
                            CurrentStation.FlowConfig.Processlst[e.RowIndex].Name = dgFlow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                            break;
                        case 2:
                            //该报警是否启用
                            CurrentStation.FlowConfig.Processlst[e.RowIndex].IsEnbale = (bool)dgFlow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            break;
                        case 3:
                            //报警信息
                            CurrentStation.FlowConfig.Processlst[e.RowIndex].ShowMessage = dgFlow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                            break;
                    }
                }
            }
        }

        private void txtPlcParaAdr_Validated(object sender, EventArgs e)
        {
            if (CurrentStation != null)
            {
                if (PLCStationtree.SelectedNode.Level == 2)
                {
                    CurrentStation.IsEnbale = this.chbStationIsEnable.Checked;
                    CurrentStation.PlcParamAndRes_StartAddress = int.Parse(this.txtPlcParaAdr.Text);
                    CurrentStation.PlcParamAndRes_ByteCount = int.Parse(this.txtPlcParaByteCount.Text);
                    CurrentStation.PcParamAndRes_StartAddress = int.Parse(this.txtPcParaAdr.Text);
                    CurrentStation.PcParamAndRes_ByteCount = int.Parse(this.txtPcParaByteCount.Text);
                }
            }
        }

        private void txtStationName_Validated(object sender, EventArgs e)
        {
            if (CurrentStation != null)
            {
                if (PLCStationtree.SelectedNode.Level == 2)
                {
                    //查重
                    if (NameCheckModify(this.CurrentConfig.plclst[PLCStationtree.SelectedNode.Parent.Index].PLCStationlst, txtStationName.Text, PLCStationtree.SelectedNode.Index))
                    {
                        this.CurrentStation.Name = txtStationName.Text;
                        PLCStationtree.SelectedNode.Name = this.CurrentStation.Name;
                        PLCStationtree.SelectedNode.Text = this.CurrentStation.Name;
                    }
                    else
                    {
                        txtStationName.Text = this.CurrentStation.Name;
                        MessageBox.Show("'" + txtStationName.Text + "'" + "已存在。");
                    }
                }


            }
        }

        private void treeEvent_AfterSelect(object sender, TreeViewEventArgs e)
        {
            do
            {
                if (!isEdit)
                {
                    break;
                }
                System.Threading.Thread.Sleep(1);
            } while (true);


            lock (lockCurrentEvent)
            {
                CurrentEvent = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index];
            }
            this.txtEventName.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].Name;
            this.rtxtDescription.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].EventDescription;
            this.txtEventIndex.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].id.ToString();
            //this.cmbSenderType.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].Sender.ToString();
            this.cmbPrType.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].PrType.ToString();
            this.cmbResValueType.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].ResultValueType.ToString();
            this.txtPrLength.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].PrLength.ToString();
            this.txtResValueLength.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].ResultValueLength.ToString();


            //合法性检查
            //检查是否当前类包含该DeviceName

            //dgPara.Rows.Clear();
            cmbDeviceName.Items.Clear();
            cmbDeviceName.Items.Add("");
            cmbMethodName.Items.Clear();
            cmbMethodName.Items.Add("");
            if (!DicClasses.ContainsKey(CurrentEvent.Mycommand.DeviceName))
            {

                Islinkage = false;
                cmbDeviceName.Text = "";
            }
            else
            {
                cmbDeviceName.Items.Add(CurrentEvent.Mycommand.DeviceName);
                Islinkage = false;
                cmbDeviceName.Text = CurrentEvent.Mycommand.DeviceName;

                cmbMethodName.Items.Clear();
                cmbMethodName.Items.Add("");
                ////检查是否包含
                List<MethodInfo> m = MethodList(cmbDeviceName.Text).ToList();
                MethodInfo mf = m.Find(x => GetFullMethodName(x) == CurrentEvent.Mycommand.MethodName);
                if (mf != null)
                {
                    cmbMethodName.Items.Add(CurrentEvent.Mycommand.MethodName);
                    Islinkage = false;
                    cmbMethodName.Text = CurrentEvent.Mycommand.MethodName;


                    //ParameterInfo[] Pars = mf.GetParameters();
                    //for (int i = 0; i < Pars.Length; i++)
                    //{
                    //    string s = "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name;
                    //    if (s == CurrentEvent.Mycommand.Parameters[i].Type)
                    //    {
                    //        dgPara.Rows.Add(new object[] { i, CurrentEvent.Mycommand.Parameters[i].Type, CurrentEvent.Mycommand.Parameters[i].Value });
                    //        CurrentEvent.Mycommand.Parameters[i].id = i;
                    //    }
                    //    else
                    //    {
                    //        dgPara.Rows.Add(new object[] { i, "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name, "" });
                    //        CurrentEvent.Mycommand.Parameters[i].id = i;
                    //        CurrentEvent.Mycommand.Parameters[i].Type = s;
                    //        CurrentEvent.Mycommand.Parameters[i].Value = "";
                    //    }
                    //}
                }
                else
                {
                    Islinkage = false;
                    cmbMethodName.Text = "";
                }
            }
        }

        private void cmbDeviceName_DropDown(object sender, EventArgs e)
        {
            cmbDeviceName.Items.Clear();
            cmbDeviceName.Items.Add("");
            foreach (KeyValuePair<string, Type> kp in DicClasses)
            {
                cmbDeviceName.Items.Add(kp.Key);
            }
        }

        private void cmbMethodName_DropDown(object sender, EventArgs e)
        {
            cmbMethodName.Items.Clear();
            cmbMethodName.Items.Add("");
            if (cmbDeviceName.Text != "")
            {


                MethodInfo[] totalm = MethodList(cmbDeviceName.Text);
                foreach (MethodInfo yy in totalm)
                {
                    //  cmbMethodName.Items.Add(yy.Name);
                    if (yy.GetParameters().Length == 1 && yy.ReturnType.Name == "ParaAndResultStrut")
                    {
                        if (yy.GetParameters()[0].ParameterType.Name == "ParaAndResultStrut")
                        {
                            string s = GetFullMethodName(yy);
                            cmbMethodName.Items.Add(s);
                        }
                    }

                }
            }
        }

        private void txtEventName_Validated(object sender, EventArgs e)
        {
            if (CurrentEvent != null)
            {

                //查重
                if (NameCheckModify(CurrentStation.PLC2PCEventConfig.Eventlst, this.txtEventName.Text, treeEvent.SelectedNode.Index))
                {
                    CurrentEvent.Name = this.txtEventName.Text;

                    treeEvent.SelectedNode.Name = CurrentEvent.Name;
                    treeEvent.SelectedNode.Text = CurrentEvent.Name;
                }
                else
                {
                    this.txtEventName.Text = CurrentEvent.Name;
                    MessageBox.Show("'" + this.txtEventName.Text + "'" + "已存在。");
                }
            }
        }

        private void cmbSenderType_Validated(object sender, EventArgs e)
        {
            if (CurrentEvent != null)
            {
                //CurrentEvent.Name = this.txtEventName.Text;
                CurrentEvent.EventDescription = this.rtxtDescription.Text;
                //CurrentEvent.Vaddress = int.Parse(this.cmbEventAdr.Text);
                //CurrentEvent.Sender = (SenderType)Enum.Parse(typeof(SenderType), this.cmbSenderType.Text);
                CurrentEvent.PrType = (PLCPCPrType)Enum.Parse(typeof(PLCPCPrType), this.cmbPrType.Text);
                CurrentEvent.PrLength = int.Parse(txtPrLength.Text);
                CurrentEvent.ResultValueType = (PLCPCPrType)Enum.Parse(typeof(PLCPCPrType), this.cmbResValueType.Text);
                CurrentEvent.ResultValueLength = int.Parse(txtResValueLength.Text);
            }
        }

        private void cmbDeviceName_SelectedValueChanged(object sender, EventArgs e)
        {

            if (!Islinkage)
            {
                Islinkage = true;
                return;
            }
            if (cmbDeviceName.Text == "")
            {
                cmbMethodName.Items.Clear();
                cmbMethodName.Items.Add("");
                cmbMethodName.Text = "";

                CurrentEvent.Mycommand.DeviceName = cmbDeviceName.Text;

            }
            else
            {
                cmbMethodName.Items.Clear();
                cmbMethodName.Items.Add("");
                MethodInfo[] totalm = MethodList(cmbDeviceName.Text);

                foreach (MethodInfo yy in totalm)
                {
                    //  cmbMethodName.Items.Add(yy.Name);
                    if (yy.GetParameters().Length == 1 && yy.ReturnType.Name == "ParaAndResultStrut")
                    {
                        if (yy.GetParameters()[0].ParameterType.Name == "ParaAndResultStrut")
                        {
                            string s = GetFullMethodName(yy);
                            cmbMethodName.Items.Add(s);
                        }
                    }

                }

                //cmbMethodName.Items.Add(ProjectFileEditor.Global.GetFullMethodName(totalm[0]));
                cmbMethodName.SelectedIndex = 0;

                CurrentEvent.Mycommand.DeviceName = cmbDeviceName.Text;
                //ParameterInfo[] Pars = totalm[0].GetParameters();
                //for (int i = 0; i < Pars.Length; i++)
                //{
                //    dgPara.Rows.Add(new object[] { i, "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name, "" });
                //}
            }
        }


        private void cmbDeviceName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbMethodName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbMethodName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!Islinkage)
            {
                Islinkage = true;
                return;
            }
            //dgPara.Rows.Clear();
            if (cmbMethodName.Text != "")
            {




                ////ParameterInfo[] Pars = MethodList(cmbDeviceName.Text)[cmbMethodName.SelectedIndex - 1].GetParameters();
                //ParameterInfo[] Pars = ParameterList(cmbDeviceName.Text, cmbMethodName.Text);
                //if (cmbMethodName.Text != CurrentEvent.Mycommand.MethodName || CurrentEvent.Mycommand.Parameters.Count != Pars.Length)
                //{
                //    CurrentEvent.Mycommand.Parameters.Clear();
                //    for (int i = 0; i < Pars.Length; i++)
                //    {
                //        dgPara.Rows.Add(new object[] { i, "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name, "" });
                //        parameter p = new parameter();
                //        p.Type = "(" + Pars[i].ParameterType.FullName + ") "+ Pars[i].Name;
                //        p.Value = "";
                //        p.id = i;
                //        CurrentEvent.Mycommand.Parameters.Add(p);
                //    }
                //}
                //else
                //{
                //    if (Pars.Length == CurrentEvent.Mycommand.Parameters.Count)
                //    {

                //        for (int i = 0; i < Pars.Length; i++)
                //        {
                //            string s = "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name;
                //            if (s == CurrentEvent.Mycommand.Parameters[i].Type)
                //            {
                //                dgPara.Rows.Add(new object[] { i, CurrentEvent.Mycommand.Parameters[i].Type, CurrentEvent.Mycommand.Parameters[i].Value });
                //                CurrentEvent.Mycommand.Parameters[i].id = i;
                //            }
                //            else
                //            {
                //                dgPara.Rows.Add(new object[] { i, "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name, "" });
                //                CurrentEvent.Mycommand.Parameters[i].id = i;
                //                CurrentEvent.Mycommand.Parameters[i].Type = s;
                //                CurrentEvent.Mycommand.Parameters[i].Value = "";

                //            }
                //        }
                //    }
                //}
                ////给para赋值

                CurrentEvent.Mycommand.MethodName = cmbMethodName.Text;
            }
            else
            {
                CurrentEvent.Mycommand.MethodName = cmbMethodName.Text;
            }
        }

        private void treeEventPC_AfterSelect(object sender, TreeViewEventArgs e)
        {
            do
            {
                if (!isEdit)
                {
                    break;
                }
                System.Threading.Thread.Sleep(1);
            } while (true);


            lock (lockCurrentEvent)
            {
                CurrentEvent = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index];
            }
            this.txtEventNamePC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].Name;
            this.rtxtDescriptionPC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].EventDescription;
            this.txtEventIndexPC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].id.ToString();
            //this.cmbSenderTypePC.Text = CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].Sender.ToString();
            this.cmbPrTypePC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].PrType.ToString();
            this.cmbResValueTypePC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].ResultValueType.ToString();
            this.txtPrLengthPC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].PrLength.ToString();
            this.txtResValueLengthPC.Text = CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].ResultValueLength.ToString();


            //合法性检查
            //检查是否当前类包含该DeviceName

            //dgPara.Rows.Clear();
            cmbDeviceNamePC.Items.Clear();
            cmbDeviceNamePC.Items.Add("");
            cmbMethodNamePC.Items.Clear();
            cmbMethodNamePC.Items.Add("");
            if (!DicClasses.ContainsKey(CurrentEvent.Mycommand.DeviceName))
            {

                Islinkage = false;
                cmbDeviceNamePC.Text = "";
            }
            else
            {
                cmbDeviceNamePC.Items.Add(CurrentEvent.Mycommand.DeviceName);
                Islinkage = false;
                cmbDeviceNamePC.Text = CurrentEvent.Mycommand.DeviceName;

                cmbMethodNamePC.Items.Clear();
                cmbMethodNamePC.Items.Add("");
                ////检查是否包含
                List<MethodInfo> m = MethodList(cmbDeviceNamePC.Text).ToList();
                MethodInfo mf = m.Find(x => GetFullMethodName(x) == CurrentEvent.Mycommand.MethodName);
                if (mf != null)
                {
                    cmbMethodNamePC.Items.Add(CurrentEvent.Mycommand.MethodName);
                    Islinkage = false;
                    cmbMethodNamePC.Text = CurrentEvent.Mycommand.MethodName;


                    //ParameterInfo[] Pars = mf.GetParameters();
                    //for (int i = 0; i < Pars.Length; i++)
                    //{
                    //    string s = "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name;
                    //    if (s == CurrentEvent.Mycommand.Parameters[i].Type)
                    //    {
                    //        dgPara.Rows.Add(new object[] { i, CurrentEvent.Mycommand.Parameters[i].Type, CurrentEvent.Mycommand.Parameters[i].Value });
                    //        CurrentEvent.Mycommand.Parameters[i].id = i;
                    //    }
                    //    else
                    //    {
                    //        dgPara.Rows.Add(new object[] { i, "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name, "" });
                    //        CurrentEvent.Mycommand.Parameters[i].id = i;
                    //        CurrentEvent.Mycommand.Parameters[i].Type = s;
                    //        CurrentEvent.Mycommand.Parameters[i].Value = "";
                    //    }
                    //}
                }
                else
                {
                    Islinkage = false;
                    cmbMethodNamePC.Text = "";
                }
            }
        }

        private void txtEventNamePC_Validated(object sender, EventArgs e)
        {
            if (CurrentEvent != null)
            {

                //查重
                if (NameCheckModify(CurrentStation.PC2PLCEventConfig.Eventlst, this.txtEventName.Text, treeEvent.SelectedNode.Index))
                {
                    CurrentEvent.Name = this.txtEventNamePC.Text;

                    treeEventPC.SelectedNode.Name = CurrentEvent.Name;
                    treeEventPC.SelectedNode.Text = CurrentEvent.Name;
                }
                else
                {
                    this.txtEventNamePC.Text = CurrentEvent.Name;
                    MessageBox.Show("'" + this.txtEventNamePC.Text + "'" + "已存在。");
                }
            }
        }

        private void cmbPrTypePC_Validated(object sender, EventArgs e)
        {
            if (CurrentEvent != null)
            {
                //CurrentEvent.Name = this.txtEventName.Text;
                CurrentEvent.EventDescription = this.rtxtDescriptionPC.Text;
                //CurrentEvent.Vaddress = int.Parse(this.cmbEventAdr.Text);
                //CurrentEvent.Sender = (SenderType)Enum.Parse(typeof(SenderType), this.cmbSenderType.Text);
                CurrentEvent.PrType = (PLCPCPrType)Enum.Parse(typeof(PLCPCPrType), this.cmbPrTypePC.Text);
                CurrentEvent.PrLength = int.Parse(txtPrLengthPC.Text);
                CurrentEvent.ResultValueType = (PLCPCPrType)Enum.Parse(typeof(PLCPCPrType), this.cmbResValueTypePC.Text);
                CurrentEvent.ResultValueLength = int.Parse(txtResValueLengthPC.Text);
            }
        }

        private void cmbDeviceNamePC_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbMethodNamePC_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void treeEvent_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (CurrentEvent != null)
            {
                CurrentStation.PLC2PCEventConfig.Eventlst[e.Node.Index].IsEnbale = e.Node.Checked;
            }
        }

        private void treeEventPC_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (CurrentEvent != null)
            {
                CurrentStation.PC2PLCEventConfig.Eventlst[e.Node.Index].IsEnbale = e.Node.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
         

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //byte[] p = new byte[4];
            //string s = p.GetType().FullName;
            byte[,] ff;
            bool rr = false;
            //    InteropAssembly.LabVIEWExports.丹诺1F5算法(0, out ff,out rr);
            System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
            sp.Start();
            PLCHandle p = null;
            if (CurrentPLC != null)
            {
                p = CurrentPLC.Clone();
            }
            sp.Stop();
            MessageBox.Show(sp.ElapsedMilliseconds.ToString());
            p.PLCStationlst[1].PLC2PCEventConfig.Eventlst[1].plcEventState = PLCEventState.NC;
            CurrentPLC.PLCStationlst[1].PLC2PCEventConfig.Eventlst[1].plcEventState = PLCEventState.InHanding;

        }

        private void chbStationIsEnable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CurrentStation.PLC2PCEventConfig.Eventlst.Count; i++)
            {
                CurrentStation.PLC2PCEventConfig.Eventlst[i].IsEnbale = false;
                CurrentStation.PC2PLCEventConfig.Eventlst[i].IsEnbale = false;
            }
        }

        private void cmbDeviceNamePC_DropDown(object sender, EventArgs e)
        {

        }

        private void cmbMethodNamePC_DropDown(object sender, EventArgs e)
        {

        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cmbRack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PLCStationtree.SelectedNode != null && PLCStationtree.SelectedNode.Level == 1)
            {
                short stnum = short.Parse(cmbRack.Text);
                CurrentPLC.Rack = stnum;
            }
        }

        private void cmbSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PLCStationtree.SelectedNode != null && PLCStationtree.SelectedNode.Level == 1)
            {
                short stnum = short.Parse(cmbSlot.Text);
                CurrentPLC.Slot = stnum;
            }
        }

        private void nupDBIndex_ValueChanged(object sender, EventArgs e)
        {
            if (PLCStationtree.SelectedNode != null && PLCStationtree.SelectedNode.Level == 1)
            {

                CurrentPLC.DBIndex = (int)nupDBIndex.Value;
            }
        }

        private void dgEMGWarningMessage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentStation != null)
            {
                if (PLCStationtree.SelectedNode.Level == 2)
                {
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            //点位的名称
                            CurrentStation.WarningConfig.EMGMessagelst[e.RowIndex].Name = dgEMGWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                            break;
                        case 2:
                            //该报警是否启用
                            CurrentStation.WarningConfig.EMGMessagelst[e.RowIndex].IsEnbale = (bool)dgEMGWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            break;
                        case 3:
                            //报警信息
                            CurrentStation.WarningConfig.EMGMessagelst[e.RowIndex].ShowMessage = dgEMGWarningMessage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                            break;
                    }
                }
            }
        }
    }
}