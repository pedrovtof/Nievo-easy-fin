using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace NievoEasyfin.Application.Infrastructure.Auth
{
    public class SmtpProvider
    {
        protected readonly string SMTP_DEFAULT_FROM_ADRESS_MAIL = DotNetEnv.Env.GetString("SMTP_DEFAULT_FROM_ADRESS_MAIL");

        protected readonly string SMTP_SERVER_USER_NAME = DotNetEnv.Env.GetString("SMTP_SERVER_USER_NAME");

        protected readonly string SMTP_SERVER_USER_PASSWORD = DotNetEnv.Env.GetString("SMTP_SERVER_USER_PASSWORD");

        protected readonly string SMTP_SERVER_HOST = DotNetEnv.Env.GetString("SMTP_SERVER_HOST");

        protected readonly int SMTP_SERVER_PORT = DotNetEnv.Env.GetInt("SMTP_SERVER_PORT");

        protected readonly string PATH_MAIL_BODY_TEMPLATE_PASSWORD_RESET_TOKEN = DotNetEnv.Env.GetString("PATH_MAIL_BODY_TEMPLATE_PASSWORD_RESET_TOKEN");

        public SmtpProvider() { }

        /// <summary>
        /// Method to send email From X@x.com to Y@y.com
        /// </summary>
        /// <param name="mailSubject">User to send email</param>
        /// <param name="mailTo">user writer from the email</param>
        /// <param name="mailBody">content of email</param>
        /// <param name="from">Optional custom user from</param>
        /// <returns>bool</returns>
        protected async Task<bool> SendMailFromToAsync(string mailSubject, string mailTo, string mailBody, string from = null)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(
                SMTP_SERVER_HOST,
                SMTP_SERVER_PORT,
                SecureSocketOptions.SslOnConnect
            );
            smtp.Authenticate(
                SMTP_SERVER_USER_NAME,
                SMTP_SERVER_USER_PASSWORD
            );

            var email = await CreateEmailMessageAsync(mailSubject, mailTo, mailBody, from);
            var send = smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }

        /// <summary>
        /// Method to send email From X@x.com to Y@y.com
        /// </summary>
        /// <param name="email">MimeMessage</param>
        /// <returns>bool</returns>
        protected async Task<bool> SendMailFromToAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(
                SMTP_SERVER_HOST,
                SMTP_SERVER_PORT,
                SecureSocketOptions.SslOnConnect
            );
            smtp.Authenticate(
                SMTP_SERVER_USER_NAME,
                SMTP_SERVER_USER_PASSWORD
            );

            var send = smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }

        /// <summary>
        /// Method to generate the mail
        /// </summary>
        /// <param name="mailSubject">User to send email</param>
        /// <param name="mailTo">user writer from the email</param>
        /// <param name="mailBody">content of email</param>
        /// <param name="from">Optional custom user from</param>
        /// <returns>MimeMessage</returns>
        protected async Task<MimeMessage> CreateEmailMessageAsync(string mailSubject, string mailTo, string mailBody, string? from)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(from == null ? SMTP_DEFAULT_FROM_ADRESS_MAIL : from));
            email.To.Add(MailboxAddress.Parse(mailTo));
            email.Subject = mailSubject;
            email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

            return email;
        }
    }
}