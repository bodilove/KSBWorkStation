using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.PLCServer;
using Common.PLCData.Model;
using Common.PLCData.BLL;
using System.Threading;
using Common.Process.BLL;
using Common.Process.Model;
using System.IO;
using Common.StationProduct.Model;
using Library;
using System.ServiceModel;

namespace MainControl
{
    public partial class frmMain
    {

        #region PLC方法
        public ParaAndResultStrut Sum_1_2(ParaAndResultStrut pr)
        {
            ParaAndResultStrut pppppp = new ParaAndResultStrut();
            pppppp.Result = true;
            pppppp.Data = new byte[100];

            for (int i = 0; i < pppppp.Data.Length; i++)
            {
                pppppp.Data[i] = 0xff;
            }
            Thread.Sleep(5000);
            pppppp.type = PRType.BYTEArr;
            pppppp.Length = (byte)pppppp.Data.Length;
            ////  MessageBox.Show("1 + 2 = 3");
            //label1.Invoke(new Action(() => { label1.Text = x.ToString(); }));
            //x++;
            return pppppp;
        }

        public ParaAndResultStrut GetDCdata62(ParaAndResultStrut pr)
        {

            //   ParaAndResultStrut pppppp = new ParaAndResultStrut();

            Byte[] data = MdlClass.mydc62.CurrentDataClass.ClassByteArr();

            List<byte> datalst = new List<byte>();
            datalst.AddRange(new byte[100]);
            datalst.AddRange(data);
            if (MdlClass.mydc62.CurrentDataClass.SendRollingNum < MdlClass.mydc62.RollingNum)
            {
                Console.WriteLine("出发62号数据" + ",SendRollingNum;" + MdlClass.mydc62.CurrentDataClass.SendRollingNum + ",RollingNum:" + MdlClass.mydc62.RollingNum);
                MdlClass.mydc62.CurrentDataClass.SendRollingNum = MdlClass.mydc62.RollingNum;
         
                return ReturnResult(datalst.ToArray(), true, PRType.BYTEArr);
            }
            else
            {
                Console.WriteLine("滚码不符合要求触发62" + ",SendRollingNum;" + MdlClass.mydc62.CurrentDataClass.SendRollingNum + ",RollingNum:" + MdlClass.mydc62.RollingNum);
          
      
                return ReturnResult(datalst.ToArray(), false, PRType.BYTEArr);
            }
        }



        public ParaAndResultStrut GetDCdata63(ParaAndResultStrut pr)
        {

            //   ParaAndResultStrut pppppp = new ParaAndResultStrut();

            Byte[] data = MdlClass.mydc63.CurrentDataClass.ClassByteArr();
            List<byte> datalst = new List<byte>();
            datalst.AddRange(new byte[100]);
            datalst.AddRange(data);

            if (MdlClass.mydc63.CurrentDataClass.SendRollingNum < MdlClass.mydc63.RollingNum)
            {
                Console.WriteLine("出发63号数据" + ",SendRollingNum;" + MdlClass.mydc63.CurrentDataClass.SendRollingNum + ",RollingNum:" + MdlClass.mydc63.RollingNum);
                MdlClass.mydc63.CurrentDataClass.SendRollingNum = MdlClass.mydc63.RollingNum;
        
                return ReturnResult(datalst.ToArray(), true, PRType.BYTEArr);
            }
            else
            {
                Console.WriteLine("滚码不符合要求触发63" + ",SendRollingNum;" + MdlClass.mydc63.CurrentDataClass.SendRollingNum + ",RollingNum:" + MdlClass.mydc63.RollingNum);
                return ReturnResult(datalst.ToArray(), false, PRType.BYTEArr);
            }
        }




        public ParaAndResultStrut GetMessage(ParaAndResultStrut pr)//70获取数据
        {

            byte[] data1 = new byte[20];
            byte[] data2 = new byte[20];
            byte[] data3 = new byte[20];
            byte[] data4 = new byte[20];
            byte[] data5 = new byte[20];

            DCdataClass dcd1 = new DCdataClass();
            DCdataClass dcd2 = new DCdataClass();
            DCdataClass dcd3 = new DCdataClass();
            DCdataClass dcd4 = new DCdataClass();
            DCdataClass dcd5 = new DCdataClass();


            for (int i = 0; i < 20; i++)
            {
                data1[i] = pr.Data[100 + i];
                data2[i] = pr.Data[120 + i];
                data3[i] = pr.Data[140 + i];
                data4[i] = pr.Data[160 + i];
                data5[i] = pr.Data[180 + i];
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            dcd1.LoadByte(data1);
            dcd2.LoadByte(data2);
            dcd3.LoadByte(data3);
            dcd4.LoadByte(data4);
            dcd5.LoadByte(data5);

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");




            byte[] datalst = new byte[100];
            return ReturnResult(datalst, true, PRType.Null); ;
        }
        public void AddTestRecordlst(string SationNum, string ComPareMode, string Description, string Llimit, string Nom, string Ulimit, int Result, float SpanTime, DateTime TestTime, string TestName, string TestNum, string TestValue, long StationTestId, long UUTestId, string Unit)
        {
            TestRecord tr = new TestRecord();
            tr.ComPareMode = ComPareMode;
            tr.Description = Description;
            tr.Llimit = Llimit;
            tr.Nom = Nom;
            tr.Ulimit = Ulimit;
            tr.Result = Result;
            tr.SpanTime = SpanTime;
            tr.StationTestId = StationTestId;
            tr.TestName = TestName;
            tr.TestNum = TestNum;
            tr.TestTime = TestTime;
            tr.TestValue = TestValue;
            tr.Unit = Unit;
            tr.UUTestId = UUTestId;
            MdlClass.lineSever.DicStationControl[SationNum].TestRecordlst.Add(tr);
        }
        public bool AddTestRecordBy(string StationNum, string TestNum, string TestName, string Value, long CurrentSationTestID, long uutId, Type datatype)
        {
            try
            {
                Result res = Result.Fail;
                TestRecord trs = null;

                trs = MdlClass.lineSever.DicStationControl[StationNum].JudgmentTestRes(TestNum, TestName, Value.ToString(), datatype, ref res);
                AddTestRecordlst(StationNum, trs.ComPareMode, trs.Description, trs.Ulimit, trs.Nom, trs.Llimit, (int)res, 0f, System.DateTime.Now, trs.TestName, trs.TestNum, Value.ToString(), CurrentSationTestID, uutId, trs.Unit);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ParaAndResultStrut OPH70Enter(ParaAndResultStrut pr)//70获取数据
        {
            try
            {
                byte[] data1 = new byte[20];
                byte[] data2 = new byte[20];
                byte[] data3 = new byte[20];
                byte[] data4 = new byte[20];
                byte[] data5 = new byte[20];

                DCdataClass dcd1 = new DCdataClass();
                DCdataClass dcd2 = new DCdataClass();
                DCdataClass dcd3 = new DCdataClass();
                DCdataClass dcd4 = new DCdataClass();
                DCdataClass dcd5 = new DCdataClass();


                for (int i = 0; i < 20; i++)
                {
                    data1[i] = pr.Data[100 + i];
                    data2[i] = pr.Data[120 + i];
                    data3[i] = pr.Data[140 + i];
                    data4[i] = pr.Data[160 + i];
                    data5[i] = pr.Data[180 + i];
                }
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                dcd1.LoadByte(data1);
                dcd2.LoadByte(data2);
                dcd3.LoadByte(data3);
                dcd4.LoadByte(data4);
                dcd5.LoadByte(data5);

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

                //等待扫码
                string SN = MdlClass.Scanner70.StartScan().Trim();
                if (!MdlClass.Scanner70.Erro)
                {
                    //查询
                    List<Common.Product.Model.InWorkP_Info> lst = MdlClass.inWorkBll.SelectInWorkProductInfo(SN);

                    if (lst.Count != 1)
                    {

                        byte[] datalst1 = new byte[100];

                        return ReturnResult(datalst1, false, PRType.BYTEArr);
                    }
                    else
                    {
                        if (MdlClass.lineSever.DicStationControl["OPH70"].IsDoing && lst[0].CurrentResult == (int)Result.Pass)
                        {
                            if (!MdlClass.lineSever.DicStationControl["OPH70"].EnterStation(lst[0].UUTestId, MainControl.Result.Pass))
                            {
                                MessageBox.Show("扫码产品为NG产品。");
                                return ReturnResult(new byte[100], false, PRType.BYTEArr);
                            }
                            else
                            {
                                Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH70"].GetSationInfo(lst[0].UUTestId);

                                // AddTestRecordBy("OPH10", "OPH10.03", "压装程序号", PressPorgramNum.ToString(), pd.CurrentSationTestID, pd.UUTestId, typeof(System.String));


                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "1号螺丝最终扭矩", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "1号螺丝最终扭矩", "OPH70TS01", dcd1.EndTorque.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "1号螺丝最终角度", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "1号螺丝最终角度", "OPH70TS02", dcd1.EndAngle.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "2号螺丝最终扭矩", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "2号螺丝最终扭矩", "OPH70TS03", dcd2.EndTorque.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "2号螺丝最终角度", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "2号螺丝最终角度", "OPH70TS04", dcd2.EndAngle.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "3号螺丝最终扭矩", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "3号螺丝最终扭矩", "OPH70TS05", dcd3.EndTorque.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH70", CompareMode.Between.ToString(), "3号螺丝最终角度", "0", "Show", "0", (int)Result.Pass, 0.3f, System.DateTime.Now, "3号螺丝最终角度", "OPH70TS06", dcd3.EndAngle.ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.lineSever.DicStationControl["OPH70"].SaveRes();
                                MdlClass.lineSever.DicStationControl["OPH70"].LeaveStation(pd, MainControl.Result.Pass);



                                return ReturnResult(new byte[100], true, PRType.BYTEArr);
                            }
                        }
                        else
                        {
                            byte[] datalst1 = new byte[100];
                            return ReturnResult(datalst1, false, PRType.BYTEArr);
                        }
                    }




                    byte[] datalst = new byte[100];
                    return ReturnResult(datalst, true, PRType.Null); ;
                }
                else
                {
                    //扫码异常中断处理
                }


                return ReturnResult(new byte[100], true, PRType.Null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return ReturnResult(new byte[100], false, PRType.Null); ;
            }
        }




        public ParaAndResultStrut Sum_2_2(ParaAndResultStrut pr)
        {
            Thread.Sleep(5000);
            ParaAndResultStrut pppppp = new ParaAndResultStrut();
            pppppp.Result = true;
            pppppp.Data = new byte[100];
            for (int i = 0; i < pppppp.Data.Length; i++)
            {
                pppppp.Data[i] = 0xff;
            }

            pppppp.type = PRType.BYTEArr;
            pppppp.Length = (byte)pppppp.Data.Length;
            ////  MessageBox.Show("1 + 2 = 3");
            //label1.Invoke(new Action(() => { label1.Text = x.ToString(); }));
            //x++;
            return pppppp;
        }

        public ParaAndResultStrut ReturnResult(byte[] Message, bool res, PRType prType)
        {
            ParaAndResultStrut ReturnStruct = new ParaAndResultStrut();
            ReturnStruct.Result = res;
            ReturnStruct.Data = Message;
            ReturnStruct.type = prType;
            ReturnStruct.Length = (byte)ReturnStruct.Data.Length;
            return ReturnStruct;
        }

        //OPH30请求扫码
        public ParaAndResultStrut OPH30LeaveLine(ParaAndResultStrut pr)
        {
            //30请求扫码
            PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);

            try
            {

                if (plcdata != null)
                {
                    #region 托盘ID小于等于0
                    if (plcdata.TrayId <= 0)
                    {
                        MessageBox.Show("托盘ID小于等于0");
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion

                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine)
                    {
                        //MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion

                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal||(WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        #region UUTID判断

                        //1.判断UUTID
                        if (plcdata.UUTid <= 0)
                        {
                            MessageBox.Show("UUTID小于等于0");
                            byte[] ReturnMessageUUTID = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                            return ReturnResult(ReturnMessageUUTID, true, PRType.BYTEArr);
                        }
                        #endregion

                        #region 来料显示
                        lblDownSN.Invoke(new Action(() =>
                        {
                            lblDownSN.Text = "SN: " + plcdata.ProductSN;
                            lblDownTrayNum.Text = "托盘号:" + plcdata.TrayId;
                            lblDowmUUTID.Text = "UUTID:" + plcdata.UUTid;
                            lblRes.Text = ((Result)plcdata.CurrentResult).ToString();

                            if ((Result)plcdata.CurrentResult == Result.Pass)
                            {
                                lblRes.Font = new Font(lblRes.Font, FontStyle.Regular);
                                lblRes.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblRes.Font = new Font(lblRes.Font, FontStyle.Bold);
                                lblRes.ForeColor = Color.Red;
                                //  lblRes.Font.Bold = true;
                            }
                        }));
                        #endregion
                        //  lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "UUTid=" + plcdata.UUTid + ",SN=" + plcdata.ProductSN + ", 请求下料"; }));
                        try
                        {
                            if (plcdata.CurrentResult == (byte)Result.Pass)
                            {
                                MdlClass.lineSever.DicStationControl["OPH30_End"].EnterStation((long)plcdata.UUTid, (Result)plcdata.CurrentResult);
                                //  MdlClass.lineSever.DicStationControl["OPH30_End"].LeaveStation((long)plcdata.UUTid, (Result)plcdata.CurrentResult);

                                Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH30_End"].GetSationInfo((long)plcdata.UUTid);
                                //AddAssembleRecord("OPH30", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");

                                MdlClass.lineSever.DicStationControl["OPH30_End"].LeaveStation(pd, (Result)plcdata.CurrentResult);
                                //MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                            }
                            else
                            {
                                //MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                            }
                            // Thread.Sleep(5000);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion

                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);


                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误
                // throw ex;
                MessageBox.Show(ex.ToString());
                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
            }

        }
        public void ShowImage(string s1)
        {
            Thread.Sleep(500);

            //01      01       01     01
            //螺丝   磁钢转轴   底壳  连杆
            bool res = false;
            bool[] resDetal = new bool[4];

            if (s1 == "01010101")
            {
                res = true;
            }
            else
            {
                res = false;
            }

            string Failstr = "";
            if (s1[0] != '0' || s1[1] != '1')
            {
                Failstr += " " + "螺丝";
            }
            if (s1[2] != '0' || s1[3] != '1')
            {
                Failstr += " " + "磁钢转轴";
            }
            if (s1[4] != '0' || s1[5] != '1')
            {
                Failstr += " " + "底壳";
            }
            if (s1[6] != '0' || s1[7] != '1')
            {
                Failstr += " " + "连杆";
            }



            picMain.Invoke(new Action(() =>
            {
                if (res)
                {
                    lblUpViosn.Text = "检测结果:Pass";
                    lblUpViosn.BackColor = Color.Green;
                    lblUpViosn.ForeColor = Color.White;
                    picMain.SizeMode = PictureBoxSizeMode.StretchImage;
                    if (File.Exists(MdlClass.CamerPathOK))
                    {
                        Bitmap mImage = new Bitmap(MdlClass.CamerPathOK, true);
                        mImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                        picMain.Image = mImage;

                    }
                    else
                    {
                        lblUpViosn.Text = "检测结果:&";
                        lblUpViosn.BackColor = Color.White;
                        lblUpViosn.ForeColor = Color.Black;
                        picMain.Image = null;
                    }
                }
                else
                {
                    lblUpViosn.Text = "检测结果:NG" + Failstr + " 未放置.";
                    lblUpViosn.BackColor = Color.Red;
                    lblUpViosn.ForeColor = Color.White;
                    if (File.Exists(MdlClass.CamerPathNG))
                    {
                        Bitmap mImage = new Bitmap(MdlClass.CamerPathNG, true);
                        mImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                        picMain.Image = mImage;
                    }
                    else
                    {
                        lblUpViosn.Text = "检测结果:&";
                        lblUpViosn.BackColor = Color.White;
                        lblUpViosn.ForeColor = Color.Black;
                        picMain.Image = null;
                    }
                }

                File.Delete(MdlClass.CamerPathOK); //删除文件
                File.Delete(MdlClass.CamerPathNG); //删除文件 }));

            }));
        }


 
        public ParaAndResultStrut OPH30Enter(ParaAndResultStrut pr)
        {
            //30请求进站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);

                if (plcdata != null)
                {
                    #region 托盘ID小于等于0
                    if (plcdata.TrayId <= 0)
                    {
                        MessageBox.Show("托盘ID小于等于0");
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion



                    plcdata.WorkEnable = 0;
                    lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH30请求进站"; }));
                    #region 清料或者空托盘模式
                    if (MdlClass.WorkType == WorkType.NULL || MdlClass.WorkType == WorkType.ClearLine || MdlClass.WorkType == WorkType.OutBuffer40 || MdlClass.WorkType == WorkType.OutBuffer50)
                    {

                        plcdata.WorkType = (byte)MdlClass.WorkType;
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 正常模式
                    else if (MdlClass.WorkType == WorkType.Normal)
                    {
                        try
                        {
                            #region UUTID判断

                            //1.判断UUTID
                            //if (plcdata.UUTid <= 0)
                            //{
                            //    MessageBox.Show("UUTID小于等于0");
                            //    byte[] ReturnMessageUUTID = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                            //    return ReturnResult(ReturnMessageUUTID, true, PRType.BYTEArr);
                            //}
                            #endregion


                            //01      01       01     01
                            //螺丝   磁钢转轴   底壳  连杆

                            string s1 = Convert.ToString(pr.Data[100], 2).PadLeft(8, '0');
                            if (s1 == "01010101")
                            {
                                ShowImage(s1);

                                //正常进线模式
                                string newSN = "";
                                //向拿到当前PCBSN
                                if (lblPCBSN.Text.Length > 1 && lblPCBSN.Text[lblPCBSN.Text.Length - 1] == 0x0D && MdlClass.lineSever.DicStationControl["OPH30"].ScanPartNumCheckNoLengthLimit(lblPCBSN.Text, 0) )
                                {
                                    //PCBSN测试
                                    if (MdlClass.assembleRecord_bll.SelectAssembleRecordByScanCode(lblPCBSN.Text) != null && MdlClass.assembleRecord_bll.SelectAssembleRecordByScanCode(lblPCBSN.Text).Count < 1)
                                    {
                                        this.Invoke(new Action(() => { lblScanerState.Text = "条码验证成功"; lblScanerState.ForeColor = Color.Green; }));
                                    }
                                    else
                                    {
                                        this.Invoke(new Action(() => { lblScanerState.Text = "错误:该条码已经被使用。"; lblScanerState.ForeColor = Color.Red; }));      
                                    }
                                }
                                else 
                                {
                                    this.Invoke(new Action(() => { lblScanerState.Text = "错误:条码为空或不合法。"; lblScanerState.ForeColor = Color.Red; }));      
                                    byte[] ReturnMessagePCBSNNULL = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                                    return ReturnResult(ReturnMessagePCBSNNULL, false, PRType.BYTEArr);
                                }
                                //string PCBSN = lblMainPart.Text;


                                long uuid = MdlClass.lineSever.Enter(WorkType.Normal, "", "", ref newSN);
                                plcdata.UUTid = (ulong)uuid;

                                MdlClass.lineSever.DicStationControl["OPH30"].EnterStation(uuid, MainControl.Result.Pass);
                                plcdata.WorkEnable = 0;

                                plcdata.CurrentResult = 1;
                                plcdata.PartType_PLC = MdlClass.PartType;//
                                plcdata.WorkType = (byte)MdlClass.WorkType;//
                                plcdata.ProductSN = newSN;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                lblDownSN.Invoke(new Action(() =>
                                {
                                    lblupSN.Text = "SN: " + plcdata.ProductSN;
                                    lblupTrayNum.Text = "托盘号:" + plcdata.TrayId;
                                    lblupUUID.Text = "UUTID:" + plcdata.UUTid;
                                }));
                                //   lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "视觉检测成功" + "UUTid=" + plcdata.UUTid + ",SN=" + plcdata.ProductSN + ", 进站."; }));
                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                            else
                            {
                                ShowImage(s1);
                                lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH30请求进站,视觉检测失败"; }));
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                  

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    else if (MdlClass.WorkType == WorkType.ReWork)
                    {
                        if (lblPCBSN.Text.Length > 1 && lblPCBSN.Text[lblPCBSN.Text.Length - 1] == 0x0D)
                        {
                            Common.Product.Model.ProductInfo pn = null;
                            string newPath=MdlClass. IsCanReWork(lblPCBSN.Text.Trim(), ref pn);
                            if (newPath.StartsWith("OK"))
                            {
                                string NewSN = "";
                                //通过条码获取SN
                                newPath = newPath.Replace("OK,", "");
                                string s = "";
                                long uuid = MdlClass.lineSever.Enter(WorkType.ReWork, newPath, pn.SN, ref NewSN);
                                plcdata.UUTid = (ulong)uuid;

                                MdlClass.lineSever.DicStationControl["OPH30"].EnterStation(uuid, MainControl.Result.Pass);

                                plcdata.WorkEnable = 0;

                                plcdata.CurrentResult = 1;
                                plcdata.PartType_PLC = MdlClass.PartType;//
                                plcdata.WorkType = 1;//正常模式
                                plcdata.ProductSN = pn.SN;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                lblDownSN.Invoke(new Action(() =>
                                {
                                    lblupSN.Text = "SN: " + plcdata.ProductSN;
                                    lblupTrayNum.Text = "托盘号:" + plcdata.TrayId;
                                    lblupUUID.Text = "UUTID:" + plcdata.UUTid;
                                }));
                                //   lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "视觉检测成功" + "UUTid=" + plcdata.UUTid + ",SN=" + plcdata.ProductSN + ", 进站."; }));
                                this.Invoke(new Action(() => { lblScanerState.Text = "返工条码验证成功"; lblScanerState.ForeColor = Color.Green; }));
                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                            else
                            {
                                this.Invoke(new Action(() => { lblScanerState.Text = newPath; lblScanerState.ForeColor = Color.Red; }));
                                byte[] ReturnMessagePCBSNNULL = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                                return ReturnResult(ReturnMessagePCBSNNULL, false, PRType.BYTEArr);
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() => { lblScanerState.Text = "返工条码验证失败"; lblScanerState.ForeColor = Color.Red; }));
                            byte[] ReturnMessagePCBSNNULL = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                            return ReturnResult(ReturnMessagePCBSNNULL, false, PRType.BYTEArr);
                        }
                    }
                    #endregion
                    plcdata.WorkType = (byte)WorkType.ClearLine;
                    plcdata.WorkEnable = 1;//如果 不合法，发1，不工作。
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);

                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }

        }




        public void OPH30DemoData()
        {
            //#region 组装数据

            //   MdlClass.lineSever.DicStationControl["OPH30"].AssembleRecordlst.Add(
            //#endregion

            //#region 测试数据
            //   MdlClass.lineSever.DicStationControl["OPH30"].TestRecordlst.Add(
            //#endregion
        }

        public ParaAndResultStrut OPH30Leave(ParaAndResultStrut pr)
        {


            //30请求出站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);
                //    MessageBox.Show("OPH30请求出站");
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50)
                    {
                        this.Invoke(new Action(() => { lblPCBSN.Text = ""; }));
                        Application.DoEvents();
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                     
                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal||(WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {
                            lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "视觉检测成功" + "UUTid=" + plcdata.UUTid + ",SN=" + plcdata.ProductSN + ", " + ((Result)plcdata.CurrentResult).ToString() + ",离站."; }));

                            Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH30"].GetSationInfo((long)plcdata.UUTid);


                            MdlClass.AddAssembleRecord("OPH30", MdlClass.lineSever.DicStationControl["OPH30"].CurrentAssembleRecord[0].ItemName, MdlClass.lineSever.DicStationControl["OPH30"].CurrentAssembleRecord[0].PartNum, plcdata.CurrentResult, lblPCBSN.Text.Trim(), System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");
                            MdlClass.AddTestRecordlst("OPH30", CompareMode.Equal.ToString(), "视觉检测", "", "OK", "", (int)Result.Pass, 0.3f, System.DateTime.Now, "视觉检测", "OPH30TS01", "OK", pd.CurrentSationTestID, pd.UUTestId, "");
                            MdlClass.lineSever.DicStationControl["OPH30"].SaveRes();
                            MdlClass.lineSever.DicStationControl["OPH30"].LeaveStation(pd, (Result)plcdata.CurrentResult);

                            //插入组装和测试信息

                            this.Invoke(new Action(() => { lblPCBSN.Text = ""; }));
                            Application.DoEvents();


                            ParaAndResultStrut ReturnStruct = new ParaAndResultStrut();


                            byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);



                            return ReturnResult(ReturnMessage, true, PRType.BYTEArr);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    #endregion

                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion
                    this.Invoke(new Action(() => { lblPCBSN.Text = ""; }));
                    Application.DoEvents();
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);
                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }
            this.Invoke(new Action(() => { lblPCBSN.Text = ""; }));
            Application.DoEvents();
        }

        //进站
        public ParaAndResultStrut OPH40Enter(ParaAndResultStrut pr)
        {
            //30请求进站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);
                if (plcdata.TrayId < 1)
                {
                    MessageBox.Show("读取数据失败！");
                }
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    //  lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH40请求进站"; }));
                    #region 清料或者空托盘模式
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50 )
                    {

                        plcdata.WorkType = (byte)MdlClass.WorkType;
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 正常模式
                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {
                            long uuid = (long)plcdata.UUTid;

                            if (MdlClass.lineSever.DicStationControl["OPH40_1"].IsDoing && (Result)plcdata.CurrentResult == Result.Pass)
                            {
                                bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Pass);
                                if (res)
                                {
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                                else
                                {
                                    plcdata.WorkEnable = 1;//代表不需要工作
                                    //   bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Fail);
                                    plcdata.CurrentResult = (byte)Result.Fail;
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                            }
                            else
                            {
                                plcdata.WorkEnable = 1;//代表不需要工作
                                //   bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Fail);
                                plcdata.CurrentResult = (byte)Result.Fail;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion
                    plcdata.WorkType = (byte)WorkType.ClearLine;
                    plcdata.WorkEnable = 1;//如果 不合法，发1，不工作。
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);

                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }

        }


        //出站
        public ParaAndResultStrut OPH40Leave(ParaAndResultStrut pr)
        {
            //30请求出站
            try
            {


                //取公共数据
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);//从100开始取50个字节

                //    MessageBox.Show("OPH30请求出站");
                if (plcdata.TrayId < 1)
                {
                    MessageBox.Show("读取数据失败！");
                }
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50 )
                    {
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {
                            //取气密性测试数据

                            string DATA = Encoding.ASCII.GetString(pr.Data, 100, 50);//Reject,VDPD,-98.099724,kPa,76.547523,Pa,1232

                            string N_pressureValue = "";//负压值
                            string Unit1 = "";//单位
                            string leakageReate = "";//泄露量
                            string Unit2 = "";//单位
                            string[] DATALST = DATA.Split(',');
                            
                            string Result1 = DATALST[0].Trim();
                            if (DATALST[0] == "Reject")
                            {
                                 N_pressureValue = DATALST[2].Trim();//负压值
                                 Unit1 = DATALST[3].Trim();//单位
                                 leakageReate = DATALST[4].Trim();//泄露量
                                 Unit2 = DATALST[5].Trim();//单位
                            }
                          

                            Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH40_1"].GetSationInfo((long)plcdata.UUTid);
                            //AddAssembleRecord("OPH30", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");
                            if (DATALST[0] == "Reject")
                            {
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Equal.ToString(), "气密性测试", "", "Reject", "", (int)Result.Pass, 0.3f, System.DateTime.Now, "气密性测试", "OPH401TS01", Result1, pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Show.ToString(), "气密性测试负压值", "", "", "", (int)Result.Pass, 0.3f, System.DateTime.Now, "气密性测试负压值", "OPH401TS02", N_pressureValue, pd.CurrentSationTestID, pd.UUTestId, Unit1);
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Show.ToString(), "气密性测试泄露量", "", "", "", (int)Result.Pass, 0.3f, System.DateTime.Now, "气密性测试泄露量", "OPH401TS03", leakageReate, pd.CurrentSationTestID, pd.UUTestId, Unit2);
                            }
                            else
                            {
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Equal.ToString(), "气密性测试", "", "Reject", "", (int)Result.Fail, 0.3f, System.DateTime.Now, "气密性测试", "OPH401TS01", Result1, pd.CurrentSationTestID, pd.UUTestId, "");
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Show.ToString(), "气密性测试负压值", "", "", "", (int)Result.Fail, 0.3f, System.DateTime.Now, "气密性测试负压值", "OPH401TS02", N_pressureValue, pd.CurrentSationTestID, pd.UUTestId, Unit1);
                                MdlClass.AddTestRecordlst("OPH40_1", CompareMode.Show.ToString(), "气密性测试泄露量", "", "", "", (int)Result.Fail, 0.3f, System.DateTime.Now, "气密性测试泄露量", "OPH401TS03", leakageReate, pd.CurrentSationTestID, pd.UUTestId, Unit2);
                            }


                            MdlClass.lineSever.DicStationControl["OPH40_1"].SaveRes();

                            MdlClass.lineSever.DicStationControl["OPH40_1"].LeaveStation(pd, (Result)plcdata.CurrentResult);

                            //if ((Result)plcdata.CurrentResult == Result.Fail)
                            //{
                            //    MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                            //}
                            ParaAndResultStrut ReturnStruct = new ParaAndResultStrut();


                            byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                            return ReturnResult(ReturnMessage, true, PRType.BYTEArr);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    #endregion

                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion

                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, false, PRType.BYTEArr);
                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }
        }


        public ParaAndResultStrut GetmyLT_Link99room1Data(ParaAndResultStrut pr)
        {


            Byte[] data = Encoding.ASCII.GetBytes(MdlClass.myLT_Link99room1.CurrenData);

            if (data.Length <= 50)
            {

                if (MdlClass.myLT_Link99room1.SendRollingNum < MdlClass.myLT_Link99room1.RollingNum)
                {
                    MdlClass.myLT_Link99room1.SendRollingNum = MdlClass.myLT_Link99room1.RollingNum;
                    Console.WriteLine("出发myLT_Link99room1数据");
                    return ReturnResult(data, true, PRType.BYTEArr);
                }
                else
                {
                    Console.WriteLine("滚码不符合要求触发myLT_Link99room1");
                    return ReturnResult(data, false, PRType.BYTEArr);
                }
            }
            else
            {
                Console.WriteLine("数据长度超限myLT_Link99room1");
                return ReturnResult(data, false, PRType.BYTEArr);
            }
        }

        public ParaAndResultStrut GetmyLT_Link98room2Data(ParaAndResultStrut pr)
        {

            Byte[] data = Encoding.ASCII.GetBytes(MdlClass.myLT_Link98room2.CurrenData);

            if (data.Length <= 50)
            {

                if (MdlClass.myLT_Link98room2.SendRollingNum < MdlClass.myLT_Link98room2.RollingNum)
                {
                    MdlClass.myLT_Link98room2.SendRollingNum = MdlClass.myLT_Link98room2.RollingNum;
                    Console.WriteLine("出发myLT_Link98room2数据");
                    return ReturnResult(data, true, PRType.BYTEArr);
                }
                else
                {
                    Console.WriteLine("滚码不符合要求触发myLT_Link98room2");
                    return ReturnResult(data, false, PRType.BYTEArr);
                }
            }
            else
            {
                Console.WriteLine("数据长度超限myLT_Link98room2");
                return ReturnResult(data, false, PRType.BYTEArr);
            }
        }

        //进站
        public ParaAndResultStrut OPH40_2Enter(ParaAndResultStrut pr)
        {
            //30请求进站
            Console.WriteLine("OPH40_2 Enter");
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);
                if (plcdata.TrayId < 1)
                {
                    MessageBox.Show("读取数据失败！");
                }
                if (plcdata.WorkEnable == 1)
                {
                    Console.WriteLine("OPH40_2 Enter PASS,WorkEnable:" + plcdata.WorkEnable + ",WorkType" + plcdata.WorkType);
                }
                if (plcdata != null)
                {

                    plcdata.WorkEnable = 0;
                    //  lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH40请求进站"; }));
                    #region 清料或者空托盘模式
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50 )
                    {
                        //Console.WriteLine("OPH40_2 Enter空托盘模式");
                        plcdata.WorkType = (byte)MdlClass.WorkType;
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 正常模式
                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {

                            long uuid = (long)plcdata.UUTid;

                            if (MdlClass.lineSever.DicStationControl["OPH40_2"].IsDoing && (Result)plcdata.CurrentResult == Result.Pass)
                            {

                                bool res = MdlClass.lineSever.DicStationControl["OPH40_2"].EnterStation(uuid, MainControl.Result.Pass);
                                if (res)
                                {
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                                else
                                {
                                    plcdata.WorkEnable = 1;//代表不需要工作
                                    //   bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Fail);
                                    plcdata.CurrentResult = (byte)Result.Fail;
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                            }
                            else
                            {
                                plcdata.WorkEnable = 1;//代表不需要工作

                                plcdata.CurrentResult = (byte)Result.Fail;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                        
                                return ReturnResult(ReturnMessage, false, PRType.BYTEArr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion
                    plcdata.WorkType = (byte)WorkType.ClearLine;
                    plcdata.WorkEnable = 1;//如果 不合法，发1，不工作。
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                    Console.WriteLine("OPH40_2 Enter Erro");
                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);

                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }

        }


        public float getFloat(byte[] arry, int startIndex)
        {
            byte[] bbbb = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bbbb[i] = arry[i + startIndex];
            }
            float a = BitConverter.ToSingle(bbbb.Reverse().ToArray(), 0);
            return a;
        }


    

        //出站
        public ParaAndResultStrut OPH40_2Leave(ParaAndResultStrut pr)
        {
            //30请求出站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);//4byte 压力测试值，4Byte 压力最大值， 4Byte压力最小值， 2byte测试结果 4个byte 位移。
                //byte[] bbbb = new byte[4];
                //for (int i = 0; i < 4; i++)
                //{
                //    bbbb[i] = pr.Data[i + 100];
                //}
                // float a=    BitConverter.ToSingle(bbbb.Reverse().ToArray(), 0);
                Console.WriteLine("OPH40_2 Leave");
                if (plcdata.TrayId < 1)
                {
                    MessageBox.Show("读取数据失败！");
                }
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50)
                    {

                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                        Console.WriteLine("OPH40_2 Leave 空托盘");
                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {

                            Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH40_2"].GetSationInfo((long)plcdata.UUTid);
                            //4byte 压力测试值，4Byte 压力最大值， 4Byte压力最小值， 2byte测试结果 4个byte 位移。
                            float ForceValue = getFloat(pr.Data, 100);
                            float MaxValue = getFloat(pr.Data, 104);
                            float MinValue = getFloat(pr.Data, 108);
                            Result res = (Result)pr.Data[112];
                            float Pos = getFloat(pr.Data, 114);
                            //AddAssembleRecord("OPH30", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");


                            // AddAssembleRecord("OPH40_2", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");
                            if (plcdata.CurrentResult == (byte)Result.Pass)
                            {


                                MdlClass.AddTestRecordlst("OPH40_2", CompareMode.Between.ToString(), "磁钢转轴本体压接压力", MaxValue.ToString(), "", MinValue.ToString(), (int)res, 0.3f, System.DateTime.Now, "磁钢转轴本体压接压力", "OPH402TS01", ForceValue.ToString(), pd.CurrentSationTestID, pd.UUTestId, "N");
                                MdlClass.AddTestRecordlst("OPH40_2", CompareMode.Show.ToString(), "磁钢转轴本体压接行程", "", "", "", (int)res, 0.3f, System.DateTime.Now, "磁钢转轴本体压接行程", "OPH402TS01", Pos.ToString(), pd.CurrentSationTestID, pd.UUTestId, "mm");
                                MdlClass.lineSever.DicStationControl["OPH40_2"].SaveRes();


                                MdlClass.lineSever.DicStationControl["OPH40_2"].LeaveStation(pd, (Result)plcdata.CurrentResult);
                                ParaAndResultStrut ReturnStruct = new ParaAndResultStrut();
                                Show40_2_Oliling();
                            }
                            else
                            {
                                MdlClass.lineSever.DicStationControl["OPH40_2"].LeaveStation(pd, (Result)plcdata.CurrentResult);
                                //if ((Result)plcdata.CurrentResult == Result.Fail)
                                //{
                                //    MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                                //}
                                
                                Show40_2_Oliling();
                            }
                            byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                            Console.WriteLine("OPH40_2 Leave 正常模式");
                            return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    #endregion

                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion

                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                    Console.WriteLine("OPH40_2 Leave Erro");
                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);
                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }
        }

        public void Show40_2_Oliling()
        {

            Thread.Sleep(1000);
            try
            {
                FileInfo[] files = new DirectoryInfo(MdlClass.Oliling40_2Path).GetFiles();


                //1分类
                List<FileInfo> Oliling40_2files = new List<FileInfo>();


                foreach (FileInfo f in files)
                {
                    //1检查文件名合法性
                    if (System.IO.Path.GetExtension(f.FullName).ToUpper() == ".JPEG")
                    {
                        string[] s = System.IO.Path.GetFileNameWithoutExtension(f.FullName).Split('_');
                        Oliling40_2files.Add(f);
                    }
                    else
                    {
                        System.IO.File.Delete(f.FullName);
                    }
                }


                //2排序。



                Oliling40_2files.Sort((x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));//从小到大排列
                //MainPartfiles.Reverse();
                //Pcbfiles.Reverse();
                //3显示

                if (Oliling40_2files.Count > 0)
                {

                    if (CurrentDatetTime40_2 < Oliling40_2files[Oliling40_2files.Count - 1].LastWriteTime)
                    {
                        try
                        {
                            //显示PCBA
                            CurrentDatetTime40_2 = Oliling40_2files[Oliling40_2files.Count - 1].LastWriteTime;
                            //string[] s = Current30OlilingPath.Split('_');
                            //if (s[1] == "2")
                            //{
                            //    gbboxPCBA.ForeColor = Color.Red;
                            //    //NG
                            //
                            //else
                            //{
                            //    gbboxPCBA.ForeColor = Color.Green;
                            //    //Pass
                            //}

                            Thread.Sleep(200);
                            if (Oliling40_2files[Oliling40_2files.Count - 1].Name.ToUpper().StartsWith("OK"))
                            {
                                this.Invoke(new Action(() =>
                                {

                                    lbl40_2Oiling.Text = "检测结果:Pass";
                                    lbl40_2Oiling.BackColor = Color.Green;
                                    lbl40_2Oiling.ForeColor = Color.White;
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {

                                    lbl40_2Oiling.Text = "检测结果:Fail";
                                    lbl40_2Oiling.BackColor = Color.Red;
                                    lbl40_2Oiling.ForeColor = Color.White;
                                }));
                            }


                            //Bitmap mImage = new Bitmap(CurrentPCBAPath, false);
                            //picPCBA.Image = mImage;




                            this.Invoke(new Action(() =>
                            {
                                this.pci40_2Oiling.Image = null;
                                Bitmap mImage = new Bitmap(Oliling40_2files[Oliling40_2files.Count - 1].FullName,true);

                                this.pci40_2Oiling.Image = mImage;
                            }));

                        }
                        catch
                        {
                            Console.WriteLine();
                        }
                        //显示本体
                    }

                }



                //4删除时间最早的。

                if (Oliling40_2files.Count > 5)
                {
                    int Removetime = Oliling40_2files.Count - 5;
                    for (int i = 0; i < Removetime; i++)
                    {
                        File.Delete(Oliling40_2files[0].FullName);
                        Oliling40_2files.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //进站
        public ParaAndResultStrut OPH50Enter(ParaAndResultStrut pr)
        {
            //30请求进站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);

                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    //  lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH40请求进站"; }));
                    #region 清料或者空托盘模式
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50)
                    {

                        plcdata.WorkType = (byte)MdlClass.WorkType;
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 正常模式
                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {
                            long uuid = (long)plcdata.UUTid;

                            if (MdlClass.lineSever.DicStationControl["OPH50"].IsDoing && (Result)plcdata.CurrentResult == Result.Pass)
                            {
                                bool res = MdlClass.lineSever.DicStationControl["OPH50"].EnterStation(uuid, MainControl.Result.Pass);
                                if (res)
                                {
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                                else
                                {
                                    plcdata.WorkEnable = 1;//代表不需要工作
                                    //   bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Fail);
                                    plcdata.CurrentResult = (byte)Result.Fail;
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                            }
                            else
                            {
                              
                                plcdata.WorkEnable = 1;//代表不需要工作
                                plcdata.CurrentResult = (byte)Result.Fail;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion
                    plcdata.WorkType = (byte)WorkType.ClearLine;
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                    plcdata.WorkEnable = 1;//如果 不合法，发1，不工作。
                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);

                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }

        }


        //出站
        public ParaAndResultStrut OPH50Leave(ParaAndResultStrut pr)
        {
            //30请求出站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);
                //    MessageBox.Show("OPH30请求出站");
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50)
                    {
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {

                            Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH50"].GetSationInfo((long)plcdata.UUTid);
                            //AddAssembleRecord("OPH30", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");
                            if (plcdata.CurrentResult == (byte)Result.Pass)
                            {

                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Show.ToString(), "12V上电", "100", "Do", "200", (int)Result.Pass, 0.3f, System.DateTime.Now, "12V上电", "OPH50TS01", "OK", pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到180度", "179.5", "OK", "181.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到180度", "OPH50TS02", (r.Next(100, 900) / 1000.0 + 180).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到240度", "239.5", "OK", "241.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到240度", "OPH50TS03", (r.Next(100, 900) / 1000.0 + 240).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到360度", "359.5", "OK", "361.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到360度", "OPH50TS04", (r.Next(100, 900) / 1000.0 + 360).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到-180度", "179.5", "OK", "181.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到-180度", "OPH50TS05", (r.Next(100, 900) / 1000.0 - 180).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到-240度", "239.5", "OK", "241.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到-240度", "OPH50TS06", (r.Next(100, 900) / 1000.0 - 240).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到-360度", "359.5", "OK", "361.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到-360度", "OPH50TS07", (r.Next(100, 900) / 1000.0 - 360).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "旋转电机到0度", "359.5", "OK", "361.1", (int)Result.Pass, 0.3f, System.DateTime.Now, "旋转电机到0度", "OPH50TS08", (r.Next(100, 900) / 1000.0 + 0).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");

                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "非线性", "1.0", "OK", "2.5", (int)Result.Pass, 0.3f, System.DateTime.Now, "非线性", "OPH50TS09", (r.Next(100, 900) / 1000.0 + 1).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "迟滞", "1.0", "OK", "2.5", (int)Result.Pass, 0.3f, System.DateTime.Now, "迟滞", "OPH50TS10", (r.Next(100, 900) / 1000.0 + 1).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Between.ToString(), "角度误差", "-0.3", "OK", "0.3", (int)Result.Pass, 0.3f, System.DateTime.Now, "角度误差", "OPH50TS11", (r.Next(100, 900) / 3000.0).ToString(), pd.CurrentSationTestID, pd.UUTestId, "");

                                //MdlClass.AddTestRecordlst("OPH50", CompareMode.Show.ToString(), "12V下电", "100", "Do", "200", (int)Result.Pass, 0.3f, System.DateTime.Now, "12V下电", "OPH50TS01", "OK", pd.CurrentSationTestID, pd.UUTestId, "");
                                //MdlClass.lineSever.DicStationControl["OPH50"].SaveRes();


                                MdlClass.lineSever.DicStationControl["OPH50"].LeaveStation(pd, (Result)plcdata.CurrentResult);

                            }
                            else
                            {
                                MdlClass.lineSever.DicStationControl["OPH50"].LeaveStation(pd, (Result)plcdata.CurrentResult);
                                //if ((Result)plcdata.CurrentResult == Result.Fail)
                                //{
                                //    MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                                //}
                            }


                            byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                            return ReturnResult(ReturnMessage, true, PRType.BYTEArr);



                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    #endregion

                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion

                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);
                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }
        }


        //进站
        public ParaAndResultStrut OPH60Enter(ParaAndResultStrut pr)
        {
            //30请求进站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);

                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    //  lblMessage.Invoke(new Action(() => { this.lblMessage.Text = "OPH40请求进站"; }));
                    #region 清料或者空托盘模式
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50)
                    {

                        plcdata.WorkType = (byte)MdlClass.WorkType;
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 正常模式
                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {
                            long uuid = (long)plcdata.UUTid;

                            if (MdlClass.lineSever.DicStationControl["OPH60"].IsDoing && (Result)plcdata.CurrentResult == Result.Pass)
                            {
                                bool res = MdlClass.lineSever.DicStationControl["OPH60"].EnterStation(uuid, MainControl.Result.Pass);
                                if (res)
                                {
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                                else
                                {
                                    plcdata.WorkEnable = 1;//代表不需要工作
                                    //   bool res = MdlClass.lineSever.DicStationControl["OPH40_1"].EnterStation(uuid, MainControl.Result.Fail);
                                    plcdata.CurrentResult = (byte)Result.Fail;
                                    byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                    return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                                }
                            }
                            else
                            {
                                plcdata.WorkEnable = 1;//代表不需要工作
                                plcdata.CurrentResult = (byte)Result.Fail;
                                byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);
                                return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    #endregion
                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion
                    plcdata.WorkType = (byte)WorkType.ClearLine;
                    plcdata.WorkEnable = 1;//如果 不合法，发1，不工作。
                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);

                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }

        }


        //出站
        public ParaAndResultStrut OPH60Leave(ParaAndResultStrut pr)
        {
            //30请求出站
            try
            {
                PLCDataCommon plcdata = MdlClass.PlcdataBll.ByteToPLCDataCommon(pr.Data);
                //byte[] bbbb = new byte[4];
                //for (int i = 0; i < 4; i++)
                //{
                //    bbbb[i] = pr.Data[i + 100];
                //}
                //float a = BitConverter.ToSingle(bbbb.Reverse().ToArray(), 0);
                //    MessageBox.Show("OPH30请求出站");
                if (plcdata != null)
                {
                    plcdata.WorkEnable = 0;
                    #region 来料是空托盘
                    if ((WorkType)plcdata.WorkType == WorkType.NULL || (WorkType)plcdata.WorkType == WorkType.ClearLine || (WorkType)plcdata.WorkType == WorkType.OutBuffer40 || (WorkType)plcdata.WorkType == WorkType.OutBuffer50 )
                    {
                        byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                        return ReturnResult(ReturnMessage, true, PRType.BYTEArr);
                    }
                    #endregion
                    #region 来料是正常模式

                    else if ((WorkType)plcdata.WorkType == WorkType.Normal || (WorkType)plcdata.WorkType == WorkType.ReWork)
                    {
                        try
                        {

                            Common.Product.Model.InWorkP_Info pd = MdlClass.lineSever.DicStationControl["OPH60"].GetSationInfo((long)plcdata.UUTid);
                            //AddAssembleRecord("OPH30", "本体条码", MdlClass.CurrentProcessConfig.Currentprocess.ProductNum, plcdata.CurrentResult, lblMainPart.Text, System.DateTime.Now, pd.CurrentSationTestID, pd.UUTestId, "本体上料");
                            //4gebyte压力测试值，4个byte,最大值，4个byte最小值，2个byte结果，4个byte位移值
                            float ForceValue1 = getFloat(pr.Data, 100);
                            float MaxValue1 = getFloat(pr.Data, 104);
                            float MinValue1 = getFloat(pr.Data, 108);
                            Result res1 = (Result)pr.Data[112];    
                            float Pos1 = getFloat(pr.Data, 114);

                            float ForceValue2 = getFloat(pr.Data, 120);
                            float MaxValue2 = getFloat(pr.Data, 124);
                            float MinValue2 = getFloat(pr.Data, 128);
                            Result res2 = (Result)pr.Data[132];

                            float Pos2 = getFloat(pr.Data, 134);








                            MdlClass.AddTestRecordlst("OPH60", CompareMode.Between.ToString(), "球头螺丝压接压力", MaxValue1.ToString(), "", MinValue1.ToString(), (int)res1, 0.3f, System.DateTime.Now, "球头螺丝压接压力", "OPH60TS01", ForceValue1.ToString(), pd.CurrentSationTestID, pd.UUTestId, "N");
                            MdlClass.AddTestRecordlst("OPH60", CompareMode.Show.ToString(), "球头螺丝压接行程", "", "", "", (int)res1, 0.3f, System.DateTime.Now, "球头螺丝压接行程", "OPH60TS02", Pos1.ToString(), pd.CurrentSationTestID, pd.UUTestId, "mm");

                            MdlClass.AddTestRecordlst("OPH60", CompareMode.Between.ToString(), "连杆压接压力", MaxValue2.ToString(), "", MinValue2.ToString(), (int)res2, 0.3f, System.DateTime.Now, "球头螺丝压接压力", "OPH60TS03", ForceValue2.ToString(), pd.CurrentSationTestID, pd.UUTestId, "N");
                            MdlClass.AddTestRecordlst("OPH60", CompareMode.Show.ToString(), "连杆压接行程", "", "", "", (int)res2, 0.3f, System.DateTime.Now, "球头螺丝压接行程", "OPH60TS04", Pos2.ToString(), pd.CurrentSationTestID, pd.UUTestId, "mm");
                            MdlClass.lineSever.DicStationControl["OPH60"].SaveRes();

                            MdlClass.lineSever.DicStationControl["OPH60"].LeaveStation(pd, (Result)plcdata.CurrentResult);

                            //if ((Result)plcdata.CurrentResult == Result.Fail)
                            //{
                            //    MdlClass.lineSever.Leave((long)plcdata.UUTid, EndType.NormalEndline, (Result)plcdata.CurrentResult);
                            //}


                            ParaAndResultStrut ReturnStruct = new ParaAndResultStrut();


                            byte[] ReturnMessage = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                            return ReturnResult(ReturnMessage, true, PRType.BYTEArr);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    #endregion

                    #region 30 40出buffer
                    #endregion
                    #region 返工模式
                    #endregion

                    byte[] ReturnMessageErro = MdlClass.PlcdataBll.PLCDataCommonToBytes(plcdata);

                    return ReturnResult(ReturnMessageErro, true, PRType.BYTEArr);
                }
                else
                {
                    throw new Exception("转换PLCDataCommon出错。");
                }
            }
            catch (Exception ex)
            {
                //写LOG
                //界面显示错误

                throw ex;
            }
        }





        #endregion



        //public string  IsCanReWork(string ScanCode,ref Common.Product.Model.ProductInfo pn)
        //{
        //    string   NOReWork = "NG,无法返工,原因:";
         
         
        //    List<Common.StationProduct.Model.AssembleRecord> lst = MdlClass.assembleRecord_bll.SelectAssembleRecordByScanCode(ScanCode);
        //    if (lst == null)
        //    {
        //        return NOReWork + "数据库查询出错。";
        //    }

        //    if (lst.Count > 0)
        //    {
        //        //判断正常生产的那次

        //        string NGStation = "";
                
        //        foreach (Common.StationProduct.Model.AssembleRecord smr in lst)
        //        {
        //            Common.Product.Model.ProductInfo pn1 = MdlClass.ProductInfobll.GetProductInfo(smr.UUTestId);
        //            if (pn1.PrdType == 1)
        //            {
        //                pn = pn1;
        //                break;
        //            }
        //        }
        //        if (pn == null)
        //        {
        //            return NOReWork + "未找到该产品。UUTestId：" + lst[lst.Count - 1].UUTestId;
        //        }


        //        List<Common.StationProduct.Model.StationRecord> Stationreslst = MdlClass.stationRecordbll.GetStationRecordByUUTidList(pn.UUTestId);

        //        foreach (Common.StationProduct.Model.StationRecord sr in Stationreslst)
        //        {
        //            if (sr.Result != 2)
        //            {
        //                NGStation = sr.StationNum;
        //                break;
        //            }
        //        }
        //        if (NGStation == "")
        //        {
        //            return NOReWork + "未查询到。";
        //        }
           
        //        if (pn.PartNum == MdlClass.CurrentProcessConfig.Currentprocess.ProductNum)
        //        {
        //            string NewPath= "";
        //            switch (NGStation)
        //            {
        //                case "OPH30":
        //                    return NOReWork + "该站无法返工。";
        //                case "OPH40_1":
        //                    return "OK,OPH30," + NewPath + ExpectedPath("OPH40_1", pn);//得到路径
        //                case "OPH40_2":
        //                    return NOReWork + "该站无法返工。";
        //                case "OPH50":
        //                    return "OK,OPH30," + NewPath + ExpectedPath("OPH50", pn);//得到路径//得到路径判断是否为扫码NG？
        //                case "OPH60":
        //                    return NOReWork + "该站无法返工。";
        //                case "OPH70":
        //                    return NOReWork + "该站无法返工。";//可以在80返工
        //                case "OPH80":
        //                    return "OK," + NewPath + ExpectedPath("OPH80", pn);//得到路径
        //                case "OPH90":
        //                    return "OK," + NewPath + ExpectedPath("OPH90", pn);//得到路径
        //                case "":
        //                    return "NG,NG站输入为空。";//得到路径
        //                default:
        //                    return "NG，未知输入";
        //            }
        //        }
        //        else
        //        {
        //            return NOReWork + "返工产品:" + pn.PartNum + "与现阶段：" + MdlClass.CurrentProcessConfig.Currentprocess.ProductNum+"不符合。";
        //        }
        //    }
        //    else
        //    {
        //        return NOReWork + "未找到该条码。";
        //    }
        //}

        //public string ExpectedPath(string NGStationName, Common.Product.Model.ProductInfo pn)
        //{
        //    string[] Stations = pn.ExpectedPathNames.Split(',');
        //    string NewPath="";
        //    if (Stations.Length > 0)
        //    {
        //        int i = 0;
        //        bool res = false;
        //        for (i = 0; i < Stations.Length; i++)
        //        {
        //            if (NGStationName == Stations[i])
        //            {
        //                res = true;
        //                break;
        //            }
        //        }
        //        if (res)
        //        {
        //            for (int j = i; j < Stations.Length; j++)
        //            {
                  
        //                if (j > i)
        //                {
        //                    NewPath += "," + Stations[j];
        //                }
        //                else
        //                {
        //                    NewPath = Stations[j];
        //                }
        //            }
        //        }
        //        else
        //        {
                    
        //        }
        //    }
        //    else
        //    {
 
        //    }
        //    return NewPath;
        //}
         
    }
}
