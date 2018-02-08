using Entity.Entity;
using Entity.DAL;
using HanYi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Entity.Util;

namespace HanYi.Controllers
{
    //企业管理模块
    public class EnterpriseController : BaseController
    {
       /// <summary>
       /// 列表页
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取列表页的表格数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EnterpriseList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                List<companys> list = new List<companys>();
                int total = 0;
                var temp = CompanysDAL.getCompanys(pageIndex, pageSize);
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
        /// 保存实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModelPostData(companys model)
        {
            try
            {
                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    //检查账号和手机好是否存在，用于登录
                    var companys = CompanysDAL.findCompanysByAccount(model.account);
                    if (companys != null)
                    {
                        return Json(new { success = false, message = "账号已存在" });
                    }
                    model.state = true;
                    BaseDAL.AddModel<companys>(model);
                }
                else
                {
                    var entity = BaseDAL.getEntryById<companys, long>(model.id);
                    entity.name = model.name;
                    entity.account = model.account;
                    if (model.password!=null)
                    entity.password = CommonBLL.MD5Encrypt(model.password);
                    entity.email = model.email;
                    entity.legalperson = model.legalperson;
                    entity.address = model.address;
                    entity.cover = model.cover;
                    BaseDAL.EditEntry<companys>(entity, "id");
                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 删除实体-公司
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelEnterprise(List<int> ids)
        {
            try
            {
                var res = CompanysDAL.delCompanys(ids);
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
        /// 企业相关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EnterpriseInfo(int? id)
        {
            if (id.HasValue)
            {
                var model = BaseDAL.getEntryById<companys, int>(id.Value);
                ViewBag.model = model;
            }
            else
            {
                return Redirect("/Enterprise/Index");
            }
            ViewBag.companyid = id;
            return View();
        }

        public ActionResult EnterpriseUserList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int companyid = CommonBLL.GetJsonValue<int>(obj, "companyid");
                List<users> list = new List<users>();
                int total = 0;
                var temp = UsersDAL.getCompanys(companyid, pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchUser");
            }
            catch { }
            return View();
        }

        public ActionResult EnterpriseCourseList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int companyid = CommonBLL.GetJsonValue<int>(obj, "companyid");
                List<classes> list = new List<classes>();
                int total = 0;
                var temp = ClassesDAL.getCLasses(companyid, pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchCourse");
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModelPostData_User(users model)
        {
            try
            {
                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    //检查账号和手机好是否存在，用于登录
                    var user = UsersDAL.findUserByUsername(model.username);
                    if(user != null)
                    {
                        return Json(new { success = false, message = "账号已存在" });
                    }
                    if (!string.IsNullOrEmpty(model.tel))
                    {
                        var usertel = UsersDAL.findUserByTel(model.tel);
                        if (usertel != null)
                        {
                            return Json(new { success = false, message = "手机号码已存在" });
                        }
                    }
                    model.state = true;
                    model.password = Encrypt.MD5Encrypt(model.password);
                    BaseDAL.AddModel<users>(model);
                }
                else
                {
                    var user = UsersDAL.getEntryById<users,int>(model.id);
                    if (user != null)
                    {
                        user.username = model.username;
                        user.name = model.name;
                        user.department = model.department;
                        user.tel = model.tel;
                        user.position = model.position;
                        user.email = model.email;
                        user.company = model.company;
                        BaseDAL.EditEntry<users>(user, "id");
                    }          
                   
                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 删除实体-公司
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelEnterpriseUser(List<int> ids)
        {
            try
            {
                var res = UsersDAL.delUsers(ids);
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
                users model = BaseDAL.getEntryById<users, int>(id);
                model.password = Encrypt.MD5Encrypt("abc123");
                BaseDAL.EditEntry<users>(model, "id");
                return Json(new { success = true, message = "重置成功" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    
	}
}