using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostResetPasswordRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostResetPasswordRequestBuilder : PostResetPasswordRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostResetPasswordRequestBuilder()
    {
        Email = _faker.Person.Email;
    }

    public PostResetPasswordRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }
}
