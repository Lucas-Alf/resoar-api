using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "User, Admin")]
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
        public ResponseMessageModel GetById(int id)
        {
            return _userService.GetById(id);
        }

        [HttpPut]
        public ResponseMessageModel Update(UserUpdateModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _userService.Update(userId, model);
        }

        [HttpDelete]
        public ResponseMessageModel Delete()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _userService.Remove(userId);
        }
    }
}