using Domain.Entities;
using Domain.Models;
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

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
                return dbSet.AsQueryable();

            return dbSet.Where(filter).AsQueryable();
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

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
                return dbSet;

            return dbSet.Where(filter);
        }

        private PaginationQuery<TEntity> PaginateQuery(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
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

            query = query.Skip((page - 1) * pageSize);
            query = query.Take(pageSize);

            return new PaginationQuery<TEntity>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = total,
                Query = query
            };
        }

        public PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            var data = pagination.Query.ToList();

            return new PaginationModel<TEntity>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = data
            };
        }

        public PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            var data = pagination.Query
                .Select(selector)
                .ToList();

            return new PaginationModel<T>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = data
            };
        }

        public virtual TEntity? GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual bool Remove(object id)
        {
            TEntity? entity = GetById(id);

            if (entity == null) return false;

            return Remove(entity) > 0;
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

            if (avoidingAttachedEntity == null) return false;

            dbContext.Entry(avoidingAttachedEntity).State = EntityState.Detached;

            var entry = dbContext.Entry(obj);
            if (entry.State == EntityState.Detached) dbContext.Attach(obj);

            dbContext.Entry(obj).State = EntityState.Modified;
            return Commit() > 0;
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
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            var data = pagination.Query.ToList();

            return await Task.FromResult(new PaginationModel<TEntity>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = data
            });
        }

        public virtual async Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            var data = pagination.Query
                .Select(selector)
                .ToList();

            return await Task.FromResult(new PaginationModel<T>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = data
            });
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> RemoveAsync(object id)
        {
            TEntity? entity = await GetByIdAsync(id);

            if (entity == null) return false;

            return await RemoveAsync(entity) > 0;
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

            if (avoidingAttachedEntity == null) return false;

            dbContext.Entry(avoidingAttachedEntity).State = EntityState.Detached;

            var entry = dbContext.Entry(obj);
            if (entry.State == EntityState.Detached) dbContext.Attach(obj);

            dbContext.Entry(obj).State = EntityState.Modified;
            return await CommitAsync() > 0;
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

        protected override IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
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
            Expression<Func<TEntity, bool>>? filter)
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