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

public class GetUserCategoryAsyncTest : AccountsServiceTestBase
{
    public GetUserCategoryAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "GetUserCategory With Valid Request Returns Ok")]
    public async Task GetUserCategory_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var request = new GetUserCategoryRequestBuilder().WithEmail(existingUser.Email);

        // Seed category
        var category = new CategoryEntity
        {
            Name = "Test Category",
            Description = "Description",
            UserId = existingUser.Id,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };
        origin.Category.Add(category);
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserCategory(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<GetUserCategoryResponse>();

        var data = (GetUserCategoryResponse)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(1);
        data.Items.First().Name.Should().Be(category.Name);

        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetUserCategory With No Categories Returns Empty List Ok")]
    public async Task GetUserCategory_WithNoCategories_ReturnsEmptyListOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var existingUser = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var request = new GetUserCategoryRequestBuilder().WithEmail(existingUser.Email);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserCategory(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<ResponsePaginationBase<UserCategoryView>>();

        var data = (ResponsePaginationBase<UserCategoryView>)response.Data;
        data.Items.Should().BeEmpty();

        Output.WriteLine("Success test executed correctly.");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "GetUserCategory With Invalid Request Returns BadRequest")]
    public async Task GetUserCategory_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetUserCategoryRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserCategory(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.GETUSERCATEGORYASYNC_CORESERVICE_400_INVALID_PAGE.ToString()) || e.Contains("The specified condition was not met for 'Page'."));

        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetUserCategory When User Not Found Returns NotFound")]
    public async Task GetUserCategory_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new GetUserCategoryRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserCategory(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.GETUSERCATEGORYASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());

        Output.WriteLine("Validation executed successfully.");
    }

    #endregion
}
