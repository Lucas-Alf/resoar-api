using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchKnowledgeAreaRepository : DomainRepository<ResearchKnowledgeArea>, IResearchKnowledgeAreaRepository
    {
        public ResearchKnowledgeAreaRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}