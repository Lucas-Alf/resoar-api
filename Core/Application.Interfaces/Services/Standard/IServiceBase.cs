using System.Linq.Expressions;
using Domain.Models;

namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        ResultMessageModel Add(TEntity obj);
        ResultMessageModel AddRange(IEnumerable<TEntity> entities);
        ResultMessageModel Remove(int id);
        ResultMessageModel Remove(TEntity obj);
        ResultMessageModel RemoveRange(IEnumerable<TEntity> entities);
        ResultMessageModel Update(TEntity obj);
        ResultMessageModel UpdateRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<ResultMessageModel> AddAsync(TEntity obj);
        Task<ResultMessageModel> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<ResultMessageModel> RemoveAsync(int id);
        Task<ResultMessageModel> RemoveAsync(TEntity obj);
        Task<ResultMessageModel> RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<ResultMessageModel> UpdateAsync(TEntity obj);
        Task<ResultMessageModel> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity?> GetByIdAsync(int id);
        TEntity? GetById(int id);
    }
}
