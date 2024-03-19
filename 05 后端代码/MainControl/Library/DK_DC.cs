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
    public class DK_DC
    {
        TcpClient PLCclient;
        NetworkStream PLCstream;

        Stopwatch sp = new Stopwatch();
        public readonly object Lockobj = new object();
        Thread thKeepACQ = null;
        bool flageRead = false;
       public long RollingNum = 0;
        public    DCdataClass CurrentDataClass = new DCdataClass();
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
            if (SendAck())
            {
                if (thKeepACQ == null || !thKeepACQ.IsAlive)
                {
                    flageRead = true;
                    thKeepACQ = new Thread(KeepRead);
                    thKeepACQ.Start();
                }
            }
        }

        public void StopACQ()
        {
            if (thKeepACQ != null&& thKeepACQ.IsAlive)
            {
                flageRead = false;
                thKeepACQ.Join(1000);
                if (thKeepACQ.IsAlive)
                {
                    thKeepACQ.Abort();
                }
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

                    if (Hearttimer.ElapsedMilliseconds > 15000)
                    {
                        SendHeart();
                        Hearttimer.Restart();
                    }
                    //从总线拿数据找02包头
                    if (PLCstream.DataAvailable)
                    {

                        int t = PLCstream.ReadByte();
                        if (t == 02)
                        {
                           // Console.WriteLine("从总线找到包头02.");
                            int Length = 0;
                            sp.Restart();
                            do
                            {
                                System.Threading.Thread.Sleep(2);
                                if (PLCclient.Available >= 4)
                                {

                                    //找到头，然后从流里面取5个数据
                                    byte[] Lengthbytr = new byte[4];
                                    PLCstream.Read(Lengthbytr, 0, Lengthbytr.Length);
                                    Lengthbytr = Lengthbytr.Reverse().ToArray();
                                    Length = BitConverter.ToInt32(Lengthbytr, 0);
                                    break;
                                }
                                if (sp.ElapsedMilliseconds > 500)
                                {
                                    throw new Exception("等待数据长度超时！");
                                }
                            } while (flageRead);
                            if (Length > 0)
                            {
                                byte[] data = new byte[Length + 1];
                                do
                                {
                                    System.Threading.Thread.Sleep(2);
                                    if (PLCclient.Available >= Length + 1)
                                    {

                                        PLCstream.Read(data, 0, data.Length);
                                        if (data[Length] != 0x03)
                                        {
                                            throw new Exception("数据结尾符不等于03！");
                                        }
                                        AnPacketData(data);
                                        break;
                                    }
                                    if (sp.ElapsedMilliseconds > 500)
                                    {
                                        throw new Exception("等待数据本体超时！");
                                    }
                                } while (flageRead);
                            }
                        }
                    }


                } while (flageRead);
            }catch(Exception ex)
            {
                Console.WriteLine();
            }
        }


        public void AnPacketData(byte[] data)
        {
            if (data[0] == 0x54)
            {
                if (data[1] == 0x30 && data[2] == 0x32 && data[3] == 0x30 && data[4] == 0x32)
                {
                    RollingNum++;
            
                    try
                    {
                        string str = Encoding.ASCII.GetString(data);
                        str = str.Remove(0, 5).TrimEnd();
                        string[] strlst = str.Split(';');
                        //for (int i = 0; i < strlst.Length-1; i++)
                        //{
                        //    Console.WriteLine("拧紧枪数据包"+i+":" + strlst[i]);
                        //}
                        Console.WriteLine("拧紧枪数据包:" + strlst[0]);






                        str = strlst[0].Remove(0, 4);
                        string[] strlst1 = str.Split(',');
                       // Console.WriteLine("最终扭矩:" + strlst1[5]);
                        CurrentDataClass.EndTorque = float.Parse(strlst1[5]);
                        //float a = float.Parse(strlst1[5]);
                        //byte[] b = BitConverter.GetBytes(a);
                        //float c = BitConverter.ToSingle(b,0);
                      //  Console.WriteLine("最终角度:" + strlst1[6]);
                        CurrentDataClass.EndAngle = float.Parse(strlst1[6]);
                       // Console.WriteLine("监控角度:" + strlst1[7]);
                        CurrentDataClass.MonitorAngle = float.Parse(strlst1[7]);
                       // Console.WriteLine("最终拧紧时长:" + strlst1[8]);
                        CurrentDataClass.EndSpantime = float.Parse(strlst1[8]);

                       // Console.WriteLine("最终拧紧结果:" + strlst1[9]);
                        CurrentDataClass.Result = byte.Parse(strlst1[9]);

                       // Console.WriteLine("NG代码:" + strlst1[10]);
                        CurrentDataClass.NGCODE = byte.Parse(strlst1[10]);
                        CurrentDataClass .IsErro = true;
                        CurrentDataClass.PLCResult = 255;
                        //byte[] bbbbb=   CurrentDataClass.ClassByteArr();
                        //CurrentDataClass.LoadByte(bbbbb);
                    }
                    catch
                    {
                        CurrentDataClass.IsErro = false;
                    }
                }
                if (data[1] == 0x30 && data[2] == 0x30 && data[3] == 0x30 && data[4] == 0x33)
                {
                    string str = Encoding.ASCII.GetString(data);
                    str = str.Remove(0, 5);
                    Console.WriteLine("心跳数据包:"+ str);
                }
            }
        }



        public void SendHeart()
        {
            lock (Lockobj)
            {
                byte[] data = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x0b, 0x57, 0x30, 0x30, 0x30, 0x33, 0x30, 0x30, 0x31, 0x3D, 0x030, 0x3B, 0x03 };
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

        public bool SendAck()
        {
            try
            {
                lock (Lockobj)
                {

                    byte[] data = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x05, 0x52, 0x30, 0x32, 0x30, 0x32, 0x03 };
                    PLCstream.Write(data, 0, data.Length);
                    // 02 00 00 00 08 41 30 32 30 32 41 43 4B 03 
                    sp.Restart();
                    do
                    {
                        System.Threading.Thread.Sleep(5);
                        if (PLCclient.Available >= 14)
                        {
                            break;
                        }
                        if (sp.ElapsedMilliseconds > 500)
                        {
                            throw new Exception("拧紧枪心跳回复超时");
                        }
                    } while (true);
                    byte[] recdata = new byte[PLCclient.Available];
                    PLCstream.Read(recdata, 0, recdata.Length);
                    if (recdata[recdata.Length - 1] == 0x03)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch
            {
                return false;
            }
        }

      
       
       

      

    }

    public class DCdataClass
    {

        public bool IsErro = false;
    

        public float EndTorque =0;//最终扭矩4
        public float EndAngle =0;//最终角度8
        public float MonitorAngle = 0;//监控角度12
        public float EndSpantime = 0;//  最终拧紧时长  16
        public byte Result = 0;//最终拧紧结果17
        public byte NGCODE = 0;//NG代码18
        public byte PLCResult = 0;//2.OK.3.NG
        public byte[] ClassByteArr()
        {
            List<byte> bytelst = new List<byte>();
            byte[] EndTorque1 = BitConverter.GetBytes(EndTorque);
            
            byte[] EndAngle1 = BitConverter.GetBytes(EndAngle);

            byte[] MonitorAngle1 = BitConverter.GetBytes(MonitorAngle);
            byte[] EndSpantime1 = BitConverter.GetBytes(EndSpantime);
            bytelst.AddRange(EndTorque1);
            bytelst.AddRange(EndAngle1);
            bytelst.AddRange(MonitorAngle1);
            bytelst.AddRange(EndSpantime1);
            bytelst.Add(Result);
            bytelst.Add(NGCODE);
            bytelst.Add(PLCResult);
            return bytelst.ToArray();
            
        }
        public long SendRollingNum = 0;

        public void LoadByte(byte[] data)
        {


            byte[] b = new byte[4];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = data[i];
            }
            EndTorque = BitConverter.ToSingle(b,0);


            b = new byte[4];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = data[4+i];
            }
            EndAngle = BitConverter.ToSingle(b, 0);

             b = new byte[4];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = data[8+i];
            }
            MonitorAngle = BitConverter.ToSingle(b, 0);

            b = new byte[4];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = data[12+i];
            }
            EndSpantime = BitConverter.ToSingle(b, 0);

            Result = data[16];

            NGCODE = data[17];
            PLCResult = data[18];
            //public float EndTorque =0;//最终扭矩4
            //public float EndAngle =0;//最终角度8
            //public float MonitorAngle = 0;//监控角度12
            //public float EndSpantime = 0;//  最终拧紧时长  16
            Console.WriteLine("==============================================" );
            Console.WriteLine("最终扭矩:" + EndTorque);
            Console.WriteLine("最终角度:" + EndAngle);
            Console.WriteLine("监控角度:" + MonitorAngle);
            Console.WriteLine("最终拧紧时长:" + EndSpantime);
            Console.WriteLine("最终拧紧结果:" + Result);
            Console.WriteLine("NG代码:" + NGCODE);
            Console.WriteLine("PLCRes:" + PLCResult);
            Console.WriteLine("==============================================");
        }     
    }
}
