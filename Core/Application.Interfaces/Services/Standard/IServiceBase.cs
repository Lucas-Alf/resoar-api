using System.Linq.Expressions;
using Domain.Models;

namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity obj);
        int AddRange(IEnumerable<TEntity> entities);
        TEntity GetById(object id);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        PaginationModel<object> GetPagedAnonymous(int page, int pageSize, Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null);
        bool Update(TEntity obj);
        int UpdateRange(IEnumerable<TEntity> entities);
        bool Remove(object id);
        int Remove(TEntity obj);
        int RemoveRange(IEnumerable<TEntity> entities);
    }
}
