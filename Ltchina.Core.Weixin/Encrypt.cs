using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ltchina.Core.Weixin
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

        /// <summary>
        /// 生成唯一字符串(16位)
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 生成唯一数字（19位）
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntID()
        {

            byte[] buffer = Guid.NewGuid().ToByteArray();

            return BitConverter.ToInt64(buffer, 0);

        }

        /// <summary>
        /// 生成8位字符串，重复概率低
        /// </summary>
        /// <returns></returns>
        public static string GenUniqueString()
        {
            string KeleyiStr = "23456789abcdefghijkmnpqrstuvwxyz";
            char[] rtn = new char[8];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 8; i++)
            {
                rtn[i] = KeleyiStr[((ba[i] + ba[8 + i]) % 31)];
            }
            return "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5] + rtn[6] + rtn[7];
        }

        /// <summary>
        /// 生成6位字符串，重复概率低
        /// </summary>
        /// <returns></returns>
        public static string GenUniqueStringSix()
        {
            string KeleyiStr = "23456789abcdefghijkmnpqrstuvwxyz";
            char[] rtn = new char[6];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 6; i++)
            {
                rtn[i] = KeleyiStr[((ba[i] + ba[6 + i]) % 31)];
            }
            return "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5];
        }

        // <summary>
        /// 生成6位数字，重复概率低
        /// </summary>
        /// <returns></returns>
        public static string GenUniqueNumSix()
        {
            string KeleyiStr = "012345678998765432101234567899876";
            char[] rtn = new char[6];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 6; i++)
            {
                rtn[i] = KeleyiStr[((ba[i] + ba[6 + i]) % 31)];
            }
            return "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5];
        }

        /// <summary>
        /// 生成8位字符串，重复概率低
        /// </summary>
        /// <returns></returns>
        public static string GenUniqueNumEight()
        {
            string KeleyiStr = "01234567898765432101234567898765";
            char[] rtn = new char[8];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < 8; i++)
            {
                rtn[i] = KeleyiStr[((ba[i] + ba[8 + i]) % 31)];
            }
            return "" + rtn[0] + rtn[1] + rtn[2] + rtn[3] + rtn[4] + rtn[5] + rtn[6] + rtn[7];
        }

        public static String EncryptUtil(String strSrc, String encName)
        {
            String strDes = null;
            byte[] bt = new byte[0];
            try
            {
                bt = Encoding.UTF8.GetBytes(strSrc);
            }
            catch (Exception)
            {
                return null;
            }
            try
            {
                if (string.IsNullOrEmpty(encName))
                {
                    encName = "SHA-256";
                }
                byte[] tmpByte;
                switch (encName)
                {
                    case "SHA-1":
                        SHA1 sha1 = new SHA1CryptoServiceProvider();
                        tmpByte = sha1.ComputeHash(bt);
                        sha1.Clear();
                        break;
                    case "MD5":
                        MD5 md5 = new MD5CryptoServiceProvider();
                        tmpByte = md5.ComputeHash(bt);
                        md5.Clear();
                        break;
                    case "SHA-256":
                    default:
                        SHA256 sha256 = new SHA256Managed();
                        tmpByte = sha256.ComputeHash(bt);
                        sha256.Clear();
                        break;
                }
                strDes = bytes2Hex(tmpByte); // to HexString
            }
            catch (Exception)
            {
                return null;
            }
            return strDes;
        }

        public static String bytes2Hex(byte[] bts)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bts.Length; i++)
            {
                sb.Append(bts[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
    
}
