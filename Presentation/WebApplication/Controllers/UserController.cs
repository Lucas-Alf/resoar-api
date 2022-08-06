using System.Security.Claims;
using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public PaginationModel<UserViewModel> GetPaged(int page, int pageSize)
        {
            return _userService.GetPaged(page, pageSize);
        }

        [HttpGet("id")]
        public ResultMessageModel GetById(int id)
        {
            return _userService.GetById(id);
        }

        [HttpPut]
        public ResultMessageModel Update(UserUpdateModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _userService.Update(userId, model);
        }

        [HttpDelete]
        public ResultMessageModel Delete()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _userService.Remove(userId);
        }
    }
}