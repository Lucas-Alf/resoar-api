using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchAdvisorService : ServiceBase<ResearchAdvisor>, IResearchAdvisorService
    {
        private readonly IResearchAdvisorRepository _repository;

        public ResearchAdvisorService(IResearchAdvisorRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
