using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchKnowledgeAreaService : ServiceBase<ResearchKnowledgeArea>, IResearchKnowledgeAreaService
    {
        private readonly IResearchKnowledgeAreaRepository _repository;

        public ResearchKnowledgeAreaService(IResearchKnowledgeAreaRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
