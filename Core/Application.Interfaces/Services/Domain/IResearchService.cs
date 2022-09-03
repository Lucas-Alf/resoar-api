using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService
    {
        ResponseMessageModel Add(ResearchCreateModel model);
        PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, int? userId = null);
    }
}
