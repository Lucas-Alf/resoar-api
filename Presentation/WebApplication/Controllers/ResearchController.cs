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
        public PaginationModel<ResearchViewModel> GetPaged(string? title, int page, int pageSize, int? userId)
        {
            return _researchService.GetPagedSimple(page, pageSize, title, userId);
        }

        [HttpGet("advanced")]
        public PaginationModel<ResearchFullTextViewModel> GetPagedAdvanced([FromQuery] ResearchFullTextQueryModel model)
        {
            return _researchService.GetPagedAdvanced(model);
        }

        [HttpGet("{id}")]
        public ResponseMessageModel GetById(int id)
        {
            return _researchService.GetById(id);
        }

        [HttpPost]
        public async Task<ResponseMessageModel> Add([FromForm] ResearchCreateModel model)
        {
            return await _researchService.Add(model);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseMessageModel> Delete(int id)
        {
            return await _researchService.Delete(id);
        }
    }
}