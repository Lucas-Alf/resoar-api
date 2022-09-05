using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;

namespace Application.Services.Domain
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _repository;
        private readonly IPdfDocumentService _pdfDocumentService;
        public ResearchService(
            IResearchRepository repository,
            IPdfDocumentService pdfDocumentService
        )
        {
            _repository = repository;
            _pdfDocumentService = pdfDocumentService;
        }

        public PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, int? userId = null)
        {
            var filter = new FilterBy<Research>();

            if (userId.HasValue)
                filter.Add(x => x.Authors!.Any(y => y.UserId == userId));

            var data = _repository.GetPagedAnonymous<ResearchViewModel>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                selector: x => new ResearchViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Visibility = x.Visibility,
                    Language = x.Language,
                    FilePath = x.FilePath,
                    ThumbnailPath = x.ThumbnailPath,
                    Institution = new InstitutionViewModel
                    {
                        Id = x.InstitutionId,
                        Name = x.Institution!.Name
                    },
                    Authors = x.Authors!
                        .Select(y => new AuthorViewModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name
                        })
                        .ToList()
                }
            );

            return data;
        }

        public ResponseMessageModel Add(ResearchCreateModel model)
        {
            try
            {
                var domain = PopulateDomain(model);

                var validation = domain.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                _repository.Add(domain);

                return new ResponseMessageModel("Registro salvo com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        private Research PopulateDomain(ResearchCreateModel model)
        {
            if (model.AuthorIds == null || !model.AuthorIds.Any())
                throw new BusinessException("Ao menos 1 autor precisa ser informado");

            var domain = new Research
            {
                Title = model.Title,
                Year = model.Year,
                Type = model.Type,
                Visibility = model.Visibility,
                InstitutionId = model.InstitutionId,
                Language = model.Language?.ToString().ToLower(),
                CreatedAt = DateTime.Now
            };

            domain.Authors = model.AuthorIds
                .Select(authorId => new ResearchAuthor { UserId = authorId })
                .ToList();

            if (model.AdvisorIds != null && model.AdvisorIds.Any())
                domain.Advisors = model.AdvisorIds
                    .Select(advisorId => new ResearchAdvisor { UserId = advisorId })
                    .ToList();

            using (var ms = new MemoryStream())
            {
                model.File!.CopyTo(ms);
                var fileBytes = ms.ToArray();

                domain.RawContent = _pdfDocumentService.ExtractText(fileBytes);
                var thumbnail = _pdfDocumentService.GenerateThumbnail(fileBytes);

                domain.ThumbnailPath = "123";
                domain.FilePath = "123";
            }

            return domain;
        }
    }
}
