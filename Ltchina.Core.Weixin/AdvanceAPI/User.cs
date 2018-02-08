using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;

namespace Ltchina.Core.Weixin.AdvanceAPI
{

    /// <summary>
    /// 高级接口获取的用户信息
    /// </summary>
    public class UserInfoJson
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public long subscribe_time { get; set; }
    }

    public class OpenIdResultJson : WxJsonResult
    {
        public int total { get; set; }
        public int count { get; set; }
        public OpenIdResultJson_Data data { get; set; }
        public string next_openid { get; set; }
    }

    public class OpenIdResultJson_Data
    {
        public List<string> openid { get; set; }
    }

    public static class UserInfo
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openId">普通用户的标识，对当前公众号唯一</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public static UserInfoJson Info(string accessToken, string openId, Language lang = Language.zh_CN)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}",
                accessToken, openId, lang.ToString());
            return HttpUtility.Get.GetJson<UserInfoJson>(url);

            //错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
            //{"errcode":40013,"errmsg":"invalid appid"}
        }

        /// <summary>
        /// 获取关注着OpenId信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="nextOpenId"></param>
        /// <returns></returns>
        public static OpenIdResultJson Get(string accessToken, string nextOpenId)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}",
                accessToken);
            if (!string.IsNullOrEmpty(nextOpenId))
            {
                url += "&next_openid=" + nextOpenId;
            }
            return HttpUtility.Get.GetJson<OpenIdResultJson>(url);
        }

        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="id"></param>
        /// <param name="name">备注名字（30个字符以内）</param>
        /// <returns></returns>
        public static WxJsonResult Remark(string accessToken, string openid, string remark)
        {
            var urlFormat = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}";
            var data = new
            {
                openid = openid,
                remark = remark
            };
            return CommonJsonSend.Send(accessToken, urlFormat, data);
        }


        /// <summary>
        /// 验证openid是否有效
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool isOpenIdValid(string openid, long weixinuid, string AppID, string AppSecret)
        {
            try
            {
                string access_token = AccessToken.GetAccessToken(AppID, AppSecret);
                UserInfoJson info = Info(access_token, openid);
                if (info == null) return false;
                else return true;
            }
            catch
            {
                return false;
            }
        }
    }
}