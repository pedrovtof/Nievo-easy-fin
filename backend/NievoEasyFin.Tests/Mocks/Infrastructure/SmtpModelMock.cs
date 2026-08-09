using NievoEasyFin.Application.Models;

namespace NievoEasyFin.Tests.Mocks.Infrastructure;

/// <summary>
/// Mock for SmtpModel that avoids real network calls.
/// Since SmtpModel methods are NOT virtual, we shadow them here with 'new'
/// and use this class in tests instead of the real one.
/// </summary>
public class SmtpModelMock : SmtpModel
{
    public bool WasResetTokenMailCalled { get; private set; }
    public bool WasSingUpUserTokenMailCalled { get; private set; }
    public string? LastEmailSentTo { get; private set; }

    public override Task<bool> ResetTokenMailAsync(string email, int token)
    {
        WasResetTokenMailCalled = true;
        LastEmailSentTo = email;
        return Task.FromResult(true);
    }

    /// <summary>
    /// Shadows SmtpModel.SingUpUserTokenMailAsync to avoid real SMTP calls in tests.
    /// </summary>
    public override Task<bool> SingUpUserTokenMailAsync(string email, int token)
    {
        WasSingUpUserTokenMailCalled = true;
        LastEmailSentTo = email;
        return Task.FromResult(true);
    }

    public override Task<bool> TestSendEmailAsync(string email)
    {
        return Task.FromResult(true);
    }
}

