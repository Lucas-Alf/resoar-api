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
        public ResponseMessageModel Add(KnowledgeArea model)
        {
            return _knowledgeAreaService.Add(model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Update(KnowledgeArea model)
        {
            return _knowledgeAreaService.Update(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Delete(int id)
        {
            return _knowledgeAreaService.Delete(id);
        }
    }
}