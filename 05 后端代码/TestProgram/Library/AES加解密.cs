using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Library
{
    class AES加解密
    {
        ///// AES加密
        ///// </summary>
        ///// <param name="inputdata">输入的数据</param>
        ///// <param name="iv">向量128位，首次为全0</param>
        ///// <param name="strKey">加密密钥</param>
        ///// <returns></returns>
        //public byte[] AESEncrypt(byte[] inputdata, byte[] iv, string strKey)
        //  {
        //      //分组加密算法   
        //     SymmetricAlgorithm des = Rijndael.Create();
        //     byte[] inputByteArray = inputdata;//得到需要加密的字节数组       
        //     //设置密钥及密钥向量
        //     des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 32));
        //     des.IV = iv;
        //     using (MemoryStream ms = new MemoryStream())
        //     {
        //         using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
        //         {
        //             cs.Write(inputByteArray, 0, inputByteArray.Length);
        //             cs.FlushFinalBlock();
        //             byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组   
        //             cs.Close();
        //             ms.Close();                   
        //             return cipherBytes;
        //         }
        //     }
        // }


        /// AES加密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量128位，首次为全0</param>
        /// <param name="strKey">加密密钥</param>
        /// <returns></returns>
        public byte[] AESEncrypt(byte[] inputdata, byte[] iv, byte[] Key)
        {
            //分组加密算法   
            SymmetricAlgorithm des = Rijndael.Create();//Rijndael----高级加密标准（Advanced Encryption Standard，AES)
            byte[] inputByteArray = inputdata;//得到需要加密的字节数组       
            //设置密钥及密钥向量
            des.Key = Key;
            des.IV = iv;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组   
                    cs.Close();
                    ms.Close();
                    return cipherBytes;
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="inputdata">输入的数据</param>
        /// <param name="iv">向量128</param>
        /// <param name="strKey">key</param>
        /// <returns></returns>
        public byte[] AESDecrypt(byte[] inputdata, byte[] iv, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey.Substring(0, 32));
            des.IV = iv;
            byte[] decryptBytes = new byte[inputdata.Length];
            using (MemoryStream ms = new MemoryStream(inputdata))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return decryptBytes;
        }

    }
}
