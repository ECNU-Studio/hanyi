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
    public class TeacheresDAL : BaseDAL
    {
        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<teacheres>, int> getCompanys(int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.teacheres.Include("courses").AsNoTracking()
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<teacheres> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<teacheres>, int> res = new Tuple<List<teacheres>, int>(list, total);
                return res;
            }
        }

        /// <summary>
        /// 获取全部培训师
        /// </summary>
        /// <returns></returns>
        public static Tuple<List<teacheres>, int> getAllTeacher()
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.teacheres.Include("courses").AsNoTracking()
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<teacheres> list = query.ToList();
                Tuple<List<teacheres>, int> res = new Tuple<List<teacheres>, int>(list, total);
                return res;
            }
        }

        public static List<teacheres> MatchTeacher(string name, int num)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.teacheres.AsNoTracking()
                            where string.IsNullOrEmpty(name) || s.name.Contains(name)
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<teacheres> list = query.Take(num).ToList();
                return list;
            }
        }

        /// <summary>
        /// 培训师登录
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static teacheres login(string username)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.teacheres
                        where u.state == true &&( u.username == username||u.phone==username)
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        /// <summary>
        /// 根据登录查找教师
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static teacheres findUserByUsername(string username)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.teacheres
                        where u.state == true && u.username == username
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }

        /// <summary>
        /// 根据手机查找教师 
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static teacheres findUserByPhone(string phone)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.teacheres
                        where u.state == true && u.phone == phone
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delTeacheres(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {


                using (HanYiContext db = new HanYiContext())
                {
                    using (DbContextTransaction tran = db.Database.BeginTransaction())
                    {
                        try
                        {
                             var query = from s in db.teacheres
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                                db.teacheres.RemoveRange(query.ToList());
                                db.SaveChanges();

                            var query_album = from s in db.album
                                              where (s.teacherid.HasValue && ids.Contains(s.teacherid.Value))
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
        /// 更新用户列表回复提示
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool updateNewQa(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.teacheres
                                where ids.Contains(s.id)
                                select s;
                    var temp = query.ToList();
                    foreach (var item in temp)
                    {
                        item.new_ans = true;
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
