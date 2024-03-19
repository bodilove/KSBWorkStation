using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace Common
{
    public class Modbus
    {
        SerialPort _MyPort = null;

        public SerialPort MyPort
        {
            get { return _MyPort; }
        }

        /// <summary>
        ///功能类型枚举
        /// </summary>
        public enum FuntionType : byte
        {
            /// <summary>
            /// 01读取线圈状态
            /// </summary>
            Read_Coil_State = 01,
            /// <summary>
            ///02读取输入状态
            /// </summary>
            Read_Input_State = 02,
            /// <summary>
            ///03读保持寄存器
            /// </summary>
            Read_Register = 03,
            /// <summary>
            ///04输入寄存器
            /// </summary>
            Input_Register = 04,
            /// <summary>
            ///05设置单个继电器状态
            /// </summary>
            Set_Single_Relay = 05,
            /// <summary>
            ///06设置单个保持寄存器
            /// </summary>
            Set_Single_Register = 06,
            /// <summary>
            ///15设置多个继电器状态
            /// </summary>
            Set_Multi_Relay = 15,
            /// <summary>
            ///16设置多个保持寄存器
            /// </summary>
            Set_Multi_Register = 16
        }


        public void ConfigCom(string name, int baud, Parity parity, int databit, StopBits stopbits)
        {
            _MyPort = new SerialPort();
            _MyPort.PortName = name;
            _MyPort.BaudRate = baud;
            _MyPort.Parity = parity;
            _MyPort.DataBits = databit;
            _MyPort.StopBits = stopbits;
        }

        public void Initialize(SerialPort Port)
        {
            _MyPort = Port;
        }

        /// <summary>
        ///功能码1的结构
        /// </summary>
        public struct Modbus_Send_1
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///读取数量
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readcount;
        }
        /// <summary>
        ///功能码2的结构
        /// </summary>
        public struct Modbus_Send_2
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///读取数量
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readcount;
        }
        /// <summary>
        ///功能码3的结构
        /// </summary>
        public struct Modbus_Send_3
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///读取数量
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readcount;
        }
        /// <summary>
        ///功能码4的结构
        /// </summary>
        public struct Modbus_Send_4
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///读取数量
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readcount;
        }
        /// <summary>
        ///功能码5的结构
        /// </summary>
        public struct Modbus_Send_5
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///设置地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///设置内容
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] content;
        }
        /// <summary>
        ///功能码6的结构
        /// </summary>
        public struct Modbus_Send_6
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///设置地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///设置内容
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] content;
        }

        /// <summary>
        ///功能码15的结构
        /// </summary>
        public struct Modbus_Send_15
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///设置起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///设置长度
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readCount;
            /// <summary>
            ///字节计数
            /// </summary>
            public byte byteCount;
            /// <summary>
            ///设置内容
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] content;
        }

        /// <summary>
        ///功能码16的结构
        /// </summary>
        public struct Modbus_Send_16
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;
            /// <summary>
            ///设置起始地址
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] start_L;
            /// <summary>
            ///设置长度
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] readCount;
            /// <summary>
            ///字节计数
            /// </summary>
            public byte byteCount;
            /// <summary>
            ///设置内容
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] content;
        }

        /// <summary>
        ///
        /// </summary>
        public struct Modbus_Return_1
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;//站号
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_2
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;//站号
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }
        /// <summary>
        ///
        /// </summary>
        public struct Modbus_Return_3
        {
            /// <summary>
            /// 站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_4
        {
            /// <summary>
            /// 站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            /// 功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            /// 字节数
            /// </summary>
            public byte byteCount;//字节数
            /// <summary>
            /// 相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            /// 状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            /// 错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_5
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte[] byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_6
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte[] byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_15
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte[] byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }

        public struct Modbus_Return_16
        {
            /// <summary>
            ///站号
            /// </summary>
            public byte stationNO;
            /// <summary>
            ///功能码
            /// </summary>
            public byte funtionType;//功能码
            /// <summary>
            ///字节数
            /// </summary>
            public byte[] byteCount;//字节数
            /// <summary>
            ///相应内容
            /// </summary>
            public byte[] content;//相应内容
            /// <summary>
            ///状态True代表返回正确，false代表返回错误
            /// </summary>
            public bool state;//状态True代表返回正确，false代表返回错误
            /// <summary>
            ///错误代码
            /// </summary>
            public byte err;//错误代码
        }


        public class StructHelper : ISerialize
        {
            public byte[] Serialize<T>(T Data)
            {
                //得到结构体的大小
                int size = Marshal.SizeOf(Data);

                //创建byte数组
                byte[] bytes = new byte[size];
                //分配结构体大小的内存空间
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                //将结构体拷到分配好的内存空间
                Marshal.StructureToPtr(Data, structPtr, false);
                //从内存空间拷到byte数组
                Marshal.Copy(structPtr, bytes, 0, size);
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
                //返回byte数组
                return bytes;
            }

            public T DeSerialize<T>(byte[] Data)
            {
                //得到结构体的大小
                int size = Marshal.SizeOf(typeof(T));
                //byte数组长度小于结构体的大小
                //if (size > Data.Length)
                //{
                //    //返回空
                //    return
                //}
                //分配结构体大小的内存空间
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                //将byte数组拷到分配好的内存空间
                Marshal.Copy(Data, 0, structPtr, size);
                //将内存空间转换为目标结构体
                object obj = Marshal.PtrToStructure(structPtr, typeof(T));
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
                //返回结构体
                return (T)obj;
            }
        }

        public interface ISerialize
        {
            /// <summary>
            /// 序列化
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="stream"></param>
            /// <param name="item"></param>
            byte[] Serialize<T>(T Data);


            /// <summary>
            /// 反序列化
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="stream"></param>
            /// <returns></returns>
            T DeSerialize<T>(byte[] Data);
        }

        /// <summary>
        ///发送请求并且返回请求（功能码1：读取线圈状态）
        /// </summary>
        public Modbus_Return_1 Modbus_Send1(Modbus_Send_1 ms)
        {
            lock (Lockobj)
            {

                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_1 m1 = new Modbus_Return_1();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {
                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = res[2];
                        m1.byteCount = 8;
                        byte[] by1 = new byte[Convert.ToInt32(m1.byteCount)];
                        int j = 0;
                        for (int i = 3; i < res.Length - 2; i++)
                        {

                            by1[j] = res[i];
                            j++;
                        }
                        m1.content = by1;
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }



        /// <summary>
        ///发送请求并且返回请求（功能码2：读取输入状态）
        /// </summary>
        public Modbus_Return_2 Modbus_Send2(Modbus_Send_2 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_2 m1 = new Modbus_Return_2();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        //m1.byteCount = res[2];
                        m1.byteCount = 8;
                        byte[] by1 = new byte[Convert.ToInt32(m1.byteCount)];
                        int j = 0;
                        for (int i = 3; i < res.Length - 2; i++)
                        {

                            by1[j] = res[i];
                            j++;
                        }
                        m1.content = by1;
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }

        public static object Lockobj = new object();

        public Modbus_Return_3 ReadV(Modbus_Send_3 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                //得到需要收到多少数据
                Modbus_Return_3 m1 = new Modbus_Return_3();
                int Lengeth = ms.readcount[1] * 2;

                byte[] buffer = null;

                byte[] d1 = crc16(by);
                string str = null;

                foreach (int i in d1)
                {
                    str = str + " " + Convert.ToString(i, 16);
                }
                try
                {
                    _MyPort.DiscardInBuffer();
                    _MyPort.Write(d1, 0, d1.Length);
                    List<byte> buf = new List<byte>();
                    //先从缓冲区里面读3个byte
                    buf.Add((byte)_MyPort.ReadByte());//读取站号
                    buf.Add((byte)_MyPort.ReadByte());//读取功能码
                    buf.Add((byte)_MyPort.ReadByte());//读取数量
                    // 判断站号是否正确
                    if (buf[0] != ms.stationNO)
                    {
                        //站号有问题
                        throw new Exception("接收站号有问题");
                    }
                    if (buf[1] != ms.funtionType)
                    {
                        throw new Exception("接收功能码有问题");
                        //功能码回应有问题
                    }

                    if (buf[2] != Lengeth)
                    {
                        throw new Exception("接收长度有问题");
                        //应答数据有问题
                    }
                    int offset = 0;
                    do
                    {
                        byte[] b = new byte[255];
                        int num = _MyPort.Read(b, 0, 255);
                        byte[] b1 = new byte[num];
                        Array.Copy(b, 0, b1, 0, num);
                        buf.AddRange(b1);
                        offset += num;
                        if (offset == Lengeth + 2)
                        {
                            break;
                        }
                    }
                    while (true);

                    byte[] buf1 = buf.ToArray();


                    if (buf1.Length > 0)
                    {
                        m1.stationNO = buf1[0];
                        m1.funtionType = buf1[1];
                        if (buf1.Length - 2 == 3)//代表错误
                        {

                            m1.state = false;
                            m1.err = buf1[2];
                        }
                        else
                        {
                            m1.state = true;
                            m1.byteCount = buf1[2];
                            byte[] by1 = new byte[Convert.ToInt32(m1.byteCount)];
                            int j = 0;
                            for (int i = 3; i < buf1.Length - 2; i++)
                            {

                                by1[j] = buf1[i];
                                j++;
                            }
                            m1.content = by1;
                        }
                    }
                    else
                    {
                        m1.state = false;
                        m1.err = 00;
                    }
                    return m1;
                }
                catch
                {
                    m1.state = false;
                    m1.err = 00;
                    return m1;
                }
            }
        }

        /// <summary>
        ///发送请求并且返回请求（功能码3：读保持寄存器）
        /// </summary>
        public Modbus_Return_3 Modbus_Send3(Modbus_Send_3 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_3 m1 = new Modbus_Return_3();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = res[2];
                        byte[] by1 = new byte[Convert.ToInt32(m1.byteCount)];
                        int j = 0;
                        for (int i = 3; i < res.Length - 2; i++)
                        {

                            by1[j] = res[i];
                            j++;
                        }
                        m1.content = by1;
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }


        /// <summary>
        ///发送请求并且返回请求（功能码4：输入寄存器）
        /// </summary>
        public Modbus_Return_4 Modbus_Send4(Modbus_Send_4 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_4 m1 = new Modbus_Return_4();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = res[2];
                        byte[] by1 = new byte[Convert.ToInt32(m1.byteCount)];
                        int j = 0;
                        for (int i = 3; i < res.Length - 2; i++)
                        {

                            by1[j] = res[i];
                            j++;
                        }
                        m1.content = by1;
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }


        /// <summary>
        ///发送请求并且返回请求（功能码5：设置单个继电器状态）
        /// </summary>
        public Modbus_Return_5 Modbus_Send5(Modbus_Send_5 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_5 m1 = new Modbus_Return_5();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = new byte[] { res[2], res[3] };


                        m1.content = new byte[] { res[4], res[5] };
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }


        /// <summary>
        ///发送请求并且返回请求（功能码6：设置单个保持寄存器）
        /// </summary>
        public Modbus_Return_6 Modbus_Send6(Modbus_Send_6 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_6 m1 = new Modbus_Return_6();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = new byte[] { res[2], res[3] };


                        m1.content = new byte[] { res[4], res[5] };
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }


        /// <summary>
        ///发送请求并且返回请求（功能码15：设置多个继电器状态）
        /// </summary>
        public Modbus_Return_15 Modbus_Send15(Modbus_Send_15 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_15 m1 = new Modbus_Return_15();
                if (res.Length > 0)
                {
                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = new byte[] { res[2], res[3] };


                        m1.content = new byte[] { res[4], res[5] };
                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }


        /// <summary>
        ///发送请求并且返回请求（功能码16：设置多个保持寄存器）
        /// </summary>
        public Modbus_Return_16 Modbus_Send16(Modbus_Send_16 ms)
        {
            lock (Lockobj)
            {
                byte[] by = Activator.CreateInstance<StructHelper>().Serialize(ms);
                byte[] res = SendCommand(by);
                Modbus_Return_16 m1 = m1 = new Modbus_Return_16();
                if (res.Length > 0)
                {

                    m1.stationNO = res[0];
                    m1.funtionType = res[1];
                    if (res.Length - 2 == 3)//代表错误
                    {

                        m1.state = false;
                        m1.err = res[2];
                    }
                    else
                    {
                        m1.state = true;
                        m1.byteCount = new byte[] { res[2], res[3] };


                        m1.content = new byte[] { res[4], res[5] };

                    }
                }
                else
                {
                    m1.state = false;
                    m1.err = 00;
                }
                return m1;
            }
        }





        private bool Exists(string port_name)
        {
            foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
            return false;
        }

        public byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }



        //字节数组转换十六进制
        public string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }


        public byte[] crc16(byte[] Data)
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


        public Boolean open_Port()
        {

            if (_MyPort.IsOpen) _MyPort.Close();
            if (!Exists(_MyPort.PortName))
                return false;
            else
                _MyPort.Open();
            return true;
        }


        public void close_Port()
        {
            if (_MyPort.IsOpen)
            {
                _MyPort.Close();
            }
        }


        public byte[] SendCommand(byte[] by)
        {
            lock (this)
            {


                byte[] buffer = null;
                try
                {
                    byte[] d1 = crc16(by);
                    string str = null;
                    foreach (int i in d1)
                    {
                        str = str + " " + Convert.ToString(i, 16);
                    }
                    _MyPort.Write(d1, 0, d1.Length);
                    System.Threading.Thread.Sleep(100);


                    #region 根据结束字节来判断是否全部获取完成
                    int bytes = _MyPort.BytesToRead;
                    // 创建字节数组
                    buffer = new byte[bytes];
                    // 读取缓冲区的数据到数组
                    _MyPort.Read(buffer, 0, bytes);
                    // 显示读取的数据到数据窗口
                    //msg = ByteArrayToHexString(buffer);
                    #endregion
                    //字符转换         

                }
                catch (Exception ex)
                {
                    if (ex.Message != "端口被关闭。")
                    {
                        throw ex;
                    }
                }
                return buffer;
            }

        }

        //获取校验位
        public byte[] GetCheck(string Command)
        {
            byte[] jy = new byte[2];
            byte[] d1 = crc16(HexStringToByteArray(Command.Trim()));
            jy[0] = d1[d1.Length - 1];
            jy[1] = d1[d1.Length - 2];
            return jy;
        }

        public bool CheckCoammandSame(string Command)
        {
            WriteData(Command);
            string rd = ReadData();
            if (rd == Command)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void WriteData(string Command)
        {
            byte[] d1 = HexStringToByteArray(Command.Trim());
            string str = null;
            foreach (int i in d1)
            {
                str = str + " " + Convert.ToString(i, 16);
            }
            _MyPort.Write(d1, 0, d1.Length);
            System.Threading.Thread.Sleep(100);
        }

        public string ReadData()
        {
            string msg = "";
            #region 根据结束字节来判断是否全部获取完成
            int bytes = _MyPort.BytesToRead;
            // 创建字节数组
            byte[] buffer = new byte[bytes];
            // 读取缓冲区的数据到数组
            _MyPort.Read(buffer, 0, bytes);
            // 显示读取的数据到数据窗口
            msg = ByteArrayToHexString(buffer);
            #endregion
            //字符转换         
            return msg;
        }
        public string D2S(int Value, int Length)
        {
            string str = Convert.ToString(Value, 16);
            string str2 = null;
            int oldL = str.Length;
            if (Value >= 0)
            {
                if (oldL <= Length)
                {
                    for (int i = 0; i < Length - oldL; i++)
                    {
                        str = "0" + str;
                    }
                }
                else
                {
                    throw new Exception("数值越界！");
                }
            }
            else
            {
                if (oldL < Length)
                {
                    for (int i = 0; i < Length - oldL; i++)
                    {
                        str = "F" + str;
                    }
                }
                else if (oldL > Length)
                {
                    for (int i = 0; i < Length - oldL; i++)
                    {
                        str = str.Remove(0, 1);
                    }
                }
            }

            char[] st = str.ToCharArray();
            for (int i = 0; i < st.Length; i++)
            {
                if (i % 2 == 0)
                {
                    str2 = str2 + " " + st[i];
                }
                else
                {
                    str2 = str2 + str[i];
                }
            }

            return (str2.ToUpper()).Trim();
        }
    }
}
