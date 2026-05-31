using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PatchResetPasswordRequestFaker
{
    public static Faker<PatchResetPasswordRequest> Create()
    {
        return new Faker<PatchResetPasswordRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.PinToken, f => f.Random.Number(100000, 999999))
            .RuleFor(r => r.Password, f => f.Internet.Password(8));
    }
}
