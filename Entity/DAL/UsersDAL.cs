using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class UsersDAL : BaseDAL
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static users login(string username)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.users
                        where u.state == true && (u.username == username || u.tel == username)
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        /// <summary>
        /// 根据openid获取用户
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static users getUserInfobyOpenId(string openid)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.users
                        where u.state == true && u.openid == openid  
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }

        
        /// <summary>
        /// 根据登录查找用户  
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static users findUserByUsername(string username)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.users
                        where u.state == true &&  u.username == username 
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        /// <summary>
        /// 根据手机查找用户 
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static users findUserByTel(string tel)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.users
                        where u.state == true && u.tel == tel
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="companyid">企业id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<users>, int> getCompanys(int companyid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.users.Include("classstudent").Include("classstudent.classes").Include("classstudent.classes.courses").AsNoTracking()
                            where s.state == true && s.companyid == companyid
                            orderby s.username descending
                            select s;
                int total = query.Count();
                List<users> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<users>, int> res = new Tuple<List<users>, int>(list, total);
                return res;
            }
        }


        /// <summary>
        /// 企业未添加用户列表
        /// </summary>
        /// <param name="companyid">企业id</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<users>, int> getCompanysNot(int companyid, int currpage, int pagesize, int classid)
        {
            using (HanYiContext db = new HanYiContext())
            {

                var query = from s in db.users.Include("classstudent").Include("classstudent.classes").Include("classstudent.classes.courses").AsNoTracking()
                            where s.state == true && s.companyid == companyid
                            where !s.classstudent.Any(l => l.userid == s.id && l.classid == classid)
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<users> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<users>, int> res = new Tuple<List<users>, int>(list, total);
                return res;
            }
        }

        /// <summary>
        /// 用户列表列表
        /// </summary>
        /// <param name="classid">班级</param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<users>, int> getClassUser(int classid,string username, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.users.Include("company").AsNoTracking()
                            where s.state == true
                            where string.IsNullOrEmpty(username)||s.name.Contains(username)
                            where s.classstudent.Any(p=>p.classid==classid)
                            orderby s.username descending
                            select s;
                int total = query.Count();
                List<users> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<users>, int> res = new Tuple<List<users>, int>(list, total);
                return res;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delUsers(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.users
                                where ids.Contains(s.id)                                
                                select s;
                    db.users.RemoveRange(query.ToList());
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
        /// 更新用户列表回复提示
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool updateNewQa(List<int>ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.users
                                where ids.Contains(s.id)
                                select s;
                    var temp = query.ToList();
                    foreach(var item in temp)
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
