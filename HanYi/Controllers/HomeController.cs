
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanYi.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
         
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePwd(string name, string returnurl = "/Home/ChangePwd")
        {
            ViewBag.returnurl = returnurl;
            ViewBag.name = name;
            return View();
        }
    }
}
