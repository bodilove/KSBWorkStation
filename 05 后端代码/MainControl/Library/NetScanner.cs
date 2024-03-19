using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
namespace Library
{
    public class NetScanner
    {
        TcpClient client;
        NetworkStream stream;

        Stopwatch sp = new Stopwatch();
        public readonly object Lockobj = new object();
        Thread thKeepACQ = null;
        bool flageRead = false;
        public long RollingNum = 0;
        public DCdataClass CurrentDataClass = new DCdataClass();
        public bool BreakScan = false;
        public bool Erro = false;
        /// <summary>
        /// 关闭TCP通讯
        /// </summary>
        public void CloseTCP()
        {
            client.Close();
        }

        public void Init_TCP(int PORT, string IPnum)
        {
            //Int32 port = 9000;
            IPAddress Addr = IPAddress.Parse(IPnum);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(Addr, PORT);

            //TcpClient client = new TcpClient(ipLocalEndPoint);
            client = new TcpClient(IPnum, PORT);
            client.ReceiveTimeout = 1000;
            client.SendTimeout = 1000;
            stream = client.GetStream();
            RollingNum = 0;
        }//192.168.0.65 ：502



        public string StartScan()
        {
            Erro = false;
            BreakScan = true;
            byte[] data = new byte[] { 0x16, 0x54, 0x0D };
            List<byte> getdata =new List<byte>();
            string s="";
            if (client.Available > 0)
            {
                byte[] buffer1 = new byte[client.Available];
                stream.Read(buffer1, 0, buffer1.Length);
                buffer1 = null;
            }
            Stopwatch sp = new Stopwatch();
            do
            {
                stream.Write(data, 0, data.Length);
                getdata.Clear();
                sp.Restart();
                do
                {
                    if (client.Available > 0)
                    {
                        int size = client.Available;
                        byte[] buffer = new byte[size];
                        stream.Read(buffer, 0, size);
                        getdata.AddRange(buffer);
                    }
                    if (getdata.Count>0&&  getdata[getdata.Count - 1] == 0x0D)
                    {
                        s = ASCIIEncoding.ASCII.GetString(getdata.ToArray());
                        BreakScan = false;
                        break;
                    }
                    if (sp.ElapsedMilliseconds > 30000)
                    {
                        break;
                    }
                } while (BreakScan);
            } while (BreakScan);
            return s;
        }
    }
}
