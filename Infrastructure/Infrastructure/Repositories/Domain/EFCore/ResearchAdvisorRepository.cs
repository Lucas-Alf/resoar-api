using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchAdvisorRepository : DomainRepository<ResearchAdvisor>, IResearchAdvisorRepository
    {
        public ResearchAdvisorRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}