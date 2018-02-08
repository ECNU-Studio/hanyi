using Ltchina.Core.Weixin.CommonAPI;
using Ltchina.Core.Weixin.Helper;
using Ltchina.Core.Weixin.HttpUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    public class SendRedpack 
    {
        /// <summary>
        /// 随机字符串	nonce_str	是	5K8264ILTKCH16CQ2502SI8ZNMTM67VS	String(32)	随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名	sign	是	C380BEC2BFD727A4B6845133519F3AD6	String(32)	详见签名生成算法
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商户订单号	mch_billno	是	1.00001E+25	String(28)	商户订单号（每个订单号必须唯一）
		///	组成： mch_id+yyyymmdd+10位一天内不能重复的数字。
		///	接口根据商户订单号支持重入， 如出现超时可再调用。
        /// </summary>
        public string mch_billno { get; set; }
        /// <summary>
        /// 商户号	mch_id	是	10000098	String(32)	微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 子商户号	sub_mch_id	否	10000090	String(32)	微信支付分配的子商户号，受理模式下必填
        /// </summary>
        public string sub_mch_id { get; set; }
        /// <summary>
        /// 公众账号appid	wxappid	是	wx8888888888888888	String(32)	商户appid
        /// </summary>
        public string wxappid { get; set; }
        /// <summary>
        /// 提供方名称	nick_name	是	天虹百货	String(32)	提供方名称
        /// </summary>
        public string nick_name { get; set; }
        /// <summary>
        /// 商户名称	send_name	是	天虹百货	String(32)	红包发送者名称
        /// </summary>
        public string send_name { get; set; }
        /// <summary>
        ///用户openid	re_openid	是	oxTWIuGaIt6gTKsQRLau2M0yL16E	String(32)	接受收红包的用户 用户在wxappid下的openid
        /// </summary>
        public string re_openid { get; set; }
        /// <summary>
        ///付款金额	total_amount	是	1000	int	付款金额，单位分
        /// </summary>
        public int total_amount { get; set; }
        /// <summary>
        /// 最小红包金额	min_value	是	1000	int	最小红包金额，单位分
        /// </summary>
        public int min_value { get; set; }
        /// <summary>
        /// 最大红包金额	max_value	是	1000	int	最大红包金额，单位分（ 最小金额等于最大金额： min_value=max_value =total_amount）
        /// </summary>
        public int max_value { get; set; }
        /// <summary>
        /// 红包发放总人数	total_num	是	1	int	红包发放总人数 total_num=1
        /// </summary>
        public int total_num { get; set; }
        /// <summary>
        /// 红包祝福语	wishing	是	感谢您参加猜灯谜活动，祝您元宵节快乐！	String(128)	红包祝福语
        /// </summary>
        public string wishing { get; set; }
        /// <summary>
        /// Ip地址	client_ip	是	192.168.0.1	String(15)	调用接口的机器Ip地址
        /// </summary>
        public string client_ip { get; set; }
        /// <summary>
        /// 活动名称	act_name	是	猜灯谜抢红包活动	String(32)	活动名称
        /// </summary>
        public string act_name { get; set; }
        /// <summary>
        /// 备注	remark	是	猜越多得越多，快来抢！	String(256)	备注信息
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 商户logo的url	logo_imgurl	否	https://wx.gtimg.com/mch/img/ico-logo.png	String(128)	商户logo的url
        /// </summary>
        public string logo_imgurl { get; set; }
        /// <summary>
        /// 分享文案	share_content	否	快来参加猜灯谜活动	String(256)	分享文案
        /// </summary>
        public string share_content { get; set; }
        /// <summary>
        /// 分享链接	share_url	否	http://www.qq.com	String(128)	分享链接
        /// </summary>
        public string share_url { get; set; }
        /// <summary>
        /// 分享的图片	share_imgurl	否	https://wx.gtimg.com/mch/img/ico-logo.png	String(128)	分享的图片url
        /// </summary>
        public string share_imgurl { get; set; }
    }

    public class RedPackResult 
    {
        /// <summary>
        /// 返回状态码	return_code	是	SUCCESS	String(16)	SUCCESS/FAIL
        /// 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息	return_msg	否	签名失败	String(128)	返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        public string return_msg { get; set; }

        //以下字段在return_code为SUCCESS的时候有返回
				
        /// <summary>
        ///签名	sign	是	C380BEC2BFD727A4B6845133519F3AD6	String(32)	生成签名方式详见签名生成算法
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 业务结果	result_code	是	SUCCESS	String(16)	SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码	err_code	否	SYSTEMERROR	String(32)	错误码信息
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述	err_code_des	否	系统错误	String(128)	结果信息描述
        /// </summary>
        public string err_code_des { get; set; }

        //以下字段在return_code 和result_code都为SUCCESS的时候有返回
					
        /// <summary>
        /// 商户订单号	mch_billno	是	1.00001E+25	String(28)	商户订单号（每个订单号必须唯一）
        /// 组成： mch_id+yyyymmdd+10位一天内不能重复的数字
        /// </summary>
        public string mch_billno { get; set; }
        /// <summary>
        /// 商户号	mch_id	是	10000098	String(32)	微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 公众账号appid	wxappid	是	wx8888888888888888	String(32)	商户appid
        /// </summary>
        public string wxappid { get; set; }
        /// <summary>
        /// 用户openid	re_openid	是	oxTWIuGaIt6gTKsQRLau2M0yL16E	String(32)	接受收红包的用户
        /// 用户在wxappid下的openid
        /// </summary>
        public string re_openid { get; set; }
        /// <summary>
        /// 付款金额	total_amount	是	1000	int	付款金额，单位分
        /// </summary>
        public int total_amount { get; set; }
    }

    public class RedPack
    {
        private const string URL_FORMAT = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <returns></returns>
        public static RedPackResult SendRedPack(string wxappid, string apisecret,string re_openid, string mch_billno, string mch_id, string nick_name, string send_name,
            int total_amount,string wishing,string client_ip,string act_name,string remark,string logo_imgurl,string share_content,
            string share_url,string share_imgurl,string cerpath) 
        {
            SendRedpack pack = new SendRedpack();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            pack.mch_billno = mch_billno; dic.Add("mch_billno", mch_billno);
            pack.mch_id = mch_id; dic.Add("mch_id", mch_id);
            pack.wxappid = wxappid; dic.Add("wxappid", wxappid);
            pack.re_openid = re_openid; dic.Add("re_openid", re_openid);
            pack.nick_name = nick_name; dic.Add("nick_name", nick_name);
            pack.send_name = send_name; dic.Add("send_name", send_name);
            pack.total_amount = total_amount; dic.Add("total_amount", total_amount.ToString());
            pack.min_value = total_amount; dic.Add("min_value", total_amount.ToString());
            pack.max_value = total_amount; dic.Add("max_value", total_amount.ToString());
            pack.total_num = 1; dic.Add("total_num", "1");
            pack.wishing = wishing; dic.Add("wishing", wishing);
            pack.client_ip = client_ip; dic.Add("client_ip", client_ip);
            pack.act_name = act_name; dic.Add("act_name", act_name);
            pack.remark = remark; dic.Add("remark", remark);
            if (!string.IsNullOrEmpty(logo_imgurl)) 
            {
                pack.logo_imgurl = logo_imgurl; dic.Add("logo_imgurl", logo_imgurl);
            }
            if (!string.IsNullOrEmpty(share_content))
            {
                pack.share_content = share_content; dic.Add("share_content", share_content);
            }
            if (!string.IsNullOrEmpty(share_url))
            {
                pack.share_url = share_url; dic.Add("share_url", share_url);
            }
            if (!string.IsNullOrEmpty(share_imgurl))
            {
                pack.share_imgurl = share_imgurl; dic.Add("share_imgurl", share_imgurl);
            }
            pack.nonce_str = Encrypt.GenerateStringID(); dic.Add("nonce_str", pack.nonce_str);

            //第一步：对参数按照key=value的格式，并按照参数名ASCII字典序排序如下： 
            string stringA = CreateLinkString(dic);
            //第二步：拼接API密钥： 
            string stringSignTemp = string.Format("{0}&key={1}", stringA, apisecret);
            string sign = Encrypt.EncryptUtil(stringSignTemp, "MD5").ToUpper();
            pack.sign = sign;
            //string xml = XmlUtility.Serializer<SendRedpack>(pack);
            XDocument xml = EntityHelper.ConvertEntityToXml(pack);

            byte[] postData = Encoding.UTF8.GetBytes(xml.ToString());//编码，尤其是汉字，事先要看下抓取网页的编码方式
            X509Certificate cer = getCert(mch_id, cerpath);
            using (CertificateWebClient webClient = new CertificateWebClient(cer))
            {
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                byte[] responseData = webClient.UploadData(URL_FORMAT, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);
                if (!string.IsNullOrEmpty(srcString))
                {
                    xml = XDocument.Parse(srcString);
                    RedPackResult ret = new RedPackResult();
                    EntityHelper.FillEntityWithXml<RedPackResult>(ret, xml);
                    return ret;
                }
                return null;
            }
        }

        private static X509Certificate getCert(string mch_id,string cerpath) 
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
        public static string CreateLinkString(SortedDictionary<string, string> dicArray)
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
}
