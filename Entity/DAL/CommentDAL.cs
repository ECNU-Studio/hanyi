using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class CommentDAL : BaseDAL
    {
        public static List<comment> getCLasses(int classesid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("commentsub").Include("commentsub.user").Include("commentsub.teacher").AsNoTracking()
                            where s.classid == classesid
                            orderby s.id descending
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }
        public static List<comment> getCLasses(int classesid,int userid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("commentsub").Include("commentsub.user").Include("commentsub.teacher").AsNoTracking()
                            where s.classid == classesid && s.userid==userid
                            orderby s.id descending
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 根据用户id获取comment
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<comment> getUserComment(int userid, string content)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("classes").Include("classes.courses").Include("commentsub").Include("commentsub.teacher").Include("commentsub.user").AsNoTracking()
                            where s.userid == userid || s.commentsub.Any(p => p.userid == userid)
                            where string.IsNullOrEmpty(content) || s.content.Contains(content) || s.commentsub.Any(p => p.content.Contains(content))
                            orderby s.id descending
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 教师收到的评论
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<comment> getTeacherComment(int teacherid, string content)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("classes").Include("classes.courses").Include("commentsub").Include("commentsub.teacher").Include("commentsub.user").AsNoTracking()
                            where s.classes.courses.teacherid == teacherid  
                            where string.IsNullOrEmpty(content) || s.content.Contains(content) || s.commentsub.Any(p => p.content.Contains(content))
                            orderby s.id descending
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 根据用户id获取comment
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<comment> getUserComment(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("classes").Include("classes.courses").Include("commentsub").Include("commentsub.user").Include("commentsub.teacher").AsNoTracking()
                            where s.id == id
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="ids_parent"></param>
        /// <returns></returns>
        public static bool DelComment(List<int> ids, List<int> ids_parent)
        {
            using (HanYiContext db = new HanYiContext())
            {
                using (DbContextTransaction tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (ids_parent != null && ids_parent.Count > 0)
                        {
                            var query = from s in db.comment
                                        where ids_parent.Contains(s.id)
                                        orderby s.id descending
                                        select s;
                            var list = query.ToList();
                            db.comment.RemoveRange(list);
                            db.SaveChanges();
                        }
                        if (ids != null && ids.Count > 0)
                        {
                            var query = from s in db.commentsub
                                        where ids.Contains(s.id)
                                        orderby s.id descending
                                        select s;
                            var list = query.ToList();
                            db.commentsub.RemoveRange(list);
                            db.SaveChanges();
                        }
                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return false;
                    }

                }
            }
        }

        /// <summary>
        /// 统计用户评论过的课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionary<int, bool> getUserCommentStatic(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                Dictionary<int, bool> res = new Dictionary<int, bool>();
                var query = from s in db.comment
                            where s.userid == id
                            group s by s.classid into g
                            select new { classid = g.Key, count = g.Count() };
                var temp = query.ToList();
                foreach (var item in temp)
                {
                    if (item.count > 0)
                    {
                        res.Add(item.classid, true);
                    }
                    else
                    {
                        res.Add(item.classid, false);

                    }
                }
                return res;
            }
        }

    }
}
