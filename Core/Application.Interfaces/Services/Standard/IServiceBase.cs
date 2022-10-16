using System.Linq.Expressions;
using Domain.Enums;
using Domain.Models;
using Domain.Utils;

namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(FilterBy<TEntity>? filter = null);
        IQueryable<TEntity> Query(FilterBy<TEntity>? filter = null);
        PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        ResponseMessageModel Add(TEntity obj);
        ResponseMessageModel AddRange(IEnumerable<TEntity> entities);
        ResponseMessageModel Delete(int id);
        ResponseMessageModel Delete(TEntity obj);
        ResponseMessageModel DeleteRange(IEnumerable<TEntity> entities);
        ResponseMessageModel Update(TEntity obj);
        ResponseMessageModel UpdateRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync(FilterBy<TEntity>? filter = null);
        Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        Task<ResponseMessageModel> AddAsync(TEntity obj);
        Task<ResponseMessageModel> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<ResponseMessageModel> DeleteAsync(int id);
        Task<ResponseMessageModel> DeleteAsync(TEntity obj);
        Task<ResponseMessageModel> DeleteRangeAsync(IEnumerable<TEntity> entities);
        Task<ResponseMessageModel> UpdateAsync(TEntity obj);
        Task<ResponseMessageModel> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<ResponseMessageModel> GetByIdAsync(int id);
        ResponseMessageModel GetById(int id);
    }
}
