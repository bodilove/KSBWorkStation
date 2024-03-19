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

        public WriteLog(string path)
        {
            this.dirPath = path;
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

        public void SysLog(string methodName, string content)
        {
            Log(methodName, content, "sys");
        }

        public void dbErrLog(string methodName, string content)
        {
            Log(methodName, content, "dbErr");
        }

        public void sysErrLog(string methodName, string content)
        {
            Log(methodName, content, "sysErr");
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

