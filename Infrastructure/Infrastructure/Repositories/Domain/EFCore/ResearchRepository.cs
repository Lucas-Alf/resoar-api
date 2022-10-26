using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchRepository : DomainRepository<Research>, IResearchRepository
    {
        public ApplicationContext _dbContext { get; set; }
        public ResearchRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public PaginationModel<ResearchFullTextModel> GetPagedAdvanced(
            string? query,
            IList<int>? institutions,
            IList<int>? authors,
            IList<int>? advisors,
            IList<int>? keywords,
            IList<int>? knowledgeAreas,
            int page,
            int pageSize
        )
        {
            if (pageSize > 100)
                throw new Exception("O tamanho máximo de uma página é 100 registros");

            if (pageSize <= 0)
                pageSize = 10;

            var offset = page * pageSize;

            var totalRecords = _dbContext.GenericIntModel!
                .FromSqlInterpolated($"SELECT COUNT(id) AS \"Value\" FROM query_research({query}, {institutions}, {authors}, {advisors}, {keywords}, {knowledgeAreas}, NULL, NULL)")
                .FirstOrDefault();

            var records = _dbContext.ResearchFullTextModel!
                .FromSqlInterpolated($"SELECT * FROM query_research({query}, {institutions}, {authors}, {advisors}, {keywords}, {knowledgeAreas}, {pageSize}, {offset})")
                .ToList();

            foreach (var record in records)
            {
                var validLanguage = Enum.TryParse(typeof(ResearchLanguage), record.LanguageName, true, out var language);
                if (validLanguage)
                    record.Language = (ResearchLanguage)language!;

                record.Authors = _dbContext.ResearchAuthor!
                    .Where(x => x.ResearchId == record.Id)
                    .Select(x => new AuthorViewModel
                    {
                        Id = x.UserId,
                        Name = x.User!.Name,
                        ImagePath = x.User!.ImagePath
                    })
                    .ToList();

                record.Advisors = _dbContext.ResearchAdvisor!
                    .Where(x => x.ResearchId == record.Id)
                    .Select(x => new AdvisorViewModel
                    {
                        Id = x.UserId,
                        Name = x.User!.Name,
                        ImagePath = x.User!.ImagePath
                    })
                    .ToList();

                record.Keywords = _dbContext.ResearchKeyWord!
                    .Where(x => x.ResearchId == record.Id)
                    .Select(x => new KeyWordViewModel
                    {
                        Id = x.KeyWordId,
                        Description = x.KeyWord!.Description,
                    })
                    .ToList();

                record.KnowledgeAreas = _dbContext.ResearchKnowledgeArea!
                    .Where(x => x.ResearchId == record.Id)
                    .Select(x => new KnowledgeAreaViewModel
                    {
                        Id = x.KnowledgeAreaId,
                        Description = x.KnowledgeArea!.Description,
                    })
                    .ToList();
            }

            return new PaginationModel<ResearchFullTextModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords?.Value ?? 0,
                Records = records
            };
        }
    }
}