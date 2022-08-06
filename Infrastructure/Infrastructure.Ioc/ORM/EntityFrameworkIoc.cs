using Domain.Utils;
using Infrastructure.DBConfiguration.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IoC
{
    public static class EntityFrameworkIoc
    {
        public static void AddEntityFramework(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(EnvironmentManager.GetConnectionString()));
        }
    }
}