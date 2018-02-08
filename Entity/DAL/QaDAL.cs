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
    public class QaDAL : BaseDAL
    {
        /// <summary>
        /// 根据班级 获取问答
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public static List<qa> getCLasses(int classesid,string content="")
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.qa.Include("user").Include("teacher").Include("qasub").Include("qasub.user").Include("qasub.touser").Include("qasub.teacher").Include("qasub.toteacher").AsNoTracking()
                            where s.classid == classesid
                            where string.IsNullOrEmpty(content) || s.content.Contains(content) || s.qasub.Any(p => p.content.Contains(content)) || (s.user != null && (s.user.name.Contains(content) || s.qasub.Any(p => p.user.name.Contains(content)))) || (s.teacher != null && (s.teacher.name.Contains(content) || s.qasub.Any(p => p.teacher.name.Contains(content))))
                            orderby s.id descending
                            select s;
                List<qa> res = query.ToList();
                return res;
            }
        }

        /// <summary>
        /// 根据用户id获取qa
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<qa> getUserQA(int userid, string content)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.qa.Include("user").Include("teacher").Include("classes").Include("classes.courses").Include("qasub").Include("qasub.user").Include("qasub.touser").Include("qasub.teacher").Include("qasub.toteacher").AsNoTracking()
                            where s.userid == userid||s.qasub.Any(p=>p.userid==userid)
                            where string.IsNullOrEmpty(content) || s.content.Contains(content) || s.qasub.Any(p => p.content.Contains(content)) || s.user.name.Contains(content) || s.qasub.Any(p => p.user.name.Contains(content))
                            orderby s.id descending
                            select s;
                List<qa> res = query.ToList();
                return res;
            }
        }

        /// <summary>
        /// 根据用户id获取qa
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<qa> getTeacherQA(int teacherid, string content)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.qa.Include("user").Include("teacher").Include("classes").Include("classes.courses").Include("qasub").Include("qasub.user").Include("qasub.touser").Include("qasub.teacher").Include("qasub.toteacher").AsNoTracking()
                            where s.teacherid == teacherid || s.qasub.Any(p => p.teacherid == teacherid)
                            where string.IsNullOrEmpty(content) || s.content.Contains(content) || s.qasub.Any(p => p.content.Contains(content)) || (s.teacher.name.Contains(content) || s.qasub.Any(p => p.teacher.name.Contains(content)))
                            orderby s.id descending
                            select s;
                List<qa> res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 根据用户id获取qa
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<qa> getUserQA(int  id )
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.qa.Include("user").Include("teacher").Include("classes").Include("classes.courses").Include("qasub").Include("qasub.user").Include("qasub.touser").Include("qasub.teacher").Include("qasub.toteacher").AsNoTracking()
                            where s.id == id                            
                            select s;
                List<qa> res = query.ToList();
                return res;
            }
        }

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="ids_parent"></param>
        /// <returns></returns>
        public static bool DelQA(List<int> ids,List<int> ids_parent)
        {
            using (HanYiContext db = new HanYiContext())
            {
                using (DbContextTransaction tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (ids_parent != null && ids_parent.Count > 0)
                        {
                            var query = from s in db.qa
                                        where ids_parent.Contains(s.id)
                                        orderby s.id descending
                                        select s;
                            var list = query.ToList();
                            db.qa.RemoveRange(list);
                            db.SaveChanges();
                        }
                        if (ids != null && ids.Count > 0)
                        {
                            var query = from s in db.qasub
                                        where ids.Contains(s.id)
                                        orderby s.id descending
                                        select s;
                            var list = query.ToList();
                            db.qasub.RemoveRange(list);
                            db.SaveChanges();
                        }
                        tran.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        return false;
                    }
                    
                }
            }
        }
    }
}
