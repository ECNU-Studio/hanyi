using Ltchina.Core.Weixin.Helper;
using Ltchina.Core.Weixin.HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public class EnterprisePay
    {
        private const string URL_FORMAT = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";

        public static EpayResult SendEnterprisePay(string mch_appid,string apisecret, string mchid, string partner_trade_no, 
            string openid, int amount, string desc,string spbill_create_ip, string cerpath) 
        {
            EpayPack pack = new EpayPack();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            pack.partner_trade_no = partner_trade_no; dic.Add("partner_trade_no", partner_trade_no);
            pack.mchid = mchid; dic.Add("mchid", mchid);
            pack.mch_appid = mch_appid; dic.Add("mch_appid", mch_appid);
            pack.openid = openid; dic.Add("openid", openid);
            pack.amount = amount.ToString(); dic.Add("amount", amount.ToString());
            pack.desc = desc; dic.Add("desc", desc);
            pack.spbill_create_ip = spbill_create_ip; dic.Add("spbill_create_ip", spbill_create_ip.ToString());
            pack.nonce_str = GenerateStringID(); dic.Add("nonce_str", pack.nonce_str);
            pack.check_name = "NO_CHECK"; dic.Add("check_name", pack.check_name);

            //第一步：对参数按照key=value的格式，并按照参数名ASCII字典序排序如下： 
            string stringA = CreateLinkString(dic);
            //第二步：拼接API密钥： 
            string stringSignTemp = string.Format("{0}&key={1}", stringA, apisecret);
            string sign = Encrypt.EncryptUtil(stringSignTemp, "MD5").ToUpper();
            pack.sign = sign;

            XDocument xml = EntityHelper.ConvertEntityToXml(pack);

            byte[] postData = Encoding.UTF8.GetBytes(xml.ToString());//编码，尤其是汉字，事先要看下抓取网页的编码方式
            X509Certificate cer = getCert(mchid, cerpath);
            using (CertificateWebClient webClient = new CertificateWebClient(cer))
            {
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webClient.UploadData(URL_FORMAT, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);
                if (!string.IsNullOrEmpty(srcString))
                {
                    xml = XDocument.Parse(srcString);
                    EpayResult ret = new EpayResult();
                    EntityHelper.FillEntityWithXml<EpayResult>(ret, xml);
                    return ret;
                }
                return null;
            }
        }

        /// <summary>
        /// 生成唯一字符串(16位)
        /// </summary>
        /// <returns></returns>
        private static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        private static X509Certificate getCert(string mch_id, string cerpath)
        {
            string cert = string.Format("{0}\\{1}apiclient_cert.p12", cerpath, mch_id);//证书存放的地址
            string password = mch_id;//证书密码 即商户号
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            return cer;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        private static string CreateLinkString(SortedDictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }
    }

    public class EpayResult 
    {
        // <summary>
        /// SUCCESS/FAIL
        ///此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }

        //以下字段在return_code为SUCCESS的时候有返回 

        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string mch_appid { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 微信支付分配的终端设备号，
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误码信息
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 结果信息描述
        /// </summary>
        public string err_code_des { get; set; }

        //以下字段在return_code 和result_code都为SUCCESS的时候有返回 

        /// <summary>
        /// 商户订单号，需保持唯一性
        /// </summary>
        public string partner_trade_no { get; set; }
        /// <summary>
        /// 企业付款成功，返回的微信订单号
        /// </summary>
        public string payment_no { get; set; }
        /// <summary>
        /// 企业付款成功时间
        /// </summary>
        public string payment_time { get; set; }
    }

    public class EpayPack 
    {
        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string mch_appid{get;set;}
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mchid{get;set;}
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str{get;set;}
        /// <summary>
        /// 签名
        /// </summary>
        public string sign{get;set;}
        /// <summary>
        /// 商户订单号，需保持唯一性
        /// </summary>
        public string partner_trade_no{get;set;}
        /// <summary>
        /// 商户appid下，某用户的openid
        /// </summary>
        public string openid{get;set;}
        /// <summary>
        /// NO_CHECK：不校验真实姓名 FORCE_CHECK：强校验真实姓名（未实名认证的用户会校验失败，无法转账） OPTION_CHECK：针对已实名认证的用户才校验真实姓名（未实名认证用户不校验，可以转账成功）
        /// </summary>
        public string check_name{get;set;}
        /// <summary>
        /// 企业付款金额，单位为分
        /// </summary>
        public string amount{get;set;}
        /// <summary>
        /// 企业付款操作说明信息。必填。
        /// </summary>
        public string desc{get;set;}
        /// <summary>
        /// 调用接口的机器Ip地址
        /// </summary>
        public string spbill_create_ip { get; set; }
    }
}