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
     
    public class AlbumDAL : BaseDAL
    {
        /// <summary>
        /// 根据班级 获取相册
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public static List<album> getCLasses(int classesid)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.album.Include("user").Include("teacher").Include("albumsub").Include("albumsub.teacher").Include("albumsub.user").AsNoTracking()
                            where s.classid == classesid
                            orderby s.id descending
                            select s;
                List<album> res = query.ToList();
                return res;
            }
        }
      
        public static album getAlbumById(int id)
        {
            using (HanYiContext db = new HanYiContext())
            {
                var query = from s in db.album.Include("user").Include("teacher").Include("albumsub").Include("albumsub.teacher").Include("albumsub.user").AsNoTracking()
                            where s.id == id
                            orderby s.id descending
                            select s;
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="ids_parent"></param>
        /// <returns></returns>
        public static bool DelAlbum(List<string> ids, List<int> ids_parent)
        {
            using (HanYiContext db = new HanYiContext())
            {
                using (DbContextTransaction tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (ids_parent != null && ids_parent.Count > 0)
                        {
                            var query = from s in db.album
                                        where ids_parent.Contains(s.id)
                                        orderby s.id descending
                                        select s;
                            var list = query.ToList();
                            db.album.RemoveRange(list);
                            db.SaveChanges();
                        }
                        Dictionary<int,string> dic = new Dictionary<int,string>();
                        if (ids != null && ids.Count > 0)
                        {
                            foreach(var item in ids)
                            {
                                var arr = item.ToString().Split('-');
                                if (ids_parent != null && ids_parent.Contains(int.Parse(arr[0])))
                                {
                                    continue;
                                }
                                if(dic.Keys.Contains(int.Parse(arr[0])))
                                {
                                    var old = dic[int.Parse(arr[0])];
                                    var now = old + "," + arr[1];
                                    dic[int.Parse(arr[0])] = now;
                                }
                                else
                                {
                                    dic.Add(int.Parse(arr[0]), arr[1]);
                                }
                            }
                            
                            if (dic.Count > 0)
                            {
                                foreach(var item in dic)
                                {
                                    var model = db.album.First(l => l.id == item.Key);
                                    if (model != null)
                                    {
                                        string[] pic = item.Value.Split(',');
                                        foreach(var picItem in pic)
                                        {
                                            switch(picItem)
                                            {
                                                case "1":
                                                    model.pic1 = "";
                                                    break;
                                                case "2":
                                                    model.pic2 = "";
                                                    break;
                                                case "3":
                                                    model.pic3 = "";
                                                    break;
                                                case "4":
                                                    model.pic4 = "";
                                                    break;
                                                case "5":
                                                    model.pic5 = "";
                                                    break;
                                                case "6":
                                                    model.pic6 = "";
                                                    break;
                                            }
                                        }
                                        db.Entry<album>(model).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
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

    }
}
