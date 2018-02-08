using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    /// <summary>
    /// 学员完成情况
    /// </summary>
    public class StudystateDAL : BaseDAL
    {
        /// <summary>
        /// 学员完成情况列表
        /// </summary>
        /// <param name="classid">班级id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static List<studystate> getState(int classid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.studystate.Include("classmodel").Include("users").AsNoTracking()
                            where s.classid == classid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<studystate> res = query.ToList();

                return res;
            }
        }


        /// <summary>
        /// 紧紧获取班级学习状态
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public static List<studystate> getStateOnly(int classid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.studystate.AsNoTracking()
                            where s.classid == classid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<studystate> res = query.ToList();

                return res;
            }
        }
        /// <summary>
        /// 获取 学生完成状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionary<int,int> getStudentState(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                Dictionary<int, int> res = new Dictionary<int, int>();
                var query = from s in db.studystate.Include("classmodel").Where(p => p.isfinish == true && p.userid == id)
                                                   
                            group s by s.classmodel.classesid into g                          
                            select new
                            {
                                classid = g.Key,
                                count = g.Count()
                            };
                 
                var temp = query.ToList();
                foreach(var item in temp)
                {
                    res.Add(item.classid, item.count);
                }
                return res;
            }
        }

        /// <summary>
        /// 获取班级的学生进度
        /// </summary>
        /// <param name="classesid"></param>
        /// <param name="username"></param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<StudystateView>, int> getCLassState(int classesid, string username, int currpage, int pagesize)
        {
               using (HanYiContext db = new HanYiContext())
                {

                    List<StudystateView> res = new List<StudystateView>();

                    var query = from cs in db.classstudent.Include("users").Where(p => p.users.name.Contains(username) && p.classid == classesid)
                                join sm in db.classmodels.Where(m => m.classesid == classesid) on cs.classid equals sm.classesid 
                                join s in db.studystate.Where(p => p.classid == classesid) on new { cs.classid, cs.userid } equals new { s.classid, s.userid } into css
                                
                                from l in css.DefaultIfEmpty()
                                select new
                                {
                                    id = cs.id,
                                    classid = cs.classid,
                                    user = cs.users,
                                    userid = cs.userid,
                                    isfinish = l.isfinish,
                                    modelid = sm.id,
                                    classmodel=sm,
                                };
                    int total = query.Count();
                    var temp = query.ToList().Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

                    foreach (var item in temp)
                    {
                        StudystateView sv = new StudystateView();
                        sv.classid = item.classid;
                        sv.user = item.user;
                        sv.userid = item.userid;
                        sv.id = item.id;
                        sv.isfinish = item.isfinish;
                        sv.modelid = item.modelid;
                        sv.classmodel  = item.classmodel;
                        res.Add(sv);
                    }
                    Tuple<List<StudystateView>, int> resTuple = new Tuple<List<StudystateView>, int>(res, total);
                    return resTuple;
                }
             
        }
        

        /// <summary>
        /// 保存学生状态
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SaveState(int classid, List<studystate> list)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var temp = db.studystate.Where(p => p.classid == classid);

                if (temp != null)
                {   //新的学生成绩
                    foreach (var item0 in list)
                    {
                        bool has = false;
                        foreach (var item1 in temp)
                        {
                            if (item0.userid == item1.userid&&item0.modelid==item1.modelid)
                            {
                                has = true;
                                item1.isfinish = item0.isfinish;
                                item1.score = item0.score;
                                break;
                            }
                        }
                        if (!has)
                            db.studystate.Add(item0);
                    }

                }

                db.SaveChanges();
                return true;
            }
        }
   
    
        /// <summary>
        /// 获取某个用户在某个班级的学习状态
        /// </summary>
        /// <param name="classesid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<studystate> getUserClassStudystate(int classesid,int userid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from ss in db.studystate
                            where ss.classid == classesid && ss.userid == userid
                            select ss;
                return query.ToList();
            }
        }
        /// <summary>
        /// 某个学生的摸个模块的
        /// </summary>
        /// <param name="classesid"></param>
        /// <param name="userid"></param>
        /// <param name="modleid"></param>
        /// <returns></returns>
        public static  studystate getUserClassStudystate(int classesid, int userid,int modleid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from ss in db.studystate
                            where ss.classid == classesid && ss.userid == userid && ss.modelid==modleid
                            select ss;
                return query.ToList().FirstOrDefault();
            }
        }
    }
}