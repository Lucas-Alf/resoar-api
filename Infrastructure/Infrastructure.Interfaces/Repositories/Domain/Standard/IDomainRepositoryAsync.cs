using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Standard;

namespace Infrastructure.Interfaces.Repositories.Domain.Standard
{
    public interface IDomainRepositoryAsync<TEntity> : IRepositoryBaseAsync<TEntity> where TEntity : class, IIdentityEntity
    {
    }
}