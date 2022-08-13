namespace Domain.Utils
{
    public class EnvironmentManager
    {
        private const string RESOAR_CONNECTION = "RESOAR_CONNECTION";
        private const string RESOAR_CONNECTION_TESTS = "RESOAR_CONNECTION_TESTS";
        private const string RESOAR_JWT_AUDIENCE = "RESOAR_JWT_AUDIENCE";
        private const string RESOAR_JWT_ISSUER = "RESOAR_JWT_ISSUER";
        private const string RESOAR_JWT_SECRET = "RESOAR_JWT_SECRET";
        private const string RESOAR_RECAPTCHA_SECRET = "RESOAR_RECAPTCHA_SECRET";

        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? "";
        }

        public static string GetConnectionString() => GetEnvironmentVariable(RESOAR_CONNECTION);
        public static string GetConnectionStringTests() => GetEnvironmentVariable(RESOAR_CONNECTION_TESTS);
        public static string GetJwtAudience() => GetEnvironmentVariable(RESOAR_JWT_AUDIENCE);
        public static string GetJwtIssuer() => GetEnvironmentVariable(RESOAR_JWT_ISSUER);
        public static string GetJwtSecret() => GetEnvironmentVariable(RESOAR_JWT_SECRET);
        public static string GetReCaptchaSecret() => GetEnvironmentVariable(RESOAR_RECAPTCHA_SECRET);

        public static void Validate()
        {
            if (String.IsNullOrEmpty(GetConnectionString()))
                throw new Exception($"Environment variable {RESOAR_CONNECTION} is not set");

            if (String.IsNullOrEmpty(GetJwtAudience()))
                throw new Exception($"Environment variable {RESOAR_JWT_AUDIENCE} is not set");

            if (String.IsNullOrEmpty(GetJwtIssuer()))
                throw new Exception($"Environment variable {RESOAR_JWT_ISSUER} is not set");

            if (String.IsNullOrEmpty(GetReCaptchaSecret()))
                throw new Exception($"Environment variable {RESOAR_RECAPTCHA_SECRET} is not set");

            var jwtSecret = GetJwtSecret();
            if (String.IsNullOrEmpty(jwtSecret))
                throw new Exception($"Environment variable {RESOAR_JWT_SECRET} is not set");

            if (jwtSecret.Length < 32)
                throw new Exception($"Environment variable {RESOAR_JWT_SECRET} must have at least 32 characters");
        }
    }
}
