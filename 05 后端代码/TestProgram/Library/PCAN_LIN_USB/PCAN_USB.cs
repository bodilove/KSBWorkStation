using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Inclusion of PEAK PCAN-Basic namespace
/// </summary>
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;
using TPCANBitrateFD = System.String;
using TPCANTimestampFD = System.UInt64;


namespace Library
{
    public class PCAN_USB
    {
        #region Structures
        /// <summary>
        /// Message Status structure used to show CAN Messages
        /// in a ListView
        /// </summary>
        private class MessageStatus
        {
            private TPCANMsgFD m_Msg;
            private TPCANTimestampFD m_TimeStamp;
            private TPCANTimestampFD m_oldTimeStamp;
            private int m_iIndex;
            private int m_Count;
            private bool m_bShowPeriod;
            private bool m_bWasChanged;

            public MessageStatus(TPCANMsgFD canMsg, TPCANTimestampFD canTimestamp, int listIndex)
            {
                m_Msg = canMsg;
                m_TimeStamp = canTimestamp;
                m_oldTimeStamp = canTimestamp;
                m_iIndex = listIndex;
                m_Count = 1;
                m_bShowPeriod = true;
                m_bWasChanged = false;
            }

            public void Update(TPCANMsgFD canMsg, TPCANTimestampFD canTimestamp)
            {
                m_Msg = canMsg;
                m_oldTimeStamp = m_TimeStamp;
                m_TimeStamp = canTimestamp;
                m_bWasChanged = true;
                m_Count += 1;
            }

            public TPCANMsgFD CANMsg
            {
                get { return m_Msg; }
            }

            public TPCANTimestampFD Timestamp
            {
                get { return m_TimeStamp; }
            }

            public int Position
            {
                get { return m_iIndex; }
            }

            public bool ShowingPeriod
            {
                get { return m_bShowPeriod; }
                set
                {
                    if (m_bShowPeriod ^ value)
                    {
                        m_bShowPeriod = value;
                        m_bWasChanged = true;
                    }
                }
            }

            public bool MarkedAsUpdated
            {
                get { return m_bWasChanged; }
                set { m_bWasChanged = value; }
            }

            public string TimeString
            {
                get { return GetTimeString(); }
            }

            private string GetTimeString()
            {
                double fTime;

                fTime = (m_TimeStamp / 1000.0);
                if (m_bShowPeriod)
                    fTime -= (m_oldTimeStamp / 1000.0);
                return fTime.ToString("F1");
            }
        }
        #endregion

        #region Delegates
        /// <summary>
        /// Read-Delegate Handler
        /// </summary>
        private delegate void ReadDelegateHandler();
        #endregion

        #region Members
        /// <summary>
        /// Saves the desired connection mode
        /// </summary>
        private bool m_IsFD = false;
        /// <summary>
        /// Saves the handle of a PCAN hardware
        /// </summary>
        private TPCANHandle m_PcanHandle;
        /// <summary>
        /// Saves the baudrate register for a conenction
        /// </summary>
        private TPCANBaudrate m_Baudrate;
        /// <summary>
        /// Saves the type of a non-plug-and-play hardware
        /// </summary>
        private TPCANType m_HwType;
        /// <summary>
        /// Stores the status of received messages for its display
        /// </summary>
        private System.Collections.ArrayList m_LastMsgsList;
        /// <summary>
        /// Read Delegate for calling the function "ReadMessages"
        /// </summary>
        private ReadDelegateHandler m_ReadDelegate;
        /// <summary>
        /// Receive-Event
        /// </summary>
        private System.Threading.AutoResetEvent m_ReceiveEvent;
        /// <summary>
        /// Thread for message reading
        /// </summary>
        Thread read_th = null;
        bool read_th_status = false;
        private uint ReadID;
        List<string> CanMsgList = new List<string>();
        object CanMsgListlock = new object();
        int readInterTimesMs = 1;

        /// <summary>
        /// Thread for message sending
        /// </summary>
        Thread send_th = null;
        bool send_th_status = false;
        string send_th_Msg = "";
        uint SendID;
        object CanSendlock = new object();
        int sendInterTimesMs = 500;

        /// <summary>
        /// Handles of the current available PCAN-Hardware
        /// </summary>
        private TPCANHandle[] m_HandlesArray;
        #endregion

        #region Methods
        /// <summary>
        /// Help Function used to get an error as text
        /// </summary>
        /// <param name="error">Error code to be translated</param>
        /// <returns>A text with the translated error</returns>
        private string GetFormatedError(TPCANStatus error)
        {
            StringBuilder strTemp;

            // Creates a buffer big enough for a error-text
            //
            strTemp = new StringBuilder(256);
            // Gets the text using the GetErrorText API function
            // If the function success, the translated error is returned. If it fails,
            // a text describing the current error is returned.
            //
            if (PCANBasic.GetErrorText(error, 0, strTemp) != TPCANStatus.PCAN_ERROR_OK)
                return string.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", error);
            else
                return strTemp.ToString();
        }

        /// <summary>
        /// Convert a CAN DLC value into the actual data length of the CAN/CAN-FD frame.
        /// </summary>
        /// <param name="dlc">A value between 0 and 15 (CAN and FD DLC range)</param>
        /// <param name="isSTD">A value indicating if the msg is a standard CAN (FD Flag not checked)</param>
        /// <returns>The length represented by the DLC</returns>
        private static int GetLengthFromDLC(int dlc, bool isSTD)
        {
            if (dlc <= 8)
                return dlc;

            if (isSTD)
                return 8;

            switch (dlc)
            {
                case 9: return 12;
                case 10: return 16;
                case 11: return 20;
                case 12: return 24;
                case 13: return 32;
                case 14: return 48;
                case 15: return 64;
                default: return dlc;
            }
        }


        private TPCANStatus WriteFrame(uint sendID, byte DLC, bool ifExtended, bool frameIsRTR, byte[] sendMSG)
        {
            TPCANMsg CANMsg;

            // We create a TPCANMsg message structure 
            //
            CANMsg = new TPCANMsg();
            CANMsg.DATA = new byte[8];

            // We configurate the Message.  The ID,
            // Length of the Data, Message Type
            // and the data
            //
            CANMsg.ID = sendID;
            CANMsg.LEN = DLC;
            CANMsg.MSGTYPE = ifExtended ? TPCANMessageType.PCAN_MESSAGE_EXTENDED : TPCANMessageType.PCAN_MESSAGE_STANDARD;
            // If a remote frame will be sent, the data bytes are not important.
            //
            if (frameIsRTR)
                CANMsg.MSGTYPE |= TPCANMessageType.PCAN_MESSAGE_RTR;
            else
            {
                // We get so much data as the Len of the message
                //
                for (int i = 0; i < GetLengthFromDLC(CANMsg.LEN, true); i++)
                {
                    CANMsg.DATA[i] = sendMSG[i];
                }
            }

            // The message is sent to the configured hardware
            //
            return PCANBasic.Write(m_PcanHandle, ref CANMsg);
        }

        private TPCANStatus WriteFrameFD(uint sendID, byte DLC, bool ifExtended, bool FD_BRS_ON, bool frameIsRTR, byte[] sendMSG)
        {
            TPCANMsgFD CANMsg;
            int iLength;

            // We create a TPCANMsgFD message structure 
            //
            CANMsg = new TPCANMsgFD();
            CANMsg.DATA = new byte[64];

            // We configurate the Message.  The ID,
            // Length of the Data, Message Type 
            // and the data
            //
            CANMsg.ID = sendID;
            CANMsg.DLC = DLC;
            CANMsg.MSGTYPE = (ifExtended) ? TPCANMessageType.PCAN_MESSAGE_EXTENDED : TPCANMessageType.PCAN_MESSAGE_STANDARD;
            CANMsg.MSGTYPE |= (m_IsFD) ? TPCANMessageType.PCAN_MESSAGE_FD : TPCANMessageType.PCAN_MESSAGE_STANDARD;
            CANMsg.MSGTYPE |= (FD_BRS_ON) ? TPCANMessageType.PCAN_MESSAGE_BRS : TPCANMessageType.PCAN_MESSAGE_STANDARD;

            // If a remote frame will be sent, the data bytes are not important.
            //
            if (frameIsRTR)
                CANMsg.MSGTYPE |= TPCANMessageType.PCAN_MESSAGE_RTR;
            else
            {
                // We get so much data as the Len of the message
                //
                iLength = GetLengthFromDLC(CANMsg.DLC, (CANMsg.MSGTYPE & TPCANMessageType.PCAN_MESSAGE_FD) == 0);
                for (int i = 0; i < iLength; i++)
                {
                    CANMsg.DATA[i] = sendMSG[i];
                }
            }

            // The message is sent to the configured hardware
            //
            return PCANBasic.WriteFD(m_PcanHandle, ref CANMsg);
        }

        /// <summary>
        /// Function for reading messages on FD devices
        /// </summary>
        /// <returns>A TPCANStatus error code</returns>
        private TPCANStatus ReadMessageFD()
        {
            TPCANTimestampFD CANTimeStamp;
            TPCANStatus stsResult;

            TPCANMsgFD msgFD;
            // We execute the "Read" function of the PCANBasic                
            //
            stsResult = PCANBasic.ReadFD(m_PcanHandle, out msgFD, out CANTimeStamp);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                CANMsgFD.ID = msgFD.ID;
                CANMsgFD.DLC = msgFD.DLC;
                CANMsgFD.MSGTYPE = msgFD.MSGTYPE;
                CANMsgFD.DATA = new byte[64];
                for (int i = 0; i < msgFD.DLC; i++)
                {
                    CANMsgFD.DATA[i] = msgFD.DATA[i];
                }
            }
            return stsResult;
        }

        TPCANMsg CANMsg;
        TPCANMsgFD CANMsgFD;

        /// <summary>
        /// Function for reading CAN messages on normal CAN devices
        /// </summary>
        /// <returns>A TPCANStatus error code</returns>
        private TPCANStatus ReadMessage()
        {
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;
            TPCANMsg msg;

            // We execute the "Read" function of the PCANBasic                
            //
            stsResult = PCANBasic.Read(m_PcanHandle, out msg, out CANTimeStamp);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                CANMsg.ID = msg.ID;
                CANMsg.LEN = msg.LEN;
                CANMsg.DATA= new byte[24];
                for (int i = 0; i < ((msg.LEN > 8) ? 8 : msg.LEN); i++)
                {
                    CANMsg.DATA[i] = msg.DATA[i];
                }
            }
            return stsResult;
        }

        private string TrimString(string Result)
        {
            if (Result.Contains("timeout")) return "timeout";
            if (Result == null || Result == "") return "";
            string RtnResult = "";
            string STR1 = Result;
            int j = 0;
            try
            {
                if (Result.StartsWith("03 7F 2F 78") && Result.Length > 25)
                {
                    STR1 = Result.Substring(24, Result.Length - 24);
                }

                string[] splitsendstring = STR1.Split(' ');
                int[] ConverttoInt = new int[splitsendstring.Length];
                for (j = 0; j < splitsendstring.Length; j++)
                {
                    ConverttoInt[j] = Convert.ToInt32(splitsendstring[j], 16);
                }

                if (ConverttoInt[0] == 0x10)
                {
                    int length = ConverttoInt[1];
                    int count = 0;
                    int index = 1;
                    do
                    {
                        index++;
                        if (index % 8 != 0)
                        {
                            RtnResult += ConverttoInt[index].ToString("X2") + " ";
                            count++;
                        }
                    } while (count < length);
                    RtnResult = RtnResult.TrimEnd(' ');
                }
                else
                {
                    string strB = null;
                    strB = STR1.Substring(3, ConverttoInt[0] * 3 - 1);
                    RtnResult = strB;
                }
            }
            catch (Exception ex)
            {
                return Result;
                //throw ex;
            }
            return RtnResult;
        }

        #endregion

        #region 初始化，退出
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="channel">1或者2</param>
        /// <param name="bitRate">kbit per sec</param>
        public void Initialize(int channel, int KbitRate)
        {
            TPCANStatus stsResult;

            if (channel == 1) m_PcanHandle = 0x51;
            else if (channel == 2) m_PcanHandle = 0x52;
            else throw new Exception("PCAN_USB卡通道只有1和2");

            switch (KbitRate)
            {
                case 1000:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_1M;
                    break;
                case 800:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_800K;
                    break;
                case 500:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_500K;
                    break;
                case 250:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
                    break;
                case 125:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_125K;
                    break;
                case 100:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_100K;
                    break;
                case 95:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_95K;
                    break;
                case 83:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_83K;
                    break;
                case 50:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_50K;
                    break;
                case 47:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_47K;
                    break;
                case 33:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_33K;
                    break;
                case 20:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_20K;
                    break;
                case 10:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_10K;
                    break;
                case 5:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_5K;
                    break;
            }
            // Connects a selected PCAN-Basic channel
            //
            if (m_IsFD)
            {
                string fdBitRate = "f_clock_mhz=20, nom_brp=5, nom_tseg1=2, nom_tseg2=1, nom_sjw=1, data_brp=2, data_tseg1=3, data_tseg2=1, data_sjw=1";
                stsResult = PCANBasic.InitializeFD(
                    m_PcanHandle,
                    fdBitRate);
            }
            else
            {
                m_HwType = TPCANType.PCAN_TYPE_ISA;
                stsResult = PCANBasic.Initialize(
                    m_PcanHandle,
                    m_Baudrate,
                    m_HwType,
                    0x100,
                    3);
            }

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                if (stsResult != TPCANStatus.PCAN_ERROR_CAUTION)
                {
                    throw new Exception("PCAN:" + GetFormatedError(stsResult));
                }
                else
                {
                    stsResult = TPCANStatus.PCAN_ERROR_OK;
                }
            }
        }


        public void Quit()
        {
            // Releases a current connected PCAN-Basic channel
            //
            PCANBasic.Uninitialize(m_PcanHandle);
            Read_Stop();
            Send_Stop();
            read_th = null;
            send_th = null;
        }
        #endregion

        #region 发送，接受线程
        private bool Send(uint sendID, byte DLC, byte[] sendDataArr)
        {
            TPCANStatus stsResult;

            bool isExt = false;
            if (sendID > 0x7ff) isExt = true;

            // Send the message
            //
            if (m_IsFD)
            {
                stsResult = WriteFrameFD(sendID, DLC, isExt, false, false, sendDataArr);
            }
            else
            {
                stsResult = WriteFrame(sendID, DLC, isExt, false, sendDataArr);
            }

            // The message was successfully sent
            //
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Send(uint sendID, byte DLC, string sendStr)
        {
            string[] StringDataArr = sendStr.Split(' ');
            byte[] Data = new byte[StringDataArr.Length];

            for (int i = 0; i < StringDataArr.Length; i++)
            {
                Data[i] = Convert.ToByte(StringDataArr[i], 16);
            }

            return Send(sendID, DLC, Data);
        }

        private void KeepSend()
        {
            TPCANStatus stsResult;
            uint sendID = SendID;

            byte DLC = 8;

            string[] StringDataArr = send_th_Msg.Split(' ');
            DLC = (byte)(StringDataArr.Length);
            byte[] Data = new byte[StringDataArr.Length];       

            for (int i = 0; i < StringDataArr.Length; i++)
            {
                Data[i] = Convert.ToByte(StringDataArr[i], 16);
            }

            bool isExt = false;
            if (sendID > 0x7ff) isExt = true;

            // Send the message
            //

            while (send_th_status)
            {
                if (m_IsFD)
                {
                    stsResult = WriteFrameFD(sendID, DLC, isExt, false, false, Data);
                }
                else
                {
                    stsResult = WriteFrame(sendID, DLC, isExt, false, Data);
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(sendInterTimesMs));//发送间隔时间
            }
        }

        //02 3E 00 00 00 00 00 00
        public void Send_Begin(uint send_id, int DLC, string send_msg) //开线程发送数据
        {
                SendID = send_id;
                send_th_Msg = send_msg;
                if (send_th_status == false)
                {
                    send_th_status = true;
                    send_th = new Thread(KeepSend);
                    send_th.IsBackground = false;
                    send_th.Start();
                }
        }

        public void Send_Stop() //关闭读数据线程
        {
                send_th_status = false;
                if (send_th != null && send_th.IsAlive == true)
                {
                    send_th.Abort();
                    Thread.Sleep(20);
                }
        }

        /// <summary>
        /// Function for reading PCAN-Basic messages
        /// </summary>
        private string Read(uint readID, int waitMs)
        {
            TPCANStatus stsResult;
            //TPCANMsg CANMsg = new TPCANMsg();
            //TPCANMsgFD CANMsgFD = new TPCANMsgFD();

            string rtnStr = "";
            uint ID = 0;
            byte[] dataBytesArr = new byte[64];

            stsResult = 0;

            Stopwatch myWatch = new Stopwatch();
            myWatch.Start();
            // We read at least one time the queue looking for messages.
            // If a message is found, we look again trying to find more.
            // If the queue is empty or an error occurr, we get out from
            // the dowhile statement.
            //			
            do
            {

                if (Convert.ToBoolean(stsResult & TPCANStatus.PCAN_ERROR_QRCVEMPTY))
                {
                    continue;
                }
                else
                {
                    stsResult = m_IsFD ? ReadMessageFD() : ReadMessage();
                    if (stsResult == TPCANStatus.PCAN_ERROR_ILLOPERATION)
                    {
                        rtnStr = "Invalid operation";
                        break;
                    }

                    ID = m_IsFD ? CANMsgFD.ID : CANMsg.ID;

                    if (ID == readID)
                    {
                        if (m_IsFD)
                        {
                            for (int i = 0; i < CANMsgFD.DLC; i++)
                            {
                                rtnStr += CANMsgFD.DATA[i].ToString("X2") + ' ';
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ((CANMsg.LEN > 8) ? 8 : CANMsg.LEN); i++)
                            {
                                rtnStr += CANMsg.DATA[i].ToString("X2") + ' ';
                            }
                        }
                        break;
                    }
                    else
                    {
                        rtnStr = "timeout";
                    }
                }
            } while (myWatch.ElapsedMilliseconds <= waitMs);

            return rtnStr;
        }

        private void KeepRead()
        {
            TPCANStatus stsResult;

            string rtnStr = "";
            uint ID = 0;
            byte[] dataBytesArr = new byte[64];

            do
            {
                stsResult = m_IsFD ? ReadMessageFD() : ReadMessage();
                if (stsResult == TPCANStatus.PCAN_ERROR_ILLOPERATION)
                {
                    rtnStr = "Invalid operation";
                    break;
                }

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                {
                    Console.WriteLine(stsResult + ":" + CANMsg.ID + "\n");
                }

                ID = m_IsFD ? CANMsgFD.ID : CANMsg.ID;

                if (ID == ReadID)
                {
                    rtnStr = "";
                    if (m_IsFD)
                    {
                        for (int i = 0; i < CANMsgFD.DLC; i++)
                        {
                            rtnStr += CANMsgFD.DATA[i].ToString("X2") + ' ';
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ((CANMsg.LEN > 8) ? 8 : CANMsg.LEN); i++)
                        {
                            rtnStr += CANMsg.DATA[i].ToString("X2") + ' ';
                        }
                    }

                    //Console.WriteLine(rtnStr);

                    rtnStr = rtnStr.ToUpper();

                    lock (CanMsgListlock)
                    {
                        CanMsgList.Add(rtnStr);
                    }
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(readInterTimesMs));//接受间隔时间
            } while (read_th_status);
        }

        private string Return_CanMsg(string waitMsg, long waitTime)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            string rtn = "";
            int msgNum = 0;
            do
            {
                    msgNum = CanMsgList.Count;
                    if (msgNum > 0)
                    {
                        lock (CanMsgListlock)
                        {
                            foreach (string s in CanMsgList)
                            {
                                if (s.Contains(waitMsg.ToUpper()))
                                {
                                    rtn = s;
                                    
                                    //lock (CanMsgListlock)
                                    //{
                                    CanMsgList.Clear();
                                    //}
                                    return rtn;
                                }
                            }
                        }
                    }

            } while (timer.ElapsedMilliseconds < waitTime);

            rtn = "timeout";
            timer.Stop();
            lock (CanMsgListlock)
            {
                CanMsgList.Clear();
            }
            return rtn;
        }


        public void Read_Begin(uint read_id) //开线程读数据
        {
            ReadID = read_id;
            if (read_th_status == false)
            {
                read_th_status = true;
                read_th = new Thread(KeepRead);
                read_th.IsBackground = false;
                read_th.Start();
            }
        }

        public void Read_Stop() //关闭读数据线程
        {
            read_th_status = false;
            if (read_th != null && read_th.IsAlive == true)
            {
                read_th.Abort();
                Thread.Sleep(20);
            }
        }

        #endregion

        #region send_read

        public string UrgeFrame = "30 08 00 00 00 00 00 00";

        public void SetUrgeFrame(string urgeFrame)
        {
            UrgeFrame = urgeFrame;
        }

        /// <summary>
        /// 发单帧收单帧/多帧,需要先开通接收线程,结束之后记得关闭接受线程
        /// </summary>
        /// <param name="SendID">PC ID</param>
        /// <param name="ReadID">产品ID</param>
        /// <param name="SendString">10 03</param>
        /// <param name="waitString">50 03</param>
        /// <param name="ReadTimeout">读数据超时</param>
        /// <returns>返回读到的数据</returns>
        private string SendSingleFrame_Read(uint SendID, uint ReadID, string SendString, string waitString, int ReadTimeout)
        {
            Stopwatch timer = new Stopwatch();
            int frameNum;
            string[] splitsendstring = SendString.Trim().Split(' ');
            string RTResult = "";
            string STR1 = "";
            int msgNum = 0;

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

            Send(SendID, 8, Data);
            RTResult = Return_CanMsg(waitString, ReadTimeout);//收头帧
            if (RTResult == "timeout")
            {
                return "timeout1"; //100ms timeout
            }

            int x = RTResult.IndexOf(waitString) - 3;
            int dataLen = Convert.ToInt32(RTResult.Substring(x, 2), 16);
            if (dataLen <= 7)       //单帧
            {
                frameNum = 1;

                string tmpStr = RTResult;
                try
                {
                    RTResult = RTResult.Substring(x, 23);
                }
                catch (Exception ex)
                {
                    RTResult = tmpStr;
                }

                return RTResult = TrimString(RTResult);
            }

            //多帧
            string tmpStr1 = RTResult;
            try
            {
                x = RTResult.IndexOf(waitString) - 6;
                RTResult = RTResult.Substring(x, 23) + ' ';
            }
            catch (Exception ex)
            {
                RTResult = tmpStr1;
            }

            int temp = dataLen + 1;
            if (temp % 7 == 0)
            {
                frameNum = temp / 7;
            }
            else
            {
                frameNum = temp / 7 + 1;
            }

            Send(SendID, 8, UrgeFrame);   //////////////////////////////////发送30帧

            timer.Start();
            STR1 = "";
            do
            {
                int index = 0;
                msgNum = CanMsgList.Count;
                if (msgNum > 0)
                {
                    for (int i = 0; i < msgNum; i++)
                    {
                        //Console.WriteLine("canlist count:" + CanMsgList.Count + '\n');
                        if (CanMsgList[i].StartsWith("21"))
                        {
                            index = i;
                            break;
                        }
                    }

                    if (msgNum >= index + frameNum - 1)
                    {
                        for (int i = index; i < index + frameNum - 1; i++)
                        {
                            STR1 = STR1 + CanMsgList[i];
                        }
                        break;
                    }
                }
                Thread.Sleep(3);

            } while (timer.ElapsedMilliseconds < ReadTimeout);
            timer.Stop();

            RTResult += STR1;
            RTResult = RTResult.Trim();
            return RTResult = TrimString(RTResult);
        }

        /// <summary>
        /// 发多帧收单帧/多帧,需要先开通接收线程,结束之后记得关闭接受线程
        /// </summary>
        /// <param name="SendID">PC ID</param>
        /// <param name="ReadID">产品ID</param>
        /// <param name="SendString">22 5C 20 01 92 10 48 01</param>
        /// <param name="waitString">62 5C 20</param>
        /// <param name="ReadTimeout">读数据超时</param>
        /// <returns>返回读到的数据</returns>
        public string Send_Read(uint SendID, uint ReadID, string SendString, string waitString, int ReadTimeout)
        {
            Stopwatch timer = new Stopwatch();
            string[] splitsendstring = SendString.Trim().Split(' ');
            string RTResult = "";
            string STR1 = "", STR2;
            int dataLen = splitsendstring.Length;
            byte[] data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int n = 0; n < dataLen; n++)
            {
                data[n] = Convert.ToByte(splitsendstring[n], 16);
            }

            byte[] sendArray = new byte[8];

            int frameNum = 0;
            int temp, i, j;
            int msgNum = 0;

            //Read_Begin(ReadID);
            if (dataLen <= 7) //如果发送单帧
            {
                frameNum = 1;
                RTResult = SendSingleFrame_Read(SendID, ReadID, SendString, waitString, ReadTimeout);
                lock (CanMsgListlock)
                {
                    CanMsgList.Clear();
                }
                return RTResult;
            }

            temp = dataLen + 1;
            if (temp % 7 == 0)
            {
                frameNum = temp / 7;
            }
            else
            {
                frameNum = temp / 7 + 1;
            }

            sendArray[0] = 0x10;
            sendArray[1] = Convert.ToByte(dataLen);
            sendArray[2] = data[0];
            sendArray[3] = data[1];
            sendArray[4] = data[2];
            sendArray[5] = data[3];
            sendArray[6] = data[4];
            sendArray[7] = data[5];
            Send(SendID, 8, sendArray); /////////////////////////////////发送头帧

            string tempStr1 = Return_CanMsg("30", 100); //////////////接受30帧

            temp = 6;
            for (i = 1; i < frameNum; i++)
            {
                sendArray[0] = (byte)(0x20 + i);
                for (j = 1; j < 8; j++)
                {
                    sendArray[j] = data[temp++];
                }
                Send(SendID,8, sendArray);//////////////////////////////发送剩余帧
                Thread.Sleep(10);
            }
            STR1 = Return_CanMsg(waitString, ReadTimeout);/////////////////接受头帧
            if (STR1.StartsWith("10"))/////回复多帧
            {
                Send(SendID, 8, UrgeFrame);   /////////////////////////////////发送30帧

                timer.Start();
                STR2 = "";
                do
                {
                    int index = 0;
                    msgNum = CanMsgList.Count;
                    if (msgNum > 0)
                    {
                        for (int n = 0; n < CanMsgList.Count; n++)
                        {
                            if (CanMsgList[n].StartsWith("21"))
                            {
                                index = n;
                                break;
                            }
                        }

                        if (CanMsgList.Count >= index + frameNum - 1)
                        {
                            for (int n = index; n < index + frameNum - 1; n++)
                            {
                                STR2 = STR2 + CanMsgList[n];
                            }
                            break;
                        }
                    }
                    Thread.Sleep(3);

                } while (timer.ElapsedMilliseconds < ReadTimeout);
                timer.Stop();
                STR2 = STR2.Trim();

                if (STR1.Contains(waitString))
                {
                    //int x = STR1.IndexOf(waitString) - 6;
                    //STR1 = STR1.Substring(x, 24);
                }
                else
                {
                    lock (CanMsgListlock)
                    {
                        CanMsgList.Clear();
                    }
                    return STR1.Trim();
                }
                if (STR2.Contains("21 "))
                {

                    //int x = STR2.IndexOf("21 ");
                    //int l = STR2.Length - x;
                    //STR2 = STR2.Substring(x, l);
                }
                else
                {
                    lock (CanMsgListlock)
                    {
                        CanMsgList.Clear();
                    }
                    return STR1.Trim();
                }

                RTResult = STR1 + STR2;
                RTResult = RTResult.Trim();
                RTResult = TrimString(RTResult);
            }
            else/////////回复单帧
            {
                if (STR1.Contains(waitString))
                {
                    //int x = STR1.IndexOf(waitString) - 6;
                    //STR1 = STR1.Substring(x, 24);
                    RTResult = STR1.Trim();
                    RTResult = TrimString(RTResult);
                }
                else
                {
                    RTResult = STR1;
                }
            }

            lock (CanMsgListlock)
            {
                CanMsgList.Clear();
            }
            return RTResult;
        }


        #endregion

    }
}
