namespace Domain.Utils
{
    public class EnvironmentManager
    {
        private const string RESOAR_CONNECTION = "RESOAR_CONNECTION";
        private const string RESOAR_CONNECTION_TESTS = "RESOAR_CONNECTION_TESTS";
        private const string RESOAR_JWT_AUDIENCE = "RESOAR_JWT_AUDIENCE";
        private const string RESOAR_JWT_ISSUER = "RESOAR_JWT_ISSUER";
        private const string RESOAR_JWT_SECRET = "RESOAR_JWT_SECRET";
        private const string RESOAR_CAPTCHA_SECRET = "RESOAR_CAPTCHA_SECRET";
        private const string RESOAR_CAPTCHA_SITE_KEY = "RESOAR_CAPTCHA_SITE_KEY";
        private const string RESOAR_SMTP_HOST = "RESOAR_SMTP_HOST";
        private const string RESOAR_SMTP_PORT = "RESOAR_SMTP_PORT";
        private const string RESOAR_SMTP_USER = "RESOAR_SMTP_USER";
        private const string RESOAR_SMTP_EMAIL = "RESOAR_SMTP_EMAIL";
        private const string RESOAR_SMTP_PASSWORD = "RESOAR_SMTP_PASSWORD";
        private const string RESOAR_STORAGE_ACCESS_KEY = "RESOAR_STORAGE_ACCESS_KEY";
        private const string RESOAR_STORAGE_SECRET_KEY = "RESOAR_STORAGE_SECRET_KEY";
        private const string RESOAR_STORAGE_ENDPOINT = "RESOAR_STORAGE_ENDPOINT";
        private const string RESOAR_STORAGE_REGION = "RESOAR_STORAGE_REGION";

        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? "";
        }

        public static string GetConnectionString() => GetEnvironmentVariable(RESOAR_CONNECTION);
        public static string GetConnectionStringTests() => GetEnvironmentVariable(RESOAR_CONNECTION_TESTS);
        public static string GetJwtAudience() => GetEnvironmentVariable(RESOAR_JWT_AUDIENCE);
        public static string GetJwtIssuer() => GetEnvironmentVariable(RESOAR_JWT_ISSUER);
        public static string GetJwtSecret() => GetEnvironmentVariable(RESOAR_JWT_SECRET);
        public static string GetCaptchaSecret() => GetEnvironmentVariable(RESOAR_CAPTCHA_SECRET);
        public static string GetCaptchaSiteKey() => GetEnvironmentVariable(RESOAR_CAPTCHA_SITE_KEY);
        public static string GetSMTPHost() => GetEnvironmentVariable(RESOAR_SMTP_HOST);
        public static string GetSMTPPort() => GetEnvironmentVariable(RESOAR_SMTP_PORT);
        public static string GetSMTPUser() => GetEnvironmentVariable(RESOAR_SMTP_USER);
        public static string GetSMTPEmail() => GetEnvironmentVariable(RESOAR_SMTP_EMAIL);
        public static string GetSMTPPassword() => GetEnvironmentVariable(RESOAR_SMTP_PASSWORD);
        public static string GetStorageAccessKey() => GetEnvironmentVariable(RESOAR_STORAGE_ACCESS_KEY);
        public static string GetStorageSecretKey() => GetEnvironmentVariable(RESOAR_STORAGE_SECRET_KEY);
        public static string GetStorageEndpoint() => GetEnvironmentVariable(RESOAR_STORAGE_ENDPOINT);
        public static string GetStorageRegion() => GetEnvironmentVariable(RESOAR_STORAGE_REGION);

        public static void Validate()
        {
            if (String.IsNullOrEmpty(GetConnectionString()))
                throw new Exception($"Environment variable {RESOAR_CONNECTION} is not set");

            if (String.IsNullOrEmpty(GetJwtAudience()))
                throw new Exception($"Environment variable {RESOAR_JWT_AUDIENCE} is not set");

            if (String.IsNullOrEmpty(GetJwtIssuer()))
                throw new Exception($"Environment variable {RESOAR_JWT_ISSUER} is not set");

            if (String.IsNullOrEmpty(GetCaptchaSecret()))
                throw new Exception($"Environment variable {RESOAR_CAPTCHA_SECRET} is not set");

            if (String.IsNullOrEmpty(GetCaptchaSiteKey()))
                throw new Exception($"Environment variable {RESOAR_CAPTCHA_SITE_KEY} is not set");

            if (String.IsNullOrEmpty(GetSMTPHost()))
                throw new Exception($"Environment variable {RESOAR_SMTP_HOST} is not set");

            if (String.IsNullOrEmpty(GetSMTPPort()))
                throw new Exception($"Environment variable {RESOAR_SMTP_PORT} is not set");

            if (String.IsNullOrEmpty(GetSMTPUser()))
                throw new Exception($"Environment variable {RESOAR_SMTP_USER} is not set");

            if (String.IsNullOrEmpty(GetSMTPEmail()))
                throw new Exception($"Environment variable {RESOAR_SMTP_EMAIL} is not set");

            if (String.IsNullOrEmpty(GetSMTPPassword()))
                throw new Exception($"Environment variable {RESOAR_SMTP_PASSWORD} is not set");

            if (String.IsNullOrEmpty(GetStorageAccessKey()))
                throw new Exception($"Environment variable {RESOAR_STORAGE_ACCESS_KEY} is not set");

            if (String.IsNullOrEmpty(GetStorageSecretKey()))
                throw new Exception($"Environment variable {RESOAR_STORAGE_SECRET_KEY} is not set");

            var jwtSecret = GetJwtSecret();
            if (String.IsNullOrEmpty(jwtSecret))
                throw new Exception($"Environment variable {RESOAR_JWT_SECRET} is not set");

            if (jwtSecret.Length < 32)
                throw new Exception($"Environment variable {RESOAR_JWT_SECRET} must have at least 32 characters");
        }
    }
}
