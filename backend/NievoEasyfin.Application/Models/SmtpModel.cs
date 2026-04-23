using NievoEasyfin.Application.Infrastructure.Auth;

namespace NievoEasyfin.Application.Models
{
    public class SmtpModel : SmtpProvider
    {
        public SmtpModel() : base() { }

        /// <summary>
        /// Method used to send email to reset password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
                "Easy Fin - Token reset email",
                email,
                fileWithBody
            );

            return true;
        }
    }
}
