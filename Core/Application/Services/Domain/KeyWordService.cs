using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class KeyWordService : ServiceBase<KeyWord>, IKeyWordService
    {
        private readonly IKeyWordRepository _repository;

        public KeyWordService(IKeyWordRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize, string? description)
        {
            var filter = new FilterBy<KeyWord>();

            if (!String.IsNullOrEmpty(description))
                filter.Add(x => EF.Functions.ILike(x.Description!, $"%{description.Trim()}%"));

            return GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Description!,
                filter: filter,
                selector: x => new
                {
                    x.Id,
                    x.Description
                }
            );
        }
    }
}
