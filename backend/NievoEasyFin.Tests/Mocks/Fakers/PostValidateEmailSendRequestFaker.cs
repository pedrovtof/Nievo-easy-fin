using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostValidateEmailSendRequestFaker
{
    public static Faker<PostValidateEmailSendRequest> Create()
    {
        return new Faker<PostValidateEmailSendRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email);
    }
}
