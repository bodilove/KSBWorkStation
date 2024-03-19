using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Test.Common
{
    /// <summary>
    /// Xml序列化
    /// </summary>
    public class XmlSerializeHelper : ISerializeHelper
    {

        public void Serialize<T>(System.IO.Stream stream, T item)
        {
            //if (stream is FileStream)
            //{
            //    XmlSerializer serializer = new XmlSerializer(item.GetType());
            //    stream.Seek(0, System.IO.SeekOrigin.Begin);
            //    serializer.Serialize(stream, item);
            //}
            //else
            //{
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            serializer.Serialize(stream, item);

        }

        public T DeSerialize<T>(System.IO.Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return (T)serializer.Deserialize(stream);
        }
    }

    /// <summary>
    /// 二进制序列化
    /// </summary>
    public class BinarySerializeHelper : ISerializeHelper
    {

        public void Serialize<T>(System.IO.Stream stream, T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            formatter.Serialize(stream, item);
        }

        public T DeSerialize<T>(System.IO.Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    /// <summary>
    /// 序列化工具
    /// </summary>
    public interface ISerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="item"></param>
        void Serialize<T>(System.IO.Stream stream, T item);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        T DeSerialize<T>(System.IO.Stream stream);
    }
}
