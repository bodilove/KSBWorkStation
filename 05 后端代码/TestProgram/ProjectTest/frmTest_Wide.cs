using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.IO;
using Test.Library;
using Test.Common;

namespace Test.ProjectTest
{
    public enum TestKind
    {
        NormalTest = 0,
        Analysis = 1,
        FirstTest = 2
    }

    public enum QueryFromSever
    {
        NULL = 0,
        PASS = 1,
        FAIL = 2
    }

    public partial class frmTest_Wide : Form
    {
        public Project Prj = null;
        private bool mQuit = false;
        private bool mStart = true;
        private int PassCount = 0;
        private int FailCount = 0;
        private int ID = 0;
        private int DayID = 0;
        private DateTime Date;
        //private double PPM = 0;
        private bool bPPM = true;
        /// <summary>
        /// 点检SN
        /// </summary>
        private string SN = "";
        private string BoxNum = "";
        private string StationNo = "Station_02";
        private string StationName = "烧录";
        private string M_Data = "";
        private string errCode = "";
        private TestKind testKind = TestKind.NormalTest;
        int runtimes = 0;
     
        Dictionary<string, int> DictionaryRecord = new Dictionary<string, int>();
        List<SingleSteps> SingleStepList = new List<SingleSteps>();
        public User mUser = new User();

        private delegate void MessageDelegate(string Message);
        private delegate void SleepDelegate(long second);
        private delegate void TSDataDelegate(int a, string b);
        private delegate void SubTSDataDelegate(int a, int b, SubTestStep subT);
        private delegate void ResetTSDataDelegate();


        Thread InitThread;
        Thread TestThread;
        private string Lock = "";
        private bool IsBreakOnErrorEnable = true;
        private bool BreakOnError = true;
        //private bool BreakOnError = false;
        //Mutex mutex;

        public SqlConnection m_dbConnection = null;
        //private int id = 0;

        public frmTest_Wide()
        {
            InitializeComponent();
        }

        public void RunTest(string ProjectFile, bool Right)
        {
            Prj = new Project();
            Prj.File = ProjectFile;
            Prj.Load();
            statusUser.Text = "用户名：" + mUser.Name + "[" + mUser.Description + "]";
            IsBreakOnErrorEnable = Right;
            this.ShowDialog();
        }

        #region 自动切换序列
        /// <summary>
        /// 测试序列集合
        /// </summary>
        Dictionary<int, Project> dicProjectLst = new Dictionary<int, Project>();

        bool IsCanTest = false;

        /// <summary>
        /// 屏蔽箱是不是空
        /// </summary>
        bool BoxIsFull = false;

        Project CurrentPrj = null;

        bool IsPowerOnTest = false;

        bool IsCanAbortThread = false;

        public Thread BufferHandleThread = null;

      

        Dictionary<int, string> DicPathlist = null;
        /// <summary>
        /// 总记录
        /// </summary>
        Station_ProductInfo station_ProductInfo = null;




        /// <summary>
        /// 主站选生产产品，子站来直接运行
        /// </summary>
        public void RunTest_WideInit(Dictionary<int, string> dicPathlist)
        {
            //MdlClass.webserver.callbackObject._setTestSeqcallbacked += SetTestSquece;
            DicPathlist = dicPathlist;
            //try
            //{
            //    //把字典里的项目全部读到内存里
            //    foreach (KeyValuePair<int, string> kp in dicPathlist)
            //    {
            //        Project prj = new Project();
            //        prj.File = kp.Value;
            //        prj.Load();
            //        dicProjectLst.Add(kp.Key, prj);
            //    }

            //    //Prj = new Project();
            //    //Prj.File = ProjectFile;
            //    //Prj.Load();
            //    //statusUser.Text = "用户名：" + mUser.Name + "[" + mUser.Description + "]";
            //    //IsBreakOnErrorEnable = Right;
            this.ShowDialog();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

        }

        /// <summary>
        /// 程序启动完毕准备，界面的刷新，仪器仪表的初始化
        /// </summary>
        void ReadyForTest()
        {
            //   int id = MdlClass.webserver.master.GetCurrentFOBTestMode();



            // MdlClass.webserver.sss.ClientIsBusy = 1;
            //获得一个标准的序列，仪器仪表每个序列要保持一致
            //GetCurrentPrj();
            this.Prj = new Project();
            this.Prj.File = ProjectFile;
            // ProjectFileName = ProjectFile;
            Prj.Load();
            //得到最新的序列
            // GetCurrentPrj();

            //刷新界面
            ProcessUp();

            //重置表单
            CleanAndSetTable();

            //初始化仪器仪表
            InitThread = new Thread(new ThreadStart(Init));
            InitThread.Start();

            //测试线程开始
            TestThread = new Thread(new ThreadStart(Test));
            TestThread.Start();
        }

        /// <summary>
        /// 进度条，界面刷新
        /// </summary>
        void ProcessUp()
        {
            //MdlClass.labelMessage = this.lblMessage;

            //设置进度条信息
            this.Progress.Minimum = 0;
            this.Progress.Step = 1;
            this.Progress.Value = 0;

            //加载项目信息
            this.Text = "UTOP - " + DateTime.Now.ToShortDateString();
            this.lblTitle.Text = Prj.Description + " - " + Prj.PartNumber
                + " - Ver" + Prj.Version;
            //btnDate.Text = "当前日期：" + DateTime.Now.ToLongDateString();
            //btnStart.Visible = false;
            //btnStop.Visible = false;

            chkBreakOnError.Enabled = IsBreakOnErrorEnable;
            if (Prj.IsAnalyse)
            {
                ckbAnalysis.Checked = true;
            }
            else
            {
                ckbNormalTest.Checked = true;
            }
        }

        /// <summary>
        /// 表格的刷新
        /// </summary>
        void CleanAndSetTable()
        {
            DataGrid.Hide();

            //设置测试结果表格
            UltraGridLayout Layout = DataGrid.DisplayLayout;

            while (Layout.Rows.Count > 0)
            {
                UltraGridRow r = Layout.Rows[0];
                r.Delete(false);
            }


            foreach (TestStep ts in Prj.TestSteps)
            {
                UltraGridRow r = Layout.Bands[0].AddNew();

                r.Cells["StepNr"].Value = ts.Name;
                if (ts.Enable)
                {
                    r.Cells["Test Result"].Value = "未测试";
                }
                else
                {
                    r.Cells["Test Result"].Value = "不测试";
                }
                r.Cells["Description"].Value = ts.Description;
                foreach (SubTestStep subT in ts.SubTest)
                {
                    UltraGridRow subr = Layout.Bands[1].AddNew();
                    subr.Cells["StepNr"].Value = subT.Name;
                    if (subT.Enable && ts.Enable)
                    {
                        subr.Cells["Test Result"].Value = "未测试";
                    }
                    else
                    {
                        subr.Cells["Test Result"].Value = "不测试";
                    }
                    subr.Cells["Expected Value"].Value = subT.DesignedValueString();
                    subr.Cells["Unit"].Value = subT.Unit;
                    subr.Cells["ErrorCode"].Value = subT.ErrorCode;
                    subr.Cells["Description"].Value = subT.Description;
                }
            }

            DataGrid.Show();
        }

        /// <summary>
        /// 测试退出
        /// </summary>
        void TestQuit()
        {

        }

        /// <summary>
        /// 关闭测试线程和初始化线程
        /// </summary>
        void CloseAllth()
        {
            try
            {
                ShowMessage("等待初始化线程结束。");

                do
                {
                    if (InitThread != null && !InitThread.IsAlive)
                    {
                        break;
                    }
                    if (InitThread == null)
                    {
                        break;
                    }
                    Thread.Sleep(50);
                    Application.DoEvents();
                } while (true);


                //mQuit = true;
                if (TestThread != null)
                {
                    TestThread.Abort();
                    TestThread.Join(500);
                    ShowMessage("正在关闭测试线程。");
                    Application.DoEvents();
                }
                //Prj.ExitTest();
                if (!mQuit)
                {
                    mQuit = true;
                    Prj.ExitTest();
                }
                do
                {
                    if (!TestThread.IsAlive)
                    {
                        break;
                    }
                    Thread.Sleep(5);
                    Application.DoEvents();
                } while (true);
                mQuit = false;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void WaitProduct()
        {
            while (true)
            {

                if (MdlClass.readplc.CheckProduct)
                {
                    if (true)
                    {
                        //if (MdlClass.readplc)
                        //{
                        //    //IsCanTest = true;
                        //    break;
                        //}
                        //else
                        //{
                        //    ShowMessage("请按启动按钮");
                        //}
                    }
                    else
                    {
                        ShowMessage("请双手离开光栅");
                    }
                }
                else
                {
                    ShowMessage("请放入产品");
                }

            }
        }

        public TestPowerOn SNtp(string SN)
        {
            foreach (TestPowerOn tps in tpm.PowerOnLst)
            {
                if (tps.ProductSN == SN)
                {
                    return tps;

                }
            }

            return null;
        }



        /// <summary>
        /// 处理线体buffer的逻辑
        /// </summary>
        //void BufferHandleLogic()
        //{

        //    //if(CurrentPrj)
        //    //{}
        //    #region 初始化
        //    do
        //    {
        //        Thread.Sleep(10);
        //    } while (Lock == "Start");
        //    if (Lock == "ERROR") return;
        //    MdlClass.IsTestPowerON = Prj.IsStartTest;
        //    System.Threading.Thread.Sleep(1000);

        //    #endregion

        //    //do
        //    //{
        //    //    Thread.Sleep(50);
        //    //} while (tpm == null);
        //    do
        //    {
        //        if (MdlClass.TcpipServer.RobotIsReady())
        //        {
        //            break;
        //        }
        //        Thread.Sleep(300);
        //        ShowMessage("正在等待机械手到达初始位置");
        //    }while(true);
        //     //把夹具初始化一遍
        //     //PIN下降
        //     //Z轴抬起
        //    MdlClass.readplc.Z1_OrgSet();
        //    MdlClass.readplc.ShieldBoxDoorOpenSet();

        //    ShowMessage("检查产品是否有遗留。");
        //   // Thread.Sleep(2000);

        //        if (MdlClass.readplc.CheckProduct)
        //        {
        //            ShowMessage("有产品遗留。");
        //            if (MdlClass.TcpipServer.MyStationState.CurrentUUT == null)
        //            {
        //                Common.RobotServer.UUT uut = new Common.RobotServer.UUT();
        //                uut.StationID = MdlClass.TcpipServer.MyStationState.StationID;
        //                uut.Tested = true;
        //                uut.TestRes = false;
        //                uut.UUTSN = "LeftUUT";
        //                uut.LostStation = uut.StationID;
        //                MdlClass.TcpipServer.callbackObject.PutUUT(uut);
        //                MdlClass.TcpipServer.MyStationState.CurrentUUT.Tested = true;
        //                MdlClass.TcpipServer.MyStationState.CurrentUUT.TestRes = false;
        //                MdlClass.TcpipServer.MyStationState.CurrentUUT.LostStation = 3;
        //                MdlClass.TcpipServer.MyStationState.PreNeedProduct = true;
        //                MdlClass.TcpipServer.MyStationState.NeedProduct = true;
        //            }
        //        }

        //        MdlClass.TcpipServer.StationIsReady = true;
        //        MdlClass.TcpipServer.NeedUUT();


        //    do
        //    {
        //        #region 测试模式下的逻辑

        //        if(!IsCanTest)
        //        {
        //            if(MdlClass.readplc.CheckProduct && MdlClass.TcpipServer.MyStationState.CurrentUUT != null && !MdlClass.TcpipServer.MyStationState.CurrentUUT.Tested)
        //            {
        //                if (MdlClass.TcpipServer.MyStationState.CurrentUUT.UUTSN != "LeftUUT")
        //                {
        //                    ShowMessage("收到新产品，可以测试");
        //                    MdlClass.TcpipServer.NotNeedUUT();
        //                    SN = MdlClass.TcpipServer.MyStationState.CurrentUUT.UUTSN;
        //                    BoxNum = MdlClass.TcpipServer.MyStationState.CurrentUUT.BoxNum;
        //                    this.IsCanTest = true;
        //                }
        //            }
        //            else if(MdlClass.readplc.CheckProduct && MdlClass.TcpipServer.MyStationState.CurrentUUT != null && MdlClass.TcpipServer.MyStationState.CurrentUUT.Tested)
        //            {
        //                  ShowMessage("已经测试完的产品等待取出");
        //                 // MdlClass.TcpipServer.NeedUUT();
        //            }
        //            else
        //            {
        //                ShowMessage("无产品");
        //            }
        //        }
        //        else
        //        {
        //             ShowMessage("Testing...........");
        //        }

        //            //if (MdlClass.readplc.CheckProduct && MdlClass.TcpipServer.MyStationState.CurrentUUT != null && !MdlClass.TcpipServer.MyStationState.CurrentUUT.Tested && !this.IsCanTest)
        //            //{
        //            //    ShowMessage("ProductIsNull");
        //            //    MdlClass.TcpipServer.NotNeedUUT();
        //            //    //可以进入测试
        //            //    Console.WriteLine("jinru .........");
        //            //    this.IsCanTest = true;
        //            //}
        //            //else if (this.IsCanTest)
        //            //{
        //            //    Console.WriteLine("Testing...........");
        //            //    ShowMessage("Testing...........");
        //            //}
        //            //else if()
        //            //{

        //            //}
        //            //else
        //            //{
        //            //    ShowMessage("ProductIsTestedNeed");

        //            //}
        //        //}
        //        #endregion

        //        System.Threading.Thread.Sleep(200);
        //    } while (true);
        //}

        private void Test1()
        {
            Stopwatch TestTimer = new Stopwatch();
            //mutex.WaitOne();
            do
            {
                Thread.Sleep(5);
            } while (Lock == "Start");
            if (Lock == "ERROR") return;

            int nTestItems = 0;
            try
            {
                foreach (TestStep ts in Prj.TestSteps)
                {
                    if (!ts.Enable) continue;
                    foreach (SubTestStep subt in ts.SubTest)
                    {
                        if (!subt.Enable) continue;
                        nTestItems += 1;
                    }
                }

                this.Invoke(new SetProgressMaxDelegate(mySetProgressMax), new object[] { nTestItems });
                //SetProperty(ref Progress.Maximum,nTestItems);

                //Perform Test
                do
                {
                    SingleStepList.Clear();
                    this.Invoke(new ResetTSDataDelegate(tblEnableTrue));
                    // Thread.Sleep(5000);
                    int TSIndex = 0;
                    int n = 0;
                    bool TotalRes = true;

                    SN = CreateSN();

                    CreateWCFUUT();
                    //SendWCFUUT();


                    Prj.FailureStep = null;
                    runtimes = runtimes + 1;
                    foreach (Command c in Prj.PreTest)
                    {
                        c.Execute();
                    }

                    this.Invoke(new ResetTSDataDelegate(ResetTestData),
                        new object[] { });
                    this.Invoke(new TestResultDelegate(UpdateTestResult),
                        new object[] { "Testing...", Color.Gray });

                    TestTimer.Reset();
                    TestTimer.Restart();
                    this.Invoke(new MessageDelegate(SetBtnText),
                        new object[] { "测试中..." });
                    this.Invoke(new ResetTSDataDelegate(tblEnableFalse));
                    if (ckbAnalysis.Checked == true)
                    {
                        testKind = TestKind.Analysis;
                    }
                    //if(ckbFirstTest.Checked ==true)
                    //{
                    //    
                    //}
                    if (ckbNormalTest.Checked == true)
                    {
                        testKind = TestKind.NormalTest;
                    }
                    foreach (TestStep ts in Prj.TestSteps)
                    {
                        bool TSres = true;
                        TSIndex++;
                        if (!ts.Enable) continue;

                        for (int j = 0; j <= ts.RetryCount; j++)
                        {
                            TSres = true;
                            n = 0;
                            foreach (Command c in ts.PreTest)
                            {
                                c.Execute();
                            }

                            this.Invoke(new TSDataDelegate(UpdateTSData),
                                new object[] { TSIndex - 1, "测试中" });

                            int subTSIndex = 0;
                            bool Jumpto99 = false;
                            foreach (SubTestStep subt in ts.SubTest)
                            {
                                subTSIndex++;
                                if (!subt.Enable)
                                {

                                    continue;
                                }

                                if (StepLst.Contains(subt.Name))
                                {
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData2),
                                                 new object[] { TSIndex - 1, subTSIndex - 1, subt });
                                    continue;
                                }
                                this.ShowStatus(subt.Name + ": " + subt.Description);
                                this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                                bool res = subt.Test();
                                this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                    new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                if (subt.SaveResult)
                                {
                                    //SaveResult(ts, subt);
                                    SingleSteps s = singleObj(ts, subt);
                                    this.SingleStepList.Add(s);
                                }

                                if (!res)
                                {
                                    Prj.FailureStep = subt;
                                    errCode = subt.ErrorCode;
                                }
                                TSres &= res;
                                if (!res)
                                {
                                    //DictionaryRecord[subt.Name] = DictionaryRecord[subt.Name] + 1;
                                }
                                if (!res && BreakOnError)
                                {
                                    if (!subt.Name.Contains(".99"))
                                    {
                                        Jumpto99 = true;
                                    }
                                    break;
                                }
                            }
                            if (Jumpto99)
                            {
                                #region 跳到jumpto99
                                SubTestStep subt = ts.SubTest[ts.SubTest.Count - 1];
                                if (subt.Name.Contains(".99"))
                                {
                                    subTSIndex = ts.SubTest.Count;
                                    if (!subt.Enable) continue;
                                    this.ShowStatus(subt.Name + ": " + subt.Description);
                                    this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                                    bool res = subt.Test();
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                        new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                    if (subt.SaveResult)
                                    {
                                        //SaveResult(ts, subt);
                                        SingleSteps s = singleObj(ts, subt);
                                        this.SingleStepList.Add(s);
                                    }

                                    if (!res)
                                    {
                                        Prj.FailureStep = subt;
                                        errCode = subt.ErrorCode;
                                    }
                                    TSres &= res;
                                    if (!res)
                                    {
                                        //DictionaryRecord[subt.Name] = DictionaryRecord[subt.Name] + 1;
                                    }
                                    if (!res && BreakOnError)
                                    {
                                        break;
                                    }
                                }

                                #endregion
                            }
                            foreach (Command c in ts.PostTest)
                            {
                                c.Execute();
                            }
                            if (TSres) break;
                        }


                        if (TSres)
                        {
                            ts.status = TestStatus.Pass;
                        }
                        else
                        {
                            ts.status = TestStatus.Fail;
                            DictionaryRecord["FailTimes"] = DictionaryRecord["FailTimes"] + 1;
                        }

                        //更改UTT
                        UpdateWCFUUTStatus(ts);

                        TotalRes &= TSres;
                        this.Invoke(new TSDataDelegate(UpdateTSData),
                            new object[] { TSIndex - 1, TSres ? "Pass" : "Fail" });
                        if (!TSres && BreakOnError) break;
                    }

                    string testtime = ((int)TestTimer.Elapsed.TotalMinutes).ToString() + "分";
                    testtime += TestTimer.Elapsed.Seconds.ToString() + "秒";
                    this.Invoke(new MessageDelegate(SetBtnText),
                             new object[] { testtime });
                    TestTimer.Stop();

                    foreach (Command c in Prj.PostTest)
                    {
                        c.Execute();
                    }

                    if (TotalRes)
                    {
                        foreach (Command c in Prj.SucTest)
                        {
                            c.Execute();
                        }
                        this.PassCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "PASS", Color.Green });
                    }
                    else
                    {
                        foreach (Command c in Prj.FailTest)
                        {
                            c.Execute();
                        }
                        this.FailCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "FAIL", Color.Red });
                    }
                    this.Invoke(new UpdateCounterDelgate(UpdateCounter),
                            new object[] { });
                    //foreach (Command c in Prj.PostTest)
                    //{
                    //    c.Execute();
                    //}
                    if (testKind == TestKind.NormalTest)
                    {
                        SaveAllResult1(this.SingleStepList);
                    }
                    System.Threading.Thread.Sleep(500);
                    //发送UTT

                } while (true);
            }
            catch (Exception e)
            {
                this.ShowMessage(e.Message);            ////
                if (e.InnerException == null)
                {
                    this.ShowStatus(e.Message);
                }
                else
                {
                    this.ShowStatus(e.InnerException.Message);
                }
                if (!mQuit)
                {
                    Prj.ExitTest();
                }
            }
        }

        #region 点检配置新
        /// <summary>
        /// 检查点检序列，并且根据点检序列重载测试序列
        /// </summary>
        /// <param name="tp"></param>
        public void CheckAndReloadPrjStepTest(TestPowerOn tp)
        {
            for (int i = 0; i < Prj.TestSteps.Count; i++)
            {
                for (int j = 0; j < Prj.TestSteps[i].SubTest.Count; j++)
                {
                    TestPowerSubTestStep tpsts = CheckTestPowerSubTestStep(Prj.TestSteps[i].SubTest[j].Name, tp);
                    if (tpsts != null)
                    {
                        if (tpsts.Ischecked)
                        {
                            Prj.TestSteps[i].SubTest[j].LLimit = tpsts.LLimit;
                            Prj.TestSteps[i].SubTest[j].ULimit = tpsts.ULimit;
                            Prj.TestSteps[i].SubTest[j].NomValue = tpsts.NomValue;
                            Prj.TestSteps[i].SubTest[j].Enable = tpsts.Enable;
                        }
                    }
                }
            }
            this.Invoke(new Action(() =>
            {
                CleanAndSetTable();
            }));
        }

        /// <summary>
        /// 恢复原来序列
        /// </summary>
        public void RSTPrj(TestPowerOn tp, string partnum)
        {
            //Project prj = new Project();
            //prj.File = Prj.File;
            //prj.Load();
            //for (int i = 0; i < Prj.TestSteps.Count; i++)
            //{
            //    for (int j = 0; j < Prj.TestSteps[i].SubTest.Count; j++)
            //    {

            //        Prj.TestSteps[i].SubTest[j].LLimit = prj.TestSteps[i].SubTest[j].LLimit;
            //        Prj.TestSteps[i].SubTest[j].ULimit = prj.TestSteps[i].SubTest[j].ULimit;
            //        Prj.TestSteps[i].SubTest[j].NomValue = prj.TestSteps[i].SubTest[j].NomValue;
            //        Prj.TestSteps[i].SubTest[j].Enable = prj.TestSteps[i].SubTest[j].Enable;

            //    }
            //}
            for (int i = 0; i < Prj.TestSteps.Count; i++)
            {
                for (int j = 0; j < Prj.TestSteps[i].SubTest.Count; j++)
                {
                    TestPowerSubTestStep tpsts = CheckTestPowerSubTestStep(Prj.TestSteps[i].SubTest[j].Name, tp);
                    if (tpsts != null)
                    {
                        if (tpsts.Ischecked)
                        {
                            Prj.TestSteps[i].SubTest[j].LLimit = tpsts.OrgSubTestStep.LLimit;
                            Prj.TestSteps[i].SubTest[j].ULimit = tpsts.OrgSubTestStep.ULimit;
                            Prj.TestSteps[i].SubTest[j].NomValue = tpsts.OrgSubTestStep.NomValue;
                            Prj.TestSteps[i].SubTest[j].Enable = tpsts.OrgSubTestStep.Enable;
                        }
                    }
                }
            }
            Prj.PartNumber = partnum;
            this.Invoke(new Action(() =>
            {
                CleanAndSetTable();
            }));
        }

        public TestPowerSubTestStep CheckTestPowerSubTestStep(string SubTestName, TestPowerOn tp)
        {
            foreach (TestPowerSubTestStep tpsts in tp.TestPowerSubTestSteplist)
            {
                if (tpsts.Name == SubTestName)
                {
                    return tpsts;
                }
            }
            return null;
        }
        public void RefreshGridView(Dictionary<string, bool> reslit)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                dgvTestPowerOn.Rows.Clear();
                foreach (KeyValuePair<string, bool> kp in reslit)
                {
                    if (kp.Value)
                    {
                        dgvTestPowerOn.Rows.Add(new object[] { kp.Key, "已通过" });
                        dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[1].Style.ForeColor = Color.Green;
                        //dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[0].Style.ForeColor = Color.LawnGreen;
                        //dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[1].Style.ForeColor = Color.LawnGreen;
                    }
                    else
                    {
                        dgvTestPowerOn.Rows.Add(new object[] { kp.Key, "未通过" });
                        dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[1].Style.ForeColor = Color.Red;
                        //dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[0].Style.ForeColor = Color.Red;
                        //dgvTestPowerOn.Rows[dgvTestPowerOn.Rows.Count - 1].Cells[1].Style.ForeColor = Color.Red;
                    }
                    //    }

                }
            }));
        }
        Dictionary<string, bool> dicRes = null;
        TestPowerOnManage tpm = null;
        TestPowerOn Currenttp = null;
        bool IsCanPowerOnTest = false;
        public void Test_PowerOn_New()
        {
            MdlClass.IsTestPowerON = true;
            //TestPowerOnManage载入

            Stopwatch TestTimer = new Stopwatch();

            //修改测试内容
            string partnum = Prj.PartNumber;
            //弹窗
            int no = 0;
            //运行序列
            //frmPowerOnTest fm = new frmPowerOnTest();
            //fm.Show();
            Application.DoEvents();
            System.Threading.Thread.Sleep(100);
            do
            {

                dicRes = CheckTestPowerOnManageIsPass(tpm, no);
                RefreshGridView(dicRes);
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);

                //刷新弹窗
                if (dicRes.ContainsValue(false))
                {
                    #region 等待进入测试
                    bool IsDone = false;
                    this.IsCanTest = false;

                    #region 等待进入测试
               


                    

                    listTestInfo = new List<Station_TestInfo>();
                    station_ProductInfo = new Station_ProductInfo();
                    station_ProductInfo.SN = DateTime.Now.ToString("yyyyMMdHmmss");
                    station_ProductInfo.StartTime = DateTime.Now.ToString();
                    station_ProductInfo.PrdType = 1;
                    station_ProductInfo.UserID = mUser.Name;//创建人
                    station_ProductInfo.PartNum = Prj.PartNumber;//
                    station_ProductInfo.StationID = "";
                    station_ProductInfo.UUTestId = "";
                    #endregion




                    // CreateWCFUUT();
                    //MdlClass.wcfuut.UUTestId = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //SendWCFUUT();

                    #region 测试前机械动作

                    //  if (!MdlClass.TcpipServer.MyStationState.CurrentUUT.TestRes && MdlClass.TcpipServer.MyStationState.CurrentUUT.NGNam !="NoNG")





                    #endregion



                    if (SN != tpm.PowerOnLst[no].ProductSN)
                    {
                     
                        goto LabelBreak;
                    }
                    else
                    {
                        //赋值sn号
                        station_ProductInfo.SN = SN;
                        Currenttp = tpm.PowerOnLst[no];
                    }

                    CheckAndReloadPrjStepTest(Currenttp);

                    ShowMessage("放入产品SN" + tpm.PowerOnLst[no].ProductSN + "的产品");

                    #region
                    SingleStepList.Clear();
                    this.Invoke(new ResetTSDataDelegate(tblEnableTrue));
                    // Thread.Sleep(5000);
                    int TSIndex = 0;
                    int n = 0;
                    bool TotalRes = true;





                    // new 一个UUT
                    //CreateUUT();
                    //SendUUT();

                    // UUT里面加一个StationtestResult


                    Prj.FailureStep = null;
                    runtimes = runtimes + 1;
                    //MdlClass.QueryProductRes = null;
                    //bool IstoBreakLable = false;

                    foreach (Command c in Prj.PreTest)
                    {
                        c.Execute();
                    }

                    // CreateStation();

                    this.Invoke(new ResetTSDataDelegate(ResetTestData),
                        new object[] { });
                    this.Invoke(new TestResultDelegate(UpdateTestResult),
                        new object[] { "Testing...", Color.Gray });

                    TestTimer.Reset();
                    TestTimer.Restart();
                    this.Invoke(new MessageDelegate(SetBtnText),
                        new object[] { "测试中..." });
                    this.Invoke(new ResetTSDataDelegate(tblEnableFalse));
                    if (ckbAnalysis.Checked == true)
                    {
                        testKind = TestKind.Analysis;
                    }
                    //if(ckbFirstTest.Checked ==true)
                    //{
                    //    
                    //}
                    if (ckbNormalTest.Checked == true)
                    {
                        testKind = TestKind.NormalTest;
                    }
                    foreach (TestStep ts in Prj.TestSteps)
                    {
                        bool TSres = true;
                        TSIndex++;
                        if (!ts.Enable) continue;

                        for (int j = 0; j <= ts.RetryCount; j++)
                        {
                            //ClearCreateWCFSlst();
                            TSres = true;
                            n = 0;
                            foreach (Command c in ts.PreTest)
                            {
                                c.Execute();
                            }

                            this.Invoke(new TSDataDelegate(UpdateTSData),
                                new object[] { TSIndex - 1, "测试中" });

                            int subTSIndex = 0;
                            bool Jumpto99 = false;
                            Stopwatch sp = new Stopwatch();
                            foreach (SubTestStep subt in ts.SubTest)
                            {
                                sp.Restart();
                                subTSIndex++;
                                if (!subt.Enable)
                                {

                                    continue;
                                }

                                if (StepLst.Contains(subt.Name))
                                {
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData2),
                                                 new object[] { TSIndex - 1, subTSIndex - 1, subt });
                                    continue;
                                }
                                this.ShowStatus(subt.Name + ": " + subt.Description);
                                this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                                bool res = subt.Test();
                                this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                    new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                if (subt.SaveResult)
                                {
                                    //SaveResult(ts, subt);
                                    SingleSteps s = singleObj(ts, subt);
                                    this.SingleStepList.Add(s);
                                }

                                if (!res)
                                {
                                    Prj.FailureStep = subt;
                                    errCode = subt.ErrorCode;
                                }
                                TSres &= res;
                                sp.Stop();
                                //创建一个SubTest
                                CreateWCFSubTest1(subt, sp.Elapsed.TotalMilliseconds.ToString("#0.000"));
                                // CreateWCFSubTest(subt);

                                if (!res)
                                {
                                    //DictionaryRecord[subt.Name] = DictionaryRecord[subt.Name] + 1;
                                }
                                if (!res && BreakOnError)
                                {
                                    if (!subt.Name.Contains(".99"))
                                    {
                                        Jumpto99 = true;
                                    }
                                    break;
                                }
                            }
                            if (Jumpto99)
                            {
                                #region 跳到jumpto99
                                SubTestStep subt = ts.SubTest[ts.SubTest.Count - 1];
                                if (subt.Name.Contains(".99"))
                                {
                                    subTSIndex = ts.SubTest.Count;
                                    if (!subt.Enable) continue;
                                    this.ShowStatus(subt.Name + ": " + subt.Description);
                                    this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                                    bool res = subt.Test();
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                        new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                    if (subt.SaveResult)
                                    {
                                        //SaveResult(ts, subt);
                                        SingleSteps s = singleObj(ts, subt);
                                        this.SingleStepList.Add(s);
                                    }

                                    if (!res)
                                    {
                                        Prj.FailureStep = subt;
                                        errCode = subt.ErrorCode;
                                    }
                                    TSres &= res;
                                    if (!res)
                                    {
                                        //DictionaryRecord[subt.Name] = DictionaryRecord[subt.Name] + 1;
                                    }
                                    if (!res && BreakOnError)
                                    {
                                        break;
                                    }
                                }

                                #endregion
                            }
                            foreach (Command c in ts.PostTest)
                            {
                                c.Execute();
                            }
                            if (TSres) break;
                        }


                        if (TSres)
                        {
                            ts.status = TestStatus.Pass;
                        }
                        else
                        {
                            ts.status = TestStatus.Fail;
                            DictionaryRecord["FailTimes"] = DictionaryRecord["FailTimes"] + 1;
                        }
                        TotalRes &= TSres;

                        //Writlst();
                        //更改UUT状态
                        //UpdateWCFUUTStatus(ts);

                        this.Invoke(new TSDataDelegate(UpdateTSData),
                            new object[] { TSIndex - 1, TSres ? "Pass" : "Fail" });
                        if (!TSres && BreakOnError) break;
                    }

                    string testtime = ((int)TestTimer.Elapsed.TotalMinutes).ToString() + "分";
                    testtime += TestTimer.Elapsed.Seconds.ToString() + "秒";
                    this.Invoke(new MessageDelegate(SetBtnText),
                             new object[] { testtime });
                    TestTimer.Stop();


                    //if (CanFindInServerList)
                    //{
                    //    if (!MdlClass.QueryProductRes.Results)
                    //    {
                    //        TotalRes = false;
                    //        CreateStation();
                    //        MdlClass.stationTest.TestResult = Common.WebServer1.TestResultState.NotTest;
                    //    }
                    //}

                    foreach (Command c in Prj.PostTest)
                    {
                        c.Execute();
                    }

                    if (TotalRes)
                    {
                        foreach (Command c in Prj.SucTest)
                        {
                            c.Execute();
                        }
                        this.PassCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "PASS", Color.Green });
                    }
                    else
                    {
                        foreach (Command c in Prj.FailTest)
                        {
                            c.Execute();
                        }
                        this.FailCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "FAIL", Color.Red });
                    }
                    this.Invoke(new UpdateCounterDelgate(UpdateCounter),
                            new object[] { });
                    //foreach (Command c in Prj.PostTest)
                    //{
                    //    c.Execute();
                    //}
                    if (testKind == TestKind.NormalTest)
                    {
                        station_ProductInfo.EndTime = DateTime.Now.ToString();
                        station_ProductInfo.Result = TotalRes.ToString();
                        station_ProductInfo.PrdType = 1;
                        SaveAllResult_New();

                        //SaveAllResult1(this.SingleStepList);
                    }
                    System.Threading.Thread.Sleep(500);

                    //发送一个UUT
                    // SendWCFCheckUUT();

                    //MdlClass.stationTest.EnterTime = EnterTime;
                    //MdlClass.stationTest.LeaveTime = LeaveTime;
                    //SendStationtestResult();

                    //if (IstoBreakLable)
                    //{
                    //    MdlClass.readplc.SetVregister(MdlClass.readplc.VW10_Pallet_Come, false);
                    //    MdlClass.readplc.SetVregister(MdlClass.readplc.VW20_Product_Leave, true);
                    //}





                    #region 测试后机械动作



                    MdlClass.IscanTest = false;
                    //MdlClass.TcpipServer.refreshuut();
                    ShowMessage("跳出测试");

                    #endregion
                    Currenttp.Result = TotalRes;
                    if (Currenttp.Result)
                    {
                        no++;
                    }
                    else
                    {
                        no = 0;
                    }
                    IsDone = true;
                LabelBreak:
                    if (!IsDone)
                    {
                        #region 测试后机械动作


                        //MdlClass.TcpipServer.PreNeedUUT();
                        //MdlClass.readplc.Z1_OrgSet();
                        //MdlClass.readplc.ShieldBoxDoorOpenSet();
                        //lock (TcpIpServerCls.MyStationStateLock)
                        //{
                        //    MdlClass.TcpipServer.MyStationState.CurrentUUT.Tested = true;
                        //    MdlClass.TcpipServer.MyStationState.CurrentUUT.TestRes = false;
                        //    MdlClass.TcpipServer.MyStationState.CurrentUUT.StationID = MdlClass.CurrentstationID;


                        //    MdlClass.TcpipServer.MyStationState.CurrentUUT.NGNam = ngNAME;
                        //    MdlClass.TcpipServer.MyStationState.CurrentUUT.LostStation = MdlClass.TcpipServer.MyStationState.StationID;

                        //}
                        #endregion
                        MdlClass.IscanTest = false;
                        //MdlClass.TcpipServer.refreshuut();
                        ShowMessage("跳出测试");
                    }
                    else
                    {
                        IsDone = false;
                    }
                    #endregion
                    MdlClass.PowerOnIsOK = false;
                }
                else
                {
                    MdlClass.PowerOnIsOK = true;
                    break;
                    ////如果是正确的，不测试，跳出开班测试。
                    //if (MessageBox.Show("点检结束，是否进行正常测试", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //    break;
                    //}
                    //else
                    //{
                    //    //TestPowerOnManage载入
                    //    tpm = new TestPowerOnManage();
                    //    TestTimer = new Stopwatch();
                    //    tpm.Load(Prj.InitFileName);
                    //    //修改测试内容
                    //    //MdlClass.tp = tpm;
                    //    ////弹窗
                    //    no = 0;
                    //    //MdlClass.no = 0;
                    //    //运行序列
                    //    //fm = new frmPowerOnTest();
                    //    //displayPoweron = new Thread(DispalayPowerON);
                    //    //displayPoweron.Start();

                    //    Application.DoEvents();
                    //    System.Threading.Thread.Sleep(100);
                    //}
                }


            } while (true);
            //fm.Close();
            RSTPrj(tpm.PowerOnLst[0], partnum);
            MdlClass.IsTestPowerON = false;
        }

        Dictionary<string, bool> CheckTestPowerOnManageIsPass(TestPowerOnManage tpm, int no)
        {
            if (no == 0)
            {
                for (int i = 0; i < tpm.PowerOnLst.Count; i++)
                {
                    tpm.PowerOnLst[i].Result = false;
                }
            }
            Dictionary<string, bool> DicPowerOnTest = new Dictionary<string, bool>();
            foreach (TestPowerOn tp in tpm.PowerOnLst)
            {
                DicPowerOnTest[tp.Name] = tp.Result;
            }
            return DicPowerOnTest;
        }

        

                    #endregion
        /// <summary>
        /// 点检测试，根据数据库中的点检记录判断是否需要测试
        /// </summary>
        void TestPowerOnStart()
        {
            
            try
            {

                string Result = "Pass";
                DataTable dtPowerOnTest = null;
                string sql = "select * from PowerOnTest where Convert(varchar(10),BeginDate,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and PartNum='" + this.Prj.PartNumber + "'";

                dtPowerOnTest = Common.SQLBAL.GetDataTable(sql);
                if (dtPowerOnTest != null)
                {
                    if (dtPowerOnTest.Rows.Count == 0)
                    {
                        MessageBox.Show("今天还没有进行点检！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        sql = "insert into PowerOnTest(PartNum,BeginDate) values('" + this.Prj.PartNumber + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        Common.SQLBAL.UpdateBySql(sql);
                        //进行点检
                        Test_PowerOn_New();
                        //点检结束
                        sql = "update PowerOnTest set EndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',Result='" + Result + "' where Convert(varchar(10),BeginDate,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and PartNum='" + this.Prj.PartNumber + "'";

                        Common.SQLBAL.UpdateBySql(sql);
                        MessageBox.Show("点检完毕！");

                    }
                    else if (dtPowerOnTest.Rows.Count > 0)
                    {
                        if (dtPowerOnTest.Rows[0]["Result"].ToString() == "Pass" || dtPowerOnTest.Rows[0]["Result"].ToString() == "Fail")
                        {
                            if (MessageBox.Show("今天已进行点检，是否重新点检！", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                Test_PowerOn_New();
                                sql = "update PowerOnTest set EndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',Result='" + Result + "' where Convert(varchar(10),BeginDate,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and PartNum='" + this.Prj.PartNumber + "'";

                                Common.SQLBAL.UpdateBySql(sql);
                                MessageBox.Show("点检完毕！");

                            }
                        }
                        else
                        {
                            MessageBox.Show("需要重新点检！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            Test_PowerOn_New();
                            sql = "update PowerOnTest set EndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',Result='" + Result + "' where Convert(varchar(10),BeginDate,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and PartNum='" + this.Prj.PartNumber + "'";

                            Common.SQLBAL.UpdateBySql(sql);
                            MessageBox.Show("点检完毕！");

                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            Test_PowerOn_New();
        }



        private void Test()
        {
            if (Prj.IsStartTest)
            {
                tpm = new TestPowerOnManage();
                tpm.Load(Prj.InitFileName);
            }
            MdlClass.IsTestPowerON = false;
            Stopwatch TestTimer = new Stopwatch();
            //mutex.WaitOne();
            do
            {
                Thread.Sleep(5);
            } while (Lock == "Start");
            if (Lock == "ERROR") return;

            int nTestItems = 0;
            try
            {
                #region 开机测试
                if (Prj.IsStartTest)
                {
                    this.Invoke(new MethodInvoker(delegate() { panel1.Visible = true; }));
                    this.Invoke(new MethodInvoker(delegate() { dgvTestPowerOn.Visible = true; }));
                    this.Invoke(new MethodInvoker(delegate() { tableLayoutPanel1.ColumnCount = 2; }));

                    //lblTestPowerOnTitle.Enabled = true;
                    //dgvTestPowerOn.Enabled = true;

                    TestPowerOnStart();
                    this.Invoke(new MethodInvoker(delegate() { panel1.Visible = false; }));
                    this.Invoke(new MethodInvoker(delegate() { dgvTestPowerOn.Visible = false; }));
                    this.Invoke(new MethodInvoker(delegate() { tableLayoutPanel1.ColumnCount = 1; }));
                }
                else
                {
                    this.Invoke(new MethodInvoker(delegate() { panel1.Visible = false; }));
                    this.Invoke(new MethodInvoker(delegate() { dgvTestPowerOn.Visible = false; }));
                    this.Invoke(new MethodInvoker(delegate() { tableLayoutPanel1.ColumnCount = 1; }));
                }
                #endregion
                foreach (TestStep ts in Prj.TestSteps)
                {
                    if (!ts.Enable) continue;
                    foreach (SubTestStep subt in ts.SubTest)
                    {
                        if (!subt.Enable) continue;
                        nTestItems += 1;
                    }
                }

                this.Invoke(new SetProgressMaxDelegate(mySetProgressMax), new object[] { nTestItems });
                //SetProperty(ref Progress.Maximum,nTestItems);

                //Perform Test
                do
                {
                    SingleStepList.Clear();
                    this.Invoke(new ResetTSDataDelegate(tblEnableTrue));
                    // Thread.Sleep(5000);
                    int TSIndex = 0;
                    int n = 0;
                    bool TotalRes = true;
                    //SN=   MdlClass.TcpipServer.MyStationState.CurrentUUT.UUTSN;
                    #region 等待进入测试
                    this.IsCanTest = false;

                    listTestInfo = new List<Station_TestInfo>();
                    station_ProductInfo = new Station_ProductInfo();
                    station_ProductInfo.SN = DateTime.Now.ToString("yyyyMMdHmmss");
                    station_ProductInfo.StartTime = DateTime.Now.ToString();
                    station_ProductInfo.PrdType = 2;
                    station_ProductInfo.UserID = mUser.Name;//创建人
                    station_ProductInfo.PartNum = Prj.PartNumber;//
                    station_ProductInfo.StationID = "";
                    station_ProductInfo.UUTestId = "";

                    #endregion


                    #region 测试前机械动作


                    if(mUser.Description == "")

                    #endregion



                    Prj.FailureStep = null;
                    runtimes = runtimes + 1;
                    //MdlClass.QueryProductRes = null;
                    //bool IstoBreakLable = false;

                    foreach (Command c in Prj.PreTest)
                    {
                        c.Execute();
                    }

                    // CreateStation();

                    this.Invoke(new ResetTSDataDelegate(ResetTestData),
                        new object[] { });
                    this.Invoke(new TestResultDelegate(UpdateTestResult),
                        new object[] { "Testing...", Color.Gray });

                    TestTimer.Reset();
                    TestTimer.Restart();
                    this.Invoke(new MessageDelegate(SetBtnText),
                        new object[] { "测试中..." });
                    this.Invoke(new ResetTSDataDelegate(tblEnableFalse));
                    if (ckbAnalysis.Checked == true)
                    {
                        testKind = TestKind.Analysis;
                    }
                    //if(ckbFirstTest.Checked ==true)
                    //{
                    //    
                    //}
                    if (ckbNormalTest.Checked == true)
                    {
                        testKind = TestKind.NormalTest;
                    }
                    foreach (TestStep ts in Prj.TestSteps)
                    {
                        bool TSres = true;
                        TSIndex++;
                        if (!ts.Enable) continue;

                        for (int j = 0; j <= ts.RetryCount; j++)
                        {
                            //ClearCreateWCFSlst();
                            TSres = true;
                            n = 0;
                            foreach (Command c in ts.PreTest)
                            {
                                c.Execute();
                            }

                            this.Invoke(new TSDataDelegate(UpdateTSData),
                                new object[] { TSIndex - 1, "测试中" });

                            int subTSIndex = 0;
                            bool Jumpto99 = false;
                            Stopwatch sp = new Stopwatch();
                            foreach (SubTestStep subt in ts.SubTest)
                            {
                                sp.Restart();
                                subTSIndex++;
                                if (!subt.Enable)
                                {

                                    continue;
                                }

                                if (StepLst.Contains(subt.Name))
                                {
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData2),
                                                 new object[] { TSIndex - 1, subTSIndex - 1, subt });
                                    continue;
                                }
                                this.ShowStatus(subt.Name + ": " + subt.Description);
                                this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });



                                bool res = subt.Test();
                                this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                    new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                if (subt.SaveResult)
                                {
                                    //SaveResult(ts, subt);
                                    SingleSteps s = singleObj(ts, subt);
                                    this.SingleStepList.Add(s);
                                }

                                if (!res)
                                {
                                    Prj.FailureStep = subt;
                                    errCode = subt.ErrorCode;
                                }
                                TSres &= res;

                                //创建一个SubTest
                                // CreateSubTest(subt);
                                sp.Stop();
                                CreateWCFSubTest1(subt, sp.Elapsed.TotalMilliseconds.ToString("#0.000"));

                                if (!res && BreakOnError)
                                {
                                    if (!subt.Name.Contains(".99"))
                                    {
                                        Jumpto99 = true;
                                    }
                                    break;
                                }
                            }
                            if (Jumpto99)
                            {
                                #region 跳到jumpto99
                                SubTestStep subt = ts.SubTest[ts.SubTest.Count - 1];
                                if (subt.Name.Contains(".99"))
                                {
                                    subTSIndex = ts.SubTest.Count;
                                    if (!subt.Enable) continue;
                                    this.ShowStatus(subt.Name + ": " + subt.Description);
                                    this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                                    bool res = subt.Test();
                                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                        new object[] { TSIndex - 1, subTSIndex - 1, subt });

                                    if (subt.SaveResult)
                                    {
                                        //SaveResult(ts, subt);
                                        SingleSteps s = singleObj(ts, subt);
                                        this.SingleStepList.Add(s);
                                    }

                                    if (!res)
                                    {
                                        Prj.FailureStep = subt;
                                        errCode = subt.ErrorCode;
                                    }
                                    TSres &= res;
                                    if (!res)
                                    {
                                        //DictionaryRecord[subt.Name] = DictionaryRecord[subt.Name] + 1;
                                    }
                                    if (!res && BreakOnError)
                                    {
                                        break;
                                    }
                                }

                                #endregion
                            }
                            foreach (Command c in ts.PostTest)
                            {
                                c.Execute();
                            }
                            if (TSres) break;
                        }


                        if (TSres)
                        {
                            ts.status = TestStatus.Pass;
                        }
                        else
                        {

                            ts.status = TestStatus.Fail;
                            DictionaryRecord["FailTimes"] = DictionaryRecord["FailTimes"] + 1;
                        }
                        TotalRes &= TSres;

                        //Writlst();
                        //更改UUT状态

                        //UpdateWCFUUTStatus(ts);
                        this.Invoke(new TSDataDelegate(UpdateTSData),
                            new object[] { TSIndex - 1, TSres ? "Pass" : "Fail" });
                        if (!TSres && BreakOnError) break;
                    }

                    string testtime = ((int)TestTimer.Elapsed.TotalMinutes).ToString() + "分";
                    testtime += TestTimer.Elapsed.Seconds.ToString() + "秒";
                    this.Invoke(new MessageDelegate(SetBtnText),
                             new object[] { testtime });
                    TestTimer.Stop();



                    foreach (Command c in Prj.PostTest)
                    {
                        c.Execute();
                    }

                    if (TotalRes)
                    {
                        foreach (Command c in Prj.SucTest)
                        {
                            c.Execute();
                        }
                        this.PassCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "PASS", Color.Green });
                    }
                    else
                    {
                        foreach (Command c in Prj.FailTest)
                        {
                            c.Execute();
                        }
                        this.FailCount += 1;
                        this.Invoke(new TestResultDelegate(UpdateTestResult),
                            new object[] { "FAIL", Color.Red });
                    }
                    this.Invoke(new UpdateCounterDelgate(UpdateCounter),
                            new object[] { });

                    if (testKind == TestKind.NormalTest)
                    {
                        station_ProductInfo.EndTime = DateTime.Now.ToString();
                        station_ProductInfo.Result = TotalRes ? "Pass" : "Fail";
                        SaveAllResult_New();

                        //SaveAllResult1(this.SingleStepList);
                    }
                    System.Threading.Thread.Sleep(500);


                    #region 测试后机械动作



                    #endregion

                    MdlClass.IscanTest = false;

                    ShowMessage("跳出测试");


                } while (true);
            }
            catch (Exception e)
            {
                this.ShowMessage(e.Message);
                Console.WriteLine(e.ToString());
                if (e.InnerException == null)
                {
                    this.ShowStatus(e.Message);
                }
                else
                {
                    this.ShowStatus(e.InnerException.Message);
                }
                if (!mQuit)
                {
                    mQuit = true;
                    Prj.ExitTest();
                }
            }
        }


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    CloseAllth();
        //    Prj = dicProjectLst[1];
        //    ProcessUp();

        //    //重置表单
        //    CleanAndSetTable();

        //    //初始化仪器仪表
        //    InitThread = new Thread(new ThreadStart(Init));
        //    InitThread.Start();

        //    //测试线程开始
        //    TestThread = new Thread(new ThreadStart(Test));
        //    TestThread.Start();
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    CloseAllth();
        //    Prj = dicProjectLst[2];
        //    ProcessUp();

        //    //重置表单
        //    CleanAndSetTable();

        //    //初始化仪器仪表
        //    InitThread = new Thread(new ThreadStart(Init));
        //    InitThread.Start();

        //    //测试线程开始
        //    TestThread = new Thread(new ThreadStart(Test));
        //    TestThread.Start();
        //}

        #endregion


        private void frmTestNew_Load(object sender, EventArgs e)
        {
            MdlClass.labelMessage = this.lblMessage;
            MdlClass.LabelTestModeState = this.LabelTestModeState;

            ////设置进度条信息
            //this.Progress.Minimum = 0;
            //this.Progress.Step = 1;
            //this.Progress.Value = 0;

            ////加载项目信息
            //this.Text = "UTOP - " + DateTime.Now.ToShortDateString();
            //this.lblTitle.Text = Prj.Description + " - " + Prj.PartNumber
            //    + " - Ver" + Prj.Version;
            ////btnDate.Text = "当前日期：" + DateTime.Now.ToLongDateString();
            ////btnStart.Visible = false;
            ////btnStop.Visible = false;

            //chkBreakOnError.Enabled = IsBreakOnErrorEnable;
            //if (Prj.IsAnalyse)
            //{
            //    ckbAnalysis.Checked = true;
            //}
            //else
            //{
            //    ckbNormalTest.Checked = true;
            //}

        }
        public string ProjectFile = "";

        private void frmTest_Wide_Shown(object sender, EventArgs e)
        {

            //     m_dbConnection
            //SQLBAL.dbname = m_dbConnection
            //ProjectTestBAL.dbname = m_dbConnection

            ReadyForTest();
            Thread.Sleep(500);
            //BufferHandleThread = new Thread(BufferHandleLogic);
            //BufferHandleThread.Start();
        }

        private void Init()
        {
            Lock = "Start";
            try
            {
                DictionaryRecord.Clear();
                DictionaryRecord.Add("FailTimes", 0);
                InitCounter();
                this.CreateVariables();
                this.CreateObjects();
            }
            catch (Exception ex)
            {
                this.ShowMessage("准备测试序列失败");
                Lock = "ERROR";
                return;
            }

            //perform Init Test
            ShowMessage("正在进行初始化，请稍候...");
            try
            {
                Prj.InitTest();
                ShowMessage("初始化成功，准备测试");
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                Lock = "ERROR";
                return;
            }
            Lock = "Stop";
            return;
        }

        public void tblEnableTrue()
        {
            this.tblChooseKind.Enabled = true;
        }

        public void tblEnableFalse()
        {
            this.tblChooseKind.Enabled = false;
        }

        bool QueryNode(XmlNodeList NodeList, string Stepname)
        {
            bool a = false;
            foreach (XmlNode xn in NodeList)
            {
                if (xn.Attributes["name"].Value == Stepname)
                {
                    a = true;
                    break;
                }
            }
            return a;
        }

        string failSampleName = null;
        string failstep = null;
        XmlDocument iniXML = new XmlDocument();
        //private int Test_PowerOn(XmlNodeList FailureSteps, XmlNodeList NotTestSteps)
        //{
        //    try
        //    {
        //        List<string> FailSteps = new List<string>();
        //        //ShowMessage("请放入软件不合格样件！");
        //        DialogResult testOrNot = MessageBox.Show("请按提示放入“" + failSampleName + "”！", "开机测试", MessageBoxButtons.OK);

        //        Stopwatch TestTimer = new Stopwatch();
        //        //Perform Test
        //        int TSIndex = 0;
        //        int n = 0;
        //        bool TotalRes = true;

        //        Prj.FailureStep = null;

        //        foreach (Command c in Prj.PreTest)
        //        {
        //            c.Execute();
        //        }

        //        ShowMessage("正在进行“" + failSampleName + "”测试...");

        //        this.Invoke(new ResetTSDataDelegate(ResetTestData),
        //            new object[] { });
        //        this.Invoke(new TestResultDelegate(UpdateTestResult),
        //            new object[] { "Testing...", Color.Gray });

        //        TestTimer.Reset();
        //        TestTimer.Restart();
        //        this.Invoke(new MessageDelegate(SetBtnText),
        //            new object[] { "测试中..." });
        //        testKind = TestKind.FirstTest;

        //        foreach (TestStep ts in Prj.TestSteps)
        //        {
        //            foreach (SubTestStep subt in ts.SubTest)
        //            {
        //                subt.Status = TestStatus.NotTested;
        //                if (QueryNode(NotTestSteps, subt.Name))
        //                {
        //                    subt.Status = TestStatus.NotTested;
        //                }
        //            }
        //        }

        //        foreach (TestStep ts in Prj.TestSteps)
        //        {
        //            bool TSres = true;
        //            TSIndex++;
        //            if (!ts.Enable) continue;
        //            for (int j = 0; j <= ts.RetryCount; j++)
        //            {
        //                TSres = true;
        //                n = 0;

        //                foreach (Command c in ts.PreTest)
        //                {
        //                    c.Execute();
        //                }

        //                this.Invoke(new TSDataDelegate(UpdateTSData),
        //                    new object[] { TSIndex - 1, "测试中" });

        //                int subTSIndex = 0;
        //                foreach (SubTestStep subt in ts.SubTest)
        //                {
        //                    subTSIndex++;
        //                    if (!subt.Enable) continue;
        //                    this.ShowStatus(subt.Name + ": " + subt.Description);
        //                    this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

        //                    bool res = subt.Test();
        //                    //modify By Simon 2014 7.13
        //                    if (!res)
        //                    {
        //                        FailSteps.Add(subt.Name);
        //                    }
        //                    this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
        //                        new object[] { TSIndex - 1, subTSIndex - 1, subt });

        //                    //if (subt.SaveResult) SaveResult(ts, subt);

        //                    if (!res)
        //                        Prj.FailureStep = subt;
        //                    TSres &= res;
        //                    //  if (!res && BreakOnError) break;
        //                }

        //                foreach (Command c in ts.PostTest)
        //                {
        //                    c.Execute();
        //                }


        //                if (TSres) break;
        //            }
        //            if (TSres)
        //            {
        //                ts.status = TestStatus.Pass;
        //            }
        //            else
        //            {
        //                ts.status = TestStatus.Fail;
        //            }
        //            TotalRes &= TSres;
        //            this.Invoke(new TSDataDelegate(UpdateTSData),
        //                new object[] { TSIndex - 1, TSres ? "Pass" : "Fail" });
        //            // if (!TSres && BreakOnError) break;
        //        }

        //        string testtime = ((int)TestTimer.Elapsed.TotalMinutes).ToString() + "分";
        //        testtime += TestTimer.Elapsed.Seconds.ToString() + "秒";
        //        this.Invoke(new MessageDelegate(SetBtnText),
        //                 new object[] { testtime });
        //        TestTimer.Stop();

        //        if (TotalRes)
        //        {
        //            this.Invoke(new TestResultDelegate(UpdateTestResult),
        //                new object[] { "PASS", Color.Green });
        //        }
        //        else
        //        {
        //            this.Invoke(new TestResultDelegate(UpdateTestResult),
        //                new object[] { "FAIL", Color.Red });
        //        }

        //        foreach (Command c in Prj.PostTest)
        //        {
        //            c.Execute();
        //        }

        //        System.Threading.Thread.Sleep(500);

        //        #region 判断是设定某些项不过
        //        int x = 0;
        //        if (FailSteps.Count == FailureSteps.Count)
        //        {
        //            for (int i = 0; i < FailSteps.Count; i++)
        //            {
        //                if (FailSteps[i] != FailureSteps[i].Attributes["name"].Value)
        //                {
        //                    x = -1;
        //                    break;
        //                }
        //                else
        //                {
        //                }
        //            }
        //        }
        //        else
        //        {
        //            x = -2;
        //        }
        //        return x;
        //        #endregion

        //        //if (failstep != "")
        //        //{
        //        //    if (failstep == "NULL")
        //        //    {
        //        //        if (TotalRes)
        //        //            return 0;
        //        //        else return -1;
        //        //    }
        //        //    else if (failstep == "ANY")
        //        //    {
        //        //        if (!TotalRes)
        //        //            return 0;
        //        //        else return -1;
        //        //    }
        //        //    else
        //        //    {
        //        //        if (!TotalRes)
        //        //        {
        //        //            foreach (TestStep ts in Prj.TestSteps)
        //        //            {
        //        //                foreach (SubTestStep subt in ts.SubTest)
        //        //                {
        //        //                    if (subt.Name == failstep)
        //        //                    {
        //        //                        if (subt.Status == TestStatus.Fail)
        //        //                        {
        //        //                            return 0;
        //        //                        }
        //        //                    }
        //        //                }
        //        //            }
        //        //            return -1;
        //        //        }
        //        //        else return -1;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    return -2;
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        //this.ShowMessage("由于意外原因，测试已终止");
        //        if (e.InnerException == null)
        //        {
        //            this.ShowStatus(e.Message);
        //        }
        //        else
        //        {
        //            this.ShowStatus(e.InnerException.Message);
        //        }
        //        Prj.ExitTest();
        //        return -3;
        //    }
        //}


        private int Test_PowerOn(XmlNodeList FailureSteps, XmlNodeList NotTestSteps)
        {
            try
            {
                List<bool> NotTestStepOldStates = new List<bool>();
                List<string> FailSteps = new List<string>();
                //ShowMessage("请放入软件不合格样件！");
                DialogResult testOrNot = MessageBox.Show("请按提示放入“" + failSampleName + "”！", "开机测试", MessageBoxButtons.OK);

                Stopwatch TestTimer = new Stopwatch();
                //Perform Test
                int TSIndex = 0;
                int n = 0;
                bool TotalRes = true;

                Prj.FailureStep = null;

                foreach (Command c in Prj.PreTest)
                {
                    c.Execute();
                }

                ShowMessage("正在进行“" + failSampleName + "”测试...");

                this.Invoke(new ResetTSDataDelegate(ResetTestData),
                    new object[] { });
                this.Invoke(new TestResultDelegate(UpdateTestResult),
                    new object[] { "Testing...", Color.Gray });

                TestTimer.Reset();
                TestTimer.Restart();
                this.Invoke(new MessageDelegate(SetBtnText),
                    new object[] { "测试中..." });
                testKind = TestKind.FirstTest;

                //foreach (TestStep ts in Prj.TestSteps)
                for (int i = 0; i < Prj.TestSteps.Count; i++)
                {
                    // foreach (SubTestStep subt in ts.SubTest)

                    for (int j = 0; j < Prj.TestSteps[i].SubTest.Count; j++)
                    {
                        // subt.Status = TestStatus.NotTested;
                        if (QueryNode(NotTestSteps, Prj.TestSteps[i].SubTest[j].Name))
                        {
                            NotTestStepOldStates.Add(Prj.TestSteps[i].SubTest[j].Enable);
                            Prj.TestSteps[i].SubTest[j].Enable = false;
                            Prj.TestSteps[i].SubTest[j].Status = TestStatus.NotTested;
                        }
                    }
                }

                foreach (TestStep ts in Prj.TestSteps)
                {
                    bool TSres = true;
                    TSIndex++;
                    if (!ts.Enable) continue;
                    for (int j = 0; j <= ts.RetryCount; j++)
                    {
                        TSres = true;
                        n = 0;

                        foreach (Command c in ts.PreTest)
                        {
                            c.Execute();
                        }

                        this.Invoke(new TSDataDelegate(UpdateTSData),
                            new object[] { TSIndex - 1, "测试中" });

                        int subTSIndex = 0;
                        foreach (SubTestStep subt in ts.SubTest)
                        {
                            subTSIndex++;
                            if (!subt.Enable) continue;
                            this.ShowStatus(subt.Name + ": " + subt.Description);
                            this.Invoke(new SetProgressValueDelegate(mySetProgressValue), new object[] { ++n });

                            bool res = subt.Test();
                            //modify By Simon 2014 7.13
                            if (!res)
                            {
                                FailSteps.Add(subt.Name);
                            }
                            this.Invoke(new SubTSDataDelegate(UpdateSubTSData),
                                new object[] { TSIndex - 1, subTSIndex - 1, subt });

                            //if (subt.SaveResult) SaveResult(ts, subt);

                            if (!res)
                                Prj.FailureStep = subt;
                            TSres &= res;
                            //  if (!res && BreakOnError) break;
                        }

                        foreach (Command c in ts.PostTest)
                        {
                            c.Execute();
                        }


                        if (TSres) break;
                    }
                    if (TSres)
                    {
                        ts.status = TestStatus.Pass;
                    }
                    else
                    {
                        ts.status = TestStatus.Fail;
                    }
                    TotalRes &= TSres;
                    this.Invoke(new TSDataDelegate(UpdateTSData),
                        new object[] { TSIndex - 1, TSres ? "Pass" : "Fail" });
                    // if (!TSres && BreakOnError) break;
                }

                string testtime = ((int)TestTimer.Elapsed.TotalMinutes).ToString() + "分";
                testtime += TestTimer.Elapsed.Seconds.ToString() + "秒";
                this.Invoke(new MessageDelegate(SetBtnText),
                         new object[] { testtime });
                TestTimer.Stop();

                if (TotalRes)
                {
                    this.Invoke(new TestResultDelegate(UpdateTestResult),
                        new object[] { "PASS", Color.Green });
                }
                else
                {
                    this.Invoke(new TestResultDelegate(UpdateTestResult),
                        new object[] { "FAIL", Color.Red });
                }

                foreach (Command c in Prj.PostTest)
                {
                    c.Execute();
                }

                System.Threading.Thread.Sleep(500);

                int NotTestStepOldIndex = 0;
                for (int i = 0; i < Prj.TestSteps.Count; i++)
                {
                    // foreach (SubTestStep subt in ts.SubTest)

                    for (int j = 0; j < Prj.TestSteps[i].SubTest.Count; j++)
                    {
                        // subt.Status = TestStatus.NotTested;
                        if (QueryNode(NotTestSteps, Prj.TestSteps[i].SubTest[j].Name))
                        {
                            Prj.TestSteps[i].SubTest[j].Enable = NotTestStepOldStates[NotTestStepOldIndex];
                            Prj.TestSteps[i].SubTest[j].Status = TestStatus.NotTested;
                            NotTestStepOldIndex++;
                        }
                    }

                }



                #region 判断是设定某些项不过
                int x = 0;
                if (FailSteps.Count == FailureSteps.Count)
                {
                    for (int i = 0; i < FailSteps.Count; i++)
                    {
                        if (FailSteps[i] != FailureSteps[i].Attributes["name"].Value)
                        {
                            x = -1;
                            break;
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    x = -2;
                }
                return x;
                #endregion

                //if (failstep != "")
                //{
                //    if (failstep == "NULL")
                //    {
                //        if (TotalRes)
                //            return 0;
                //        else return -1;
                //    }
                //    else if (failstep == "ANY")
                //    {
                //        if (!TotalRes)
                //            return 0;
                //        else return -1;
                //    }
                //    else
                //    {
                //        if (!TotalRes)
                //        {
                //            foreach (TestStep ts in Prj.TestSteps)
                //            {
                //                foreach (SubTestStep subt in ts.SubTest)
                //                {
                //                    if (subt.Name == failstep)
                //                    {
                //                        if (subt.Status == TestStatus.Fail)
                //                        {
                //                            return 0;
                //                        }
                //                    }
                //                }
                //            }
                //            return -1;
                //        }
                //        else return -1;
                //    }
                //}
                //else
                //{
                //    return -2;
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                if (e.InnerException == null)
                {
                    this.ShowStatus(e.Message);
                }
                else
                {
                    this.ShowStatus(e.InnerException.Message);
                }
                Prj.ExitTest();
                return -3;
            }
        }

        /// <summary>
        /// 存储测试记录
        /// </summary>
        /// <returns></returns>
        //public bool AddUUTEST()
        //{
        //    bool Isinsert = true;
        //    try
        //    {
        //        //插入主数据库
        //        int UUTTestID = productBll.AddProductInfo(station_ProductInfo);



        //        if (UUTTestID > 0)
        //        {
        //            // List<StationTestResult> stationtestlist = uutest.StationTestResultList;
        //            //Dictionary<string, StationTestResult> DicStationTestResultList = uutest.DicStationTestResultList;

        //            long uutid = UUTTestID;


        //            foreach (Station_TestInfo kp in listTestInfo)
        //            {

        //                //插入测试记录表
        //                kp.UUTestId = UUTTestID.ToString();
        //                //插入测试小步
        //                long SubTestId = testBLL.AddProductInfo(kp);

        //                if (SubTestId <= 0)
        //                {
        //                    Isinsert = false;
        //                    break;
        //                }


        //                if (Isinsert == false)
        //                {
        //                    break;
        //                }
        //                //}
        //            }

        //        }
        //        else
        //        {
        //            Isinsert = false;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Isinsert = false;
        //        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Log\TestError" + DateTime.Now.ToString("yyyyMMdd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":AddUUTTest;" + ex.ToString() + Environment.NewLine);

        //    }
        //    station_ProductInfo = null;
        //    listTestInfo.Clear();
        //    return Isinsert;
        //}


        public void SetTRayno(string trayno)
        {
            TrayNoooo = trayno;
        }



        public void SuspendThread()
        {
            // ShowMessage("该工位处于急停,所有测试全部终止.");
            if (TestThread != null && TestThread.IsAlive)
            {
                TestThread.Suspend();
            }

            if (BufferHandleThread != null && BufferHandleThread.IsAlive)
            {
                BufferHandleThread.Suspend();
              
            }

            //Prj.ExitTest();

            //MessageBox.Show("该工位处于急停,程序将退出测试界面!", "提示");
            ////this.Close();
            //closeTestForm();
        }
        public void EndThread()
        {
            ShowMessage("该工位处于急停,所有测试全部终止.");
            if (TestThread != null && TestThread.IsAlive)
            {
                TestThread.Abort();
            }

            if (BufferHandleThread != null && BufferHandleThread.IsAlive)
            {
                BufferHandleThread.Abort();
                //MyScan.Quit();
            }

            //Prj.ExitTest();

            MessageBox.Show("该工位处于急停,程序将退出测试界面!", "提示");
            //this.Close();
            closeTestForm();
        }

        public void closeTestForm()
        {
            this.Invoke(new MethodInvoker(delegate() { this.Close(); }));
        }

        string TrayNoooo = "";


        public void SendWCFCheckUUT()
        {
            //if (MdlClass.wcfsublist != null)
            //{
            //    MdlClass.wcfuut.SubTestResultList = MdlClass.wcfsublist.ToArray();
            //}
            //MdlClass.wcfserver.SendCheckUUT(MdlClass.wcfuut);
        }

        /// <summary>
        ///   创建小步测试加入  LIST  修改人:洪伟刚
        /// </summary>
        /// <param name="ts"></param>
        public void CreateWCFSubTest1(SubTestStep ts, string time)
        {
            Station_TestInfo stinfo = new Station_TestInfo();
            stinfo.Llimit = ts.LLimit;
            stinfo.Result = ts.Status == TestStatus.Pass ? "Pass" : "Fail";
            stinfo.Ulimit = ts.ULimit;
            stinfo.TestValue = ts.Value.ToString();
            stinfo.Unit = ts.Unit;
            stinfo.Description = ts.Description;
            stinfo.Nom = ts.NomValue;
            stinfo.Product_ID = Prj.PartNumber;
            stinfo.StepName = ts.Name;
            stinfo.TestName = ts.Name;
            stinfo.SpanTime = time;
            stinfo.TestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            stinfo.ComPareMode = ts.CmpMode.ToString();

            listTestInfo.Add(stinfo);


            //Common.WCFServer.SubTestResult str = new Common.WCFServer.SubTestResult();
            //str.ComPareMode = ts.CmpMode.ToString();
            //str.Description = ts.Description;
            //str.LLimit = ts.LLimit;
            //str.Nom = ts.NomValue;
            //str.Result = ts.Status == TestStatus.Pass ? "Pass" : "Fail";
            //str.subStepName = ts.Name;
            //str.TestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //str.TestName = "";
            //str.ULimit = ts.ULimit;
            //str.Unit = ts.Unit;
            //str.Value = ts.Value.ToString();
            //CreateWCFSlst.Add(str);

        }

        public void CreateWCFUUT()
        {

            //             MdlClass.wcfsublist = new List<Common.WCFServer.SubTestResult>();
            //             MdlClass.wcfuut = new Common.WCFServer.UUTTestResult();
            //             Common.WCFServer.SubTestResult sub = new Common.WCFServer.SubTestResult();
            //             List<Common.WCFServer.SubTestResult> sublist = new List<Common.WCFServer.SubTestResult>();
            //             //MdlClass.wcfsublist = new List<Common.WCFServer.SubTestResult>();
            //             //MdlClass.wcfsubTest = new Common.WCFServer.SubTestResult();
            //             MdlClass.wcfuut.SN = SN;
            //             MdlClass.wcfuut.BoxNum = BoxNum;
            //             MdlClass.wcfuut.SlaveStationNo = StationNo;
            //             MdlClass.wcfuut.SlaveStationNoName = StationName;
            //             MdlClass.wcfuut.TrayNo = "101";
            //             MdlClass.wcfuut.UUTestId = "";
            //             MdlClass.wcfuut.Result = "";
            //             MdlClass.wcfuut.DBName = "";
            //             MdlClass.wcfuut.StationOrder = "";
            //             sub.ComPareMode = "";
            //             sub.Description = "";
            //             sub.LLimit = "";
            //             sub.ULimit = "";
            //             sub.TestName = "";
            //             sub.subStepName = "";
            //             sub.TestTime = "";
            //             sub.Nom = "";
            //             sub.Result = "";
            //             sub.Unit = "";
            //             sub.Value = "";
            //             sublist.Add(MdlClass.wcfsubTest);
            //             MdlClass.wcfuut.SubTestResultList = sublist.ToArray();

        }



        //创建一个SubTest
        public void CreateWCFSubTest(SubTestStep ts)
        {
            //Common.WCFServer.SubTestResult str = new Common.WCFServer.SubTestResult();
            //str.ComPareMode = ts.CmpMode.ToString();
            //str.Description = ts.Description;
            //str.LLimit = ts.LLimit;
            //str.Nom = ts.NomValue;
            //str.Result = ts.Status == TestStatus.Pass ? "Pass" : "Fail";
            //str.subStepName = ts.Name;
            //str.TestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //str.TestName = "";
            //str.ULimit = ts.ULimit;
            //str.Unit = ts.Unit;
            //str.Value = ts.Value.ToString();
            //MdlClass.wcfsublist.Add(str);
            //Common.WebServer1.SubTestResult str = new Common.WebServer1.SubTestResult();

            //str.LLimit = ts.LLimit;
            //if (ts.Status == TestStatus.Pass)
            //{
            //    str.Result = Common.WebServer1.TestResultState.Pass;
            //}
            //else
            //{
            //    str.Result = Common.WebServer1.TestResultState.Fail;
            //}
            //str.ULimit = ts.ULimit;
            //str.Value = ts.Value.ToString();
            //str.Unit = ts.Unit;
            //str.Description = ts.Description;
            //str.Nom = ts.NomValue;
            //str.SlaveStationNo = int.Parse(MdlClass.stationID);
            //str.SlaveStationNoName = MdlClass.stationName;
            //str.subStepName = ts.Name;
            //str.TestTime = System.DateTime.Now.ToString();
            //str.ComPareMode = ts.CmpMode.ToString();
            //MdlClass.sublist.Add(str);
        }

        public void UpdateWCFUUTStatus(TestStep ts)
        {
            //if (ts.status == TestStatus.Pass)
            //{
            //    //MdlClass.stationTest.TestResult = Common.WebServer1.TestResultState.Pass;
            //    MdlClass.wcfuut.Result = "Pass";

            //}
            //else
            //{
            //    //MdlClass.stationTest.TestResult = Common.WebServer1.TestResultState.Fail;
            //    MdlClass.wcfuut.Result = "Fail";
            //}


        }


        public void Writlst()
        {
            //MdlClass.wcfsublist.AddRange(CreateWCFSlst);
        }

        //创建一个UUT
        public void CreateUUT()
        {
            //    MdlClass.uut = new Common.WebServer1.UUTTestResult();
            //    MdlClass.uut.LineNo = Convert.ToInt32(MdlClass.LineNo);
            //    MdlClass.uut.LineName = MdlClass.LineName;
            //    MdlClass.uut.SN = SN.ToString();
            //    MdlClass.uut.TrayNo = TrayNoooo.ToString();
            //    MdlClass.uut.DicStationTestResultList = new Dictionary<string, Common.WebServer1.StationTestResult>();
        }

        //创建一个Station
        public void CreateStation()
        {
            //    MdlClass.stationTest = new Common.WebServer1.StationTestResult();
            //    MdlClass.stationTest.LineNo = Convert.ToInt32(MdlClass.LineNo);
            //    MdlClass.stationTest.StationNo = Convert.ToInt32(MdlClass.stationID);
            //    MdlClass.stationTest.StationTestName = MdlClass.stationName;
            //    MdlClass.stationTest.SN = SN.ToString(); ;
            //    MdlClass.stationTest.TrayNo = TrayNoooo.ToString();
            //    MdlClass.sublist = new List<Common.WebServer1.SubTestResult>();
        }
        /// <summary>
        /// 测试项记录 
        /// </summary>
        List<Station_TestInfo> listTestInfo = null;

        //创建一个SubTest
        public void CreateSubTest(SubTestStep ts)
        {

            //SubTestStepSave str = new SubTestStepSave();

            //str.LLimit = ts.LLimit;
            //if (ts.Status == TestStatus.Pass)
            //{
            //    str.Result = Common.WebServer1.TestResultState.Pass;
            //}
            //else
            //{
            //    str.Result = Common.WebServer1.TestResultState.Fail;
            //}
            //str.ULimit = ts.ULimit;
            //str.Value = ts.Value.ToString();
            //str.Unit = ts.Unit;
            //str.Description = ts.Description;
            //str.Nom = ts.NomValue;
            //str.SlaveStationNo = int.Parse(MdlClass.stationID);
            //str.SlaveStationNoName = MdlClass.stationName;
            //str.subStepName = ts.Name;
            //str.TestTime = System.DateTime.Now.ToString();
            //str.ComPareMode = ts.CmpMode.ToString();
            //MdlClass.sublist.Add(str);
        }

        //更改UUT的状态
        //public void UpdateUUTStatus(TestStep ts)
        //{
        //    if (ts.status == TestStatus.Pass)
        //    {
        //        MdlClass.stationTest.TestResult = Common.WebServer1.TestResultState.Pass;
        //        MdlClass.uut.Result = "Pass";

        //    }
        //    else
        //    {
        //        MdlClass.stationTest.TestResult = Common.WebServer1.TestResultState.Fail;
        //        MdlClass.uut.Result = "Fail";
        //    }


        //}

        public void modifyPCBSN(string PCBSN)
        {
            //bool A = MdlClass.webserver.upPCBSN(MdlClass.stationTest.SN, PCBSN);
            //if(!A)
            //{
            //    throw new Exception("修改PCBSN失败...");
            //}
        }



        //发送一个UUT
        public void SendUUT()
        {
            //MdlClass.stationTest.SubTestResultList = MdlClass.sublist.ToArray();

            //MdlClass.uut.DicStationTestResultList.Add(MdlClass.PCBSN,MdlClass.stationTest);
            ////MdlClass.webserver.SendUUT(MdlClass.uut);
            //if (MdlClass.dic_uut.ContainsKey(MdlClass.stationID+MdlClass.LineNo))
            //{
            //    MdlClass.dic_uut.Remove(MdlClass.stationID + MdlClass.LineNo);
            //}
            //MdlClass.dic_uut.Add(MdlClass.stationID + MdlClass.LineNo, MdlClass.uut);
            //MdlClass.webserver.SendStationTestResult();
            //  MdlClass.webserver.SendUUT(MdlClass.uut);
        }

        //发送一个StationtestResult
        //public void SendStationtestResult()
        //{
        //    MdlClass.stationTest.SubTestResultList = MdlClass.sublist.ToArray();


        //    // MdlClass.webserver.SendStationTestResult(MdlClass.stationTest);
        //}

        bool AssemleResult = false;
        List<AssembleResultInfo> listAssemble = new List<AssembleResultInfo>();

        /// <summary>
        /// 装配接口
        /// </summary>
        /// <param name="PartSn">壳体号</param>
        /// <param name="Result"></param>
        /// <param name="type"></param>
        public void SaveAssembleResult(string PartSn, string Result,int type)
        {
            AssemleResult = true;

           AssembleResultInfo assemble=new AssembleResultInfo ();
           assemble.PartSNNum = PartSn;
           assemble.Result = Result;
           assemble.Scantime = DateTime.Now;
           assemble.Type = type;
           listAssemble.Add(assemble);
        }


        /// <summary>
        /// 存储测试数据
        /// </summary>
        /// <returns></returns>
        private bool SaveAllResult_New()
        {




            bool Isinsert = true;
            try
            {
                
                    int UUTTestID = 0;

                    
                    try
                    {
                        Isinsert = true;
                        //插入主数据库
                        UUTTestID = Common.SQLBAL.AddT<Station_ProductInfo>(station_ProductInfo, "ST_StationProductResExample");
                        if (UUTTestID > 0)
                        {
                            foreach (Station_TestInfo kp in listTestInfo)
                            {
                                long SubTestId = 0;
                                //插入测试记录表
                                kp.UUTestId = UUTTestID.ToString();
                                kp.ID = "";
                                SubTestId = Common.SQLBAL.AddT<Station_TestInfo>(kp, "ST_StationTestResultTableExample");
                                if (SubTestId <= 0)
                                {
                                    Isinsert = false;
                                    break;
                                }
                            }
                            #region 插入组装记录表
                            if (AssemleResult)
                            {
                                if (listAssemble != null && listAssemble.Count>0)
                                {
                                    long AssembleId = 0;
                                    for (int i = 0; i < listAssemble.Count; i++)
                                    {
                                        listAssemble[i].UUTestId = UUTTestID;
                                       AssembleId= Common.SQLBAL.AddT<AssembleResultInfo>(listAssemble[i], "ST_assembleResultTableExample");
                                       if (AssembleId <= 0)
                                       {
                                           Isinsert = false;
                                           break;
                                       }
                                    }
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            Isinsert = false;
                        }

                    }
                    catch (Exception ex)
                    {

                        Isinsert = false;
                        File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Log\TestError" + DateTime.Now.ToString("yyyyMMdd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":AddUUTTest;" + ex.ToString() + Environment.NewLine);
                    }

                    if (!Isinsert)
                    {
                        DialogResult resault = MessageBox.Show("插入数据库失败，是否继续测试！", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (resault == DialogResult.No)
                        {
                            return false;
                          
                        }
                       
                    }
                   

              

            }
            catch (Exception ex)
            {
                Isinsert = false;
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Log\TestError" + DateTime.Now.ToString("yyyyMMdd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":AddUUTTest;" + ex.ToString() + Environment.NewLine);

            }

            #region 写入错误日志
            if (!Isinsert)
            {
                string ErrText = "大步：\r\n";
                //总记录 错误写到日志
                System.Reflection.PropertyInfo[] properties = null;

                properties = station_ProductInfo.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    ErrText += item.Name + " ";                                                  //实体类字段名称
                    ErrText += item.GetValue(station_ProductInfo, null) + ",";
                }

                ErrText = ErrText + "\r\n小步：\r\n";
                //小步
                foreach (Station_TestInfo kp in listTestInfo)
                {

                    properties = kp.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                    foreach (System.Reflection.PropertyInfo item in properties)
                    {
                        ErrText += item.Name + " ";                                                  //实体类字段名称
                        ErrText += item.GetValue(kp, null) + ",";
                    }
                    ErrText += "\r\n";
                }
                if (AssemleResult)
                {
                    ErrText = ErrText + "\r\n组装：\r\n";
                    //组装
                    foreach (AssembleResultInfo kp in listAssemble)
                    {

                        properties = kp.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                        foreach (System.Reflection.PropertyInfo item in properties)
                        {
                            ErrText += item.Name + " ";                                                  //实体类字段名称
                            ErrText += item.GetValue(kp, null) + ",";
                        }
                        ErrText += "\r\n";
                    }
                    
                }
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"Log\CreatTestError" + DateTime.Now.ToString("yyyyMMdd") + ".log", ErrText);

            }
            #endregion

            station_ProductInfo = null;
            listTestInfo.Clear();
            listAssemble.Clear();
            AssemleResult = false;

            return Isinsert;
        }


        private void SaveAllResult1(List<SingleSteps> SingleStepList)
        {
            long nMaxStep = 10;
            if (m_dbConnection.State == ConnectionState.Closed)
                m_dbConnection.Open();
            DataContext dc = new DataContext(m_dbConnection);
            ITable<SingleSteps> TabSingleStep = dc.GetTable<SingleSteps>();
            SingleSteps s = new SingleSteps();

            //var max = TabSingleStep.Select(p=>p.StepIndex).DefaultIfEmpty().Max();

            var query = from u in TabSingleStep where u.StepIndex <= 5 select u;
            //var query = from u in TabSingleStep where u.StepIndex <= 6 select u;
            if (query.Count() == 0)
            {
                nMaxStep = 1;
            }
            else
            {
                IEnumerable<long> L = dc.ExecuteQuery<long>(
                    "Select MAX(StepIndex) FROM dbo.SingleSteps", new object[] { });

                nMaxStep = L.First<long>() + 1;
            }

            int x = int.Parse(nMaxStep.ToString());
            for (int i = x; i < SingleStepList.Count + x; i++)
            {
                SaveSingleStep1(SingleStepList[i - x], dc, TabSingleStep, i);
            }

            //foreach (SingleSteps s in SingleStepList)
            //{
            //    SaveSingleStep(s);
            //}
        }

        private bool SaveSingleStep1(SingleSteps s, DataContext dc, ITable<SingleSteps> TabSingleStep, long StepIndex)
        {
            try
            {
                //long nMaxStep = 10;
                //if (m_dbConnection.State == ConnectionState.Closed)
                //    m_dbConnection.Open();
                //DataContext dc = new DataContext(m_dbConnection);
                //ITable<SingleSteps> TabSingleStep = dc.GetTable<SingleSteps>();
                //// SingleSteps s = new SingleSteps();

                ////var max = TabSingleStep.Select(p=>p.StepIndex).DefaultIfEmpty().Max();

                //var query = from u in TabSingleStep where u.StepIndex <= 5 select u;
                ////var query = from u in TabSingleStep where u.StepIndex <= 6 select u;
                //if (query.Count() == 0)
                //{
                //    nMaxStep = 1;
                //}
                //else
                //{
                //    IEnumerable<long> L = dc.ExecuteQuery<long>(
                //        "Select MAX(StepIndex) FROM dbo.SingleSteps", new object[] { });

                //    nMaxStep = L.First<long>() + 1;

                //}
                //nMaxStep++;
                s.StepIndex = StepIndex;
                s.SN = SN;
                s.DayID = DayID.ToString();
                TabSingleStep.InsertOnSubmit(s);
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }


        public void QueryProduct()
        {
            //MdlClass.QueryProductRes = MdlClass.webserver.Query(TrayNoooo.ToString());
            //if (MdlClass.QueryProductRes != null)
            //{
            //    SN = MdlClass.QueryProductRes.SN;
            //    TrayNoooo = MdlClass.QueryProductRes.TrayNo; 
            //}
        }

        private string CreateSN()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar ci = dfi.Calendar;

            //int week = ci.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            //int day =(int) ci.GetDayOfWeek(DateTime.Now);
            //if (day == 0) day = 7;
            M_Data = DateTime.Now.ToString("yyMMdd");
            string year = DateTime.Now.ToString("yyyy");
            if (year == "2014") year = "E";
            else if (year == "2015") year = "F";
            string s = Prj.PartNumber + year + DateTime.Now.ToString("MMdd")
                        + DayID.ToString("D4");
            return s;
        }



        #region Modify SN
        public void ModifySN(string sn)
        {
            this.SN = sn;
        }

        public void ModifySN(string Date, string DayID)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string year = "E";
            if (slist[0] == "14") year = "E";
            else if (slist[0] == "15") year = "F";
            else if (slist[0] == "16") year = "0";
            else if (slist[0] == "17") year = "1";
            else if (slist[0] == "18") year = "2";
            else if (slist[0] == "19") year = "3";
            else if (slist[0] == "20") year = "4";

            string date = slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];
            int Ind = Convert.ToInt32(dayid, 16);
            dayid = Ind.ToString("D4");

            this.SN = Prj.PartNumber + year + date + dayid;
        }

        public void ModifySN(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string year = "E";
            if (slist[0] == "14") year = "E";
            else if (slist[0] == "15") year = "F";
            else if (slist[0] == "16") year = "0";
            else if (slist[0] == "17") year = "1";
            else if (slist[0] == "18") year = "2";
            else if (slist[0] == "19") year = "3";
            else if (slist[0] == "20") year = "4";

            string date = slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = Prj.PartNumber + year + date + dayid;
        }

        public void ModifySN_F10(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string year = slist[0];
            string date = slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = year + date + Prj.PartNumber + dayid;
        }

        public void ModifySN_GL(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string year = slist[0];
            string date = slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = Prj.PartNumber + year + date + dayid;
        }

        public void ModifySN_CVTC(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string year = "F";
            if (slist[0] == "14") year = "E";
            else if (slist[0] == "15") year = "F";
            else if (slist[0] == "16") year = "G";
            else if (slist[0] == "17") year = "H";
            else if (slist[0] == "18") year = "I";
            else if (slist[0] == "19") year = "J";
            else if (slist[0] == "20") year = "K";

            string month = "5";
            int monthInt = Convert.ToInt32(slist[1]);
            month = monthInt.ToString("X1");
            string date = year + month + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = Prj.PartNumber + date + dayid;
        }

        public void ModifySN_PN_DATE_ID(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string date = slist[0] + slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = Prj.PartNumber + date + dayid;
        }

        public void ModifySN_DATE_PN_ID(string Date, string DayID, string HEXorBCD)
        {
            if (Date.Trim().Length != 8 || DayID.Trim().Length != 8)
            {
                return;
            }

            string[] slist = Date.Split(' ');
            string date = slist[0] + slist[1] + slist[2];

            string[] slist1 = DayID.Split(' ');
            string dayid = slist1[1] + slist1[2];

            int Ind;
            string s = HEXorBCD.ToUpper();
            if (s == "HEX")
            {
                Ind = Convert.ToInt32(dayid, 16);
                dayid = Ind.ToString("D4");
            }

            this.SN = date + Prj.PartNumber + dayid;
        }
        #endregion

        private void InitCounter()
        {
            //return;
            bool bExist = false;

            if (m_dbConnection.State == ConnectionState.Closed)
                m_dbConnection.Open();
            DataContext dc = new DataContext(m_dbConnection);
            ITable<Statistic> TabStatistic = dc.GetTable<Statistic>();
            var query = from u in TabStatistic select u;
            foreach (var q in query)
            {
                if (q.PartNr == Prj.PartNumber)
                {
                    PassCount = (int)q.PassCount;
                    FailCount = (int)q.FailCount;
                    ID = (int)q.ID;
                    DayID = (int)q.DayID;

                    bExist = true;
                    break;
                }
            }
            if (!bExist)
            {
                Statistic newS = new Statistic();
                newS.ID = 0;
                newS.PassCount = 0;
                newS.FailCount = 0;
                newS.Date = DateTime.Now;
                newS.PartNr = Prj.PartNumber;
                newS.DayID = 0;
                TabStatistic.InsertOnSubmit(newS);
                dc.SubmitChanges();
            }
            this.Invoke(new UpdateCounterDelgate(UpdateCounter),
                    new object[] { });

        }

        public int GetID()
        {
            return DayID;
        }

        public string GetSN()
        {
            return SN;
        }

        public string GetMdata()
        {
            return M_Data;
        }

        public string GetErrCode()
        {
            return errCode;
        }

        public string GetPartNumber()
        {
            return Prj.PartNumber;
        }

        private bool SaveResult(TestStep ts, SubTestStep subT)
        {
            try
            {
                long nMaxStep = 10;
                if (m_dbConnection.State == ConnectionState.Closed)
                    m_dbConnection.Open();
                DataContext dc = new DataContext(m_dbConnection);
                ITable<SingleSteps> TabSingleStep = dc.GetTable<SingleSteps>();
                SingleSteps s = new SingleSteps();

                //var max = TabSingleStep.Select(p=>p.StepIndex).DefaultIfEmpty().Max();

                var query = from u in TabSingleStep where u.StepIndex <= 5 select u;
                //var query = from u in TabSingleStep where u.StepIndex <= 6 select u;
                if (query.Count() == 0)
                {
                    nMaxStep = 1;
                }
                else
                {
                    IEnumerable<long> L = dc.ExecuteQuery<long>(
                        "Select MAX(StepIndex) FROM dbo.SingleSteps", new object[] { });

                    nMaxStep = L.First<long>() + 1;

                }
                //nMaxStep++;
                s.StepIndex = nMaxStep;
                s.PartNr = Prj.PartNumber;
                s.TestTime = DateTime.Now;
                //id++;
                string DayIDstr = DayID.ToString();
                if (DayIDstr.Length < 4)
                {
                    string a = "";
                    for (int i = DayIDstr.Length; i < 4; i++)
                    {
                        a = a + "0";
                    }
                    DayIDstr = a + DayIDstr;
                }
                else if (DayIDstr.Length > 4)
                {
                    int b = DayIDstr.Length - 4;
                    DayIDstr = DayIDstr.Remove(0, b);
                }
                s.ID = ID.ToString();
                s.DayID = DayIDstr;
                s.SN = SN;
                s.StepName = ts.Name;
                s.SubStepName = subT.Name;
                s.LLimit = subT.LLimit_OBJ.ToString();
                s.ULimit = subT.ULimit_OBJ.ToString();
                s.Nom = subT.NomValue_OBJ.ToString();
                s.Value = subT.Value.ToString();
                s.Unit = subT.Unit;
                s.Result = subT.Status.ToString();
                s.Description = subT.Description;
                s.ErrorCode = subT.ErrorCode;
                s.TestKind = testKind.ToString();
                s.CompareMode = subT.CmpMode.ToString();
                TabSingleStep.InsertOnSubmit(s);
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }


        private bool SaveSingleStep(SingleSteps s)
        {
            try
            {
                long nMaxStep = 10;
                if (m_dbConnection.State == ConnectionState.Closed)
                    m_dbConnection.Open();
                DataContext dc = new DataContext(m_dbConnection);
                ITable<SingleSteps> TabSingleStep = dc.GetTable<SingleSteps>();
                // SingleSteps s = new SingleSteps();

                //var max = TabSingleStep.Select(p=>p.StepIndex).DefaultIfEmpty().Max();

                var query = from u in TabSingleStep where u.StepIndex <= 5 select u;
                //var query = from u in TabSingleStep where u.StepIndex <= 6 select u;
                if (query.Count() == 0)
                {
                    nMaxStep = 1;
                }
                else
                {
                    IEnumerable<long> L = dc.ExecuteQuery<long>(
                        "Select MAX(StepIndex) FROM dbo.SingleSteps", new object[] { });

                    nMaxStep = L.First<long>() + 1;

                }
                //nMaxStep++;
                s.StepIndex = nMaxStep;
                s.SN = SN;
                s.DayID = DayID.ToString();
                TabSingleStep.InsertOnSubmit(s);
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        private SingleSteps singleObj(TestStep ts, SubTestStep subT)
        {
            SingleSteps s = new SingleSteps();
            //  s.StepIndex = nMaxStep;
            s.PartNr = Prj.PartNumber;
            s.TestTime = DateTime.Now;
            //id++;
            string DayIDstr = DayID.ToString();
            if (DayIDstr.Length < 4)
            {
                string a = "";
                for (int i = DayIDstr.Length; i < 4; i++)
                {
                    a = a + "0";
                }
                DayIDstr = a + DayIDstr;
            }
            else if (DayIDstr.Length > 4)
            {
                int b = DayIDstr.Length - 4;
                DayIDstr = DayIDstr.Remove(0, b);
            }
            s.ID = ID.ToString();
            s.DayID = DayIDstr;
            s.SN = SN;
            s.StepName = ts.Name;
            s.SubStepName = subT.Name;
            s.LLimit = subT.LLimit_OBJ.ToString();
            s.ULimit = subT.ULimit_OBJ.ToString();
            s.Nom = subT.NomValue_OBJ.ToString();
            s.Value = subT.Value.ToString();
            s.Unit = subT.Unit;
            s.Result = subT.Status.ToString();
            s.Description = subT.Description;
            s.ErrorCode = subT.ErrorCode;
            s.TestKind = testKind.ToString();
            s.CompareMode = subT.CmpMode.ToString();
            return s;
        }

        private void SaveAllResult(List<SingleSteps> SingleStepList)
        {
            foreach (SingleSteps s in SingleStepList)
            {
                SaveSingleStep(s);
            }
        }


        private bool CreateVariables()
        {
            object obj = null;
            Type tp = null;
            bool bRet = true;
            try
            {
                foreach (KeyValuePair<string, Variable> o in Prj.Variables)
                {
                    //create a new instance for this type, only for type of namespace system,
                    //such as float,int,double,and string....
                    tp = System.Type.GetType("System." + o.Value.Type.Trim());
                    switch (tp.Name)
                    {
                        case "DateTime":
                            o.Value.DefaultValue = DateTime.Now.ToLocalTime().ToString();
                            break;
                    }

                    obj = Convert.ChangeType(o.Value.DefaultValue, tp);
                    if (null == obj)
                    {
                        bRet = false;
                        break;
                    }
                    o.Value.Content = obj;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                bRet = false;
            }
            return bRet;
        }


        private void CreateObjects()
        {
            string[] AsmFiles = Directory.GetFiles(Application.StartupPath + "\\Device");

            List<Assembly> AssList = new List<Assembly>();

            for (int i = 0; i < AsmFiles.Length; i++)
            {
                Assembly A = Assembly.LoadFrom(AsmFiles[i]);


                AssList.Add(A);
                //foreach (Type tp in A.GetTypes())
                //{
                //   //Global.Classes.Add(tp.FullName, tp);
                //}
            }



            //Device
            //Library.DMM dmm = new Library.DMM();
            Library.SystemLib sys = new Library.SystemLib();
            Assembly asm = null;
            AssemblyName[] asmNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();

            foreach (AssemblyName asmname in asmNames)
            {
                if (asmname.Name == "Library")
                {
                    asm = Assembly.Load(asmname);
                    AssList.Add(asm);
                    break;
                }
            }



            foreach (KeyValuePair<String, Device> d in Prj.Devices)
            {
                bool CreatObjectResult = false;
                if (d.Value.Class.Contains("ProjectTest"))
                {
                    d.Value.Instance = this;
                }
                else
                {
                    foreach (Assembly assm in AssList)
                    {
                        if (!d.Value.CreateObject(assm))
                        {
                            CreatObjectResult = false;
                        }
                        else
                        {
                            CreatObjectResult = true;
                            break;
                        }

                        //if (!d.Value.CreateObject(asm))
                        //{
                        //    throw new Exception("创建设备失败: 设备名:[" + d.Value.Name + "]");
                        //}
                    }
                    if (!CreatObjectResult)
                    {
                        throw new Exception("创建设备失败: 设备名:[" + d.Value.Name + "]");
                    }
                }
            }

            //set device instance for all commands
            foreach (Command c in Prj.TestInits)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }

            foreach (Command c in Prj.TestExits)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }

            foreach (Command c in Prj.PreTest)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }
            foreach (Command c in Prj.SucTest)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }
            foreach (Command c in Prj.FailTest)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }
            foreach (Command c in Prj.PostTest)
            {
                c.Device = Prj.Devices[c.DeviceName];
            }

            //---------set device for all step command,add by ydw @2011-0806
            foreach (TestStep s in Prj.TestSteps)
            {
                foreach (Command c in s.PreTest)
                {
                    c.Device = Prj.Devices[c.DeviceName];
                }

                foreach (SubTestStep sub in s.SubTest)
                {
                    foreach (Command c in sub.Commands)
                    {
                        c.Device = Prj.Devices[c.DeviceName];
                    }

                    sub.MeasureCmd.Device = Prj.Devices[sub.MeasureCmd.DeviceName];
                }

                foreach (Command c in s.PostTest)
                {
                    c.Device = Prj.Devices[c.DeviceName];
                }
            }
        }

        private void UpdateTSData(int index, string TS_Stuaus)
        {
            UltraGridRow Row = DataGrid.Rows[index];
            Row.Cells["Test Result"].Value = TS_Stuaus;
            switch (TS_Stuaus)
            {
                case "测试中":
                    break;
                case "Pass":
                    Row.Cells["Test Result"].Appearance.BackColor = Color.Green;
                    if (Row.HasChild()) Row.CollapseAll();
                    break;
                case "Fail":
                    Row.Cells["Test Result"].Appearance.BackColor = Color.Red;
                    //if (Row.HasChild()) Row.ExpandAll();
                    break;
            }

        }

        private void UpdateSubTSData(int TSIndex, int SubTSIndex, SubTestStep subT)
        {
            UltraGridRow Row = DataGrid.Rows[TSIndex];

            int i = 0;
            if (Row.HasChild())
            {
                Row.ExpandAll();
                UltraGridRow r = DataGrid.Rows[TSIndex].GetChild(ChildRow.First);
                while (r != null && i != SubTSIndex)
                {
                    i++;
                    r = r.GetSibling(SiblingRow.Next);
                }

                if (subT.Status == TestStatus.Pass)
                {
                    r.Cells["Test Result"].Value = "Pass";
                    r.Appearance.BackColor = Color.White;
                    r.Cells["Test Result"].Appearance.BackColor = Color.Green;
                }
                else if (subT.Status == TestStatus.Fail)
                {
                    r.Cells["Test Result"].Value = "Fail";
                    r.Cells["Test Result"].Appearance.BackColor = Color.Red;
                    r.Appearance.BackColor = Color.Red;
                }
                r.Cells["Test Value"].Value = subT.Value;

                r.Cells["Expected Value"].Value = subT.DesignedValueString();
            }
        }


        /// <summary>
        /// 临时不测试的项目
        /// </summary>
        /// <param name="TSIndex"></param>
        /// <param name="SubTSIndex"></param>
        /// <param name="subT"></param>
        private void UpdateSubTSData2(int TSIndex, int SubTSIndex, SubTestStep subT)
        {
            UltraGridRow Row = DataGrid.Rows[TSIndex];

            int i = 0;
            if (Row.HasChild())
            {
                Row.ExpandAll();
                UltraGridRow r = DataGrid.Rows[TSIndex].GetChild(ChildRow.First);
                while (r != null && i != SubTSIndex)
                {
                    i++;
                    r = r.GetSibling(SiblingRow.Next);
                }

                r.Cells["Test Result"].Value = "不测试";
            }
        }


        private void ResetTestData()
        {
            foreach (UltraGridRow Row in DataGrid.Rows)
            {

                if (Row.Cells["Test Result"].Value.ToString() != "不测试")
                {
                    Row.Cells["Test Result"].Appearance.BackColor = Color.White;
                    Row.Cells["Test Result"].Value = "未测试";
                    if (Row.HasChild())
                    {
                        UltraGridRow r = Row.GetChild(ChildRow.First);
                        do
                        {

                            if (r.Cells["Test Result"].Value.ToString() != "不测试")
                            {
                                r.Appearance.BackColor = Color.White;
                                r.Cells["Test Result"].Appearance.BackColor = Color.White;
                                r.Cells["Test Result"].Value = "未测试";
                                r.Cells["Test Value"].Value = "";
                            }
                            else
                            {
                                if (StepLst.Contains(r.Cells["StepNr"].Value.ToString()))
                                {
                                    r.Appearance.BackColor = Color.White;
                                    r.Cells["Test Result"].Appearance.BackColor = Color.White;
                                    r.Cells["Test Result"].Value = "未测试";
                                    r.Cells["Test Value"].Value = "";
                                }
                            }
                            r = r.GetSibling(SiblingRow.Next);
                        } while (r != null);
                    }
                }
                Row.CollapseAll();
            }
        }

        delegate void TestResultDelegate(string txt, Color color);
        private void UpdateTestResult(string txt, Color color)
        {
            lblResult.Text = txt;
            lblResult.BackColor = color;
        }

        delegate void UpdateCounterDelgate();
        private void UpdateCounter()
        {
            int Total = PassCount + FailCount;
            double PPM = 0;
            lblPass.Text = PassCount.ToString();
            lblFail.Text = FailCount.ToString();

            if (Total == 0)
            {
                PPM = 0;
            }
            else
            {
                PPM = (double)FailCount / Total;
            }
            //if (bPPM)
            //    lblPPM.Text = ((int)(PPM * 1000000)).ToString();
            //else
            lblPPM.Text = PPM.ToString("p2");

            if (m_dbConnection.State == ConnectionState.Closed)
                m_dbConnection.Open();
            DataContext dc = new DataContext(m_dbConnection);
            ITable<Statistic> TabStatistic = dc.GetTable<Statistic>();
            var query = from u in TabStatistic where u.PartNr == Prj.PartNumber select u;
            foreach (var q in query)
            {
                Statistic t = new Statistic();
                t.PartNr = q.PartNr;
                t.PassCount = PassCount;
                t.FailCount = FailCount;

                DateTime dt = (DateTime)q.Date;
                if (dt.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    if (runtimes > 0)
                    {
                        if ((int)q.DayID == 0)
                        {
                            t.DayID = 1;
                        }
                        else
                        {
                            t.DayID = (int)q.DayID + 1;
                        }
                    }
                    else
                    {
                        if ((int)q.DayID == 0)
                        {
                            t.DayID = 1;
                        }
                        else
                        {
                            t.DayID = (int)q.DayID;
                        }
                    }
                }
                else
                {
                    t.DayID = 1;
                }
                if (runtimes > 0)
                {
                    if ((int)q.ID == 0)
                    {
                        t.ID = 1;
                    }
                    else
                    {
                        t.ID = (int)q.ID + 1;
                    }
                }
                else
                {
                    if ((int)q.ID == 0)
                    {
                        t.ID = 1;
                    }
                    else
                    {
                        t.ID = (int)q.ID;
                    }
                }


                //t.ID = q.ID + 1;
                t.Date = (DateTime)DateTime.Now;
                ID = (int)t.ID;
                DayID = (int)t.DayID;
                TabStatistic.InsertOnSubmit(t);
                TabStatistic.DeleteOnSubmit(q);
            }
            dc.SubmitChanges();

        }

        delegate void SetProgressMaxDelegate(int value);
        private void mySetProgressMax(int value)
        {
            this.Progress.Maximum = value;
        }

        delegate void SetProgressValueDelegate(int value);
        private void mySetProgressValue(int value)
        {
            this.Progress.Value = value;
        }

        private void SetBtnText(string text)
        {
            lblTestTime.Text = "测试时间：" + text;
        }

        private void myShowMessage(string Message)
        {
            this.lblMessage.Text = Message;
        }

        public void ShowMessage(string Message)
        {
            try
            {
                MessageDelegate msg = new MessageDelegate(myShowMessage);
                this.Invoke(msg, new object[] { Message });
            }
            catch (Exception)

            { }
        }

        private void myShowStatus(string Status)
        {
            this.lblStatus.Text = Status;
        }

        public void ShowStatus(string Status)
        {
            try
            {
                MessageDelegate msg = new MessageDelegate(myShowStatus);
                this.Invoke(msg, new object[] { Status });
            }
            catch
            { }
        }

        public void Sleep(double second)
        {
            //System.Threading.Thread.Sleep((int)(second*1000));
            Stopwatch watch = new Stopwatch();

            watch.Start();
            do
            {
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            } while (watch.ElapsedMilliseconds / 1000 < second);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            //InitThread.Abort();
            //InitThread.Join();
            //if (TestThread != null)
            //{

            //    TestThread.Abort();
            //    TestThread.Join(1000);
            //}
            //Prj.ExitTest();
            this.Close();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //mStart = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnClearCount_Click(object sender, EventArgs e)
        {
            PassCount = 0;
            FailCount = 0;
            UpdateCounter();
        }

        private void btnTogglePPM_Click(object sender, EventArgs e)
        {
            bPPM = !bPPM;
            UpdateCounter();
        }

        private void frmTest_Wide_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!btnQuit.Enabled)
            {
                e.Cancel = true;
                return;
            }
            MdlClass.labelMessage = this.lblMessage;
            MdlClass.LabelTestModeState = this.LabelTestModeState;
            try
            {
                //MdlClass.webserver.callbackObject._setTestSeqcallbacked -= SetTestSquece;
                //mQuit = true;
                if (TestThread != null)
                {

                    TestThread.Abort();
                    TestThread.Join(500);
                }
                if (BufferHandleThread != null)
                {
                    if (BufferHandleThread.IsAlive)
                    {
                        BufferHandleThread.Abort();
                        BufferHandleThread.Join(500);
                       
                    }
                }
                //Prj.ExitTest();
                if (!mQuit)
                {
                    mQuit = true;
                    Prj.ExitTest();
                }
            }
            catch (Exception)
            {
            }
        }

        private void chkBreakOnError_CheckedChanged(object sender, EventArgs e)
        {
            BreakOnError = chkBreakOnError.Checked;
        }

        public object ReturnLab()
        {
            return lblMessage as object;
        }

        public object ReturnMe()
        {
            return this as object;
        }

        public string ReturnSN()
        {
            return this.SN;
        }

        public object ReturnButton()
        {
            return this.btnQuit as object;
        }

        public object ReturnDictionaryRecord()
        {
            return DictionaryRecord as object;
        }

        private void ckbAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAnalysis.Checked == true)
            {
                BreakOnError = false;
                chkBreakOnError.Checked = false;
            }
        }

        private void ckbNormalTest_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbNormalTest.Checked == true)
            {
                BreakOnError = true;
                chkBreakOnError.Checked = true;
            }
        }

        List<string> StepLst = new List<string>();
        public void JumpStep(bool NotDo, string Steps)
        {
            if (NotDo)
            {
                StepLst = Steps.Split(',').ToList();
                foreach (TestStep ts in Prj.TestSteps)
                {
                    foreach (SubTestStep st in ts.SubTest)
                    {
                        if (StepLst.Contains(st.Name) && st.Enable == false)
                        {
                            StepLst.Remove(st.Name);
                        }
                    }
                }
            }
        }


        public bool IsEquel(int value)
        {
            if (6 > value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    CloseAllth();
        //    Prj = dicProjectLst[1];
        //    ProcessUp();

        //    //重置表单
        //    CleanAndSetTable();

        //    //初始化仪器仪表
        //    InitThread = new Thread(new ThreadStart(Init));
        //    InitThread.Start();

        //    //测试线程开始
        //    TestThread = new Thread(new ThreadStart(Test));
        //    TestThread.Start();
        //}

        //private void button2_Click_1(object sender, EventArgs e)
        //{
        //    CloseAllth();
        //    Prj = dicProjectLst[2];
        //    ProcessUp();

        //    //重置表单
        //    CleanAndSetTable();

        //    //初始化仪器仪表
        //    InitThread = new Thread(new ThreadStart(Init));
        //    InitThread.Start();

        //    //测试线程开始
        //    TestThread = new Thread(new ThreadStart(Test));
        //    TestThread.Start();
        //}

        /// <summary>
        /// 设置测试序列
        /// </summary>
        /// <param name="SequenceID"></param>
        public void SetTestSquece(int SequenceID)
        {
            this.Invoke(new Action(() =>
            {
                //MdlClass.webserver.sss.ClientIsBusy = 1;
                ShowMessage("正在关闭所有线程");

                CloseAllth();
                this.Prj = new Project();
                this.Prj.File = DicPathlist[SequenceID];
                Prj.Load();
                ProcessUp();
                ShowMessage("重置表单");

                //重置表单
                CleanAndSetTable();

                //初始化仪器仪表
                InitThread = new Thread(new ThreadStart(Init));
                InitThread.Start();

                //测试线程开始
                TestThread = new Thread(new ThreadStart(Test));
                TestThread.Start();
            }));
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //Common.RobotServer.UUT uut = new Common.RobotServer.UUT();
            //MdlClass.TcpipServer.callbackObject.PutUUT(uut);
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            //MdlClass.TcpipServer.callbackObject.GetUUT();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MdlClass.readplc.ShieldBoxDoorCloseSet();
            //MdlClass.readplc.ShieldBoxDoorOpenSet();
        }
    }

}
        #endregion