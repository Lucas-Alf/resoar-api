using System.Linq.Expressions;
using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Standard;

namespace Application.Services.Standard
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class, IIdentityEntity
    {
        protected readonly IRepositoryBase<TEntity> repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.Query(filter);
        }

        public virtual TEntity Add(TEntity obj)
        {
            return repository.Add(obj);
        }

        public virtual int AddRange(IEnumerable<TEntity> entities)
        {
            return repository.AddRange(entities);
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetAll(filter);
        }

        public virtual PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetPaged(page, pageSize, orderBy, filter);
        }

        public virtual PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetPagedAnonymous(page, pageSize, selector, orderBy, filter);
        }

        public virtual TEntity GetById(object id)
        {
            return repository.GetById(id);
        }

        public virtual bool Remove(object id)
        {
            return repository.Remove(id);
        }

        public virtual int Remove(TEntity obj)
        {
            return repository.Remove(obj);
        }

        public virtual int RemoveRange(IEnumerable<TEntity> entities)
        {
            return repository.RemoveRange(entities);
        }

        public virtual bool Update(TEntity obj)
        {
            return repository.Update(obj);
        }

        public virtual int UpdateRange(IEnumerable<TEntity> entities)
        {
            return repository.UpdateRange(entities);
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            return await repository.AddAsync(obj);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await repository.AddRangeAsync(entities);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetAllAsync(filter);
        }

        public virtual async Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetPagedAsync(page, pageSize, orderBy, filter);
        }

        public virtual async Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetPagedAnonymousAsync(page, pageSize, selector, orderBy, filter);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await repository.GetByIdAsync(id);
        }

        public virtual async Task<bool> RemoveAsync(object id)
        {
            return await repository.RemoveAsync(id);
        }

        public virtual async Task RemoveAsync(TEntity obj)
        {
            await repository.RemoveAsync(obj);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            await repository.RemoveRangeAsync(entities);
        }

        public virtual async Task<bool> UpdateAsync(TEntity obj)
        {
            return await repository.UpdateAsync(obj);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await repository.UpdateRangeAsync(entities);
        }
    }
}
