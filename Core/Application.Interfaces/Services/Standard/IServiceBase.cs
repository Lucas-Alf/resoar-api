﻿namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity obj);
        int AddRange(IEnumerable<TEntity> entities);
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        bool Update(TEntity obj);
        int UpdateRange(IEnumerable<TEntity> entities);
        bool Remove(object id);
        int Remove(TEntity obj);
        int RemoveRange(IEnumerable<TEntity> entities);
    }
}
