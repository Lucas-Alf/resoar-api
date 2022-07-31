using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IUserService : IServiceBase<User>
    {
        PaginationModel<UserViewModel> GetPaged(int page, int pageSize);
    }
}
