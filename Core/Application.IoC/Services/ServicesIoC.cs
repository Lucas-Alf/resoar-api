using Application.Interfaces.Services.Domain;
using Application.Interfaces.Services.Standard;
using Application.Services.Domain;
using Application.Services.Standard;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC
{
    public static class ServicesIoC
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IKeyWordService, KeyWordService>();
            services.AddScoped<IKnowledgeAreaService, KnowledgeAreaService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IResearchAdvisorApprovalService, ResearchAdvisorApprovalService>();
            services.AddScoped<IResearchAdvisorService, ResearchAdvisorService>();
            services.AddScoped<IResearchAuthorService, ResearchAuthorService>();
            services.AddScoped<IResearchKeyWordService, ResearchKeyWordService>();
            services.AddScoped<IResearchKnowledgeAreaService, ResearchKnowledgeAreaService>();
            services.AddScoped<IResearchService, ResearchService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}