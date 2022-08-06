using Application.Services.Standard;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories.Domain;
using Application.Interfaces.Services.Domain;

namespace Application.Services.Domain
{
    public class ResearchAdvisorApprovalService : ServiceBase<ResearchAdvisorApproval>, IResearchAdvisorApprovalService
    {
        private readonly IResearchAdvisorApprovalRepository _repository;

        public ResearchAdvisorApprovalService(IResearchAdvisorApprovalRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
