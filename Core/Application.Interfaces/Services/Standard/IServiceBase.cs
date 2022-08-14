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
        ResponseMessageModel Add(TEntity obj);
        ResponseMessageModel AddRange(IEnumerable<TEntity> entities);
        ResponseMessageModel Remove(int id);
        ResponseMessageModel Remove(TEntity obj);
        ResponseMessageModel RemoveRange(IEnumerable<TEntity> entities);
        ResponseMessageModel Update(TEntity obj);
        ResponseMessageModel UpdateRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        Task<ResponseMessageModel> AddAsync(TEntity obj);
        Task<ResponseMessageModel> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<ResponseMessageModel> RemoveAsync(int id);
        Task<ResponseMessageModel> RemoveAsync(TEntity obj);
        Task<ResponseMessageModel> RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task<ResponseMessageModel> UpdateAsync(TEntity obj);
        Task<ResponseMessageModel> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity?> GetByIdAsync(int id);
        TEntity? GetById(int id);
    }
}
