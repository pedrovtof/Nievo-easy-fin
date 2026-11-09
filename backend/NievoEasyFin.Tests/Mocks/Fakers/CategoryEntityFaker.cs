using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class CategoryEntityFaker
{
    public static Faker<CategoryEntity> Create()
    {
        return new Faker<CategoryEntity>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
            .RuleFor(c => c.Active, true)
            .RuleFor(c => c.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.GoalId, f => f.Random.Int(1, 100))
            .RuleFor(c => c.ParentCategory, (int?)null)
            .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
            .RuleFor(c => c.UpdatedAt, f => f.Date.Recent());
    }
}
