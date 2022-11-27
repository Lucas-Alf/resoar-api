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
        public PaginationModel<ResearchViewModel> GetPaged(string? title, int page, int pageSize, int? userId, int? advisorId)
        {
            return _researchService.GetPagedSimple(page, pageSize, title, userId, advisorId);
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

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var file = await _researchService.GetFile(id);
                return File(file.FileContents!, file.ContentType!, file.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex));
            }
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