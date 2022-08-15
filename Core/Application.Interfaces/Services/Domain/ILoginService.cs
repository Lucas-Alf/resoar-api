using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface ILoginService
    {
        ResponseMessageModel GetToken(LoginModel model);
        ResponseMessageModel RecoverPassword(RecoverPasswordModel model);
        ResponseMessageModel Register(UserCreateModel model);
        ResponseMessageModel ResetPassword(int userId, ResetPasswordModel model);
    }
}
