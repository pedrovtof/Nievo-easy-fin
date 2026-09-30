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

public class PostUserCategoryAsyncTest : AccountsServiceTestBase
{
    public PostUserCategoryAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "PostUserCategory With Valid Request Returns Ok")]
    public async Task PostUserCategory_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new PostUserCategoryRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_200_CREATED.GetDescription());

        // Verify category was created in database
        var categoryInDb = await origin.Category.FirstOrDefaultAsync(c => c.Name == request.Name && c.UserId == existingUser.Id);
        categoryInDb.Should().NotBeNull();
        categoryInDb!.Active.Should().BeTrue();
        categoryInDb.Description.Should().Be(request.Description);
        categoryInDb.GoalId.Should().Be(request.Goal);

        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "PostUserCategory With Valid Parent Category Returns Ok")]
    public async Task PostUserCategory_WithValidParentCategory_ReturnsOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        // Seed parent category
        var parentCategory = new CategoryEntity
        {
            Name = "Parent Category",
            Description = "Parent Description",
            UserId = existingUser.Id,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };
        origin.Category.Add(parentCategory);
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var request = new PostUserCategoryRequestBuilder()
            .WithEmail(existingUser.Email)
            .WithParentCategory(parentCategory.Id);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        // Verify category was created in database
        var categoryInDb = await origin.Category.FirstOrDefaultAsync(c => c.Name == request.Name && c.UserId == existingUser.Id);
        categoryInDb.Should().NotBeNull();
        categoryInDb!.ParentCategory.Should().Be(parentCategory.Id);

        Output.WriteLine("Success test executed correctly.");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "PostUserCategory With Invalid Request Returns BadRequest")]
    public async Task PostUserCategory_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserCategoryRequestBuilder()
            .WithName("")
            .WithGoal(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_EMPTY_NAME.ToString()) || e.Contains("'Name' must not be empty.") || e.Contains("Goal"));

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserCategory When User Not Found Returns NotFound")]
    public async Task PostUserCategory_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserCategoryRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserCategory When Parent Category Not Found Returns NotFound")]
    public async Task PostUserCategory_WhenParentCategoryNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserCategoryRequestBuilder()
            .WithParentCategory(999);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_404_PARENTCATEGORY_NOT_FOUND.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserCategory When Category Already Exists Returns BadRequest")]
    public async Task PostUserCategory_WhenCategoryAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserCategoryRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        // Seed category
        var existingCategory = new CategoryEntity
        {
            Name = request.Name,
            Description = request.Description ?? "",
            UserId = existingUser.Id,
            GoalId = request.Goal,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };
        origin.Category.Add(existingCategory);
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCategory(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_400_CATEGORY_ALREADY_EXIST.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    #endregion
}
