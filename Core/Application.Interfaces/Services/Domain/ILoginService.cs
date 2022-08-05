using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface ILoginService
    {
        ResultMessageModel GetToken(LoginModel model);
    }
}
