using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
   public class AdminDAL:BaseDAL
   {
       /// <summary>
       /// 用户登录
       /// </summary>
       /// <param name="username"></param>
       /// <returns></returns>
       public static admin login(string username)
       {
           using (HanYiContext _dc = new HanYiContext())
           {
               var q = from u in _dc.admin
                       where  u.username == username || u.phone == username 
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
       public static admin findUserByUsername(string username)
       {
           using (HanYiContext _dc = new HanYiContext())
           {
               var q = from u in _dc.admin
                       where   u.username == username
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
       public static admin findUserByTel(string tel)
       {
           using (HanYiContext _dc = new HanYiContext())
           {
               var q = from u in _dc.admin
                       where   u.phone == tel
                       select u;
               var res = q.FirstOrDefault();
               return res;
           }
       }

       /// <summary>
       /// 管理员列表
       /// </summary>
       /// <param name="currpage"></param>
       /// <param name="pagesize"></param>
       /// <returns></returns>
       public static Tuple<List<admin>, int> getAdmin(int currpage, int pagesize)
       {
           using (HanYiContext db = new HanYiContext())
           {
               var query = from s in db.admin.AsNoTracking()                         
                           orderby s.id descending
                           select s;
               int total = query.Count();
               List<admin> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
               Tuple<List<admin>, int> res = new Tuple<List<admin>, int>(list, total);
               return res;
           }
       }


       /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
       public static bool delAdmin(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.admin 
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                    var admin = query.ToList();

                    db.admin.RemoveRange(admin);
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
