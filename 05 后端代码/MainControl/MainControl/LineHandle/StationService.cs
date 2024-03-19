using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Process.Model;
using Common.SysConfig.Model;
using System.Windows.Forms;
using Common.StationProduct.Model;
namespace MainControl
{
    public class StationService
    {
        long allcount = 0;
        public LineService Parentline = null;
        public string StationNum = "";
        public string Stationname = "";
        public bool IsDoing = true;
        public TabPage tabPage = null;
        public StationUI stationUI = null;
        public List<AssembleRecord> AssembleRecordlst = new List<AssembleRecord>();

        public List<TestRecord> TestRecordlst = new List<TestRecord>();

        public P_Detail CurrentDetail = null;
        List<P_BomPart> bomparts = new List<P_BomPart>();
        List<P_TestItemConfig> TestItemConfiglst = new List<P_TestItemConfig>();
        // public List<TestRecord> CurrentExceptTestRecord = new List<TestRecord>();
        public List<TestRecord> CurrentExceptTestRecord = new List<TestRecord>();
        public List<AssembleRecord> CurrentAssembleRecord = new List<AssembleRecord>();
        public long CurrentUUtid = -1;
        public long CurrentStationId = -1;
        //后期还需要增加检测项

        public void InitControl()
        {
            tabPage = new TabPage();
            tabPage.Name = StationNum;
            tabPage.Text = StationNum;

            //MonitorControl.Parent = tabControl;
            stationUI = new StationUI();
            tabPage.Controls.Add(stationUI);
            stationUI.Dock = DockStyle.Fill;
            //  MonitorControl.StarMonitor();
            stationUI.lblStationNum.Text = StationNum;
            stationUI.lblIsByPass.Text = "UnKnow";
            stationUI.lblStationName.Text = Stationname;
        }



        public bool ScanPartNumCheck(string ScanNum, int ItemIndex)//检查ScanPart
        {
            string Scannumtemp = ScanNum.Trim();
            if (CurrentAssembleRecord.Count < ItemIndex + 1)
            {
                return false;
            }
            if (Scannumtemp.Length == CurrentAssembleRecord[ItemIndex].PartNumLength)
            {
                Scannumtemp = Scannumtemp.Substring(CurrentAssembleRecord[ItemIndex].StartIndex, CurrentAssembleRecord[ItemIndex].PartNum.Length);
                if (Scannumtemp == CurrentAssembleRecord[ItemIndex].PartNum)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ScanPartNumCheckNoLengthLimit(string ScanNum, int ItemIndex)//检查ScanPart
        {
            string Scannumtemp = ScanNum.Trim();
            if (CurrentAssembleRecord.Count < ItemIndex + 1)
            {
                return false;
            }



            if (Scannumtemp.StartsWith(CurrentAssembleRecord[ItemIndex].PartNum))
            {
                return true;
                //Scannumtemp = Scannumtemp.Substring(CurrentAssembleRecord[ItemIndex].StartIndex, CurrentAssembleRecord[ItemIndex].PartNum.Length);
                //if (Scannumtemp == CurrentAssembleRecord[ItemIndex].PartNum)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
        }

        private void SetEnable(bool Value)
        {

            IsDoing = Value;
            if (IsDoing)
            {
                stationUI.lblIsByPass.Text = "Doing";
            }
            else
            {
                stationUI.lblIsByPass.Text = "ByPass";
            }
        }

        public void Init(LineService CurrentParentline, LocalStationConfig lcost)
        {
            try
            {
                // bomparts.Clear();
                //还需读取本站配置
                Parentline = CurrentParentline;

                StationNum = lcost.StationNum;
                Stationname = lcost.StationName;
                InitControl();
                //   bomparts = MdlClass.p_BomPartBLL.GetBomPartlList(10, 1, ref allcount, "AND [ProcessId]=" + Parent.CurrentprocessConfig.Currentprocess.Id + "AND [DetailsId]=" + CurrentDetail.Id + "  ORDER BY [OrderNum] ASC", false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetNewProduct(string SN, string uutid)//新产品来先初始化界面
        {

            //初始化BOM清单
            //初始化测试值清单

            stationUI.ResetTable(SN, uutid);
        }

        public void UpOneAssembleRecord(int index, string ScanCode, Result res)
        {
            CurrentAssembleRecord[index].Scantime = DateTime.Now;
            CurrentAssembleRecord[index].ScanCode = ScanCode;
            CurrentAssembleRecord[index].UUTestId = CurrentUUtid;
            CurrentAssembleRecord[index].Result = (int)res;
            CurrentAssembleRecord[index].StationTestId = CurrentStationId;
            //刷新界面
            stationUI.UpAssembleRecord(CurrentAssembleRecord[index], index);
        }

        public void UpOneTestRecord(int index, string TestValue, Result res)
        {
            CurrentExceptTestRecord[index].TestTime = DateTime.Now;
            CurrentExceptTestRecord[index].TestValue = TestValue;
            CurrentExceptTestRecord[index].UUTestId = CurrentUUtid;
            CurrentExceptTestRecord[index].Result = (int)res;
            CurrentExceptTestRecord[index].StationTestId = CurrentStationId;
            stationUI.UpTestRecord(CurrentExceptTestRecord[index], index);
        }

        public void Selcet(bool Value, List<P_BomPart> bomlst, List<P_TestItemConfig> testitemConfiglst)
        {
            if (Value)
            {
                bomparts = bomlst;
                TestItemConfiglst = testitemConfiglst;
                //加载点检序列
                //加载组件BOM
                //加载测试项目
                stationUI.InitTable(bomparts, TestItemConfiglst);

                CurrentExceptTestRecord.Clear();
                CurrentAssembleRecord.Clear();
                foreach (P_BomPart pb in bomparts)
                {
                    Common.Part.BLL.PartInfo p = MdlClass.partbll.GetPartInfoByPartNo(pb.PartNo);
                    Common.StationProduct.Model.AssembleRecord ass = new Common.StationProduct.Model.AssembleRecord();
                    ass.ItemName = pb.ItemName;
                    ass.Description = ass.Description;
                    ass.PartNum = pb.PartNo;
                    ass.Result = (int)Result.NoResult;
                    ass.ScanCode = "";
                    ass.Scantime = System.DateTime.MinValue;
                    ass.StationTestId = -1;
                    ass.UUTestId = -1;
                    if (p != null)
                    {
                        ass.StartIndex = p.StartIndex;
                        ass.PartNumLength = p.PartNumLength;
                    }
                    else
                    {
                        ass.StartIndex = -1;
                        ass.PartNumLength = -1;
                    }
                    CurrentAssembleRecord.Add(ass);

                }
                foreach (P_TestItemConfig pt in TestItemConfiglst)
                {
                    Common.StationProduct.Model.TestRecord tes = new Common.StationProduct.Model.TestRecord();
                    tes.ComPareMode = pt.ComPareMode;
                    tes.Description = pt.Description;
                    tes.Llimit = pt.Llimit;
                    tes.Nom = pt.Nom;
                    tes.Result = (int)Result.NoResult;
                    tes.SpanTime = 0;
                    tes.StationTestId = 0;
                    tes.TestNum = pt.TestNum;
                    tes.TestName = pt.TestName;
                    tes.TestTime = System.DateTime.MinValue;
                    tes.TestValue = "";
                    tes.Ulimit = pt.Ulimit;
                    tes.Unit = pt.Unit;

                    tes.StationTestId = -1;
                    tes.UUTestId = -1;
                    CurrentExceptTestRecord.Add(tes);
                }

            }
            SetEnable(Value);
        }

        public void Start()
        {
            stationUI.InitTable(bomparts, TestItemConfiglst);
        }

        public void Stop()
        {
            stationUI.Dispose();
        }

        public void Dispose()
        {

            Parentline = null;
            StationNum = "";
            Stationname = null;
            CurrentDetail = null;
            // bomparts.Clear();
        }

        public bool EnterStation(long uutId, Result PLCres)
        {
            //在制品列表 查询 uutid
            bool res = false;
            try
            {
                Common.Product.Model.InWorkP_Info pd = MdlClass.inWorkBll.SelectInWorkProductInfo(uutId);

                if (pd != null)
                {
                    if (pd.CurrentStation == this.StationNum)
                    {

                        if (pd.CurrentResult != (int)Result.Fail && PLCres != Result.Fail)
                        {
                            //进站
                            Common.StationProduct.Model.StationRecord pdif = new Common.StationProduct.Model.StationRecord();
                            pdif.UUTestId = uutId;
                            DateTime time = System.DateTime.Now;
                            pdif.StartTime = time;
                            pdif.EndTime = time;
                            pdif.STPrdType = (int)StationEndType.InWork;
                            pdif.Result = (int)MainControl.Result.NoResult;
                            pdif.StationName = this.Stationname;
                            pdif.StationNum = this.StationNum;

                            long StationId = MdlClass.stresbll.StationStart(pdif);
                            if (StationId < 1)
                            {
                                throw new Exception("进站失败。");
                            }
                            else
                            {
                                //更新掉在制品的CurrentStationUUTid
                                pd.CurrentSationTestID = StationId;
                                if (!MdlClass.inWorkBll.UpdateByUUTestId(pd))
                                {
                                    throw new Exception("更新在制品StationID失败。");
                                }
                            }
                            CurrentUUtid = uutId;
                            CurrentStationId = StationId;
                            ResetNewProduct(uutId.ToString(), pd.SN);
                            res = true;
                        }

                    }



                    else
                    {
                        return false;
                        //拒绝进站
                        // throw new Exception("未查询到该产品");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[" + this.StationNum + "]站:进站失败。产品UUTID：" + uutId + ",异常原因:" + ex.ToString());
            }
            return res;

            //1.查询进站还是过站
            //2.拿到当站工序
            //3.进行组件扫描--PLC
            //4。过程数据--PLC
        }

    



        object ConvertData(string Value, Type type)
        {
            object p;
            string typeName = type.FullName;

            switch (typeName)
            {
                case "System.String":
                    p = Value;


                    break;
                case "System.Boolean":
                    p = bool.Parse(Value);

                    break;
                case "System.Int32":
                    p = int.Parse(Value);
                    break;
                case "System.Single":
                case "System.Double":
                    p = double.Parse(Value);
                    break;
                default:
                    throw new Exception("未知类型参数");
                //break;

            }
            return p;
        }
        private bool Evaluate(CompareMode CmpMode, string LLimit, string ULimit, string NOM, string Val, Type DataType)
        {

            object NomValue_OBJ = ConvertData(NOM, DataType);
            object Value = ConvertData(Val, DataType);
            object LLimit_OBJ = ConvertData(LLimit, DataType);
            object ULimit_OBJ = ConvertData(ULimit, DataType);

            double LL, UL, v;

            v = -9.9e37;


            switch (CmpMode)
            {

                case CompareMode.CaseEqual:
                    string nom1 = NomValue_OBJ.ToString();
                    string val1 = Value.ToString();
                    string[] switchs = nom1.Split(',');
                    return switchs.Contains(val1);

                case CompareMode.Between:
                    if (LLimit_OBJ == "LLimit_Null")
                        LL = -9.9e37;
                    else
                        LL = Convert.ToDouble(LLimit_OBJ);

                    if (ULimit == "ULimit_Null")
                        UL = 9.9e37;
                    else
                        UL = Convert.ToDouble(ULimit_OBJ);

                    v = Convert.ToDouble(Value);

                    return (v >= LL && v <= UL);


                case CompareMode.Equal:

                    string nom = NomValue_OBJ.ToString();
                    string val = Value.ToString();
                    if (nom == val)
                        return true;
                    else
                        return (nom == val);

                case CompareMode.HexStringBetween:

                    string[] Lstr = LLimit_OBJ.ToString().Split(' ');
                    string[] Ustr = ULimit_OBJ.ToString().Split(' ');
                    string[] Vstr = Value.ToString().Split(' ');

                    bool res = true;


                    if (Lstr.Length == Ustr.Length && Vstr.Length == Lstr.Length)
                    {

                    }
                    else
                    {
                        return false;
                    }


                    for (int i = 0; i < Lstr.Length; i++)
                    {
                        byte l = Convert.ToByte(Lstr[i], 16);
                        byte u = Convert.ToByte(Ustr[i], 16);
                        byte vv = Convert.ToByte(Vstr[i], 16);
                        bool bok = (vv >= l && vv <= u);
                        res &= bok;
                        if (!res) break;
                    }

                    return res;
                case CompareMode.Contains:
                    string nom2 = NomValue_OBJ.ToString();
                    string val2 = Value.ToString();
                    if (val2.Contains(nom2))
                        return true;
                    else
                        return false;
                case CompareMode.Show:
                    return true;

            }
            return false;
        }

        public TestRecord JudgmentTestRes(string TestNum, string TestName, string Value, Type DataType, ref  Result res)//判断测试项对错
        {
            TestRecord CurrentTs = null;
            foreach (TestRecord ts in this.CurrentExceptTestRecord)
            {
                if (TestNum == ts.TestNum && ts.TestName == TestName)
                {
                    CurrentTs = ts;
                }
            } //(PLCPCPrType)Enum.Parse(typeof(PLCPCPrType), this.cmbPrType.Text);
            CompareMode cmp = (CompareMode)Enum.Parse(typeof(CompareMode), CurrentTs.ComPareMode);

            if (Evaluate(cmp, CurrentTs.Llimit, CurrentTs.Ulimit, CurrentTs.Nom, Value, DataType))
            {
                res = Result.Pass;
            }
            else
            {
                res = Result.Fail;
            }
            return CurrentTs;
        }
        public void SaveRes()
        {
            SaveAssembleRecord();
            SaveTestRecord();
        }


        public Common.Product.Model.InWorkP_Info GetSationInfo(long uutId)
        {
            return MdlClass.inWorkBll.SelectInWorkProductInfo(uutId);
        }

        public void LeaveStation(Common.Product.Model.InWorkP_Info pd, Result res)
        {
            try
            {
                //通过UUTID，通过站号，查找记录，获得StationID。
                //    Common.Product.Model.InWorkP_Info pd = MdlClass.inWorkBll.SelectInWorkProductInfo(uutId);
                if (pd != null)
                {
                    if (pd.CurrentStation == this.StationNum)
                    {
                        if (pd.CurrentSationTestID != null)
                        {
                            //通过StationID 更新记录。

                            if (MdlClass.stresbll.StationEnd(pd.CurrentSationTestID, (int)StationEndType.NormalEndStation, (int)res, this.StationNum))
                            {
                                pd.CurrentSationTestID = -1;

                                string nextStation = "";
                                string IsENDStation =    GetNextStation(pd,ref nextStation);
                                //写入下一工站
                                if (pd.CurrentStation != nextStation)
                                {
                                    pd.CurrentStation = nextStation;
                                }
                                else
                                {
                                    pd.CurrentStation = "";
                                }

                                pd.CurrentResult = (int)res;
                                if (!MdlClass.inWorkBll.UpdateByUUTestId(pd))
                                {
                                    throw new Exception("更新在制品StationID失败。");
                                }
                                CurrentUUtid = -1;
                                CurrentStationId = -1;
                                if (res == Result.Fail)
                                {
                                    Parentline.Leave(pd.UUTestId, EndType.NormalEndline, res);
                                }
                                if (res == Result.Pass)
                                {
                                    if (IsENDStation == "END")
                                    {
                                        Parentline.Leave(pd.UUTestId, EndType.NormalEndline, res);
                                    }
                                }
                                else
                                {
                                    if (IsENDStation == "END")
                                    {
                                        Parentline.Leave(pd.UUTestId, EndType.AbnormalEndLine, res);
                                    }
                                }
                               
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("离站产品不是当站StationNum。");
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("[" + this.StationNum + "]站:离站失败。产品UUTID：" + pd.UUTestId + ",异常原因:" + ex.ToString());
            }

            //Common.Product.Model.InWorkP_Info pd = MdlClass.inWorkBll.SelectProductInfo(uutId);
            //MdlClass.stresbll.
        }

        public string GetNextStation(Common.Product.Model.InWorkP_Info pd,ref string NextStation)
        {
            int index = -1;
          //string NextStation = "";
            string res="Doing。";
            if (pd.PrdType == 1)//正常生产
            {
                for (int i = 0; i < Parentline.CurrentprocessConfig.P_Detaillst.Count; i++)
                {
                    if (Parentline.CurrentprocessConfig.P_Detaillst[i].StationNum == pd.CurrentStation)
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                {
                    throw new Exception("输入当前站号有误。");
                }
                else if (index > Parentline.CurrentprocessConfig.P_Detaillst.Count - 2)
                {
                    // throw new Exception("当前站号已经是最后一站。");
                    NextStation = pd.CurrentStation;
                    res = "END";
                }
                else
                {
                    NextStation = Parentline.CurrentprocessConfig.P_Detaillst[index + 1].StationNum;
                }
            }
            if (pd.PrdType == 2)//返工模式
            {

                string[] StationNumlst = pd.ExpectedPathNames.Split(',');
                for (int i = 0; i < StationNumlst.Length; i++)
                {
                    if (StationNumlst[i] == pd.CurrentStation)
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                {
                    throw new Exception("输入当前站号有误。");
                }
                else if (index > StationNumlst.Length - 2)
                {
                    // throw new Exception("当前站号已经是最后一站。");
                    NextStation = pd.CurrentStation;
                    res = "END";
                }
                else
                {
                    NextStation = StationNumlst[index + 1];
                }
            }

            return res;


        }

        public void SaveAssembleRecord()
        {
            if (AssembleRecordlst.Count > 0)
            {
                if (!MdlClass.assembleRecord_bll.MultiAssembleRecordWrite(AssembleRecordlst))
                {
                    throw new Exception("组装信息:数据库存储失败。");
                }
                AssembleRecordlst.Clear();
            }
        }

        public void SaveTestRecord()
        {
            if (TestRecordlst.Count > 0)
            {
                if (!MdlClass.testRecord_bll.MultiTestResWrite(TestRecordlst))
                {
                    throw new Exception("测试信息:数据库存储失败。");
                }
                TestRecordlst.Clear();
            }
        }
    }
}
