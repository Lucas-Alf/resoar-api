using System.Linq.Expressions;
using Domain.Entities;
using Domain.Models;

namespace Infrastructure.Interfaces.Repositories.Standard
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class, IIdentityEntity
    {
        TEntity Add(TEntity obj);
        int AddRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null);
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        bool Update(TEntity obj);
        int UpdateRange(IEnumerable<TEntity> entities);
        bool Remove(object id);
        int Remove(TEntity obj);
        int RemoveRange(IEnumerable<TEntity> entities);
    }
}