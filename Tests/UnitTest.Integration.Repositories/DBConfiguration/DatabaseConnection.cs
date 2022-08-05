using Domain.Utils;
using Microsoft.Extensions.Options;

namespace UnitTest.Integration.Repositories.DBConfiguration
{
    public class DatabaseConnection
    {
        public static IOptions<DataOptionFactory> ConnectionConfiguration
        {
            get
            {
                return Options.Create(new DataOptionFactory { DefaultConnection = EnvironmentManager.GetConnectionStringTests() });
            }
        }
    }
}