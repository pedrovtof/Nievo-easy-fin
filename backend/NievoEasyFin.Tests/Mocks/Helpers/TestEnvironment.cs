using System;

namespace NievoEasyFin.Tests.Mocks.Helpers
{
    public static class TestEnvironment
    {
        public static void Setup()
        {
            Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_ITERATIONS", "350000");
            Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_KEYSIZE", "64");
            Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_SALT", "4142434445464748494A4B4C4D4E4F505152535455565758595A6162636465666768696A6B6C6D6E6F707172737475767778797A31323334353637383930");
            Environment.SetEnvironmentVariable("REGEX_PASSWORD_RULE", "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,12}$");
            
            Environment.SetEnvironmentVariable("GOOGLE_ID_CLIENT", "test-google-client-id");
            Environment.SetEnvironmentVariable("GOOGLE_ID_KEY", "");
            Environment.SetEnvironmentVariable("GOOGLE_API_USER_INFO", "http://localhost:8080/userinfo");
            Environment.SetEnvironmentVariable("GOOGLE_API_TOKEN_INFO", "http://localhost:8080/tokeninfo");
            
            Environment.SetEnvironmentVariable("JWT_PRIVATE_CONTRACT_STRING", "super-secret-private-key-long-enough-32-chars");
            Environment.SetEnvironmentVariable("JWT_PUBLIC_CONTRACT_STRING", "super-secret-public-key-long-enough-32-chars");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "Nievo");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "EasyFin");
            
            Environment.SetEnvironmentVariable("SMTP_DEFAULT_PREFIX_MAIL_CONTENT", "Easy Fin - ");
            Environment.SetEnvironmentVariable("SMTP_DEFAULT_FROM_ADRESS_MAIL", "noreply@example.com");
            Environment.SetEnvironmentVariable("SMTP_SERVER_USER_NAME", "smtp_user");
            Environment.SetEnvironmentVariable("SMTP_SERVER_USER_PASSWORD", "smtp_password");
            Environment.SetEnvironmentVariable("SMTP_SERVER_HOST", "127.0.0.1");
            Environment.SetEnvironmentVariable("SMTP_SERVER_PORT", "25");
            Environment.SetEnvironmentVariable("PATH_MAIL_BODY_TEMPLATE_PASSWORD_RESET_TOKEN", "");
            
            Environment.SetEnvironmentVariable("CODE_SINGUP_TERMS", "SINGUP_TERMS_V1");
        }
    }
}
