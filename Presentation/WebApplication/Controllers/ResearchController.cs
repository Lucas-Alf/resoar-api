using Application.Interfaces.Services.Domain;
using Domain.Entities;
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
        public PaginationModel<ResearchModel> GetPaged(int page, int pageSize, int? userId)
        {
            return _researchService.GetPaged(page, pageSize, userId);
        }

        [HttpPost]
        public ResponseMessageModel Add(Research model)
        {
            return _researchService.Add(model);
        }

        [HttpPut]
        public ResponseMessageModel Update(Research model)
        {
            return _researchService.Update(model);
        }

        [HttpDelete]
        public ResponseMessageModel Delete(int id)
        {
            return _researchService.Delete(id);
        }
    }
}