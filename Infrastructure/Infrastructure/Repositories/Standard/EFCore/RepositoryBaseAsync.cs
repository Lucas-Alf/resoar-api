using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.EFCore;
using Infrastructure.Interfaces.Repositories.Standard;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Standard.EFCore
{
    public class RepositoryBaseAsync<TEntity> : SpecificMethods<TEntity>, IRepositoryBaseAsync<TEntity> where TEntity : class, IIdentityEntity
    {

        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        protected RepositoryBaseAsync(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
                return dbSet.AsQueryable();

            return dbSet.Where(filter).AsQueryable();
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            var r = await dbSet.AddAsync(obj);
            await CommitAsync();
            return r.Entity;
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
            return await CommitAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
                return await Task.FromResult(dbSet);

            return await Task.FromResult(dbSet.Where(filter));
        }

        public virtual async Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            if (page == 0)
                page = 1;

            if (pageSize == 0)
                pageSize = 10;

            var query = dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var total = query.Count();

            if (orderBy != null)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderBy(x => x.Id);

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PaginationModel<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = total,
                Records = data
            });
        }

        public virtual async Task<PaginationModel<object>> GetPagedAnonymousAsync(int page, int pageSize, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            if (page == 0)
                page = 1;

            if (pageSize == 0)
                pageSize = 10;

            var query = dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var total = query.Count();

            if (orderBy != null)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderBy(x => x.Id);

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToList();

            return await Task.FromResult(new PaginationModel<object>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = total,
                Records = data
            });
        }


        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> RemoveAsync(object id)
        {
            TEntity entity = await GetByIdAsync(id);

            if (entity == null) return false;

            return await RemoveAsync(entity) > 0 ? true : false;
        }

        public virtual async Task<int> RemoveAsync(TEntity obj)
        {
            dbSet.Remove(obj);
            return await CommitAsync();
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            return await CommitAsync();
        }

        public virtual async Task<bool> UpdateAsync(TEntity obj)
        {
            var avoidingAttachedEntity = await GetByIdAsync(obj.Id);
            dbContext.Entry(avoidingAttachedEntity).State = EntityState.Detached;

            var entry = dbContext.Entry(obj);
            if (entry.State == EntityState.Detached) dbContext.Attach(obj);

            dbContext.Entry(obj).State = EntityState.Modified;
            return await CommitAsync() > 0 ? true : false;
        }

        public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
            return await CommitAsync();
        }

        private async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync();
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