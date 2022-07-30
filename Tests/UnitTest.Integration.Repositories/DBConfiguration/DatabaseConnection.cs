using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace UnitTest.Integration.Repositories.DBConfiguration
{
    public class DatabaseConnection
    {
        public static IOptions<DataOptionFactory> ConnectionConfiguration
        {
            get
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.test.json")
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("ResoarTests");

                return Options.Create(new DataOptionFactory { DefaultConnection = connectionString });
            }
        }
    }
}