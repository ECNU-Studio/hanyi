using Entity.Entity;
using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
   public class ClassmodelsDAL:BaseDAL
   {
       /// <summary>
       /// 根据班级 获取模块
       /// </summary>
       /// <param name="courseid"></param>
       /// <returns></returns>
       public static List<classmodels> getCLasses(int classesid)
       {
           using (HanYiContext db = new HanYiContext())
           {
               var query = from s in db.classmodels.AsNoTracking()
                           where s.classesid == classesid
                           orderby s.id 
                           select s;
               List<classmodels> res = query.ToList();
               return res;
           }
       }
       /// <summary>
       /// 删除该课程所有模块
       /// </summary>
       /// <param name="classesid"></param>
       /// <returns></returns>
       public static bool removeAll(int classesid)
       {
           using (HanYiContext db = new HanYiContext())
           {
               var query = from s in db.classmodels
                           where s.classesid == classesid                           
                           select s;
                 db.classmodels.RemoveRange( query.ToList());
                 db.SaveChanges();
               return true;
           }
       }

       public static bool removeUpdate(List<classmodels> model, int classesid)
       {
           using (HanYiContext db = new HanYiContext())
           {
               var query = from s in db.classmodels
                           where s.classesid == classesid
                           select s;
               var templist = query.ToList();
               //删除未在新加列表中的 
               if(templist!=null)
                   foreach (var itemout in templist)
                   {
                       var hasfind = false;
                       foreach (var itemin in model)
                       {
                           if (itemout.id == itemin.id)
                           {
                               hasfind = true;
                               itemout.name = itemin.name;
                               itemout.path = itemin.path;
                               itemout.percent = itemin.percent;
                               itemout.title = itemin.title;
                               itemout.type = itemin.type;
                           }

                       }
                       if (!hasfind)
                       {
                           db.classmodels.Remove(itemout);
                       }

                   }

               foreach(var item in model)
               {
                   if(item.id<=0)
                   {
                       db.classmodels.Add(item);
                   }
               }           
               db.SaveChanges();
               return true;
           }
       }
    }
}
