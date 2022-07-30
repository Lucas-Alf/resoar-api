using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain.Standard;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Standard.EFCore
{
    public class DomainRepositoryAsync<TEntity> : RepositoryBaseAsync<TEntity>, IDomainRepositoryAsync<TEntity> where TEntity : class, IIdentityEntity
    {
        protected DomainRepositoryAsync(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
