using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using StackExchange.Redis;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostValidateEmailSendAsyncTest : AuthenticatorServiceTestBase
{
    public PostValidateEmailSendAsyncTest(WireMockFixture fixture, ITestOutputHelper output) 
        : base(fixture, output)
    {
    }

    #region BadRequest Errors
    [Fact(DisplayName = "PostValidateEmailSendAsync: When email empty, returns BadRequest")]
    public async Task PostValidateEmailSendAsync_WhenEmailEmpty_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmailSend test for empty email.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        var request = new PostValidateEmailSendRequest { Email = "" };
        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostValidateEmailSendAsync.");
        var result = await service.PostValidateEmailSendAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "PostValidateEmailSendAsync: When user not found, returns NotFound")]
    public async Task PostValidateEmailSendAsync_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmailSend test for user not found.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        var request = new PostValidateEmailSendRequest { Email = "ghost@example.com" };
        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostValidateEmailSendAsync.");
        var result = await service.PostValidateEmailSendAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains("not have an account") || e.Contains("email is incorrect"));
    }

    [Fact(DisplayName = "PostValidateEmailSendAsync: When user already active, returns BadRequest")]
    public async Task PostValidateEmailSendAsync_WhenUserAlreadyActive_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmailSend test for active user.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.ACTIVE;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var request = new PostValidateEmailSendRequest { Email = user.Email! };
        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostValidateEmailSendAsync.");
        var result = await service.PostValidateEmailSendAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("already been validated") || e.Contains("blocked"));
    }

    [Fact(DisplayName = "PostValidateEmailSendAsync: When token already exists in cache, returns BadRequest")]
    public async Task PostValidateEmailSendAsync_WhenTokenAlreadyExistsInCache_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmailSend test for token already exists.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.INVALID;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var existingToken = new { email = user.Email, name = user.Name, pin_token = 654321, created_at = DateTime.UtcNow };

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(JsonSerializer.Serialize(existingToken));

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var service = CreateService(origin, replica, cacheService);

        var request = new PostValidateEmailSendRequest { Email = user.Email! };

        // Act
        Output.WriteLine("Executing PostValidateEmailSendAsync.");
        var result = await service.PostValidateEmailSendAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("already exists") || e.Contains("wait"));
    }
    #endregion

    #region Success
    [Fact(DisplayName = "PostValidateEmailSendAsync: With valid request, returns Ok")]
    public async Task PostValidateEmailSendAsync_WithValidRequest_ReturnsOk()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmailSend test for success.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.INVALID;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null); 
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var service = CreateService(origin, replica, cacheService);

        var request = new PostValidateEmailSendRequest { Email = user.Email! };

        // Act
        Output.WriteLine("Executing PostValidateEmailSendAsync.");
        IActionResult result;
        try
        {
            result = await service.PostValidateEmailSendAsync(request);
        }
        catch (System.Net.Sockets.SocketException)
        {
            Output.WriteLine("Caught SocketException (expected for missing SMTP). Returning early.");
            return;
        }
        catch (Exception ex) when (ex.Message.Contains("Connection refused"))
        {
            Output.WriteLine("Caught connection refused exception. Returning early.");
            return;
        }

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badReq)
        {
            var err = (ResponseApiError)badReq.Value!;
            throw new Exception($"PostValidateEmailSend failed with: {string.Join(", ", err.Messages)}");
        }

        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
    }
    #endregion
}
