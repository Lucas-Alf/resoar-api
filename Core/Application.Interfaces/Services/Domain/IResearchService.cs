using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService
    {
        Task<ResponseMessageModel> Add(ResearchCreateModel model, int userId);
        Task<ResponseMessageModel> Delete(int id, int userId);
        PaginationModel<object> GetPaged(int page, int pageSize, int? userId = null);
    }
}
