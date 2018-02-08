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
    public class ClassstudentDAL:BaseDAL
    {
        /// <summary>
        /// 关联学生
        /// </summary>
        /// <param name="classstudent"></param>
        /// <returns></returns>
        public static bool addClassstudent(List<classstudent>classstudent  )
        {
            using (HanYiContext db = new HanYiContext())
            {
                db.classstudent.AddRange(classstudent);
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// 根系关联学生
        /// </summary>
        /// <param name="classstudent"></param>
        /// <returns></returns>
        public static bool updateClassstudent(int classid,List<classstudent> classstudent)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var temp = db.classstudent.Where(p => p.classid == classid);
             
                if(temp!=null)
                {   //新的学生添加
                    foreach(var item0 in classstudent)
                    {
                        bool has =false;
                        foreach(var item1 in temp)
                        {
                            if(item0.userid==item1.userid)
                            {
                                has=true;
                                break;
                            }
                        }
                        if(!has)
                            db.classstudent.Add(item0);
                    }
                    //删除的学生删除
                    //foreach (var item0 in temp)
                    //{
                    //    bool has = false;
                    //    foreach (var item1 in classstudent)
                    //    {
                    //        if (item0.userid == item1.userid)
                    //        {
                    //            has = true;
                    //            break;
                    //        }
                    //    }
                    //    if (!has)
                    //        db.classstudent.Remove(item0);
                    //}
                }
                  
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 从班级中批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delUsers(int classid,List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    using (DbContextTransaction tran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var query = from s in db.classstudent
                                        where ids.Contains(s.userid) && s.classid == classid
                                        select s;
                            db.classstudent.RemoveRange(query.ToList());
                            db.SaveChanges();

                            var query_album = from s in db.album
                                              where (s.userid.HasValue && ids.Contains(s.userid.Value)) && s.classid == classid  
                                              select s;
                            db.album.RemoveRange(query_album.ToList());
                            db.SaveChanges();
                            
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
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 根据学员id获取 学员课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classstudent>, int> getUserClass(int userid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classstudent.Include("classes").Include("classes.classmodels").Include("classes.courses").Include("classes.courses.teacher").Include("classes.comment").AsNoTracking()
                            where   s.userid == userid
                            orderby s.id descending
                            select s;
                int total = query.ToList().Count();
                List<classstudent> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classstudent>, int> res = new Tuple<List<classstudent>, int>(list, total);
                return res;
            }
        }

        /// <summary>
        /// 获取用户所有的课程
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<classstudent> getUserClass(int userid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classstudent.Include("classes").Include("classes.courses").AsNoTracking()
                            where s.userid == userid
                            orderby s.id descending
                            select s;
                List<classstudent> res = query.ToList();
                 return res;
            }
        }
        
    }
}
