using System.Security.Claims;
using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class KeyWordController : ControllerBase
    {
        private readonly IKeyWordService _keyWordService;

        public KeyWordController(IKeyWordService keyWordService)
        {
            _keyWordService = keyWordService;
        }

        [HttpGet]
        public PaginationModel<object> GetPaged(int page, int pageSize, string? description)
        {
            return _keyWordService.GetPaged(page, pageSize, description);
        }

        [HttpGet("id")]
        public ResponseMessageModel GetById(int id)
        {
            return _keyWordService.GetById(id);
        }

        [HttpPost]
        public ResponseMessageModel Add(KeyWordNewModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _keyWordService.Add(model, userId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Update(KeyWordUpdateModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _keyWordService.Update(model, userId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Delete(int id)
        {
            return _keyWordService.Delete(id);
        }
    }
}