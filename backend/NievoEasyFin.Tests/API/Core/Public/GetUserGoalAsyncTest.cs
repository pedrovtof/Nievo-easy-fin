using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetUserGoalAsyncTest : AccountsServiceTestBase
{
    public GetUserGoalAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "GetUserGoal With Valid Request Returns Ok")]
    public async Task GetUserGoal_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var request = new GetUserGoalRequestBuilder().WithEmail(existingUser.Email);

        // Seed goal
        var goal = new GoalEntity
        {
            Name = "Test Goal",
            Description = "Description",
            UserId = existingUser.Id,
            Amount = 1000,
            IsPercent = false,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(30)
        };
        origin.Goal.Add(goal);
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserGoal(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<GetUserGoalResponse>();

        var data = (GetUserGoalResponse)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(1);
        data.Items.First().Name.Should().Be(goal.Name);

        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetUserGoal With No Goals Returns Empty List Ok")]
    public async Task GetUserGoal_WithNoGoals_ReturnsEmptyListOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var request = new GetUserGoalRequestBuilder().WithEmail(existingUser.Email);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserGoal(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<ResponsePaginationBase<UserGoalView>>();

        var data = (ResponsePaginationBase<UserGoalView>)response.Data;
        data.Items.Should().BeEmpty();

        Output.WriteLine("Success test executed correctly.");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "GetUserGoal With Invalid Request Returns BadRequest")]
    public async Task GetUserGoal_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetUserGoalRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserGoal(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.GETUSERGOALASYNC_CORESERVICE_400_INVALID_PAGE.ToString()) || e.Contains("The specified condition was not met for 'Page'."));

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetUserGoal When User Not Found Returns NotFound")]
    public async Task GetUserGoal_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new GetUserGoalRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserGoal(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.GETUSERGOALASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    #endregion
}
