using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Interfaces.Repositories.Standard;
using Infrastructure.Repositories.Domain.EFCore;
using Infrastructure.Repositories.Standard.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC
{
    public class EntityFrameworkIoC : OrmTypes
    {
        internal override IServiceCollection AddOrm(IServiceCollection services, IConfiguration? configuration = null)
        {
            IConfiguration dbConnectionSettings = ResolveConfiguration.GetConnectionSettings(configuration);
            string conn = dbConnectionSettings.GetConnectionString("Resoar");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(conn));

            services.AddScoped(typeof(IRepositoryBaseAsync<>), typeof(RepositoryBaseAsync<>));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}