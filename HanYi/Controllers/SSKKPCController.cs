using Entity.DAL;
using Entity.Entity;
using HanYi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HanYi.Controllers
{
    public class SSKKPCController : BaseController
    {
        //
        // GET: /SSKKPC/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <returns></returns>

        public ActionResult CourseList(int pageIndex, int pageSize)
        {
            try
            {

                List<classstudent> list = new List<classstudent>();
                int total = 0;
                var temp = ClassstudentDAL.getUserClass(user.id, pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //Dictionary<int, int> state = StudystateDAL.getStudentState(user.id);
                //Dictionary<int, bool> comment = CommentDAL.getUserCommentStatic(user.id);
                //ViewBag.state = state;
                //ViewBag.comment = comment;
                //获取分页信息
                ViewBag.total = total;
            }
            catch { }
            return View();

        }


        /// <summary>
        /// 班级详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ClassDetail(int id, int type = 0)
        {
            try
            {
                var modle = ClassesDAL.getClassById(id);
                ViewBag.classid = id;
                ViewBag.modle = modle;
                ViewBag.userid = user.id;
                ViewBag.username = user.name;
                ViewBag.type = type;

            }
            catch { }
            return View();

        }
	}
}