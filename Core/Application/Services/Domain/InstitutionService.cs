using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class InstitutionService : ServiceBase<Institution>, IInstitutionService
    {
        private readonly IInstitutionRepository _repository;

        public InstitutionService(IInstitutionRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
