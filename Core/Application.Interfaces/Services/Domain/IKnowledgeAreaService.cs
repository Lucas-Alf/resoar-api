using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Services.Domain
{
    public interface IKnowledgeAreaService : IServiceBase<KnowledgeArea>
    {
        PaginationModel<object> GetPaged(int page, int pageSize, string? description);
        ResponseMessageModel Add(KnowledgeAreaNewModel model);
        ResponseMessageModel Update(KnowledgeAreaUpdateModel model);
    }
}
