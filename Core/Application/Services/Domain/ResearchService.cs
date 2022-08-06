using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchService : ServiceBase<Research>, IResearchService
    {
        private readonly IResearchRepository _repository;

        public ResearchService(IResearchRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
