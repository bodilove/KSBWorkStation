using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using Test.Common;
using System.Text;
using System.Xml;
using Test.Library;
using Infragistics.Win.UltraWinGrid;
using Test.ProjectTest;

namespace Test.ProjectFileEditor
{

    public partial class frmMain : Form
    {
        #region LHB 新增 触发事件
        
        public int OptionType = 0;//0:新增，需要添加项目，1：修改；保存测试走不同的逻辑分支；
        public ProjectTS projectTS = null;
       
        // 定义自定义事件
        public event EventHandler<MyNewValueEventArgs> CustomControlClicked;

        // 触发自定义事件
        protected virtual void OnCustomControlClicked(MyNewValueEventArgs e)
        {
            CustomControlClicked?.Invoke(this, e);
        }

        public delegate void MyNewValueEventHandler(MyNewValueEventArgs value);
        public class MyNewValueEventArgs : EventArgs
        {
            //private NormalWindowsCurveData _Normal;
            public ProjectTS NewValue
            {
                get;
            }
            public MyNewValueEventArgs(ProjectTS value)
            {
                NewValue = value;
            }
        }

        public string StationNum { get; set; }
        #endregion

        #region Viables Defind
        Project pl = new Project();
        Color bkColor = Color.White;
        bool HasShown = false;
        List<Command> CurCmdList = null;
        SubTestStep CurSubTS = null;
        //TestStep CurTS = null;
        int CurCmdIndex = 0;
        Control CurMenuCtr = null;
        TreeNode td = new TreeNode();
        public bool Loading = false;
        ComboBox cmbCompare;
        UltraGridComboEditor ultraCompare;
        nodeOperatorForDIYtree treeViewOperator;
        CommandOperator cmdOperator;
        private bool CanClose = false;
        private bool CanDelete = false;
        private bool Change = false;
        string newName = "New Project";

        bool[] SubTChecked;

        #endregion

        #region User Defined Functions

        private void UpdateCmdList()
        {
            int oldIndex = CurCmdIndex;
            if (tabTotal.SelectedTab != tabPageCommand1 &&
                tabTotal.SelectedTab != tabPageCommand2 &&
                tabTotal.SelectedTab != tabPageCommand3 &&
                tabTotal.SelectedTab != tabPageCommand4)
            {
                return;
            }
            Loading = true;
            datParameter.Rows.Clear();
            datCommand.Rows.Clear();
            if (CurCmdList == null) { return; }
            if (Global.Prj.Devices.Count == 0)
            {
                datCommand.Enabled = false;
                return;
            }
            else
            {
                datCommand.Enabled = true;
            }
            foreach (Command c in CurCmdList)
            {
                int r = datCommand.Rows.Add();
                datCommand[0, r].Value = (r + 1);
                DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)datCommand[1, r];
                DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)datCommand[2, r];
                DataGridViewComboBoxCell c4 = (DataGridViewComboBoxCell)datCommand[4, r];
                DataGridViewCheckBoxCell c5 = (DataGridViewCheckBoxCell)datCommand[5, r];
                foreach (KeyValuePair<string, Device> p in Global.Prj.Devices)
                {
                    c1.Items.Add(p.Key);
                }
                c5.Value = c.IsBreak;
                if (c1.Items.Count > 0)
                {
                    c1.Value = c.DeviceName;

                    foreach (MethodInfo m in Global.MethodList(Global.Prj.Devices[c.DeviceName].Class))
                    {
                        c2.Items.Add(Global.GetFullMethodName(m));
                    }
                    c2.Value = c.MethodName;
                    datCommand[3, r].Value = c.ParameterName;

                    c4.Items.Clear();
                    c4.Items.Add("");
                    foreach (KeyValuePair<string, Variable> p in Global.Prj.Variables)
                    {
                        c4.Items.Add(p.Key);
                    }
                    c4.Value = c.LinkedVariable;
                }
            }
            Loading = false;
            if (datCommand.Rows.Count > 0)
            {
                if (oldIndex < datCommand.Rows.Count)
                {
                    datCommand[1, oldIndex].Selected = true;
                }
                else
                {
                    datCommand[1, CurCmdIndex].Selected = true;
                }
            }

        }

        private void UpdateMeasure()
        {
            if (tabTotal.SelectedTab != tabpageMeasure)
            {
                return;
            }
            if (CurSubTS == null) { return; }

            if (Global.Prj.Devices.Count == 0)
            {
                datMeasure.Enabled = false;
                datCompare.Enabled = false;
                return;
            }
            else
            {
                datMeasure.Enabled = true;
                datCompare.Enabled = true;
            }

            Loading = true;

            datCompare[0, 0].Value = CurSubTS.CmpMode.ToString();
            datCompare[1, 0].Value = CurSubTS.LLimit;
            datCompare[2, 0].Value = "...";
            datCompare[3, 0].Value = CurSubTS.NomValue;
            datCompare[4, 0].Value = "...";
            datCompare[5, 0].Value = CurSubTS.ULimit;
            datCompare[6, 0].Value = "...";
            datCompare[7, 0].Value = CurSubTS.Unit;
            //datMeasure[0, 0].Value = CurSubTS.MeasureCmd.DeviceName;
            CheckCellVisible(CurSubTS.CmpMode);

            datMeasure.Rows.Clear();
            datMeasure.Rows.Add();
            DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)datMeasure[0, 0];
            DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)datMeasure[1, 0];
            DataGridViewComboBoxCell c4 = (DataGridViewComboBoxCell)datMeasure[3, 0];
            DataGridViewCheckBoxCell c5 = (DataGridViewCheckBoxCell)datMeasure[4, 0];
            foreach (KeyValuePair<string, Device> p in Global.Prj.Devices)
            {
                c1.Items.Add(p.Key);
            }
            if (c1.Items.Count > 0)
            {
                c1.Value = CurSubTS.MeasureCmd.DeviceName;

                if (CurSubTS.MeasureCmd.DeviceName != "")
                {
                    foreach (MethodInfo m in Global.MethodList(Global.Prj.Devices[CurSubTS.MeasureCmd.DeviceName].Class))
                    {
                        c2.Items.Add(Global.GetFullMethodName(m));
                    }
                    c2.Value = CurSubTS.MeasureCmd.MethodName;

                    datMeasure[2, 0].Value = CurSubTS.MeasureCmd.ParameterName;
                }
                c5.Value = CurSubTS.MeasureCmd.IsBreak;
                c4.Items.Clear();
                c4.Items.Add("");
                foreach (KeyValuePair<string, Variable> p in Global.Prj.Variables)
                {
                    c4.Items.Add(p.Key);
                }
                c4.Value = CurSubTS.MeasureCmd.LinkedVariable;
            }
            Loading = false;

            datMeasure[1, 0].Selected = true;
        }

        private void LoadNodes()
        {
            treeTS.Nodes.Clear();
            TreeNode node, snode;
            node = treeTS.Nodes.Add("TestInit", "测试初始化");
            //node.BackColor = bkColor;
            node.Checked = true;

            node = treeTS.Nodes.Add("TestSteps", "测试循环");
            node.Checked = true;
            //node.Nodes.Add("PreTest", "测试准备").BackColor = bkColor;

            int i = 1;
            foreach (TestStep ts in Global.Prj.TestSteps)
            {
                int j = 1;
                snode = node.Nodes.Add("TS" + (i++).ToString(), ts.Name);
                snode.Checked = ts.Enable;
                //snode.Nodes.Add("PreTest", "测试准备").BackColor = bkColor;
                foreach (SubTestStep subTS in ts.SubTest)
                {
                    TreeNode ssnode = snode.Nodes.Add("SubTS" + (j++).ToString(), subTS.Name);
                    ssnode.Checked = subTS.Enable;
                }
                //snode.Nodes.Add("PostTest", "测试后处理").BackColor = bkColor;

            }
            //node.Nodes.Add("PostTest", "测试后处理").BackColor = bkColor;
            node = treeTS.Nodes.Add("TestExit", "测试退出");
            node.Checked = true;
            //node.BackColor = bkColor;

            treeTS.SelectedNode = treeTS.TopNode;
        }

        private void UpdateGUI()
        {
            bool Enable = Global.Prj != null;
            if (Enable) { this.Text = Global.Prj.Title + " - Project Editor"; }
            else { this.Text = "Project Editor"; }

            menuCloseFile.Enabled = Enable;
            menuSaveFile.Enabled = Enable;
            menuSaveFileAs.Enabled = Enable;
            menuVariableList.Enabled = Enable;
            menuProperty.Enabled = Enable;
            menuDeviceList.Enabled = Enable;
            menuTestPins.Enabled = Enable;
            submenuIL.Enabled = Enable;
            submenuTL.Enabled = Enable;
            treeTS.Enabled = Enable;
            groupBox6.Enabled = Enable;
            tabTotal.Enabled = Enable;
            datParameter.Enabled = Enable;
            datCommand.Enabled = Enable;


            foreach (ToolStripItem m in menuEdit.DropDownItems)
            {
                if (m.GetType() == typeof(ToolStripMenuItem))
                {
                    m.Enabled = Enable;
                }
            }
            if (Enable)
            {

                treeTS.Select();
            }
            datCommand.Rows.Clear();

            //Initialize datCompare
            datCompare.Rows.Clear();
            datCompare.Rows.Add();
            DataGridViewComboBoxCell cmbC = (DataGridViewComboBoxCell)datCompare[0, 0];
            foreach (string s in Enum.GetNames(typeof(CompareMode)))
            {
                cmbC.Items.Add(s);
            }
            cmbC.Value = cmbC.Items[0];

        }

        private void CheckCellVisible(CompareMode cmp)
        {
            //datCompare.Columns[1].HeaderText = "下限";
            //datCompare.Columns[3].HeaderText = "设计值";
            //datCompare.Columns[5].HeaderText = "上限";
            switch (cmp)
            {
                case CompareMode.Between:
                case CompareMode.NotBetween:
                case CompareMode.HexStringBetween:
                    datCompare.Columns[1].Visible = true;
                    datCompare.Columns[2].Visible = true;
                    datCompare.Columns[3].Visible = false;
                    datCompare.Columns[4].Visible = false;
                    datCompare.Columns[5].Visible = true;
                    datCompare.Columns[6].Visible = true;
                    break;


                case CompareMode.Equal:
                    datCompare.Columns[1].Visible = false;
                    datCompare.Columns[2].Visible = false;
                    datCompare.Columns[3].Visible = true;
                    datCompare.Columns[4].Visible = true;
                    datCompare.Columns[5].Visible = false;
                    datCompare.Columns[6].Visible = false;
                    break;

                //case CompareMode.:
                //    datCompare.Columns[1].Visible = false;
                //    datCompare.Columns[2].Visible = false;
                //    datCompare.Columns[3].Visible = true;
                //    datCompare.Columns[4].Visible = true;
                //    datCompare.Columns[5].Visible = false;
                //    datCompare.Columns[6].Visible = false;
                //    break;

                case CompareMode.Contains:
                    datCompare.Columns[1].Visible = false;
                    datCompare.Columns[2].Visible = false;
                    datCompare.Columns[3].Visible = true;
                    datCompare.Columns[4].Visible = true;
                    datCompare.Columns[5].Visible = false;
                    datCompare.Columns[6].Visible = false;
                    break;

                //case CompareMode.CaseEqual:
                //    datCompare.Columns[1].Visible = false;
                //    datCompare.Columns[2].Visible = false;
                //    datCompare.Columns[3].Visible = true;
                //    datCompare.Columns[4].Visible = true;
                //    datCompare.Columns[5].Visible = false;
                //    datCompare.Columns[6].Visible = false;
                //    break;
            }
        }

        private void cmbCompare_TextChanged(object sender, System.EventArgs e)
        {
            CompareMode cmp = (CompareMode)Enum.Parse(typeof(CompareMode), cmbCompare.SelectedItem.ToString());
            CheckCellVisible(cmp);
        }

        private void ShowTab(TabPage tp, bool Visible)
        {
            if (Visible)
            {
                if (!tabTotal.TabPages.Contains(tp)) { tabTotal.TabPages.Add(tp); }
            }
            else
            {
                if (tabTotal.TabPages.Contains(tp)) { tabTotal.TabPages.Remove(tp); }
            }
        }

        #endregion

        #region File Operation

        void NewFile()
        {
            datCommand.EndEdit();
            datMeasure.EndEdit();
            datCompare.EndEdit();
            //CurCmdList = null;

            if (Global.Prj != null && Global.Prj.CheckChange())
            {
                DialogResult result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    newName = "New Project";
                    Global.Prj = new Project();
                    Global.Prj.Title = "Project2";
                    UpdateGUI();
                    LoadNodes();

                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    newName = "New Project";
                    Global.Prj = new Project();
                    Global.Prj.Title = "Project2";
                    UpdateGUI();
                    LoadNodes();
                }
                else
                {

                    //UpdateCmdList();
                    //UpdateMeasure();
                    //UpdateError();
                }

            }
            else
            {
                newName = "New Project";
                Global.Prj = new Project();
                Global.Prj.Title = "Project1";
                UpdateGUI();
                LoadNodes();

            }
            timCheckChange.Enabled = false;
        }

        public void LoadFile(string Title, string ProjectFile)
        {
            Global.Prj = new Project();
            Global.Prj.File = ProjectFile;
            Global.Prj.Title = Title + ".prj";
            Global.Prj.Load();

            UpdateRecent();
            UpdateGUI();
            LoadNodes();
            UpdateCmdList();
            UpdateMeasure();
            timCheckChange.Enabled = false;

            this.ShowDialog();
        }

        void OpenFile()
        {
            datCommand.EndEdit();
            datMeasure.EndEdit();
            datCompare.EndEdit();
            //CurCmdList = null;

            DialogResult result;
            if (Global.Prj != null)
            {

                result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.No)
                {
                    diagOpenFile.InitialDirectory = Application.StartupPath + "\\Projects";
                    diagOpenFile.Filter = "Project files (*.prj)|*.prj";
                    diagOpenFile.Multiselect = false;
                    diagOpenFile.FileName = "";
                    result = diagOpenFile.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        Global.Prj = new Project();
                        Global.Prj.File = diagOpenFile.FileName;
                        Global.Prj.Title = diagOpenFile.SafeFileName;
                        Global.Prj.Load();
                        newName = diagOpenFile.SafeFileName;

                        //-----------Write history to ini file
                        StreamWriter s = new StreamWriter(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Recent.ini", true);
                        s.WriteLine(diagOpenFile.FileName);//写入INI文件

                        SaveFile();
                        s.Flush();
                        s.Close();
                        UpdateRecent();
                        UpdateGUI();
                        LoadNodes();
                        UpdateCmdList();
                        UpdateMeasure();
                    }
                    else
                    {

                    }

                }
                if (result == DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    MessageBox.Show("已保存！");
                }
                if (result == DialogResult.Cancel)
                {

                }
            }
            else
            {
                diagOpenFile.InitialDirectory = Application.StartupPath + "\\Projects";
                diagOpenFile.Filter = "Project files (*.prj)|*.prj";
                diagOpenFile.Multiselect = false;
                diagOpenFile.FileName = "";
                result = diagOpenFile.ShowDialog(this);
                if (result == DialogResult.OK)             // 选中prj文件！
                {
                    Global.Prj = new Project();
                    Global.Prj.File = diagOpenFile.FileName;
                    Global.Prj.Title = diagOpenFile.SafeFileName;
                    Global.Prj.Load();
                    newName = diagOpenFile.SafeFileName;

                    //-----------Write history to ini file
                    StreamWriter s = new StreamWriter(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Recent.ini", true);
                    s.WriteLine(diagOpenFile.FileName);//写入INI文件

                    SaveFile(3);
                    s.Flush();
                    s.Close();
                    UpdateRecent();
                    UpdateGUI();
                    LoadNodes();
                    UpdateCmdList();
                    UpdateMeasure();
                }
                else
                {
                    //UpdateRecent();
                    //UpdateGUI();
                    //LoadNodes();
                    //UpdateCmdList();
                    //UpdateMeasure();
                    //UpdateError();
                }

            }
            timCheckChange.Enabled = false;
        }
        /// <summary>
        /// LHB：增加OptionType 保存测试项目 1:新增；2：修改；3：导入
        /// </summary>
        /// <param name="OptionType"></param>
        void SaveFile(int OptionType=0)
        {
            datCommand.EndEdit();
            datMeasure.EndEdit();
            datCompare.EndEdit();
            //CurCmdList = null;
            //CurCmdIndex = 0;

            if (Global.Prj == null)
            {
                MessageBox.Show("Save Project file Error!");
            }
            else
            {
                if (Global.Prj.File == null && !String.IsNullOrEmpty(StationNum))
                {
                    //假如新增的文件：添加测试，
                    menuSaveFileAs_Click(null, null);
                }
                else if (!String.IsNullOrEmpty(Global.Prj.File) && OptionType == 3)
                {
                    //LHB：2024-03-19 假如是导入的文件则要重新添加测试项
                    menuSaveFileAs_Click(null, null);
                }
                else
                {
                    Global.Prj.Save();
                }

            }
        }
        void SaveFileAs()
        {
            diagSaveFile.InitialDirectory = Application.StartupPath + "\\Projects";
            diagSaveFile.Filter = "Project files (*.prj)|*.prj";
            diagSaveFile.FileName = Global.Prj.Title;
            if (diagSaveFile.ShowDialog(this) == DialogResult.OK)
            {
                Global.Prj.File = diagSaveFile.FileName;
                FileInfo f = new FileInfo(Global.Prj.File);
                Global.Prj.Title = f.Name;
                newName = f.Name;
                Global.Prj.Save();
                UpdateGUI();
                #region LHB 增加出发事件
                projectTS = new ProjectTS() { StationNum = StationNum, Name = Global.Prj.Title, FilePath = Global.Prj.File };
                OnCustomControlClicked(new MyNewValueEventArgs(projectTS));//触发事件
                #endregion

                MessageBox.Show("已保存！");
            }
        }


        void CloseFile()
        {
            datCommand.EndEdit();
            datMeasure.EndEdit();
            datCompare.EndEdit();

            CanClose = true;

            if (Global.Prj != null && Global.Prj.CheckChange())
            {

                DialogResult result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    MessageBox.Show("已保存！！");
                    CurCmdList = null;
                    Global.Prj = null;
                    treeTS.CollapseAll();
                    groupBox2.Text = "命令列表";
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    CurCmdList = null;
                    Global.Prj = null;
                    treeTS.CollapseAll();
                    groupBox2.Text = "命令列表";
                }
                else
                {

                }


            }
            else
            {

                CurCmdList = null;
                Global.Prj = null;
                treeTS.CollapseAll();
                groupBox2.Text = "命令列表";

            }

            UpdateGUI();
            UpdateCmdList();
        }
        #endregion

        #region Clipboard Operation
        void CopyCommand()
        {
        }

        void CutCommand()
        {
        }

        void PasteCommand()
        {
        }
        #endregion


        public frmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.Prj != null)
            {

                DialogResult result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    MessageBox.Show("已保存！！");

                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    Application.Exit();
                }
                else
                {

                }


            }
            else
            {
                Application.Exit();
            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            SystemLib mysys = new SystemLib();
            mysys.Delay_ms(0);


            Global.Classes = new Dictionary<string, Type>();

            string[] AsmFiles = Directory.GetFiles(Application.StartupPath + "\\Device");

        //    string[] AsmFiles = Directory.GetFiles(Global.LibraryPath);
       
            // Global.Classes = new Dictionary<string, Type[]>();
            //Global.Methods = new Dictionary<string, MethodInfo[]>();

            for (int i = 0; i < AsmFiles.Length; i++)
            {
                Assembly A = Assembly.LoadFrom(AsmFiles[i]);
                //Global.Libraries[i] = A;

                foreach(Type tp in A.GetTypes())
                {
                    Global.Classes.Add(tp.FullName, tp);
                }
            }



          
            frmTest_Wide frm = new frmTest_Wide();
           

            Assembly asm;
            AssemblyName[] asmNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (AssemblyName asmname in asmNames)
            {
                if (asmname.Name == "Test.ProjectTest"
                    || asmname.Name == "Test.Library")
                {
                    asm = Assembly.Load(asmname);
                    Type[] t = asm.GetTypes();
                    for (int i = 0; i < t.Length; i++)
                    {
                        if (asmname.Name == "Test.ProjectTest")
                        {
                            if (t[i].Name != "frmTest_Wide") continue;
                        }

                        Global.Classes.Add(t[i].FullName, t[i]);

                    }
                }
            }


            treeViewOperator = new nodeOperatorForDIYtree(treeTS);
            cmdOperator = new CommandOperator(this, datCommand);

            //---------读入文件历史
            //StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Recent.ini");
            //int j = this.menuRecent.DropDownItems.Count;//减2
            //while (sr.Peek() >= 0)
            //{
            //    ToolStripMenuItem menuitem = new ToolStripMenuItem(sr.ReadLine());//读取INI文件并将信息加入菜单
            //    this.menuRecent.DropDownItems.Insert(j, menuitem); //在文件的下拉菜单中添加历史信息
            //    j++;
            //}
            //sr.Close();
            ////----------------
            ////LoadNodes();
            //UpdateGUI();
            UpdateRecent();
            submenuIL.Checked = true;
            menuView.Visible = false;
        }

        #region Recent功能
        public void UpdateRecent()
        {
            return;
            this.menuRecent.DropDownItems.Clear();
            List<string> lines = new List<string>(File.ReadAllLines(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\recent.ini", Encoding.UTF8));
            lines.Reverse();
            List<string> liststring = new List<string>();
            foreach (string eachstring in lines)//去除重复且原先有的 调到第一个
            {
                if (!liststring.Contains(eachstring))
                    liststring.Add(eachstring);

            }
            for (int i = 5; i < liststring.Count; )//控制行数，删掉第6行
            {
                liststring.RemoveAt(5);
            }


            for (int j = 0; j < liststring.Count; j++)//解决复制到别位置时，还能打开recent；在开始运行之前 修改recent.ini中文件路径。
            {
                string[] strlist = new string[5];
                strlist[j] = liststring[j];
                int k;
                k = strlist[j].LastIndexOf(@"\");
                string strsub = strlist[j].Substring(k);
                string str = Application.StartupPath + "\\Projects" + strsub;
                liststring[j] = str;
            }
            int i3 = 0;
            int ii = 1;
            foreach (string str in liststring)
            {
                //int i;
                string newString;//路径前面加序号
                newString = ii.ToString() + " " + str;

                //string str2 = str.Substring(i);
                //string str3 = Application.StartupPath + str2;
                ToolStripMenuItem menuitem = new ToolStripMenuItem(newString);
                this.menuRecent.DropDownItems.Insert(i3, menuitem);
                i3++;
                ii++;
                menuitem.Click += new EventHandler(menuitem_Click);

            }
            liststring.Reverse();
            File.WriteAllLines(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\recent.ini", liststring.ToArray(), Encoding.UTF8);
            UpdateGUI();
        }
        private void menuitem_Click(object sender, EventArgs e)
        {

            if (Global.Prj != null)
            {
                DialogResult result;
                result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    UpdateRecent();
                    UpdateGUI();
                    LoadNodes();
                    UpdateCmdList();
                    UpdateMeasure();
                    ToolStripMenuItem menu = (ToolStripMenuItem)sender;

                    OpenRecent(menu.Text);
                }
                if (result == DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    MessageBox.Show("已保存！");
                    ToolStripMenuItem menu = (ToolStripMenuItem)sender;
                    OpenRecent(menu.Text);
                }

            }
            else
            {
                ToolStripMenuItem menu = (ToolStripMenuItem)sender;
                OpenRecent(menu.Text);
            }

        }
        private void OpenRecent(string text)
        {
            string newText = text.Substring(2);//去除路径前面的序号
            Global.Prj = new Project();
            Global.Prj.File = newText;
            string str4 = Path.GetFileName(newText);
            Global.Prj.Title = str4;
            Global.Prj.Load();
            // Global.Prj.File2 = newText;
            newName = Path.GetFileName(newText);

            StreamWriter s = new StreamWriter(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Recent.ini", true);
            s.WriteLine(newText);//写入INI文件
            SaveFile();
            s.Flush();
            s.Close();
            UpdateRecent();

            LoadNodes();
            UpdateCmdList();
            UpdateMeasure();
        }
        #endregion
        private void menuNew_Click(object sender, EventArgs e)
        {
            NewFile();
        }
        private void menuOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        private void menuSaveFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }
        private void menuCloseFile_Click(object sender, EventArgs e)
        {

            CanDelete = true;
            CloseFile();
            // CloseFile();
            CanDelete = false;
            CanClose = false;
            timCheckChange.Enabled = false;
        }
        private void menuSaveFileAs_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }


        private void frmMain_Shown(object sender, EventArgs e)
        {
            HasShown = true;
        }

        private void datParameter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                frmSelectVariable frm = new frmSelectVariable();
                string s = frm.GetTP(this);
                if (s == "") return;
                if (tabTotal.SelectedTab == tabPageCommand1 ||
                tabTotal.SelectedTab == tabPageCommand2 ||
                tabTotal.SelectedTab == tabPageCommand3 ||
                tabTotal.SelectedTab == tabPageCommand4)
                {
                    //datParameter[2, e.RowIndex].Value = "["+s+"]";
                    CurCmdList[CurCmdIndex].Parameter[e.RowIndex].Type = datParameter[1, e.RowIndex].Value.ToString();
                    CurCmdList[CurCmdIndex].Parameter[e.RowIndex].Value = s;
                    UpdateCmdList();
                }
                else if (tabTotal.SelectedTab == tabpageMeasure)
                {
                    CurSubTS.MeasureCmd.Parameter[e.RowIndex].Type = datParameter[1, e.RowIndex].Value.ToString();
                    CurSubTS.MeasureCmd.Parameter[e.RowIndex].Value = s;
                    UpdateMeasure();
                }
            }
        }
        private void datParameter_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!HasShown | Loading) { return; }
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) { return; }

            if (tabTotal.SelectedTab == tabPageCommand1 ||
                tabTotal.SelectedTab == tabPageCommand2 ||
                tabTotal.SelectedTab == tabPageCommand3 ||
                tabTotal.SelectedTab == tabPageCommand4)
            {
                if (CurCmdList == null) return;
                if (CurCmdList.Count == 0) { return; }
                Command cmd = CurCmdList[CurCmdIndex];
                if (datParameter[2, e.RowIndex].Value == null)
                    datParameter[2, e.RowIndex].Value = "";
                cmd.Parameter[e.RowIndex].Type = datParameter[1, e.RowIndex].Value.ToString();
                cmd.Parameter[e.RowIndex].Value = datParameter[2, e.RowIndex].Value.ToString();
                datCommand[3, CurCmdIndex].Value = cmd.ParameterName;
            }
            else if (tabTotal.SelectedTab == tabpageMeasure)
            {
                Command cmd = CurSubTS.MeasureCmd;
                //cmd.Parameter = new Command.parameter[datParameter.Rows.Count];
                if (datParameter[2, e.RowIndex].Value == null)
                    datParameter[2, e.RowIndex].Value = "";
                cmd.Parameter[e.RowIndex].Type = datParameter[1, e.RowIndex].Value.ToString();
                cmd.Parameter[e.RowIndex].Value = datParameter[2, e.RowIndex].Value.ToString();
                datMeasure[2, 0].Value = cmd.ParameterName;
            }
        }
        private void datCommand_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (CanClose == true)
            {
                return;
            }
            if (CanDelete == true)
            {
                return;
            }
            CurCmdIndex = e.RowIndex;
            if (Loading) return;
            if (datCommand[0, 0].Value == null) return;

            Command cmd = CurCmdList[CurCmdIndex];

            string device = datCommand[1, e.RowIndex].Value.ToString();
            DataGridViewComboBoxCell cc = (DataGridViewComboBoxCell)datCommand[2, e.RowIndex];
            if (cc.Value == null) return;
            int index = cc.Items.IndexOf(cc.Value);
            if (index < 0) return;
            Loading = true;
            datParameter.Rows.Clear();
            Loading = false;
            ParameterInfo[] Pars = Global.MethodList(Global.Prj.Devices[device].Class)[index].GetParameters();
            if (Pars.Length > 0)
            {
                datParameter.Rows.Add(Pars.Length);
                for (int i = 0; i < Pars.Length; i++)
                {
                    DataGridViewRow r = datParameter.Rows[i];
                    r.Cells[0].Value = (i + 1).ToString();
                    r.Cells[1].Value = "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name;
                    if (cmd.Parameter == null)
                    {
                        cmd.Parameter = new Command.parameter[Pars.Length];
                    }
                    else
                    {
                        if (Change == false)
                        {
                            r.Cells[2].Value = cmd.Parameter[i].Value;
                        }
                        // r.Cells[2].Value = cmd.Parameter[i].Value;//下一行参数比上一行多时，删除上一行报错！
                        else
                        {
                            Change = false;
                            return;
                        }
                    }
                }
            }
            datCommand[3, e.RowIndex].Value = cmd.ParameterName;
        }
        private void datCommand_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!HasShown || Loading) return;
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            string device;
            DataGridViewCell C = datCommand[col, row];

            try
            {
                switch (col)
                {
                    case 1:
                        CurCmdList[row].DeviceName = C.Value.ToString();
                        device = C.Value.ToString();
                        DataGridViewComboBoxCell CobCell = (DataGridViewComboBoxCell)datCommand[2, row];
                        CobCell.Items.Clear();
                        foreach (MethodInfo m in Global.MethodList(Global.Prj.Devices[device].Class))
                        {
                            CobCell.Items.Add(Global.GetFullMethodName(m));
                        }
                        if (CobCell.Items.Count > 0) CobCell.Value = CobCell.Items[0];
                        break;
                    case 2:
                        if (C.Value != null)
                        {
                            Command cmd = CurCmdList[row];
                            cmd.MethodName = C.Value.ToString();
                            cmd.Parameter = null;
                            datCommand_CellEnter(sender, e);
                        }
                        break;
                    case 4:
                        if (C.Value == null)
                        {
                            CurCmdList[row].LinkedVariable = "";
                        }
                        else
                        {
                            CurCmdList[row].LinkedVariable = C.Value.ToString();
                        }
                        break;
                    case 5:
                        if (C.Value == null)
                        {
                            CurCmdList[row].IsBreak = false;
                        }
                        else
                        {
                            CurCmdList[row].IsBreak =(bool)C.Value;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void datCommand_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                datCommand.ClearSelection();
                CurMenuCtr = datCommand;
                foreach (ToolStripItem m in treeMenu.Items)
                {
                    m.Visible = true;
                }
                cmMenuRename.Visible = false;
                cmMenuAdd.Text = "添加命令";
                cmMenuDelete.Enabled = (datCommand.Rows.Count > 0) ? true : false;
            }
            else if (e.Button == MouseButtons.Left)
            {
                CurMenuCtr = datCommand;
            }
        }
        private void datCommand_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex < 0) { return; }
                datCommand[e.ColumnIndex, e.RowIndex].Selected = true;
                foreach (ToolStripItem m in treeMenu.Items)
                {
                    m.Enabled = true;
                }
                if (e.RowIndex == 0)
                {
                    cmMenuUp.Enabled = false;
                }
                if (e.RowIndex == datCommand.Rows.Count - 1)
                {
                    cmMenuDown.Enabled = false;
                }
            }
        }
        private void datCommand_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (datCommand.SelectedCells.Count == 0)
                {
                    foreach (ToolStripItem m in treeMenu.Items)
                    {
                        m.Enabled = false;
                    }
                    cmMenuAdd.Enabled = true;
                    cmMenuPaste.Enabled = true;
                }
            }
        }
        private void datCompare_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            switch (((DataGridView)sender).CurrentCell.ColumnIndex)
            {
                case 0:
                    cmbCompare = (ComboBox)datCompare.EditingControl;
                    cmbCompare.SelectedIndexChanged += new EventHandler(cmbCompare_TextChanged);
                    break;
                case 1:
                case 2:
                case 3:
                    break;
            }
        }
        private void datCompare_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!HasShown || CurSubTS == null) { return; }
            if (e.RowIndex < 0) return;

            if (datCompare[e.ColumnIndex, e.RowIndex].Value == null)
            {
                datCompare[e.ColumnIndex, e.RowIndex].Value = "";
            }

            switch (e.ColumnIndex)
            {
                case 0:
                    CurSubTS.CmpMode = (CompareMode)Enum.Parse(typeof(CompareMode), datCompare[e.ColumnIndex, e.RowIndex].Value.ToString());
                    break;
                case 1:
                    CurSubTS.LLimit = datCompare[e.ColumnIndex, e.RowIndex].Value.ToString();
                    break;
                case 3:
                    CurSubTS.NomValue = datCompare[e.ColumnIndex, e.RowIndex].Value.ToString();
                    break;
                case 5:
                    CurSubTS.ULimit = datCompare[e.ColumnIndex, e.RowIndex].Value.ToString();
                    break;
                case 7:
                    CurSubTS.Unit = datCompare[e.ColumnIndex, e.RowIndex].Value.ToString();
                    break;   
            }
        }
        private void datMeasure_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!HasShown || Loading) return;
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            string device;
            DataGridViewCell C = datMeasure[col, row];

            try
            {
                switch (col)
                {
                    case 0:
                        CurSubTS.MeasureCmd.DeviceName = C.Value.ToString();
                        device = C.Value.ToString();
                        DataGridViewComboBoxCell CobCell = (DataGridViewComboBoxCell)datMeasure[1, row];
                        CobCell.Items.Clear();
                        foreach (MethodInfo m in Global.MethodList(Global.Prj.Devices[device].Class))
                        {
                            CobCell.Items.Add(Global.GetFullMethodName(m));
                        }
                        if (CobCell.Items.Count > 0) CobCell.Value = CobCell.Items[0];
                        break;
                    case 1:
                        if (C.Value != null)
                        {
                            Command cmd = CurSubTS.MeasureCmd;
                            cmd.MethodName = C.Value.ToString();
                            cmd.Parameter = null;
                            datMeasure_CellEnter(sender, e);
                        }
                        break;
                    case 3:
                        if (C.Value == null)
                        {
                            CurSubTS.MeasureCmd.LinkedVariable = "";
                        }
                        else
                        {
                            CurSubTS.MeasureCmd.LinkedVariable = C.Value.ToString();
                        }
                        break;
                    case 4:
                        if (C.Value == null)
                        {
                            CurSubTS.MeasureCmd.IsBreak = false;
                        }
                        else
                        {
                            CurSubTS.MeasureCmd.IsBreak = (bool)C.Value;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        private void datMeasure_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                CurCmdIndex = e.RowIndex;
                if (Loading) return;
                if (datMeasure[0, 0].Value == null) return;

                Command cmd = CurSubTS.MeasureCmd;

                string device = datMeasure[0, e.RowIndex].Value.ToString();
                DataGridViewComboBoxCell cc = (DataGridViewComboBoxCell)datMeasure[1, e.RowIndex];
                if (cc.Value == null) return;
                int index = cc.Items.IndexOf(cc.Value);
                if (index < 0) return;
                Loading = true;
                datParameter.Rows.Clear();
                Loading = false;
                ParameterInfo[] Pars = Global.MethodList(Global.Prj.Devices[device].Class)[index].GetParameters();
                if (Pars.Length > 0)
                {
                    datParameter.Rows.Add(Pars.Length);
                    for (int i = 0; i < Pars.Length; i++)
                    {
                        DataGridViewRow r = datParameter.Rows[i];
                        r.Cells[0].Value = (i + 1).ToString();
                        r.Cells[1].Value = "(" + Pars[i].ParameterType.FullName + ") " + Pars[i].Name;
                        if (cmd.Parameter == null)
                        {
                            cmd.Parameter = new Command.parameter[Pars.Length];
                        }
                        else
                        {
                            r.Cells[2].Value = cmd.Parameter[i].Value;
                        }
                    }
                }
                datMeasure[2, e.RowIndex].Value = cmd.ParameterName;
            }
            catch (Exception ex)
            {
                
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Log\GetSubString" + DateTime.Now.ToString("yyyyMMdd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":datMeasure_CellEnter;" + ex.ToString() + Environment.NewLine);
            }
        }

        private void cmMenuAdd_Click(object sender, EventArgs e)
        {
            if (CurMenuCtr == treeTS)
            {
                treeViewOperator.AddNode();
            }
            else if (CurMenuCtr == datCommand)
            {
                cmdOperator.AddCommand(CurCmdList);
            }
        }
        private void cmMenuDelete_Click(object sender, EventArgs e)
        {
            Change = true;
            if (CurMenuCtr == treeTS)
            {
                treeViewOperator.DeleteNodes();
            }
            else if (CurMenuCtr == datCommand)
            {
                cmdOperator.DeleteCommand(CurCmdList);
            }
            UpdateCmdList();
            UpdateMeasure();
        }
        private void cmMenuRename_Click(object sender, EventArgs e)
        {
            treeTS.LabelEdit = true;

            TreeNode node = treeTS.SelectedNode;
            node.BeginEdit();
        }
        private void cmMenuUp_Click(object sender, EventArgs e)
        {
            if (CurMenuCtr == treeTS)
            {
                treeViewOperator.MoveUp();
            }
            else if (CurMenuCtr == datCommand)
            {
                cmdOperator.MoveUp(CurCmdList, CurCmdIndex);
              CurCmdIndex=  datCommand.SelectedCells[0].RowIndex;
            }
            //UpdateCmdList();
        }
        private void cmMenuDown_Click(object sender, EventArgs e)
        {
            if (CurMenuCtr == treeTS)
            {
                treeViewOperator.MoveDown();
            }
            else if (CurMenuCtr == datCommand)
            {
                cmdOperator.MoveDown(CurCmdList, CurCmdIndex);
                CurCmdIndex = datCommand.SelectedCells[0].RowIndex;
            }
        }

        private void tabTotal_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabControl T = (TabControl)sender;

            if (e.TabPage == tabpageMeasure)
            {
                datCommand.Visible = false;
                splitContent.Panel2Collapsed = false;
            }
            else
            {
                if (treeTS.SelectedNode.Name == "TestSteps")
                {
                    if (e.TabPage == tabPageCommand1)
                    { CurCmdList = Global.Prj.PreTest; }
                    else if (e.TabPage == tabPageCommand2)
                    { CurCmdList = Global.Prj.PostTest; }
                    else if (e.TabPage == tabPageCommand3)
                    { CurCmdList = Global.Prj.SucTest; }
                    else if (e.TabPage == tabPageCommand4)
                    { CurCmdList = Global.Prj.FailTest; }
                }
                else if (treeTS.SelectedNode.Name.StartsWith("TS"))
                {
                    int n = treeTS.SelectedNode.Index;
                    if (e.TabPage == tabPageCommand1)
                    { CurCmdList = Global.Prj.TestSteps[n].PreTest; }
                    else if (e.TabPage == tabPageCommand2)
                    { CurCmdList = Global.Prj.TestSteps[n].PostTest; }
                    //else if (e.TabPage == tabPageCommand3)
                    //{ CurCmdList = Global.Prj.TestSteps[n].SucTest; }
                    //else if (e.TabPage == tabPageCommand4)
                    //{ CurCmdList = Global.Prj.TestSteps[n].FailTest; }
                }

                datCommand.Visible = true;
                splitContent.Panel2Collapsed = false;
            }
            UpdateCmdList();
            UpdateMeasure();
        }
        private void projectPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProperty frm = new frmProperty();
            frm.ShowDialog(this);
        }

        private void testPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestPin frm = new frmTestPin();
            frm.ShowDialog(this);
            UpdateCmdList();
            UpdateMeasure();
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void 打开OToolStripButton_ButtonClick(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void cmMenuCopy_Click(object sender, EventArgs e)
        {
           
                if (CurMenuCtr == treeTS)
                {
                //    if (treeTS.SelectedNodes.Count <= 1)
                //    {
                //        treeViewOperator.CopyNode(false);
                //    }
                    //else
                    //{
                        treeViewOperator.CopyNodes(false);
                    //}
                }
                else if (CurMenuCtr == datCommand)
                {
                    List<int> poss = new List<int>();
                    foreach (DataGridViewCell dr in datCommand.SelectedCells)
                    {
                        if (!poss.Contains(dr.RowIndex))
                        {
                            poss.Add(dr.RowIndex);
                        }
                    }
                    cmdOperator.CopyCommands(false, CurCmdList, poss);
                }
            
      
        }

        private void cmMenuPaste_Click(object sender, EventArgs e)
        {

            if (CurMenuCtr == treeTS)
            {
                //if (treeViewOperator..Count <= 1)
                //{
                //    treeViewOperator.PasteNode();
                //}
                //else
                //{
                    treeViewOperator.PasteNodes();
                //}
            }
            else if (CurMenuCtr == datCommand)
            {
               // cmdOperator.PasteCommand(CurCmdList);
                cmdOperator.PasteCommands(CurCmdList);
            }

            UpdateCmdList();


        }

        private void cmMenuCut_Click(object sender, EventArgs e)
        {
            if (CurMenuCtr == treeTS)
            {
                treeViewOperator.CopyNodes(true);
            }
            else if (CurMenuCtr == datCommand)
            {
                List<int> poss = new List<int>();
                foreach (DataGridViewCell dr in datCommand.SelectedCells)
                {
                    if (!poss.Contains(dr.RowIndex))
                    {
                        poss.Add(dr.RowIndex);
                    }
                }
                cmdOperator.CopyCommands(true, CurCmdList, poss);
            }
        }

        private void variableListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVariable frm = new frmVariable();
            frm.ShowDialog(this);
            UpdateCmdList();
            UpdateMeasure();
        }

        private void menuUp_Click(object sender, EventArgs e)
        {
            cmMenuUp_Click(this, null);
        }

        private void menuDown_Click(object sender, EventArgs e)
        {
            cmMenuDown_Click(this, null);
        }

        private void menuCut_Click(object sender, EventArgs e)
        {
            cmMenuCut_Click(this, null);
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            cmMenuCopy_Click(this, null);
        }

        private void menuPaste_Click(object sender, EventArgs e)
        {
            cmMenuPaste_Click(this, null);
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            cmMenuDelete_Click(this, null);
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            cmMenuAdd_Click(this, null);
        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            cmMenuCut_Click(this, null);
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            cmMenuCopy_Click(this, null);
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            cmMenuPaste_Click(this, null);
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {

        }



        public void Compare(XmlDocument prj1, XmlDocument prj2)
        {
            if (prj1.InnerText == prj2.InnerText)
            {
                this.Text = newName + " - Project Editor";
                //prj1.Clone();
                //数据没变动，随时关闭。
            }
            else
                this.Text = newName + " - Project Editor*";
            //提示保存代码，数据已变动。
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.Prj != null && Global.Prj.CheckChange())
            {

                DialogResult result = MessageBox.Show("是否保存未保存工程？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    menuSaveFile_Click(null, null);
                    MessageBox.Show("已保存！！");

                    datCommand.EndEdit();
                    datMeasure.EndEdit();
                    datCompare.EndEdit();
                    CanDelete = true;
                    CanClose = true;
                    CurCmdList = null;
                    Global.Prj = null;
                    treeTS.CollapseAll();
                    groupBox2.Text = "命令列表";
                    UpdateGUI();
                    UpdateCmdList();
                    CanDelete = false;
                    CanClose = false;
                    base.Dispose();
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    // e.Cancel = false;
                    //base.Dispose();
                    datCommand.EndEdit();
                    datMeasure.EndEdit();
                    datCompare.EndEdit();
                    CanDelete = true;
                    CanClose = true;
                    CurCmdList = null;
                    Global.Prj = null;
                    treeTS.CollapseAll();
                    groupBox2.Text = "命令列表";
                    UpdateGUI();
                    UpdateCmdList();
                    CanDelete = false;
                    CanClose = false;

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }


            }
            else
            {
                menuCloseFile_Click(this, null);
                base.Dispose();


            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            cmMenuUp_Click(this, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            cmMenuDown_Click(this, null);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            cmMenuDelete_Click(this, null);
        }

        private void 设备列表DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLibrary frm = new frmLibrary();
            frm.ShowDialog(this);
            UpdateCmdList();
            UpdateMeasure();

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            if (node.Name.StartsWith("TS"))
            {
                Global.Prj.TestSteps[node.Index].Description = txtDescription.Text;
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].Description = txtDescription.Text;
            }

        }

        private void txtErrorNum_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            if (node.Name.StartsWith("TS"))
            {

            }
            else if (node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].ErrorCode = txtErrorNum.Text;
            }
        }

        private void timCheckChange_Tick(object sender, EventArgs e)
        {
            if (Global.Prj != null && Global.Prj.CheckChange())
            {
                this.Text = Global.Prj.Title + " - Project Editor *";
            }
            else
            {
                this.Text = Global.Prj.Title + " - Project Editor";
            }

        }



        private void cmbMeasureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].MeasureType = (enumMeasureType)cmbMeasureType.SelectedIndex + 1;
        }

        private void txtTimeout_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].Timeout = Convert.ToInt16(txtTimeout.Text);
        }

        private void chkRecordTotal_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            if (node.Name.StartsWith("TS"))
            {
                //MessageBox.Show(chkRecordTotal.CheckState.ToString());
                //Global.Prj.TestSteps[node.Index].Description = txtDescription.Text;
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].SaveResult = chkRecordTotal.Checked;
            }
        }

        private void chkRecordTotal_CheckStateChanged(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            if (node.Name.StartsWith("TS"))
            {
                //MessageBox.Show(chkRecordTotal.CheckState.ToString());
                TestStep ts = Global.Prj.TestSteps[node.Index];
                if (chkRecordTotal.CheckState == CheckState.Checked)
                {
                    foreach (SubTestStep subt in ts.SubTest)
                    {
                        subt.SaveResult = true;
                    }
                }
                if (chkRecordTotal.CheckState == CheckState.Unchecked)
                {
                    foreach (SubTestStep subt in ts.SubTest)
                    {
                        subt.SaveResult = false;
                    }
                }
                if (chkRecordTotal.CheckState == CheckState.Indeterminate)
                {
                    for (int k = 0; k < ts.SubTest.Count; k++)
                    {
                        ts.SubTest[k].SaveResult = SubTChecked[k];
                    }
                }
            }
        }

        private void numRetryCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void datCommand_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            this.lblStatus.Text = "There are command Errors!";
        }

        private void submenuIL_Click(object sender, EventArgs e)
        {
            submenuIL.Checked = true;
            submenuTL.Checked = false;
            splitTestStep.BringToFront();
        }

        private void submenuTL_Click(object sender, EventArgs e)
        {
            submenuIL.Checked = false;
            submenuTL.Checked = true;
            DataGrid.BringToFront();
            //splitTestList.BringToFront();
        }

        private void datCompare_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            this.lblStatus.Text = "There are Compare command Errors!";
        }

        private void datCompare_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 ||
                e.ColumnIndex == 4 ||
                e.ColumnIndex == 6)
            {
                frmSelectVariable frm = new frmSelectVariable();
                string s = frm.GetTP(this);
                if (s == "") return;

                datCompare[e.ColumnIndex - 1, 0].Value = s;

                //.MeasureCmd.Parameter[e.RowIndex].Type = datParameter[1, e.RowIndex].Value.ToString();
                //CurSubTS.MeasureCmd.Parameter[e.RowIndex].Value = s;
                //UpdateMeasure();

            }
        }

        private void numRetryCount_Validated(object sender, EventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;
            Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].RetryCount = Convert.ToInt16(numRetryCount.Text);

        }

        private void treeTS_AfterSelect(object sender, TreeViewEventArgs e)
        {
          
            TreeView view = (TreeView)sender;
            tabPageCommand1.Text = "命令列表";
            groupBox2.Text = e.Node.Text;
            datCommand.BringToFront();

            //txtDescription.Enabled = false;

            switch (e.Node.Name)
            {
                case "TestInit":
                    ShowTab(tabPageCommand1, true);
                    ShowTab(tabPageCommand2, false);
                    ShowTab(tabPageCommand3, false);
                    ShowTab(tabPageCommand4, false);
                    ShowTab(tabpageMeasure, false);
                    CurCmdList = Global.Prj.TestInits;
                    break;
                case "TestExit":
                    ShowTab(tabPageCommand1, true);
                    ShowTab(tabPageCommand2, false);
                    ShowTab(tabPageCommand3, false);
                    ShowTab(tabPageCommand4, false);
                    ShowTab(tabpageMeasure, false);
                    CurCmdList = Global.Prj.TestExits;
                    break;
                case "TestSteps":
                    ShowTab(tabPageCommand1, true);
                    ShowTab(tabPageCommand3, true);
                    ShowTab(tabPageCommand4, true);
                    ShowTab(tabPageCommand2, true);

                    ShowTab(tabpageMeasure, false);
                    tabPageCommand1.Text = "测试准备命令列表";
                    tabPageCommand2.Text = "测试后处理命令列表";
                    tabPageCommand3.Text = "测试成功命令列表";
                    tabPageCommand4.Text = "测试失败命令列表";
                    if (tabTotal.SelectedTab == tabPageCommand1)
                    {
                        CurCmdList = Global.Prj.PreTest;
                    }
                    else if (tabTotal.SelectedTab == tabPageCommand2)
                    {
                        CurCmdList = Global.Prj.PostTest;
                    }
                    else if (tabTotal.SelectedTab == tabPageCommand3)
                    {
                        CurCmdList = Global.Prj.SucTest;
                    }
                    else if (tabTotal.SelectedTab == tabPageCommand4)
                    {
                        CurCmdList = Global.Prj.FailTest;
                    }
                    else
                    {
                        CurCmdList = null;
                    }
                    break;
            }


            if (e.Node.Level > 0)
            {
                //node location
                int i = 0;
                int j = 0;
                j = e.Node.Index;
                i = e.Node.Parent.Index;

                //handle test steps
                if (e.Node.Name.StartsWith("TS"))
                {
                    TestStep ts = Global.Prj.TestSteps[j];
                    //Test Step General Settings
                    groupBox6.Enabled = true;
                    numRetryCount.Enabled = false;
                    cmbMeasureType.Enabled = false;
                    txtTimeout.Enabled = false;
                    txtErrorNum.Enabled = false;
                    txtErrorNum.Text = "";
                    txtDescription.Text = ts.Description;

                    chkRecordTotal.ThreeState = true;
                    int nCheck = 0;
                    SubTChecked = new bool[ts.SubTest.Count];
                    for (int k = 0; k < ts.SubTest.Count; k++)
                    {
                        SubTChecked[k] = ts.SubTest[k].SaveResult;
                        if (SubTChecked[k]) nCheck++;
                    }

                    if (nCheck == 0)
                    {
                        chkRecordTotal.CheckState = CheckState.Unchecked;
                    }
                    else if (nCheck == ts.SubTest.Count)
                    {
                        chkRecordTotal.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkRecordTotal.CheckState = CheckState.Indeterminate;
                    }

                    ShowTab(tabPageCommand1, true);
                    ShowTab(tabPageCommand2, true);
                    ShowTab(tabPageCommand3, false);
                    ShowTab(tabPageCommand4, false);
                    ShowTab(tabpageMeasure, false);
                    tabPageCommand1.Text = "测试准备命令列表";
                    tabPageCommand2.Text = "测试后处理命令列表";
                    tabPageCommand3.Text = "测试成功命令列表";
                    tabPageCommand4.Text = "测试失败命令列表";

                    if (tabTotal.SelectedTab == tabPageCommand1)
                    {
                        CurCmdList = Global.Prj.TestSteps[j].PreTest;
                    }
                    else if (tabTotal.SelectedTab == tabPageCommand2)
                    {
                        CurCmdList = Global.Prj.TestSteps[j].PostTest;
                    }
                    //else if (tabTotal.SelectedTab == tabPageCommand3)
                    //{
                    //    CurCmdList = Global.Prj.TestSteps[j].SucTest;
                    //}
                    //else if (tabTotal.SelectedTab == tabPageCommand4)
                    //{
                    //    CurCmdList = Global.Prj.TestSteps[j].FailTest;
                    //}
                    else
                    {
                        CurSubTS = null;
                    }
                }

                //handle subtest steps
                if (e.Node.Name.StartsWith("SubTS"))
                {
                    //Sub Test Step General Settings
                    SubTestStep subT = Global.Prj.TestSteps[i].SubTest[j];
                    groupBox6.Enabled = true;
                    numRetryCount.Enabled = true;
                    cmbMeasureType.Enabled = true;
                    txtTimeout.Enabled = true;
                    txtErrorNum.Enabled = true;
                    txtDescription.Text = subT.Description;
                    //txtErrorNum.Text = subT.ErrorNum;
                    txtErrorNum.Text = subT.ErrorCode;
                    numRetryCount.Text = subT.RetryCount.ToString();
                    cmbMeasureType.SelectedIndex = Convert.ToInt16(subT.MeasureType) - 1;
                    txtTimeout.Text = subT.Timeout.ToString();
                    chkRecordTotal.ThreeState = false;
                    //chkRecordTotal.Checked = false;
                    if (subT.SaveResult)
                    {
                        chkRecordTotal.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkRecordTotal.CheckState = CheckState.Unchecked;
                    }

                    ShowTab(tabPageCommand1, true);
                    ShowTab(tabPageCommand2, false);
                    ShowTab(tabPageCommand3, false);
                    ShowTab(tabPageCommand4, false);
                    ShowTab(tabpageMeasure, true);
                    CurCmdList = Global.Prj.TestSteps[i].SubTest[j].Commands;
                    CurSubTS = Global.Prj.TestSteps[i].SubTest[j];
                    tabpageMeasure.Text = "测试结果比对";
                }
            }
            else
            {
                txtDescription.Text = "";
                txtErrorNum.Text = "";
                groupBox6.Enabled = false;

            }
            UpdateCmdList();
            UpdateMeasure();
        }
        private void treeTS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //TreeView view = (TreeView)sender;
            if (e.Node.Level == 0 && e.Node.Checked == false)
            {
                e.Node.Checked = true;

            }

            if (e.Node.Name.StartsWith("TS"))
            {
                Global.Prj.TestSteps[e.Node.Index].Enable = e.Node.Checked;
            }
            else if (e.Node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[e.Node.Parent.Index].SubTest[e.Node.Index].Enable = e.Node.Checked;
            }

        }
        private void treeTS_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CurMenuCtr = treeTS;
            cmMenuRename.Visible = true;
            treeTS.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                //设置菜单文字
                if (e.Node.Name == "TestSteps")
                {
                    cmMenuAdd.Text = "添加测试项目";
                }
                else if (e.Node.Name.StartsWith("TS"))
                {
                    cmMenuAdd.Text = "添加测试子项";
                }

                //菜单可见性设置
                foreach (ToolStripItem m in treeMenu.Items)
                {
                    m.Enabled = true;
                }

                if (e.Node.Level == 0)
                {
                    foreach (ToolStripItem m in treeMenu.Items)
                    {
                        m.Visible = false;
                    }
                    if (e.Node.Name == "TestSteps")
                    {
                        cmMenuAdd.Visible = true;
                        cmMenuPaste.Visible = true;
                    }
                }
                else
                {
                    cmMenuUp.Enabled = true;
                    cmMenuDown.Enabled = true;
                    if (e.Node == e.Node.Parent.LastNode)
                    {
                        cmMenuDown.Enabled = false;
                    }
                    if (e.Node == e.Node.Parent.FirstNode)
                    {
                        cmMenuUp.Enabled = false;
                    }
                    foreach (ToolStripItem m in treeMenu.Items)
                    {
                        m.Visible = true;
                    }
                    if (e.Node.Name.StartsWith("SubTS"))
                    {
                        cmMenuAdd.Visible = false;
                    }
                }

            }

        }
        private void treeTS_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            TreeNode node = treeTS.SelectedNode;

            if (e.Label == null)
            {
                treeTS.LabelEdit = false;
                return;
            }

            foreach (TreeNode snode in e.Node.Parent.Nodes)
            {
                if (snode.Text == e.Label)
                {
                    MessageBox.Show("[" + e.Label + "] 已经存在！", "Warning", MessageBoxButtons.OK);
                    e.CancelEdit = true;
                }
            }

            if (e.Label == "")
            {
                MessageBox.Show("节点不能为空！", "Warning", MessageBoxButtons.OK);
                e.CancelEdit = true;
            }

            if (node.Name.StartsWith("TS"))
            {
                Global.Prj.TestSteps[node.Index].Name = e.Label;
            }
            else if (node.Name.StartsWith("SubTS"))
            {
                Global.Prj.TestSteps[node.Parent.Index].SubTest[node.Index].Name = e.Label;
            }
            treeTS.LabelEdit = false;
        }
        Point Position = new Point();
        private void treeTS_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                TreeNode myNode;
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
                Position.X = e.X;
                Position.Y = e.Y;
                Position = treeTS.PointToClient(Position);
                TreeNode DropNode = treeTS.GetNodeAt(Position);
                if (myNode.Parent.Level == DropNode.Parent.Level && myNode.Parent.Index == DropNode.Parent.Index)
                {
                    treeViewOperator.InsertSameParent(myNode, DropNode);
                }
                if (myNode.Parent.Level == DropNode.Parent.Level && myNode.Parent.Index != DropNode.Parent.Index)
                {
                    treeViewOperator.InsertdiffrentParent(myNode, DropNode);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("error!");
            }


        }

        private void treeTS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                cmMenuCopy_Click(this,null);
            }
            if (e.KeyCode == Keys.V && e.Control)
            {
                cmMenuPaste_Click(this,null);
            }

            if (e.KeyCode == Keys.X && e.Control)
            {
                cmMenuCut_Click(this,null);
            }
            if (e.KeyCode == Keys.Delete)
            {
                cmMenuDelete_Click(this,null);
            }
        }

        private void datCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                cmMenuCopy_Click(this, null);
            }
            if (e.KeyCode == Keys.V && e.Control)
            {
                cmMenuPaste_Click(this, null);
            }

            if (e.KeyCode == Keys.X && e.Control)
            {
                cmMenuCut_Click(this, null);
            }
            if (e.KeyCode == Keys.Delete)
            {
                cmMenuDelete_Click(this, null);
            }
        }

        private void datCompare_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void datCommand_MouseLeave(object sender, EventArgs e)
        {
            this.groupBox6.Focus();
           // datCommand.EndEdit();
        }

        private void datCompare_MouseLeave(object sender, EventArgs e)
        {
            this.groupBox6.Focus();
            //datCompare.EndEdit();
        }

        private void datMeasure_MouseLeave(object sender, EventArgs e)
        {
            this.groupBox6.Focus();
         //   datMeasure.EndEdit();

        }

        private void datParameter_MouseLeave(object sender, EventArgs e)
        {
            this.groupBox6.Focus();
           // datParameter.EndEdit();
           
        }

        private void 多线程配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMutilTestConfig fmc = new frmMutilTestConfig();
            fmc.ShowDialog(this);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}








