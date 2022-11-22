using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class UserSavedResearchController : ControllerBase
    {
        private readonly IUserSavedResearchService _userSavedResearchService;

        public UserSavedResearchController(IUserSavedResearchService userSavedResearchService)
        {
            _userSavedResearchService = userSavedResearchService;
        }

        [HttpGet]
        public PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, string? title)
        {
            return _userSavedResearchService.GetPaged(page, pageSize, title);
        }

        [HttpGet("{researchId}")]
        public ResponseMessageModel VerifyExists(int researchId)
        {
            return _userSavedResearchService.VerifyExists(researchId);
        }

        [HttpPost("{researchId}")]
        public ResponseMessageModel Add(int researchId)
        {
            return _userSavedResearchService.Add(researchId);
        }

        [HttpDelete("{researchId}")]
        public ResponseMessageModel Delete(int researchId)
        {
            return _userSavedResearchService.Delete(researchId);
        }
    }
}