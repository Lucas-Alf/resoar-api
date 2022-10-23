using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService
    {
        Task<ResponseMessageModel> Add(ResearchCreateModel model, int userId);
        Task<ResponseMessageModel> Delete(int id, int userId);
        PaginationModel<object> GetPagedSimple(int page, int pageSize, int currentUserId, string? title, int? userId = null);
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
