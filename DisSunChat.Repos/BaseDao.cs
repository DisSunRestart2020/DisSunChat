using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DisSunChat.Common;

namespace DisSunChat.Repos
{
    public class BaseDao
    {
        public DbContext db = new DBContextFactory().CreateDbContext();

        public IQueryable<T> LoadEntities<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            return db.Set<T>().Where<T>(whereLambda);
        }


        public IQueryable<T> LoadEntities<T, OrderType>(EFPager<T, OrderType> pager) where T : class
        {
            if (pager.PageSize < 1) pager.PageSize = 1;
            if (pager.PageIndex < 0) pager.PageIndex = 0;

            IQueryable<T> query = db.Set<T>().Where<T>(pager.WhereLambds);
            int rowsCount = query.Count();
            pager.SetTotalCount(rowsCount);
            int pagesCount = rowsCount % pager.PageSize == 0 ? (rowsCount / pager.PageSize) : (rowsCount / pager.PageSize + 1);
            pager.SetTotalPages(pagesCount);
            if (pager.PageIndex >= pagesCount) pager.PageIndex = (pagesCount<=0?0: pagesCount - 1);

            if (pager.IsAsc)
            {
                query = query.OrderBy<T, OrderType>(pager.OrderByLambds);               
            }
            else
            {
                query = query.OrderByDescending<T, OrderType>(pager.OrderByLambds);
            }
           
            query = query.Skip<T>(pager.PageSize * pager.PageIndex).Take<T>(pager.PageSize);


            return query;
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
