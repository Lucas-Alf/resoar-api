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
    public class KnowledgeAreaController : ControllerBase
    {
        private readonly IKnowledgeAreaService _knowledgeAreaService;

        public KnowledgeAreaController(IKnowledgeAreaService knowledgeAreaService)
        {
            _knowledgeAreaService = knowledgeAreaService;
        }

        [HttpGet]
        public PaginationModel<object> GetPaged(int page, int pageSize, string? description)
        {
            return _knowledgeAreaService.GetPaged(page, pageSize, description);
        }

        [HttpGet("id")]
        public ResponseMessageModel GetById(int id)
        {
            return _knowledgeAreaService.GetById(id);
        }

        [HttpPost]
        public ResponseMessageModel Add(KnowledgeAreaNewModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _knowledgeAreaService.Add(model, userId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Update(KnowledgeAreaUpdateModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _knowledgeAreaService.Update(model, userId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Delete(int id)
        {
            return _knowledgeAreaService.Delete(id);
        }
    }
}