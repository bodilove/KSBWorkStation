using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Library
{
    public class SccanerHoneyWell
    {

        SerialPort _MyPort = null;
        SerialDataReceivedEventHandler myfuc = null;

        public SerialPort MyPort
        {
            get { return _MyPort; }
        }



        public void Open(string name, int baud, Parity parity, int databit, StopBits stopbits,SerialDataReceivedEventHandler fuc)
        {
            myfuc = fuc;
            _MyPort = new SerialPort();
            _MyPort.PortName = name;
            _MyPort.BaudRate = baud;
            _MyPort.Parity = parity;
            _MyPort.DataBits = databit;
            _MyPort.StopBits = stopbits;
            if (_MyPort.IsOpen)
            {
                _MyPort.Close();
            }
            else
            {


                _MyPort.DataReceived += myfuc;
                _MyPort.Open();
            }
        }



        public void Close()
        {
        
            if (_MyPort != null)
            {
                if (myfuc != null)
                {
                    _MyPort.DataReceived -= myfuc;
                }
                if (_MyPort.IsOpen)
                {
                    _MyPort.Close();
                }
            }
         
        }



        
        

    }
}
