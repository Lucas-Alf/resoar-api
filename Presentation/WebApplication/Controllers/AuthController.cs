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
        private readonly IUserService _userService;

        public AuthController(ILoginService loginService, IUserService userService)
        {
            _loginService = loginService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ResultMessageModel GetToken(LoginModel model)
        {
            return _loginService.GetToken(model);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ResultMessageModel Register(UserCreateModel model)
        {
            return _userService.Add(model);
        }
    }
}