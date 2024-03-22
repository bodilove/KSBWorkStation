using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
 
    /// <summary>
    /// 加密、解密帮助类
    /// </summary>
    public static class DESEncrypt
    {
        private static string Key = "kashibao";//再次加密Key
        private static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return mD5CryptoServiceProvider.ComputeHash(original);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="MingWen"></param>
        /// <param name="KeyString"></param>
        /// <returns></returns>
        public static string Encrypt(string MingWen, string KeyString)
        {
            byte[] bytes = Encoding.Default.GetBytes(MingWen);
            byte[] bytes2 = Encoding.Default.GetBytes(KeyString + Key);
            return Convert.ToBase64String(new TripleDESCryptoServiceProvider
            {
                Key = DESEncrypt.MakeMD5(bytes2),
                Mode = CipherMode.ECB
            }.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="MiWen">解密字符串</param>
        /// <param name="KeyString">解密key ；解密key 要与加密key一致</param>
        /// <returns></returns>
        public static string Decrypt(string MiWen, string KeyString)
        {
            string result = "";
            try
            {
                byte[] array = Convert.FromBase64String(MiWen);
                byte[] bytes = Encoding.Default.GetBytes(KeyString + Key);//key再次加Key
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = DESEncrypt.MakeMD5(bytes);
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                result = Encoding.Default.GetString(tripleDESCryptoServiceProvider.CreateDecryptor().TransformFinalBlock(array, 0, array.Length));
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message,"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
