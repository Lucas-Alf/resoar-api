using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IResearchService
    {
        PaginationModel<ResearchViewModel> GetPagedSimple(int page, int pageSize, string? title, int? userId = null);
        PaginationModel<ResearchFullTextViewModel> GetPagedAdvanced(ResearchFullTextQueryModel model);
        ResponseMessageModel GetById(int id);
        Task<ResponseMessageModel> Add(ResearchCreateModel model);
        Task<ResponseMessageModel> Delete(int id);
        Task<FileResultModel> GetFile(int researchId);
    }
}
