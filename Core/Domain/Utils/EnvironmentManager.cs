namespace Domain.Utils
{
    public class EnvironmentManager
    {
        private const string RESOAR_CONNECTION = "Resoar:Connection";
        private const string RESOAR_CONNECTION_TESTS = "Resoar:ConnectionTests";
        private const string RESOAR_JWT_AUDIENCE = "Resoar:JwtAudience";
        private const string RESOAR_JWT_ISSUER = "Resoar:JwtIssuer";
        private const string RESOAR_JWT_SECRET = "Resoar:JwtSecret";

        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? "";
        }

        public static string GetConnectionString() => GetEnvironmentVariable(RESOAR_CONNECTION);
        public static string GetConnectionStringTests() => GetEnvironmentVariable(RESOAR_CONNECTION_TESTS);
        public static string GetJwtAudience() => GetEnvironmentVariable(RESOAR_JWT_AUDIENCE);
        public static string GetJwtIssuer() => GetEnvironmentVariable(RESOAR_JWT_ISSUER);
        public static string GetJwtSecret() => GetEnvironmentVariable(RESOAR_JWT_SECRET);

        public static void Validate()
        {
            if (String.IsNullOrEmpty(GetConnectionString()))
                throw new Exception($"Environment variable {RESOAR_CONNECTION} is not set");

            if (String.IsNullOrEmpty(GetJwtAudience()))
                throw new Exception($"Environment variable {RESOAR_JWT_AUDIENCE} is not set");

            if (String.IsNullOrEmpty(GetJwtIssuer()))
                throw new Exception($"Environment variable {RESOAR_JWT_ISSUER} is not set");

            if (String.IsNullOrEmpty(GetJwtSecret()))
                throw new Exception($"Environment variable {RESOAR_JWT_SECRET} is not set");
        }
    }
}

