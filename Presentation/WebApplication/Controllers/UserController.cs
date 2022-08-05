using Application.Interfaces.Services.Domain;
using Domain.Entities;
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
        public User GetById(int id)
        {
            return _userService.GetById(id);
        }

        [HttpPost]
        public ResultMessageModel Create(UserCreateModel user)
        {
            return _userService.Add(user);
        }

        [HttpPut]
        public ResultMessageModel Update(UserUpdateModel user)
        {
            return _userService.Update(user);
        }

        [HttpDelete]
        public ResultMessageModel Delete(int id)
        {
            return _userService.Remove(id);
        }
    }
}