/***********************************************************************************************************/
/* Generic Repository - to ensure there is no reduntant code & result in partial updates                   */
/* E.g: if you update 2 different entity types as part of same                                             */
/*       transaction and uses separate db context which may result one succeed and another fail            */
/***********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;

using CollateralCreatorAdminWeb.Models;

namespace CollateralCreatorAdminWeb.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal XeroxCCToolEntities context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(XeroxCCToolEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetByID(object id, object value)
        {
            return dbSet.Find(id, value);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);           
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}