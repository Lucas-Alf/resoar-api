using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IUserService : IServiceBase<User>
    {
        new ResultMessageModel GetById(int id);
        PaginationModel<UserViewModel> GetPaged(int page, int pageSize);
        ResultMessageModel Add(UserCreateModel model);
        ResultMessageModel Update(int userId, UserUpdateModel model);
    }
}
