using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class KeyValueDAL : BaseDAL
    {
        public static keyvalue fingByKey(string key)
        {

            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.keyvalue.AsNoTracking()
                            where s.key == key
                            orderby s.id descending
                            select s;
                var res = query.FirstOrDefault();
                if (res == null)
                {
                    res = new keyvalue() { key = key }; 
                }
                return res;
            }

        }
        /// <summary>
        /// 添加key
        /// </summary>
        /// <param name="kv"></param>
        public static void SetKeyValue(keyvalue kv)
        {
            using (HanYiContext db = new HanYiContext())
            {
                if (kv.id > 0)
                {
                    var query = from s in db.keyvalue
                                where s.id == kv.id
                                orderby s.id descending
                                select s;
                    var res = query.FirstOrDefault();
                    if (res != null)
                    {
                        res.dateexpiry = kv.dateexpiry;
                        res.dateintput = kv.dateintput;
                        res.value = kv.value;
                    }
                    else
                    {
                        db.keyvalue.Add(kv);
                    }
                }
                else
                {
                    db.keyvalue.Add(kv);
                }
                db.SaveChanges();
            }
        }
    }
}
