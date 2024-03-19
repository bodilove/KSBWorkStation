using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
namespace Test.Common
{
   public class ReadPLC
    {
        int stationNo = 02;

        //frmHardWareDebug Masterform = null;

        Common.Modbus MyModbus = null;

        Thread Readallth = null;


        #region

        //S1	I0.0	顶升气缸原位信号
        //S2	I0.1	顶升气缸工作信号
        //S3	I0.2	挡停1气缸原位信号
        //S4	I0.3	挡停1气缸工作信号
        //S5	I0.4	挡停2气缸原位信号
        //S6	I0.5	挡停2气缸工作信号
        //S7	I0.6	托盘检测信号1
        //S8	I0.7	托盘检测信号2
        //S9	I1.0	载具检测信号
        //S10	I1.1	机械手Z轴气缸原位信号(Z1)
        //S11	I1.2	机械手Z轴气缸工作位信号(Z1)
        //S12	I1.3	旋转气缸原位信号
        //S13	I1.4	旋转气缸工作位信号
        //S14	I1.5	抓手1气缸原位信号
        //S15	I1.6	抓手1气缸工作位信号
        //S16	I1.7	抓手2气缸原位信号
        //S17	I2.0	抓手2气缸工作位信号
        //S18	I2.1	屏蔽箱抽屉气缸原位信号
        //S19	I2.2	屏蔽箱抽屉气缸工作位信号
        //S20	I2.3	屏蔽箱Z轴气缸原位信号(Z2)
        //S21	I2.4	屏蔽箱Z轴气缸工作位信号(Z2)
        //S32	I16.7	急停按钮信号
        //S33	I17.0	复位按钮信号
        //S34	I17.1	接收工位2要板信号1
        //S35	I17.2	接收工位2放板信号1
        //S36	I17.3	接收工位4要板信号2
        //S37	I17.4	接收工位4放板信号2
        //S38	I17.5	收产品模式信号
        //EM2_输入14	I17.6	
        //EM2_输入15	I17.7	
        //KA1	Q0.0	高低频切换控制继电器
        //KA2	Q0.1	发送放板信号1到工位2控制继电器（外）
        //KA4	Q0.2	发送要板信号1到工位2控制继电器（内）
        //KA6	Q0.3	发送放板信号2到工位4控制继电器（内）
        //KA8	Q0.4	发送要板信号1到工位4控制继电器（外）
        //YV1	Q0.5	总电磁阀
        //YV2	Q0.6	顶升气缸归位电磁阀
        //YV3	Q0.7	顶升气缸工作电磁阀
        //YV4	Q1.0	挡停1气缸归位电磁阀
        //YV5	Q1.1	挡停1气缸工作电磁阀
        //YV6	Q1.2	挡停2气缸归位电磁阀
        //YV7	Q1.3	挡停2气缸工作电磁阀
        //YV8	Q1.4	机械手Z轴气缸归位电磁阀
        //YV9	Q1.5	机械手Z轴气缸工作电磁阀
        //YV10	Q1.6	旋转气缸归位电磁阀
        //YV11	Q1.7	旋转气缸工作电磁阀
        //YV12	Q8.0	抓手气缸归位电磁阀
        //YV13	Q8.1	抓手气缸工作电磁阀
        //YV14	Q8.2	屏蔽箱抽屉归位电磁阀
        //YV15	Q8.3	屏蔽箱抽屉工作电磁阀
        //YV16	Q8.4	屏蔽箱Z轴归位电磁阀
        //YV17	Q8.5	屏蔽箱Z轴工作电磁阀
        //YV18	Q8.6	按键气缸1归位电磁阀
        //YV19	Q8.7	按键气缸1工作电磁阀
        //YV20	Q12.0	按键气缸2归位电磁阀
        //YV21	Q12.1	按键气缸2工作电磁阀
        //YV22	Q12.2	按键气缸3归位电磁阀
        //YV23	Q12.3	按键气缸3工作电磁阀
        //YV24	Q12.4	按键气缸4归位电磁阀
        //YV25	Q12.5	按键气缸4工作电磁阀
        //YV26	Q12.6	按键气缸5归位电磁阀
        //YV27	Q12.7	按键气缸5工作电磁阀
        //YV28	Q16.0	按键气缸6归位电磁阀
        //YV29	Q16.1	按键气缸6工作电磁阀
        //H2	Q16.2	设备报警指示灯
        //H3	Q16.3	设备待机指示灯
        //H4	Q16.4	设备运行指示灯
        //Buzzer	Q16.5	蜂鸣器
        //H5	Q16.6	3D标定失败指示灯
        //H6	Q16.7	3D标定指示灯
        //H7	Q17.0	3D标定合格指示灯


        //PLC的V区：

        //VW0		Safe_door 安全门使能(16#FFFF）
        //VW10	Tray_scan	 托盘扫码（扫码使能：16#FFFF）
        //VW20	Tack_out	取出产品（取出产品完成：16#FFFF）
        //VW30	Test_mode 测试模式（自动模式：16#0000，收产品模式：16#FFFF）


        #endregion

        #region PLC读取点


        //public bool run = false;//运行
        //public bool stop = false;//急停
        //public bool standby = false;//待机
        //public bool IsReady = false;//是否准备完毕

        /// <summary>
        /// I0.2 终检Z轴原位信号 
        /// </summary>
        public bool Z1_Org = false;         //终检Z轴原位信号  

        /// <summary>
        /// I0.3 终检Z轴工作位信号 
        /// </summary>
        public bool Z1_Work = false;        //终检Z轴工作位信号 

        /// <summary>
        /// I0.0 屏蔽箱原位信号
        /// </summary>
        public bool ShieldedBoxDoor_CLose = false;  //屏蔽箱原位信号

        /// <summary>
        /// I0.1 屏蔽箱工作位信号（弹出）
        /// </summary>
        public bool ShieldedBoxDoor_Open = false;       //屏蔽箱工作位信号

        public bool CheckProduct = false;             //产品检测传感器

        public bool LED_status = false;                 //LED灯

        public bool fig1 = false;    //手指气缸1

        public bool fig2 = false;    //手指气缸2

        public bool fig3 = false;    //手指气缸3

        public bool fig4 = false;    //手指气缸4

        public bool fig5 = false;    //手指气缸5

        public bool fig6 = false;    //手指气缸6

        public bool Reset = false;    //复位按钮

        public bool Estop = false;    //急停按钮

        public bool IsRun = false;  //急停状态

        public bool LightTest = false;    //光亮测试结果












        /// <summary>
        /// I1.3 旋转气缸原位信号
        /// </summary>
        public bool Rotatry_Org = false;

        /// <summary>
        /// I1.4 旋转气缸工作位信号
        /// </summary>
        public bool Rotatry_Work = false;

        /// <summary>
        /// I1.5 抓手1气缸原位信号
        /// </summary>
        public bool GripPawl1_Org = false;

        /// <summary>
        /// I1.6 抓手1气缸工作位信号
        /// </summary>
        public bool GripPawl1_Work = false;

        /// <summary>
        /// I1.7 抓手2气缸原位信号
        /// </summary>
        public bool GripPawl2_Org = false;

        /// <summary>
        /// I2.0 抓手2气缸工作位信号
        /// </summary>
        public bool GripPawl2_Work = false;


        /// <summary>
        /// I2.3 屏蔽箱Z轴气缸原位信号
        /// </summary>
        public bool Z2_Org = false;

        /// <summary>
        /// I2.4 屏蔽箱Z轴气缸工作位信号
        /// </summary>
        public bool Z2_Work = false;

        /// <summary>
        /// Q16.5 蜂鸣器输出点
        /// </summary>
        public bool Buzzer_On = false;

        /// <summary>
        /// VW0 安全门使能，PC置位或清零
        /// </summary>
        public bool SecurityDoor_EN = false;

        /// <summary>
        /// VW10 托盘扫码，PC读到FFFF后，启动扫码后清零
        /// </summary>
        public bool Pallet_Come = false;

        /// <summary>
        /// VW20 取出产品，PC取走产品后，将该区置位
        /// </summary>
        public bool Product_Leave = false;

        /// <summary>
        /// VW30 测试模式
        /// </summary>
        public bool Test_Mode = false;

        #endregion

        #region PLC输出点

        /// <summary>
        /// Q0.0 高低频切换控制继电器
        /// </summary>
        public int RF_Switch = 0;



        /// <summary>
        /// Q0.2 Z轴气缸归位电磁阀
        /// </summary>

        public int QZ1_org = 02;     // 气缸松开 

        /// <summary>
        /// Q0.3 Z轴气缸工作电磁阀
        /// </summary>
        public int QZ1_work = 03;    //气缸压紧

        /// <summary>
        /// Q0.0 屏蔽箱抽屉归位电磁阀
        /// </summary>
        public int QShieldBoxDoor_Close = 00;  //屏蔽箱关闭

        /// <summary>
        /// Q0.1 屏蔽箱抽屉工作电磁阀
        /// </summary>
        public int QShieldBoxDoor_Open = 01;   //屏蔽箱打开





        /// <summary>
        /// Q0.4 按键气缸1归位电磁阀
        /// </summary>
        public int Qfig1_org = 04;

        /// <summary>
        /// Q0.5 按键气缸1工作位电磁阀
        /// </summary>
        public int Qfig1_work = 05;

        /// <summary>
        /// Q0.6 按键气缸2归位电磁阀
        /// </summary>
        public int Qfig2_org = 06;

        /// <summary>
        /// Q0.7 按键气缸2工作位电磁阀
        /// </summary>
        public int Qfig2_work = 07;

        /// <summary>
        /// Q1.0 按键气缸3归位电磁阀
        /// </summary>
        public int Qfig3_org = 08;

        /// <summary>
        /// Q1.1 按键气缸3工作位电磁阀
        /// </summary>
        public int Qfig3_work = 09;



        /// <summary>
        /// Q1.2 按键气缸4归位电磁阀
        /// </summary>
        public int Qfig4_org = 10;

        /// <summary>
        /// Q1.3 按键气缸4工作位电磁阀
        /// </summary>
        public int Qfig4_work = 11;

        /// <summary>
        /// Q1.4 按键气缸5归位电磁阀
        /// </summary>
        public int Qfig5_org = 12;

        /// <summary>
        /// Q1.5 按键气缸5工作位电磁阀
        /// </summary>
        public int Qfig5_work = 13;

        /// <summary>
        /// Q1.6按键气缸6归位电磁阀
        /// </summary>
        public int Qfig6_org = 14;

        /// <summary>
        /// Q1.7 按键气缸6工作位电磁阀
        /// </summary>
        public int Qfig6_work = 15;


        /// <summary>
        /// Q8.0 终检失败指示灯
        /// </summary>
        public int TFailLED = 64;   //终检失败指示灯

        /// <summary>
        /// Q8.1 终检测试指示灯
        /// </summary>
        public int TestingLED = 65;   //终检测试指示灯

        /// <summary>
        /// Q8.2 终检测试指示灯
        /// </summary>
        public int TSucessLED = 66;   //终检测试成功指示灯


        /// <summary>
        /// Q8.3 ST3通讯成功
        /// </summary>
        public int ST3LED = 67;   //ST3通讯成功



        /// <summary>
        ///   //test_en	VW10	光亮传感器信号输出（16#FFFF）
        /// </summary>
        public int VW10_Light = 5;


        /// <summary>
        /// //test_en	VW20	光亮传感器测试使能（16#FFFF）
        /// </summary>
        public int VW20_CanTest = 10;








        /// <summary>
        /// Q1.6 旋转气缸归位电磁阀
        /// </summary>
        public int QRotatry_Org = 14;

        /// <summary>
        /// Q1.7 旋转气缸工作电磁阀
        /// </summary>
        public int QRotatry_Work = 15;

        /// <summary>
        /// Q8.0 抓手气缸归位电磁阀
        /// </summary>
        public int QGripPawl_Org = 64;

        /// <summary>
        /// Q8.1 抓手气缸工作电磁阀
        /// </summary>
        public int QGripPawl_Work = 65;



        /// <summary>
        /// Q8.4 屏蔽箱Z轴归位电磁阀
        /// </summary>
        public int QZ2_org = 68;
        /// <summary>
        /// Q8.5 屏蔽箱Z轴工作位电磁阀
        /// </summary>
        public int QZ2_work = 69;






        /// <summary>
        /// Q16.5 蜂鸣器
        /// </summary>
        public int QBuzzer = 133;

        /// <summary>
        /// Q16.6 测试失败指示灯
        /// </summary>
        public int QLED_testFail = 134;

        /// <summary>
        /// Q16.7 正在测试指示灯
        /// </summary>
        public int QLED_testing = 135;

        /// <summary>
        /// Q17.0 测试合格指示灯
        /// </summary>
        public int QLED_testPass = 136;

        /// <summary>
        /// VW0 安全门使能
        /// </summary>
        public int VW0_SecurityDoor_EN = 0;

        /// <summary>
        /// VW10 托盘扫码
        /// </summary>
        public int VW10_Pallet_Come = 5;

        /// <summary>
        /// VW20 取出产品
        /// </summary>
        public int VW20_Product_Leave = 10;

        /// <summary>
        /// VW30 测试模式
        /// </summary>
        public int VW30_Test_Mode = 15;


        public bool AS22UpperModePart = false;
        public bool AS22LowerModePart = false;

        public bool EP21UpperModePart = false;
        public bool EP21LowerModePart = false;

        #endregion

        #region Modbus结构体
        Common.Modbus.Modbus_Send_1 SendStruct1 = new Common.Modbus.Modbus_Send_1();//读取线圈状态

        Common.Modbus.Modbus_Send_3 SendStruct3 = new Common.Modbus.Modbus_Send_3();//读取保持寄存器

        Common.Modbus.Modbus_Send_6 SendStruct6 = new Common.Modbus.Modbus_Send_6();//写入单个保持寄存器

        Common.Modbus.Modbus_Send_16 SendStruct16 = new Common.Modbus.Modbus_Send_16();//写入多个保持寄存器

        Common.Modbus.Modbus_Send_5 SendStruct5 = new Common.Modbus.Modbus_Send_5();//设置单个线圈

        Common.Modbus.Modbus_Send_2 SendStruct2 = new Common.Modbus.Modbus_Send_2();//读取多个I点
        #endregion

        /// <summary>
        /// 初始化PLC
        /// </summary>
        public ReadPLC Init(int ComNo)
        {
            MyModbus = new Common.Modbus();
           MyModbus.ConfigCom("COM" + ComNo, 19200, System.IO.Ports.Parity.Even, 8, System.IO.Ports.StopBits.One);
            try
            {

               // MyModbus.ConfigCom("COM" + ComNo, 19200, System.IO.Ports.Parity.Even, 8, System.IO.Ports.StopBits.One);
                MyModbus.open_Port();
                return this;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
            return this;
        }

        /// <summary>
        /// 释放PLC资源
        /// </summary>
        public void Quit()
        {
            MyModbus.close_Port();
        }


        public string atr = "";

        /// <summary>
        /// 读取所有的点
        /// </summary>
        public void ReadallPoint()
        {
            try
            {
                SendStruct2.stationNO = (byte)this.stationNo;        //读取I点
                SendStruct2.funtionType = (byte)Common.Modbus.FuntionType.Read_Input_State;
                SendStruct2.start_L = new byte[] { 0x00, 0 };
                SendStruct2.readcount = new byte[] { 0x00, 64 };
                Common.Modbus.Modbus_Return_2 ReturnStruct2 = MyModbus.Modbus_Send2(SendStruct2);
                if (ReturnStruct2.state)
                {

                    string redata1 = Convert.ToString(ReturnStruct2.content[0], 2);
                    redata1 = redata1.PadLeft(8, '0');
                    Z1_Org = Convert.ToBoolean(int.Parse(redata1[5].ToString()));
                    Z1_Work = Convert.ToBoolean(int.Parse(redata1[4].ToString()));


                    ShieldedBoxDoor_Open = Convert.ToBoolean(int.Parse(redata1[6].ToString()));
                    ShieldedBoxDoor_CLose = Convert.ToBoolean(int.Parse(redata1[7].ToString()));
                    //Console.WriteLine(ShieldedBoxDoor_Open.ToString() + "  " + ShieldedBoxDoor_CLose.ToString());
                    CheckProduct = Convert.ToBoolean(int.Parse(redata1[3].ToString()));
                    LED_status = Convert.ToBoolean(int.Parse(redata1[2].ToString()));
                    fig1 = Convert.ToBoolean(int.Parse(redata1[1].ToString()));
                    fig2 = Convert.ToBoolean(int.Parse(redata1[0].ToString()));

                    //I1.0-1.7
                    string redata2 = Convert.ToString(ReturnStruct2.content[1], 2);
                    redata2 = redata2.PadLeft(8, '0');
                    fig3 = Convert.ToBoolean(int.Parse(redata2[7].ToString()));
                    fig4 = Convert.ToBoolean(int.Parse(redata2[6].ToString()));
                    fig5 = Convert.ToBoolean(int.Parse(redata2[5].ToString()));
                    fig6 = Convert.ToBoolean(int.Parse(redata2[4].ToString()));
                    Reset = Convert.ToBoolean(int.Parse(redata2[3].ToString()));
                    Estop = Convert.ToBoolean(int.Parse(redata2[2].ToString()));

                    //I2.0-2.7
                    //01000110
                    string redata3 = Convert.ToString(ReturnStruct2.content[2], 2);
                    redata3 = redata3.PadLeft(8, '0');
                    if (redata3[2] == '0' && redata3[3] == '0')
                    {
                        AS22UpperModePart = true;
                    }
                    else
                    {
                        AS22LowerModePart = false;
                    }

                    if (redata3[5] == '0' && redata3[6] == '0')
                    {
                        EP21UpperModePart = true;
                    }
                    else
                    {
                        EP21LowerModePart = false;
                    }
                    if (redata3[1] == '1')
                    {
                        AS22LowerModePart = true;
                    }
                    else
                    {
                        AS22LowerModePart = false;
                    }
                    if (redata3[4] == '1')
                    {
                        EP21LowerModePart = true;
                    }
                    else
                    {
                        EP21LowerModePart = false;
                    }


                    //01110110
                    //if(

                    //string redata4 = Convert.ToString(ReturnStruct2.content[3], 2);
                    Console.WriteLine(":::::::::::" + redata3);
                }
                //Q点
                IsRun = ReadSingleCoil(ST3LED);    //读取Q点


            }
            catch(Exception e)
            {
                Console.WriteLine("-------------------"+e.ToString());
            }
            ////  V区
            //  byte[] v_data = ReadVregister(VW0_SecurityDoor_EN, 16);
            //  if (v_data[0] == 0xff && v_data[1] == 0xff)
            //  {
            //      SecurityDoor_EN = true;
            //  }
            //  else SecurityDoor_EN = false;

            //  if (v_data[10] == 0xff && v_data[11] == 0xff)
            //  {
            //      Pallet_Come = true;
            //  }
            //  else Pallet_Come = false;

            //  if (v_data[20] == 0xff && v_data[21] == 0xff)
            //  {
            //      Product_Leave = true;
            //  }
            //  else Product_Leave = false;

            //  if (v_data[30] == 0xff && v_data[31] == 0xff)
            //  {
            //      Test_Mode = true;
            //  }
            //  else Test_Mode = false;
        }

        public void AS22Check()
        {
            if (AS22UpperModePart)
            {

            }
            else
            {
                throw new Exception("上模部分不是AS22的夹具,请重新更换夹具。。。");
            }

            if (AS22LowerModePart)
            {

            }
            else
            {
                throw new Exception("下模部分不是AS22的夹具,请重新更换夹具。。。");
            }
        }

        public void EP21Check()
        {
            if (EP21UpperModePart)
            {

            }
            else
            {
                throw new Exception("上模部分不是EP21的夹具,请重新更换夹具。。。");
            }

            if (EP21LowerModePart)
            {

            }
            else
            {
                throw new Exception("下模部分不是EP21的夹具,请重新更换夹具。。。");
            }
        }


        /// <summary>
        /// 保持接收
        /// </summary>
        void KeepRec()
        {
            do
            {
                ReadallPoint();
              //  Thread.Sleep(TimeSpan.FromMilliseconds(100));
            } while (true);
        }

        /// <summary>
        /// 开启读取线程
        /// </summary>
        public void StartReadth()
        {
            Readallth = new Thread(KeepRec);
            Readallth.Start();
        }

        /// <summary>
        /// 停止读取线程
        /// </summary>
        public void StopReadth()
        {
            if (Readallth != null && Readallth.IsAlive)
            {
                Readallth.Abort();
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 测试完成后发送给PLC完成信号
        /// </summary>
        public void TestOverSendtoPLC()
        {
            SendStruct16.stationNO = (byte)this.stationNo; ;
            SendStruct16.funtionType = (byte)Common.Modbus.FuntionType.Set_Multi_Register;
            SendStruct16.start_L = new byte[2] { 00, 05 };

            SendStruct16.readCount = new byte[2] { 00, 02 };
            SendStruct16.byteCount = 04;
            SendStruct16.content = new byte[4] { 0xFF, 00, 0xFF, 00 };
            MyModbus.Modbus_Send16(SendStruct16);
        }

        /// <summary>
        /// 读取单个线圈状态
        /// </summary>
        /// <param name="address"></param>
        public bool ReadSingleCoil(int address)
        {
            SendStruct1.stationNO = (byte)this.stationNo;
            SendStruct1.funtionType = (byte)Common.Modbus.FuntionType.Read_Coil_State;
            SendStruct1.start_L = new byte[2] { 00, (byte)address };
            SendStruct1.readcount = new byte[2] { 0, 1 };
            Common.Modbus.Modbus_Return_1 ReturnStruct1 = MyModbus.Modbus_Send1(SendStruct1);
            if (ReturnStruct1.content[0] == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 读取单个V区状态（保持继电器）
        /// </summary>
        /// <param name="startAddress"></param>
        public byte[] ReadVregister(int startAddress, byte readNum_2BYTES)
        {
            SendStruct3.stationNO = (byte)this.stationNo;
            SendStruct3.funtionType = (byte)Common.Modbus.FuntionType.Read_Register;
            SendStruct3.start_L = new byte[2] { 00, (byte)startAddress };
            SendStruct3.readcount = new byte[2] { 0, readNum_2BYTES };
            Common.Modbus.Modbus_Return_3 ReturnStruct3 = MyModbus.Modbus_Send3(SendStruct3);
            byte[] rtn485Data = ReturnStruct3.content;
            return rtn485Data;
        }

        /// <summary>
        /// 设置单个V区状态（保持继电器）
        /// </summary>
        /// <param name="address"></param>
        public void SetVregister(int address, bool Value)
        {
            int i = 0;
            for (i =0;i <3;i++)
            {
           

                SendStruct6.stationNO = (byte)this.stationNo;
                SendStruct6.funtionType = (byte)Common.Modbus.FuntionType.Set_Single_Register;
                SendStruct6.start_L = new byte[2] { 00, (byte)address };
                if (Value)
                {
                    SendStruct6.content = new byte[2] { 0xFF, 0xFF };
                }
                else
                {
                    SendStruct6.content = new byte[2] { 00, 00 };
                }
                Modbus.Modbus_Return_6 dr = MyModbus.Modbus_Send6(SendStruct6);
                if (dr.state)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            if (i >= 2)
            {
                Console.WriteLine("控制PLC失败。");
            }
           
        }

        /// <summary>
        /// 设置单个线圈
        /// </summary>
        public void SetSingleCoil(int address, bool Value)
        {
            int i = 0;
            for (i = 0; i < 3; i++)
            {
           
                SendStruct5.stationNO = (byte)this.stationNo;
                SendStruct5.funtionType = (byte)Common.Modbus.FuntionType.Set_Single_Relay;
                SendStruct5.start_L = new byte[2] { 00, (byte)address };
                if (Value)
                {
                    SendStruct5.content = new byte[2] { 0xFF, 00 };
                }
                else
                {
                    SendStruct5.content = new byte[2] { 00, 00 };
                }

                Modbus.Modbus_Return_5 dr = MyModbus.Modbus_Send5(SendStruct5);
                if (dr.state)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            if (i >= 2)
            {
                Console.WriteLine("控制PLC失败。");
            }
           
        }

        /// <summary>
        /// 机械手Z轴气缸工作
        /// </summary>
        public void Z1_WorkSet()
        {
            SetSingleCoil(QZ1_work, true);
            SetSingleCoil(QZ1_org, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (Z1_Work && !Z1_Org)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("Z轴下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 机械手Z轴气缸归位
        /// </summary>
        public void Z1_OrgSet()
        {
            SetSingleCoil(QZ1_work, false);
            SetSingleCoil(QZ1_org, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (Z1_Org && !Z1_Work)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 6000)
                {
                    throw new Exception("Z轴抬起不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 设置VW20
        /// </summary>
        public void Set_VW20()
        {
            SetVregister(VW20_CanTest, true);
        }

        public void CLR_VW20()
        {
            SetVregister(VW20_CanTest, false);
        }

        /// <summary>
        /// 读写VW0
        /// </summary>
        public bool Read_VW10()
        {
            byte[] v_data = ReadVregister(VW10_Light, 1);
            if (v_data[0] == 0xff && v_data[1] == 0xff)
            {
                LightTest = true;
            }
            else
            {
                LightTest = false;
            }
            return LightTest;
        }

        public bool CLR_VW10()
        {
            bool s = false;
            SetVregister(VW10_Light, false);
            bool l = Read_VW10();
            if (l == false)
            {
                s = true;
            }
            return s;
        }



        /// <summary>
        /// 旋转气缸工作
        /// </summary>
        public void RotatryWorkSet()
        {
            SetSingleCoil(QRotatry_Work, true);
            SetSingleCoil(QRotatry_Org, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (Rotatry_Work && !Rotatry_Org)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 6000)
                {
                    throw new Exception("旋转气缸旋转不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        public void Testing_failLED_OFF()
        {
            SetSingleCoil(TFailLED, false);


            //Stopwatch sp = new Stopwatch();
            //sp.Start();
            //do
            //{
            //    if (Z1_Org && !Z1_Work)
            //    {
            //        break;
            //    }
            //    if (sp.ElapsedMilliseconds > 6000)
            //    {
            //        throw new Exception("测试失败指示灯关闭失败。。。");
            //    }
            //    Thread.Sleep(100);

            //} while (true);
            //sp.Stop();
        }

        public void Testing_failLED_ON()
        {
            SetSingleCoil(TFailLED, true);
            //SetSingleCoil(QZ1_org, true);

            //Stopwatch sp = new Stopwatch();
            //sp.Start();
            //do
            //{
            //    if (Z1_Org && !Z1_Work)
            //    {
            //        break;
            //    }
            //    if (sp.ElapsedMilliseconds > 6000)
            //    {
            //        throw new Exception("测试失败指示灯打开失败。。。");
            //    }
            //    Thread.Sleep(100);

            //} while (true);
            //sp.Stop();
        }

        public void Testing_LED_ON()
        {
            SetSingleCoil(TestingLED, true);
        }

        public void Testing_LED_OFF()
        {
            SetSingleCoil(TestingLED, false);
        }

        public void TSucessLED_ON()
        {
            SetSingleCoil(TSucessLED, true);
        }

        public void TSucessLED_OFF()
        {
            SetSingleCoil(TSucessLED, false);
        }


        /// <summary>
        /// 旋转气缸归位
        /// </summary>
        public void RotatryOrgSet()
        {
            SetSingleCoil(QRotatry_Work, false);
            SetSingleCoil(QRotatry_Org, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (Rotatry_Org && !Rotatry_Work)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("旋转气缸归位不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 抓手气缸工作
        /// </summary>
        public void GripPawlWorkSet()
        {
            SetSingleCoil(QGripPawl_Work, true);
            SetSingleCoil(QGripPawl_Org, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (GripPawl1_Work && !GripPawl1_Org)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("抓手抓紧不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 抓手气缸回位
        /// </summary>
        public void GripPawlOrgSet()
        {
            SetSingleCoil(QGripPawl_Work, false);

            SetSingleCoil(QGripPawl_Org, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (GripPawl1_Org && !GripPawl1_Work)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("抓手松开不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 屏蔽箱抽屉关闭
        /// </summary>
        public void ShieldBoxDoorCloseSet()
        {
            SetSingleCoil(QShieldBoxDoor_Open, false);
            SetSingleCoil(QShieldBoxDoor_Close, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (ShieldedBoxDoor_CLose && !ShieldedBoxDoor_Open)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 6000)
                {
                    throw new Exception("屏蔽箱关闭不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 屏蔽箱抽屉打开
        /// </summary>
        public void ShieldBoxDoorOpenSet()
        {
            SetSingleCoil(QShieldBoxDoor_Open, true);
            SetSingleCoil(QShieldBoxDoor_Close, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (ShieldedBoxDoor_Open && !ShieldedBoxDoor_CLose)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 6000)
                {
                    throw new Exception("屏蔽箱打开不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 屏蔽箱Z轴气缸工作
        /// </summary>
        public void Z2_WorkSet()
        {
            SetSingleCoil(QZ2_work, true);
            SetSingleCoil(QZ2_org, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (Z2_Work && !Z2_Org)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("屏蔽箱内Z轴下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 屏蔽箱Z轴气缸归位
        /// </summary>
        public void Z2_OrgSet()
        {
            SetSingleCoil(QZ2_work, false);
            SetSingleCoil(QZ2_org, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (!Z2_Work && Z2_Org)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("屏蔽箱内Z轴抬起不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 按键气缸1动作
        /// </summary>
        public void Finger1_Work()
        {
            SetSingleCoil(Qfig1_work, true);
            SetSingleCoil(Qfig1_org, false);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (fig1)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        public void Finger1_org()
        {
            SetSingleCoil(Qfig1_work, false);
            SetSingleCoil(Qfig1_org, true);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (!fig1)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 按键气缸2动作
        /// </summary>
        public void Finger2_Work()
        {
            SetSingleCoil(Qfig2_work, true);
            SetSingleCoil(Qfig2_org, false);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (fig2)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        public void Finger2_org()
        {
            SetSingleCoil(Qfig2_work, false);
            SetSingleCoil(Qfig2_org, true);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (!fig2)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();
        }

        /// <summary>
        /// 按键气缸3动作
        /// </summary>
        public void Finger3_Work()
        {
            SetSingleCoil(Qfig3_work, true);
            SetSingleCoil(Qfig3_org, false);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (fig3)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();

        }

        public void Finger3_org()
        {
            SetSingleCoil(Qfig3_work, false);
            SetSingleCoil(Qfig3_org, true);

            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                if (!fig3)
                {
                    break;
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    throw new Exception("手指气缸1下压不到位。。。");
                }
                Thread.Sleep(100);

            } while (true);
            sp.Stop();

        }

        /// <summary>
        /// 按键气缸4动作
        /// </summary>
        public void Finger4_Work(bool ifWork)
        {
            SetSingleCoil(Qfig4_work, ifWork);
            SetSingleCoil(Qfig4_org, !ifWork);
            Thread.Sleep(100);
        }

        /// <summary>
        /// 按键气缸5动作
        /// </summary>
        public void Finger5_Work(bool ifWork)
        {
            SetSingleCoil(Qfig5_work, ifWork);
            SetSingleCoil(Qfig5_org, !ifWork);
            Thread.Sleep(100);
        }

        /// <summary>
        /// 按键气缸6动作
        /// </summary>
        public void Finger6_Work(bool ifWork)
        {
            SetSingleCoil(Qfig6_work, ifWork);
            SetSingleCoil(Qfig6_org, !ifWork);
            Thread.Sleep(100);
        }

        /// <summary>
        /// RF Swtich Close: 315MHz
        /// </summary>
        public void RF_SwtichClose()
        {
            SetSingleCoil(RF_Switch, true);
            Thread.Sleep(100);
        }

        /// <summary>
        /// RF Swtich Open: 433MHz
        /// </summary>
        public void RF_SwtichOpen()
        {
            SetSingleCoil(RF_Switch, false);
            Thread.Sleep(100);
        }

        public void LED_TestFail(bool ifFail)
        {
            SetSingleCoil(QLED_testFail, ifFail);
        }

        public void LED_Testing(bool ifTesting)
        {
            SetSingleCoil(QLED_testing, ifTesting);
        }

        public void LED_TestPass(bool ifPass)
        {
            SetSingleCoil(QLED_testPass, ifPass);
        }

        #region Write VW

        /// <summary>
        /// 安全门使能
        /// </summary>
        public void SecurityDoor_Enable()
        {
            SetVregister(VW0_SecurityDoor_EN, true);
        }

        /// <summary>
        /// 安全门不使能
        /// </summary>
        public void SecurityDoor_Disable()
        {
            SetVregister(VW0_SecurityDoor_EN, false);
        }

        /// <summary>
        /// 托盘扫码结束，将VW10清0
        /// </summary>
        public void PalletScan_End()
        {
            SetVregister(VW10_Pallet_Come, false);
        }

        /// <summary>
        /// 产品已经取走，将VW20写FFFF
        /// </summary>
        public void ProductLeave_End()
        {
            SetVregister(VW20_Product_Leave, true);
        }

        #endregion



    }
}
