using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ltchina.Core.Weixin.Entity;
using Ltchina.Core.Weixin.CommonAPI;
using Ltchina.Core.Weixin.HttpUtility;

namespace Ltchina.Core.Weixin.AdvanceAPI
{
    /// <summary>
    /// 获取OAuth AccessToken的结果
    /// 如果错误，返回结果{"errcode":40029,"errmsg":"invalid code"}
    /// </summary>
    public class OAuthAccessTokenResult : WxJsonResult
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }

    /// <summary>
    /// 通过OAuth的获取到的用户信息（snsapi_userinfo=scope）
    /// </summary>
    public class OAuthUserInfo
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// 作者注：其实这个格式称不上JSON，只是个单纯数组。
        /// </summary>
        public string[] privilege { get; set; }
    }

    // <summary>
    /// 应用授权作用域
    /// </summary>
    public enum OAuthScope
    {
        /// <summary>
        /// 不弹出授权页面，直接跳转，只能获取用户openid
        /// </summary>
        snsapi_base,
        /// <summary>
        /// 弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息
        /// </summary>
        snsapi_userinfo
    }

    public static class OAuth
    {
        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static string GetAuthorizeUrl(string appId, string redirectUrl, string state, OAuthScope scope, string responseType = "code")
        {
            var url =
                string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",
                                appId, redirectUrl.UrlEncode(), responseType, scope, state);

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetAccessToken(string appId, string secret, string code, string grantType = "authorization_code")
        {
            var url =
                string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}",
                                appId, secret, code, grantType);

            return CommonJsonSend.Send<OAuthAccessTokenResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 刷新access_token（如果需要）
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="grantType"></param>
        /// <returns></returns>
        public static OAuthAccessTokenResult RefreshToken(string appId, string refreshToken, string grantType = "refresh_token")
        {
            var url =
                string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type={1}&refresh_token={2}",
                                appId, grantType, refreshToken);

            return CommonJsonSend.Send<OAuthAccessTokenResult>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static OAuthUserInfo GetUserInfo(string accessToken, string openId)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", accessToken, openId);
            return CommonJsonSend.Send<OAuthUserInfo>(null, url, null, CommonJsonSendType.GET);
        }

        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId">用户的唯一标识</param>
        /// <returns></returns>
        public static WxJsonResult Auth(string accessToken, string openId)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", accessToken, openId);
            return CommonJsonSend.Send<WxJsonResult>(null, url, null, CommonJsonSendType.GET);
        }
    }
}