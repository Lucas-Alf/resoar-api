using System.Linq.Expressions;
using Domain.Models;

namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBaseAsync<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity obj);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<object>> GetPagedAnonymousAsync(int page, int pageSize, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<bool> UpdateAsync(TEntity obj);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(object id);
        Task RemoveAsync(TEntity obj);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
