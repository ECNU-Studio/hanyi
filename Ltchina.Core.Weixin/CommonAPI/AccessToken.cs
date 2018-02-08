using Ltchina.Core.Weixin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ltchina.Core.Weixin.CommonAPI
{
    /// <summary>
    /// access_token请求后的JSON返回格式
    /// </summary>
    public class AccessTokenResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// JSAPI Ticket 请求后的JSON返回格式
    /// </summary>
    public class JsAPITicketResult : WxJsonResult
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }
    }

    public class AccessToken
    {
        public static string GetAccessToken(string appid, string AppSecret) 
        {
            try
            {
                string key = string.Format("{0}:accesstion", appid);
                string access_token = "";
                if (string.IsNullOrEmpty(access_token) || access_token == "null")
                {
                    AccessTokenResult ret = CommonApi.GetToken(appid, AppSecret);
                    access_token = ret.access_token;
                 }
                return access_token;
            }
            catch
            {
                return "";
            }
        }

        public static string GetJsAPITicket(string appid, string AppSecret)
        {
            try
            {
                string key = string.Format("{0}:accesstionjsapiticket", appid);
                string ticket = "";
                if (string.IsNullOrEmpty(ticket) || ticket == "null")
                {
                    JsAPITicketResult ret = CommonApi.GetJsApiTicket(GetAccessToken(appid, AppSecret));
                    ticket = ret.ticket;
                 
                }
                return ticket;
            }
            catch
            {
                return "";
            }
        }

        public static string GetCardAPITicket(string appid, string AppSecret)
        {
            try
            {
                string key = string.Format("{0}:accesstioncardapiticket", appid);
                string ticket = "";
                if (string.IsNullOrEmpty(ticket) || ticket == "null")
                {
                    JsAPITicketResult ret = CommonApi.GetJsApiTicket(GetAccessToken(appid, AppSecret));
                    ticket = ret.ticket;
                }
                return ticket;
            }
            catch
            {
                return "";
            }
        }
    }
}