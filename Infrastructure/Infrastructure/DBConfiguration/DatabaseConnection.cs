using Microsoft.Extensions.Configuration;

namespace Infrastructure.DBConfiguration
{
    public class DatabaseConnection
    {
        public static IConfiguration ConnectionConfiguration
        {
            get
            {
                var rootPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString();
                var configPath = $"{rootPath}\\Infrastructure\\Infrastructure";

                IConfigurationRoot Configuration = new ConfigurationBuilder()
                    .SetBasePath(configPath)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                return Configuration;
            }
        }
    }
}