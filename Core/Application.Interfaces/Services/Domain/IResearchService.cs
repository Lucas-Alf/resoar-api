using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService
    {
        Task<ResponseMessageModel> Add(ResearchCreateModel model, int userId);
        Task<ResponseMessageModel> Delete(int id, int userId);
        PaginationModel<object> GetPagedSimple(int page, int pageSize, int currentUserId, string? title, int? userId = null);
        PaginationModel<ResearchFullTextModel> GetPagedAdvanced(ResearchFullTextQueryModel model);
    }
}
