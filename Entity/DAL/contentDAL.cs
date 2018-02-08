using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class ContentDAL : BaseDAL
    {
        /// <summary>
        /// 根据班级 获取问答
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public static List<comment> getCLasses(int classesid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.comment.Include("user").Include("commentsub").Include("commentsub.user").AsNoTracking()
                            where s.classid == classesid
                            orderby s.id descending
                            select s;
                List<comment> res = query.ToList();
                return res;
            }
        }
        /// <summary>
        /// 统计用户评论过的课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionary<int, bool> getUserComment(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                Dictionary<int, bool> res = new Dictionary<int, bool>();
                var query = from s in db.comment
                            where s.userid == id
                            group s by s.classid into g
                            select new { classid = g.Key, count = g.Count() };
                var temp = query.ToList();
                foreach (var item in temp)
                {
                    if (item.count > 0)
                    {
                        res.Add(item.classid, true);
                    }
                    else
                    {
                        res.Add(item.classid, false);

                    }
                }
                return res;
            }
        }
    }
}
