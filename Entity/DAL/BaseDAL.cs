using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public class BaseDAL
    {
        public static TEntity AddModel<TEntity>(TEntity entry) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                _db.Set<TEntity>().Add(entry);
                _db.SaveChanges();
                return entry;
            }
        }


        public static bool AddModel<TEntity>(List<TEntity> entry) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                _db.Set<TEntity>().AddRange(entry);
                _db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entry"></param>
        /// <param name="ID"></param>
        public static void EditEntry<TEntity>(TEntity entity, string ID) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                _db.UpdateEntryByProperty<TEntity>(entity, ID);
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TEntity getEntryById<TEntity, T>(T id) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                return _db.Set<TEntity>().Find(id);
            }
        }

        /// <summary>
        /// 根据id删除实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public static void DeleteEntry<TEntity, T>(T id) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                TEntity entity = _db.Set<TEntity>().Find(id);
                _db.Set<TEntity>().Attach(entity);
                _db.Set<TEntity>().Remove(entity);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entry"></param>
        public static void DeleteEntry<TEntity>(TEntity entity) where TEntity : class
        {
            using (HanYiContext _db = new HanYiContext())
            {
                _db.Set<TEntity>().Attach(entity);
                _db.Set<TEntity>().Remove(entity);
                _db.SaveChanges();
            }
        }
    }
}
