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
    public class CoursesDAL : BaseDAL
    {
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<courses>, int> getCourses(int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.courses.Include("teacher").Include("classes.companys").AsNoTracking()
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<courses> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<courses>, int> res = new Tuple<List<courses>, int>(list, total);
                return res;
            }
        }
        public static Tuple<List<courses>, int> getCourses(int teacherid,int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.courses.Include("classes").AsNoTracking()
                            where s.state == true
                            where s.teacherid == teacherid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<courses> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<courses>, int> res = new Tuple<List<courses>, int>(list, total);
                return res;
            }
        }

        public static courses getItem(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.courses.Include("teacher").Include("classes.companys").AsNoTracking()
                            where s.state == true && s.id == id
                            orderby s.id descending
                            select s;
                return query.FirstOrDefault();
            }
        }



        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delCourses(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {


                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.courses
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                    db.courses.RemoveRange(query.ToList());
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
        /// 根据课程名字匹配课程
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<courses> MatchCourse(string name, int num)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.courses.AsNoTracking()
                            where string.IsNullOrEmpty(name) || s.name.Contains(name)
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<courses> list = query.Take(num).ToList();
                return list;
            }
        }


        /// <summary>
        /// 添加章节
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool CoursesCatalogAddOld(List<catalog> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    using (DbContextTransaction tran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var catalogItem in ids)
                            {
                                if (catalogItem.id == 0)
                                {
                                    catalog newItem = new catalog();
                                    newItem.subcatalog = catalogItem.subcatalog;
                                    catalogItem.subcatalog = null;
                                    db.catalog.Add(catalogItem);
                                    db.SaveChanges();
                                    catalogItem.subcatalog = newItem.subcatalog;
                                }
                                else
                                {
                                    var old = db.catalog.Find(catalogItem.id);
                                    old.name = catalogItem.name;
                                }
                                if (catalogItem.subcatalog != null && catalogItem.subcatalog.Count > 0)
                                {
                                    foreach (var subcatalogItem in catalogItem.subcatalog)
                                    {
                                        subcatalogItem.catalogid = catalogItem.id;
                                        if (subcatalogItem.id == 0)
                                        {
                                            subcatalog newItem_sub = new subcatalog();
                                            newItem_sub.subcatalogattachment = subcatalogItem.subcatalogattachment;
                                            subcatalogItem.subcatalogattachment = null;
                                            db.subcatalog.Add(subcatalogItem);
                                            db.SaveChanges();
                                            subcatalogItem.subcatalogattachment = newItem_sub.subcatalogattachment;
                                        }
                                        else
                                        {
                                            var old = db.subcatalog.Find(subcatalogItem.id);
                                            old.name = subcatalogItem.name;
                                        }
                                        var query = from s in db.subcatalogattachment
                                                    where s.subcatalogid == subcatalogItem.id
                                                    orderby s.id descending
                                                    select s;
                                        var classes = query.ToList();
                                        if (classes != null && classes.Count > 0)
                                        {
                                            db.subcatalogattachment.RemoveRange(classes);
                                        }
                                        if (subcatalogItem.subcatalogattachment != null && subcatalogItem.subcatalogattachment.Count > 0)
                                        {
                                            List<subcatalogattachment> subList = new List<subcatalogattachment>();
                                            subList = subcatalogItem.subcatalogattachment;
                                            subList.ForEach(l => l.subcatalogid = subcatalogItem.id);
                                            db.subcatalogattachment.AddRange(subList);
                                        }
                                        db.SaveChanges();
                                        subcatalogItem.subcatalogattachment = null;


                                    }
                                    int cid=catalogItem.subcatalog[0].catalogid;
                                    var needremove = db.subcatalog.Where(p => p.catalogid ==cid ).ToArray();
                                    foreach (var item in needremove)
                                    {
                                        if (!catalogItem.subcatalog.Any(p => p.id == item.id))
                                            db.subcatalog.Remove(item);
                                    }
                                    db.SaveChanges();
                                }
                                else
                                {
                                    var query = from s in db.subcatalog
                                                where s.catalogid == catalogItem.id
                                                orderby s.id descending
                                                select s;
                                    var classes = query.ToList();
                                    if (classes != null && classes.Count > 0)
                                    {
                                        db.subcatalog.RemoveRange(classes);
                                    }
                                    db.SaveChanges();
                                }
                                catalogItem.subcatalog = null;

                                int courseid = ids[0].courseid;
                                var needremovec = db.catalog.Where(p => p.courseid == courseid).ToArray();
                                foreach (var item in needremovec)
                                {
                                    if (!ids.Any(p => p.id == item.id))
                                        db.catalog.Remove(item);
                                }                               
                                db.SaveChanges();
                            }
                            
                            tran.Commit();
                            return true;
                        }
                        catch
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


        public static bool CoursesCatalogAdd(List<catalog> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    using (DbContextTransaction tran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var catalogItem in ids)
                            {
                                if (catalogItem.id == 0)
                                {
                                    //3个表一起添加
                                    db.catalog.Add(catalogItem);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    if(catalogItem.subcatalog != null && catalogItem.subcatalog.Count > 0)
                                    {
                                        var old = db.catalog.Find(catalogItem.id);
                                        old.name = catalogItem.name;
                                        foreach (var subcatalogItem in catalogItem.subcatalog)
                                        {
                                            if (subcatalogItem.id == 0)
                                            {
                                                //2个表一起添加
                                                subcatalogItem.catalogid = catalogItem.id;
                                                db.subcatalog.Add(subcatalogItem);
                                                db.SaveChanges();
                                            }
                                            else
                                            {
                                                var oldsubcatalog = db.subcatalog.Find(subcatalogItem.id);
                                                oldsubcatalog.name = subcatalogItem.name;
                                                if(subcatalogItem.subcatalogattachment != null && subcatalogItem.subcatalogattachment.Count > 0)
                                                {
                                                    foreach (var subcatalogattachmentItem in subcatalogItem.subcatalogattachment)
                                                    {
                                                        if (subcatalogattachmentItem.id == 0)
                                                        {
                                                            //1个表添加
                                                            subcatalogattachmentItem.subcatalogid = subcatalogItem.id;
                                                            db.subcatalogattachment.Add(subcatalogattachmentItem);
                                                            db.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            //此处暂时无用，附件只能删除还不能进行修改操作
                                                            var oldsubcatalogattachmentItem = db.subcatalogattachment.Find(subcatalogattachmentItem.id);
                                                            oldsubcatalogattachmentItem.name = subcatalogattachmentItem.name;
                                                            oldsubcatalogattachmentItem.size = subcatalogattachmentItem.size;
                                                            oldsubcatalogattachmentItem.type = subcatalogattachmentItem.type;
                                                            oldsubcatalogattachmentItem.path = subcatalogattachmentItem.path;
                                                            oldsubcatalogattachmentItem.type = subcatalogattachmentItem.type;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                    #region 删除
                                                    int caid = subcatalogItem.id;
                                                    List<int> subcatalogattachids = new List<int>();
                                                    var needremoveb = db.subcatalogattachment.Where(p => p.subcatalogid == caid).ToArray();
                                                    foreach (var item in needremoveb)
                                                    {
                                                        if (!subcatalogItem.subcatalogattachment.Any(p => p.id == item.id))
                                                        {
                                                            subcatalogattachids.Add(item.id);
                                                        }
                                                    }
                                                    if (subcatalogattachids.Count > 0)
                                                    {
                                                        var querysubcatalogattachmentids = from s in db.subcatalogattachment
                                                                                           where subcatalogattachids.Contains(s.id)
                                                                                           orderby s.id descending
                                                                                           select s;
                                                        var res_attachment = querysubcatalogattachmentids.ToList();
                                                        db.subcatalogattachment.RemoveRange(res_attachment);
                                                    }
                                                    db.SaveChanges(); 
                                                    #endregion
                                                }
                                                else
                                                {
                                                    var query = from s in db.subcatalogattachment
                                                                where s.subcatalogid == subcatalogItem.id
                                                                orderby s.id descending
                                                                select s;
                                                    var classes = query.ToList();
                                                    if (classes != null && classes.Count > 0)
                                                    {
                                                        db.subcatalogattachment.RemoveRange(classes);
                                                    }
                                                }
                                                db.SaveChanges();
                                            }
                                        }
                                        #region 删除subcatalog
                                        //删除catalog
                                        int cid = catalogItem.subcatalog[0].catalogid;
                                        List<int> subcatalogids = new List<int>();
                                        var needremoveca = db.subcatalog.Where(p => p.catalogid == cid).ToArray();
                                        foreach (var item in needremoveca)
                                        {
                                            if (!catalogItem.subcatalog.Any(p => p.id == item.id))
                                            {
                                                subcatalogids.Add(item.id);
                                            }
                                        }
                                        if (subcatalogids.Count > 0)
                                        {
                                            var querysubcatalogids = from s in db.subcatalog.Include("subcatalogattachment")
                                                                     where subcatalogids.Contains(s.id)
                                                                     orderby s.id descending
                                                                     select s;
                                            var res = querysubcatalogids.ToList();
                                            foreach (var item in res)
                                            {
                                                db.subcatalogattachment.RemoveRange(item.subcatalogattachment);
                                                db.SaveChanges();
                                            }
                                            if (res.Count > 0)
                                            {
                                                res.ForEach(l => l.subcatalogattachment = null);
                                            }
                                            db.subcatalog.RemoveRange(res);
                                        }
                                        db.SaveChanges(); 
                                        #endregion
                                    }
                                    else
                                    {
                                        var old = db.catalog.Find(catalogItem.id);
                                        old.name = catalogItem.name;
                                        var query = from s in db.subcatalog.Include("subcatalogattachment")
                                                    where s.catalogid == catalogItem.id
                                                    orderby s.id descending
                                                    select s;
                                        var classes = query.ToList();
                                        if (classes != null && classes.Count > 0)
                                        {
                                            foreach (var item1 in classes)
                                            {
                                                db.subcatalogattachment.RemoveRange(item1.subcatalogattachment);
                                                item1.subcatalogattachment = null;
                                                db.SaveChanges();
                                            }
                                            db.subcatalog.RemoveRange(classes);
                                        }
                                        db.SaveChanges();
                                    }
                                }
                            }

                            #region 删除catalog
                            //删除catalog
                            int courseid = ids[0].courseid;
                            List<int> catalogids = new List<int>();
                            var needremovec = db.catalog.Where(p => p.courseid == courseid).ToArray();
                            foreach (var item in needremovec)
                            {
                                if (!ids.Any(p => p.id == item.id))
                                {
                                    catalogids.Add(item.id);
                                }
                            }
                            if(catalogids.Count > 0)
                            {
                                var querycatalogids = from s in db.catalog.Include("subcatalog").Include("subcatalog.subcatalogattachment")
                                                      where catalogids.Contains(s.id)
                                                      orderby s.id descending
                                                      select s;
                                var res_catalog = querycatalogids.ToList();
                                foreach (var item in res_catalog)
                                {
                                    foreach (var item1 in item.subcatalog)
                                    {
                                        db.subcatalogattachment.RemoveRange(item1.subcatalogattachment);
                                        item1.subcatalogattachment = null;
                                        db.SaveChanges();
                                    }
                                    db.subcatalog.RemoveRange(item.subcatalog);
                                    db.SaveChanges();
                                }
                                if (res_catalog.Count > 0)
                                {
                                    res_catalog.ForEach(l => l.subcatalog = null);
                                }
                                db.catalog.RemoveRange(res_catalog);
                            }
                            db.SaveChanges(); 
                            #endregion

                            tran.Commit();
                            return true;
                        }
                        catch
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
        /// 删除章节
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DelMain(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    using (HanYiContext db = new HanYiContext())
                    {
                        var query = from s in db.catalog.Include("subcatalog").Include("subcatalog.subcatalogattachment")
                                    where ids.Contains(s.id)
                                    orderby s.id descending
                                    select s;
                        var res = query.ToList();
                        foreach(var item in res)
                        {
                            foreach (var item1 in item.subcatalog)
                            {
                                db.subcatalogattachment.RemoveRange(item1.subcatalogattachment);
                                item1.subcatalogattachment = null;
                                db.SaveChanges();
                            }
                            db.subcatalog.RemoveRange(item.subcatalog);
                            db.SaveChanges();
                        }
                        if (res.Count > 0)
                        {
                            res.ForEach(l => l.subcatalog = null);
                        }
                        db.catalog.RemoveRange(res);
                        db.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
