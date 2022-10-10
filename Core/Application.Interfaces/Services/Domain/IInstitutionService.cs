using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IInstitutionService : IServiceBase<Institution>
    {
        PaginationModel<object> GetPaged(int page, int pageSize, string? name);
    }
}
