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
    public class InstitutionController : ControllerBase
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }

        [HttpGet]
        public PaginationModel<object> GetPaged(int page, int pageSize, string? name)
        {
            return _institutionService.GetPaged(page, pageSize, name);
        }

        [HttpGet("id")]
        public ResponseMessageModel GetById(int id)
        {
            return _institutionService.GetById(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Add(Institution model)
        {
            return _institutionService.Add(model);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Update(Institution model)
        {
            return _institutionService.Update(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseMessageModel Delete(int id)
        {
            return _institutionService.Delete(id);
        }
    }
}