using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace Library
{
    class ZlanIO
    {
        SerialPort _MyPort = new SerialPort();
        Thread LedRead;
        
        private void ConfigCom(string name, int baud, Parity parity, int databit, StopBits stopbits)
        {
            _MyPort = new SerialPort();
            _MyPort.PortName = name;
            _MyPort.BaudRate = baud;
            _MyPort.Parity = parity;
            _MyPort.DataBits = databit;
            _MyPort.StopBits = stopbits;
        }

        public void Quit()
        {
            _MyPort.Close();
        }

        private Boolean open_Port()
        {

            if (_MyPort.IsOpen) _MyPort.Close();
            if (!Exists(_MyPort.PortName))
                return false;
            else
                _MyPort.Open();
            return true;
        }

        private void ClearPort()
        {
            //_MyPort.DiscardOutBuffer();
            _MyPort.DiscardInBuffer();
        }
        private bool Exists(string port_name)
        {
            foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
            return false;
        }

        private string[] AutoUpload()
        {
            //byte[] d1 = new byte[] {0x03 ,0x01,0x00,0x01,0xff,0x00 };
            //d1 = crc16(d1);
            //_MyPort.Write(d1, 0, d1.Length);
            string ss;
            byte sum=0;
            int num = _MyPort.BytesToRead;
            byte[] bb=new byte[num+1];
            _MyPort.Read(bb,0,num);
             ss = Convert.ToString(bb);
             string s = BitConverter.ToString(bb);
             Console.WriteLine(s);
             string[] temp = s.Split('-');
            //string s = string.Join(" ", bb);
            //string[] sss = new string[10];
            ////sss= s.Split();
            //Console.WriteLine(s);
            //foreach (string ee in temp)
            //{
            //    Console.WriteLine(ee);
            //}
            return temp;
        }

        private byte[] ReadIO(byte StartByte, byte ReadNum)
        {
            byte[] d1 = new byte[] { 0x01, 0x01, 0x00, StartByte, 0x00, ReadNum };
            d1 = crc16(d1);
            _MyPort.Write(d1, 0, d1.Length);
            string ss;
            byte sum = 0;
            int num = _MyPort.BytesToRead;
            byte[] bb = new byte[num + 1];
            //System.Threading.Thread.Sleep(1000);
            _MyPort.Read(bb, 0, num);
            ss = Convert.ToString(bb);
            string s = BitConverter.ToString(bb);
            string[] temp = s.Split('-');
            Console.WriteLine(s);
            return bb;
        }

        private bool ReadIOAuto()
        {
            //byte[] d1 = new byte[] { 0x01, 0x01, 0x00, 0X10, 0x00, 0X01 };
            //d1 = crc16(d1);
            //_MyPort.Write(d1, 0, d1.Length);
            string ss;
            byte sum = 0;
            int num = _MyPort.BytesToRead;
            byte[] bb = new byte[num + 1];
            System.Threading.Thread.Sleep(1000);
            _MyPort.Read(bb, 0, num);
            if (bb.Length > 2)
            {
                ss = Convert.ToString(bb);
                string s = BitConverter.ToString(bb);
                string[] temp = s.Split('-');
                Console.WriteLine(s);
                if (bb[0] == 0x06 && bb[1] == 0x05 & (bb[4] == 0xff || bb[12] == 0xff))
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

        private byte[] crc16(byte[] Data)
        {
            //Convert.ToInt32("FF", 16);
            int[] data = new int[Data.Length];
            for (int x = 0; x < Data.Length; x++)
            {
                data[x] = Convert.ToInt32(Data[x]);
            }

            int[] temdata = new int[data.Length + 2];
            int xda, xdapoly;
            int i, j, xdabit;
            xda = 0xFFFF;
            xdapoly = 0xA001;
            for (i = 0; i < data.Length; i++)
            {
                xda ^= data[i];
                for (j = 0; j < 8; j++)
                {
                    xdabit = (int)(xda & 0x01);
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }
            Array.Copy(data, 0, temdata, 0, data.Length);
            temdata[temdata.Length - 2] = (int)(xda & 0xFF);
            temdata[temdata.Length - 1] = (int)(xda >> 8);
            byte[] reDate = new byte[temdata.Length];
            for (int x = 0; x < temdata.Length; x++)
            {
                reDate[x] = Convert.ToByte(temdata[x]);
            }
            return reDate;
        }

        private void  SetZlanD0(byte address ,bool value)
        {
            byte Func = 0x05;
            byte[] d1= new byte[6] ;
            if(value)
            {
                 d1 = new byte[]{ 0x01, Func, 0x00, address, 0xff, 0x00 };
                
            }
            else
            {
                d1 = new byte[] { 0x01, Func, 0x00, address, 0x00, 0x00 };
            }
            d1 = crc16(d1);
            _MyPort.Write(d1, 0, d1.Length);
        }

        public void Init(string Com)
        {
            this.ConfigCom(Com, 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            this.open_Port();
        }

        public void ResetZlanAll()
        {

            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x10, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x11, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x12, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x13, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x14, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x15, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x16, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0x17, false);
        }

        public void Start2Link()
        {
            this.SetZlanD0(0X10, true);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X11, true);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X12, true);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X13, true);
        }

        public void Stop2Link()
        {
            this.SetZlanD0(0X10, false);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X11, false);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X12, false);
            System.Threading.Thread.Sleep(100);
            this.SetZlanD0(0X13, false);
        }

        public void StartMeasCurr()
        {
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0X16, true);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0X17, true);
            System.Threading.Thread.Sleep(200);
        }

        public void StopMeaCurr()
        {

            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0X16, false);
            System.Threading.Thread.Sleep(200);
            this.SetZlanD0(0X17, false);
            System.Threading.Thread.Sleep(200);
        }
    }
}
