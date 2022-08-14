using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IUserService
    {
        PaginationModel<UserViewModel> GetPaged(int page, int pageSize);
        ResponseMessageModel GetById(int id);
        ResponseMessageModel Remove(int userId);
        ResponseMessageModel Update(int userId, UserUpdateModel model);
    }
}
