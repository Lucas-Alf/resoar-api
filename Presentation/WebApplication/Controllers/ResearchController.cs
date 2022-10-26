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
    public class ResearchController : ControllerBase
    {
        private readonly IResearchService _researchService;

        public ResearchController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpGet]
        public PaginationModel<object> GetPaged(string? title, int page, int pageSize, int? userId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _researchService.GetPagedSimple(page, pageSize, currentUserId, title, userId);
        }

        [HttpGet("advanced")]
        public PaginationModel<ResearchFullTextModel> GetPagedAdvanced([FromQuery] ResearchFullTextQueryModel model)
        {
            return _researchService.GetPagedAdvanced(
                query: model.Query,
                institutions: model.Institutions,
                authors: model.Authors,
                advisors: model.Advisors,
                keywords: model.Keywords,
                knowledgeAreas: model.KnowledgeAreas,
                page: model.Page,
                pageSize: model.PageSize
            );
        }

        [HttpPost]
        public async Task<ResponseMessageModel> Add([FromForm] ResearchCreateModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _researchService.Add(model, userId);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseMessageModel> Delete(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _researchService.Delete(id, userId);
        }
    }
}