using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class KnowledgeAreaService : ServiceBase<KnowledgeArea>, IKnowledgeAreaService
    {
        private readonly IKnowledgeAreaRepository _repository;

        public KnowledgeAreaService(IKnowledgeAreaRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize, string? description)
        {
            var filter = new FilterBy<KnowledgeArea>();

            if (!String.IsNullOrEmpty(description))
                filter.Add(x => EF.Functions.ILike(x.Description!, $"%{description.Trim()}%"));

            return GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Description!,
                filter: filter,
                selector: x => new
                {
                    x.Id,
                    x.Description
                }
            );
        }

        public ResponseMessageModel Add(KnowledgeAreaNewModel model, long userId)
        {
            try
            {
                var domain = new KnowledgeArea
                {
                    Description = model.Description,
                    CreatedById = userId
                };

                return base.Add(domain);
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public ResponseMessageModel Update(KnowledgeAreaUpdateModel model, long userId)
        {
            try
            {
                _repository.UpdateSomeFields(
                    new KnowledgeArea
                    {
                        Id = model.Id!.Value,
                        Description = model.Description,
                        ModifiedById = userId
                    },
                    x => x.Description!,
                    x => x.ModifiedById!
                );

                return new ResponseMessageModel("Registro atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }
    }
}
