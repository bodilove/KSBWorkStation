using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class WriteLog
    {
        private string dirPath;    // 日志文件路径
        public bool IsEnable = true;

        public WriteLog(string path)
        {
            this.dirPath = path;
        }

        //每站的流程日志
        public void StationLog(string MethodName, string content, string SationNum)
        {
            if (IsEnable)
            {
                Log(MethodName, content, SationNum);
            }
        }

        //数据库操作错误日志
        public void dbErrLog(string methodName, string content)
        {
            if (IsEnable)
            {
                Log(methodName, content, "dbErr");
            }
        }

        //PLC操作日志
        public void PLCLog(string methodName, string content)
        {
            if (IsEnable)
            {
                Log(methodName, content, "PLC");
            }
        }
        //系统操作报错日志
        public void SysErroLog(string methodName, string content)
        {
            if (IsEnable)
            {
                Log(methodName, content, "SysErr");
            }
        }

        //WCF请求交互日志
        public void WCFLog(string methodName, string content)
        {
            if (IsEnable)
            {
                Log(methodName, content, "WCFSever");
            }
        }

        //容量检测
        public void SizeCheck(int fileCountMaxmin)
        {
            try
            {
                FileInfo[] files = new DirectoryInfo(dirPath).GetFiles();

                List<FileInfo> filelist = new List<FileInfo>();


                foreach (FileInfo f in files)
                {
                    //1检查文件名合法性
                    if (System.IO.Path.GetExtension(f.FullName).ToUpper() == ".LOG")
                    {

                        filelist.Add(f);
                    }
                }
                filelist.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
                if (filelist.Count > fileCountMaxmin)
                {
                    int Removetime = filelist.Count - fileCountMaxmin;
                    for (int i = 0; i < Removetime; i++)
                    {
                        File.Delete(files[0].FullName);
                        filelist.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



        private void Log(string methodName, string content, string prefix)
        {
            string fileName = string.Format("{0}{1:yyyyMMdd}.log", prefix, DateTime.Now);

            string filePath = GetDirPath(this.dirPath) + fileName;

            try
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("{0}\t{1}\t{2}", DateTime.Now.ToString("HH:mm:ss:fff"), methodName, content);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string GetDirPath(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                return string.Format(@"{0}\", dirInfo.FullName);
            }
            else
            {
                try
                {
                    dirInfo.Create();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return Environment.CurrentDirectory + @"\Log";
                }

                return string.Format(@"{0}\", dirInfo.FullName);
            }
        }

       



        public void sysErrLog(string methodName, string content)
        {
            Log(methodName, content, "sysErr");
        }

        public void sysDelSN_DBLog(string methodName, string content)
        {
            Log(methodName, content, "DelSN_DB");
        }


        public void Delete(string path, int period, string prefix)
        {
            if (period == 0)
            {
                return;
            }

            string[] files = Directory.GetFiles(path, prefix + "*.log", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (DateTime.Now.Date.CompareTo(fileInfo.LastWriteTime.Date) > period)
                {
                    fileInfo.Delete();
                }
            }
        }
    }
}

