using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Domain.Standard;

namespace Infrastructure.Interfaces.Repositories.Domain
{
    public interface IResearchRepository : IDomainRepository<Research>
    {
        PaginationModel<ResearchFullTextModel> GetPagedFullText(
            string? query,
            IList<int>? institutions,
            IList<int>? authors,
            IList<int>? advisors,
            IList<int>? keywords,
            IList<int>? knowledgeAreas,
            int page,
            int pageSize
        );
    }
}