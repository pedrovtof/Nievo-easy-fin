using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class SchedulerTypeEntityFaker
{
    public static Faker<SchedulerTypeEntity> Create()
    {
        return new Faker<SchedulerTypeEntity>()
            .RuleFor(st => st.Id, f => f.Random.Int(1, 100000))
            .RuleFor(st => st.Name, f => f.Commerce.ProductName())
            .RuleFor(st => st.Description, f => f.Commerce.ProductDescription())
            .RuleFor(st => st.CronTab, f => "0 0 * * *")
            .RuleFor(st => st.Active, true)
            .RuleFor(st => st.CreatedAt, f => f.Date.Recent())
            .RuleFor(st => st.UpdatedAt, f => f.Date.Recent());
    }
}
