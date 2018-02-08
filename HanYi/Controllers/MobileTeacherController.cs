using Entity.DAL;
using Entity.Entity;
using Entity.Util;
using HanYi.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HanYi.Controllers
{
    public class MobileTeacherController : BaseController
    {
        //
        // GET: /MobileTeacher/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 班级列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult CourseList(int pageIndex, int pageSize)
        {
            try
            {

                List<classes> list = new List<classes>();
                int total = 0;
                var temp = ClassesDAL.getTeacherCLasses(teacher.id, pageIndex, pageSize);
                if (temp != null)
                {
                    list = temp.Item1;
                    total = temp.Item2;
                }
                ViewBag.list = list;
               
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
            string[] color = new string[] { "#2A579A", "#077164", "#1B7247", "#D14524", "#AE4D50", "#7E3877", "#6B3782" };
            try
            {
                string svgData = "[]";
                List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
                var modle = ClassesDAL.getClassById(id);
                var classAddress = ClassaddressDAL.getClassAddress(id, 1, int.MaxValue);
                List<classaddress> classAddresslist = new List<classaddress>();
                if (classAddresslist != null)
                {
                    classAddresslist = classAddress.Item1;
                }
                if (modle.classmodels != null)
                {
                    modle.classmodels = modle.classmodels.OrderBy(p => p.id).ToList();
                    int i = 0;
                    foreach (var item in modle.classmodels)
                    {

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("desc", item.title + ":" + item.percent + "%");
                        dic.Add("color", color[i]);
                        dic.Add("share", item.percent);
                        res.Add(dic);
                        i++;
                        i = i % 7;
                    }
                }
                JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
                ViewBag.modle = modle;
                ViewBag.classAddress = classAddresslist;
                string upToken = CommonBLL.GetToken();
                ViewBag.upToken = upToken;
                ViewBag.svgData = (res.Count > 0 ? jsonSerialize.Serialize(res) : "");
                ViewBag.classid = id;
                ViewBag.userid = user.id;
                ViewBag.type = type;
            }
            catch { }
            return View();

        }



        /// <summary>
        /// 班级详情--成绩排名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MarkOrder(int id)
        {
            try
            {

                int classid = id;
                List<users> listuser = null;
                List<classmodels> listcm = null;
                List<studystate> listss = null;
                List<comment> listcc = null;
                List<ClassRankView> listrank = new List<ClassRankView>();
                int total = 0;
                var temp = UsersDAL.getClassUser(classid, "", 1, int.MaxValue);
                if (temp != null)
                {
                    listuser = temp.Item1;
                    total = temp.Item2;
                }
                listcm = ClassmodelsDAL.getCLasses(classid);
                listss = StudystateDAL.getStateOnly(classid);
                listcc = CommentDAL.getCLasses(classid);

                foreach (var user in listuser)
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
                    //            score += 100 * (itemcm.percent / 100.0);
                                 
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
                       
                    //    crv.pingjia = ((cc.c1 + cc.c2 + cc.c3 + cc.c4 + cc.c5 + cc.c6 + cc.c7 + cc.c8 + cc.c9)*2 / 9.0).ToString("#0.#");
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
                    crv.score = score; 

                    listrank.Add(crv);
                }

                listrank = listrank.OrderByDescending(c => c.score).ToList();

                ViewBag.listrank = listrank;
                ViewBag.listcm = listcm;
                //获取分页信息
            }
            catch { }
            return View();


        }

        /// <summary>
        /// 课程问答 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HuDongQA(int id, string content)
        {
            try
            {
                List<qa> qas = QaDAL.getCLasses(id, content);
                ViewBag.qas = qas;

            }
            catch (Exception e) { }


            return View();
        }
        /// <summary>
        /// 课程问答 关键字搜索
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public ActionResult HuDongQAKeyWord(int id, string content)
        {
            try
            {
                List<qa> qas = QaDAL.getCLasses(id, content);
                ViewBag.qas = qas;

            }
            catch (Exception e) { }


            return View();
        }
        /// <summary>
        /// 课程评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HuDongComment(int id)
        {
            try
            {

                List<comment> comments = CommentDAL.getCLasses(id);
                double total = 0;
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                if (comments != null & comments.Count > 0)
                {


                    foreach (var cm in comments)
                    {
                        total += (cm.c1 + cm.c2 + cm.c3 + cm.c4 + cm.c5 + cm.c6 + cm.c7 + cm.c8 + cm.c9) / 9.0;
                        total1 += (cm.c1 + cm.c2 + cm.c3) / 3.0;
                        total2 += (cm.c4 + cm.c5 + cm.c6) / 3.0;
                        total3 += (cm.c7 + cm.c8 + cm.c9) / 3.0;
                    }
                    total = total / comments.Count;
                    total1 = total1 / comments.Count;
                    total2 = total2 / comments.Count;
                    total3 = total3 / comments.Count;
                }
                ViewBag.total = total;
                ViewBag.total1 = total1;
                ViewBag.total2 = total2;
                ViewBag.total3 = total3;
                ViewBag.comments = comments;

            }
            catch (Exception e) { }


            return View();
        }

       
        /// <summary>
        /// 班级相册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HuDongAlbum(int id)
        {
            try
            {

                List<album> albums = AlbumDAL.getCLasses(id);
                ViewBag.albums = albums;
            }
            catch (Exception e) { }


            return View();
        }

         /// <summary>
        /// 保存评论信息  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostSaveEvaluate(int classid, string type, int? itemid, string txtdetail, string srclist, int? touserid, int totype)
        {
            try
            {
                List<string> urls = new List<string>();
                if (!string.IsNullOrEmpty(srclist))
                {
                    string[] pics = srclist.Split(new string[] { "[||]" }, StringSplitOptions.None);
                    foreach (string s in pics)
                    {
                        if (s != null && s != "")
                        {
                            urls.Add(s);
                        }
                    }
                }
                switch (type)
                {
                    case "wenda":
                        if (itemid.HasValue)
                        {
                            qasub qasub = new qasub();
                            qasub.qaid = itemid.Value;
                            qasub.content = txtdetail;
                            qasub.teacherid = teacher.id;
                            qasub.date = DateTime.Now;
                            if (totype == 1)
                            {
                                if (touserid.HasValue)
                                {
                                    qasub.touserid = touserid;
                                }
                            }
                            else if (totype == 0)
                            {
                                if (touserid.HasValue)
                                {
                                    qasub.toteacherid = touserid;
                                }
                            }
                            qasub.state = true;
                            if (urls.Count > 0)
                                qasub.pic1 = urls[0];
                            if (urls.Count > 1)
                                qasub.pic2 = urls[1];
                            if (urls.Count > 2)
                                qasub.pic3 = urls[2];
                            if (urls.Count > 3)
                                qasub.pic4 = urls[3];
                            if (urls.Count > 4)
                                qasub.pic5 = urls[4];
                            if (urls.Count > 5)
                                qasub.pic6 = urls[5];
                            BaseDAL.AddModel<qasub>(qasub);
                            List<qa> qas = QaDAL.getUserQA(itemid.Value);
                            if (qas != null && qas.Count > 0)
                            {
                                List<int> userids = new List<int>();
                                List<int> teacherids = new List<int>();
                                if (qas[0].userid.HasValue)
                                    userids.Add(qas[0].userid.Value);
                                if (qas[0].teacherid.HasValue)
                                    teacherids.Add(qas[0].teacherid.Value);
                                foreach (var item in qas[0].qasub)
                                {
                                    if (item.userid.HasValue)
                                        userids.Add(item.userid.Value);
                                    if (item.teacherid.HasValue)
                                        teacherids.Add(item.teacherid.Value);
                                }
                                TeacheresDAL.updateNewQa(teacherids);
                                UsersDAL.updateNewQa(userids);
                            }
                            keyvalue kv = KeyValueDAL.fingByKey(Encrypt.MD5Encrypt16("wenda_new" + classid));
                            kv.value = JsonConvert.SerializeObject(qasub);
                            kv.dateintput = DateTime.Now;
                            KeyValueDAL.SetKeyValue(kv);
                            return Json(new { success = true, message = "" });
                        }
                        else
                        {
                            qa qa = new qa();
                            qa.classid = classid;
                            qa.content = txtdetail;
                            qa.teacherid = teacher.id;
                            qa.date = DateTime.Now;
                            qa.state = true;
                            if (urls.Count > 0)
                                qa.pic1 = urls[0];
                            if (urls.Count > 1)
                                qa.pic2 = urls[1];
                            if (urls.Count > 2)
                                qa.pic3 = urls[2];
                            if (urls.Count > 3)
                                qa.pic4 = urls[3];
                            if (urls.Count > 4)
                                qa.pic5 = urls[4];
                            if (urls.Count > 5)
                                qa.pic6 = urls[5];
                            BaseDAL.AddModel<qa>(qa);
                            keyvalue kv = KeyValueDAL.fingByKey(Encrypt.MD5Encrypt16("wenda_new" + classid));
                            kv.value = JsonConvert.SerializeObject(qa);
                            kv.dateintput = DateTime.Now;
                            KeyValueDAL.SetKeyValue(kv);
                            return Json(new { success = true, message = "" });
                        }





                        break;
                    case "pingjia":
                        if (itemid.HasValue)
                        {
                            commentsub cs = new commentsub();
                            cs.commentid = itemid.Value;
                            cs.content = txtdetail;
                            cs.date = DateTime.Now;
                            cs.state = true;
                            cs.teacherid = teacher.id;
                            BaseDAL.AddModel<commentsub>(cs);
                            keyvalue kv = KeyValueDAL.fingByKey(Encrypt.MD5Encrypt16("pingjia_new" + classid));
                            kv.value = JsonConvert.SerializeObject(cs);
                            kv.dateintput = DateTime.Now;
                            KeyValueDAL.SetKeyValue(kv);
                            return Json(new { success = true, message = "" });
                        }
                        else
                        {

                            return Json(new { success = false, message = "请选择相应学员进行回复" });
                        }


                        break;
                    case "xiangce":
                        album album = new album();
                        album.classid = classid;
                        album.content = txtdetail;
                        album.teacherid = teacher.id;
                        album.date = DateTime.Now;
                        album.state = true;
                        if (urls.Count > 0)
                            album.pic1 = urls[0];
                        if (urls.Count > 1)
                            album.pic2 = urls[1];
                        if (urls.Count > 2)
                            album.pic3 = urls[2];
                        if (urls.Count > 3)
                            album.pic4 = urls[3];
                        if (urls.Count > 4)
                            album.pic5 = urls[4];
                        if (urls.Count > 5)
                            album.pic6 = urls[5];
                        BaseDAL.AddModel<album>(album);

                        keyvalue kvx = KeyValueDAL.fingByKey(Encrypt.MD5Encrypt16("xiangce_new" + classid));
                        kvx.value = JsonConvert.SerializeObject(album);
                        kvx.dateintput = DateTime.Now;
                        KeyValueDAL.SetKeyValue(kvx);
                        return Json(new { success = true, message = "" });
                        break;
                }


                return Json(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PerCenter()
        {
            ViewBag.teacher =teacher;
            
            return View();
        }

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Information()
        {
            ViewBag.teacher = teacher;
            string upToken = CommonBLL.GetToken();
            ViewBag.upToken = upToken;
            return View();
        }


        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="txurl"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostSaveInfo(string txurl,string name,string introduce, string phone, string email)
        {
            try
            {
                teacher.header = txurl;
                teacher.name = name;
                teacher.introduce = introduce;
                teacher.phone = phone;
                teacher.email = email;
                BaseDAL.EditEntry<teacheres>(teacher, "id");
                return Json(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// 收到回答
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAnswer()
        {
            teacher.new_ans = false;
            BaseDAL.EditEntry<teacheres>(teacher, "id");
            return View();
        }

        public ActionResult MyAnswerList(string content)
        {
            try
            {
                List<qa> list = QaDAL.getTeacherQA(teacher.id, content);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 收到回答详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAnswerDetail(int id)
        {
            try
            {
                List<qa> list = QaDAL.getUserQA(id);
                ViewBag.list = list;
            }
            catch { }
            string upToken = CommonBLL.GetToken();
            ViewBag.upToken = upToken;
            return View();
        }


        /// <summary>
        /// 收到评论
        /// </summary>
        /// <returns></returns>
        public ActionResult MyComment()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ActionResult MyCommentList(string content)
        {
            try
            {
                List<comment> list = CommentDAL.getTeacherComment(teacher.id, content);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }

        /// <summary>
        /// 收到评论详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCommentDetail(int id)
        {
            try
            {
                List<comment> list = CommentDAL.getUserComment(id);
                ViewBag.list = list;
            }
            catch { }
            return View();
        }


        /// 设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            ViewBag.teacher = teacher;
            return View();
        }

        /// <summary>
        /// 设置修改
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SettingPost(string type, bool val)
        {
            try
            {
                switch (type)
                {
                    case "1":
                        teacher.notice_wenda = val;
                        break;
                    case "2":
                        teacher.notice_pinglun = val;
                        break;
                    case "3":
                        teacher.language = 2;
                        break;
                }
                BaseDAL.EditEntry<teacheres>(teacher, "id");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePW()
        {
            return View();
        }

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="oldPW"></param>
        /// <param name="newPW"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostSavePW(string oldPW, string newPW)
        {
            try
            {
                if (teacher.password != Encrypt.MD5Encrypt(oldPW))
                {
                    return Json(new { success = false, message = "原密码错误" });
                }
                else
                {
                    teacher.password = Encrypt.MD5Encrypt(newPW);
                    BaseDAL.EditEntry<teacheres>(teacher, "id");
                }

                return Json(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Help()
        {
            return View();
        }

        /// <summary>
        /// 平台介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult Intro()
        {
            return View();
        }

        public ActionResult Cartoon()
        {
            return View();
        }
    }
}