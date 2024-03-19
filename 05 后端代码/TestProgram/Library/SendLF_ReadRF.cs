using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Xml;
using Common;

namespace Library
{
    public class SendLF_ReadRF
    {
        static byte dataNum = 6;
        private List<byte> dataArrayByte = new List<byte>();
        private List<byte> bitList = new List<byte>();
        private List<double> AO_Datalist = new List<double>();

        static double lf_bitRate = 7800; //3.9K*2
        //static double lf_bitRate = 3900;

        static double RKE_LEVEL0 = -1;  //AO口输出0基带信号电平为-1V
        static double RKE_LEVEL1 = 1;   //AO口输出1基带信号电平为1V
        static string DAQ_NAME = "Dev1/ao0";

        Library.AG_N9320B myN9320B;
        Library.AG33210A my33210;
        Library.NI_DAQ myDAQ = new Library.NI_DAQ();
        Library.AES加解密 aes1 = new Library.AES加解密();
        Library.BOX_control mybox = new BOX_control();
        double vs1x = 0;
        double vs2x = 0;
        double vs3x = 0;

        double vs1y = 0;
        double vs2y = 0;
        double vs3y = 0;

        double vs1z = 0;
        double vs2z = 0;
        double vs3z = 0;

        byte C1 = 0;
        byte C2 = 0;
        byte C3 = 0;


        int FRR1 = 0;
        int FRR2 = 0;
        int FRR3 = 0;
        int CalFR = 0;//获取数据字节长度不够   
        int CalR = 1;//计算的标定值不合要求
        


        #region 初始化，配置，退出
        public void Initialize(object one9320, object one33210, string daq_name)
        {
            myN9320B = one9320 as Library.AG_N9320B;
            my33210 = one33210 as Library.AG33210A;
            DAQ_NAME = daq_name;
            myDAQ.AO_Create_Task2(daq_name);
            myDAQ.DO_Create_Task("dev1/line2");
            mybox.Initialize(7, 9600);
        }

        public void ConfigLF(string FM_AM, double Freq, double VPP, int depth_deviation)
        {
            if (FM_AM.ToUpper() == "FM")
            {
                my33210.Set_FM_EXT(Freq, VPP, depth_deviation);
                my33210.AM_StateOff();
                my33210.FM_StateOn();
            }
            else if (FM_AM.ToUpper() == "AM")
            {
                my33210.Set_AM_EXT(Freq, VPP, depth_deviation);
                my33210.FM_StateOff();
                my33210.AM_StateOn();
            }
        }

        public void quitbox()
        {
            mybox.Quit();
        }
        public void Quit()
        {
            myDAQ.AO_Dispose_Task2();
        }
        #endregion

        #region caculate LF SEND date
        private void CreateByteArray(string dataHex)
        {
            int i = 0;
            dataArrayByte.Clear();

            //ZERO+RFC+ADDR_LOW+FC+DATA CONTROL
            //dataArrayByte.Add(0x29);
            //dataArrayByte.Add(0xFF);

            //command--6 bytes
            string[] splitsendstring = dataHex.Trim().Split(' '); //用空格隔开转化为数组
            int datalen = splitsendstring.Length;
            for (i = 0; i < datalen; i++)
            {
                dataArrayByte.Add(Convert.ToByte(splitsendstring[i], 16));
            }

            //checksum
            dataArrayByte.Add(CRC8(dataArrayByte));
            //dataArrayByte.Add(CaculateCRC(dataArrayByte, 3, (ushort)datalen));//from Command
            //dataArrayByte.Add(0x37);
            //dataArrayByte.Add(CaculateCRC(dataArrayByte, 0, (ushort)(datalen+2)));//ALL DATA
        }

        //private void CreateBitArray(string dataHex)
        //{
        //    byte bitData, byteDate;
        //    ushort i, j, k, t;

        //    bitList.Clear();
        //    //prea


        //    for (i = 0; i < 2; i++)
        //    {
        //        bitList.Add(0);
        //    }

        //    for (i = 0; i < 128; i++)
        //    {
        //        if (((i/8)%2)==1)
        //        bitList.Add(0);
        //        else
        //        bitList.Add(1);
        //    }
        //    for (i = 0; i < 48; i++)
        //    {
        //        if (((i / 18) % 2) == 1)
        //            bitList.Add(0);
        //        else
        //            bitList.Add(1);
        //    }
        //    for (i = 0; i < 16; i++)
        //    {
        //        if (((i / 8) % 2) == 1)
        //            bitList.Add(0);
        //        else
        //            bitList.Add(1);
        //    }
        //    for (i = 0; i < 64; i++)
        //    {
        //        if (((i / 10) % 2) == 1)
        //            bitList.Add(0);
        //        else
        //            bitList.Add(1);
        //    }
        //    for (i = 0; i < 16; i++)
        //    {
        //        if (((i / 8) % 2) == 1)
        //            bitList.Add(0);
        //        else
        //            bitList.Add(1);
        //    }


        //    //DATA////////////////////////////////////////////////////
        //    CreateByteArray(dataHex);
        //    int dataNum = dataArrayByte.Count;

        //    for (i = 0; i < dataNum; i++)
        //    {
        //        byteDate = dataArrayByte[i];

        //        for (k = 8; k > 0; k--)      //先高后低
        //        {
        //            j = (ushort)(8 - k);
        //            bitData = (byte)((byte)(byteDate << j) >> 7);
        //            if (bitData == 0)
        //            {
        //                //T0
        //                bitList.Add(1);
        //                bitList.Add(0);
        //            }
        //            else if (bitData == 1)
        //            {
        //                //T1
        //                bitList.Add(0);
        //                bitList.Add(1);
        //            }
        //        }
        //    }

        //    //RSSIP///////////////////////////////////////////////////
        //    //
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);
        //    bitList.Add(1);

        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);
        //    bitList.Add(0);

        //    //LF Carrier/////////////////////////////////////////////
        //    for (i = 0; i < 800; i++)
        //    {
        //        bitList.Add(1);
        //    }

        //    //bitList.Add(0);

        //    foreach(byte s in bitList)
        //    {
        //        Console.Write(s);
        //    }
        //    string m = "";
        //    foreach (byte e in bitList)
        //    {
        //        m = m + e.ToString();
        //    }
        //    m = m;
        //}

        private void CreateBitArray(string dataHex)
        {
            byte bitData, byteDate;
            ushort i, j, k, t;

            bitList.Clear();

            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);


            //prea
            
            bitList.Add(0);
            bitList.Add(0);
            for (i = 0; i < 8; i++)       //先发送PREA 8个反相曼切斯特0
            {
                bitList.Add(1);
                bitList.Add(0);
            }
            //LFSYNC                     //发送LFSYNC(物理层)：111000101100110010
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);

            //WUP
            bitList.Add(1);               //发送WUP（物理层）：10101001 10100110 10100101 10011010 1001 0101
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);

            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(0);

            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);

            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);

            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(0);
            bitList.Add(1);

            bitList.Add(0);
            bitList.Add(1);
            bitList.Add(0);
            bitList.Add(1);


            //DATA////////////////////////////////////////////////////    发送data部分：29 FF ..... checksum
            CreateByteArray(dataHex);
            int dataNum = dataArrayByte.Count;

            for (i = 0; i < dataNum; i++)
            {
                byteDate = dataArrayByte[i];

                for (k = 8; k > 0; k--)      //先高后低  
                {
                    j = (ushort)(8 - k);
                    bitData = (byte)((byte)(byteDate << j) >> 7);
                    if (bitData == 0)
                    {
                        //T0
                        bitList.Add(1);
                        bitList.Add(0);
                    }
                    else if (bitData == 1)
                    {
                        //T1
                        bitList.Add(0);
                        bitList.Add(1);
                    }
                }
            }

            //RSSIP///////////////////////////////////////////////////  反相曼切斯特1
            //
            bitList.Add(0);
            bitList.Add(1);

            //RSSIP/////////////////////////////////////////////        延时13ms  
            for (i = 0; i < 110; i++)
            {
                bitList.Add(1);
            }

            //bitList.Add(0);

            //foreach (byte s in bitList)
            //{
            //    Console.Write(s);
            //}
            bitList.Add(0);
            string m = "";
            foreach (byte e in bitList)
            {
                m = m + e.ToString();
            }
            m = m;
        }

        private void CreateAOvoltArray(string dataHex)
        {
            //int index = 0;
            int i;
            CreateBitArray(dataHex);

            AO_Datalist.Clear();

            ////preamble////////////////////////////////////////////////
            ////11111111111111111111111111
            //double step = (RKE_LEVEL1-0)/50;
            //double level_x = 0;
            //for (i = 0; i < 50; i++)
            //{
            //    AO_Datalist.Add(RKE_LEVEL1);

            //    //AO_Datalist.Add(level_x);
            //    //level_x += step;
            //}

            //Sof+data+checksum+eof+LFcarrier///////////////////////////////
            foreach (uint x in bitList)
            {
                if (x == 0)
                {
                    AO_Datalist.Add(RKE_LEVEL0);
                }
                else if (x == 1)
                {

                    AO_Datalist.Add(RKE_LEVEL1);
                }
            }

            for (int iii = 0; iii < 150; iii++)
            {
                AO_Datalist.Add(1);
            }
            for (int iii = 0; iii < 150; iii++)
            {
                AO_Datalist.Add(0);
            }
            myDAQ.AO_Task2_Config(lf_bitRate, AO_Datalist.Count);
        }

        //查表法校验
        public byte GetCRC8(byte Index)
        {

            byte[] list = new byte[] { 0x00, 0x07, 0x0E, 0x09, 0x1C, 0x1B, 0x12, 0x15, 0x38, 0x3F, 0x36, 0x31, 0x24, 0x23, 0x2A, 0x2D, 0x70, 0x77, 0x7E, 0x79, 0x6C, 0x6B, 0x62, 0x65, 0x48, 0x4F, 0x46, 0x41, 0x54, 0x53, 0x5A, 0x5D, 0xE0, 0xE7, 0xEE, 0xE9, 0xFC, 0xFB, 0xF2, 0xF5, 0xD8, 0xDF, 0xD6, 0xD1, 0xC4, 0xC3, 0xCA, 0xCD, 0x90, 0x97, 0x9E, 0x99, 0x8C, 0x8B, 0x82, 0x85, 0xA8, 0xAF, 0xA6, 0xA1, 0xB4, 0xB3, 0xBA, 0xBD, 0xC7, 0xC0, 0xC9, 0xCE, 0xDB, 0xDC, 0xD5, 0xD2, 0xFF, 0xF8, 0xF1, 0xF6, 0xE3, 0xE4, 0xED, 0xEA, 0xB7, 0xB0, 0xB9, 0xBE, 0xAB, 0xAC, 0xA5, 0xA2, 0x8F, 0x88, 0x81, 0x86, 0x93, 0x94, 0x9D, 0x9A, 0x27, 0x20, 0x29, 0x2E, 0x3B, 0x3C, 0x35, 0x32, 0x1F, 0x18, 0x11, 0x16, 0x03, 0x4, 0x0D, 0x0A, 0x57, 0x50, 0x59, 0x5E, 0x4B, 0x4C, 0x45, 0x42, 0x6F, 0x68, 0x61, 0x66, 0x73, 0x74, 0x7D, 0x7A, 0x89, 0x8E, 0x87, 0x80, 0x95, 0x92, 0x9B, 0x9C, 0xB1, 0xB6, 0xBF, 0xB8, 0xAD, 0xAA, 0xA3, 0xA4, 0xF9, 0xFE, 0xF7, 0xF0, 0xE5, 0xE2, 0xEB, 0xEC, 0xC1, 0xC6, 0xCF, 0xC8, 0xDD, 0xDA, 0xD3, 0xD4, 0x69, 0x6E, 0x67, 0x60, 0x75, 0x72, 0x7B, 0x7C, 0x51, 0x56, 0x5F, 0x58, 0x4D, 0x4A, 0x43, 0x44, 0x19, 0x1E, 0x17, 0x10, 0x05, 0x2, 0xB, 0xC, 0x21, 0x26, 0x2F, 0x28, 0x3D, 0x3A, 0x33, 0x34, 0x4E, 0x49, 0x40, 0x47, 0x52, 0x55, 0x5C, 0x5B, 0x76, 0x71, 0x78, 0x7F, 0x6A, 0x6D, 0x64, 0x63, 0x3E, 0x39, 0x30, 0x37, 0x22, 0x25, 0x2C, 0x2B, 0x06, 0x01, 0x08, 0x0F, 0x1A, 0x1D, 0x14, 0x13, 0xAE, 0xA9, 0xA0, 0xA7, 0xB2, 0xB5, 0xBC, 0xBB, 0x96, 0x91, 0x98, 0x9F, 0x8A, 0x8D, 0x84, 0x83, 0xDE, 0xD9, 0xD0, 0xD7, 0xC2, 0xC5, 0xCC, 0xCB, 0xE6, 0xE1, 0xE8, 0xEF, 0xFA, 0xFD, 0xF4, 0xF3 };
            return list[Index];
        }
        public byte CRC8(List<byte> data)
        {
            byte CRC_Temp = 0x01;
            byte CRC_Index = 0x00;

            for (int i = 0; i < data.Count; i++)
            {
                CRC_Index = (byte)(CRC_Temp ^ data[i]);
                CRC_Temp = GetCRC8(CRC_Index);
            }
            return CRC_Temp;

        }
        //带进位加法，计算CRC校验
        private byte CaculateCRC(List<byte> data, ushort startIndex, ushort lenth)
        {
            byte crc = 0x01;
            int i = 0;
            Int16 temp;
            Int16 array;
            byte[] d = data.ToArray();

            for (i = startIndex; i < (startIndex + lenth); i++)
            {
                //temp = (Int16)(crc + d[i]);
                //array = (Int16)((temp >> 8) & 0x01);
                //crc = (byte)(temp % 0x100 + array);
                for (int j = 1; j < 256; j = j * 2)
                {
                    if ((crc & 1) != 0) { crc /= 2; crc ^= 0xe0; }
                    else crc /= 2;
                    if (d[i] != 0) crc ^= 0xe0;
                }
            }
            //crc = (byte)(~crc);

            return crc;
        }

        private string GetUsefulString(string strIn, int startIndex, int dataNum)
        {
            if (strIn == "timeout")
                return "timeout";
            if (strIn.Contains(' '))
            {
                string[] strArray = strIn.Split(' ');
                string returnStr = null;
                for (int i = startIndex; i < startIndex + dataNum; i++)
                {
                    returnStr += strArray[i] + ' ';
                }
                return returnStr;
            }
            else
            {
                return strIn;
            }
        }

        #endregion

        #region send LF data
        public void LF_Send(string sendDataHex)
        {
            //产生载波信号
            double waveVPP = 10;
            LF_MOD_Config(134200, waveVPP, 100);
            LF_MOD_IN(sendDataHex);
        }

        public void LF_MOD_Config(double carrierFreq, double carrierVPP, int ASK_Depth)
        {
            //产生载波信号
            my33210.Set_AM_EXT(carrierFreq, carrierVPP, 100);
            RKE_LEVEL0 = -5; //AO口输出0基带信号电平为-5V
            RKE_LEVEL1 = 5;  //AO口输出1基带信号电平为5V
            my33210.AM_StateOn();//或者先调用ConfigLF
        }

        public void LF_MOD_Config(double carrierVPP)
        {
            //产生载波信号
            my33210.Set_AM_EXT_VPP(carrierVPP);
            //RKE_LEVEL0 = -5; //AO口输出0基带信号电平为-5V
            //RKE_LEVEL1 = 5;  //AO口输出1基带信号电平为5V
            //my33210.AM_StateOn();//或者先调用ConfigLF
        }

        public void LF_OUTPUT_ON()
        {
            my33210.Output_On();
        }

        public void LF_OUTPUT_OFF()
        {
            my33210.Output_Off();
        }

        public void LF_MOD_IN(string sendDataHex)
        {
            //my33210.Output_On();
            //System.Threading.Thread.Sleep(500);

            //产生基带信号
            CreateAOvoltArray(sendDataHex);
            double[] AOarray = AO_Datalist.ToArray();
            myDAQ.AO_Task2_Write_Data2Buffer(AOarray);
            myDAQ.AO_Start_Task2();
            System.Threading.Thread.Sleep(500);
            myDAQ.AO_Stop_Task2();

            //myDAQ.AO_Start_Task2();
            //System.Threading.Thread.Sleep(300);
            //myDAQ.AO_Stop_Task2();

            //myDAQ.AO_Start_Task2();
            //System.Threading.Thread.Sleep(300);
            //myDAQ.AO_Stop_Task2();
            //my33210.Output_Off();
        }

        #endregion


        #region 3D标定

        public void LF_X_ON()
        {

            mybox.SetX();
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Y, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Z, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_X, true);
        }

        public void LF_Y_ON()
        {

            mybox.SetY();
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_X, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Z, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Y, true);
        }

        public void LF_Z_ON()
        {

            mybox.SetZ();
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_X, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Y, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Z, true);
        }

        public void LF_XYZ_OFF()
        {
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_X, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Y, false);
            //MdlClass.readplc.SetSingleCoil(MdlClass.readplc.Q_LF_Z, false);
        }

        public float ConverInt32TOfloat(int x)
        {
            byte[] y = BitConverter.GetBytes(x);
            float z = BitConverter.ToSingle(y, 0);
            return z;
        }

        public int ConverFloatTOint32(float x)
        {
            byte[] y = BitConverter.GetBytes(x);
            int z = y[3] * 0x1000000 + y[2] * 0x10000 + y[1] * 0x100 + y[0];
            return z;
        }

        private string VRSSIX, VRSSIY, VRSSIZ;
        private double Vrssix, Vrssiy, Vrssiz;

        #region 3d backup
        //public string FOB_3Dcalibration(int k)
        //{
        //    string rfDataHexStr = "";
        //    try
        //    {
        //        int waitTimes_ms = 2000;
        //        string waitStr = "FF FE";
        //        byte[] randomNumber = new byte[3] { 0, 0, 0 };      

        //        LF_MOD_Config(0.61); //0.61V vpp
        //        System.Threading.Thread.Sleep(1000);

        //        //read manuCode and SN
        //        LF_MOD_IN("F1 10 00 00 00 00");
        //        System.Threading.Thread.Sleep(waitTimes_ms);
        //        rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains("F1 10"))
        //        {
        //            return "Read manuCode and SN fail:" + rfDataHexStr;
        //        }
        //        int x = rfDataHexStr.IndexOf("FE F1 10");
        //        string McodeSNstr = rfDataHexStr.Substring(x + 9, 11);
        //        string[] str1 = McodeSNstr.Trim().Split(' ');
        //        byte[] McodeSN = new byte[str1.Length];
        //        for (int i = 0; i < str1.Length; i++)
        //        {
        //            McodeSN[i] = Convert.ToByte(str1[i], 16);
        //        }

        //        //read Kx, Ky, Kz
        //        for (int i = 0; i < 4; i++)
        //        {
        //            LF_MOD_IN("F1 14 00 00 00 00");
        //            System.Threading.Thread.Sleep(waitTimes_ms);
        //            rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //            if (rfDataHexStr.Contains("F1 14") && rfDataHexStr.Length >= 38)
        //            {
        //                break;
        //            }              
        //        }
        //        if (!rfDataHexStr.Contains("F1 14"))
        //        {
        //            return "Read Kx, Ky, Kz fail:" + rfDataHexStr;
        //        }
        //        x = rfDataHexStr.IndexOf("FE F1 14");
        //        string Kstr = rfDataHexStr.Substring(x + 24, 8);
        //        str1 = Kstr.Trim().Split(' ');
        //        byte Kx, Ky, Kz;
        //        Kx = Convert.ToByte(str1[0], 16);
        //        Ky = Convert.ToByte(str1[1], 16);
        //        Kz = Convert.ToByte(str1[2], 16);

        //        //meas Vrssi-------x
        //        //切换继电器

        //        System.Threading.Thread.Sleep(300);
        //        LF_MOD_IN("F4 00 00 00 00 00");
        //        System.Threading.Thread.Sleep(waitTimes_ms);
        //        rfDataHexStr = "x" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains("FE F4"))
        //        {
        //            return "Meas Vrssix fail:" + rfDataHexStr;
        //        }
        //        x = rfDataHexStr.IndexOf("FE F4");
        //        string Vstr = rfDataHexStr.Substring(x + 6, 11);
        //        str1 = Vstr.Trim().Split(' ');
        //        byte[] b1 = new byte[str1.Length];
        //        for (int i = 0; i < str1.Length; i++)
        //        {
        //            b1[i] = Convert.ToByte(str1[i], 16);
        //        }
        //        int Vint32 = b1[3] * 0x1000000 + b1[2] * 0x10000 + b1[1] * 0x100 + b1[0];
        //        double Vrssix = Math.Sqrt(ConverInt32TOfloat(Vint32));
        //        if (Kx != 0)
        //        {
        //            Vrssix = Vrssix / (Kx * Kx);
        //        }
        //        VRSSIX = rfDataHexStr;

        //        //meas Vrssi-------y
        //        //切换继电器
        //        myN9320B.Danalyse_Trace_ClearWrite();
        //        LF_Y_ON();
        //        System.Threading.Thread.Sleep(300);
        //        LF_MOD_IN("F4 00 00 00 00 00");
        //        System.Threading.Thread.Sleep(waitTimes_ms);
        //        rfDataHexStr = "y" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains("FE F4"))
        //        {
        //            return "Meas Vrssiy fail:" + rfDataHexStr;
        //        }
        //        x = rfDataHexStr.IndexOf("FE F4");
        //        Vstr = rfDataHexStr.Substring(x + 6, 11);
        //        str1 = Vstr.Trim().Split(' ');
        //        byte[] b2 = new byte[str1.Length];
        //        for (int i = 0; i < str1.Length; i++)
        //        {
        //            b2[i] = Convert.ToByte(str1[i], 16);
        //        }
        //        Vint32 = b2[3] * 0x1000000 + b2[2] * 0x10000 + b2[1] * 0x100 + b2[0];
        //        double Vrssiy = Math.Sqrt(ConverInt32TOfloat(Vint32));
        //        if (Ky != 0)
        //        {
        //            Vrssiy = Vrssiy / (Ky * Ky);
        //        }
        //        VRSSIY = rfDataHexStr;

        //        //meas Vrssi-------z
        //        //切换继电器
        //        myN9320B.Danalyse_Trace_ClearWrite();
        //        LF_Z_ON();
        //        System.Threading.Thread.Sleep(300);
        //        LF_MOD_IN("F4 00 00 00 00 00");
        //        System.Threading.Thread.Sleep(waitTimes_ms);
        //        rfDataHexStr = "z" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains("FE F4"))
        //        {
        //            return "Meas Vrssiz fail:" + rfDataHexStr;
        //        }
        //        x = rfDataHexStr.IndexOf("FE F4");
        //        Vstr = rfDataHexStr.Substring(x + 6, 11);
        //        str1 = Vstr.Trim().Split(' ');
        //        byte[] b3 = new byte[str1.Length];
        //        for (int i = 0; i < str1.Length; i++)
        //        {
        //            b3[i] = Convert.ToByte(str1[i], 16);
        //        }
        //        Vint32 = b3[3] * 0x1000000 + b3[2] * 0x10000 + b3[1] * 0x100 + b3[0];
        //        double Vrssiz = Math.Sqrt(ConverInt32TOfloat(Vint32));
        //        if (Ky != 0)
        //        {
        //            Vrssiz = Vrssiz / (Kz * Kz);
        //        }
        //        VRSSIZ = rfDataHexStr;

        //        //Enter EEPROM Security Access
        //        byte[] data = new byte[] { 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }; //加密数据
        //        byte[] key = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, randomNumber[0], randomNumber[1], randomNumber[2] }; //密钥，random number: 1,2,3
        //        byte[] v = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //密钥向量
        //        data[0] = McodeSN[0];
        //        data[1] = McodeSN[1];
        //        data[2] = McodeSN[2];
        //        data[3] = McodeSN[3];
        //        byte[] aesData = aes1.AESEncrypt(data, v, key);  //caculate AES data
        //        string aesB1B3str = aesData[1].ToString("X2") + " " + aesData[3].ToString("X2");
        //        string randomSTR = randomNumber[0].ToString("X2") + ' ' + randomNumber[1].ToString("X2") + ' ' + randomNumber[2].ToString("X2") + ' ';
        //        string cal3Dstr = "F3 " + randomSTR + aesB1B3str;

        //        string saSuccessStr = "FF FE " + aesData[3].ToString("X2") + ' ' + aesData[1].ToString("X2");
        //        for (int i = 0; i < 4; i++)
        //        {
        //            LF_MOD_IN(cal3Dstr);
        //            System.Threading.Thread.Sleep(waitTimes_ms);
        //            rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);

        //            if (rfDataHexStr.Contains(saSuccessStr))
        //            {
        //                break;
        //            }
        //        }
        //        //LF_MOD_IN(cal3Dstr);
        //        //System.Threading.Thread.Sleep(2000);
        //        //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains(saSuccessStr))
        //        {
        //            return "Enter EEPROM Security Access fail:" + rfDataHexStr + " must contain:" + saSuccessStr;
        //        }

        //        //Write new Kx, Ky, Kz
        //        //int k = 1;
        //        double Vmin = Math.Min(Vrssix, Vrssiy);
        //        Vmin = Math.Min(Vmin, Vrssiz);
        //        Kx = (byte)(Math.Sqrt(Vmin / Vrssix) * k * 100);
        //        Ky = (byte)(Math.Sqrt(Vmin / Vrssiy) * k * 100);
        //        Kz = (byte)(Math.Sqrt(Vmin / Vrssiz) * k * 100);
        //        string KxyzStr = "F2 12 00 " + Kx.ToString("X2") + " " + Ky.ToString("X2") + " " + Kz.ToString("X2");

        //        for (int i = 0; i < 4; i++)
        //        {
        //            LF_MOD_IN(KxyzStr);
        //            System.Threading.Thread.Sleep(waitTimes_ms);
        //            rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);

        //            if (rfDataHexStr.Contains("F2 12"))
        //            {
        //                break;
        //            }
        //        }
        //        //LF_MOD_IN(KxyzStr);
        //        //System.Threading.Thread.Sleep(waitTimes_ms);
        //        //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
        //        if (!rfDataHexStr.Contains("F2 12"))
        //        {
        //            return "Write new Kx, Ky, Kz fail:" + rfDataHexStr;
        //        }
        //        x = rfDataHexStr.IndexOf("F2 12");
        //        string kxkykz = rfDataHexStr.Substring(x + 9, 8);
        //        return "Kx, Ky, Kz:" + kxkykz;
        //    }
        //    catch(Exception ex)
        //    {
        //        return "try catch error " + rfDataHexStr + ":" + ex.Message;
        //    }
        //}
        #endregion

        //FF FE F1 10 B0 1C 2B 8C 79
        //FF FE F1 11 00 44 00 08 B0
        //FF FE F1 12 76 10 74 CB 35
        //FF FE F1 13 00 00 00 13 E7
        //FF FE F1 14 00 01 00 00 00 63 64 63 CD
        //FF FE F4 5E A7 A8 3A 22
        //FF FE 00 00 00 00 00 00 FF
        //FF FE F2 12 00 00 00 00 FA
        public string FOB_3Dcalibration(int k)
        {
            string rfDataHexStr = "";
            try
            {
                int waitTimes_ms = 1000;
                string waitStr = "FF FE";
                byte[] randomNumber = new byte[3] { 0, 0, 0 };

                myN9320B.SetMeasLength("FSK", 280);

                ////串1K
                //LF_MOD_Config(0.61); //0.61V vpp

                //串10K
                LF_MOD_Config(5.88);
                System.Threading.Thread.Sleep(1000);

                //read manuCode and SN
                for (int i = 0; i < 4; i++)
                {
                    LF_MOD_IN("F1 10 00 00 00 00");
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, "FF FE F1 10");
                    if (rfDataHexStr.Contains("FF FE F1 10") && rfDataHexStr.Length >= 23)
                    {
                        break;
                    }
                }
                if (!rfDataHexStr.Contains("F1 10"))
                {
                    return "Read manuCode and SN fail:" + rfDataHexStr;
                }
                int x = rfDataHexStr.IndexOf("FE F1 10");
                string McodeSNstr = rfDataHexStr.Substring(x + 9, 11);
                string[] str1 = McodeSNstr.Trim().Split(' ');
                byte[] McodeSN = new byte[str1.Length];
                for (int i = 0; i < str1.Length; i++)
                {
                    McodeSN[i] = Convert.ToByte(str1[i], 16);
                }

                //read Kx, Ky, Kz
                byte Kx, Ky, Kz;
                Kx = 0;
                Ky = 0;
                Kz = 0;

                //meas Vrssi-------x
                //切换继电器

                System.Threading.Thread.Sleep(300);

                for (int i = 0; i < 4; i++)
                {
                    LF_MOD_IN("F4 00 00 00 00 00");
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, "FF FE F4", 8);
                    if (rfDataHexStr.Contains("FF FE F4") && rfDataHexStr.Length >= 20)
                    {
                        break;
                    }
                }
                if (!rfDataHexStr.Contains("FE F4"))
                {
                    return "Meas Vrssix fail:" + rfDataHexStr;
                }
                x = rfDataHexStr.IndexOf("FE F4");
                string Vstr = rfDataHexStr.Substring(x + 6, 11);
                str1 = Vstr.Trim().Split(' ');
                byte[] b1 = new byte[str1.Length];
                for (int i = 0; i < str1.Length; i++)
                {
                    b1[i] = Convert.ToByte(str1[i], 16);
                }
                if (b1[3] != 0X3A) b1[3] = 0X3A;
                int Vint32 = b1[3] * 0x1000000 + b1[2] * 0x10000 + b1[1] * 0x100 + b1[0];
                double Vrssix = Math.Sqrt(ConverInt32TOfloat(Vint32));
                if (Kx != 0)
                {
                    Vrssix = Vrssix / (Kx * Kx);
                }
                VRSSIX = rfDataHexStr;

                //meas Vrssi-------y
                //切换继电器
                myN9320B.Danalyse_Trace_ClearWrite();
                LF_Y_ON();
                System.Threading.Thread.Sleep(300);
                for (int i = 0; i < 4; i++)
                {
                    LF_MOD_IN("F4 00 00 00 00 00");
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, "FF FE F4", 8);
                    if (rfDataHexStr.Contains("FF FE F4") && rfDataHexStr.Length >= 20)
                    {
                        break;
                    }
                }
                if (!rfDataHexStr.Contains("FE F4"))
                {
                    return "Meas Vrssix fail:" + rfDataHexStr;
                }
                x = rfDataHexStr.IndexOf("FE F4");
                Vstr = rfDataHexStr.Substring(x + 6, 11);
                str1 = Vstr.Trim().Split(' ');
                byte[] b2 = new byte[str1.Length];
                for (int i = 0; i < str1.Length; i++)
                {
                    b2[i] = Convert.ToByte(str1[i], 16);
                }
                if (b2[3] != 0X3A) b2[3] = 0X3A;
                Vint32 = b2[3] * 0x1000000 + b2[2] * 0x10000 + b2[1] * 0x100 + b2[0];
                double Vrssiy = Math.Sqrt(ConverInt32TOfloat(Vint32));
                if (Ky != 0)
                {
                    Vrssiy = Vrssiy / (Ky * Ky);
                }
                VRSSIY = rfDataHexStr;

                //meas Vrssi-------z
                //切换继电器
                myN9320B.Danalyse_Trace_ClearWrite();
                LF_Z_ON();
                System.Threading.Thread.Sleep(300);
                for (int i = 0; i < 4; i++)
                {
                    LF_MOD_IN("F4 00 00 00 00 00");
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, "FF FE F4", 8);
                    if (rfDataHexStr.Contains("FF FE F4") && rfDataHexStr.Length >= 20)
                    {
                        break;
                    }
                }
                if (!rfDataHexStr.Contains("FE F4"))
                {
                    return "Meas Vrssix fail:" + rfDataHexStr;
                }
                x = rfDataHexStr.IndexOf("FE F4");
                Vstr = rfDataHexStr.Substring(x + 6, 11);
                str1 = Vstr.Trim().Split(' ');
                byte[] b3 = new byte[str1.Length];
                for (int i = 0; i < str1.Length; i++)
                {
                    b3[i] = Convert.ToByte(str1[i], 16);
                }
                if (b3[3] != 0X3A) b3[3] = 0X3A;
                Vint32 = b3[3] * 0x1000000 + b3[2] * 0x10000 + b3[1] * 0x100 + b3[0];
                double Vrssiz = Math.Sqrt(ConverInt32TOfloat(Vint32));
                if (Ky != 0)
                {
                    Vrssiz = Vrssiz / (Kz * Kz);
                }
                VRSSIZ = rfDataHexStr;

                //Enter EEPROM Security Access
                byte[] data = new byte[] { 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }; //加密数据
                byte[] key = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, randomNumber[0], randomNumber[1], randomNumber[2] }; //密钥，random number: 1,2,3
                byte[] v = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //密钥向量
                data[0] = McodeSN[0];
                data[1] = McodeSN[1];
                data[2] = McodeSN[2];
                data[3] = McodeSN[3];
                byte[] aesData = aes1.AESEncrypt(data, v, key);  //caculate AES data
                string aesB1B3str = aesData[1].ToString("X2") + " " + aesData[3].ToString("X2");
                string randomSTR = randomNumber[0].ToString("X2") + ' ' + randomNumber[1].ToString("X2") + ' ' + randomNumber[2].ToString("X2") + ' ';
                string cal3Dstr = "F3 " + randomSTR + aesB1B3str;

                string saSuccessStr = "FF FE " + aesData[3].ToString("X2") + ' ' + aesData[1].ToString("X2");
                int j;
                for (j = 0; j < 4; j++)
                {
                    LF_MOD_IN(cal3Dstr);
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr, 9);

                    if (rfDataHexStr.Contains("FF FE") && !rfDataHexStr.Contains("FF FE 00 00"))
                    {
                        break;
                    }
                }
                if (j > 4)
                {
                    return "Enter EEPROM Security Access fail:" + rfDataHexStr + " must contain:" + saSuccessStr;
                }

                //Write new Kx, Ky, Kz
                //int k = 1;
                //myN9320B.SetMeasLength("FSK", 350);
                System.Threading.Thread.Sleep(500);
                double Vmin = Math.Min(Vrssix, Vrssiy);
                Vmin = Math.Min(Vmin, Vrssiz);
                Kx = (byte)(Math.Sqrt(Vmin / Vrssix) * k * 100);
                Ky = (byte)(Math.Sqrt(Vmin / Vrssiy) * k * 100);
                Kz = (byte)(Math.Sqrt(Vmin / Vrssiz) * k * 100);
                string KxyzStr = "F2 12 00 " + Kx.ToString("X2") + " " + Ky.ToString("X2") + " " + Kz.ToString("X2");

                for (int i = 0; i < 4; i++)
                {
                    LF_MOD_IN(KxyzStr);
                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr, 9);

                    if (rfDataHexStr.Contains("F2 12"))
                    {
                        break;
                    }
                }
                //LF_MOD_IN(KxyzStr);
                //System.Threading.Thread.Sleep(waitTimes_ms);
                //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
                if (!rfDataHexStr.Contains("F2 12"))
                {
                    return "Write new Kx, Ky, Kz fail:" + rfDataHexStr;
                }
                x = rfDataHexStr.IndexOf("F2 12");
                //string kxkykz = rfDataHexStr.Substring(x + 9, 8);
                return "Kx, Ky, Kz:" + rfDataHexStr;
            }
            catch (Exception ex)
            {
                return "try catch error " + rfDataHexStr + ":" + ex.Message;
            }
        }

        public void Calibration()
        {

            
            double K1 = 0;
            double K2 = 0;
            double K3 = 0;

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string Wrstring = "";        //标定写入字符串
            byte[] d1 = new byte[3] { 0, 0, 0 };  //Ch1
            byte[] d2 = new byte[3] { 0, 0, 0 };  //Ch2
            byte[] d3 = new byte[3] { 0, 0, 0 };  //Ch3

            double V1X = 0;
            double V2X = 0;
            double V3X = 0;
            double V1Y = 0;
            double V2Y = 0;
            double V3Y = 0;
            double V1Z = 0;
            double V2Z = 0;
            double V3Z = 0;

            double K = 6;
            double[] C = new double[3] { 0, 0, 0 };

            byte C1 = 0;
            byte C2 = 0;
            byte C3 = 0;



            /////////////////////////////////////////////////////////////////LF_X();    //X轴 获得Vin1(X) Vin2(X) Vin3(X) 
            this.LF_MOD_IN("29 FF 32");
            System.Threading.Thread.Sleep(2000);
            s1 = myN9320B.GetTreace("TRACE1");
            d1 = getdata1(s1);
            V1X = calVin(d1);


            d2 = getdata2(s1);
            V2X = calVin(d2);


            d3 = getdata3(s1);
            V3X = calVin(d3);

            Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
            V1X = Math.Pow(V1X, 2);
            V2X = Math.Pow(V2X, 2);
            V3X = Math.Pow(V3X, 2);

            ////////////////////////////////////////////LF_Y();   //Y轴 获得Vin1(Y) Vin2(Y) Vin3(Y)
            this.LF_MOD_IN("29 FF 32");
            System.Threading.Thread.Sleep(3000);
            s2 = myN9320B.GetTreace("TRACE1");
            d1 = getdata1(s2);
            V1Y = calVin(d1);


            d2 = getdata2(s2);
            V2Y = calVin(d2);


            d3 = getdata3(s2);
            V3Y = calVin(d3);

            Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
            V1Y = Math.Pow(V1Y, 2);
            V2Y = Math.Pow(V2Y, 2);
            V3Y = Math.Pow(V3Y, 2);



            //////////////////////////////////////////////////////////LF_Z();   //Z轴 获得Vin1(Z) Vin2(Z) Vin3(Z)
            this.LF_MOD_IN("29 FF 32");
            System.Threading.Thread.Sleep(3000);
            s3 = myN9320B.GetTreace("TRACE1");
            d1 = getdata1(s3);
            V1Z = calVin(d1);


            d2 = getdata2(s3);
            V2Z = calVin(d2);


            d3 = getdata3(s3);
            V3Z = calVin(d3);

            Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
            V1Z = Math.Pow(V1Z, 2);
            V2Z = Math.Pow(V2Z, 2);
            V3Z = Math.Pow(V3Z, 2);

            C = calCn(V1X, V2X, V3X, V1Y, V2Y, V3Y, V1Z, V2Z, V3Z, K, K, K);


            K1 = Math.Pow(C[0], 2) * V1X + Math.Pow(C[1], 2) * V2X + Math.Pow(C[2], 2) * V3X;
            K2 = Math.Pow(C[0], 2) * V1Y + Math.Pow(C[1], 2) * V2Y + Math.Pow(C[2], 2) * V3Y;
            K3 = Math.Pow(C[0], 2) * V1Z + Math.Pow(C[1], 2) * V2Z + Math.Pow(C[2], 2) * V3Z;
            Console.WriteLine("K1=" + K1.ToString() + " " + "K2=" + K2.ToString() + " " + "K3=" + K3.ToString());

            C1 = (byte)(C[0] * 100);
            C2 = (byte)(C[1] * 100);
            C3 = (byte)(C[2] * 100);



            //重新设置信号发送电压5V
            this.LF_MOD_Config(125000, 6, 100);
            this.LF_OUTPUT_ON();
            System.Threading.Thread.Sleep(500);

            //写入标定值
            Wrstring = "29 FF 31 20 00 03" + " " + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + Convert.ToString(C3, 16).PadLeft(2, '0') + " " + "FF";
            this.LF_MOD_IN(Wrstring);


            Console.WriteLine("C1=" + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + "C2=" + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + "C3=" + Convert.ToString(C3, 16).PadLeft(2, '0'));

        }

        public void Calibration1()
        {
            vs1x = 0;
            vs2x = 0;
            vs3x = 0;

            vs1y = 0;
            vs2y = 0;
            vs3y = 0;

            vs1z = 0;
            vs2z = 0;
            vs3z = 0;

            
            
            
            double K1 = 0;
            double K2 = 0;
            double K3 = 0;

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string Wrstring = "";        //标定写入字符串
            byte[] d1 = new byte[3] { 0, 0, 0 };  //Ch1
            byte[] d2 = new byte[3] { 0, 0, 0 };  //Ch2
            byte[] d3 = new byte[3] { 0, 0, 0 };  //Ch3

            double V1X = 0;
            double V2X = 0;
            double V3X = 0;
            double V1Y = 0;
            double V2Y = 0;
            double V3Y = 0;
            double V1Z = 0;
            double V2Z = 0;
            double V3Z = 0;

            double K = 6;
            double[] C = new double[3] { 0, 0, 0 };

            C1 = 0;
            C2 = 0;
            C3 = 0;



            /////////////////////////////////////////////////////////////////LF_X();    //X轴 获得Vin1(X) Vin2(X) Vin3(X) 
            CaliX(1.14);
            V1X = getvs1x();
            V2X = getvs2x();
            V3X = getvs3x();



            ////////////////////////////////////////////LF_Y();   //Y轴 获得Vin1(Y) Vin2(Y) Vin3(Y)

            CaliY(1.41);
            V1Y = getvs1y();
            V2Y = getvs2y();
            V3Y = getvs3y();



            //////////////////////////////////////////////////////////LF_Z();   //Z轴 获得Vin1(Z) Vin2(Z) Vin3(Z)
            CaliZ(1.25);
            V1Z = getvs1z();
            V2Z = getvs2z();
            V3Z = getvs3z();

            C = calCn(V1X, V2X, V3X, V1Y, V2Y, V3Y, V1Z, V2Z, V3Z, K, K, K);


            K1 = Math.Pow(C[0], 2) * V1X + Math.Pow(C[1], 2) * V2X + Math.Pow(C[2], 2) * V3X;
            K2 = Math.Pow(C[0], 2) * V1Y + Math.Pow(C[1], 2) * V2Y + Math.Pow(C[2], 2) * V3Y;
            K3 = Math.Pow(C[0], 2) * V1Z + Math.Pow(C[1], 2) * V2Z + Math.Pow(C[2], 2) * V3Z;
            Console.WriteLine("K1=" + K1.ToString() + " " + "K2=" + K2.ToString() + " " + "K3=" + K3.ToString());

            C1 = (byte)(C[0] * 100);
            C2 = (byte)(C[1] * 100);
            C3 = (byte)(C[2] * 100);

            Console.WriteLine("C1=" + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + "C2=" + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + "C3=" + Convert.ToString(C3, 16).PadLeft(2, '0'));

        }

        public void Calibration2()
        {
            vs1x = 0;
            vs2x = 0;
            vs3x = 0;

            vs1y = 0;
            vs2y = 0;
            vs3y = 0;

            vs1z = 0;
            vs2z = 0;
            vs3z = 0;




            double K1 = 0;
            double K2 = 0;
            double K3 = 0;

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string Wrstring = "";        //标定写入字符串
            byte[] d1 = new byte[3] { 0, 0, 0 };  //Ch1
            byte[] d2 = new byte[3] { 0, 0, 0 };  //Ch2
            byte[] d3 = new byte[3] { 0, 0, 0 };  //Ch3

            double V1X = 0;
            double V2X = 0;
            double V3X = 0;
            double V1Y = 0;
            double V2Y = 0;
            double V3Y = 0;
            double V1Z = 0;
            double V2Z = 0;
            double V3Z = 0;

            double K = 6;
            double[] C = new double[3] { 0, 0, 0 };

            C1 = 0;
            C2 = 0;
            C3 = 0;



            /////////////////////////////////////////////////////////////////LF_X();    //X轴 获得Vin1(X) Vin2(X) Vin3(X) 
            CaliX(1.56);
            V1X = getvs1x();
            V2X = getvs2x();
            V3X = getvs3x();



            ////////////////////////////////////////////LF_Y();   //Y轴 获得Vin1(Y) Vin2(Y) Vin3(Y)

            CaliY(1.76);
            V1Y = getvs1y();
            V2Y = getvs2y();
            V3Y = getvs3y();



            //////////////////////////////////////////////////////////LF_Z();   //Z轴 获得Vin1(Z) Vin2(Z) Vin3(Z)
            CaliZ(1.29);
            V1Z = getvs1z();
            V2Z = getvs2z();
            V3Z = getvs3z();

            C = calCn(V1X, V2X, V3X, V1Y, V2Y, V3Y, V1Z, V2Z, V3Z, K, K, K);


            K1 = Math.Pow(C[0], 2) * V1X + Math.Pow(C[1], 2) * V2X + Math.Pow(C[2], 2) * V3X;
            K2 = Math.Pow(C[0], 2) * V1Y + Math.Pow(C[1], 2) * V2Y + Math.Pow(C[2], 2) * V3Y;
            K3 = Math.Pow(C[0], 2) * V1Z + Math.Pow(C[1], 2) * V2Z + Math.Pow(C[2], 2) * V3Z;
            Console.WriteLine("K1=" + K1.ToString() + " " + "K2=" + K2.ToString() + " " + "K3=" + K3.ToString());

            C1 = (byte)(C[0] * 100);
            C2 = (byte)(C[1] * 100);
            C3 = (byte)(C[2] * 100);

            Console.WriteLine("C1=" + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + "C2=" + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + "C3=" + Convert.ToString(C3, 16).PadLeft(2, '0'));

        }


        public void CalibrationX()
        {

            FRR1 = 0;
            string s1 = "";

            double V1X = 0;
            double V2X = 0;
            double V3X = 0;

            int i = 0;

            /////////////////////////////////////////////////////////////////LF_X();    //X轴 获得Vin1(X) Vin2(X) Vin3(X) 
            for (i = 0; i < 3; i++)
            {
                FRR1 = 0;
                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s1 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s1))
                {
                    V1X = calVin(getdata1(s1));
                    V2X = calVin(getdata2(s1));
                    V3X = calVin(getdata3(s1));
                    //if (V1X > 0 && V1X < 1 && V2X > 2.1 && V2X < 3.3 && V3X > 0 && V3X < 1)
                    //{
                        Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
                        vs1x = Math.Pow(V1X, 2);
                        vs2x = Math.Pow(V2X, 2);
                        vs3x = Math.Pow(V3X, 2);
                        FRR1 = 0;
                        break;
                    //}
                    //else
                    //{
                    //    Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
                    //    vs1x = Math.Pow(V1X, 2);
                    //    vs2x = Math.Pow(V2X, 2);
                    //    vs3x = Math.Pow(V3X, 2);
                    //    FRR1 = 1;
 
                    //}
                }
                else
                {
                    Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
                    vs1x = Math.Pow(V1X, 2);
                    vs2x = Math.Pow(V2X, 2);
                    vs3x = Math.Pow(V3X, 2);
                    FRR1 = 1;

                }
            }


        }

        public void CaliX(double V)
        {

            string s1 = "";

            double V1X = 0;
            double V2X = 0;
            double V3X = 0;

            int i = 0;
            //MdlClass.readplc.LF_X_ON();
            //LF_MOD_Config(1.14);
            mybox.SetX();
            LF_MOD_Config(V);
            System.Threading.Thread.Sleep(500);
            /////////////////////////////////////////////////////////////////LF_X();    //X轴 获得Vin1(X) Vin2(X) Vin3(X) 
            for (i = 0; i < 3; i++)
            {

                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s1 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s1))
                {
                    V1X = calVin(getdata1(s1));
                    V2X = calVin(getdata2(s1));
                    V3X = calVin(getdata3(s1));

                    Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
                    vs1x = Math.Pow(V1X, 2);
                    vs2x = Math.Pow(V2X, 2);
                    vs3x = Math.Pow(V3X, 2);
      
                    break;

                }
                else
                {
                    Console.WriteLine("V1X=" + Convert.ToString(V1X) + " " + "V2X=" + Convert.ToString(V2X) + " " + "V3X=" + Convert.ToString(V3X));
                    vs1x = Math.Pow(V1X, 2);
                    vs2x = Math.Pow(V2X, 2);
                    vs3x = Math.Pow(V3X, 2);
                }
            }


        }

        public double getvs1x()
        {
            return vs1x;
        }

        public double getvs2x()
        {
            return vs2x;
        }

        public double getvs3x()
        {
            return vs3x;
        }

        public double getvs1y()
        {
            return vs1y;
        }

        public double getvs2y()
        {
            return vs2y;
        }

        public double getvs3y()
        {
            return vs3y;
        }

        public double getvs1z()
        {
            return vs1z;
        }

        public double getvs2z()
        {
            return vs2z;
        }

        public double getvs3z()
        {
            return vs3z;
        }

        public void CalibrationY()
        {

            FRR2 = 0;
            string s2 = "";

            int i = 0;
            double V1Y = 0;
            double V2Y = 0;
            double V3Y = 0;


            for (i = 0; i < 3; i++)
            {
                FRR2 = 0;
                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s2 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s2))
                {
                    V1Y = calVin(getdata1(s2));
                    V2Y = calVin(getdata2(s2));
                    V3Y = calVin(getdata3(s2));
                    //if (V1Y > 0 && V1Y < 1 && V2Y > 0 && V2Y < 1 && V3Y > 1.8 && V3Y < 3.1)
                    //{
                        Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
                        vs1y = Math.Pow(V1Y, 2);
                        vs2y = Math.Pow(V2Y, 2);
                        vs3y = Math.Pow(V3Y, 2);
                        FRR2 = 0;
                        break;
                    //}
                    //else
                    //{
                    //    Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
                    //    vs1y = Math.Pow(V1Y, 2);
                    //    vs2y = Math.Pow(V2Y, 2);
                    //    vs3y = Math.Pow(V3Y, 2);
                    //    FRR2 = 1;

                    //}
                }
                else
                {
                    Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
                    vs1y = Math.Pow(V1Y, 2);
                    vs2y = Math.Pow(V2Y, 2);
                    vs3y = Math.Pow(V3Y, 2);
                    FRR2 = 1;
                }
            }
           
        }

        public void CaliY(double V)
        {

            string s2 = "";

            int i = 0;
            double V1Y = 0;
            double V2Y = 0;
            double V3Y = 0;

            //MdlClass.readplc.LF_Y_ON();
            //LF_MOD_Config(1.41);
            mybox.SetY();
            LF_MOD_Config(V);
            System.Threading.Thread.Sleep(500);
            for (i = 0; i < 3; i++)
            {
                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s2 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s2))
                {
                    V1Y = calVin(getdata1(s2));
                    V2Y = calVin(getdata2(s2));
                    V3Y = calVin(getdata3(s2));
                    Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
                    vs1y = Math.Pow(V1Y, 2);
                    vs2y = Math.Pow(V2Y, 2);
                    vs3y = Math.Pow(V3Y, 2);
   
                    break;
 
                }
                else
                {
                    Console.WriteLine("V1Y=" + Convert.ToString(V1Y) + " " + "V2Y=" + Convert.ToString(V2Y) + " " + "V3Y=" + Convert.ToString(V3Y));
                    vs1y = Math.Pow(V1Y, 2);
                    vs2y = Math.Pow(V2Y, 2);
                    vs3y = Math.Pow(V3Y, 2);
                }
            }

        }

        public void CalibrationZ()
        {



            FRR3 = 0;
            string s3 = "";
            int i = 0;

            double V1Z = 0;
            double V2Z = 0;
            double V3Z = 0;

          
            for (i = 0; i < 3; i++)
            {
                FRR3 = 0;
                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s3 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s3))
                {
                    V1Z = calVin(getdata1(s3));
                    V2Z = calVin(getdata2(s3));
                    V3Z = calVin(getdata3(s3));
                    //if (V1Z > 1.5 && V1Z < 2.9 && V2Z > 0 && V2Z < 1 && V3Z > 0 && V3Z < 1)
                    //{
                        Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
                        vs1z = Math.Pow(V1Z, 2);
                        vs2z = Math.Pow(V2Z, 2);
                        vs3z = Math.Pow(V3Z, 2);
                        FRR3 =0;
                        break;
                    //}
                    //else
                    //{
                    //    Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
                    //    vs1z = Math.Pow(V1Z, 2);
                    //    vs2z = Math.Pow(V2Z, 2);
                    //    vs3z = Math.Pow(V3Z, 2);    
                    //    FRR3 = 1;
                    //}
                }
                else
                {
                    Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
                    vs1z = Math.Pow(V1Z, 2);
                    vs2z = Math.Pow(V2Z, 2);
                    vs3z = Math.Pow(V3Z, 2);
                    FRR3 = 1;

                }
            }

         
        }

        public void CaliZ(double V)
        {


            string s3 = "";
            int i = 0;

            double V1Z = 0;
            double V2Z = 0;
            double V3Z = 0;

            //MdlClass.readplc.LF_Z_ON();
           // LF_MOD_Config(1.25);
            mybox.SetZ();
            LF_MOD_Config(V);
            System.Threading.Thread.Sleep(500);
            for (i = 0; i < 3; i++)
            {
                this.LF_MOD_IN("29 FF 32");
                System.Threading.Thread.Sleep(1000);
                s3 = myN9320B.GetTreace("TRACE1");
                if (CheckLength(s3))
                {
                    V1Z = calVin(getdata1(s3));
                    V2Z = calVin(getdata2(s3));
                    V3Z = calVin(getdata3(s3));
                    Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
                    vs1z = Math.Pow(V1Z, 2);
                    vs2z = Math.Pow(V2Z, 2);
                    vs3z = Math.Pow(V3Z, 2);
                 
                    break;
                }
                else
                {
                    Console.WriteLine("V1Z=" + Convert.ToString(V1Z) + " " + "V2Z=" + Convert.ToString(V2Z) + " " + "V3Z=" + Convert.ToString(V3Z));
                    vs1z = Math.Pow(V1Z, 2);
                    vs2z = Math.Pow(V2Z, 2);
                    vs3z = Math.Pow(V3Z, 2);

                }
            }


        }
        public int GetFR1()
        {
            return FRR1;
        }

        public int GetFR2()
        {
            return FRR2;
        }

        public int GetFR3()
        {
            return FRR3;
        }

        public int GetCalR()
        {
            return CalR;
        }
        public void Calresl(double V1X, double V2X, double V3X, double V1Y, double V2Y, double V3Y, double V1Z, double V2Z, double V3Z,int FR1,int FR2,int FR3)
        {
            CalR = 1;
            C1 = 0;
            C2 = 0;
            C3 = 0;
            
            if ((FR1 == 1) | (FR2 == 1) | (FR3 == 1))
            {
                CalFR = 1;
            }
            else
            {
                double K = 6;
 
                double K1 = 0;
                double K2 = 0;
                double K3 = 0;
                double[] C = new double[3] { 0, 0, 0 };



                C = calCn(V1X, V2X, V3X, V1Y, V2Y, V3Y, V1Z, V2Z, V3Z, K, K, K);


                //if (CalR == 0)//标定系数在范围内才写入标定值，否则为初始值
                //{
                    K1 = Math.Pow(C[0], 2) * V1X + Math.Pow(C[1], 2) * V2X + Math.Pow(C[2], 2) * V3X;
                    K2 = Math.Pow(C[0], 2) * V1Y + Math.Pow(C[1], 2) * V2Y + Math.Pow(C[2], 2) * V3Y;
                    K3 = Math.Pow(C[0], 2) * V1Z + Math.Pow(C[1], 2) * V2Z + Math.Pow(C[2], 2) * V3Z;
                    Console.WriteLine("K1=" + K1.ToString() + " " + "K2=" + K2.ToString() + " " + "K3=" + K3.ToString());

                    C1 = (byte)(C[0] * 100);
                    C2 = (byte)(C[1] * 100);
                    C3 = (byte)(C[2] * 100);
 
                //}
 
            }
 
        }

        public byte GetC1()
        {
            return C1;
        }

        public byte GetC2()
        {
            return C2;
        }
        public byte GetC3()
        {
            return C3;
        }

        public void Writeres(byte C1,byte C2,byte C3)
        {

            //重新设置信号发送电压5V
            this.LF_MOD_Config(5);
            System.Threading.Thread.Sleep(500);


            string Wrstring = "";


                    //写入标定值
                    Wrstring = "29 FF 31 20 00 03" + " " + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + Convert.ToString(C3, 16).PadLeft(2, '0') + " " + "FF";
                    this.LF_MOD_IN(Wrstring);


                    Console.WriteLine("C1=" + Convert.ToString(C1, 16).PadLeft(2, '0') + " " + "C2=" + Convert.ToString(C2, 16).PadLeft(2, '0') + " " + "C3=" + Convert.ToString(C3, 16).PadLeft(2, '0'));



 
        }
        public int GetCalFR()
        {
            return CalFR;
        }
        public bool CheckLength(string input)
        {
            string w = input;
            string u, v, m = "";
            string k = "";

            int j = w.IndexOf("111101010101101010");
            //Console.WriteLine(j);
            for (int i = j + 34; i < w.Length - 1; i = i + 2)
            {
                u = w.Substring(i, 2);
                if (u == "01")
                    k = k + "0";
                else if (u == "10")
                    k = k + "1";
                else
                    break;

            }
            for (int l = 0; l < k.Length - 7; l = l + 8)       //CMD1 RFRSP           
            {
                v = k.Substring(l, 8);
                m = m + Convert.ToByte(v, 2).ToString("X2") + ' ';
            }
            if (m.Length < 28)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public byte[] getdata1(string input)      //得到测量RSSI时RF中的Ch1[2:0]  Ch1[2]=q[0] Ch1[1]=q[1]  Ch1[0]=q[2]
        {

            byte[] q = new byte[3];

            string w = input;
            string u, v, m = "";
            string k = "";

            int j = w.IndexOf("111101010101101010");
            //Console.WriteLine(j);
            for (int i = j + 34; i < w.Length - 1; i = i + 2)
            {
                u = w.Substring(i, 2);
                if (u == "01")
                    k = k + "0";
                else if (u == "10")
                    k = k + "1";
                else
                    break;

            }
            for (int l = 0; l < k.Length - 7; l = l + 8)       //CMD1 RFRSP           
            {
                v = k.Substring(l, 8);
                m = m + Convert.ToByte(v, 2).ToString("X2") + ' ';
            }
            Console.Write(m);
            if (m.Substring(0, 2) == "00")
            {
                for (int countn = 0; countn < 3; countn++)       //3表示截取字符串中byte的个数
                {
                    int add = (countn + 1) * 3;                  //(count+1)*3表示取得的三个byte起始位置3，6，9
                    string label = m.Substring(add, 2);          //(count+4)*3表示取得的三个byte起始位置12，15，18
                    q[countn] = Convert.ToByte(label, 16);       //(count+7)*3表示取得的三个byte起始位置21，24，27

                }
            }
            return q;
        }

        public byte[] getdata2(string input)                   //得到测量RSSI时RF中的Ch2[2:0]  Ch2[2]=q[0] Ch2[1]=q[1]  Ch2[0]=q[2]
        {
            byte[] q = new byte[3];

            string w = input;
            string u, v, m = "";
            string k = "";

            int j = w.IndexOf("111101010101101010");
            //Console.WriteLine(j);
            for (int i = j + 34; i < w.Length - 1; i = i + 2)
            {
                u = w.Substring(i, 2);
                if (u == "01")
                    k = k + "0";
                else if (u == "10")
                    k = k + "1";
                else
                    break;

            }
            for (int l = 0; l < k.Length - 7; l = l + 8)       //CMD1 RFRSP           
            {
                v = k.Substring(l, 8);
                m = m + Convert.ToByte(v, 2).ToString("X2") + ' ';
            }
            Console.Write(m);
            if (m.Substring(0, 2) == "00")
            {
                for (int countn = 0; countn < 3; countn++)       //3表示截取字符串中byte的个数
                {
                    int add = (countn + 4) * 3;                  //(count+1)*3表示取得的三个byte起始位置3，6，9
                    string label = m.Substring(add, 2);          //(count+4)*3表示取得的三个byte起始位置12，15，18
                    q[countn] = Convert.ToByte(label, 16);       //(count+7)*3表示取得的三个byte起始位置21，24，27

                }
            }
            return q;
        }

        public byte[] getdata3(string input)                      //得到测量RSSI时RF中的Ch3[2:0]  Ch3[2]=q[0] Ch3[1]=q[1]  Ch3[0]=q[2]
        {
            byte[] q = new byte[3];

            string w = input;
            string u, v, m = "";
            string k = "";

            int j = w.IndexOf("111101010101101010");
            //Console.WriteLine(j);
            for (int i = j + 34; i < w.Length - 1; i = i + 2)
            {
                u = w.Substring(i, 2);
                if (u == "01")
                    k = k + "0";
                else if (u == "10")
                    k = k + "1";
                else
                    break;

            }
            for (int l = 0; l < k.Length - 7; l = l + 8)       //CMD1 RFRSP           
            {
                v = k.Substring(l, 8);
                m = m + Convert.ToByte(v, 2).ToString("X2") + ' ';
            }
            Console.Write(m);
            if (m.Substring(0, 2) == "00")
            {
                for (int countn = 0; countn < 3; countn++)       //3表示截取字符串中byte的个数
                {
                    int add = (countn + 7) * 3;                  //(count+1)*3表示取得的三个byte起始位置3，6，9
                    string label = m.Substring(add, 2);          //(count+4)*3表示取得的三个byte起始位置12，15，18
                    q[countn] = Convert.ToByte(label, 16);       //(count+7)*3表示取得的三个byte起始位置21，24，27

                }
            }
            return q;
        }

        public double calVin(byte[] Ch)                         //通过Ch[2:0]计算Vin值
        {
            int e = 0;
            double Val = 0;
            string m = "";
            int s = 1;
            for (int j = 1; j >= 0; j--)
            {

                m = m + Convert.ToString(Ch[j], 2).PadLeft(8, '0');

            }
            if (m.Substring(0, 1) == "1")
            {
                s = -1;
            }
            for (int w1 = 1; w1 < 16; w1++)
            {
                if (m.Substring(w1, 1) == "1")
                {
                    Val = Val + Math.Pow(2, (-w1 - 1));

                }
            }
            if (Ch[2] >= 0x80)
            {
                e = -1 * (0x100 - Ch[2]);
            }
            else
            {
                e = Ch[2];
            }

            Val = Val + 0.5;
            Val = s * Val;
            Val = Val * Math.Pow(2, e);
            return Val;

        }

        public double[] calCn(double a1, double a2, double a3, double b1, double b2, double b3, double e1, double e2, double e3, double k1, double k2, double k3)  //计算标定系数  a1为Vin1(X)的平方,a2为Vin2(X)的平方,a3为Vin3(X)的平方    
        {                                                                                                                                                                    //b1为Vin1(Y)的平方,b2为Vin2(Y)的平方,b3为Vin3(Y)的平方


            //e1为Vin1(Z)的平方,e2为Vin2(Z)的平方,e3为Vin3(Z)的平方
            double D = 0;
            double D1 = 0;
            double D2 = 0;
            double D3 = 0;

            double[] C = { 0, 0, 0 };
            try
            {
                //C[0]表示C1,C[1]表示C2,C[2]表示C3
                D = a1 * b2 * e3 + a2 * b3 * e1 + a3 * b1 * e2 - a3 * b2 * e1 - b3 * e2 * a1 - e3 * a2 * b1;
                D1 = k1 * b2 * e3 + a2 * b3 * k3 + a3 * k2 * e2 - a3 * b2 * k3 - b3 * e2 * k1 - e3 * a2 * k2;
                D2 = a1 * k2 * e3 + k1 * b3 * e1 + a3 * b1 * k3 - a3 * k2 * e1 - b3 * k3 * a1 - e3 * k1 * b1;
                D3 = a1 * b2 * k3 + a2 * k2 * e1 + k1 * b1 * e2 - k1 * b2 * e1 - k2 * e2 * a1 - k3 * a2 * b1;
                C[0] = Math.Sqrt(D1 / D);
                C[1] = Math.Sqrt(D2 / D);
                C[2] = Math.Sqrt(D3 / D);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            if ((C[0] >= 0.9) && (C[0] <= 1.5) && (C[1] >= 0.6) && (C[1] <= 1.1) && (C[2] >= 0.8) && (C[2] <= 1.3))
            {
                CalR = 0;
                return C;
            }
            else
            {
                Console.WriteLine("计算系数不在给定范围内!");
                CalR = 1;
                return C;
            }
        }

        public string Return_VRSSIX()
        {
            return VRSSIX;
        }

        public string Return_VRSSIY()
        {
            return VRSSIY;
        }

        public string Return_VRSSIZ()
        {
            return VRSSIZ;
        }

        public double Meas_VRSSIX()
        {
            string rfDataHexStr = "";
            int waitTimes_ms = 2000;
            string waitStr = "FF FE";
            byte[] randomNumber = new byte[3] { 0, 0, 0 };
            int x;
            string[] str1;

            //LF_MOD_Config(0.61); //1k-0.61V vpp
            LF_MOD_Config(5.88); //10k-5.88V vpp
            System.Threading.Thread.Sleep(1000);

            LF_X_ON();
            System.Threading.Thread.Sleep(300);
            LF_MOD_IN("F4 00 00 00 00 00");
            System.Threading.Thread.Sleep(waitTimes_ms);
            rfDataHexStr = "x" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
            if (!rfDataHexStr.Contains("FE F4"))
            {
                return -1;
            }
            x = rfDataHexStr.IndexOf("FE F4");
            string Vstr = rfDataHexStr.Substring(x + 6, 11);
            str1 = Vstr.Trim().Split(' ');
            byte[] b1 = new byte[str1.Length];
            for (int i = 0; i < str1.Length; i++)
            {
                b1[i] = Convert.ToByte(str1[i], 16);
            }
            int Vint32 = b1[3] * 0x1000000 + b1[2] * 0x10000 + b1[1] * 0x100 + b1[0];
            double Vrssix = Math.Sqrt(ConverInt32TOfloat(Vint32));
            VRSSIX = rfDataHexStr;

            return Vrssix;
        }

        public double Meas_VRSSIY()
        {
            string rfDataHexStr = "";
            int waitTimes_ms = 1000;
            string waitStr = "FF FE";
            byte[] randomNumber = new byte[3] { 0, 0, 0 };
            int x;
            string[] str1;

            //LF_MOD_Config(0.61); //1k-0.61V vpp
            LF_MOD_Config(5.88); //10k-5.88V vpp
            System.Threading.Thread.Sleep(1000);

            LF_Y_ON();
            System.Threading.Thread.Sleep(300);
            LF_MOD_IN("F4 00 00 00 00 00");
            System.Threading.Thread.Sleep(waitTimes_ms);
            rfDataHexStr = "x" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
            if (!rfDataHexStr.Contains("FE F4"))
            {
                return -1;
            }
            x = rfDataHexStr.IndexOf("FE F4");
            string Vstr = rfDataHexStr.Substring(x + 6, 11);
            str1 = Vstr.Trim().Split(' ');
            byte[] b1 = new byte[str1.Length];
            for (int i = 0; i < str1.Length; i++)
            {
                b1[i] = Convert.ToByte(str1[i], 16);
            }
            int Vint32 = b1[3] * 0x1000000 + b1[2] * 0x10000 + b1[1] * 0x100 + b1[0];
            double Vrssix = Math.Sqrt(ConverInt32TOfloat(Vint32));
            VRSSIX = rfDataHexStr;

            return Vrssix;
        }

        public double Meas_VRSSIZ()
        {
            string rfDataHexStr = "";
            int waitTimes_ms = 1000;
            string waitStr = "FF FE";
            byte[] randomNumber = new byte[3] { 0, 0, 0 };
            int x;
            string[] str1;

            //LF_MOD_Config(0.61); //1k-0.61V vpp
            LF_MOD_Config(5.88); //10k-5.88V vpp
            System.Threading.Thread.Sleep(1000);

            LF_Z_ON();
            System.Threading.Thread.Sleep(300);
            LF_MOD_IN("F4 00 00 00 00 00");
            System.Threading.Thread.Sleep(waitTimes_ms);
            rfDataHexStr = "x" + myN9320B.GetAnalyzeDataHexStr(true, waitStr);
            if (!rfDataHexStr.Contains("FE F4"))
            {
                return -1;
            }
            x = rfDataHexStr.IndexOf("FE F4");
            string Vstr = rfDataHexStr.Substring(x + 6, 11);
            str1 = Vstr.Trim().Split(' ');
            byte[] b1 = new byte[str1.Length];
            for (int i = 0; i < str1.Length; i++)
            {
                b1[i] = Convert.ToByte(str1[i], 16);
            }
            int Vint32 = b1[3] * 0x1000000 + b1[2] * 0x10000 + b1[1] * 0x100 + b1[0];
            double Vrssix = Math.Sqrt(ConverInt32TOfloat(Vint32));
            VRSSIX = rfDataHexStr;

            return Vrssix;
        }

        #endregion

        #region 灵敏度测试 1k-0.05-0.02  10k-0.23-0.20

        string sendSTR = "F1 10 00 00 00 00";
        string compareStr = "FE F1 10";

        /// <summary>
        /// 设置灵敏度测试需要发送的LF信息
        /// </summary>
        /// <param name="sdSTR">发送信息</param>
        /// <param name="compSTR">比较RF信息</param>
        public void Set_SensitivityTest_Str(string sdSTR, string compSTR)
        {
            sendSTR = sdSTR;
            compareStr = compSTR;
        }

        public double LF_Sensitivity_Test(double start_V_max, double end_V_min, double step_V, int repeat_Times)
        {
            string s = "";
            string rfDataHexStr = "";
            int waitTimes_ms = 1100;
            string waitStr = "00";
            double now_VPP = start_V_max;

            double sensitivity_V = start_V_max;

            //read manuCode and SN
            

            do
            {

                LF_MOD_Config(now_VPP);
                sensitivity_V = now_VPP;
                int i = 0;
                for (i = 0; i < repeat_Times; i++)
                {
                    //先清空N9320B的解调数据
                    myN9320B.Danalyse_Trace_ClearWrite("FSK");                   
                    LF_MOD_IN("29 FF 33");
                    //System.Threading.Thread.Sleep(100);
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(waitTimes_ms));
                    //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
                    s = myN9320B.GetTreace("TRACE1");
                    rfDataHexStr = converttobyte(s);
                    if (rfDataHexStr.Contains("00 83"))
                    {
                        break;
                    }
                }
                if (i == repeat_Times)
                {
                    //在这个功率点上反复测试repeat_Times，依然没有过，则这一点是灵敏度点。
                    Console.WriteLine("sensitivity_V="+sensitivity_V+" "+"receive message="+rfDataHexStr);
                    return sensitivity_V;
                }
                now_VPP = now_VPP - step_V;
            } while (now_VPP >= end_V_min);

            //end_V_min设置的不够小
            return sensitivity_V;
        }

        public double LF_Sensitivity_Test1(double start_V_min, double end_V_max, double step_V, int repeat_Times)
        {
            string s = "";
            string rfDataHexStr = "";
            int waitTimes_ms = 1100;
            string waitStr = "00";
            double now_VPP = start_V_min;

            double sensitivity_V = start_V_min;

            //read manuCode and SN


            do
            {

                LF_MOD_Config(now_VPP);
                sensitivity_V = now_VPP;
                int i = 0;
                for (i = 0; i < repeat_Times; i++)
                {
                    //先清空N9320B的解调数据
                    myN9320B.Danalyse_Trace_ClearWrite("FSK");
                    LF_MOD_IN("29 FF 33");
                    //System.Threading.Thread.Sleep(100);
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(waitTimes_ms));
                    //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
                    s = myN9320B.GetTreace("TRACE1");
                    rfDataHexStr = converttobyte(s);
                    if (rfDataHexStr.Contains("00 83"))
                    {
                        break;
                    }
                }
                if (i == repeat_Times)
                {
                    //在这个功率点上反复测试repeat_Times，依然没有过，则这一点是灵敏度点。
                    Console.WriteLine("sensitivity_V=" + sensitivity_V + " " + "receive message=" + rfDataHexStr);
                    return sensitivity_V;
                }
                now_VPP = now_VPP + step_V;
            } while (now_VPP <= end_V_max);

            //end_V_min设置的不够小
            return sensitivity_V;
        }

        public double LF_Sensitivity_TestS(double V_min, double V_max, double V_middle,double step_V, int repeat_Times)
        {
            string s = "";
            string rfDataHexStr = "";
            int waitTimes_ms = 1100;
            string waitStr = "00";
            double now_VPP = V_middle;
            int m = 0;

            double sensitivity_V = V_middle;

            //read manuCode and SN


            LF_MOD_Config(now_VPP);
            int i = 0;
            for (i = 0; i < repeat_Times; i++)
            {
                //先清空N9320B的解调数据
                myN9320B.Danalyse_Trace_ClearWrite("FSK");
                LF_MOD_IN("29 FF 33");
               //System.Threading.Thread.Sleep(100)
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(waitTimes_ms));
                //rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);
                s = myN9320B.GetTreace("TRACE1");
                rfDataHexStr = converttobyte(s);
                if (rfDataHexStr.Contains("00 83"))
                {
                    m = 1;
                    break;
                }
             }
            if (m == 1)
            {
                sensitivity_V = LF_Sensitivity_Test(V_middle - step_V, V_min, step_V, repeat_Times);
                return sensitivity_V + step_V;

            }
            else
            {
                sensitivity_V = LF_Sensitivity_Test1(V_middle + step_V, V_max, step_V, repeat_Times);
                return sensitivity_V - step_V;
            }
        }


        public string LF_Sensitivity_TestSS(double VPP)
        {
            string s = "";
            string rfDataHexStr = "";
            int waitTimes_ms = 1100;
            string waitStr = "00";
         
            int m = 0;

            //read manuCode and SN


            LF_MOD_Config(VPP);
            myN9320B.Danalyse_Trace_ClearWrite("FSK");
            LF_MOD_IN("29 FF 33");
            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(1100));
            s = myN9320B.GetTreace("TRACE1");
           rfDataHexStr = converttobyte(s);
           return rfDataHexStr;
        }

        public string converttobyte(string inputs)
        {
            string s = "";
            int l = 0;
            string w = inputs;
            string u, v, m = "";
            string k = "";
            int j = w.IndexOf("111101010101101010");
            for (int i = j + 34; i < w.Length - 1; i = i + 2)
            {

                u = w.Substring(i, 2);
                if (u == "01")
                {
                    k = k + "0";

                }
                else if (u == "10")
                {
                    k = k + "1";

                }
                else
                    break;
            }

            for (l = 0; l < k.Length - 7; l = l + 8) //CMD1 RFRSP           
            {
                v = k.Substring(l, 8);
                m = m + Convert.ToByte(v, 2).ToString("X2") + ' ';
            }
            s += m + "\r";
            return s;
        }
        
        #endregion

        
        
        
        #region 灵敏度测试--new
        List<string> sendLFstrList = new List<string>();
        List<string> compStrList = new List<string>();


        /// <summary>
        /// 往LIST里面添加灵敏度测试需要发送的LF信息，需要在初始化里面调用至少两遍。如果在测试大循环里，测试后处理中需要清空LIST。
        /// </summary>
        /// <param name="sdSTR">发送信息</param>
        /// <param name="compSTR">比较RF信息</param>
        public void Add_SensitivityTest_Str(string sdSTR, string compSTR)
        {
            sendLFstrList.Add(sdSTR);
            compStrList.Add(compSTR);
        }


        public void Clear_SensitivityTest_Str()
        {
            sendLFstrList.Clear();
            compStrList.Clear();
        }

        public double LF_Sensitivity_Test_New(double start_V_max, double end_V_min, double step_V, int repeat_Times)
        {
            string rfDataHexStr = "";
            int waitTimes_ms = 2000;
            string waitStr = "FF FE";
            double now_VPP = start_V_max;

            double sensitivity_V = start_V_max;
            int rp_t = repeat_Times;

            int count = sendLFstrList.Count;
            string[] lfStr = new string[count];
            string[] compStr = new string[count];
            int m = 0;
            for (int n = 0; n < sendLFstrList.Count; n++)
            {
                lfStr[n] = sendLFstrList[n];
                compStr[n] = compStrList[n];
            }

            //read manuCode and SN
            do
            {
                System.Threading.Thread.Sleep(500);
                if (now_VPP == start_V_max)
                {
                    rp_t = 3;
                }
                else
                {
                    rp_t = repeat_Times;
                }

                LF_MOD_Config(now_VPP);
                sensitivity_V = now_VPP;
                sendSTR = lfStr[m % count];
                compareStr = compStr[m % count];

                int i = 0;
                for (i = 0; i < rp_t; i++)
                {
                    LF_MOD_IN(sendSTR);

                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);

                    if (rfDataHexStr.Contains(compareStr))
                    {
                        break;
                    }
                }
                m++;
                if (i == rp_t)
                {
                    //在这个功率点上反复测试repeat_Times，依然没有过，则这一点是灵敏度点。
                    return sensitivity_V;
                }
                now_VPP = now_VPP - step_V;
            } while (now_VPP >= end_V_min);

            //end_V_min设置的不够小
            return sensitivity_V;
        }

        public double LF_Sensitivity_Test_New(double start_V, double end_V_max, double end_V_min, double step_V, int repeat_Times)
        {
            string rfDataHexStr = "";
            int waitTimes_ms = 2000;
            string waitStr = "FF FE";
            double now_VPP = start_V;
            double step = step_V;
            double sensitivity_V = start_V;
            int rp_t = repeat_Times;

            int count = sendLFstrList.Count;
            string[] lfStr = new string[count];
            string[] compStr = new string[count];
            int m = 0;
            int i = 0;
            for (int n = 0; n < sendLFstrList.Count; n++)
            {
                lfStr[n] = sendLFstrList[n];
                compStr[n] = compStrList[n];
            }


            LF_MOD_Config(now_VPP);
            sensitivity_V = now_VPP;
            sendSTR = lfStr[m % count];
            compareStr = compStr[m % count];
            for (i = 0; i < 3; i++)
            {
                LF_MOD_IN(sendSTR);

                System.Threading.Thread.Sleep(waitTimes_ms);
                rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);

                if (rfDataHexStr.Contains(compareStr))
                {
                    break;
                }
            }
            if (i == 3)
            {
                step = -Math.Abs(step_V);//up
            }
            else
            {
                step = Math.Abs(step_V);//down
            }
            m++;

            //read manuCode and SN
            do
            {
                System.Threading.Thread.Sleep(500);
                LF_MOD_Config(now_VPP);
                sensitivity_V = now_VPP;
                sendSTR = lfStr[m % count];
                compareStr = compStr[m % count];

                for (i = 0; i < rp_t; i++)
                {
                    LF_MOD_IN(sendSTR);

                    System.Threading.Thread.Sleep(waitTimes_ms);
                    rfDataHexStr = myN9320B.GetAnalyzeDataHexStr(true, waitStr);

                    if (rfDataHexStr.Contains(compareStr))
                    {
                        break;
                    }
                }
                m++;
                if (i == rp_t)
                {
                    //在这个功率点上反复测试repeat_Times，依然没有过，则这一点是灵敏度点。
                    return sensitivity_V;
                }


                now_VPP = now_VPP - step;
                if (now_VPP > end_V_max || now_VPP < end_V_min)
                {
                    break;
                }

            } while (true);

            //end_V_min设置的不够小
            return sensitivity_V;
        }
        #endregion

        public void initBox(uint port, int baud)
        {
            mybox.Initialize(port, baud);
        }

       
        public void BoxPreStrat()
        {
            mybox.CloseRelay(0x00, 0x04);
        }

        public void SetBoxXAxis()
        {
            mybox.CloseRelay(0x00, 0x00);
        }

        public void ReSetBoxXAxis()
        {
            mybox.OpenRelay(0x00, 0x00);
        }

        public void SetBoxYAxis()
        {
            mybox.CloseRelay(0x00, 0x01);
        }

        public void ReSetBoxYAxis()
        {
            mybox.OpenRelay(0x00, 0x01);
        }

        public void SetBoxZAxis()
        {
            mybox.CloseRelay(0x00, 0x02);
        }

        public void ReSetBoxZAxis()
        {
            mybox.OpenRelay(0x00, 0x02);
        }

        public void ReSetAllBox()
        {
            mybox.ResetAllRelayCard();
        }

        public void Set5V()
        {
            myDAQ.DO_Write(5);
        }

        public void Set0V()
        {
            myDAQ.DO_Write(0);
        }

    }
}
