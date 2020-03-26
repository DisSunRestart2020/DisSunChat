using DisSunChat.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Services
{
    public class BaseService
    {
        private BaseDao dao = new BaseDao();
        public DbContext Db
        {
            get
            {
                return dao.db;
            }
        }

        public IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return dao.LoadEntities(whereLambda);
        }       

        public T SaveEntity<T>(T entity) where T : class, new()
        {
            return dao.SaveEntity<T>(entity);
        }

        public T EditEntity<T>(T entity) where T : class, new()
        {
            return dao.EditEntity<T>(entity);
        }
         
    }
}
