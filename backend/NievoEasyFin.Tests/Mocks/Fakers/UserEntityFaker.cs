using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class UserEntityFaker
{
    public static Faker<UserEntity> Create()
    {
        return new Faker<UserEntity>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Phone, f => f.Random.Int(10000000, 99999999))
            .RuleFor(u => u.StatusId, 1)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt, f => f.Date.Recent())
            .RuleFor(u => u.UpdatedAt, f => f.Date.Recent());
    }
}
