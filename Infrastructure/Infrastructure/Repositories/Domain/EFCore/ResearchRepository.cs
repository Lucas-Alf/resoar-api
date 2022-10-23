using Domain.Entities;
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

        public PaginationModel<ResearchFullTextModel> GetPagedFullText(string? title, int page, int pageSize)
        {
            if (pageSize > 100)
                throw new Exception("O tamanho máximo de uma página é 100 registros");

            if (pageSize <= 0)
                pageSize = 10;

            var offset = page * pageSize;

            var totalRecords = _dbContext.GenericIntModel!
                .FromSqlInterpolated($"SELECT COUNT(id) AS \"Value\" FROM query_research({title}, null, null)")
                .FirstOrDefault();

            var records = _dbContext.ResearchFullTextModel!
                .FromSqlInterpolated($"SELECT * FROM query_research({title}, {pageSize}, {offset})")
                .ToList();

            foreach (var record in records)
            {
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