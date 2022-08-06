using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class InstitutionRepository : DomainRepository<Institution>, IInstitutionRepository
    {
        public InstitutionRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}