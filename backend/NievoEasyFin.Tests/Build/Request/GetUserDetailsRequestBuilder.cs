using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for <see cref="GetUserDetailsRequest"/>.
/// </summary>
public class GetUserDetailsRequestBuilder : GetUserDetailsRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    /// <summary>
    /// Initializes a new instance of <see cref="GetUserDetailsRequestBuilder"/> with a valid email.
    /// </summary>
    public GetUserDetailsRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
    }

    /// <summary>
    /// Sets a custom email on the request.
    /// </summary>
    /// <param name="email">Email string to assign.</param>
    /// <returns>The current builder instance.</returns>
    public GetUserDetailsRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }
}
