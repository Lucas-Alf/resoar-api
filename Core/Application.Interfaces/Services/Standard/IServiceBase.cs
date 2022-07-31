using System.Linq.Expressions;
using Domain.Models;

namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        bool Remove(object id);
        bool Update(TEntity obj);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        int AddRange(IEnumerable<TEntity> entities);
        int Remove(TEntity obj);
        int RemoveRange(IEnumerable<TEntity> entities);
        int UpdateRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task RemoveAsync(TEntity obj);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(object id);
        Task<bool> UpdateAsync(TEntity obj);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity> AddAsync(TEntity obj);
        Task<TEntity> GetByIdAsync(object id);
        TEntity Add(TEntity obj);
        TEntity GetById(object id);
    }
}
