using HanYi.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DAL
{
    public static class UpdateExtension
    {
        public static int UpdateEntryByProperty<T>(this HanYiContext _db, T entity, string EntityKey) where T : class
        {
            DbSet<T> dbSet = _db.Set<T>();
            dbSet.Attach(entity);
            MemberInfo[] members = entity.GetType().GetMembers();
            IEnumerable<MemberInfo> properties = members.Where(m => m.MemberType == MemberTypes.Property && m.Name != EntityKey);
            foreach (MemberInfo mInfo in properties)
            {

                object o = entity.GetType().InvokeMember(mInfo.Name, BindingFlags.GetProperty, null, entity, null);
                if (o != null)
                {
                    if (o.GetType().IsPrimitive || o.GetType().IsPublic)
                    {
                        try
                        {
                            DbEntityEntry entry = _db.Entry<T>(entity);
                            entry.Property(mInfo.Name).IsModified = true;
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            DbEntityEntry entry = _db.Entry<T>(entity);
                            entry.Property(mInfo.Name).IsModified = true;
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    try
                    {
                        DbEntityEntry entry = _db.Entry<T>(entity);
                        entry.Property(mInfo.Name).IsModified = true;
                    }
                    catch
                    {

                    }
                }
            }

            return _db.SaveChanges();
        }
    }
}
