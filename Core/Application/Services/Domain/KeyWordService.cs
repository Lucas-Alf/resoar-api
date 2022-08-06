using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class KeyWordService : ServiceBase<KeyWord>, IKeyWordService
    {
        private readonly IKeyWordRepository _repository;

        public KeyWordService(IKeyWordRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
