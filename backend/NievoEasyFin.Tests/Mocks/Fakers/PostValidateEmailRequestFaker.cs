using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostValidateEmailRequestFaker
{
    public static Faker<PostValidateEmailRequest> Create()
    {
        return new Faker<PostValidateEmailRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.PinToken, f => f.Random.Number(100000, 999999));
    }
}
