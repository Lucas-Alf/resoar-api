using Domain.Utils;
using Infrastructure.DBConfiguration.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTest.Integration.Repositories.DBConfiguration.EFCore
{
    public class EntityFrameworkConnection
    {
        private IServiceProvider _provider;

        public ApplicationContext DataBaseConfiguration()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(EnvironmentManager.GetConnectionStringTests()));
            _provider = services.BuildServiceProvider();
            return _provider.GetService<ApplicationContext>();
        }
    }
}