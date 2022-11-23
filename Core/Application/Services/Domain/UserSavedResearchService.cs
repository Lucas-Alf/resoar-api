using System.Linq.Expressions;
using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class UserSavedResearchService : ServiceBase<UserSavedResearch>, IUserSavedResearchService
    {
        private readonly IUserSavedResearchRepository _repository;
        private readonly IResearchRepository _researchRepository;
        private readonly ICurrentUserService _currentUserService;

        public UserSavedResearchService(
            IUserSavedResearchRepository repository,
            IResearchRepository researchRepository,
            ICurrentUserService currentUserService
        ) : base(repository)
        {
            _repository = repository;
            _researchRepository = researchRepository;
            _currentUserService = currentUserService;
        }

        public PaginationModel<ResearchViewModel> GetPaged(int page, int pageSize, string? title)
        {
            var filter = new FilterBy<UserSavedResearch>(x => x.UserId == _currentUserService.GetId());
            if (!String.IsNullOrEmpty(title))
                filter.Add(x => EF.Functions.ILike(x.Research!.Title!, $"%{title.Trim()}%"));

            var data = _repository.GetPagedAnonymous<ResearchViewModel>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                orderBy: x => x.Id,
                orderDirection: OrderDirection.Descending,
                selector: x => new ResearchViewModel
                {
                    Id = x.Research!.Id,
                    Title = x.Research!.Title,
                    Year = x.Research!.Year,
                    Type = x.Research!.Type,
                    Visibility = x.Research!.Visibility,
                    LanguageName = x.Research!.Language,
                    ThumbnailKey = x.Research!.ThumbnailKey,
                    CreatedAt = x.Research!.CreatedAt,
                    Abstract = x.Research!.Abstract,
                    CreatedBy = new ResearchCreatedByViewModel
                    {
                        Id = x.Research!.CreatedById,
                        Name = x.Research!.CreatedBy!.Name,
                        ImagePath = x.Research!.CreatedBy!.ImagePath,
                    },
                    Institution = new ResearchCreatedByViewModel
                    {
                        Id = x.Research!.InstitutionId,
                        Name = x.Research!.Institution!.Name,
                        ImagePath = x.Research!.Institution!.ImagePath
                    },
                    Authors = x.Research!.Authors!
                        .Select(y => new AuthorViewModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath
                        })
                        .ToList(),
                    Advisors = x.Research!.Advisors!
                        .Select(y => new AdvisorViewModel
                        {
                            Id = y.UserId,
                            Name = y.User!.Name,
                            ImagePath = y.User!.ImagePath,
                            Approved = x.Research!.ResearchAdvisorApprovals!.Any(z => z.UserId == y.UserId && z.Approved)
                        })
                        .ToList(),
                    Keywords = x.Research!.ResearchKeyWords!
                        .Select(x => new KeyWordViewModel
                        {
                            Id = x.KeyWordId,
                            Description = x.KeyWord!.Description
                        })
                        .ToList(),
                    KnowledgeAreas = x.Research!.ResearchKnowledgeAreas!
                        .Select(x => new KnowledgeAreaViewModel
                        {
                            Id = x.KnowledgeAreaId,
                            Description = x.KnowledgeArea!.Description
                        })
                        .ToList()
                }
            );

            foreach (var record in data.Records)
            {
                var validLanguage = Enum.TryParse(typeof(ResearchLanguage), record.LanguageName, true, out var language);
                if (validLanguage)
                    record.Language = (ResearchLanguage)language!;
            }

            return data;
        }

        public ResponseMessageModel VerifyExists(int researchId)
        {
            try
            {
                var currentUserId = _currentUserService.GetId();
                var exists = _repository
                    .Query(new FilterBy<UserSavedResearch>(x =>
                        x.ResearchId == researchId &&
                        x.UserId == currentUserId
                    ))
                    .Any();

                return new ResponseMessageModel(new { Exists = exists });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public ResponseMessageModel Add(int researchId)
        {
            try
            {
                var currentUserId = _currentUserService.GetId();
                var exists = _repository
                    .Query(new FilterBy<UserSavedResearch>(x =>
                        x.ResearchId == researchId &&
                        x.UserId == currentUserId
                    ))
                    .Any();

                if (exists)
                    throw new BusinessException("Esta publicação já consta como salva");

                // Verify if research is public, or the user is a author or advisor.
                var hasPermission = _researchRepository
                    .Query(new FilterBy<Research>(x =>
                        x.Id == researchId &&
                        x.Visibility == ResearchVisibility.Public ||
                        (
                            x.Visibility == ResearchVisibility.Private &&
                            (
                                x.Authors!.Any(y => y.Id == currentUserId) ||
                                x.Advisors!.Any(y => y.UserId == currentUserId)
                            )
                        )
                    ))
                    .Any();

                if (!hasPermission)
                    throw new BusinessException("Usuário não possui acesso a esta publicação");

                var domain = new UserSavedResearch
                {
                    ResearchId = researchId,
                    UserId = currentUserId
                };

                var messages = base.Add(domain);
                if (!messages.Success)
                    return messages;

                return new ResponseMessageModel("Publicação salva com sucesso", new { Id = domain.Id });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public new ResponseMessageModel Delete(int researchId)
        {
            try
            {
                var currentUserId = _currentUserService.GetId();
                var savedResearchId = _repository
                    .Query(new FilterBy<UserSavedResearch>(x =>
                        x.ResearchId == researchId &&
                        x.UserId == currentUserId
                    ))
                    .Select(x => (int?)x.Id)
                    .FirstOrDefault();

                if (!savedResearchId.HasValue)
                    throw new NotFoundException();

                var domain = new UserSavedResearch
                {
                    Id = savedResearchId.Value
                };

                var messages = base.Delete(domain);
                if (!messages.Success)
                    return messages;

                return new ResponseMessageModel("Publicação removida com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }
    }
}
