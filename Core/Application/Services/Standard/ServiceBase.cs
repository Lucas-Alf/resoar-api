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

        public virtual PaginationModel<object> GetPagedAnonymous(int page, int pageSize, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
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
    }
}
