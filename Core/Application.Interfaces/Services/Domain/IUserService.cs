using Domain.Entities;
using Domain.Models;
using Domain.Utils;

namespace Application.Interfaces.Services.Domain
{
    public interface IUserService
    {
        IQueryable<User> Query(FilterBy<User> filter);
        PaginationModel<UserViewModel> GetPaged(int page, int pageSize, string? name);
        ResponseMessageModel GetById(int id);
        ResponseMessageModel Remove(int userId);
        ResponseMessageModel Update(int userId, UserUpdateModel model);
    }
}
