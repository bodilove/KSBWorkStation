
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Data;
namespace Common.PLCServer
{
    public abstract class AGeneralCf
    {
        public string Name = "";
        public int id = 0;
        public bool IsEnbale = true;
    }

    [Serializable]
    public enum SenderType
    {
        /// <summary>
        /// 请求者为PLC
        /// </summary>
        PLC = 0x10,

        /// <summary>
        /// 请求者为PC
        /// </summary>
        PC = 0x20,
    }


    public enum PLCPCPrType
    {
        /// <summary>
        /// 空类型
        /// </summary>
        NULL = 0,
        /// <summary>
        /// BYTE数组
        /// </summary>
        BYTEArr = 1
    }

    [Serializable]
    public enum PLCEventState
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        NC = 0,

        /// <summary>
        /// 请求
        /// </summary>
        Request = 1,


        /// <summary>
        /// 处理中
        /// </summary>
        InHanding = 0xFF,

        /// <summary>
        /// 返回值OK
        /// </summary>
        ResponseOK = 2,


        /// <summary>
        /// 返回值NG
        /// </summary>
        ResponseNG = 3,

        /// <summary>
        /// 处理中
        /// </summary>
        DeviceEroo = 0x0F,

    }

    public class PLCEventConfig
    {
        public int PLCEvent_StartAddress = 0;

        public List<PLCEvent> Eventlst = new List<PLCEvent>();
        public PLCEventConfig Clone()
        {
            PLCEventConfig p = new PLCEventConfig();
            p.PLCEvent_StartAddress = this.PLCEvent_StartAddress;
            p.Eventlst = new List<PLCEvent>();
            foreach (PLCEvent pe in Eventlst)
            {
                p.Eventlst.Add(pe.Clone());
            }
            return p;
        }
    }

    [XmlRoot("PLCEvent")]
    public class PLCEvent : AGeneralCf
    {
        public PLCEvent(){}

        /// <summary>
        /// 是否启用
        /// </summary>
     //   public bool Enable = false;

        /// <summary>
        /// 事件的请求者是PLC还是PC
        /// </summary>
        public SenderType Sender = SenderType.PLC;

        public PLCEventState plcEventState = PLCEventState.NC;

        public bool IsPCInHanding = false;

        /// <summary>
        /// 事件描述
        /// </summary>
        public string EventDescription = "事件描述";


        public PLCCommand Mycommand = new PLCCommand();


        /// <summary>
        /// 参数类型
        /// </summary>
        public PLCPCPrType PrType = PLCPCPrType.NULL;

        public int PrLength = 0;


        public PLCPCPrType ResultValueType = PLCPCPrType.NULL;


        public int ResultValueLength = 0;
        public PLCEvent Clone()
        {

            PLCEvent spNew = new PLCEvent();
            spNew.id = this.id;
            spNew.IsEnbale = this.IsEnbale;

            spNew.Name = this.Name;
            spNew.IsPCInHanding = this.IsPCInHanding;
            spNew.Mycommand = new PLCCommand();
            spNew.Mycommand.DeviceName = this.Mycommand.DeviceName;
            spNew.Mycommand.id = this.Mycommand.id;
            spNew.Mycommand.DeviceName = this.Mycommand.DeviceName;
            spNew.Mycommand.Instance = this.Mycommand.Instance;
            spNew.Mycommand.IsEnbale = this.IsEnbale;
            spNew.Mycommand.MethodName = this.Mycommand.MethodName;
            spNew.Mycommand.Name = this.Mycommand.Name;
            spNew.Mycommand.Parameters = this.Mycommand.Parameters;

            spNew.Name = this.Name;
            spNew.plcEventState = this.plcEventState;
            spNew.PrLength = this.PrLength;
            spNew.PrType = this.PrType;
            spNew.ResultValueLength = this.ResultValueLength;
            spNew.ResultValueType = this.ResultValueType;
            spNew.Sender = this.Sender;
            return spNew;



        }


    }

    [XmlRoot("PLCCommand")]
    public class PLCCommand : AGeneralCf
    {
        public string DeviceName = "";

        /// <summary>
        /// 调用方法名
        /// </summary>
        public string MethodName = "";

        public object Instance = null;
        public List<parameter> Parameters = new List<parameter>();
        public void Clone()
        {

        }
    }

    /// <summary>
    /// PLC当前站的流程
    /// </summary>
    [XmlRoot("StationFLowConfig")]
    public class StationFLowConfig
    {
        int CurrentFlow = 0;
        public void SetCurrentFlow(int Value)
        {
            CurrentFlow = Value;
        }

        public int GetCurrentFlow()
        {
            return CurrentFlow;
        }


        public string GetFlowMessageDebug()
        {
            if (CurrentFlow < Processlst.Count)
            {
                //if (Processlst[CurrentFlow].IsEnbale)
                //{
                    return Processlst[CurrentFlow].Name + ":" + Processlst[CurrentFlow].ShowMessage;
                //}
                //else
                //{
                //    return "NA";
                //}
            }
            else
            {
                return "NA";
            }
        }

        public string GetFlowMessage()
        {
            if (CurrentFlow < Processlst.Count)
            {
                if (Processlst[CurrentFlow].IsEnbale)
                {
                    return Processlst[CurrentFlow].ShowMessage;
                }
                else
                {
                    return "NA";
                }
                //else
                //{
                //    return "NA";
                //}
            }
            else
            {
                return "NA";
            }
        }


        public int FlowAddress = 0;
        public List<SubProcess> Processlst = new List<SubProcess>();
        public StationFLowConfig Clone()
        {
            StationFLowConfig stf = new StationFLowConfig();
            stf.FlowAddress = this.FlowAddress;
           foreach(SubProcess sp in   Processlst)
           {
               stf.Processlst.Add(sp.Clone());
           }
           return stf;
        }
    }

    /// <summary>
    /// 子流程
    /// </summary>
    [XmlRoot("SubProcess")]
    public class SubProcess : AGeneralCf
    {
        //public bool IsEnable = false;
        
        public string ShowMessage = "未知流程";
        public SubProcess Clone()
        {
            SubProcess sbp = new SubProcess();
            sbp.IsEnbale = this.IsEnbale;
            sbp.id = this.id;
            sbp.Name = this.Name;
          
            sbp.ShowMessage = new string(ShowMessage.ToCharArray());
            return sbp;
        }
    }

    /// <summary>
    /// 报警信息配置
    /// </summary>
    [XmlRoot("WarningMessageConfig")]
    public class WarningMessageConfig
    {
        public int WarningAddress = 0;
        string EMGWarningBitString = "";
        string WarningBitString = "";

        
       

        public void SetEMGWarningBitString(string Value)
        {
            EMGWarningBitString = Value;
        }

        public string GetEMGWarningBitString()
        {
            return EMGWarningBitString;
        }

        public void SetWarningBitString(string Value)
        {
            WarningBitString = Value;
        }

        public string GetWarningBitString()
        {
            return WarningBitString;
        }

        public List<WarningMessage> EMGMessagelst = new List<WarningMessage>();
        public List<WarningMessage> WMessagelst = new List<WarningMessage>();
        public WarningMessageConfig Clone()
        {
            WarningMessageConfig pnew = new WarningMessageConfig();
            pnew.WarningAddress = this.WarningAddress;
            pnew.WMessagelst = new List<WarningMessage>();
            foreach (WarningMessage wm in WMessagelst)
            {
                WMessagelst.Add(wm.Clone());
            }
           

            return pnew;
        }
    }

    /// <summary>
    /// 报警信息
    /// </summary>
    [XmlRoot("WarningMessage")]
    public class WarningMessage : AGeneralCf
    {
       
       // public bool Enable = false;
        public string ShowMessage = "未知报警";
        public bool Trigered = false;
        public WarningMessage Clone()
        {
            WarningMessage wm = new WarningMessage();
            wm.IsEnbale = this.IsEnbale;
            wm.id = this.id;
            wm.Name = this.Name;
            wm.ShowMessage = new string(ShowMessage.ToCharArray());
            return wm;
        }
    }

    [XmlRoot("PLCStation")]
    public class PLCStation : AGeneralCf
    {


        ushort CurrentEventIndex = 0;
        ushort CurrentState = 0;


        public void SetCurrentEventState(ushort eventIndex,ushort state)
        {
            CurrentEventIndex = eventIndex;
            CurrentState = state;
        }

        public ushort getCurrentEventIndex()
        {
            return CurrentEventIndex;
        }

        public ushort getCurrentEventState()
        {
            return CurrentState;
        }



        /// <summary>
        /// plc参数区起始地址
        /// </summary>
        public int PlcParamAndRes_StartAddress = 100;//参数和结果起始地址，默认长度为200

        /// <summary>
        /// plc参数区长度
        /// </summary>
        public int PlcParamAndRes_ByteCount = 20;

        /// <summary>
        /// pc参数区起始地址
        /// </summary>
        public int PcParamAndRes_StartAddress = 100;//参数和结果起始地址，默认长度为200

        /// <summary>
        /// pc参数区长度
        /// </summary>
        public int PcParamAndRes_ByteCount = 20;

        /// <summary>
        /// 报警配置表
        /// </summary>
        public WarningMessageConfig WarningConfig = new WarningMessageConfig();

        /// <summary>
        /// 流程配置表
        /// </summary>
        public StationFLowConfig FlowConfig = new StationFLowConfig();

        /// <summary>
        /// PLC2PC事件配置
        /// </summary>
        public PLCEventConfig PLC2PCEventConfig = new PLCEventConfig();


        /// <summary>
        /// PC2PLC事件配置
        /// </summary>
        public PLCEventConfig PC2PLCEventConfig = new PLCEventConfig();

        public PLCStation() 
        {
            CurrentWarning = new DataTable();
            CurrentWarning.PrimaryKey= new DataColumn[]{    CurrentWarning.Columns.Add("WarningId")};
   
            CurrentWarning.Columns.Add("Message");

            CurrentEMGWarning = new DataTable();
            CurrentEMGWarning.PrimaryKey = new DataColumn[] { CurrentEMGWarning.Columns.Add("EMGWarningID") };
           
            CurrentEMGWarning.Columns.Add("Message");



            FlowShowMessage = "";
        }

        DataTable CurrentWarning = null;
        DataTable CurrentEMGWarning = null;

        string FlowShowMessage = "";


        public void InitDataTable()
        {
            CurrentWarning.Rows.Clear();
            for (int i = 0; i < WarningConfig.WMessagelst.Count; i++)
            {
                if(WarningConfig.WMessagelst[i].IsEnbale)
                {
                    CurrentWarning.Rows.Add(WarningConfig.WMessagelst[i].id,false ,WarningConfig.WMessagelst[i].ShowMessage);
                }
            }
            CurrentEMGWarning.Clear();
            for (int i = 0; i < WarningConfig.EMGMessagelst.Count; i++)
            {
                if (WarningConfig.EMGMessagelst[i].IsEnbale)
                {
                    CurrentWarning.Rows.Add(WarningConfig.EMGMessagelst[i].id, false, WarningConfig.EMGMessagelst[i].ShowMessage);
                }
            }
        }

        /// <summary>
        /// 获取当前报警的列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentWarning()
        {
            if (CurrentWarning != null)
            {
                for (int j = 0; j < WarningConfig.WMessagelst.Count; j++)
                {
                    if (WarningConfig.WMessagelst[j].Trigered)
                    {
                        if (!CurrentWarning.Rows.Contains(WarningConfig.WMessagelst[j].id))
                        {
                            CurrentWarning.Rows.Add(new object[] { WarningConfig.WMessagelst[j].id, WarningConfig.WMessagelst[j].ShowMessage });
                        }
                    }
                    else
                    {
                        if (CurrentWarning.Rows.Contains(WarningConfig.WMessagelst[j].id))
                        {
                            DataRow dr = CurrentWarning.Rows.Find(WarningConfig.WMessagelst[j].id);
                            CurrentWarning.Rows.Remove(dr);
                        }
                    }
                }
            }
          
            return CurrentWarning;
        }

        /// <summary>
        /// 获取当前急停报警的列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentEMGWarning()
        {
            if (CurrentEMGWarning != null)
            {
                for (int j = 0; j < WarningConfig.EMGMessagelst.Count; j++)
                {
                    if (WarningConfig.EMGMessagelst[j].Trigered)
                    {
                        if (!CurrentEMGWarning.Rows.Contains(WarningConfig.EMGMessagelst[j].id))
                        {
                            CurrentEMGWarning.Rows.Add(new object[] { WarningConfig.EMGMessagelst[j].id, WarningConfig.EMGMessagelst[j].ShowMessage });
                        }
                    }
                    else
                    {
                        if (CurrentEMGWarning.Rows.Contains(WarningConfig.EMGMessagelst[j].id))
                        {
                            DataRow dr = CurrentEMGWarning.Rows.Find(WarningConfig.EMGMessagelst[j].id);
                            CurrentEMGWarning.Rows.Remove(dr);
                        }
                    }
                }
            }
            return CurrentEMGWarning;
        }


        /// <summary>
        /// 获取当前流程的数据
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public int GetCurrentFlowMessage(out string Message)
        {
            int FlowNum = this.FlowConfig.GetCurrentFlow();
            Message = FlowConfig.GetFlowMessage();
            return FlowNum;
        }




        public PLCStation Clone()
        {
            PLCStation plcstNew = new PLCStation();
            plcstNew.IsEnbale = this.IsEnbale;
            plcstNew.id = this.id;
            plcstNew.Name = this.Name;

            plcstNew.PcParamAndRes_ByteCount = this.PcParamAndRes_ByteCount;
            plcstNew.PcParamAndRes_StartAddress = this.PcParamAndRes_StartAddress;
            plcstNew.PlcParamAndRes_ByteCount = this.PlcParamAndRes_ByteCount;
            plcstNew.PlcParamAndRes_StartAddress = this.PlcParamAndRes_StartAddress;
            plcstNew.WarningConfig = this.WarningConfig.Clone();
            plcstNew.FlowConfig = this.FlowConfig.Clone();
            plcstNew.PLC2PCEventConfig = this.PLC2PCEventConfig.Clone();
            plcstNew.PC2PLCEventConfig = this.PC2PLCEventConfig.Clone();
            return plcstNew;
        }
    }
    [XmlRoot("Parameter")]
    public class parameter : AGeneralCf
    {
        public string Type = "";
        public string Value = "";
        public void Clone()
        {

        }
    }
    [XmlRoot("PLCHandle")]
    public class PLCHandle : AGeneralCf
    {
        public List<PLCStation> PLCStationlst = new List<PLCStation>();
        public string ip = "127.0.0.1";//192.168.0.10
        public int port = 502;//port 502
       
        public int ThreadNum = 4;//线程数
        public int HeatBeatAdr = 0;//心跳地址


        public int DBIndex = 1000;//"DB1000"

        public short Rack =0;

        public short Slot = 1;

        
        public PLCHandle Clone()
        {
            PLCHandle p = new PLCHandle();
            p.Name = this.Name;
            p.id = this.id;
            p.IsEnbale = this.IsEnbale;

            p.ip = this.ip;
            p.port = this.port;
            //p.IsEnable = this.IsEnable;
            p.ThreadNum = this.ThreadNum;
            p.HeatBeatAdr = this.HeatBeatAdr;
            p.PLCStationlst = new List<PLCStation>();
            foreach (PLCStation pclst in PLCStationlst)
            {
                p.PLCStationlst.Add(pclst.Clone());
            }
            return p;
        }
        //    foreach (PLCStation pclst in PLCStationlst)
        //    {
        //        PLCStation plcstNew = new PLCStation();
        //        plcstNew.IsEnbale = pclst.IsEnbale;
        //        plcstNew.id = pclst.id;
        //        plcstNew.Name = pclst.Name;
        //        plcstNew.PcParamAndRes_ByteCount = pclst.PcParamAndRes_ByteCount;
        //        plcstNew.PcParamAndRes_StartAddress = pclst.PcParamAndRes_StartAddress;
        //        plcstNew.PlcParamAndRes_ByteCount = pclst.PlcParamAndRes_ByteCount;
        //        plcstNew.PlcParamAndRes_StartAddress = pclst.PlcParamAndRes_StartAddress;


        //        plcstNew.FlowConfig = new StationFLowConfig();
        //        plcstNew.FlowConfig.FlowAddress = pclst.FlowConfig.FlowAddress;
        //        plcstNew.FlowConfig.Processlst = new List<SubProcess>();
        //        foreach (SubProcess sp in pclst.FlowConfig.Processlst)
        //        {
        //            SubProcess spNew = new SubProcess();
        //            spNew.id = sp.id;
        //            spNew.IsEnbale = sp.IsEnbale;
        //            spNew.ShowMessage = sp.ShowMessage;
        //            spNew.Name = sp.Name;
        //            plcstNew.FlowConfig.Processlst.Add(spNew);
        //        }

        //        plcstNew.WarningConfig = new WarningMessageConfig();
        //        plcstNew.WarningConfig.WarningAddress = pclst.WarningConfig.WarningAddress;
        //        plcstNew.WarningConfig.WMessagelst = new List<WarningMessage>();
        //        foreach (WarningMessage sp in pclst.WarningConfig.WMessagelst)
        //        {
        //            WarningMessage spNew = new WarningMessage();
        //            spNew.id = sp.id;
        //            spNew.IsEnbale = sp.IsEnbale;
        //            spNew.ShowMessage = sp.ShowMessage;
        //            spNew.Name = sp.Name;
        //            plcstNew.WarningConfig.WMessagelst.Add(spNew);
        //        }

        //        plcstNew.PC2PLCEventConfig = new PLCEventConfig();

        //        plcstNew.PC2PLCEventConfig.PLCEvent_StartAddress = pclst.PC2PLCEventConfig.PLCEvent_StartAddress;
        //        plcstNew.PC2PLCEventConfig.Eventlst = new List<PLCEvent>();
        //        foreach (PLCEvent sp in pclst.PC2PLCEventConfig.Eventlst)
        //        {
        //            PLCEvent spNew = new PLCEvent();
        //            spNew.id = sp.id;
        //            spNew.IsEnbale = sp.IsEnbale;
                 
        //            spNew.Name = sp.Name;
        //            spNew.IsPCInHanding = sp.IsPCInHanding;
        //            spNew.Mycommand = new PLCCommand();
        //            spNew.Mycommand.DeviceName = sp.Mycommand.DeviceName;
        //            spNew.Mycommand.id = sp.Mycommand.id;
        //            spNew.Mycommand.DeviceName = sp.Mycommand.DeviceName;
        //            spNew.Mycommand.Instance = sp.Mycommand.Instance;
        //            spNew.Mycommand.IsEnbale = sp.IsEnbale;
        //            spNew.Mycommand.MethodName = sp.Mycommand.MethodName;
        //            spNew.Mycommand.Name = sp.Mycommand.Name;
        //            spNew.Mycommand.Parameters = sp.Mycommand.Parameters;

        //            spNew.Name = sp.Name;
        //            spNew.plcEventState = sp.plcEventState;
        //            spNew.PrLength = sp.PrLength;
        //            spNew.PrType = sp.PrType;
        //            spNew.ResultValueLength = sp.ResultValueLength;
        //            spNew.ResultValueType = sp.ResultValueType;
        //            spNew.Sender = sp.Sender;
        //            plcstNew.PC2PLCEventConfig.Eventlst.Add(spNew);
        //        }

        //        plcstNew.PLC2PCEventConfig = new PLCEventConfig();
        //        plcstNew.PLC2PCEventConfig.PLCEvent_StartAddress = pclst.PLC2PCEventConfig.PLCEvent_StartAddress;
        //        plcstNew.PLC2PCEventConfig.Eventlst = new List<PLCEvent>();
        //        foreach (PLCEvent sp in pclst.PLC2PCEventConfig.Eventlst)
        //        {
        //            PLCEvent spNew = new PLCEvent();
        //            spNew.id = sp.id;
        //            spNew.IsEnbale = sp.IsEnbale;

        //            spNew.Name = sp.Name;
        //            spNew.IsPCInHanding = sp.IsPCInHanding;
        //            spNew.Mycommand = new PLCCommand();
        //            spNew.Mycommand.DeviceName = sp.Mycommand.DeviceName;
        //            spNew.Mycommand.id = sp.Mycommand.id;
        //            spNew.Mycommand.DeviceName = sp.Mycommand.DeviceName;
        //            spNew.Mycommand.Instance = sp.Mycommand.Instance;
        //            spNew.Mycommand.IsEnbale = sp.IsEnbale;
        //            spNew.Mycommand.MethodName = sp.Mycommand.MethodName;
        //            spNew.Mycommand.Name = sp.Mycommand.Name;
        //            spNew.Mycommand.Parameters = sp.Mycommand.Parameters;

        //            spNew.Name = sp.Name;
        //            spNew.plcEventState = sp.plcEventState;
        //            spNew.PrLength = sp.PrLength;
        //            spNew.PrType = sp.PrType;
        //            spNew.ResultValueLength = sp.ResultValueLength;
        //            spNew.ResultValueType = sp.ResultValueType;
        //            spNew.Sender = sp.Sender;
        //            plcstNew.PLC2PCEventConfig.Eventlst.Add(spNew);
        //        }
        //        p.PLCStationlst.Add(plcstNew);
        //    }
        //    return p;
        //}
    }

    /// <summary>
    /// 总配置
    /// </summary>
    [XmlRoot("GeneralConfig")]
    public class GeneralConfig
    {

        public string Name = "总配置";

        /// <summary>
        /// 是否与PLC相连
        /// </summary>
        public bool IsConnectPLC = false;


        /// <summary>
        /// 是否与数据服务相连
        /// </summary>
        public bool IsConnectMESService = false;


        /// <summary>
        /// 是否和数据库连接
        /// </summary>
        public bool IsConnectDB = false;


        /// <summary>
        /// 是否连接站头
        /// </summary>
        public bool IsConnectHeadStation = false;




        public List<PLCHandle> plclst = new List<PLCHandle>();


        #region 可以写一些通用配置，例如连接字符串等



        #endregion


        /// <summary>
        /// 序列化Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISerializeHelper SerializeHelper<T>() where T : ISerializeHelper
        {
            return Activator.CreateInstance<T>();
        }



        public GeneralConfig Load(string Path)
        {
            FileStream mStream = null;
            GeneralConfig p = null;

            try
            {
                //ArrPLCEvent.Clear();
                //FileStream mStream = new FileStream(@"D:\Points.xml", FileMode.Open, FileAccess.Read);
                mStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                p = SerializeHelper<XmlSerializeHelper>().DeSerialize<GeneralConfig>(mStream);
                //mStream.Close();
            }
            catch
            {
                //mStream.Close();
                p = null;
            }
            finally
            {
                if (mStream != null)
                {
                    mStream.Close();
                }
            }
            return p;
        }

        public bool Save(string path)
        {
            bool res = false;
            MemoryStream mStream = null;
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                mStream = new MemoryStream();
                SerializeHelper<XmlSerializeHelper>().Serialize(mStream, this);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                w = new BinaryWriter(fs);
                w.Write(mStream.ToArray());
                res = true;
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (mStream != null)
                {
                    mStream.Close();
                }
            }
            return res;
        }
    }
}

