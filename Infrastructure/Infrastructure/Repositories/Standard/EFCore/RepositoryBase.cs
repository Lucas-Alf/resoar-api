using System.Linq.Expressions;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Standard;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Standard.EFCore
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IIdentityEntity
    {

        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        protected RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        #region Add

        public virtual TEntity Add(TEntity obj)
        {
            var r = dbSet.Add(obj);
            Commit();
            return r.Entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            var r = await dbSet.AddAsync(obj);
            await CommitAsync();
            return r.Entity;
        }

        public virtual int AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
            return Commit();
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
            return await CommitAsync();
        }

        #endregion Add

        #region Update

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

        public virtual bool UpdateSomeFields(TEntity obj, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var avoidingAttachedEntity = GetById(obj.Id);

            if (avoidingAttachedEntity == null) return false;

            dbContext.Entry(avoidingAttachedEntity).State = EntityState.Detached;

            dbContext.Attach(obj);
            foreach (var includeProperty in includeProperties)
            {
                dbContext.Entry(obj).Property(includeProperty).IsModified = true;
            }

            return Commit() > 0;
        }

        public virtual int UpdateRange(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
            return Commit();
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

        public virtual async Task<bool> UpdateSomeFieldsAsync(TEntity obj, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var avoidingAttachedEntity = GetById(obj.Id);

            if (avoidingAttachedEntity == null) return false;

            dbContext.Attach(obj);
            foreach (var includeProperty in includeProperties)
            {
                dbContext.Entry(obj).Property(includeProperty).IsModified = true;
            }

            return await CommitAsync() > 0;
        }

        #endregion Update

        #region Delete

        public virtual bool Delete(object id)
        {
            TEntity? entity = GetById(id);

            if (entity == null) return false;

            return Delete(entity) > 0;
        }

        public virtual int Delete(TEntity obj)
        {
            dbSet.Remove(obj);
            return Commit();
        }

        public virtual int DeleteRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            return Commit();
        }

        public virtual async Task<bool> DeleteAsync(object id)
        {
            TEntity? entity = await GetByIdAsync(id);

            if (entity == null) return false;

            return await DeleteAsync(entity) > 0;
        }

        public virtual async Task<int> DeleteAsync(TEntity obj)
        {
            dbSet.Remove(obj);
            return await CommitAsync();
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            return await CommitAsync();
        }

        #endregion Delete

        #region GetAll

        public virtual IEnumerable<TEntity> GetAll(FilterBy<TEntity>? filter = null)
        {
            if (filter != null)
            {
                var compiledFilter = filter.Compile();
                if (compiledFilter != null)
                    return dbSet.Where(compiledFilter);
            }

            return dbSet;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(FilterBy<TEntity>? filter = null)
        {
            if (filter != null)
            {
                var compiledFilter = filter.Compile();
                if (compiledFilter != null)
                    return await Task.FromResult(dbSet.Where(compiledFilter));
            }

            return await Task.FromResult(dbSet);
        }

        #endregion GetAll

        #region GetPaged

        public PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            return new PaginationModel<TEntity>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = pagination.Query.ToList()
            };
        }

        public PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            return new PaginationModel<T>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = pagination.Query.Select(selector).ToList()
            };
        }

        public virtual async Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            return await Task.FromResult(new PaginationModel<TEntity>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = pagination.Query.ToList()
            });
        }

        public virtual async Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            var pagination = PaginateQuery(page, pageSize, orderBy, filter);
            return await Task.FromResult(new PaginationModel<T>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalRecords = pagination.TotalRecords,
                Records = pagination.Query.Select(selector).ToList()
            });
        }

        private PaginationQuery<TEntity> PaginateQuery(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            if (page == 0)
                page = 1;

            if (pageSize == 0)
                pageSize = 10;

            if (pageSize > 100)
                throw new Exception("O tamanho máximo de uma página é 100 registros");

            var query = dbSet.AsQueryable();

            if (filter != null)
            {
                var compiledFilter = filter.Compile();
                if (compiledFilter != null)
                    query = query.Where(compiledFilter);
            }

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

        #endregion GetPaged

        #region GetById

        public virtual TEntity? GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        #endregion GetById

        #region OtherMethods

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<TEntity> Query(FilterBy<TEntity>? filter = null)
        {
            if (filter != null)
            {
                var compiledFilter = filter.Compile();
                if (compiledFilter != null)
                    return dbSet.Where(compiledFilter).AsQueryable();
            }

            return dbSet.AsQueryable();
        }

        private int Commit()
        {
            return dbContext.SaveChanges();
        }

        private async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        #endregion OtherMethods
    }
}