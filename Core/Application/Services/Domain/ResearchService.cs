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
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        private string FILE_FOLDER { get; } = "research";
        private string THUMBNAIL_FOLDER { get; } = "thumbnail";

        public ResearchService(
            IResearchRepository repository,
            IPdfDocumentService pdfDocumentService,
            IStorageService storageService,
            IUserService userService,
            IInstitutionService institutionService,
            ICurrentUserService currentUserService,
            ILogger<Research> logger
        )
        {
            _repository = repository;
            _pdfDocumentService = pdfDocumentService;
            _storageService = storageService;
            _userService = userService;
            _institutionService = institutionService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public PaginationModel<ResearchFullTextViewModel> GetPagedAdvanced(ResearchFullTextQueryModel model)
        {
            return _repository.GetPagedAdvanced(model);
        }

        public PaginationModel<ResearchViewModel> GetPagedSimple(int page, int pageSize, string? title, int? userId = null)
        {
            var filter = new FilterBy<Research>();

            // Filter by title
            if (!String.IsNullOrEmpty(title))
                filter.Add(x => EF.Functions.ILike(x.Title!, $"%{title.Trim()}%"));

            // Filter public/private research
            if (userId.HasValue)
            {
                filter.Add(x => x.Authors!.Any(y => y.UserId == userId));
                var currentUserId = _currentUserService.GetId();
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

            var data = _repository.GetPagedAnonymous<ResearchViewModel>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                orderBy: x => x.Id,
                orderDirection: OrderDirection.Descending,
                selector: x => new ResearchViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    Type = x.Type,
                    Visibility = x.Visibility,
                    LanguageName = x.Language,
                    FileKey = x.FileKey,
                    ThumbnailKey = x.ThumbnailKey,
                    CreatedAt = x.CreatedAt,
                    Abstract = x.Abstract,
                    CreatedBy = new ResearchCreatedByViewModel
                    {
                        Id = x.CreatedById,
                        Name = x.CreatedBy!.Name,
                        ImagePath = x.CreatedBy!.ImagePath,
                    },
                    Institution = new ResearchCreatedByViewModel
                    {
                        Id = x.InstitutionId,
                        Name = x.Institution!.Name,
                        ImagePath = x.Institution!.ImagePath
                    },
                    Authors = x.Authors!
                        .Select(y => new AuthorViewModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath
                        })
                        .ToList(),
                    Advisors = x.Advisors!
                        .Select(y => new AdvisorViewModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath,
                            Approved = x.ResearchAdvisorApprovals!.Any(z => z.UserId == y.UserId && z.Approved)
                        })
                        .ToList(),
                    Keywords = x.ResearchKeyWords!
                        .Select(x => new KeyWordViewModel
                        {
                            Id = x.KeyWordId,
                            Description = x.KeyWord!.Description
                        })
                        .ToList(),
                    KnowledgeAreas = x.ResearchKnowledgeAreas!
                        .Select(x => new KnowledgeAreaViewModel
                        {
                            Id = x.KnowledgeAreaId,
                            Description = x.KnowledgeArea!.Description
                        })
                        .ToList(),
                }
            );

            // Populates the Language field, in database is a string with the name of language
            // but sometimes we need the Enum
            foreach (var record in data.Records)
            {
                var validLanguage = Enum.TryParse(typeof(ResearchLanguage), record.LanguageName, true, out var language);
                if (validLanguage)
                    record.Language = (ResearchLanguage)language!;
            }

            return data;
        }

        public ResponseMessageModel GetById(int id)
        {
            try
            {
                var domain = _repository
                    .Query(new FilterBy<Research>(x => x.Id == id))
                    .Select(x => new ResearchViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Year = x.Year,
                        Type = x.Type,
                        Visibility = x.Visibility,
                        LanguageName = x.Language,
                        FileKey = x.FileKey,
                        ThumbnailKey = x.ThumbnailKey,
                        CreatedAt = x.CreatedAt,
                        Abstract = x.Abstract,
                        InstitutionId = x.InstitutionId,
                        CreatedBy = new ResearchCreatedByViewModel
                        {
                            Id = x.CreatedById,
                            Name = x.CreatedBy!.Name,
                            ImagePath = x.CreatedBy!.ImagePath
                        },
                        Institution = new ResearchCreatedByViewModel
                        {
                            Id = x.InstitutionId,
                            Name = x.Institution!.Name,
                            ImagePath = x.Institution!.ImagePath
                        },
                        Authors = x.Authors!
                            .Select(y => new AuthorViewModel
                            {
                                Id = y.UserId,
                                Name = y.User!.Name,
                                ImagePath = y.User!.ImagePath
                            })
                            .ToList(),
                        Advisors = x.Advisors!
                            .Select(y => new AdvisorViewModel
                            {
                                Id = y.UserId,
                                Name = y.User!.Name,
                                ImagePath = y.User!.ImagePath,
                                Approved = x.ResearchAdvisorApprovals!.Any(z => z.UserId == y.UserId && z.Approved)
                            })
                            .ToList(),
                        Keywords = x.ResearchKeyWords!
                            .Select(x => new KeyWordViewModel
                            {
                                Id = x.KeyWordId,
                                Description = x.KeyWord!.Description
                            })
                            .ToList(),
                        KnowledgeAreas = x.ResearchKnowledgeAreas!
                            .Select(x => new KnowledgeAreaViewModel
                            {
                                Id = x.KnowledgeAreaId,
                                Description = x.KnowledgeArea!.Description
                            })
                            .ToList(),
                    })
                    .FirstOrDefault();

                if (domain == null)
                    throw new NotFoundException();

                var currentUserId = _currentUserService.GetId();
                if (!domain.Authors.Any(x => x.Id == currentUserId) && !domain.Advisors.Any(x => x.Id == currentUserId) && domain.Visibility != ResearchVisibility.Public)
                    throw new BusinessException("Usuário não possui acesso a esta publicação");

                // Populates the Language field with the Enum value
                var validLanguage = Enum.TryParse(typeof(ResearchLanguage), domain.LanguageName, true, out var language);
                if (validLanguage)
                    domain.Language = (ResearchLanguage)language!;

                return new ResponseMessageModel(domain);
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public async Task<ResponseMessageModel> Delete(int id)
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

                if (domain.CreatedById != _currentUserService.GetId())
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

        public async Task<ResponseMessageModel> Add(ResearchCreateModel model)
        {
            Guid? fileKey = null;
            Guid? thumbnailKey = null;

            try
            {
                // Populates the domain
                var currentUserId = _currentUserService.GetId();
                var domain = PopulateDomain(model, currentUserId);

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

                    if (String.IsNullOrEmpty(domain.RawContent))
                        throw new BusinessException("Não foi possível extrair nenhum texto legível do arquivo PDF");

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

            if (model.AdvisorIds == null || !model.AdvisorIds.Any())
                throw new BusinessException("Ao menos 1 orientador precisa ser informado");

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
