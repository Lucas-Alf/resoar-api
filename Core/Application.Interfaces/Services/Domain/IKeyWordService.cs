using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IKeyWordService : IServiceBase<KeyWord>
    {
        PaginationModel<object> GetPaged(int page, int pageSize, string? description);
    }
}
