using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchAuthorService : ServiceBase<ResearchAuthor>, IResearchAuthorService
    {
        private readonly IResearchAuthorRepository _repository;

        public ResearchAuthorService(IResearchAuthorRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
