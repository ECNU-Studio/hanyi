using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class CompanysDAL : BaseDAL
    { /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static companys login(string account)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.companys
                        where u.state == true &&  u.account == account 
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        /// <summary>
        /// 根据登录查找企业 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static companys findCompanysByAccount(string account)
        {
            using (HanYiContext _dc = new HanYiContext())
            {
                var q = from u in _dc.companys
                        where u.state == true && u.account == account
                        select u;
                var res = q.FirstOrDefault();
                return res;
            }
        }
        
        /// <summary>
        /// 企业列表
        /// </summary>
        /// <param name="currpage"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static Tuple<List<companys>, int> getCompanys(int currpage, int pagesize)
        { 
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.companys.AsNoTracking() 
                            where s.state==true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<companys> list = query.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();
                Tuple<List<companys>, int> res = new Tuple<List<companys>, int>(list, total);
                return res;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool delCompanys(List<int> ids)
        {
            if(ids!=null&&ids.Count>0)
            {


                using (HanYiContext db = new HanYiContext())
                {
                    var query = from s in db.companys
                                where ids.Contains(s.id)
                                orderby s.id descending
                                select s;
                    db.companys.RemoveRange(query.ToList());
                    db.SaveChanges();
                    return true;
                }
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据企业名字匹配企业
        /// </summary>
        /// <param name="name"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<companys> MatchCompany(string name, int num)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.companys.AsNoTracking()
                            where string.IsNullOrEmpty(name) || s.name.Contains(name)
                            where s.state == true
                            orderby s.id descending
                            select s;
                int total = query.Count();
                List<companys> list = query.Take(num).ToList();
                return list;
            }
        }

    }
}
