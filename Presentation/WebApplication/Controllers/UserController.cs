using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
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
        public IEnumerable<User> GetAll()
        {
            return _userService.GetAll();
        }

        [HttpGet("id")]
        public User GetById(int id)
        {
            return _userService.GetById(id);
        }

        [HttpPost]
        public User Create(User user)
        {
            return _userService.Add(user);
        }

        [HttpPut]
        public bool Update(User user)
        {
            return _userService.Update(user);
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return _userService.Remove(id);
        }
    }
}