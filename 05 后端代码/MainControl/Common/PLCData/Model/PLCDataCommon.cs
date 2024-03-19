using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PLCData.Model
{
    public class PLCDataCommon
    {
        //托盘号	Byte	100
        //工作模式	Byte	101
        //工位使能信息	Byte	102
        //产品类型	Byte	103
        //产品ID	DINT	104-107
        //总工位OK NG信号	Byte	108
        //备用	Byte	109-120
        //产品SN	Byte	121-180
        //备用	Byte	181-199

        public byte TrayId = 0;
        public byte WorkType = 0;
        public byte WorkEnable = 0;
        public byte PartType_PLC = 0;
        public UInt64 UUTid = 0;
        public byte CurrentResult = 0;
        public string ProductSN = "";


        public static readonly int TrayIdIndex = 0;
        public static readonly int WorkTypeIndex = 1;
        public static readonly int WorkEnableIndex = 2;
        public static readonly int PartType_PLC_Index = 3;
        public static readonly int UUTidIndex = 10;
        public static readonly int CurrentResultIndex =4;
        public static readonly int ProductSNIndex = 18;
        public static readonly int ProductSNLength = 80;

    }


    
}
