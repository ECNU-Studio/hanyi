using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class MedalDAL : BaseDAL
    {
        /// <summary>
        /// 获取 所以勋章
        /// </summary>
        /// <returns></returns>
        public static List<medal> getMedal()
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.medal.AsNoTracking()
                            where s.state == true
                            orderby s.id ascending
                            select s;              
               var res = query.ToList();
               return res;
            }
        }
    }
}
