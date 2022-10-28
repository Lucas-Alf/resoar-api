using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Domain.Standard;

namespace Infrastructure.Interfaces.Repositories.Domain
{
    public interface IResearchRepository : IDomainRepository<Research>
    {
        PaginationModel<ResearchFullTextModel> GetPagedAdvanced(ResearchFullTextQueryModel model);
    }
}