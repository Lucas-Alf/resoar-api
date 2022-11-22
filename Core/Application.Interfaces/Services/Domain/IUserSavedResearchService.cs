using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IUserSavedResearchService : IServiceBase<UserSavedResearch>
    {
        PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, string? title);
        ResponseMessageModel VerifyExists(int researchId);
        ResponseMessageModel Add(int researchId);
        new ResponseMessageModel Delete(int researchId);
    }
}
