using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class ClassesDAL : BaseDAL
    {
        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="companyid">企业id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getCLasses(int companyid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent.users").Include("courses.teacher").Include("companys").AsNoTracking()
                            where s.state == true && s.companyid == companyid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getCLasses(int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("courses.teacher").Include("companys").AsNoTracking()
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 删除班级 以及 班级关联的学生
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delClasses(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.classes.Include("classstudent")
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                    var classes = query.ToList();
                    foreach(var item in classes)
                    {
                        db.classstudent.RemoveRange(item.classstudent);
                    }
                    db.classes.RemoveRange(classes);
                    db.SaveChanges();
                    return true;
                }
            }
            else
            {

                return false;
            }
        }
        /// <summary>
        /// 班级详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static classes getClassById(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("companys").Include("courses").Include("courses.teacher").Include("classmodels").Include("courses.catalog").Include("courses.catalog.subcatalog").Include("courses.catalog.subcatalog.subcatalogattachment")
                            where  s.id==id 
                            select s;
                 return  query.FirstOrDefault();

                
            }
        }

        /// <summary>
        /// 根据培训师id获取培训师 课程
        /// </summary>
        /// <param name="companyid">教师id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classes>, int> getTeacherCLasses(int teacherid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classes.Include("classstudent").Include("courses").Include("comment").Include("companys").AsNoTracking()
                            where s.state == true && s.courses.teacherid == teacherid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classes> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classes>, int> res = new Tuple<List<classes>, int>(list, total);
                return res;
            }
        }
    }
}
