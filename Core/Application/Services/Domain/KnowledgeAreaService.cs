using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class KnowledgeAreaService : ServiceBase<KnowledgeArea>, IKnowledgeAreaService
    {
        private readonly IKnowledgeAreaRepository _repository;

        public KnowledgeAreaService(IKnowledgeAreaRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
