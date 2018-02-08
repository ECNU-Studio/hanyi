using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class ClassaddressDAL : BaseDAL
    {
        /// <summary>
        /// 获取上课地址
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<classaddress>, int> getClassAddress(int classid, int currpage, int pagesize)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.classaddress.AsNoTracking()
                            where s.classid == classid
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<classaddress> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<classaddress>, int> res = new Tuple<List<classaddress>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delAddress(  List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.classaddress
                                where ids.Contains(s.id)                               
                                select s;
                    db.classaddress.RemoveRange(query.ToList());
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
