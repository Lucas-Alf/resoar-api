using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.Extensions.Logging;

namespace Application.Services.Domain
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _repository;
        private readonly IPdfDocumentService _pdfDocumentService;
        private readonly IStorageService _storageService;
        private readonly ILogger _logger;

        public string FILE_FOLDER { get; } = "research";
        public string THUMBNAIL_FOLDER { get; } = "thumbnail";

        public ResearchService(
            IResearchRepository repository,
            IPdfDocumentService pdfDocumentService,
            IStorageService storageService,
            ILogger<Research> logger
        )
        {
            _repository = repository;
            _pdfDocumentService = pdfDocumentService;
            _storageService = storageService;
            _logger = logger;
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

        public async Task<ResponseMessageModel> Add(ResearchCreateModel model)
        {
            var fileKey = "";
            var thumbnailKey = "";

            try
            {
                // Populates the domain
                var domain = PopulateDomain(model);

                // Copy the file bytes to a MemoryStream
                using (var fileStream = new MemoryStream())
                {
                    model.File!.CopyTo(fileStream);
                    var fileBytes = fileStream.ToArray();

                    // Extracts the PDF text
                    domain.RawContent = _pdfDocumentService.ExtractText(fileBytes);

                    // Save the PDF file
                    fileKey = await SaveFile(
                        stream: fileStream,
                        folder: FILE_FOLDER,
                        contentType: model.File!.ContentType
                    );

                    domain.FilePath = $"{FILE_FOLDER}/{fileKey}";

                    // Generate the PDF Thumbnail
                    using (var thumbnailStream = new MemoryStream(_pdfDocumentService.GenerateThumbnail(fileBytes)))
                    {
                        // Save the PDF Thumbnail
                        thumbnailKey = await SaveFile(
                            stream: thumbnailStream,
                            folder: THUMBNAIL_FOLDER,
                            contentType: "image/webp"
                        );

                        domain.ThumbnailPath = $"{THUMBNAIL_FOLDER}/{thumbnailKey}";
                    }
                }

                // Validate the domain
                var validation = domain.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                // Persists on database
                _repository.Add(domain);

                return new ResponseMessageModel("Registro salvo com sucesso");
            }
            catch (Exception ex)
            {
                if (!String.IsNullOrEmpty(fileKey))
                    TryRemoveFile(fileKey, FILE_FOLDER);

                if (!String.IsNullOrEmpty(thumbnailKey))
                    TryRemoveFile(thumbnailKey, THUMBNAIL_FOLDER);

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

            return domain;
        }

        private async Task<string> SaveFile(MemoryStream stream, string folder, string contentType)
        {
            try
            {
                var fileKey = Guid.NewGuid().ToString();

                await _storageService.Upload(
                    file: stream,
                    folder: folder,
                    fileKey: fileKey,
                    contentType: contentType
                );

                return fileKey;
            }
            catch (Exception ex)
            {
                var message = "Ocorreu um erro ao salvar o arquivo";
                _logger.LogError(ex, message);
                throw new BusinessException(message);
            }
        }

        private async void TryRemoveFile(string key, string folder)
        {
            try
            {
                await _storageService.Delete(key, folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Falha na tentava de excluir o arquivo {folder}/{key}");
            }
        }
    }
}
