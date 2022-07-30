using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain.Standard;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Standard.EFCore
{
    public class DomainRepository<TEntity> : RepositoryBase<TEntity>, IDomainRepository<TEntity> where TEntity : class, IIdentityEntity
    {
        protected DomainRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
