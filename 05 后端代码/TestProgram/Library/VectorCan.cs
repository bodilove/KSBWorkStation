using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vxlapi_NET20;
using System.Timers;
using System.Diagnostics;

namespace Library
{
   public class VectorCan
    {
        private static String appName = "CanCard";
        private static XLClass.xl_driver_config driverConfig = new XLClass.xl_driver_config();

        // variables needed by XLDriver
        private static uint hwType = 59;
        private static uint hwIndex = 0;
        private static uint hwChannel = 1;
        private static uint busTypeCan = (uint)XLClass.XLbusTypes.XL_BUS_TYPE_CAN;
        private static uint flags = 0;
        private static int portHandleCan = -1;
        private static int eventHandle = -1;
        private static UInt64 accessMaskCan = 0;
        private static UInt64 permissionMask = 2;

        XLDriver CancardCan = new XLDriver();     //OK

        public VectorCan Initialize(string HwType, int HwIndex, int ChannelIndex, int Baudrate)
        {
            //XLClass.XLstatus
            if (HwType == "VN1611")              //OK
                hwType = 63;
            if (HwType == "VN1640")              //OK
                hwType = 59;
            if (HwType == "VN1630")              //OK
                hwType = 57;

            try
            {

                XLClass.XLstatus a = CancardCan.XL_OpenDriver();
                if (a != XLClass.XLstatus.XL_SUCCESS)
                {
                    throw new Exception("Can initial fail!");
                }
                a = CancardCan.XL_GetDriverConfig(ref driverConfig);
                accessMaskCan = CancardCan.XL_GetChannelMask((int)hwType, (int)hwIndex, ChannelIndex);

                permissionMask = accessMaskCan;
                a = CancardCan.XL_OpenPort(ref portHandleCan, appName, accessMaskCan, ref permissionMask, 1024, busTypeCan);
                if (a != XLClass.XLstatus.XL_SUCCESS)
                {
                    throw new Exception("Can initial fail!");
                }

                a = CancardCan.XL_CanSetChannelBitrate(portHandleCan, accessMaskCan, (uint)Baudrate);
                XLClass.xl_chip_params par = new XLClass.xl_chip_params();
                par.bitrate = 500000;

                par.tseg1 = 1;
                par.tseg2 = 0x23;

                a = CancardCan.XL_CanSetChannelParams(portHandleCan, accessMaskCan, par);

                a = CancardCan.XL_GetDriverConfig(ref driverConfig);
                a = CancardCan.XL_ActivateChannel(portHandleCan, accessMaskCan, busTypeCan, flags);
                if (a != XLClass.XLstatus.XL_SUCCESS)
                {
                    throw new Exception("Can initial fail!");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this;
        }

        public bool Quit()
        {
            CancardCan.XL_ClosePort(portHandleCan);
            CancardCan.XL_CloseDriver();
            return true;
        }

        public bool CloseChannel()
        {
            try
            {
                CancardCan.XL_DeactivateChannel(portHandleCan, 1);
                CancardCan.XL_ClosePort(portHandleCan);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        private void SetReceiveIDRange(uint FirstID, uint LastID)
        {
            CancardCan.XL_CanAddAcceptanceRange(portHandleCan, accessMaskCan, FirstID, LastID);
        }

        public void SetReceiveIDRange(uint FilterID)
        {
            //CancardCan.XL_CanAddAcceptanceRange(portHandleCan, accessMaskCan, FirstID,LastID);
            CancardCan.XL_CanSetChannelAcceptance(portHandleCan, accessMaskCan, FilterID, FilterID, 1);
        }

        public void ResetReceiveIDRange()
        {
            //CancardCan.XL_CanAddAcceptanceRange(portHandleCan, accessMaskCan, FirstID,LastID);

            CancardCan.XL_CanResetAcceptance(portHandleCan, accessMaskCan, 1);
        }

        public void Flush()
        {
            CancardCan.XL_CanFlushTransmitQueue(portHandleCan, accessMaskCan);
            CancardCan.XL_FlushReceiveQueue(portHandleCan);
        }

        public bool Send(UInt32 ID, ushort DLC, string Outdata)
        {
            string[] StringData = Outdata.Split(' ');
            byte[] Data = new byte[StringData.Length];

            for (int i = 0; i < StringData.Length; i++)
            {
                Data[i] = Convert.ToByte(StringData[i], 16);
            }
            Send(ID, DLC, Data);
            return true;
        }

        public bool Send(uint ID, ushort DLC, byte[] Outdata)//*************需要ID和数据
        {
            //02 10 60 FF FF FF FF FF
            try
            {
                this.Flush();
                XLClass.xl_event theEvent = new XLClass.xl_event();
                theEvent.tagData.can_Msg.id = ID;
                theEvent.tagData.can_Msg.dlc = DLC;
                theEvent.tagData.can_Msg.data = Outdata;
                theEvent.tag = (byte)XLClass.XLeventType.XL_TRANSMIT_MSG;
                CancardCan.XL_CanTransmit(portHandleCan, accessMaskCan, theEvent);

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private bool Read(uint IDFilter, ref byte[] Data, long Timeout)
        {
            try
            {
                string receiveString;
                Stopwatch watch = new Stopwatch();
                bool bTimeout = false;
                XLClass.xl_event receiveEvent = new XLClass.xl_event();
                XLClass.XLstatus status = new XLClass.XLstatus();

                watch.Start();
                do
                {

                    status = CancardCan.XL_Receive(portHandleCan, ref receiveEvent);
                    receiveString = CancardCan.XL_GetEventString(receiveEvent);
                    uint ID = receiveEvent.tagData.can_Msg.id;
                    Data = receiveEvent.tagData.can_Msg.data;
                    if (ID == IDFilter) break;
                    bTimeout = (watch.ElapsedMilliseconds > Timeout);
                } while (!bTimeout);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public string Read(uint IDFilter, long Timeout)
        {
            //读取ID、数据，超时之后返回超时，catch到错误返回error，正常返回数据
            string receiveString;
            Stopwatch watch = new Stopwatch();
            bool bTimeout = false;
            XLClass.xl_event receiveEvent = new XLClass.xl_event();
            XLClass.XLstatus status = new XLClass.XLstatus();
            watch.Start();
            do
            {

                status = CancardCan.XL_Receive(portHandleCan, ref receiveEvent);
                receiveString = CancardCan.XL_GetEventString(receiveEvent);
                uint ID = receiveEvent.tagData.can_Msg.id;
                if (ID != 0)
                {
                    Console.WriteLine(receiveString);
                }
                if (ID == IDFilter) break;
                bTimeout = (watch.ElapsedMilliseconds > Timeout);
            } while (!bTimeout);
            if (bTimeout) return "timeout";

            ushort DLC = receiveEvent.tagData.can_Msg.dlc;
            string tmp = null;
            for (int i = 0; i < DLC; i++)
            {

                tmp += receiveEvent.tagData.can_Msg.data[i].ToString("X2") + " ";
            }
            return tmp.TrimEnd();
        }

        private string Read_Again(uint IDFilter, long Timeout)
        {
            //读取ID、数据，超时之后返回超时，catch到错误返回error，正常返回数据
            string receiveString;
            Stopwatch watch = new Stopwatch();
            bool bTimeout = false;
            XLClass.xl_event receiveEvent = new XLClass.xl_event();
            XLClass.XLstatus status = new XLClass.XLstatus();
            ushort DLC = 0;
            string tmp = null;
            watch.Start();
            do
            {

                status = CancardCan.XL_Receive(portHandleCan, ref receiveEvent);
                receiveString = CancardCan.XL_GetEventString(receiveEvent);
                uint ID = receiveEvent.tagData.can_Msg.id;
                DLC = receiveEvent.tagData.can_Msg.dlc;

                if (ID != 0)
                {
                    Console.WriteLine(receiveString);
                }
                if (ID == IDFilter)
                {
                    if (receiveEvent.tagData.can_Msg.data[1] == 0x7F)
                    {
                        tmp = "";
                        for (int i = 0; i < DLC; i++)
                        {
                            tmp += receiveEvent.tagData.can_Msg.data[i].ToString("X2") + " ";
                        }
                        receiveString = tmp.TrimEnd();
                        receiveString = TrimString(receiveString);
                        if (receiveString.StartsWith("7F") && receiveString.EndsWith("78"))
                        {
                            System.Threading.Thread.Sleep(50);
                            continue;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
                bTimeout = (watch.ElapsedMilliseconds > Timeout);
            } while (!bTimeout);
            if (bTimeout) return "timeout";
            tmp = "";
            for (int i = 0; i < DLC; i++)
            {
                tmp += receiveEvent.tagData.can_Msg.data[i].ToString("X2") + " ";
            }
            return tmp.TrimEnd();
        }

        private string Read(uint FirstID, uint SecondID, long Timeout)
        {
            string receiveString;
            Stopwatch watch = new Stopwatch();
            bool bTimeout = false;
            XLClass.XLstatus status = new XLClass.XLstatus();
            watch.Start();
            do
            {
                XLClass.xl_event receiveEvent = new XLClass.xl_event();
                status = CancardCan.XL_Receive(portHandleCan, ref receiveEvent);
                receiveString = CancardCan.XL_GetEventString(receiveEvent);
                uint ID = receiveEvent.tagData.can_Msg.id;
                if (ID != 0)
                {
                    Console.WriteLine(receiveString);
                }
                if (ID <= SecondID && ID >= FirstID) break;
                bTimeout = (watch.ElapsedMilliseconds > Timeout);
            } while (!bTimeout);
            if (bTimeout) return "timeout";
            return receiveString;

        }

        private string SendRead(UInt32 SendID, string SendString, UInt32 ReadID, long ReadTimeout, long Timeout)
        {
            string[] splitsendstring = SendString.Split(' ');
            Stopwatch timer = new Stopwatch();
            string RTResult = null;
            timer.Start();
            int NumEx = 0;
            this.Flush();
            do
            {
                NumEx++;

                switch (splitsendstring.Length)
                {
                    case (8):
                        Send(SendID, 8, SendString);
                        break;
                    case (16):
                        Send(SendID, 8, SendString.Substring(0, 23));
                        Read(ReadID, ReadTimeout);
                        Send(SendID, 8, SendString.Substring(24, 23));
                        break;
                    case (24):
                        Send(SendID, 8, SendString.Substring(0, 23));
                        Read(ReadID, ReadTimeout);
                        Send(SendID, 8, SendString.Substring(24, 23));
                        Send(SendID, 8, SendString.Substring(48, 23));
                        break;
                    case (32):
                        Send(SendID, 8, SendString.Substring(0, 23));
                        Read(ReadID, ReadTimeout);
                        Send(SendID, 8, SendString.Substring(24, 23));
                        Send(SendID, 8, SendString.Substring(48, 23));
                        Send(SendID, 8, SendString.Substring(72, 23));
                        break;

                }
                RTResult = Read(ReadID, ReadTimeout);
                if (RTResult == "timeout")
                    continue;
                if (RTResult.StartsWith("10"))
                {
                    //this.Flush ();
                    Send(SendID, 8, "30 00 14 FF FF FF FF FF");
                    string secondstring = Read(ReadID, ReadTimeout);
                    int num = 0;
                    do
                    {
                        num++;
                        Send(SendID, 8, "30 00 14 FF FF FF FF FF");
                        secondstring = Read(ReadID, ReadTimeout);
                        if (num == 5)
                            break;

                    } while (secondstring == "timeout");
                    RTResult += " " + secondstring;
                }
                if (timer.ElapsedMilliseconds > Timeout)
                    break;
                Console.Write("NumEx={0}\n", NumEx);
            } while (NumEx != 5);
            return RTResult = TrimString(RTResult);
        }

        /// <summary>
        /// 发单帧收单帧
        /// </summary>
        /// <param name="SendID"></param>
        /// <param name="SendString"></param>
        /// <param name="ReadID"></param>
        /// <param name="ReadTimeout"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string SendSingleF_ReadSingleF(UInt32 SendID, string SendString, UInt32 ReadID, long ReadTimeout, long Timeout)
        {
            string[] splitsendstring = SendString.Split(' '); //用空格隔开转化为数组
            Stopwatch timer = new Stopwatch();
            string RTResult = null;
            byte[] Data = new byte[8];
            Data[0] = Convert.ToByte(splitsendstring.Length); //发送数据长度（can协议规定）
            for (int i = 1; i < splitsendstring.Length + 1; i++)
            {
                Data[i] = Convert.ToByte(splitsendstring[i - 1], 16);// 把16进制数转化为等效的8位无符号整数
            }
            for (int i = splitsendstring.Length + 1; i < 8; i++)
            {
                Data[i] = 255;                                    // 长度小于8补FF
            }
            Flush();
            timer.Start();
            do
            {
                Send(SendID, 8, Data);
                RTResult = Read_Again(ReadID, ReadTimeout);
            } while (timer.ElapsedMilliseconds < Timeout && RTResult == "timeout");

            return TrimString(RTResult);
        }

        /// <summary>
        /// 发多帧收单帧
        /// </summary>
        /// <param name="SendID"></param>
        /// <param name="SendString"></param>
        /// <param name="ReadID"></param>
        /// <param name="ReadTimeout"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string SendMultiF_ReadSingleF(UInt32 SendID, string SendString, UInt32 ReadID, long ReadTimeout, long Timeout)
        {
            string[] splitsendstring = SendString.Split(' ');
            Stopwatch timer = new Stopwatch();
            string RTResult = null;
            byte[] Data1 = new byte[8];
            byte[] Data2 = new byte[8];
            byte[] Data3 = new byte[8];
            byte[] Data4 = new byte[8];
            if (splitsendstring.Length > 7)
            {
                Data1[0] = 0x10;
                Data1[1] = Convert.ToByte(splitsendstring.Length);
                Data2[0] = 0x21;
                Data3[0] = 0x22;
                Data4[0] = 0x23;

                for (int i = 2; i < 8; i++)
                {
                    Data1[i] = Convert.ToByte(splitsendstring[i - 2], 16);
                }
                if (splitsendstring.Length < 14)
                {
                    for (int i = 1; i < splitsendstring.Length - 5; i++)
                    {
                        Data2[i] = Convert.ToByte(splitsendstring[i + 5], 16);
                    }
                    for (int i = splitsendstring.Length - 5; i < 8; i++)
                    {
                        Data2[i] = 255;
                    }
                    this.Flush();
                    timer.Start();
                    do
                    {
                        Send(SendID, 8, Data1);
                        Read(ReadID, ReadTimeout);
                        Send(SendID, 8, Data2);
                        RTResult = Read(ReadID, ReadTimeout);
                        Console.WriteLine(RTResult);
                    } while (timer.ElapsedMilliseconds < Timeout && RTResult == "timeout");
                }
                if (splitsendstring.Length > 13)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        Data2[i] = Convert.ToByte(splitsendstring[i + 5], 16);
                    }
                    if (splitsendstring.Length < 21)
                    {
                        for (int i = 1; i < splitsendstring.Length - 12; i++)
                        {
                            Data3[i] = Convert.ToByte(splitsendstring[i + 12], 16);
                        }
                        for (int i = splitsendstring.Length - 12; i < 8; i++)
                        {
                            Data3[i] = 255;
                        }
                        this.Flush();
                        timer.Start();
                        do
                        {
                            Send(SendID, 8, Data1);
                            Read(ReadID, ReadTimeout);
                            Send(SendID, 8, Data2);
                            Send(SendID, 8, Data3);
                            RTResult = Read(ReadID, ReadTimeout);

                        } while (timer.ElapsedMilliseconds < Timeout && RTResult == "timeout");
                    }
                    if (splitsendstring.Length > 20)
                    {
                        if (splitsendstring.Length < 27)
                        {
                            for (int i = 1; i < 8; i++)
                            {
                                Data3[i] = Convert.ToByte(splitsendstring[i + 12], 16);
                            }
                            for (int i = 1; i < splitsendstring.Length - 19; i++)
                            {
                                Data4[i] = Convert.ToByte(splitsendstring[i + 19], 16);
                            }
                            for (int i = splitsendstring.Length - 19; i < 8; i++)
                            {
                                Data4[i] = 255;
                            }

                            this.Flush();
                            timer.Start();
                            do
                            {
                                Send(SendID, 8, Data1);
                                Read(ReadID, ReadTimeout);
                                Send(SendID, 8, Data2);
                                Send(SendID, 8, Data3);
                                Send(SendID, 8, Data4);
                                RTResult = Read(ReadID, ReadTimeout);

                            } while (timer.ElapsedMilliseconds < Timeout && RTResult == "timeout");
                        }
                    }
                }
            }
            return RTResult = TrimString(RTResult);
        }

        /// <summary>
        /// 发单帧收多帧
        /// </summary>
        /// <param name="SendID"></param>
        /// <param name="SendString"></param>
        /// <param name="ReadID"></param>
        /// <param name="ReadTimeout"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public string SendSingleF_ReadMultiF(UInt32 SendID, string SendString, UInt32 ReadID, long ReadTimeout, long Timeout)
        {
            string[] splitsendstring = SendString.Split(' ');
            Stopwatch timer = new Stopwatch();
            string RTResult = null;
            byte[] Data = new byte[8];
            Data[0] = Convert.ToByte(splitsendstring.Length);
            for (int i = 1; i < splitsendstring.Length + 1; i++)
            {
                Data[i] = Convert.ToByte(splitsendstring[i - 1], 16);
            }
            for (int i = splitsendstring.Length + 1; i < 8; i++)
            {
                Data[i] = 255;
            }
            Flush();
            timer.Start();
            do
            {
                Send(SendID, 8, Data);
                RTResult = Read(ReadID, ReadTimeout);
                if (SendID == 0x765)
                {
                    Send(SendID, 8, "30 00 00 00 00 00 00 00");
                }
                if (SendID == 0x740)
                {
                    Send(SendID, 8, "30 00 0A 00 00 00 00 00");
                }
                string secondstring = Read(ReadID, ReadTimeout);
                RTResult += " " + secondstring;
            } while (timer.ElapsedMilliseconds < Timeout && RTResult == "timeout");
            return RTResult = TrimString(RTResult);
        }

        public string TrimString(string Result)
        {
            if (Result == "timeout") return "timeout";
            if (Result == null) return "";
            try
            {

                string[] splitsendstring = Result.Split(' ');
                int[] ConverttoInt = new int[splitsendstring.Length];
                for (int j = 0; j < splitsendstring.Length; j++)
                {
                    ConverttoInt[j] = Convert.ToInt32(splitsendstring[j], 16);
                }


                if (ConverttoInt[0] == 0x10)
                {
                    int length = ConverttoInt[1];
                    int count = 0;
                    int index = 1;
                    Result = "";
                    do
                    {
                        index++;
                        if (index % 8 != 0)
                        {
                            Result += ConverttoInt[index].ToString("X2") + " ";
                            count++;
                        }


                    } while (count < length);
                    Result = Result.TrimEnd(' ');
                }
                else
                {
                    string strB = null;
                    strB = Result.Substring(3, ConverttoInt[0] * 3 - 1);
                    Result = strB;
                }
            }
            catch (Exception ex)
            {
                return "timeout";

                //throw ex;
            }


            return Result;
        }

    }
}
