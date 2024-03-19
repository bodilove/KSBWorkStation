using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Xml;

namespace Library
{
    public class RFA_RF_test
    {
        static int rf_bitRate = 9600;
        //static int rf_bitRate = 2000;

        private List<byte> dataArray8bits = new List<byte>();           //byte data
        private List<byte> dataBitNum = new List<byte>();               //bit of every data
        private List<byte> manchesterDataArray = new List<byte>();      //manchester Data
        private List<double> AO_DataArray = new List<double>();         //DAQ AO data

        static double RKE_LEVEL0 = -1;  //AO口输出0基带信号电平为-1V
        static double RKE_LEVEL1 = 1;   //AO口输出1基带信号电平为1V

        UInt32 SI_RTN = 1;
        string lastReceive = "";
        string RF_Frame = "22 4F 59";
        string WaitStr = "62 4F 59";
        UInt32 SendID = 0x765;
        UInt32 ReadID = 0x705;
        public int rfOffset = 0;


        Library.AG_N9310A my9310;
        //Library.PLIN_USB myLIN;
        Library.NI_DAQ myDAQ = new Library.NI_DAQ();

        #region 初始化，配置，退出
        public void Initialize(object one9310, object oneLIN,string daq_name)
        {
            my9310 = one9310 as Library.AG_N9310A;
            //myLIN = oneLIN as Library.PLIN_USB;
            try
            {
                myDAQ.AO_Create_Task2(daq_name);
                //myDAQ.AO_Task2_Config(rf_bitRate * 2, manchesterBitNum + 1);
            }
            catch(Exception ex)
            {
                throw new Exception("NI DAQ初始化失败：" + ex.Message);
            }
        }

        public void ConfigRF(string FM_AM)
        {
            my9310.Output_Off();
            my9310.CarrieWave_SetAmplitude(-20);
            my9310.CarrieWave_SetFrequency(433.92);
            if (FM_AM == "FM" || FM_AM == "fm")
            {
                my9310.CarrieWave_SetFMdeviation(100);  //100KHz
                my9310.FM_External_Set();
            }
            else if (FM_AM == "AM" || FM_AM == "am")
            {
                my9310.CarrieWave_SetAMdepth(100);       //100%
                my9310.AM_External_Set();
            }
            //my9310.Output_On();
        }


        private double fmDeviation = 100;   //100KHz
        private int amDepth = 100;       //100%
        public string ConfigRF(string FM_AM, double center_Fre, int dbm)
        {
            my9310.Output_Off();
            my9310.CarrieWave_SetAmplitude(dbm);
            my9310.CarrieWave_SetFrequency(center_Fre);
            if (FM_AM == "FM" || FM_AM == "fm")
            {
                my9310.CarrieWave_SetFMdeviation(fmDeviation);
                my9310.FM_External_Set();
            }
            else if (FM_AM == "AM" || FM_AM == "am")
            {
                my9310.CarrieWave_SetAMdepth(amDepth);       
                my9310.AM_External_Set();
            }
            my9310.SendCommand(":FREQuency:CW?");//:AM:STATe?
            System.Threading.Thread.Sleep(500);
            string rtn = my9310.ReadReceive();
            return rtn;
            //my9310.Output_On();
        }

        public void Quit()
        {
            myDAQ.AO_Dispose_Task2();
        }
        #endregion

        #region caculate REK date

        private void CreateByteArray(string dataHex)
        {
            int i = 0;
            string[] splitsendstring = dataHex.Trim().Split(' '); //用空格隔开转化为数组
            int datalen = splitsendstring.Length;

            dataBitNum.Clear();
            dataArray8bits.Clear();

            for (i = 0; i < 54; i++)
            {
                dataBitNum.Add(8);
                dataArray8bits.Add(0xff);
            }

            dataBitNum.Add(8);
            dataArray8bits.Add(0xfe);

           
            for (i = 0; i < datalen; i++)
            {
                dataBitNum.Add(8);
                dataArray8bits.Add(Convert.ToByte(splitsendstring[i], 16));
            }

            //dataBitNum.Add(8);
            //dataArray8bits.Add(CaculateCRC(dataArray8bits, 60, 20));

        }

        private void CreateManchesterBitArray(string dataHex)
        {
            byte bitNum;
            byte bitData;
            ushort i, j,p;

            manchesterDataArray.Clear();

            CreateByteArray(dataHex);

            i = 0;
            foreach (byte num in dataArray8bits)
            {
                bitNum = dataBitNum[i];

                for (p = bitNum; p > 0; p--)      //先高后低
                {
                    j = (ushort)(8 - p);
                    bitData = (byte)((byte)(dataArray8bits[i] << j) >> 7);
                    if (bitData == 0)
                    {
                        manchesterDataArray.Add(0);
                        manchesterDataArray.Add(1);
                    }
                    else if (bitData == 1)
                    {
                        manchesterDataArray.Add(1);
                        manchesterDataArray.Add(0);
                    }
                }
                i++;
            }

            //反编译，查看数据
            //string[] VIEW = new string[50];

            //int l = 0;
            //for (int k = 0; k < dataBitNum.Count; k++)
            //{
            //    VIEW[k] = "";
            //    for (byte kk = 0; kk < dataBitNum[k]; kk++)
            //    {
            //        VIEW[k] += manchesterDataArray[l].ToString() + manchesterDataArray[l + 1].ToString() + " ";
            //        l += 2;
            //    }
            //    Console.WriteLine(VIEW[k]);
            //}
        }

        private void CreateAOvoltArray(string dataHex)
        {
            AO_DataArray.Clear();

            CreateManchesterBitArray(dataHex);

            foreach (uint x in manchesterDataArray)
            {
                if (x == 0)
                {
                    AO_DataArray.Add(RKE_LEVEL0); 
                }
                else if (x == 1)
                {
                    AO_DataArray.Add(RKE_LEVEL1);
                }
            }
            AO_DataArray.Add(0);
        }

        //带进位加法，计算CRC校验
        private byte CaculateCRC(List<byte> data, ushort startIndex, ushort lenth)
        {
            byte crc = 0x00;
            int i = 0;
            Int16 temp;
            Int16 array;
            byte[] d = data.ToArray();

            for (i = startIndex; i < (startIndex + lenth); i++)
            {
                temp = (Int16)(crc + d[i]);
                array = (Int16)((temp >> 8) & 0x01);
                crc = (byte)(temp % 0x100 + array);
            }
            crc = (byte)(~crc);

            return crc;
        }

        //将byte数据变化为曼彻斯特编码后存入array数组中，存储的起始位置为startIndex
        private ushort ChangeByteToManchester(byte data, ushort startIndex, byte bits, byte[] array)
        {
            byte bitData;
            ushort i, j;
            for (i = bits; i > 0; i--)      //先高后低
            {
                j = (ushort)(8 - i);
                bitData = (byte)((byte)(data << j) >> 7);
                if (bitData == 0)
                {
                    array[startIndex++] = 0;
                    array[startIndex++] = 1;
                }
                else if (bitData == 1)
                {
                    array[startIndex++] = 1;
                    array[startIndex++] = 0;
                }
                //if (bitData == 0)
                //{
                //    array[startIndex++] = 1;
                //    array[startIndex++] = 0;
                //}
                //else if (bitData == 1)
                //{
                //    array[startIndex++] = 0;
                //    array[startIndex++] = 1;
                //}
            }
            return startIndex;
        }

        #endregion

        #region send RF data
        public void RF_MOD_Config(double carrierFreq, int carrierDbm, int FSK_Deviation)
        {

            fmDeviation = FSK_Deviation;         //30KHz

            ConfigRF("FM", carrierFreq, carrierDbm);

            RKE_LEVEL0 = -1; //AO口输出0基带信号电平为-1V
            RKE_LEVEL1 = 1;  //AO口输出1基带信号电平为1V
        }

        public void RF_OUTPUT_ON()
        {
            my9310.Output_On();
        }

        public void RF_OUTPUT_OFF()
        {
            my9310.Output_Off();
        }

        public void RF_MOD_IN(string sendDataHex)
        {
            //产生基带信号
            CreateAOvoltArray(sendDataHex);
            myDAQ.AO_Task2_Config(rf_bitRate * 2, AO_DataArray.Count);

            double[] AOarray = AO_DataArray.ToArray();
            myDAQ.AO_Task2_Write_Data2Buffer(AOarray);
            RF_OUTPUT_ON();
            System.Threading.Thread.Sleep(100);
            myDAQ.AO_Start_Task2();
            System.Threading.Thread.Sleep(500);
            RF_OUTPUT_OFF();
            myDAQ.AO_Stop_Task2();
        }
        #endregion

    }
}
