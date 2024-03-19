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
    //气密性检测仪
    public class LT_Link_Protocol80
    {
        TcpClient PLCclient;
        NetworkStream PLCstream;

        Stopwatch sp = new Stopwatch();
        public readonly object Lockobj = new object();
        Thread thKeepACQ = null;
        bool flageRead = false;
        public long RollingNum = 0;
        public long SendRollingNum = 0;
        public string CurrenData = "";
        /// <summary>
        /// 关闭TCP通讯
        /// </summary>
        public void CloseTCP()
        {
            PLCclient.Close();
        }

        public void Init_TCP(int PORT, string IPnum)
        {
            //Int32 port = 9000;
            IPAddress Addr = IPAddress.Parse(IPnum);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(Addr, PORT);

            //TcpClient client = new TcpClient(ipLocalEndPoint);
            PLCclient = new TcpClient(IPnum, PORT);
            PLCclient.ReceiveTimeout = 1000;
            PLCclient.SendTimeout = 1000;
            PLCstream = PLCclient.GetStream();
            RollingNum = 0;
        }



        public void StartACQ()
        {

            if (thKeepACQ == null || !thKeepACQ.IsAlive)
            {
                flageRead = true;
                thKeepACQ = new Thread(KeepRead);
                thKeepACQ.Start();
            }

        }

        public void StopACQ()
        {
            if (thKeepACQ != null && thKeepACQ.IsAlive)
            {
                flageRead = false;
                thKeepACQ.Join(1000);
                if (thKeepACQ.IsAlive)
                {
                    thKeepACQ.Abort();
                }
            }
        }

        public void SendHeart()
        {
            lock (Lockobj)
            {
                byte[] data = new byte[] { 0xAA, 0x00, 0x0B, 0x01, 0x00, 0x4E, 0x00, 0x31, 0x32, 0x33, 0x34, 0x12, 0x23, 0x55 };
            
                PLCstream.Write(data, 0, data.Length);
                //sp.Restart();
                //do
                //{
                //    System.Threading.Thread.Sleep(5);
                //    if (PLCclient.Available >= 17)
                //    {
                //        break;
                //    }
                //    if (sp.ElapsedMilliseconds > 500)
                //    {
                //        throw new Exception("拧紧枪心跳回复超时");
                //    }
                //} while (true);
                //byte[] recdata = new byte[PLCclient.Available];
                //PLCstream.Read(recdata, 0, data.Length);
            }
        }


        public void KeepRead()
        {
            Stopwatch Hearttimer = new Stopwatch();
            Hearttimer.Start();
            SendHeart();
            try
            {
                do
                {
                    System.Threading.Thread.Sleep(2);

                    if (Hearttimer.ElapsedMilliseconds > 5000)
                    {
                        SendHeart();
                        Hearttimer.Restart();
                    }
                    //从总线拿数据找02包头
                    if (PLCstream.DataAvailable)
                    {

                        int t = PLCstream.ReadByte();//54 65 73 74 52 65 73 75 6C 74
                        if (t == 0x54)
                        {
                            // Console.WriteLine("从总线找到包头02.");
                          
                            sp.Restart();
                            do
                            {
                                System.Threading.Thread.Sleep(2);
                                if (PLCclient.Available >= 11)
                                {

                                    //找到头，然后从流里面取5个数据
                                    byte[] Lengthbytr = new byte[11];//TestResult: P1 07-28-2023 22:17:20 Lower_Min_P VDPD C=1619
                                    PLCstream.Read(Lengthbytr, 0, Lengthbytr.Length);
                                    string Head = Encoding.ASCII.GetString(Lengthbytr);
                                    if (Head == "estResult: ")
                                    {
                                        Console.WriteLine("找到头~~~");
                                        List<byte> blst = new List<byte>();
                                        Stopwatch sp1 = new Stopwatch();
                                        sp1.Restart();
                                        do
                                        {
                                            if (PLCclient.Available > 0)
                                            {
                                                blst.Add((byte)PLCstream.ReadByte());
                                            }
                                            if (blst[blst.Count - 1] == 0x0A && blst[blst.Count - 2] == 0x0D)
                                            {
                                                string s = Encoding.ASCII.GetString(blst.ToArray());
                                                Console.WriteLine("收到最新数据:" + s);
                                                string[] slst = s.Split(' ');
                                                slst[slst.Length - 1] = slst[slst.Length-1].TrimEnd().Remove(0, 2);
                                                RollingNum = long.Parse(slst[slst.Length - 1]);
                                                s = "";
                                                for (int i = 3; i < slst.Length - 1; i++)
                                                {
                                                    s += slst[i] + ",";
                                                }
                                                CurrenData = s.Remove(s.Length-1)+'\r'+'\n';
                                                break;
                                            }
                                            if (sp1.ElapsedMilliseconds > 1000)
                                            {
                                                throw new Exception("等待数据长度超时！");
                                            }
                                        } while (true);
                                    }

                                    break;
                                }
                                if (sp.ElapsedMilliseconds > 500)
                                {
                                    throw new Exception("等待数据长度超时！");
                                }
                            } while (flageRead);

                        }
                    }
                } while (flageRead);
            }
            catch (Exception ex)
            {
            }
        }


    }

}
