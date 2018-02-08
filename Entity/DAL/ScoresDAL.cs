using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class ScoresDAL : BaseDAL
    {
        /// <summary>
        /// 根据班级 分数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<ScoresView>, int> getCLassesScores(int classesid, string username, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {

                List<ScoresView> res = new List<ScoresView> ();

                var query = from cs in db.classstudent.Include("users").Where(p => p.users.name.Contains(username) && p.classid == classesid)
                            join s in db.scores.Where(p =>p.classid == classesid) on new { cs.classid, cs.userid } equals new { s.classid, s.userid } into css
                            from l in css.DefaultIfEmpty()
                            select new
                            { 
                                id=cs.id,
                                classid=cs.classid,
                                user = cs.users,
                                userid=cs.userid,
                                score = l.score
                            };
                int total = query.Count();
                var temp = query.ToList().Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

                foreach (var item in temp)
                {
                    ScoresView sv = new ScoresView();
                    sv.classid = item.classid;
                    sv.user = item.user;
                    sv.userid = item.userid;
                    sv.id = item.id;
                    sv.score = item.score;
                    res.Add(sv);
                }
                  Tuple<List<ScoresView>, int> resTuple = new Tuple<List<ScoresView>, int>(res, total);
                  return resTuple;
            }
        }

        /// <summary>
        /// 保存学生成绩
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SaveScore(int classid, List<scores> list)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var temp = db.scores.Where(p => p.classid == classid);

                if (temp != null)
                {   //新的学生成绩
                    foreach (var item0 in list)
                    {
                        bool has = false;
                        foreach (var item1 in temp)
                        {
                            if (item0.userid == item1.userid)
                            {
                                has = true;
                                item1.score = item0.score;
                                break;
                            }
                        }
                        if (!has)
                            db.scores.Add(item0);
                    }
                     
                }

                db.SaveChanges();
                return true;
            }
        }
    }
}
