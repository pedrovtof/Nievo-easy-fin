namespace NievoEasyfin.Application.Interfaces.Models;

public interface ISmtpModel
{
    Task<bool> ResetTokenMailAsync(string email, int token);
    Task<bool> TestSendEmailAsync(string email);
}
