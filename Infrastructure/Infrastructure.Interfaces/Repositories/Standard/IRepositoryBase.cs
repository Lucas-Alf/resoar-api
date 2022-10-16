using System.Linq.Expressions;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Utils;

namespace Infrastructure.Interfaces.Repositories.Standard
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class, IIdentityEntity
    {
        bool Delete(object id);
        bool Update(TEntity obj);
        bool UpdateSomeFields(TEntity obj, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAll(FilterBy<TEntity>? filter = null);
        int AddRange(IEnumerable<TEntity> entities);
        int Delete(TEntity obj);
        int DeleteRange(IEnumerable<TEntity> entities);
        int UpdateRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query(FilterBy<TEntity>? filter = null);
        PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        Task<bool> DeleteAsync(object id);
        Task<bool> UpdateAsync(TEntity obj);
        Task<bool> UpdateSomeFieldsAsync(TEntity obj, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAllAsync(FilterBy<TEntity>? filter = null);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<int> DeleteAsync(TEntity obj);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities);
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, OrderDirection? orderDirection = null, FilterBy<TEntity>? filter = null);
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity> AddAsync(TEntity obj);
        TEntity Add(TEntity obj);
        TEntity? GetById(object id);
    }
}