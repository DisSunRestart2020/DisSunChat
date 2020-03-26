using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DisSunChat.Repos
{
    public class BaseDao
    {
        public DbContext db = new DBContextFactory().CreateDbContext();

        public IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return db.Set<T>().Where<T>(whereLambda);
        }                

        public T SaveEntity<T>(T entity) where T : class, new()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Set<T>().Add(entity);
            db.SaveChanges();
            return entity;
        }

        public T EditEntity<T>(T entity) where T : class, new()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            db.SaveChanges();
            return entity;
        }
    }
}
