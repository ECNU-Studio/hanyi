using Entity.DAL;
using Entity.Entity;
using HanYi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HanYi.Controllers
{
    //课程模块
    public class CoursesController : BaseController
    {
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //List<teacheres> teachers = new List<teacheres>();
            //int teacherCount = 0;
            //try
            //{
            //    var temp = TeacheresDAL.getAllTeacher();
            //    if (temp != null)
            //    {
            //        teachers = temp.Item1;
            //        teacherCount = temp.Item2;
            //    }
            //}
            //catch { }
            //ViewBag.teachers = teachers;
            return View();
        }

        /// <summary>
        /// 获取列表页的表格数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CoursesList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                List<courses> list = new List<courses>();
                int total = 0;
                var temp = CoursesDAL.getCourses(pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "Search");
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 根据培训师的名称模糊匹配
        /// </summary>
        /// <param name="teachername"></param>
        /// <returns></returns>
        public JsonResult MatchTeacher(string model)
        {
            try
            {
                JObject obj = JObject.Parse(model);
                string teachername = CommonBLL.GetJsonValue<string>(obj, "teachername");
                List<teacheres> list = TeacheresDAL.MatchTeacher(teachername, 15);
                StringBuilder sbStr = new StringBuilder();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        sbStr.AppendFormat("<li data-id='{1}'>{0}</li>"
                        , item.name
                        , item.id
                        );
                    }
                }
                else
                {
                    sbStr.AppendFormat("<li data-item='no'>暂无相关信息</li>");
                }
                return Json(new { success = true, message = sbStr.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModelPostData(courses model)
        {
            try
            {
                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    model.state = true;
                    BaseDAL.AddModel<courses>(model);
                }
                else
                {
                    var old = BaseDAL.getEntryById<courses, int>(model.id);
                    if (old == null)
                    {
                        return Json(new { success = false, message = "课程不存在" });
                    }
                    old.name = model.name;
                    old.coursesabstract = model.coursesabstract;
                    old.teacherid = model.teacherid;
                    old.cover = model.cover;
                    //old.honor = model.honor;
                    old.abstractFile = model.abstractFile;
                    old.abstractFilesize = model.abstractFilesize;
                    old.abstractFilename = model.abstractFilename;
                    BaseDAL.EditEntry<courses>(old, "id");
                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CoursesCatalogAdd(List<catalog> ids)
        {
            try
            {
                var res = CoursesDAL.CoursesCatalogAdd(ids);
                if (!res)
                {
                    return Json(new { success = false, message = "刷新后重新操作" });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            
           
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelMain(List<int> ids)
        {
            try
            {
                var res = CoursesDAL.DelMain(ids);
                if (!res)
                {
                    return Json(new { success = false, message = "刷新后重新操作" });
                }
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// 删除实体-课程
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelCourses(List<int> ids)
        {
            try
            {
                var res = CoursesDAL.delCourses(ids);
                if (!res)
                {
                    return Json(new { success = false, message = "网络故障，刷新后重试" });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 公司相关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CoursesInfo(int? id)
        {
            courses model = new courses();
            if (id.HasValue)
            {
                model = CoursesDAL.GetItem(id.Value);
            }
            else
            {
                return Redirect("/Courses/Index");
            }
            ViewBag.model = model;
            ViewBag.coursesid = id;
            return View();
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetCatalog(string model)
        {
            JObject obj = JObject.Parse(model);
            int? id = CommonBLL.GetJsonValue<int?>(obj, "id");
            if (id.HasValue)
            {
              ViewBag.list = CatalogDAL.getCLasses(id.Value);
            }
            else
            {
                return Redirect("/Courses/Index");
            }
            return View();
        }


	}
}