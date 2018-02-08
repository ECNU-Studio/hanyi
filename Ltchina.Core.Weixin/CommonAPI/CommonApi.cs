using Ltchina.Core.Weixin.HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ltchina.Core.Weixin.CommonAPI
{
    public static class CommonApi
    {
        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="grant_type">获取access_token填写client_credential</param>
        /// <param name="appid">第三方用户唯一凭证</param>
        /// <param name="secret">第三方用户唯一凭证密钥，既appsecret</param>
        /// <returns></returns>
        public static AccessTokenResult GetToken(string appid, string secret, string grant_type = "client_credential")
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                                    grant_type, appid, secret);

            AccessTokenResult result = Get.GetJson<AccessTokenResult>(url);
            return result;
        }
        /// <summary>
        /// 获得jsapi_ticket（有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket）
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static JsAPITicketResult GetJsApiTicket(string access_token) 
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token={0}", access_token);
            JsAPITicketResult result = Get.GetJson<JsAPITicketResult>(url);
            return result;
        }

        /// <summary>
        /// 获得卡券api_ticket（有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket）
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static JsAPITicketResult GetCardApiTicket(string access_token)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card", access_token);
            JsAPITicketResult result = Get.GetJson<JsAPITicketResult>(url);
            return result;
        }
    }
}