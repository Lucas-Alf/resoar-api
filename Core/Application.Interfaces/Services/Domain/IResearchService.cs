using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService : IServiceBase<Research>
    {
        PaginationModel<ResearchModel> GetPaged(int page, int pageSize, int? userId = null);
    }
}
