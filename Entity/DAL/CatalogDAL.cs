using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class CatalogDAL:BaseDAL
    {
        /// <summary>
        /// 根据课程获取子目录
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public static List<catalog> getCLasses(int courseid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.catalog.Include("subcatalog").Include("subcatalog.subcatalogattachment").AsNoTracking()
                            where s.state == true && s.courseid == courseid
                            orderby s.id ascending
                            select s;
                List<catalog> res = query.ToList();
                
                return res;
            }
        }
    }
}
