using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchAuthorRepository : DomainRepository<ResearchAuthor>, IResearchAuthorRepository
    {
        public ResearchAuthorRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}