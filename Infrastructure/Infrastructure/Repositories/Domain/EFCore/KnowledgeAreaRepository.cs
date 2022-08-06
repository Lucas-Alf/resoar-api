using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class KnowledgeAreaRepository : DomainRepository<KnowledgeArea>, IKnowledgeAreaRepository
    {
        public KnowledgeAreaRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}