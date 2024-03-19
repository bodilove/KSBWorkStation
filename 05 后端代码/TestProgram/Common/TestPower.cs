using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Test.Common
{
    [XmlRoot("TestPowerOnManage")]
    public class TestPowerOnManage
    {
        public List<TestPowerOn> PowerOnLst = new List<TestPowerOn>();

        /// <summary>
        /// 序列化Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISerializeHelper SerializeHelper<T>() where T : ISerializeHelper
        {
            return Activator.CreateInstance<T>();
        }


        public void Load(string Path)
        {
            PowerOnLst.Clear();
            //FileStream mStream = new FileStream(@"D:\Points.xml", FileMode.Open, FileAccess.Read);
            FileStream mStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            TestPowerOnManage p = SerializeHelper<XmlSerializeHelper>().DeSerialize<TestPowerOnManage>(mStream);
            mStream.Close();
            foreach (TestPowerOn ps in p.PowerOnLst)
            {
                PowerOnLst.Add(ps);
            }
        }

        public void Save(string path)
        {
            MemoryStream mStream = new MemoryStream();
            SerializeHelper<XmlSerializeHelper>().Serialize(mStream, this);
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(mStream.ToArray());
            fs.Close();
            mStream.Close();
        }
    }
    [XmlRoot("TestPowerOn")]
    public class TestPowerOn
    {
        /// <summary>
        /// 点检配置的名称
        /// </summary>
        public string Name = "";

        /// <summary>
        /// 特定点检的SN
        /// </summary>
        public string ProductSN = "";

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result = false;

        /// <summary>
        /// 设置好的测试小步
        /// </summary>
       

        public List<TestPowerSubTestStep> TestPowerSubTestSteplist = new List<TestPowerSubTestStep>();
        
    }
   
    [XmlRoot("TestPowerSubTestStep")]
    public class TestPowerSubTestStep : IComparable
    {
        public Common.SubTestStep OrgSubTestStep = null;
       
        /// <summary>
        /// 测试小步的名称
        /// </summary>
        public string Name = "";

        public string Discription = "";

        /// <summary>
        /// 比较模式
        /// </summary>
        public Common.CompareMode CmpMode = new Common.CompareMode();

        /// <summary>
        /// 上限
        /// </summary>
        public string ULimit = "";

        /// <summary>
        /// 下限
        /// </summary>
        public string LLimit = "";

        /// <summary>
        /// 预设值
        /// </summary>
        public string NomValue = "";

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool Enable = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Ischecked = false;

        public int CompareTo(object obj)
        {
            int flag = 0;
            try
            {
                TestPowerSubTestStep ss = obj as TestPowerSubTestStep;
                flag = this.Name.CompareTo(ss.Name);

            }
            catch
            {
                throw new Exception("排序比较异常");
            }
            return flag;
        }
    }
}

