using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ltchina.Core.Weixin.AdvanceAPI;
using Ltchina.Core.Weixin;
using System.Configuration;
using Entity.DAL;
using Entity.Entity;
using System.Web.Security;
namespace HanYi.Controllers
{
    public class OauthController :Controller
    {

        [HttpPost]
        public JsonResult shareInit(string shareUrl)
        {
            try
            {
                //用于分享的一些参数设置
                string tempurl = shareUrl;

                //用于分享的一些参数设置
                //策划
                //string appid = "wxd6db8e13f2bd2cc2";
                //string appsecret = "8a2db93812d7d78cc5e9af295eff05e6";
                //服务
                string appid = APPID;
                string appsecret = APPSECRET;

                long timestamp = Ltchina.Core.Weixin.Helper.DateTimeHelper.GetWeixinDateTime(DateTime.Now);
                string nonceStr = "1OpUGAx67YMgOSjj";
                string jsapi_ticket = Ltchina.Core.Weixin.CommonAPI.AccessToken.GetJsAPITicket(appid, appsecret);
                string testurl = tempurl;

                string signstring = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, nonceStr, timestamp, tempurl);
                string signature = Encrypt.EncryptUtil(signstring, "SHA-1");
                ViewBag.appid = appid;
                ViewBag.timestamp = timestamp;
                ViewBag.nonceStr = nonceStr;
                ViewBag.signature = signature;
                return Json(new { success = true, appid = appid, timestamp = timestamp, nonceStr = nonceStr, signature = signature });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        private static readonly string PUBLISH_URL = ConfigurationManager.AppSettings["PublishUrl"];
        private static readonly string APPID = ConfigurationManager.AppSettings["APPID"];
        private static readonly string APPSECRET = ConfigurationManager.AppSettings["APPSECRET"];
        private static readonly string DOMAIN = ConfigurationManager.AppSettings["DOMAIN"];

        /// <summary>
        /// 网页授权,跳微信授权页面
        /// </summary>
        /// <returns></returns>
        public ActionResult oauth(string redirecturl, string cookiename)
        {
            if (redirecturl.Contains("from") && redirecturl.Contains("isappinstalled"))
            {
                int index = redirecturl.IndexOf("from");
                redirecturl = redirecturl.Substring(0, index);
            }
            string url = OAuth.GetAuthorizeUrl(APPID,
                PUBLISH_URL + "/Oauth/grantback?cookiename=" + cookiename,
                Server.UrlEncode(redirecturl), OAuthScope.snsapi_userinfo);
            return Redirect(url);
        }

        /// <summary>
        /// 微信授权回调，设置cookie
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult grantback(string code, string state, string cookiename)
        {
            //RedisHelper.SetKey("sxg:test:weixin", code + "|" + state + "|" + cookiename);
            string retcode = "";
            string idcookie = "";

            string url = Server.UrlDecode(state);
            try
            {
                if (string.IsNullOrEmpty(code))//用户不同意授权
                {
                    return Redirect(url );
                }
                OAuthAccessTokenResult ret = OAuth.GetAccessToken(APPID, APPSECRET, code);
                retcode = ret.errcode.ToString();
                if (ret.errcode != 0)//获取AccessToken失败
                {
                    return Redirect(url);
                }
                OAuthUserInfo user = OAuth.GetUserInfo(ret.access_token, ret.openid);

                idcookie = System.Web.HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(idcookie))
                {
                    HttpCookie cookie = Request.Cookies[cookiename];
                    if (cookie != null)
                    {
                        idcookie = cookie.Value;
                    }
                }

                if (!string.IsNullOrEmpty(idcookie))
                {
                    FormsAuthentication.SetAuthCookie(idcookie, true);

                    int userID = 0;
                    string[] ids = idcookie.Split('$');
                    switch (ids[0])
                    {
                        case "user":
                            int.TryParse(idcookie.Replace("user$", ""), out userID);
                            if (userID > 0)
                            {
                                var users = BaseDAL.getEntryById<users, int>(userID);
                                if (users != null)
                                {
                                    users.openid = user.openid;
                                    //users.name = user.nickname;
                                    users.photo = user.headimgurl;
                                    BaseDAL.EditEntry<users>(users, "id");
                                }
                            }
                            break;
                        case "teacher":
                            int.TryParse(idcookie.Replace("teacher$", ""), out userID);

                            if (userID > 0)
                            {
                                teacheres teacher = BaseDAL.getEntryById<teacheres, int>(userID);
                                if (teacher != null)
                                {

                                    teacher.openid = user.openid;
                                    // teacher.name = user.nickname;
                                    teacher.header = user.headimgurl;
                                    BaseDAL.EditEntry<teacheres>(teacher, "id");

                                }
                            }
                            break;

                    }
                }
                
                
                //SetCookie(cookiename, idcookie, DOMAIN);
                if (url.IndexOf("?") > -1)
                {
                    url = url + "&curOpenid="  +ret.openid;
                }
                else
                {
                    url = url + "?curOpenid="   + ret.openid;
                }
                return Redirect(url);
            }
            catch(Exception e)
            {
                return Redirect(url );
            }
        }

        /// <summary>
        /// 设置openid相关cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="openid"></param>
        /// <param name="domain"></param>
        [NonAction]
        public void SetCookie(string name, string openid, string domain)
        {
            HttpCookie cookie = Request.Cookies[name];
            if (cookie == null) cookie = new HttpCookie(name);
            cookie.Value = openid;
            cookie.Expires = DateTime.Now.AddMonths(12);
            cookie.Domain = domain;
            Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 网页授权,跳微信授权页面-报名跳转用
        /// </summary>
        /// <returns></returns>
        public ActionResult oauthUser(string redirecturl )
        {
            string url = OAuth.GetAuthorizeUrl(APPID,
                PUBLISH_URL + "/Oauth/grantbackUser",
                Server.UrlEncode(redirecturl), OAuthScope.snsapi_userinfo);
            return Redirect(url);
        }

        /// <summary>
        /// 微信授权回调，设置cookie
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult grantbackUser(string code, string state)
        {
            string retcode = "";


            string url = Url.Action("LoginUser", "Account");
            try
            {
                if (string.IsNullOrEmpty(code))//用户不同意授权
                {
                    return Redirect(url);
                }
                OAuthAccessTokenResult ret = OAuth.GetAccessToken(APPID, APPSECRET, code);
                retcode = ret.errcode.ToString();
                if (ret.errcode != 0)//获取AccessToken失败
                {
                    return Redirect(url);
                }
                OAuthUserInfo user = OAuth.GetUserInfo(ret.access_token, ret.openid);

                users users = UsersDAL.getUserInfobyOpenId(user.openid);
                if (users == null)
                    users = new users();
                users.companyid = 1;
                users.openid = user.openid;
                users.name = user.nickname;
                users.password = Encrypt.MD5Encrypt("abc123");
                users.photo = user.headimgurl;
                if (users.id > 0)
                    BaseDAL.EditEntry<users>(users, "id");
                else
                    BaseDAL.AddModel<users>(users);

                FormsAuthentication.SetAuthCookie("user$" + users.id, true);
                url = Url.Action("Index", "MobileUser");

                return Redirect(url);
            }
            catch (Exception e)
            {
                return Redirect(url);
            }
        }
	}
}