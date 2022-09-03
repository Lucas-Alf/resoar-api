using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;

namespace Application.Services.Domain
{
    public class ResearchService : ServiceBase<Research>, IResearchService
    {
        private readonly IResearchRepository _repository;

        public ResearchService(IResearchRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public PaginationModel<ResearchModel> GetPaged(int page, int pageSize, int? userId = null)
        {
            var filter = new FilterBy<Research>();

            if (userId.HasValue)
                filter.Add(x => x.Authors!.Any(y => y.UserId == userId));

            var data = _repository.GetPagedAnonymous<ResearchModel>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                selector: x => new ResearchModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Visibility = x.Visibility,
                    Language = x.Language,
                    FilePath = x.FilePath,
                    ThumbnailPath = x.ThumbnailPath,
                    Institution = new InstitutionModel
                    {
                        Id = x.InstitutionId,
                        Name = x.Institution!.Name
                    },
                    Authors = x.Authors!
                        .Select(y => new AuthorModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name
                        })
                        .ToList()
                }
            );

            return data;
        }
    }
}
