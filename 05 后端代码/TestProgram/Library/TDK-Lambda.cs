using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace Library
{
    class TKD_Lambda
    {
        SerialPort myPort;

        public TKD_Lambda()
        {
            myPort = new SerialPort();
        }

        public void Initialize(uint comNum, int baudRate,uint deviceADDR)
        {
            if (!myPort.IsOpen)
            {
                myPort.PortName = "com" + comNum;  //端口名
                myPort.BaudRate = baudRate;         //速率
                myPort.DataBits = 8;                //数据位
                myPort.StopBits = StopBits.One;     //停止位
                myPort.Parity = Parity.None;        //奇偶校验位
                myPort.RtsEnable = true;            //打开RTS
                myPort.ReadTimeout = 50000;////////////////////
                myPort.NewLine = "\r";

                myPort.Open();
            }
            Thread.Sleep(500);
            myPort.Write("ADR " + deviceADDR + "\r");
            string ret = myPort.ReadLine();
            if (ret == "OK")
            {
                myPort.Write("IDN?\r");
                Thread.Sleep(200);
                ret = myPort.ReadLine();
                if (!ret.Contains("GEN"))
                {
                    throw new Exception("TDK power initialize fail!");
                }
            }
            else
            {
                throw new Exception("Com"+  comNum + " is not open!");
            }

            myPort.Write("RMT 1\r");
            ret = myPort.ReadLine();
            myPort.Write("OVP 30\r");
            ret = myPort.ReadLine();
        }

        private void CloseCom()
        {
            myPort.Close();
        }

        public void Set_Voltage(double volt)
        {
            myPort.Write("PV " + volt + "\r");
            string ret = myPort.ReadLine();
        }

        public void Set_Current(double curr)
        {
            myPort.Write("PC " + curr + "\r");
            string ret = myPort.ReadLine();
        }

        public string Output_ON()
        {
            myPort.Write("OUT 1\r");
            string ret = myPort.ReadLine();
            return ret;
        }

        public string Output_OFF()
        {
            myPort.Write("OUT 0\r");
            string ret = myPort.ReadLine();
            return ret;
        }

        public double Measure_Voltage()
        {
            double volt;
            myPort.Write("MV?\r");
            string volt_str = myPort.ReadLine();
            volt = Convert.ToDouble(volt_str);
            return volt;
        }

        public double Measure_Current()
        {
            double curr;
            myPort.Write("MC?\r");
            string curr_str = myPort.ReadLine();
            curr = Convert.ToDouble(curr_str);
            return curr;
        }

        public void Reset()
        {
            try
            {
                myPort.Write("RST\r");  //reset device
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            try
            {
                myPort.Write("CLS\r");  //clear device registers
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Quit()
        {
            Clear();
            Output_OFF();
            CloseCom();
        }

        public void SendCommand(string command)
        {
            myPort.Write(command);
        }

        public string SendReceiveCommand(string command)
        {
            try
            {
                string strResults;
                myPort.Write(command);
                strResults = myPort.ReadLine();
                return strResults;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
