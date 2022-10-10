using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class InstitutionService : ServiceBase<Institution>, IInstitutionService
    {
        private readonly IInstitutionRepository _repository;

        public InstitutionService(IInstitutionRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize, string? name)
        {
            var filter = new FilterBy<Institution>();

            if (!String.IsNullOrEmpty(name))
                filter.Add(x => EF.Functions.ILike(x.Name!, $"%{name.Trim()}%"));

            return GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Name!,
                filter: filter,
                selector: x => new
                {
                    x.Id,
                    x.Name,
                    x.ImagePath
                }
            );
        }
    }
}
