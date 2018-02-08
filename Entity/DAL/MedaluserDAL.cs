using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class MedaluserDAL : BaseDAL
    {
        /// <summary>
        /// 获取用户勋章
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<medaluser> getMedaluser(int userid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.medaluser.Include("classes.courses").Include("medal").AsNoTracking()
                            where s.userid == userid
                            orderby s.id  ascending
                            select s;
                var res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 获取最近三个
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<medaluser> getRecentMedal(int userid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.medaluser.Include("classes.courses").Include("medal").AsNoTracking()
                            where s.userid == userid
                            orderby s.id descending
                            select s;
                var res = query.Take(3).ToList();
                return res;
            }
        }
    }
}
