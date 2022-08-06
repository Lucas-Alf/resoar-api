using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Interfaces.Repositories.Standard;
using Infrastructure.Repositories.Domain.EFCore;
using Infrastructure.Repositories.Standard.EFCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC
{
    public static class RepositoriesIoC
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IKeyWordRepository, KeyWordRepository>();
            services.AddScoped<IKnowledgeAreaRepository, KnowledgeAreaRepository>();
            services.AddScoped<IResearchAdvisorApprovalRepository, ResearchAdvisorApprovalRepository>();
            services.AddScoped<IResearchAdvisorRepository, ResearchAdvisorRepository>();
            services.AddScoped<IResearchAuthorRepository, ResearchAuthorRepository>();
            services.AddScoped<IResearchKeyWordRepository, ResearchKeyWordRepository>();
            services.AddScoped<IResearchKnowledgeAreaRepository, ResearchKnowledgeAreaRepository>();
            services.AddScoped<IResearchRepository, ResearchRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}