using NievoEasyFin.Application.Infrastructure.Auth;

namespace NievoEasyFin.Application.Models
{
    public class SmtpModel : SmtpProvider
    {
        public SmtpModel() : base() { }

        /// <summary>
        /// Method used to send email to reset password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns>true</returns>
        public async Task<bool> ResetTokenMailAsync(string email, int token)
        {
            string? fileWithBody = null;

            if (!string.IsNullOrEmpty(PATH_MAIL_BODY_TEMPLATE_PASSWORD_RESET_TOKEN))
            {
                fileWithBody = await File.ReadAllTextAsync(PATH_MAIL_BODY_TEMPLATE_PASSWORD_RESET_TOKEN);
            }
            else
            {
                fileWithBody = $"""
                    <span>
                        Email: EMAIL_USER
                        Token: TOKEN_USER
                    </span>
                """;
            }

            fileWithBody = fileWithBody
                .Replace("EMAIL_USER", email)
                .Replace("TOKEN_USER", token.ToString());

            var mail = await SendMailFromToAsync(
               $"{SMTP_DEFAULT_PREFIX_MAIL_CONTENT}Token reset email",
                email,
                fileWithBody
            );

            return true;
        }

        /// <summary>
        /// Method used to send email to singup user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns>true</returns>
        public async Task<bool> SingUpUserTokenMailAsync(string email, int token)
        {
            string? fileWithBody = null;

            if (!string.IsNullOrEmpty(PATH_MAIL_BODY_TEMPLATE_SINGUP_USER_TOKEN))
            {
                fileWithBody = await File.ReadAllTextAsync(PATH_MAIL_BODY_TEMPLATE_SINGUP_USER_TOKEN);
            }
            else
            {
                fileWithBody = $"""
                    <span>
                        Email: EMAIL_USER
                        Token: TOKEN_USER
                    </span>
                """;
            }

            fileWithBody = fileWithBody
                .Replace("EMAIL_USER", email)
                .Replace("TOKEN_USER", token.ToString());

            var mail = await SendMailFromToAsync(
               $"{SMTP_DEFAULT_PREFIX_MAIL_CONTENT}Token Singup email",
                email,
                fileWithBody
            );

            return true;
        }

        /// <summary>
        /// Method to check smtp
        /// </summary>
        /// <param name="email">email to send test</param>
        /// <returns>true</returns>
        public async Task<bool> TestSendEmailAsync(string email)
        {
            string fileWithBody = "<span>teste</span>";
            var mail = await SendMailFromToAsync(
                $"{SMTP_DEFAULT_PREFIX_MAIL_CONTENT}Teste email",
                email,
                fileWithBody
            );

            return true;
        }
    }
}
