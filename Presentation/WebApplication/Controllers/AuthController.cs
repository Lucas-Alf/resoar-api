using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ResponseMessageModel GetToken(LoginModel model)
        {
            return _loginService.GetToken(model);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ResponseMessageModel Register(UserCreateModel model)
        {
            return _loginService.Register(model);
        }

        [AllowAnonymous]
        [HttpPost("recover")]
        public ResponseMessageModel RecoverPassword(RecoverPasswordModel model)
        {
            return _loginService.RecoverPassword(model);
        }
    }
}