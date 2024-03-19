
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Data;

namespace Common.SysConfig.Model
{

    /// <summary>
    /// 总配置
    /// </summary>
    [XmlRoot("SystemConfig")]
    public class SystemConfig
    {

        public int type = 0;

        public string DataSource = ".";
        public string InitialCatalog = "LGDB";
        public string UserID = "sa";
        public string Password = "1234";

        public List<LocalStationConfig> localStlst = new List<LocalStationConfig>();

        /// 序列化Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISerializeHelper SerializeHelper<T>() where T : ISerializeHelper
        {
            return Activator.CreateInstance<T>();
        }



        public SystemConfig Load(string Path)
        {
            FileStream mStream = null;
            SystemConfig p = null;

            try
            {
                //ArrPLCEvent.Clear();
                //FileStream mStream = new FileStream(@"D:\Points.xml", FileMode.Open, FileAccess.Read);
                mStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                p = SerializeHelper<XmlSerializeHelper>().DeSerialize<SystemConfig>(mStream);
                //mStream.Close();
            }
            catch
            {
                //mStream.Close();
                p = null;
            }
            finally
            {
                if (mStream != null)
                {
                    mStream.Close();
                }
            }
            return p;
        }

        public bool Save(string path)
        {
            bool res = false;
            MemoryStream mStream = null;
            FileStream fs = null;
            BinaryWriter w = null;
            try
            {
                mStream = new MemoryStream();
                SerializeHelper<XmlSerializeHelper>().Serialize(mStream, this);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                w = new BinaryWriter(fs);
                w.Write(mStream.ToArray());
                res = true;
            }
            catch
            {
                res = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (mStream != null)
                {
                    mStream.Close();
                }
            }
            return res;
        }
    }


    [XmlRoot("LocaStationConfig")]
    public class LocalStationConfig
    {
        public string StationNum = "";
        public string StationName = "";
        public List<DeviceAddress> DeviceAddresslst = new List<DeviceAddress>();
    }

    [XmlRoot("DeviceAddress")]
    public class DeviceAddress
    {
        public string Name = "";
        public ConnectType ConnectType = ConnectType.TcpIp;
        public string Address = "";
        public int Port = 5000;
    }
    public enum ConnectType
    {
        TcpIp = 1,
        Com = 2,
    }

}

