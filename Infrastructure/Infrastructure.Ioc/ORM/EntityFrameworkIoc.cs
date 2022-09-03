using Domain.Utils;
using Infrastructure.DBConfiguration.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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