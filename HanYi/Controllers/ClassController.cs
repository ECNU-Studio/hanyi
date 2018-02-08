using Entity.DAL;
using Entity.Entity;
using HanYi.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HanYi.Controllers
{
    public class ClassController : BaseController
    {
        //
        // GET: /Class/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ClassList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                List<classes> list = new List<classes>();
                int total = 0;
                var temp = ClassesDAL.getCLasses(pageIndex, pageSize);
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
        /// 根据课程的名称模糊匹配
        /// </summary>
        /// <param name="teachername"></param>
        /// <returns></returns>
        public JsonResult MatchCourse(string model)
        {
            try
            {
                JObject obj = JObject.Parse(model);
                string teachername = CommonBLL.GetJsonValue<string>(obj, "coursename");
                List<courses> list = CoursesDAL.MatchCourse(teachername, 15);
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
        /// 根据培企业名称模糊匹配
        /// </summary>
        /// <param name="teachername"></param>
        /// <returns></returns>
        public JsonResult MatchCompany(string model)
        {
            try
            {
                JObject obj = JObject.Parse(model);
                string teachername = CommonBLL.GetJsonValue<string>(obj, "companyname");
                List<companys> list = CompanysDAL.MatchCompany(teachername, 15);
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
        /// 根据公司id 获取 所有未添加用户
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public ActionResult CompanyUsers(int companyid, int type = 0, int classid = 0)
        {
            List<users> list = new List<users>();
            int total = 0;
            if(type == 0)
            {
                var temp = UsersDAL.getCompanys(companyid, 1, Int16.MaxValue);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
            }
            else
            {
                var temp = UsersDAL.getCompanysNot(companyid, 1, Int16.MaxValue, classid);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
            }
            ViewBag.list = list;
            ViewBag.total = total;
            return View();
        }

        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModelPostData(classes model, List<int> ids)
        {

            try
            {


                //id==0新增,否则编辑
                if (model.id == 0)
                {
                    model.state = true;
                    BaseDAL.AddModel<classes>(model);
                    if (ids != null)
                    {
                        List<classstudent> list = new List<classstudent>();
                        foreach (var id in ids)
                        {
                            classstudent cs = new classstudent();
                            cs.classid = model.id;
                            cs.userid = id;
                            list.Add(cs);
                        }
                        ClassstudentDAL.addClassstudent(list);
                    }


                }
                else
                {
                    var old = BaseDAL.getEntryById<classes, int>(model.id);
                    if (old == null)
                    {
                        return Json(new { success = false, message = "课程不存在" });
                    }
                    old.coursesid = model.coursesid;
                    old.schooltime = model.schooltime;
                    old.period = model.period;
                    old.address = model.address;
                    old.companyid = model.companyid;
                    old.hour = model.hour;
                    BaseDAL.EditEntry<classes>(old, "id");
                    if (ids != null)
                    {
                        List<classstudent> list = new List<classstudent>();
                        foreach (var id in ids)
                        {
                            classstudent cs = new classstudent();
                            cs.classid = model.id;
                            cs.userid = id;
                            list.Add(cs);
                        }
                        ClassstudentDAL.updateClassstudent(model.id, list);
                    }

                }
                return Json(new { success = true, id = model.id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdatePostData(classes model )
        {

            try
            {
                    var old = BaseDAL.getEntryById<classes, int>(model.id);
                    if (old == null)
                    {
                        return Json(new { success = false, message = "课程不存在" });
                    }
                    old.schooltime = model.schooltime;
                    old.period = model.period;
                    old.address = model.address;
                    old.hour = model.hour;
                    BaseDAL.EditEntry<classes>(old, "id");                    

                
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
        public JsonResult DelClasses(List<int> ids)
        {
            try
            {
                var res = ClassesDAL.delClasses(ids);
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
        /// 班级相关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ClassInfo(int? id, int? companyid, string coursename, string companyname)
        {
            if (id.HasValue)
            {

                var model = BaseDAL.getEntryById<classes, int>(id.Value);
                ViewBag.model = model;
            }
            else
            {
                return Redirect("/Class/Index");
            }
            ViewBag.classid = id;
            ViewBag.companyid = companyid;
            ViewBag.coursename = coursename;
            ViewBag.companyname = companyname;
            return View();
        }

        /// <summary>
        /// 班级用户列表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UserList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                string username = CommonBLL.GetJsonValue<string>(obj, "username");
                List<users> list = new List<users>();
                int total = 0;
                var temp = UsersDAL.getClassUser(classid, username, pageIndex, pageSize);
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
        /// <summary>
        /// 从班级中批量删除学生
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult delUser(List<int> ids, int classid)
        {

            try
            {
                var res = ClassstudentDAL.delUsers(classid, ids);
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
        /// 给某个课程添加用户
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public JsonResult addUser(List<int> ids, int classid)
        {

            try
            {
                if (ids != null)
                {
                    List<classstudent> list = new List<classstudent>();
                    foreach (var id in ids)
                    {
                        classstudent cs = new classstudent();
                        cs.classid = classid;
                        cs.userid = id;
                        list.Add(cs);
                    }
                    var res = ClassstudentDAL.updateClassstudent(classid, list);
                    if (!res)
                    {
                        return Json(new { success = false, message = "网络故障，刷新后重试" });
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 地址列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddressList(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                List<classaddress> list = new List<classaddress>();
                int total = 0;
                var temp = ClassaddressDAL.getClassAddress(classid,  pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchAddress");
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 添加上课地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult addAddress(classaddress model)
        {
            try
            {
                if (model.id > 0)
                {
                    classaddress ca = BaseDAL.getEntryById<classaddress,int>(model.id);
                    ca.period = model.period;
                    ca.datebegin = model.datebegin;
                    ca.address = model.address;
                    BaseDAL.EditEntry<classaddress>(ca, "id");
                }
                else
                {
                    BaseDAL.AddModel<classaddress>(model);
                }


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        /// <summary>
        /// 批量删除地址
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult delAddress(List<int> ids )
        {

            try
            {
                var res = ClassaddressDAL.delAddress( ids);
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
        /// 添加模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public JsonResult addClassModels(List<classmodels> model, int classid)
        {
            try
            {
                if (model == null)
                    model = new List<classmodels>();

                ClassmodelsDAL.removeUpdate(model,classid);
                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 课程模块列表
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public ActionResult ClassModelsList(int classid)
        {
            try
            {
                List<classmodels> list = ClassmodelsDAL.getCLasses(classid);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }


        /// <summary>
        /// 获取学生分数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchScore(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                string username = CommonBLL.GetJsonValue<string>(obj, "username_score");
                List<ScoresView> list = new List<ScoresView>();
                int total = 0;
                var temp = ScoresDAL.getCLassesScores(classid, username, pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchScore");
            }
            catch { }
            return View();
        }
        /// <summary>
        /// 保存学生成绩
        /// </summary>
        /// <param name="model"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public JsonResult SaveScore(List<scores> model, int classid)
        {
            try
            {

                ScoresDAL.SaveScore(classid,model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

      
        /// <summary>
        /// 获取学生进度
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchState(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                string username = CommonBLL.GetJsonValue<string>(obj, "username_state");
                List<users> listuser = null;
                List<classmodels> listcm = null;
                List<studystate> listss = null;
                List<comment> listcc = null;
                int total = 0;
                var temp = UsersDAL.getClassUser(classid, username, pageIndex, pageSize);
                if (temp != null)
                {
                    listuser = temp.Item1;
                    total = temp.Item2;
                }
                listcm = ClassmodelsDAL.getCLasses(classid);
                listss = StudystateDAL.getStateOnly(classid);
                listcc = CommentDAL.getCLasses(classid);
                ViewBag.listuser = listuser;
                ViewBag.listcm = listcm;
                ViewBag.listss = listss;
                ViewBag.listcc = listcc;  
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchState");
            }
            catch { }
            return View();
        }
      
        /// <summary>
        /// 保存学生状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="classid"></param>
        /// <returns></returns>
        public JsonResult SaveState(List<studystate> model, int classid)
        {
            try
            {
                StudystateDAL.SaveState(classid, model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        /// <summary>
        /// 获取班级排名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchClassRank(string model = "")
        {

            try
            {
                JObject obj = JObject.Parse(model);
                int pageIndex = CommonBLL.GetJsonValue<int>(obj, "pageIndex");
                int pageSize = CommonBLL.GetJsonValue<int>(obj, "pageSize");
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                string username = CommonBLL.GetJsonValue<string>(obj, "username_state");
                List<users> listuser = null;
                List<classmodels> listcm = null;
                List<studystate> listss = null;
                List<comment> listcc = null;
                List<ClassRankView> listrank = new List<ClassRankView>();
                int total = 0;
                var temp = UsersDAL.getClassUser(classid, username, 1, int.MaxValue);
                if (temp != null)
                {
                    listuser = temp.Item1;
                    total = temp.Item2;
                }
                listcm = ClassmodelsDAL.getCLasses(classid);
                listss = StudystateDAL.getStateOnly(classid);
                listcc = CommentDAL.getCLasses(classid);

                foreach(var user in listuser)
                {
                    ClassRankView crv = new ClassRankView();
                    crv.username = user.name;
                    crv.id = user.id;
                    crv.head = user.photo;
                    crv.list = new List<ClassRankView.classScore>();
                    var score = 0.0;
                    //foreach (var itemcm in listcm)
                    //{
                    //    var ss = listss.Where(p => p.userid == user.id && p.modelid == itemcm.id).FirstOrDefault();
                    //    if (ss != null && ss.isfinish.HasValue && ss.isfinish.Value)
                    //    {
                    //        if (itemcm.type == "勾选")
                    //        {
                    //            crv.gouxuan = "已完成（100）";
                    //            score += 100 * (itemcm.percent/100.0);
                                 
                    //        }
                    //        if (itemcm.type == "打分" && ss.score.HasValue)
                    //        {
                    //            crv.dafen = ss.score.Value.ToString("##.##");
                    //            score += ss.score.Value * (itemcm.percent / 100.0);
                               
                    //        }
                    //        if (itemcm.type == "测试" && ss.score.HasValue)
                    //        {
                    //            crv.ceshi = ss.score.Value.ToString("##.##");
                    //            score += ss.score.Value * (itemcm.percent / 100.0);
                                
                    //        }
                    //        if (itemcm.type == "问卷")
                    //        {
                    //            crv.wenjuan = "已完成（100）";
                    //            score += 100 * (itemcm.percent / 100.0);
                                
                    //        }
                          
                    //    }
                    //}

                    //var cc = listcc.Where(p => p.userid == user.id).FirstOrDefault();
                    //if (cc != null)
                    //{
                    //    //score += 100;// (cc.c1 + cc.c2 + cc.c3 + cc.c4 + cc.c5 + cc.c6 + cc.c7 + cc.c8 + cc.c9) / 9.0;
                    //    //i++;
                    //    crv.pingjia ="已评价("+ ((cc.c1 + cc.c2 + cc.c3 + cc.c4 + cc.c5 + cc.c6 + cc.c7 + cc.c8 + cc.c9)*2 / 9.0).ToString("#0.##")+")";
                    //}
                    foreach (var itemcm in listcm)
                    {
                        var ss = listss.Where(p => p.userid == user.id && p.modelid == itemcm.id).FirstOrDefault();
                        if (ss != null && ss.isfinish.HasValue && ss.isfinish.Value)
                        {

                            ClassRankView.classScore cs = new ClassRankView.classScore();

                            if (itemcm.type == "勾选")
                            {
                                cs.id = itemcm.id;
                                cs.score = "已完成（100）";
                                cs.finish = true;
                                //crv.gouxuan = "已完成（100）";
                                score += 100 * (itemcm.percent / 100.0);

                            }
                            if (itemcm.type == "打分" && ss.score.HasValue)
                            {
                                cs.id = itemcm.id;
                                cs.score = ss.score.Value.ToString("##.##");
                                //crv.dafen = ss.score.Value.ToString("##.##");
                                score += ss.score.Value * (itemcm.percent / 100.0);

                            }
                            if (itemcm.type == "测试" && ss.score.HasValue)
                            {
                                cs.id = itemcm.id;
                                cs.score = ss.score.Value.ToString("##.##");
                                //crv.ceshi = ss.score.Value.ToString("##.##");
                                score += ss.score.Value * (itemcm.percent / 100.0);

                            }
                            if (itemcm.type == "问卷")
                            {
                                cs.id = itemcm.id;
                                cs.score = "已完成（100）";
                                cs.finish = true;
                                //crv.wenjuan = "已完成（100）";
                                score += 100 * (itemcm.percent / 100.0);

                            }
                            crv.list.Add(cs);
                        }
                    }

                    var cc = listcc.Where(p => p.userid == user.id).FirstOrDefault();
                    if (cc != null)
                    {
                        ClassRankView.classScore cs = new ClassRankView.classScore();
                        cs.score = ((cc.c1 + cc.c2 + cc.c3 + cc.c4 + cc.c5 + cc.c6 + cc.c7 + cc.c8 + cc.c9) * 2 / 9.0).ToString("#0.#");
                        crv.list.Add(cs);
                        //crv.pingjia = ((cc.c1 + cc.c2 + cc.c3 + cc.c4 + cc.c5 + cc.c6 + cc.c7 + cc.c8 + cc.c9)*2 / 9.0).ToString("#0.#");
                    }
                     
                    crv.score = score; 
                    listrank.Add(crv);
                }

               listrank= listrank.OrderByDescending(c => c.score).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
               
                ViewBag.listrank = listrank;
                ViewBag.listcm = listcm;
                //获取分页信息
                ViewBag.Pager = Pager.Render(pageIndex, total, pageSize, "SearchState");
            }
            catch { }
            return View();
        }




        /// <summary>
        /// 获取问答
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchQA(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                List<qa> list = new List<qa>();
                list = QaDAL.getCLasses(classid);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchComment(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                double total = 0;
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                List<comment> list = new List<comment>();
                list = CommentDAL.getCLasses(classid);
                if (list != null & list.Count > 0)
                {


                    foreach (var cm in list)
                    {
                        total += (cm.c1 + cm.c2 + cm.c3 + cm.c4 + cm.c5 + cm.c6 + cm.c7 + cm.c8 + cm.c9) / 9.0;
                        total1 += (cm.c1 + cm.c2 + cm.c3) / 3.0;
                        total2 += (cm.c4 + cm.c5 + cm.c6) / 3.0;
                        total3 += (cm.c7 + cm.c8 + cm.c9) / 3.0;
                    }
                    total = total / list.Count;
                    total1 = total1 / list.Count;
                    total2 = total2 / list.Count;
                    total3 = total3 / list.Count;
                }
                ViewBag.total = total;
                ViewBag.total1 = total1;
                ViewBag.total2 = total2;
                ViewBag.total3 = total3;
                ViewBag.list = list;
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 获取相册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchAlbum(string model = "")
        {
            try
            {
                JObject obj = JObject.Parse(model);
                int classid = CommonBLL.GetJsonValue<int>(obj, "classid");
                List<album> list = new List<album>();
                list = AlbumDAL.getCLasses(classid);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }


        /// <summary>
        /// 删除问答，评论
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(List<int> ids, List<int> ids_parent, string type)
        {
            try
            {
                var res = false;
                switch(type)
                {
                    case "QA":
                        res = QaDAL.DelQA(ids, ids_parent);
                        break;
                    case "Comment":
                        res = CommentDAL.DelComment(ids, ids_parent);
                        break;
                    case "Ablum":
                        //res = AlbumDAL.DelAlbum(ids, ids_parent);
                        break;
                }
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
        /// 删除相册
       /// </summary>
       /// <param name="ids"></param>
       /// <param name="ids_parent"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        [HttpPost]
        public JsonResult DelAblum(List<string> ids, List<int> ids_parent, string type)
        {
            try
            {
                var res = false;
                res = AlbumDAL.DelAlbum(ids, ids_parent);
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
        
    }
}