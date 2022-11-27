using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public PaginationModel<UserViewModel> GetPaged(int page, int pageSize, string? name)
        {
            return _userService.GetPaged(page, pageSize, name);
        }

        [HttpGet("{id}")]
        public ResponseMessageModel GetById(int id)
        {
            return _userService.GetById(id);
        }

        [HttpPut]
        public ResponseMessageModel Update(UserUpdateModel model)
        {
            return _userService.Update(model);
        }

        [HttpDelete]
        public ResponseMessageModel Delete()
        {
            return _userService.Remove();
        }
    }
}