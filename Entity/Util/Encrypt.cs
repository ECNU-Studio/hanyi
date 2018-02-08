using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Util
{
    public class Encrypt
    {
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string message)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.Default.GetBytes(message);
            byte[] outStr = md5.ComputeHash(result);
            string md5string = BitConverter.ToString(outStr).Replace("-", "");
            return md5string;
        }
        public static string MD5Encrypt16(string message)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.Default.GetBytes(message);
            byte[] outStr = md5.ComputeHash(result);
            string md5string = BitConverter.ToString(outStr,4,8).Replace("-", "");          
            return md5string;
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="message">明文</param>
        /// <param name="key">密钥(8位长度的字符串)</param>
        /// <returns>密文</returns>
        public static string DesEncrypt(string message, string key)
        {
            DES des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);

            byte[] bytes = Encoding.UTF8.GetBytes(message);
            byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);

            StringBuilder ret = new StringBuilder();
            foreach (byte b in resultBytes)
            {
                //Format as hex 
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// Des解密
        /// </summary>
        /// <param name="message">密文</param>
        /// <param name="key">密钥(8位长度的字符串)</param>
        /// <returns>明文</returns>
        public static string DesDecrypt(string message, string key)
        {
            DES des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(key);
            des.IV = Encoding.UTF8.GetBytes(key);

            byte[] bytes = new byte[message.Length / 2];
            for (int x = 0; x < message.Length / 2; x++)
            {
                int i = (Convert.ToInt32(message.Substring(x * 2, 2), 16));
                bytes[x] = (byte)i;
            }

            byte[] resultBytes = des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(resultBytes.ToArray());
        }
    }
}
