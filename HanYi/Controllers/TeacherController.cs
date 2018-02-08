using Entity.DAL;
using Entity.Entity;
using Entity.Util;
using HanYi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanYi.Controllers
{
    public class TeacherController : BaseController
    {
        //
        // GET: /Teacher/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeacherList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                List<teacheres> list = new List<teacheres>();
                int total = 0;
                var temp = TeacheresDAL.getCompanys(pageIndex, pageSize);
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
        /// 添加培训师
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ModelPostData(teacheres model)
        {
            try
            {
                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    model.state = true;
                    model.password = Encrypt.MD5Encrypt(model.password);
                   var teacheres= TeacheresDAL.findUserByPhone(model.phone);
                   if (teacheres != null)
                   {
                       return Json(new { success = false, message = "手机号已注册" });
                   }
                   teacheres= TeacheresDAL.findUserByUsername(model.username);

                   if (teacheres != null)
                    {
                        return Json(new { success = false, message = "账号已存在" });
                    }
                    BaseDAL.AddModel<teacheres>(model);
                }
                else
                {
                    var entity = BaseDAL.getEntryById<teacheres, long>(model.id);
                    entity.username = model.username;          
                    if(model.password!=null)
                    entity.password = CommonBLL.MD5Encrypt(model.password);
                    entity.email = model.email;
                    entity.name = model.name;
                    entity.phone = model.phone;
                    entity.weixin = model.weixin;
                    entity.header = model.header;
                    entity.cv = model.cv;
                    entity.introduce = model.introduce;
                    BaseDAL.EditEntry<teacheres>(entity, "id");
                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        /// <summary>
        /// 删除实体-培训师
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelTeachers(List<int> ids)
        {
            try
            {
                var res = TeacheresDAL.delTeacheres(ids);
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
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
 
        public JsonResult PWSet(int id)
        {
            try
            {
                teacheres model = BaseDAL.getEntryById<teacheres, int>(id);
                model.password = Encrypt.MD5Encrypt("abc123");
                BaseDAL.EditEntry<teacheres>(model, "id");
                return Json(new { success = true, message = "重置成功" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult TeacherInfo(int? id)
        {
            if (id.HasValue)
            {
                teacheres model = BaseDAL.getEntryById<teacheres,int>(id.Value);
                ViewBag.model = model;
            }
            else
            {
                return Redirect("/Teacher/Index");
            }
            ViewBag.teacherid = id;
            return View();
        }

         /// <summary>
        /// 获取列表页的表格数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CourseList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int teacherid = CommonBLL.GetJsonValue<int>(obj, "teacherid");
                List<courses> list = new List<courses>();
                int total = 0;
                var temp = CoursesDAL.getCourses(teacherid,pageIndex, pageSize);
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
    
    }
}