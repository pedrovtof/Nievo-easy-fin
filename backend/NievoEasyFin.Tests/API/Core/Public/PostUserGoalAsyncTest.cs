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

namespace NievoEasyFin.Tests.API.Core.Public;

public class PostUserGoalAsyncTest : AccountsServiceTestBase
{
    public PostUserGoalAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "PostUserGoal With Valid Request Returns Ok")]
    public async Task PostUserGoal_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new PostUserGoalRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserGoal(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_200_GOAL_CREATED.GetDescription());

        // Verify goal was created in database
        var goalInDb = await origin.Goal.FirstOrDefaultAsync(g => g.Name == request.Name && g.UserId == existingUser.Id);
        goalInDb.Should().NotBeNull();
        goalInDb!.Active.Should().BeTrue();
        goalInDb.Amount.Should().Be(request.Amount);

        Output.WriteLine("Success test executed correctly.");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "PostUserGoal With Invalid Request Returns BadRequest")]
    public async Task PostUserGoal_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserGoalRequestBuilder();
        request.Name = "";
        request.Amount = -1;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserGoal(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserGoal When User Not Found Returns NotFound")]
    public async Task PostUserGoal_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserGoalRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserGoal(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserGoal When Goal Already Exists Returns BadRequest")]
    public async Task PostUserGoal_WhenGoalAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserGoalRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        // Seed goal
        var existingGoal = new GoalEntity
        {
            Name = request.Name,
            Description = request.Description ?? "",
            UserId = existingUser.Id,
            Amount = request.Amount,
            IsPercent = request.IsPercent,
            Active = true,
            ExpireAt = request.ExpireAt,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        origin.Goal.Add(existingGoal);
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserGoal(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_GOAL_ALREADY_EXIST.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    #endregion
}
