using Domain.Entities;
using Infrastructure.Interfaces.Repositories.EFCore;
using Infrastructure.Interfaces.Repositories.Standard;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Standard.EFCore
{
    public class RepositoryBase<TEntity> : SpecificMethods<TEntity>, IRepositoryBase<TEntity> where TEntity : class, IIdentityEntity
    {

        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        protected RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual TEntity Add(TEntity obj)
        {
            var r = dbSet.Add(obj);
            Commit();
            return r.Entity;
        }

        public virtual int AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
            return Commit();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual bool Remove(object id)
        {
            TEntity entity = GetById(id);

            if (entity == null) return false;

            return Remove(entity) > 0 ? true : false;
        }

        public virtual int Remove(TEntity obj)
        {
            dbSet.Remove(obj);
            return Commit();
        }

        public virtual int RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            return Commit();
        }

        public virtual bool Update(TEntity obj)
        {
            var avoidingAttachedEntity = GetById(obj.Id);
            dbContext.Entry(avoidingAttachedEntity).State = EntityState.Detached;

            var entry = dbContext.Entry(obj);
            if (entry.State == EntityState.Detached) dbContext.Attach(obj);

            dbContext.Entry(obj).State = EntityState.Modified;
            return Commit() > 0 ? true : false;
        }

        public virtual int UpdateRange(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
            return Commit();
        }

        private int Commit()
        {
            return dbContext.SaveChanges();
        }

        #region ProtectedMethods
        protected override IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            query = GenerateQueryableWhereExpression(query, filter);
            query = GenerateIncludeProperties(query, includeProperties);

            if (orderBy != null)
                return orderBy(query);

            return query;
        }
        private IQueryable<TEntity> GenerateQueryableWhereExpression(IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> filter)
        {
            if (filter != null)
                return query.Where(filter);

            return query;
        }

        private IQueryable<TEntity> GenerateIncludeProperties(IQueryable<TEntity> query, params string[] includeProperties)
        {
            foreach (string includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return query;
        }

        protected override IEnumerable<TEntity> GetYieldManipulated(IEnumerable<TEntity> entities, Func<TEntity, TEntity> DoAction)
        {
            foreach (var e in entities)
            {
                yield return DoAction(e);
            }
            yield break;
        }
        #endregion ProtectedMethods
    }
}