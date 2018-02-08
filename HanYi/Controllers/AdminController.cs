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
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取列表页的表格数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AdminList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                List<admin> list = new List<admin>();
                int total = 0;
                var temp = AdminDAL.getAdmin(pageIndex, pageSize);
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
        /// 添加管理员
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModelPostData(admin model, List<int> ids)
        {

            try
            {


                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    var teacheres = AdminDAL.findUserByTel(model.phone);
                    if (teacheres != null)
                    {
                        return Json(new { success = false, message = "手机号已注册" });
                    }
                    teacheres = AdminDAL.findUserByUsername(model.username);

                    if (teacheres != null)
                    {
                        return Json(new { success = false, message = "账号已存在" });
                    }

                    model.password = Encrypt.MD5Encrypt(model.password);
                    BaseDAL.AddModel<admin>(model);

                }
                else
                {
                    var old = BaseDAL.getEntryById<admin, int>(model.id);
                    if (old == null)
                    {
                        return Json(new { success = false, message = "管理员不存在" });
                    }
                    old.name = model.name;
                    old.username = model.username;
                    old.phone = model.phone;
                    old.password = Encrypt.MD5Encrypt(model.password);
                    BaseDAL.EditEntry<admin>(old, "id");


                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 删除实体-班级
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelAdmin(List<int> ids)
        {
            try
            {
                var res = AdminDAL.delAdmin(ids);
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
                admin model = BaseDAL.getEntryById<admin, int>(id);
                model.password = Encrypt.MD5Encrypt("abc123");
                BaseDAL.EditEntry<admin>(model, "id");
                return Json(new { success = true, message = "重置成功" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult PWChange(string model = "")
        {
            try
            {

                JObject obj = JObject.Parse(model);
                string oldpass = CommonBLL.GetJsonValue<string>(obj, "oldpass");
                string repass = CommonBLL.GetJsonValue<string>(obj, "repass");
                if (admin.password == Encrypt.MD5Encrypt(oldpass))
                {
                    admin.password = Encrypt.MD5Encrypt(repass);
                    BaseDAL.EditEntry<admin>(admin, "id");
                    return Json(new { success = true, message = "重置成功" });
                }
                else
                {
                    return Json(new { success = false, message = "原始密码不正确" });

                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}