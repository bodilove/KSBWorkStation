using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Common;
using System.Threading.Tasks;
using System.Reflection;
using S7.Net;
using System.Data;
using System.Collections.Concurrent;
using System.Windows.Forms;
//using 
namespace Common.PLCServer
{
    public class PLCS7
    {

        public enum PLCType
        {
            Auto = 1,//属于自动运行状态
            Manual = 2,//属于手动状态
          //  Working =3,//可以工作才可以出发事件
        }
       
        public enum PLCWorkType
        {
            Busy = 3,//繁忙状态，一般用于初始化回原点。//有产品进站属于BUSY状态
            Free = 4,//空闲状态，无产品可做时//无产品生产属于free
            Ready = 5,//准备状态。//初始化后属于Ready状态//初始时属于Busy状态
            Erro=6,//出错状态。//执行命令或者PLC报警时候属于ERRO状态。
            PCForceContro =7,//PC强制控制一下。
        }

        public PLCType CunrrentPLCType = PLCType.Manual;
        public bool CanWork = false;
        public readonly object lockObj = new object();
       // public readonly object lockrWriteObj = new object();
    //    public PLCWorkType CunrrentPLCType = PLCWorkType.Ready;

        public byte SetCurrentWorkType = 0;

        public byte GetCurrentWorkType = 0;


        public TabPage tabPage = null;
        public OnePLCMonitor MonitorControl = null;

        //S7可以开两个线程。为了提高性能，开一个线程进行读，开一个线程进行写。
        Thread thRead = null;
        int HighSpeedStationInfoLength = 200;//高速区数据200;
        byte[] Tempbytes = null;
        int StartAddress = 0;
        int DBIndex = 1000;
        int StationByteCounter = 16;//每站字节
        int HeadByte = 6;
        Stopwatch CommonSp = new Stopwatch();//全局时钟
        DataTable TaskStateDataTable = new DataTable();

        DataTable PLCFlowDataTable = new DataTable();

        DataTable PLCWarningDataTable = new DataTable();

        //     DataTable StationFlowDataTable = new DataTable();
        public bool IsConnect = false;
        //Plc s1200Read = new Plc(CpuType.S71200, "192.168.0.1", 0, 1);
        //Plc s1200Write = new Plc(CpuType.S71200, "192.168.0.1", 0, 1);
        Plc PLCs7Read = null;
        Plc PLCs7Write = null;

        Thread thTrigeTask = null;
        bool thTrigeTaskFlag = false;
        public bool res = false;
        string ip = "";//192.168.0.10
        int port = 0;//port 502
        bool thReadFlag = false;
        TaskFactory MyTaskHandle = null;
        public PLCHandle mypplcHandle = null;
        List<byte[]> plcV = new List<byte[]>();
        CancellationTokenSource cts = null;
        CancellationToken ct;
        LimitedConcurrencyLevelTaskScheduler lcts;
        public ConcurrentDictionary<int, CutromTask> Dictask = new ConcurrentDictionary<int, CutromTask>();

        //public readonly object locktasklst = new object();
        public object mLibMethod = null;//动态调用的函数库

        int SendTimeOut = 5000;//ms
        #region 通用方法
        public PLCEventState GetEventStateValue(int b)
        {
            bool r = Enum.IsDefined(typeof(PLCEventState), b);
            if (r)
            {
                return (PLCEventState)b;
            }
            else
            {
                throw new Exception("PLCEventState 类型不包含：" + b.ToString("X2"));
            }
        }

        /// <summary>
        /// 解析plc返回给pc的参数
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public static object AnalysisPara(ParaAndResultStrut pr)
        {
            object p = null;
            switch (pr.type)
            {
                case PRType.Null:
                    break;
                case PRType.PLCBool:
                    bool a = false;
                    if (pr.Data[0] == 1)
                    {
                        a = true;
                    }
                    p = a;
                    break;
                case PRType.PLCInt:
                    int b = 0x7fffffff;
                    byte[] bt = new byte[4];
                    Array.Copy(pr.Data, 0, bt, 0, 4);
                    b = BitConverter.ToInt32(bt, 0);
                    p = b;
                    break;
                case PRType.PLCString:
                    string s = System.Text.Encoding.ASCII.GetString(pr.Data, 0, pr.Length);
                    p = s;
                    break;

                case PRType.BYTEArr:
                    p = pr.Data.ToArray().Clone();

                    break;
            }
            return p;
        }

        /// <summary>
        /// 将pc传递给plc的参数整合处理成plc需要的结构
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static ParaAndResultStrut IntergrationPara(object p)
        {
            ParaAndResultStrut pr = new ParaAndResultStrut();
            if (p == null)
            {
                pr.type = PRType.Null;
                pr.Length = 0;
                pr.Data = new byte[1];
                pr.Data[0] = 0;
            }
            else
            {
                string typeName = p.GetType().FullName;

                switch (typeName)
                {
                    case "System.String":
                        string s = (string)p;
                        pr.Data = System.Text.Encoding.ASCII.GetBytes(s);
                        pr.Length = (byte)pr.Data.Length;
                        pr.type = PRType.PLCString;

                        break;
                    case "System.Boolean":
                        bool b = (bool)p;
                        pr.type = PRType.PLCBool;
                        pr.Length = 1;
                        pr.Data = new byte[1];
                        pr.Data[0] = 0;
                        if (b)
                        {
                            pr.Data[0] = 1;
                        }
                        else
                        {
                            pr.Data[0] = 2;
                        }
                        break;
                    case "System.Int32":
                        int intt = (int)p;
                        pr.Data = BitConverter.GetBytes(intt);
                        pr.Length = 4;
                        pr.type = PRType.PLCInt;
                        break;
                    case "System.Byte[]":
                        //    int intt = (int)p;

                        byte[] bbb = p as byte[];
                        pr.Data = bbb;
                        pr.Length = (byte)bbb.Length;
                        pr.type = PRType.BYTEArr;
                        break;
                    default:
                        throw new Exception("未知类型参数");
                    //break;

                }
            }
            return pr;
        }

        public object[] CreatObject(PLCEvent pevent, ref MethodInfo mMethod, ParaAndResultStrut fromplcPara)
        {
            try
            {
                //创建库
                string s = pevent.Mycommand.MethodName;
                string[] SubStrings = s.Split(new Char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries);
                SubStrings[0].ToString();
                s = "";
                List<parameter> Parameter = new List<parameter>();
                pevent.Mycommand.Instance = mLibMethod;
                Parameter = pevent.Mycommand.Parameters;

                mMethod = null;
                Type[] tpDef = new Type[Parameter.Count];
                for (int i = 0; i < tpDef.Length; i++)
                {
                    int p1 = Parameter[i].Type.IndexOf("(");
                    int p2 = Parameter[i].Type.IndexOf(")");
                    string typ = pevent.Mycommand.Parameters[i].Type.Substring(p1 + 1, p2 - p1 - 1);
                    tpDef[i] = Type.GetType(typ);
                }
                tpDef = new Type[1];
                tpDef[0] = fromplcPara.GetType();
                mMethod = pevent.Mycommand.Instance.GetType().GetMethod(SubStrings[0].Trim(), tpDef);
                //Create parameters
                // Type tp = null;
                object[] oParameters = new object[Parameter.Count];
                //for (int i = 0; i < Parameter.Count; i++)
                //{
                //    int p1 = Parameter[i].Type.IndexOf("(");
                //    int p2 = Parameter[i].Type.IndexOf(")");
                //    string typ = Parameter[i].Type.Substring(p1 + 1, p2 - p1 - 1);
                //    tp = Type.GetType(typ);
                //    if (tp == null) throw new Exception("参数类型错误,参数类型:" + Parameter[i].Type);
                //    if (tp.Name != "ParaAndResultStrut")
                //    {
                //        oParameters[i] = System.Convert.ChangeType(Parameter[i].Value, tp);
                //    }
                //    else
                //    {
                //        oParameters[i] = fromplcPara;
                //    }
                //    if (oParameters[i] == null)
                //        throw new Exception("参数创建错误,参数名:" + Parameter[i].Value);
                //}

                oParameters = new object[1];
                oParameters[0] = fromplcPara;

                return oParameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// plc请求pc工作
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="PLCTrigerIndex"></param>
        /// <param name="pr"></param>
        /// <param name="plcST"></param>
        public void RunPLCTrigerTask(PLCEvent pe, int PLCTrigerIndex, PLCStation plcST)
        {
            //判断新任务是否已经包含在内。

            Stopwatch sp11 = new Stopwatch();
            sp11.Restart();


            #region 把占用的事件去除

            if (Dictask.ContainsKey(plcST.id))//Station的ID
            {
                if (Dictask[plcST.id].task.IsCompleted || Dictask[plcST.id].task.IsCanceled || Dictask[plcST.id].task.IsFaulted)
                {
                    CutromTask tempobj = null;
                    if (Dictask.TryRemove(plcST.id, out tempobj))
                    {
                        tempobj.Dispose();
                        tempobj = null;
                    }
                    else
                    {
                        throw new Exception(plcST.id + "，清除无用事件失效。");
                    }
                }
                else
                {
                    throw new Exception(plcST.id + "，触发事件未执行完就开始了下一次事件，下次事件索引：" + PLCTrigerIndex);
                }
            }
            #endregion

            //Console.WriteLine("同步时间：" + sp11.ElapsedMilliseconds);

            Task t = MyTaskHandle.StartNew(() =>
             {
                 //if (plcST.id == 0)
                 //{
                 //    Console.WriteLine("事件1开始时间：");
                 //}
                 Stopwatch sp = new Stopwatch();
                 sp.Restart();

                 try
                 {

                     ushort Value = (ushort)PLCTrigerIndex;
                     byte[] b1 = BitConverter.GetBytes(Value);
                     b1 = b1.Reverse().ToArray();
                     Value = (ushort)0XFF;
                     byte[] b2 = BitConverter.GetBytes(Value);
                     b2 = b2.Reverse().ToArray();
                     List<byte> arrb = new List<byte>();
                     arrb.AddRange(b1);
                     arrb.AddRange(b2);
                     WriteBytes(DataType.DataBlock, DBIndex, plcST.id * 16 + 14, arrb.ToArray());
                  



                     #region 获取参数
                     ParaAndResultStrut pr1 = new ParaAndResultStrut();
                     pr1.StationId = plcST.id;
                     //获取参数类型
                     if (pe.PrType == PLCPCPrType.BYTEArr)
                     {
                         pr1.Data = new byte[plcST.PlcParamAndRes_ByteCount];
                         pr1.type = PRType.BYTEArr;

                         //此处读取低速区参数吗？？？？
                         //byte[] b = myModbusTcp.ReadByteArr((ushort)p.PlcParamAndRes_StartAddress, (ushort)p.PlcParamAndRes_ByteCount, 1000).Data.ToArray();

                         byte[] b;
                         lock (lockObj)
                         {
                              b = PLCs7Read.ReadBytes(DataType.DataBlock, DBIndex, plcST.PlcParamAndRes_StartAddress, plcST.PlcParamAndRes_ByteCount);

                         }
                         if (b.Length == plcST.PlcParamAndRes_ByteCount)
                         {
                             for (int i = 0; i < plcST.PlcParamAndRes_ByteCount; i++)
                             {
                                 pr1.Data[i] = b[i];
                             }
                         }
                         else
                         {
                             throw new Exception("数据采集不符合");
                         }

                           //  Array.Copy(b, 0, pr1.Data, 0, plcST.PlcParamAndRes_ByteCount);
                        
                     }
                     else if (pe.PrType == PLCPCPrType.NULL)
                     {
                         pr1.type = PRType.Null;
                     }
                     #endregion






                     MethodInfo Method = null;
                     object[] paras = CreatObject(pe, ref Method, pr1);
                     // object[] paras = CreatObject1(pe, ref Method, pr);

                     if (Method != null)
                     {
                         object p = null;
                         p = Method.Invoke(pe.Mycommand.Instance, paras);//开始执行在plc事件中配置的函数

                         if (p != null && p is ParaAndResultStrut)
                         {
                             ParaAndResultStrut ppp = (ParaAndResultStrut)p;

                             byte[] b = new byte[plcST.PcParamAndRes_ByteCount];
                             if (ppp.Data.Length > 0)
                             {
                                 if (pe.ResultValueType != PLCPCPrType.NULL)
                                 {
                                     Array.Copy(ppp.Data, 0, b, 0, ppp.Length);
                                     // byte[] witebyte = ppp.Data;
                                     //Array.Reverse(witebyte);

                                     //myModbusTcp.WriteByteArr(
                                     //    witebyte.ToList()
                                     //    , (ushort)(plcST.PlcParamAndRes_StartAddress
                                     ////    + mypplc.HeatBeatAdr));
                                     WriteBytes(DataType.DataBlock, DBIndex, plcST.PcParamAndRes_StartAddress, b);
                                 }
                             }


                             //ushort Value = (ushort)PLCTrigerIndex;
                             //byte[] b1 = BitConverter.GetBytes(Value);
                             //b1 = b1.Reverse().ToArray();
                             //Value = (ushort)0XFF;
                             //byte[] b2 = BitConverter.GetBytes(Value);
                             //b2 = b2.Reverse().ToArray();
                             //List<byte> arrb = new List<byte>();
                             //arrb.AddRange(b1);
                             //arrb.AddRange(b2);
                          
                             //写入完成---
                             do
                             {
                                 WriteBytes(DataType.DataBlock, DBIndex, plcST.id * 16 + 14, arrb.ToArray());
                                 Thread.Sleep(5);
                                 if (this.mypplcHandle.PLCStationlst[plcST.id].PLC2PCEventConfig.Eventlst[PLCTrigerIndex].plcEventState == PLCEventState.InHanding)
                                 {
                                     break;
                                 }
                                 if (Dictask.ContainsKey(plcST.id))
                                 {
                                     if (Dictask[plcST.id].IsAbort)
                                     {
                                         throw new Exception("线程被用户手动终止！");
                                     }
                                 }
                              
                             } while (true);

                             Value = (ushort)PLCTrigerIndex;
                             b1 = BitConverter.GetBytes(Value);
                             b1 = b1.Reverse().ToArray();
                             if (ppp.Result)
                             {
                                 Value = (ushort)2;
                             }
                             else
                             {
                                 Value = (ushort)3;
                             }
                             b2 = BitConverter.GetBytes(Value);
                             b2 = b2.Reverse().ToArray();
                             arrb = new List<byte>();
                             arrb.AddRange(b1);
                             arrb.AddRange(b2);

                             //s1200Write.Write("DB1000.DBW" + StartByte, Value);
                             //     UInt32 b1111 = BitConverter.ToUInt32(arrb.ToArray(), 0);
                             WriteBytes(DataType.DataBlock, DBIndex, plcST.id * 16 + 14, arrb.ToArray());
                             //  d1("DB" + DBIndex + ".DBD" + (plcST.id * 16 + 14), b1111);
                             //if (ec1 != ErrorCode.NoError)
                             //{
                             //    throw new Exception("写入PLC数据出错：" + ec1.ToString());
                             //}
                           //  Stopwatch spp = new Stopwatch();
                             //spp.Restart();
                             //do
                             //{
                             //    if (this.mypplc.PLCStationlst[plcST.id].PLC2PCEventConfig.Eventlst[PLCTrigerIndex].plcEventState != PLCEventState.Request)
                             //    {
                             //        break;
                             //    }
                             //    Thread.Sleep(2);
                             //} while (true);
                             //spp.Stop();
                             //if (plcST.id == 0)
                             //{
                             //    Console.WriteLine("第一工位耗时：" + CommonSp.ElapsedMilliseconds);
                             //}

                         }
                     }
                     else
                     {
                         throw new Exception("方法为空");
                     }

                 }
                 catch (Exception ex)
                 {
                    // pe.IsPCInHanding = false;
                     if(Dictask.ContainsKey(plcST.id))
                     {
                       Dictask[plcST.id].ErroString = ex.ToString();
                     }
                     //throw new Exception(ex.ToString());

                 }
                 sp.Stop();
                 //     Console.WriteLine("事件" + plcST.id + "结束：" + sp.ElapsedMilliseconds);
                 pe.IsPCInHanding = false;
                 //  Console.WriteLine("ishanding =false");

             }
        , ct);

            CutromTask myt = new CutromTask(t);
            myt.PLCName = mypplcHandle.Name;
            myt.StationName = plcST.Name;
            myt.EventName = pe.Name;

            myt.PLCId = mypplcHandle.id;
            myt.StationId = plcST.id;
            myt.EventId = pe.id;
            myt.IsAbort = false;

            if (!Dictask.TryAdd(plcST.id, myt))
            {

                throw new Exception("把任务放入队列失败！！");
            }

        }

        public void DisposeEvent(int plcStationId, int eventId,int Timeout)
        {
            if (Dictask.ContainsKey(plcStationId))
            {
                Dictask[plcStationId].IsAbort = true;
            }
         //  Dictask[plcStationId].task.AsyncState


            //Stopwatch sp = new Stopwatch();
            //sp.Start();
            //do
            //{
            //    Thread.Sleep(100);
            //    if( Dictask[plcStationId].task.IsCompleted || Dictask[plcStationId].task.IsCanceled || Dictask[plcStationId].task.IsFaulted)
            //    {
            //        break;
            //    }
            //    if (sp.ElapsedMilliseconds > Timeout)
            //    {
            //        throw new Exception("重设置事件超时！");
            //    }
            //} while (true);
            //this.mypplcHandle.PLCStationlst[plcStationId].PLC2PCEventConfig.Eventlst[eventId].IsPCInHanding = false;
        }

        #endregion

        public void SetCanWork()
        {
            CanWork = true;
        }

        public void ResetNotWork()
        {
            CanWork = false;
        }


        public PLCS7(object libMethod,PLCHandle plchandle)
        {
            mLibMethod = libMethod;
            mypplcHandle = plchandle;
        }


        public bool OpenPLCServer()
        {
            try
            {
                //初始化数据
                //根据配置文件计算高速区字节数
                TaskStateDataTable = new DataTable();


                TaskStateDataTable.Columns.Add("PLCid");
                TaskStateDataTable.Columns.Add("PLCName");
                TaskStateDataTable.PrimaryKey = new DataColumn[] { TaskStateDataTable.Columns.Add("StationId") };
                TaskStateDataTable.Columns.Add("StationName");
                TaskStateDataTable.Columns.Add("EventId");
                TaskStateDataTable.Columns.Add("EventName");
                TaskStateDataTable.Columns.Add("EventState");
                TaskStateDataTable.Columns.Add("Erro");

                PLCFlowDataTable = new DataTable();
                PLCFlowDataTable.Columns.Add("PLCid");
                PLCFlowDataTable.Columns.Add("PLCName");
                PLCFlowDataTable.PrimaryKey = new DataColumn[] { PLCFlowDataTable.Columns.Add("StationId") };
                PLCFlowDataTable.Columns.Add("StationName");
                PLCFlowDataTable.Columns.Add("FlowId");
                PLCFlowDataTable.Columns.Add("FlowMessage");

                PLCWarningDataTable = new DataTable();
                PLCWarningDataTable.Columns.Add("PLCid");
                PLCWarningDataTable.Columns.Add("PLCName");
                PLCWarningDataTable.Columns.Add("StationId");
                PLCWarningDataTable.Columns.Add("StationName");
                PLCWarningDataTable.Columns.Add("WarningId");
                PLCWarningDataTable.Columns.Add("Message");
                //   PLCWarningDataTable.PrimaryKey = new DataColumn[] {PLCWarningDataTable.Columns["StationId"] ,PLCWarningDataTable.Columns["WarningId"]  };


                Dictask.Clear();


                ip = mypplcHandle.ip;
                port = mypplcHandle.port;
                cts = new CancellationTokenSource();
                ct = cts.Token;
                lcts = new LimitedConcurrencyLevelTaskScheduler(mypplcHandle.ThreadNum);
                MyTaskHandle = new TaskFactory(lcts);


                for (int i = 0; i < mypplcHandle.PLCStationlst.Count; i++)
                {
                    mypplcHandle.PLCStationlst[i].InitDataTable();

                    for (int j = 0; j < mypplcHandle.PLCStationlst[i].PLC2PCEventConfig.Eventlst.Count; j++)
                    {
                        mypplcHandle.PLCStationlst[i].PLC2PCEventConfig.Eventlst[j].IsPCInHanding = false;
                    }
                    for (int j = 0; j < mypplcHandle.PLCStationlst[i].PC2PLCEventConfig.Eventlst.Count; j++)
                    {
                        mypplcHandle.PLCStationlst[i].PC2PLCEventConfig.Eventlst[j].IsPCInHanding = false;
                    }
                }


                HighSpeedStationInfoLength = mypplcHandle.PLCStationlst.Count * StationByteCounter + HeadByte;
                DBIndex = mypplcHandle.DBIndex;

                CpuType cputype = CpuType.S71200;
                PLCs7Read = new Plc(cputype, mypplcHandle.ip, mypplcHandle.Rack, mypplcHandle.Slot);
                PLCs7Write = new Plc(cputype, mypplcHandle.ip, mypplcHandle.Rack, mypplcHandle.Slot);
                //   ErrorCode ec = PLCs7Write.Open();
                //if (ec != ErrorCode.NoError)
                //{
                //    throw new Exception("连接失败：" + ec.ToString());
                //}


                StartRead();
                //StarTrigerHandle();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void ClosePLCServer()
        {
            StopRead();
            Dictask.Clear();
            cts.Dispose();
            lcts = null;
            MyTaskHandle = null;
        }

        /// <summary>
        /// 开始读取
        /// </summary>
        private void StartRead()
        {


            //  Tempbytes = new Byte[HighSpeedStationInfoLength];
            if (thRead == null || !thRead.IsAlive)
            {
                thReadFlag = true;
                thRead = new Thread(KeepRead);
                thRead.Start();
            }
        }

        /// <summary>
        /// 停止读取
        /// </summary>
        private void StopRead()
        {
            if (thRead != null && thRead.IsAlive)
            {
                thReadFlag = false;
                Stopwatch sp = new Stopwatch();
                sp.Start();
                if (this.IsConnect)
                {
                    do
                    {
                        Thread.Sleep(200);
                        if (!thRead.IsAlive)
                        {
                            break;
                        }
                        if (sp.ElapsedMilliseconds > 5000)
                        {
                            thRead.Abort();
                            break;
                        }
                    } while (true);
                }
                else
                {
                   //  this.IsConnect = false;
                     PLCs7Read.Close();
                     PLCs7Write.Close();
                     Thread.Sleep(200);
                     thRead.Abort();
                     //Thread.Sleep(200);
                }
            }
            this.IsConnect = false;
        }

        private void KeepRead()
        {
            System.Diagnostics.Stopwatch sp = new Stopwatch();
            int StationIndex = 0;
            sp.Start();
            bool Fl = false;
            byte[] HeartBeatAndWorkType = new byte[] { 0,1 };
            try
            {
                do
                {
                    Thread.Sleep(1);
                    try
                    {
                        #region 写入心跳
                      
                        if (PLCs7Write.IsAvailable && PLCs7Write.IsConnected)
                        {
                            if (sp.ElapsedMilliseconds > 1500)
                            {

                                if (SetCurrentWorkType == 0)
                                {
                                    Console.WriteLine();
                                }
                                if (Fl)
                                {
                                 
                                    HeartBeatAndWorkType[0] = SetCurrentWorkType;
                                    HeartBeatAndWorkType[1] = 0;
                                    WriteBytes(DataType.DataBlock, DBIndex, 0, HeartBeatAndWorkType);
                                }
                                else
                                {
                                    HeartBeatAndWorkType[0] = SetCurrentWorkType;
                                    HeartBeatAndWorkType[1] = 1;
                                    WriteBytes(DataType.DataBlock, DBIndex, 0, HeartBeatAndWorkType);
                                }
                                Fl = !Fl;
                                sp.Restart();
                            }
                        }


                        #endregion




                        if (PLCs7Read.IsAvailable && PLCs7Read.IsConnected && PLCs7Write.IsAvailable && PLCs7Write.IsConnected)
                        {
                            #region PLC读取
                            //sp.Restart();
                            lock (lockObj)
                            {
                                Tempbytes = PLCs7Read.ReadBytes(DataType.DataBlock, DBIndex, StartAddress, HighSpeedStationInfoLength);
                            }
                            
                            //object A = PLCs7Read.Read("DB12.DBB4.0");
                            if (Tempbytes != null && Tempbytes.Length == this.HighSpeedStationInfoLength)
                            {
                                //进行赋值处理
                                //分包
                                //心跳区
                                GetCurrentWorkType = Tempbytes[0];
                                #region 分站解析
                                StationIndex = 0;
                                for (int i = HeadByte; i < Tempbytes.Length; i += StationByteCounter)
                                {
                                    StationIndex++;
                                    PLCStation CurrentSt = mypplcHandle.PLCStationlst[StationIndex - 1];

                                    if (!CurrentSt.IsEnbale)
                                    {
                                        continue;
                                    }

                                    //急停报警
                                    #region 急停报警
                                    byte[] EMGWarning = new byte[2];
                                    EMGWarning[1] = Tempbytes[i];
                                    EMGWarning[0] = Tempbytes[i + 1];
                                    //ushort a =  System.BitConverter.ToUInt16(EMGWarning,0);
                                    string Temp = EMGWarning.ValToBinString();
                                    if (CurrentSt.id == 0)
                                    {
                                        // Console.WriteLine("ST0"+"急停报警:" + Temp);
                                    }
                                    CurrentSt.WarningConfig.SetEMGWarningBitString(EMGWarning.ValToBinString());
                                    Temp = new string(Temp.Reverse().ToArray());
                                    for (int EMGindex = 0; EMGindex < CurrentSt.WarningConfig.EMGMessagelst.Count; EMGindex++)
                                    {
                                        if (Temp[EMGindex] == '1')
                                        {
                                            CurrentSt.WarningConfig.EMGMessagelst[EMGindex].Trigered = true;
                                        }
                                        else
                                        {
                                            CurrentSt.WarningConfig.EMGMessagelst[EMGindex].Trigered = false;
                                        }
                                    }

                                    #endregion

                                    #region 异常报警
                                    int WarningStartIndex = 2;
                                    byte[] Warning = new byte[4];
                                    Warning[0] = Tempbytes[i + WarningStartIndex];
                                    Warning[1] = Tempbytes[i + WarningStartIndex + 1];
                                    Warning[2] = Tempbytes[i + WarningStartIndex + 2];
                                    Warning[3] = Tempbytes[i + WarningStartIndex + 3];

                                    Temp = Warning.ValToBinString();

                                    if (CurrentSt.id == 0)
                                    {
                                        //Console.WriteLine("ST0" + "报警:" + Temp);
                                    }

                                    CurrentSt.WarningConfig.SetWarningBitString(Warning.ValToBinString());

                                    Temp = new string(Temp.Reverse().ToArray());
                                    for (int Warningindex = 0; Warningindex < CurrentSt.WarningConfig.WMessagelst.Count; Warningindex++)
                                    {
                                        if (Temp[Warningindex] == '1')
                                        {
                                            CurrentSt.WarningConfig.WMessagelst[Warningindex].Trigered = true;
                                        }
                                        else
                                        {
                                            CurrentSt.WarningConfig.WMessagelst[Warningindex].Trigered = false;
                                        }
                                    }


                                    #endregion

                                    #region 流程
                                    int FlowarningStartIndex = 6;
                                    byte[] Flow = new byte[2];
                                    Flow[0] = Tempbytes[i + FlowarningStartIndex + 1];
                                    Flow[1] = Tempbytes[i + FlowarningStartIndex];
                                    CurrentSt.FlowConfig.SetCurrentFlow(System.BitConverter.ToInt16(Flow, 0));

                                    #endregion

                                    //Console.WriteLine("急停报警：" + CurrentSt.WarningConfig.GetEMGWarningBitString());
                                    //Console.WriteLine("异常报警：" + CurrentSt.WarningConfig.GetWarningBitString());
                                    //Console.WriteLine("流程字：" +       CurrentSt.FlowConfig.GetCurrentFlow());
                                    #region PLC-PC
                                    int PLC_PCIndex = 8;
                                    byte[] PLC_PCFucNo = new byte[2];
                                    PLC_PCFucNo[0] = Tempbytes[i + PLC_PCIndex + 1];
                                    PLC_PCFucNo[1] = Tempbytes[i + PLC_PCIndex];
                                    int PLCEventIndex = System.BitConverter.ToUInt16(PLC_PCFucNo, 0);//获取功能码

                                    byte[] PLC_PCFucState = new byte[2];
                                    PLC_PCFucState[0] = Tempbytes[i + PLC_PCIndex + 3];
                                    PLC_PCFucState[1] = Tempbytes[i + PLC_PCIndex + 2];
                                    int PLCFucState = System.BitConverter.ToInt16(PLC_PCFucState, 0);//获取状态
                                    if (PLCEventIndex < 0xFF && PLCEventIndex >= 0)
                                    {
                                        if (CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].IsEnbale)//判断该事件是否开启
                                        {
                                            CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].plcEventState = GetEventStateValue(PLCFucState);//给事件状态赋值
                                            if (CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].plcEventState == PLCEventState.Request)
                                            {
                                                // Console.WriteLine("请求~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                                                if ( CanWork)
                                                {
                                                    if (!CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].IsPCInHanding)
                                                    {
                                                        CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].IsPCInHanding = true;
                                                        //  Console.WriteLine("ishanding =true");
                                                        //触发事件
                                                        //if (CurrentSt.id == 0)
                                                        //{
                                                        //    Console.WriteLine("第一站开始:");
                                                        //    CommonSp.Restart();
                                                        //}
                                                        RunPLCTrigerTask(CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex], CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].id, CurrentSt);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("PLC不在自动模式或PLC没有开始工作。");
                                                }
                                                
                                            }

                                            //if (CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].plcEventState == PLCEventState.InHanding && CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].IsPCInHanding == false)
                                            //{
                                            //    CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].IsPCInHanding = true;
                                            //    //  Console.WriteLine("ishanding =true");
                                            //    //触发事件
                                            //    if (CurrentSt.id == 0)
                                            //    {
                                            //        Console.WriteLine("第一站开始:");
                                            //        CommonSp.Restart();
                                            //    }
                                            //    RunPLCTrigerTask(CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex], CurrentSt.PLC2PCEventConfig.Eventlst[PLCEventIndex].id, CurrentSt);
                                            //}
                                        }
                                        for (int eventi = 0; eventi < CurrentSt.PLC2PCEventConfig.Eventlst.Count; eventi++)
                                        {
                                            if (eventi != PLCEventIndex)
                                            {
                                                CurrentSt.PLC2PCEventConfig.Eventlst[eventi].plcEventState = PLCEventState.NC;
                                            }
                                        }
                                    }

                                    #endregion

                                    #region PC-PLC
                                    int PC_PLCIndex = 12;
                                    byte[] PC_PLCFucNo = new byte[2];
                                    PC_PLCFucNo[0] = Tempbytes[i + PC_PLCIndex + 1];
                                    PC_PLCFucNo[1] = Tempbytes[i + PC_PLCIndex + 0];
                                    int PCEventIndex = System.BitConverter.ToInt16(PC_PLCFucNo, 0);//获取功能码

                                    byte[] PC_PLCFucState = new byte[2];
                                    PC_PLCFucState[0] = Tempbytes[i + PC_PLCIndex + 3];
                                    PC_PLCFucState[1] = Tempbytes[i + PC_PLCIndex + 2];
                                    int PCFucState = System.BitConverter.ToInt16(PC_PLCFucState, 0);//获取状态


                                    if (PCEventIndex < 0xFF && PCEventIndex >= 0)
                                    {
                                        //for (int PCeventi = 0; PCeventi < CurrentSt.PC2PLCEventConfig.Eventlst.Count; PCeventi++)
                                        //{
                                        CurrentSt.PC2PLCEventConfig.Eventlst[PCEventIndex].plcEventState = GetEventStateValue(PCFucState);//给事件状态赋值
                                        //}
                                        //if (CurrentSt.id == 1)
                                        //{
                                        //    Console.WriteLine("站：" + CurrentSt.id + "," + "状态:" + CurrentSt.PC2PLCEventConfig.Eventlst[PCEventIndex].plcEventState);
                                        //}
                                        //if (CurrentSt.PC2PLCEventConfig.Eventlst[PCEventIndex].plcEventState != PLCEventState.NC)
                                        //{
                                        //    
                                        //}
                                    }
                                    for (int eventi = 0; eventi < CurrentSt.PC2PLCEventConfig.Eventlst.Count; eventi++)
                                    {
                                        if (eventi != PCEventIndex)
                                        {
                                            CurrentSt.PC2PLCEventConfig.Eventlst[eventi].plcEventState = PLCEventState.NC;
                                        }
                                    }
                                    #endregion

                                }

                                #endregion


                            }
                            //Console.WriteLine("读取时间:" + sp.ElapsedMilliseconds + "读取长度:" + Tempbytes.Length);
                            //sp.Stop();
                            #endregion
                            IsConnect = true;
                        }
                        else
                        {
                            IsConnect = false;
                            #region 断线重连
                            if (!thReadFlag)
                            {
                                break;
                            }
                            if (!PLCs7Read.IsAvailable || !PLCs7Read.IsConnected)
                            {
                                Console.WriteLine(mypplcHandle.Name + ":读取客户端断线重连:");
                                ErrorCode d1 = PLCs7Read.Open();
                            }
                            if (!thReadFlag)
                            {
                                break;
                            }
                            if (!PLCs7Write.IsAvailable || !PLCs7Write.IsConnected)
                            {
                                Console.WriteLine(mypplcHandle.Name + ":写入客户端断线重连:");
                                ErrorCode d1 = PLCs7Write.Open();
                            }
                            Thread.Sleep(1500);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        IsConnect = false;
                        Console.WriteLine(ex.Message);
                    }
                } while (thReadFlag);
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        private void WriteBytes(DataType dataType, int db, int startByteAdr, byte[] value)
        {
            //if (value.Length == 2)
            //{
            //    if (value[0] != 1)
            //    {
            //        Console.WriteLine();
            //    }
            //}

            // ErrorCode er = PLCs7Write.WriteBytes(dataType, db, startByteAdr, value);
            Stopwatch sp = new Stopwatch();
            sp.Restart();
            do
            {
                if (PLCs7Write.IsAvailable && PLCs7Write.IsConnected)
                {
                    ErrorCode er = ErrorCode.ConnectionError;
                    lock (lockObj)
                    {
                        er = PLCs7Write.WriteBytes(dataType, db, startByteAdr, value);
                    }
                    if (er != ErrorCode.NoError)
                    {
                        //throw new Exception("发送数据失败。");
                        Console.WriteLine(mypplcHandle.id + "PLC:" + mypplcHandle.Name + ",发送数据失败。" + er.ToString());
                    }
                    else
                    {
                        break;
                    }

                }
                if (sp.ElapsedMilliseconds > SendTimeOut)
                {
                    throw new Exception("发送数据超时。");
                }
                Thread.Sleep(1);
            } while (true);

        }

        ///// <summary>
        ///// pc请求plc工作
        ///// </summary>
        ///// <param name="para"></param>
        ///// <param name="pe"></param>
        ///// <param name="plcST"></param>
        ///// <returns></returns>
        //private object SendPCTrigerTask(object para, PLCEvent pe, PLCStation plcST)
        //{
        //    bool a = false;
        //    bool returnvalue = false;
        //    ParaAndResultStrut pr1 = null;
        //    try
        //    {

        //        if (pe.Sender == SenderType.PC)
        //        {
        //            ParaAndResultStrut pr = IntergrationPara(para);
        //            if (pe.PrType == PLCPCPrType.BYTEArr)
        //            {
        //                if (pr.Data.Length <= pe.PrLength)
        //                {
        //                    //写入参数
        //                    WriteBytes(DataType.DataBlock, DBIndex, plcST.PcParamAndRes_StartAddress, pr.Data);
        //                }
        //                else
        //                {
        //                    throw new Exception("参数超过PLC定义的长度");
        //                }
        //            }

        //            ushort Value = (ushort)pe.id;
        //            byte[] b1 = BitConverter.GetBytes(Value);
        //            b1 = b1.Reverse().ToArray();
        //            Value = (ushort)1;
        //            byte[] b2 = BitConverter.GetBytes(Value);
        //            b2 = b2.Reverse().ToArray();
        //            List<byte> arrb = new List<byte>();
        //            arrb.AddRange(b1);
        //            arrb.AddRange(b2);

        //            //s1200Write.Write("DB1000.DBW" + StartByte, Value);
        //            //     UInt32 b1111 = BitConverter.ToUInt32(arrb.ToArray(), 0);
        //            WriteBytes(DataType.DataBlock, DBIndex, plcST.id * 16 + 18, arrb.ToArray());

        //            pr1 = new ParaAndResultStrut();
        //            do
        //            {
        //                if (pe.plcEventState == PLCEventState.ResponseOK)
        //                {
        //                    pr1.Result = true;
        //                    break;
        //                }
        //                if (pe.plcEventState == PLCEventState.ResponseNG)
        //                {
        //                    pr1.Result = false;
        //                    break;
        //                }
        //                Thread.Sleep(10);
        //            } while (true);

        //            if (pe.ResultValueType != PLCPCPrType.NULL)
        //            {

        //                byte[] b = PLCs7Read.ReadBytes(DataType.DataBlock, DBIndex, plcST.PcParamAndRes_StartAddress, pe.PrLength);
        //                pr1.type = PRType.BYTEArr;
        //                pr1.Length = (byte)pe.ResultValueLength;
        //                //Array.Copy(b, 0, pr1.Data, 0, pr1.Length);
        //                pr1.Data = b;

        //                return AnalysisPara(pr1);
        //            }
        //            else
        //            {
        //                pr1.type = PRType.Null;
        //                pr1.Length = 0;
        //                //Array.Copy(b, 0, pr1.Data, 0, pr1.Length);
        //                pr1.Data = null;

        //            }



        //        }
        //    }
        //    catch (Exception)
        //    {
        //        pr1 = null;
        //    }
        //    return pr1;
        //}

        /// <summary>
        /// pc请求plc工作
        /// </summary>
        /// <param name="para"></param>
        /// <param name="pe"></param>
        /// <param name="plcST"></param>
        /// <returns></returns>
        private ParaAndResultStrut SendPCTrigerTask(object para, int plcSTid, int peindex)
        {
            bool a = false;
            bool returnvalue = false;
            ParaAndResultStrut pr1 = null;
            // this.mypplc.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex];

            try
            {

                ushort Value = (ushort)this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].id;
                byte[] b1 = BitConverter.GetBytes(Value);
                b1 = b1.Reverse().ToArray();
                Value = (ushort)0;
                byte[] b2 = BitConverter.GetBytes(Value);
                b2 = b2.Reverse().ToArray();
                List<byte> arrb = new List<byte>();
                arrb.AddRange(b1);
                arrb.AddRange(b2);

                //s1200Write.Write("DB1000.DBW" + StartByte, Value);
                //     UInt32 b1111 = BitConverter.ToUInt32(arrb.ToArray(), 0);
                WriteBytes(DataType.DataBlock, DBIndex, this.mypplcHandle.PLCStationlst[plcSTid].id * 16 + 18, arrb.ToArray());
                Stopwatch sp = new Stopwatch();
                sp.Restart();

                do
                {
                    if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].plcEventState == PLCEventState.NC)
                    {
                        break;
                    }
                    Thread.Sleep(2);
                    if (sp.ElapsedMilliseconds > 3000)
                    {
                        throw new Exception("PC-PLC重置事件超时：" + "站号:" + plcSTid + "," + "事件号:" + peindex);
                    }
                } while (true);




                if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].Sender == SenderType.PC)
                {
                    ParaAndResultStrut pr = IntergrationPara(para);
                    if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].PrType == PLCPCPrType.BYTEArr)
                    {
                        if (pr.Data.Length <= this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].PrLength)
                        {
                            //写入参数
                            WriteBytes(DataType.DataBlock, DBIndex, this.mypplcHandle.PLCStationlst[plcSTid].PcParamAndRes_StartAddress, pr.Data);
                        }
                        else
                        {
                            throw new Exception("参数超过PLC定义的长度");
                        }
                    }

                    Value = (ushort)this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].id;
                    b1 = BitConverter.GetBytes(Value);
                    b1 = b1.Reverse().ToArray();
                    Value = (ushort)1;
                    b2 = BitConverter.GetBytes(Value);
                    b2 = b2.Reverse().ToArray();
                    arrb = new List<byte>();
                    arrb.AddRange(b1);
                    arrb.AddRange(b2);

                    //s1200Write.Write("DB1000.DBW" + StartByte, Value);
                    //     UInt32 b1111 = BitConverter.ToUInt32(arrb.ToArray(), 0);
                    WriteBytes(DataType.DataBlock, DBIndex, this.mypplcHandle.PLCStationlst[plcSTid].id * 16 + 18, arrb.ToArray());

                    pr1 = new ParaAndResultStrut();
                    sp.Restart();
                    do
                    {
                        if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].plcEventState == PLCEventState.ResponseOK)
                        {
                            pr1.Result = true;
                            break;
                        }
                        if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].plcEventState == PLCEventState.ResponseNG)
                        {
                            pr1.Result = false;
                            break;
                        }
                        //if (sp.ElapsedMilliseconds > 3000)
                        //{
                        //    throw new Exception("PC-PLC接收PLC回复超时：" + "站号:" + plcSTid + "," + "事件号:" + peindex);
                        //}
                        Thread.Sleep(2);
                    } while (true);

                    if (this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].ResultValueType != PLCPCPrType.NULL)
                    {
                        byte[] b = null;
                        lock (lockObj)
                        {
                            b = PLCs7Read.ReadBytes(DataType.DataBlock, DBIndex, this.mypplcHandle.PLCStationlst[plcSTid].PcParamAndRes_StartAddress, this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].PrLength);
                        }
                        pr1.type = PRType.BYTEArr;
                        pr1.Length = (byte)this.mypplcHandle.PLCStationlst[plcSTid].PC2PLCEventConfig.Eventlst[peindex].ResultValueLength;
                        //Array.Copy(b, 0, pr1.Data, 0, pr1.Length);
                        pr1.Data = b;
                        return pr1;

                    }
                    else
                    {
                        pr1.type = PRType.Null;
                        pr1.Length = 0;
                        //Array.Copy(b, 0, pr1.Data, 0, pr1.Length);
                        pr1.Data = null;
                    }
                }
            }
            catch (Exception)
            {
                pr1 = null;
            }
            return pr1;
        }

        public ParaAndResultStrut ExcutPC2PLCEvent(object para, string StationName, string EventName)
        {
            //找到PLC
            ParaAndResultStrut returnValue = null;
            try
            {
                PLCHandle plch = this.mypplcHandle;
                PLCStation plt = null;
                PLCEvent pe = null;

                int StationId = -1;
                int EventID = -1;


                //寻找StationID


                if (plch != null)
                {


                    //找到该PLC的Station
                    foreach (PLCStation pt in plch.PLCStationlst)
                    {
                        if (StationName == pt.Name)
                        {
                            StationId = pt.id;
                            break;
                        }
                    }
                    if (StationId != -1)
                    {
                        foreach (PLCEvent ple in plch.PLCStationlst[StationId].PC2PLCEventConfig.Eventlst)
                        {
                            if (EventName == ple.Name)
                            {
                                EventID = ple.id;
                                break;
                            }
                        }
                        if (EventID != -1)
                        {
                            returnValue = this.SendPCTrigerTask(para, StationId, EventID);

                        }
                        else
                        {
                            throw new Exception("未找到指定事件");
                        }
                    }
                    else
                    {
                        throw new Exception("未找到指定PLC站");
                    }
                }
                else
                {
                    throw new Exception("未找到指定PLC模块");
                }

            }
            catch (Exception ex)
            {
                //throw new Exception("写入PLC数据报错");
                throw ex;
            }
            return returnValue;
        }

        public object ExcutPC2PLCEvent(object para, int StationId, int EventID)
        {
            //找到PLC
            object returnValue = null;
            try
            {
                PLCHandle plch = this.mypplcHandle;
                PLCStation plt = null;
                PLCEvent pe = null;
                if (StationId < plch.PLCStationlst.Count)
                {
                    plt = plch.PLCStationlst[StationId];
                }
                else
                {
                    throw new Exception("未找到指定工作站");
                }
                if (EventID < plt.PC2PLCEventConfig.Eventlst.Count)
                {

                    returnValue = this.SendPCTrigerTask(para, StationId, EventID);

                }
                else
                {
                    throw new Exception("未找到指定事件");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public DataTable GetCurrentStWarning(int StationId)
        {
            if (StationId < mypplcHandle.PLCStationlst.Count)
            {
                return mypplcHandle.PLCStationlst[StationId].GetCurrentWarning();
            }
            else
            {
                //throw new Exception("未找到该站。");
                return null;
            }
        }

        public DataTable GetCunrrentStEMGWarning(int StationId)
        {
            if (StationId < mypplcHandle.PLCStationlst.Count)
            {
                return mypplcHandle.PLCStationlst[StationId].GetCurrentEMGWarning();
            }
            else
            {
                return null;
            }
        }


        public int GetCurrentStFlowMessage(int StationId, out string Message)
        {
            if (StationId < mypplcHandle.PLCStationlst.Count)
            {
                return mypplcHandle.PLCStationlst[StationId].GetCurrentFlowMessage(out Message);
            }
            else
            {
                throw new Exception("未找到该站。");
            }
        }




        public DataTable GetStationFlows()
        {
            if (PLCFlowDataTable != null)
            {
                foreach (PLCStation plcst in mypplcHandle.PLCStationlst)
                {
                    string s = "";
                    int Flowid = plcst.GetCurrentFlowMessage(out s);
                    if (PLCFlowDataTable.Rows.Contains(plcst.id))
                    {
                        DataRow dr = PLCFlowDataTable.Rows.Find(plcst.id);
                        dr["FlowId"] = Flowid;
                        dr["FlowMessage"] = s;
                        //StationFlowDataTable = new DataTable();
                        //StationFlowDataTable.Columns.Add("PLCid");
                        //StationFlowDataTable.Columns.Add("PLCName");
                        //StationFlowDataTable.PrimaryKey = new DataColumn[] { TaskStateDataTable.Columns.Add("StationId") };
                        //StationFlowDataTable.Columns.Add("StationName");
                        //StationFlowDataTable.Columns.Add("FlowId");
                        //StationFlowDataTable.Columns.Add("FlowMessage");
                    }
                    else
                    {
                        PLCFlowDataTable.Rows.Add(mypplcHandle.id, mypplcHandle.Name, plcst.id, plcst.Name, Flowid, s);
                        //      TaskStateDataTable.Rows.Add(new object[] { kp.Value.PLCId, kp.Value.PLCName, kp.Value.StationId, kp.Value.StationName, kp.Value.EventId, kp.Value.EventName, kp.Value.task.Status.ToString() });

                    }
                }
            }
            return PLCFlowDataTable;

        }

        public DataTable GetTaskStateData()
        {
            //TaskStateDataTable.Clear();


            if (TaskStateDataTable != null)
            {

                foreach (KeyValuePair<int, CutromTask> kp in Dictask)
                {
                    if (TaskStateDataTable.Rows.Contains(kp.Value.StationId))
                    {
                        DataRow dr = TaskStateDataTable.Rows.Find(kp.Value.StationId);
                        dr["EventId"] = kp.Value.EventId;
                        dr["EventName"] = kp.Value.EventName;
                        dr["EventState"] = kp.Value.task.Status.ToString();
                        dr["Erro"] = kp.Value.ErroString;

                    }
                    else
                    {
                        TaskStateDataTable.Rows.Add(new object[] { kp.Value.PLCId, kp.Value.PLCName, kp.Value.StationId, kp.Value.StationName, kp.Value.EventId, kp.Value.EventName, kp.Value.task.Status.ToString(), kp.Value.ErroString });

                    }
                }
            }
            return TaskStateDataTable;
        }

        public DataTable GetEMGs()
        {
            //TaskStateDataTable.Clear();


            if (PLCWarningDataTable != null)
            {
                PLCWarningDataTable.Rows.Clear();
                foreach (PLCStation plcst in mypplcHandle.PLCStationlst)
                {
                    DataTable dtt = GetCunrrentStEMGWarning(plcst.id);
                    if (dtt != null)
                    {
                        foreach (DataRow dr in dtt.Rows)
                        {
                            PLCWarningDataTable.Rows.Add(new object[] { mypplcHandle.id, mypplcHandle.Name, plcst.id, plcst.Name, dr["EMGWarningID"].ToString(), dr["Message"].ToString() });
                        }
                    }

                }
                return PLCWarningDataTable;

            }
            else
            {
                return null;
            }


        }

        #region  监控界面绑定

      

        public void InitControl(TabControl tabControl)
        {
            tabPage = new TabPage();
            tabPage.Name = "PLC" + mypplcHandle.id;
            tabPage.Text = mypplcHandle.Name;
            //MonitorControl.Parent = tabControl;
            MonitorControl = new OnePLCMonitor(this, tabControl);
            tabPage.Controls.Add(MonitorControl);
            MonitorControl.Dock = DockStyle.Fill;
          //  MonitorControl.StarMonitor();
           

        }
       
        public void DisposeControl()
        {
            MonitorControl.StopMonitor();
        }



        #endregion
    }
    


    public class CutromTask 
    {
        public int PLCId;
        public int StationId;
        public int EventId;



        public string PLCName;
        public string StationName;
        public string EventName;

        public bool IsAbort = false; 
        public Task task;

        public string ErroString="NoErro";
        public void Dispose()
        {
            EventName = null;
            ErroString = null;
            task.Dispose();
        }
        public CutromTask( Task t)
        {
            task = t;
        }
    }



    public class ParaAndResultStrut
    {
        public PRType type = PRType.Null;
        public byte Length = 0;
        public byte[] Data = null;
        public bool Result = true;
        public int StationId = 0;
    }

    public enum PRType
    {
        Null = 0,
        PLCBool = 1,
        PLCString = 2,
        PLCInt = 3,
        BYTEArr = 4
    }

}
