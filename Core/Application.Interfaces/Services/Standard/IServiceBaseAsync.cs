namespace Application.Interfaces.Services.Standard
{
    public interface IServiceBaseAsync<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity obj);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> UpdateAsync(TEntity obj);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(object id);
        Task RemoveAsync(TEntity obj);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
