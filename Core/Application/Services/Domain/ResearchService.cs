using Amazon.S3;
using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services.Domain
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _repository;
        private readonly IPdfDocumentService _pdfDocumentService;
        private readonly IStorageService _storageService;
        private readonly IUserService _userService;
        private readonly IInstitutionService _institutionService;
        private readonly ILogger _logger;

        public string FILE_FOLDER { get; } = "research";
        public string THUMBNAIL_FOLDER { get; } = "thumbnail";

        public ResearchService(
            IResearchRepository repository,
            IPdfDocumentService pdfDocumentService,
            IStorageService storageService,
            IUserService userService,
            IInstitutionService institutionService,
            ILogger<Research> logger
        )
        {
            _repository = repository;
            _pdfDocumentService = pdfDocumentService;
            _storageService = storageService;
            _userService = userService;
            _institutionService = institutionService;
            _logger = logger;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize, int currentUserId, string? title, int? userId = null)
        {
            var filter = new FilterBy<Research>();

            if (!String.IsNullOrEmpty(title))
                filter.Add(x => EF.Functions.ILike(x.Title!, $"%{title.Trim()}%"));

            if (userId.HasValue)
            {
                filter.Add(x => x.Authors!.Any(y => y.UserId == userId));
                if (currentUserId == userId)
                    filter.Add(x =>
                        x.Visibility == ResearchVisibility.Public ||
                        x.Visibility == ResearchVisibility.Private
                    );
            }
            else
            {
                filter.Add(x => x.Visibility == ResearchVisibility.Public);
            }

            var data = _repository.GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                selector: x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    Type = x.Type,
                    Visibility = x.Visibility,
                    Language = x.Language,
                    FileKey = x.FileKey,
                    ThumbnailKey = x.ThumbnailKey,
                    CreatedAt = x.CreatedAt,
                    Abstract = x.Abstract,
                    CreatedBy = new
                    {
                        Id = x.CreatedById,
                        Name = x.CreatedBy!.Name
                    },
                    Institution = new
                    {
                        Id = x.InstitutionId,
                        Name = x.Institution!.Name
                    },
                    Authors = x.Authors!
                        .Select(y => new
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath
                        })
                        .ToList(),
                    Advisors = x.Advisors!
                        .Select(y => new
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath
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
                return new ErrorMessageModel(ex);
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

                // Gets the authors names for document metadata
                var authorsNames = _userService
                    .Query(new FilterBy<User>(x => model.AuthorIds!.Contains(x.Id)))
                    .Select(x => x.Name)
                    .ToList();

                // Gets institution name for document metadata
                var institutionName = _institutionService
                    .Query(new FilterBy<Institution>(x => x.Id == domain.InstitutionId))
                    .Select(x => x.Name)
                    .FirstOrDefault();

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
                        contentType: model.File!.ContentType,
                        permission: S3CannedACL.AuthenticatedRead,
                        metadata: new Dictionary<string, string>
                        {
                            ["title"] = domain.Title!,
                            ["year"] = domain.Year.ToString()!,
                            ["type"] = domain.Type.ToString()!,
                            ["visibility"] = domain.Visibility.ToString()!,
                            ["institution"] = institutionName!,
                            ["language"] = domain.Language!,
                            ["authors"] = String.Join(";", authorsNames)
                        }
                    );

                    domain.FileKey = fileKey;

                    // Generates the PDF Thumbnail
                    using (var thumbnailStream = new MemoryStream(_pdfDocumentService.GenerateThumbnail(fileBytes)))
                    {
                        // Save the PDF Thumbnail
                        thumbnailKey = await SaveFile(
                            stream: thumbnailStream,
                            folder: THUMBNAIL_FOLDER,
                            contentType: "image/webp",
                            permission: S3CannedACL.PublicRead
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

                return new ResponseMessageModel("Registro salvo com sucesso", new { Id = domain.Id });
            }
            catch (Exception ex)
            {
                if (fileKey.HasValue)
                    await TryRemoveFile(fileKey.Value.ToString(), FILE_FOLDER);

                if (thumbnailKey.HasValue)
                    await TryRemoveFile(thumbnailKey.Value.ToString(), THUMBNAIL_FOLDER);

                return new ErrorMessageModel(ex);
            }
        }

        private Research PopulateDomain(ResearchCreateModel model, int userId)
        {
            if (model.AuthorIds == null || !model.AuthorIds.Any())
                throw new BusinessException("Ao menos 1 autor precisa ser informado");

            if (model.KeyWordIds == null || !model.KeyWordIds.Any())
                throw new BusinessException("Ao menos 1 palavra chave precisa ser informada");

            if (model.KnowledgeAreaIds == null || !model.KnowledgeAreaIds.Any())
                throw new BusinessException("Ao menos 1 área do conhecimento precisa ser informada");

            if (!model.AuthorIds.Any(x => x == userId))
                throw new BusinessException("Usuário precisa ser informado como autor da própria publicação");

            var domain = new Research
            {
                Title = model.Title,
                Year = model.Year,
                Type = model.Type,
                Visibility = model.Visibility,
                InstitutionId = model.InstitutionId,
                Language = model.Language?.ToString().ToLower(),
                Abstract = model.Abstract,
                CreatedAt = DateTime.Now,
                CreatedById = userId
            };

            domain.Authors = model.AuthorIds
                .Select(authorId => new ResearchAuthor { UserId = authorId })
                .ToList();

            domain.ResearchKeyWords = model.KeyWordIds
                .Select(keywordId => new ResearchKeyWord { KeyWordId = keywordId })
                .ToList();

            domain.ResearchKnowledgeAreas = model.KnowledgeAreaIds
                .Select(knowledgeAreaId => new ResearchKnowledgeArea { KnowledgeAreaId = knowledgeAreaId })
                .ToList();

            if (model.AdvisorIds != null && model.AdvisorIds.Any())
                domain.Advisors = model.AdvisorIds
                    .Select(advisorId => new ResearchAdvisor { UserId = advisorId })
                    .ToList();

            return domain;
        }

        private async Task<Guid> SaveFile(MemoryStream stream, string folder, string contentType, S3CannedACL permission, Dictionary<string, string>? metadata = null)
        {
            try
            {
                var fileKey = Guid.NewGuid();

                await _storageService.Upload(
                    file: stream,
                    folder: folder,
                    fileKey: fileKey.ToString(),
                    contentType: contentType,
                    permission: permission,
                    metadata: metadata
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
