using Application.Interfaces.Services.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class ResearchController : ControllerBase
    {
        private readonly IResearchService _researchService;

        public ResearchController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpGet]
        public PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, int? userId)
        {
            return _researchService.GetPaged(page, pageSize, userId);
        }

        [HttpPost]
        public async Task<ResponseMessageModel> Add([FromForm] ResearchCreateModel model)
        {
            return await _researchService.Add(model);
        }
    }
}