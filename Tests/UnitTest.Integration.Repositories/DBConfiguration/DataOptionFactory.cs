using System.Data;

namespace UnitTest.Integration.Repositories.DBConfiguration
{
    public class DataOptionFactory
    {
        public string DefaultConnection { get; set; }
        public IDbConnection DatabaseConnection => new SqlConnection(DefaultConnection);
    }
}