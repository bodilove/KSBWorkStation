using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Test.Library
{
   public class SystemLib
    {

    

        public void Delay_ms(int delayTime_ms)
        {
           // Thread.Sleep(delayTime_ms);
            Thread.Sleep(TimeSpan.FromMilliseconds(delayTime_ms));
        }

        public object SetValue(UInt32 value)
        {
            Thread.Sleep(1000);
            return value;
        }

        public void OutString(string in_str, out string out_str)
        {
            out_str = in_str;
        }

        public void MessageShow(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// 取两位16进制的整型从字符串里
        /// </summary>
        /// <param name="str_IN"></param>
        /// <param name="index_from0"></param>
        /// <returns></returns>
        public byte GetOneNumberFromHexString(string str_IN, int index_from0)
        {
            try
            {
                if (str_IN == "timeout") return 0;
                string[] splitString = str_IN.Split(' ');
                string retNum = splitString[index_from0];
                return Convert.ToByte(retNum, 16);
            }
            catch(Exception ex)
            {
                return 0xff;
            }
        }


        public string CutString(string Instr,string ContainsStr,int Length)
        {
            string s = Instr;
            try
            { 
                if (Instr.Contains(ContainsStr))
                {
                    int x = Instr.IndexOf(ContainsStr);
    
                    s = Instr.Substring(x, Length);
                }
            }
            catch
            {
                s = Instr;
            }
            return s;
        }

        /// <summary>
        /// 取两位16进制的整型从字符串里
        /// </summary>
        /// <param name="str_IN"></param>
        /// <param name="index_from0"></param>
        /// <returns></returns>
        //public string GetOneNumberFromHexString(string str_IN, int index_from0)
        //{
        //    if (str_IN == "timeout") return "timeout";
        //    string[] splitString = str_IN.Split(' ');
        //    string retNum = splitString[index_from0];
        //    return retNum;
        //}

        public string GetBitsFromHexString(string str_IN, int index_from0, int bitStartFromHi0, int bitEnd)
        {
            try
            {
                if (str_IN == "timeout" || str_IN == "" || str_IN == null) return "timeout";
                int len = bitEnd - bitStartFromHi0 + 1;
                string[] splitString = str_IN.Split(' ');
                string retNum = splitString[index_from0];
                byte num = Convert.ToByte(retNum, 16);
                byte num1 = num;
                byte num2;
                string boolStr = "";
                for (int i = 0; i < 8; i++)
                {
                    num2 = (byte)(num1 / 0x80);
                    boolStr = boolStr + num2.ToString();
                    num1 = (byte)((byte)(num1 % 0x80) << 1);
                }
                string boolRet = boolStr.Substring(bitStartFromHi0, len);
                return boolRet;
            }
            catch
            {
                throw new Exception("GetBitsFromHexString wrong!");   
            }
        }

        public string GetBitFromHexString(string str_IN, int index_from0, int bit_fromLow0)
        {
            try
            {
                if (str_IN == "timeout" || str_IN == "" || str_IN == null) return "timeout";

                string[] splitString = str_IN.Split(' ');
                string retNum = splitString[index_from0];
                byte num = Convert.ToByte(retNum, 16);
                int num1 = (byte)(num << (7 - bit_fromLow0)) / 0x80;
                string boolStr = num1.ToString();
                return boolStr;
            }
            catch
            {
                throw new Exception("GetBitFromHexString wrong!");
            }
        }

        /// <summary>
        /// get sub string -- added by lvruru 20140809
        /// </summary>
        /// <returns></returns>
        public string GetSubString(string IN_string, int start_index, int sub_len)
        {
            try
            {
                string out_string = "";
                out_string = IN_string.Substring(start_index, sub_len);
                return out_string;
            }
            catch
            {
                return IN_string;
            }
        }

        /// <summary>
        /// 将一个整型数(2位)添加到一个带有空格的字符串里
        /// </summary>
        /// <param name="added_str">要添加的数</param>
        /// <param name="str_IN"></param>
        /// <param name="index_from0">添加的位置</param>
        /// <returns></returns>
        public string SetOneNumberToHexString(int added_str, string str_IN, int index_from0)
        {
            string[] splitString = str_IN.Split(' ');
            string astr;
            astr = Convert.ToString(added_str, 16);
            if (astr.Length == 1)
            {
                splitString[index_from0] = "0" + astr;
            }
            else if (astr.Length == 2)
            {
                splitString[index_from0] = astr;
            }

            string str_OUT = null;
            foreach (string str in splitString)
            {
                str_OUT += str + " ";
            }
            return str_OUT.TrimEnd(' ');
        }
        /// <summary>
        /// 将一个字符串添加到另一个字符串里
        /// </summary>
        /// <param name="added_str"></param>
        /// <param name="str_IN"></param>
        /// <param name="index_from0"></param>
        /// <returns></returns>
        public string SetOneNumberToHexString(string added_str, string str_IN, int index_from0)
        {
            string[] splitString = str_IN.Split(' ');
            string astr;
            astr = added_str;
            if (added_str == "timeout")
            {
                astr = "00";
            }
            if (astr.Length == 1)
            {
                splitString[index_from0] = "0" + astr;
            }
            else if (astr.Length == 2)
            {
                splitString[index_from0] = astr;
            }

            string str_OUT = null;
            foreach (string str in splitString)
            {
                str_OUT += str + " ";
            }
            return str_OUT.TrimEnd(' '); 
        }
        /// <summary>
        /// 获得系统时间YY-MM-DD-HH-MM-SS
        /// </summary>
        /// <returns></returns>
        public string GetSystemTime()
        {
            //string[] timeArray = new string[6];
            string dataTime = null;

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            string year = Convert.ToString(currentTime.Year, 10);
            string mouth = Convert.ToString(currentTime.Month, 10);
            if (mouth.Length < 2)
            {
                mouth = "0" + mouth;
            }
            string day = Convert.ToString(currentTime.Day, 10);
            if (day.Length < 2)
            {
                day = "0" + day;
            }
            string hour = Convert.ToString(currentTime.Hour, 10);
            if (hour.Length < 2)
            {
                hour = "0" + hour;
            }
            string minute = Convert.ToString(currentTime.Minute, 10);
            if (minute.Length < 2)
            {
                minute = "0" + minute;
            }
            string second = Convert.ToString(currentTime.Second, 10);
            if (second.Length < 2)
            {
                second = "0" + second;
            }
            dataTime = year + mouth + day + hour + minute + second;

            return dataTime;
        }
        /// <summary>
        /// 获得时间的BCD码并带有空格
        /// </summary>
        /// <returns></returns>
        public string GetTimeBCD()
        {
            string time = GetSystemTime();
            string year1 = time.Substring(0,2);
            string year2 = time.Substring(2,2);
            string month = time.Substring(4,2);
            string day = time.Substring(6,2);
            string timeRet = year1 + " " + year2 + " " + month + " " + day;
            return timeRet;
        }
        /// <summary>
        /// 将一个整型转变成字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string ChangeIntToHexString(int number)
        {
            string str;
            str = Convert.ToString(number, 16);
            int len = str.Length;
            if (len == 1)
            {
                str = "000" + str;
            }
            else if (len == 2)
            {
                str = "00" + str;
            }
            else if (len == 3)
            {
                str = "0" + str;
            }

            //str = number.ToString("H4");
            return str;
        }

        public string ChangeIntToBCDstring(int number)
        {
            string str;
            str = number.ToString("D4");
            return str;
        }

        public string ChangeByteToHexString(byte number)
        {
            string str;
            str = Convert.ToString(number, 16);
            return str;
        }

        public string ChangeByteToHexString_2H(byte number)
        {
            string str;
            str = Convert.ToString(number, 16);
            if (str.Length == 1)
            {
                str = '0' + str;
            }
            return str;
        }

        public int ChangeByteToInt(byte data)
        {
            return (int)data;
        }

        public double ChangeIntToDouble(int data)
        {
            return (double)data;
        }

        public string SpliceIntAndString(int number, string str1)
        {
            string str;
            str = ChangeIntToHexString(number) + str1;
            return str;
        }

        public string ChangeHexStringToLMF(int number)
        {
            return "";
        }

        public string ChangeIntToLMF(int number)
        {
            string str, str1, str2;
            str = ChangeIntToHexString(number);
            if (str.Length == 1)
            {
                str = "0" + str;
                str1 = str.Substring(0, 2);
                str2 = "00";
                str = str1 + " " + str2;

            }
            else if (str.Length == 2)
            {
                str = str + " 00";
            }
            else if (str.Length == 3)
            {
                str = "0" + str;
                str1 = str.Substring(2, 2);
                str2 = str.Substring(0, 2);
                str = str1 + " " + str2;

            }
            else if (str.Length == 4)
            {
                str1 = str.Substring(2, 2);
                str2 = str.Substring(0, 2);
                str = str1 + ' ' + str2;
            }
            str = str.ToUpper();
            return str;
        }

        public string ChangeStrToUpper(string instr)
        {
            return instr.ToUpper();
        }

        /// <summary>
        /// 将两个字符串连接起来不带空格
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public string SpliceString(string str1, string str2)
        {
            string str = str1 + str2;
            return str;
        }

        /// <summary>
        /// 将两个字符串连接起来，中间有空格
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public string SpliceStringWithBlankSpace(string str1, string str2)
        {
            string str = str1 +" "+ str2;
            return str;
        }

        public string SpliceStringWithBlankSpace(string str1, string str2,string str3,string str4)
        {
            string str = str1 + " " + str2 + " " + str3 + " " + str4;
            str = str.ToUpper();
            return str;
        }

        /// <summary>
        /// 从一个字符串里获得一个char
        /// </summary>
        /// <param name="str_IN"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetOneCharFromHexString(string str_IN,int index)      
        {
            if (str_IN == "timeout") return 0;
            string oneCh = str_IN.Substring(index,1);
            return Convert.ToInt32(oneCh, 16);
        }

        /// <summary>
        /// 将两个整型数相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int ADD(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// 将两个整型数相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double ADD(double a, double b, int precision)
        {
            double n = a + b;
            n = Math.Round(n, precision);
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int MUL(int a, int b)
        {
            return a * b;
        }

        public double MUL(double a, double b)
        {
            return a * b;
        }

        public double DIV(double a, double b)
        {
            return a / b;
        }

        public byte ADD_byte(byte a, byte b)
        {
            return (byte)(a + b);
        }
        /// <summary>
        /// 获得blockNum,testName的格式：TS01.001；
        /// </summary>
        /// <param name="testName"></param>
        /// <returns></returns>
        public string GetBlockNumHex(string testName)
        {
            try
            {
                if (testName != null && testName != "")
                {
                    string[] strArray = testName.Split('.');
                    string str = strArray[0];
                    string block = str.Substring(2, 2);
                    int blo = Convert.ToInt32(block);
                    block = Convert.ToString(blo, 16);
                    if (block.Length == 1)
                    {
                        block = "0" + block;
                        return block;
                    }
                    else if (block.Length == 2)
                    {
                        return block;
                    }
                    else
                    {
                        return "00";
                    }
                }
                else
                {
                    return "00";
                }
            }
            catch
            {
                throw new Exception("GetBlockNumHex wrong!");
            }
        }
        
        public string GetStepNumHex(string testName)
        {
            try
            {
                if (testName != null && testName != "")
                {
                    string[] strArray = testName.Split('.');
                    string str = strArray[1];
                    string step;
                    int blo = Convert.ToInt32(str);
                    step = Convert.ToString(blo, 16);
                    if (step.Length == 1)
                    {
                        step = "0" + step;
                        return step;
                    }
                    else if (step.Length == 2)
                    {
                        return step;
                    }
                    else
                    {
                        return "00";
                    }
                }
                else
                {
                    return "00";
                }
            }
            catch
            {
                throw new Exception("GetStepNumHex wrong!");
            }
        }

        public int CompareString(string str1,string str2)
        {
            return str1.CompareTo(str2);
        }

        public int If_ContainStr(string in_str, string contain_str)
        {
            if (in_str.Contains(contain_str))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int If_ContainStr(string in_str, string contain_str1, string contain_str2)
        {
            if (in_str.Contains(contain_str1) && in_str.Contains(contain_str2))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int If_ContainStr_NotContainStr(string in_str, string contain_str, string notcontain_str)
        {
            if (in_str.Contains(contain_str) && !in_str.Contains(notcontain_str))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public string ReturnString(string str_IN)
        {
            return str_IN;
        }

        public string ReturnBool(Test.Common.TestStatus TS1Result)
        {
            return TS1Result.ToString();
        }

        public string ReturnB(bool TS1Result)
        {
            return TS1Result.ToString();
        }

        public double ReturnDouble(double doub_IN)
        {
            return doub_IN;
        }

        public int ReturnInt(int int_IN)
        {
            return int_IN;
        }

    }
}
