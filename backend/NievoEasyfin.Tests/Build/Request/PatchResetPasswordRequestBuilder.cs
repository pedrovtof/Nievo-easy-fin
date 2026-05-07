using Bogus;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Tests.Build.Generators;

namespace NievoEasyfin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PatchResetPasswordRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PatchResetPasswordRequestBuilder : PatchResetPasswordRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PatchResetPasswordRequestBuilder()
    {
        Email = _faker.Person.Email;
        PinToken = _faker.Random.Number(100000, 999999).ToString();
        Password = PasswordGenerator.Generate();
    }

    public PatchResetPasswordRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public PatchResetPasswordRequestBuilder WithPinToken(string pinToken)
    {
        PinToken = pinToken;
        return this;
    }

    public PatchResetPasswordRequestBuilder WithPassword(string password)
    {
        Password = password;
        return this;
    }
}
