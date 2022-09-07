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

        public PaginationModel<object> GetPaged(int page, int pageSize, int? userId = null)
        {
            var filter = new FilterBy<Research>();

            if (userId.HasValue)
                filter.Add(x => x.Authors!.Any(y => y.UserId == userId));

            var data = _repository.GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                selector: x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Visibility = x.Visibility,
                    Language = x.Language,
                    FileKey = x.FileKey,
                    ThumbnailKey = x.ThumbnailKey,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = new
                    {
                        Id = x.CreatedById,
                        Name = x.CreatedBy!.Name
                    },
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

        public async Task<ResponseMessageModel> Delete(int id, int userId)
        {
            try
            {
                var domain = _repository
                    .Query(new FilterBy<Research>(x => x.Id == id))
                    .Select(x => new
                    {
                        x.FileKey,
                        x.ThumbnailKey,
                        x.CreatedById
                    })
                    .FirstOrDefault();

                if (domain == null)
                    throw new NotFoundException();

                if (domain.CreatedById != userId)
                    throw new BusinessException("Somente o usuário criador da publicação pode realizar a exclusão");

                if (domain.FileKey.HasValue)
                    await _storageService.Delete(domain.FileKey.Value.ToString(), FILE_FOLDER);

                if (domain.ThumbnailKey.HasValue)
                    await _storageService.Delete(domain.ThumbnailKey.Value.ToString(), THUMBNAIL_FOLDER);

                await _repository.DeleteAsync(id);

                return new ResponseMessageModel("Excluído com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public async Task<ResponseMessageModel> Add(ResearchCreateModel model, int userId)
        {
            Guid? fileKey = null;
            Guid? thumbnailKey = null;

            try
            {
                // Populates the domain
                var domain = PopulateDomain(model, userId);

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

                    domain.FileKey = fileKey;

                    // Generate the PDF Thumbnail
                    using (var thumbnailStream = new MemoryStream(_pdfDocumentService.GenerateThumbnail(fileBytes)))
                    {
                        // Save the PDF Thumbnail
                        thumbnailKey = await SaveFile(
                            stream: thumbnailStream,
                            folder: THUMBNAIL_FOLDER,
                            contentType: "image/webp"
                        );

                        domain.ThumbnailKey = thumbnailKey;
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
                if (fileKey.HasValue)
                    await TryRemoveFile(fileKey.Value.ToString(), FILE_FOLDER);

                if (thumbnailKey.HasValue)
                    await TryRemoveFile(thumbnailKey.Value.ToString(), THUMBNAIL_FOLDER);

                return new ResponseMessageModel(ex);
            }
        }

        private Research PopulateDomain(ResearchCreateModel model, int userId)
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
                CreatedAt = DateTime.Now,
                CreatedById = userId
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

        private async Task<Guid> SaveFile(MemoryStream stream, string folder, string contentType)
        {
            try
            {
                var fileKey = Guid.NewGuid();

                await _storageService.Upload(
                    file: stream,
                    folder: folder,
                    fileKey: fileKey.ToString(),
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

        private async Task TryRemoveFile(string key, string folder)
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
