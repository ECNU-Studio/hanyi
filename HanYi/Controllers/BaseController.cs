using Entity.DAL;
using Entity.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanYi.Controllers
{
    public class BaseController : Controller
    {
        private static readonly string PUBLISH_URL_EN = ConfigurationManager.AppSettings["PublishUrlEN"];
        /// <summary>
        /// 当前登录用户
        /// </summary>
        /// <returns></returns>
        protected users user;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        /// <returns></returns>
        protected teacheres teacher;
        /// <summary>
        /// 当前登录 管理员
        /// </summary>
        protected admin admin;
        /// <summary>
        /// 是否有提示
        /// </summary>
        protected bool hasTip = false; 
        /// <summary>
        /// 是否有提示-培训师
        /// </summary>
        protected bool hasTip_T = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            //获得当前URL的Controller和Action
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            string url = HttpContext.Request.RawUrl;
            ViewBag.url = url;

            int userID = 0;

            if (controller.ToLower() == "sskkpc")
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    int.TryParse(System.Web.HttpContext.Current.User.Identity.Name.Substring(7), out userID);
                    if (userID > 0)
                    {
                        user = BaseDAL.getEntryById<users, int>(userID);
                        if (user == null)
                        {
                            response.Redirect("/Account/LoginPC?returnurl=" + Url.Encode(url));
                            filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                        }
                        else
                        {
                            //if (!user.dayfirst.HasValue)
                            //{
                            //    user.dayfirst = DateTime.Now;
                            //    user.daybefor = DateTime.Now;
                            //}
                            //else if ((DateTime.Now - user.daybefor.Value).Days > 1)
                            //{
                            //    user.dayfirst = DateTime.Now;
                            //    user.daybefor = DateTime.Now;
                            //}
                            //else
                            //{
                            //    user.daybefor = DateTime.Now;

                            //}
                            //user.total_day = (user.daybefor.Value - user.dayfirst.Value).Days;
                            //BaseDAL.EditEntry<users>(user, "id");
                        }
                    }
                }
                else
                {
                    response.Redirect("/Account/LoginPC?returnurl=" + Url.Encode(url));
                    filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                }
            }
            else if (controller.ToLower() == "mobileuser" && (action.ToLower() == "guidelines" || action.ToLower() == "intro" || action.ToLower() == "cartoon"))
            {
                //不需要登录的前端页面
            }
            else
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    string[] ids = System.Web.HttpContext.Current.User.Identity.Name.Split('$');
                    switch (ids[0])
                    {
                        case "user":
                            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name.Substring(5), out userID);
                            if (userID > 0)
                            {
                                user = BaseDAL.getEntryById<users, int>(userID);
                                if (user == null || (controller.ToLower() != "mobileuser" && controller.ToLower() != "oauth"))
                                {
                                    if (controller.ToLower() == "mobileteacher")
                                    {
                                        response.Redirect("/Account/LoginTeacher?returnurl=" + Url.Encode(url));
                                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                    }  
                                    else
                                    {
                                        response.Redirect("/Account/Login?returnurl=" + Url.Encode(url));
                                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                    }   
                                }
                                else
                                {
                                    if (!user.dayfirst.HasValue)
                                    {
                                        user.dayfirst = DateTime.Now;
                                        user.daybefor = DateTime.Now;
                                    }
                                    else if ((DateTime.Now - user.daybefor.Value).Days > 1)
                                    {
                                        user.dayfirst = DateTime.Now;
                                        user.daybefor = DateTime.Now;
                                    }
                                    else
                                    {
                                        user.daybefor = DateTime.Now;

                                    }
                                    user.total_day = (user.daybefor.Value - user.dayfirst.Value).Days;
                                    BaseDAL.EditEntry<users>(user, "id");
                                    if (user.new_ans)
                                    {
                                        hasTip = true;
                                    }
                                }
                            }
                            break;
                        case "teacher":
                            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name.Substring(8), out userID);
                            if (userID > 0)
                            {
                                teacher = BaseDAL.getEntryById<teacheres, int>(userID);
                                if (teacher == null || (controller.ToLower() != "mobileteacher" && controller.ToLower() != "oauth"))
                                {
                                    if (controller.ToLower() == "mobileuser")
                                    {
                                        response.Redirect("/Account/LoginUser?returnurl=" + Url.Encode(url));
                                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                    }
                                    else
                                    {
                                        response.Redirect("/Account/Login?returnurl=" + Url.Encode(url));
                                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                    }   
                                }
                                else
                                {
                                    if (teacher.new_ans)
                                    {
                                        hasTip_T = true;
                                    }
                                }
                            }
                            break;
                        case "admin":
                            int.TryParse(System.Web.HttpContext.Current.User.Identity.Name.Substring(6), out userID);
                            if (userID > 0)
                            {
                                admin = BaseDAL.getEntryById<admin, int>(userID);
                                if (admin == null)
                                {
                                    response.Redirect("/Account/Login?returnurl=" + Url.Encode(url));
                                    filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                }
                                else
                                {
                                    ViewBag.name = admin.name;
                                }
                                if (controller.ToLower() == "mobileuser"){
                                    response.Redirect("/Account/LoginUser?returnurl=" + Url.Encode(url));
                                    filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                }

                                else if (controller.ToLower() == "mobileteacher")
                                {
                                    response.Redirect("/Account/LoginTeacher?returnurl=" + Url.Encode(url));
                                    filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                                }
                                    
                            }
                            break;
                    }

                }
                else
                {
                    if (controller.ToLower() == "mobileuser")
                    {
                        if (!(action.ToLower() == "guidelines" || action.ToLower() == "intro" || action.ToLower() == "cartoon"))
                        {
                            response.Redirect("/Account/LoginUser?returnurl=" + Url.Encode(url));
                            filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                        }
                    }
                    else if (controller.ToLower() == "mobileteacher")
                    {
                        response.Redirect("/Account/LoginTeacher?returnurl=" + Url.Encode(url));
                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                    }
                    else
                    {
                        response.Redirect("/Account/Login?returnurl=" + Url.Encode(url));
                        filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                    }
                }
            }
            ViewBag.hasTip = hasTip;
            ViewBag.hasTip_T = hasTip_T;
            if (HttpContext.Request.Url.AbsoluteUri.IndexOf("localhost") < 0)
            {
                if (user != null && user.language == 2)
                {
                    if (controller.ToLower() == "mobileuser")
                    {
                        if (!(action.ToLower() == "guidelines" || action.ToLower() == "intro" || action.ToLower() == "cartoon"))
                        {
                            response.Redirect(PUBLISH_URL_EN + "/" + url);
                            filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                        }
                    }
                }
                if (teacher != null && teacher.language == 2)
                {
                    if (controller.ToLower() == "mobileuser")
                    {
                        if (!(action.ToLower() == "guidelines" || action.ToLower() == "intro" || action.ToLower() == "cartoon"))
                        {
                            response.Redirect(PUBLISH_URL_EN + "/" + url);
                            filterContext.Result = new ContentResult() { Content = "{code:9999,message:'no login'}", ContentEncoding = System.Text.Encoding.UTF8 };
                        }
                    }
                }
            }
        }

    }
}