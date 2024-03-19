using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.PLCData.Model;
namespace Common.PLCData.BLL
{
    public class PLCDataCommonBLL
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
        //public byte TrayId = 0;
        //public byte WorkType = 0;
        //public byte WorkEnable = 0;
        //public byte PartNum = 0;
        //public UInt64 UUTid = 0;
        //public byte CurrentResult = 0;
        //public byte[] ProductSN = new byte[60];



        public PLCDataCommon ByteToPLCDataCommon(byte[] bytesFromPLC)
        {
            try
            {
                if (bytesFromPLC.Length >= 100)
                {
                    PLCDataCommon p = new PLCDataCommon();
                    p.TrayId = bytesFromPLC[PLCDataCommon.TrayIdIndex];
                    p.WorkType = bytesFromPLC[PLCDataCommon.WorkTypeIndex];
                    p.WorkEnable = bytesFromPLC[PLCDataCommon.WorkEnableIndex];
                    p.PartType_PLC = bytesFromPLC[PLCDataCommon.PartType_PLC_Index];
                    p.UUTid = BitConverter.ToUInt64(bytesFromPLC, PLCDataCommon.UUTidIndex);
                    p.CurrentResult = bytesFromPLC[PLCDataCommon.CurrentResultIndex];
                    p.ProductSN = Encoding.ASCII.GetString(bytesFromPLC, PLCDataCommon.ProductSNIndex, PLCDataCommon.ProductSNLength);
                    p.ProductSN = p.ProductSN.Trim();
                    return p;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public byte[] PLCDataCommonToBytes(PLCDataCommon PLCdata)
        {
            try
            {
                byte[] bytesFromPLC = new byte[100];
                bytesFromPLC[PLCDataCommon.TrayIdIndex] = PLCdata.TrayId;
                bytesFromPLC[PLCDataCommon.WorkTypeIndex] = PLCdata.WorkType;
                bytesFromPLC[PLCDataCommon.WorkEnableIndex] = PLCdata.WorkEnable;
                bytesFromPLC[PLCDataCommon.PartType_PLC_Index] = PLCdata.PartType_PLC;
                Array.Copy(BitConverter.GetBytes(PLCdata.UUTid), 0, bytesFromPLC, PLCDataCommon.UUTidIndex, 8);
                bytesFromPLC[PLCDataCommon.CurrentResultIndex] = PLCdata.CurrentResult;
                byte[] SNbyte = new byte[PLCDataCommon.ProductSNLength];
                byte[] SNtoByte=Encoding.ASCII.GetBytes(PLCdata.ProductSN);
                Array.Copy(SNtoByte, 0, SNbyte, 0, SNtoByte.Length);
                Array.Copy(SNbyte, 0, bytesFromPLC, PLCDataCommon.ProductSNIndex, PLCDataCommon.ProductSNLength);
                return bytesFromPLC;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
