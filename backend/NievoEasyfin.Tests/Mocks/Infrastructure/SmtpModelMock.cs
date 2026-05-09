using NievoEasyfin.Application.Models;

namespace NievoEasyfin.Tests.Mocks.Infrastructure;

/// <summary>
/// Mock for SmtpModel that avoids real network calls.
/// Since SmtpModel.ResetTokenMailAsync is NOT virtual, we override it here
/// and use this class in tests instead of the real one.
/// </summary>
public class SmtpModelMock : SmtpModel
{
    public bool WasResetTokenMailCalled { get; private set; }
    public string? LastEmailSentTo { get; private set; }

    public new Task<bool> ResetTokenMailAsync(string email, int token)
    {
        WasResetTokenMailCalled = true;
        LastEmailSentTo = email;
        return Task.FromResult(true);
    }

    public new Task<bool> TestSendEmailAsync(string email)
    {
        return Task.FromResult(true);
    }
}
