using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchKeyWordService : ServiceBase<ResearchKeyWord>, IResearchKeyWordService
    {
        private readonly IResearchKeyWordRepository _repository;

        public ResearchKeyWordService(IResearchKeyWordRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
