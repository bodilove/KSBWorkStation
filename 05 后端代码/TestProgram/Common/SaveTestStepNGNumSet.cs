using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Test.Common
{
    public class SaveTestStepNGNumSet
    {
        //今日时间
        public string NowDate { get; set; }
        //NG测试项
        public string Name { get; set; }
        //NG测试描述
        public string Description { get; set; }
        //NG次数
        public int NGNum { get; set; }
        //NG时间
        public string NGdatetime { get; set; }

        private static string path = Application.StartupPath + "\\NGNumSet.cfg";
        #region
        /// <summary>
        /// 序列化Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ISerializeHelper SerializeHelper<T>() where T : ISerializeHelper
        {
            return Activator.CreateInstance<T>();
        }

        public static SaveTestStepNGNumSet Load(string Path)
        {

            FileStream mStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            SaveTestStepNGNumSet p = SerializeHelper<XmlSerializeHelper>().DeSerialize<SaveTestStepNGNumSet>(mStream);
            mStream.Close();
            return p;
        }

        public static void Save(string path, SaveTestStepNGNumSet p)
        {
            File.Delete(path);
            MemoryStream mStream = new MemoryStream();
            SerializeHelper<XmlSerializeHelper>().Serialize(mStream, p);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(mStream.ToArray());
            fs.Close();
            mStream.Close();
        }
        #endregion


        public static SaveTestStepNGNumSet LoadDate()
        {
            return Load(path);
        }


        /// <summary>
        /// 赋值只需要测试项名字和注释
        /// </summary>
        /// <param name="p"></param>
        public static void SaveNGSet(SaveTestStepNGNumSet p)
        {

            try
            {
                SaveTestStepNGNumSet NgNumSet = Load(path);
                //判断是否当天
                if (NgNumSet.NowDate == DateTime.Now.Date.ToString())
                {
                    //判断测试项是否一致
                    if (NgNumSet.Name == p.Name)
                    {
                        p.NGNum = NgNumSet.NGNum + 1;
                    }
                    else
                    {
                        p.NGNum = 1;
                    }
                }
                else
                {
                    p.NGNum = 1;
                }

                p.NowDate = DateTime.Now.Date.ToString();
                p.NGdatetime = DateTime.Now.ToString();
                MdlClass.NGTestName = p.Name;
                MdlClass.NGProduct = p.NGNum;
                Save(path, p);

            }
            catch (Exception)
            {
                p.NowDate = DateTime.Now.Date.ToString();
                p.NGdatetime = DateTime.Now.ToString();
                MdlClass.NGProduct = p.NGNum = 1;
                MdlClass.NGTestName = p.Name;
                Save(path, p);
                
            }




        }

        //删除
        public static void ClearNG()
        {
            SaveTestStepNGNumSet NGNum = new SaveTestStepNGNumSet();
            MdlClass.NGProduct = 0;
            MdlClass.NGTestName = "";
            Save(path, NGNum);
        }





    }
}
