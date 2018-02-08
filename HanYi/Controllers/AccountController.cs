using Entity.DAL;
using Entity.Entity;
using Entity.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HanYi.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string PUBLISH_URL = ConfigurationManager.AppSettings["PublishUrl"];
        private static readonly string PUBLISH_URL_EN = ConfigurationManager.AppSettings["PublishUrlEN"];

        private static readonly string DOMAIN = ConfigurationManager.AppSettings["DOMAIN"];

        /// <summary>
        /// cookieName
        /// </summary>
        protected string cookiename = "hanyiopenid";
        public ActionResult Login(string returnurl = "")
        {
            if (returnurl == "")
            {
                returnurl = "/Enterprise";
            }

            ViewBag.returnurl = returnurl;
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostLogin(string name, string pwd)
        {
            var user = AdminDAL.login(name); ;
            if (user != null)
            {
                if (user.password == Encrypt.MD5Encrypt(pwd))
                {
                    string cookie = string.Format("admin${0}", user.id);//保存用户ID
                    FormsAuthentication.SetAuthCookie(cookie, true);

                    return Json(new { success = true, url = "/" });
                }
                else
                {
                    return Json(new { success = false, message = "密码不正确" });
                }
            }
            else
            {
                return Json(new { success = false, message = "用户不存在" });
            }
        }
        [HttpPost]
        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return Json(new { success = true });

        }

        public ActionResult LoginUser(string returnurl = "")
        {

            
             return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostLoginUser(string phone, string password)
        {
            var user = UsersDAL.login(phone);
            if (user != null)
            {
                if (user.password == Encrypt.MD5Encrypt(password))
                {
                    string cookies = string.Format("user${0}", user.id);//保存用户ID
                    FormsAuthentication.SetAuthCookie(cookies, true);
                    string urlTemp = "/MobileUser";
                    user.language = 1;
                    UsersDAL.EditEntry<users>(user, "id");
                    if (HttpContext.Request.Url.AbsoluteUri.IndexOf("localhost") < 0)
                    {
                        if (user.language == 1)
                        {
                            urlTemp = PUBLISH_URL + "/MobileUser";
                        }
                        else
                        {
                            urlTemp = PUBLISH_URL_EN + "/MobileUser";

                        }
                    }
                    if (string.IsNullOrEmpty(user.openid))
                    {
                        if (user.language == 1)
                        {
                            urlTemp = string.Format(Url.Action("oauth", "Oauth", new { redirecturl = PUBLISH_URL + "/MobileUser", cookiename = cookiename }));

                        }
                        else
                        {
                            urlTemp = string.Format(Url.Action("oauth", "Oauth", new { redirecturl = PUBLISH_URL_EN + "/MobileUser", cookiename = cookiename }));

                        }

                        HttpCookie cookie = Request.Cookies[cookiename];
                        if (cookie == null) cookie = new HttpCookie(cookiename);
                        cookie.Value = "user$" + user.id;
                        cookie.Expires = DateTime.Now.AddMonths(12);
                        cookie.Domain = DOMAIN;
                        Response.Cookies.Set(cookie);
                    }
                    return Json(new { success = true, url = urlTemp });
                }
                else
                {
                    return Json(new { success = false, message = "密码不正确" });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public ActionResult LoginTeacher(string returnurl="")
        {
            
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostLoginTeacher(string phone, string password)
        {
            var teacher = TeacheresDAL.login(phone);
            if (teacher != null)
            {
                if (teacher.password == Encrypt.MD5Encrypt(password))
                {
                    teacher.language = 1;
                    UsersDAL.EditEntry<teacheres>(teacher, "id");
                    string cookies = string.Format("teacher${0}", teacher.id);//保存用户ID
                    FormsAuthentication.SetAuthCookie(cookies, true);
                    string urlTemp = "/MobileTeacher";
                    if (string.IsNullOrEmpty(teacher.openid))
                    {
                        urlTemp = string.Format(Url.Action("oauth", "Oauth", new { redirecturl = PUBLISH_URL + "/MobileTeacher", cookiename = cookiename }));
                        HttpCookie cookie = Request.Cookies[cookiename];
                        if (cookie == null) cookie = new HttpCookie(cookiename);
                        cookie.Value = "teacher$" + teacher.id;
                        cookie.Expires = DateTime.Now.AddMonths(12);
                        cookie.Domain = DOMAIN;
                        Response.Cookies.Set(cookie);
                    }

                    return Json(new { success = true, url = urlTemp });
                }
                else
                {
                    return Json(new { success = false, message = "密码不正确" });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// 扫描授权
        /// </summary>
        /// <returns></returns>
        public ActionResult oauth()
        {
            var url = string.Format(Url.Action("oauthUser", "Oauth", new { redirecturl = PUBLISH_URL + "/MobileUser", cookiename = cookiename }));
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    url = Url.Action("Index", "MobileUser");
            //}
            return Redirect(url);
        }

        protected string cookienamePC = "hanyipcd";
        public ActionResult LoginPC(string returnurl = "")
        {
            if (returnurl == "")
            {
                returnurl = "/SSKKPC";
            }

            ViewBag.returnurl = returnurl;
            string cookie = Request.Cookies.AllKeys.Contains<string>(cookienamePC) ? Request.Cookies[cookienamePC].Value : "";

            if (!string.IsNullOrEmpty(cookie))
            {
                cookie = Encrypt.DesDecrypt(cookie, cookienamePC);
                if (cookie.Split('#').Length == 3)
                {
                    ViewBag.username = cookie.Split('#')[0];
                    ViewBag.pwd = cookie.Split('#')[1];
                    ViewBag.check = cookie.Split('#')[2];
                }
            }
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostLoginPC(string phone, string password, bool b)
        {
            var user = UsersDAL.login(phone);
            if (user != null)
            {
                if (user.password == Encrypt.MD5Encrypt(password))
                {
                    string cookies = string.Format("userPC${0}", user.id);//保存用户ID
                    FormsAuthentication.SetAuthCookie(cookies, true);
                    if (b)
                    {
                        HttpCookie c = new HttpCookie(cookienamePC, Encrypt.DesEncrypt(string.Format("{0}#{1}#{2}", phone, password, (b == true ? "checked" : "")), cookienamePC));
                        c.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Set(c);
                    }
                    else
                    {
                        HttpCookie c = new HttpCookie(cookienamePC, Encrypt.DesEncrypt(string.Format("{0}#{1}#{2}", phone, password, (b == true ? "checked" : "")), cookienamePC));
                        c.Expires = DateTime.Now.AddDays(-30);
                        Response.Cookies.Set(c);
                    }
                    string urlTemp = "/SSKKPC";
                    return Json(new { success = true, url = urlTemp });
                }
                else
                {
                    return Json(new { success = false, message = "密码不正确" });
                }
            }
            else
            {
                return Json(new { success = false, message = "用户不存在" });
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            //取消会话
            Session.Abandon();

            //删除Froms验证票证
            FormsAuthentication.SignOut();

            return RedirectToAction("LoginUser", "Account");
        }
	}
}