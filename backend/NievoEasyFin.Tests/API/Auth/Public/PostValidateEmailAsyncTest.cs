using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

public class PostValidateEmailAsyncTest : AuthenticatorServiceTestBase
{
    public PostValidateEmailAsyncTest(WireMockFixture fixture, ITestOutputHelper output) 
        : base(fixture, output)
    {
    }

    #region BadRequest Errors
    [Fact(DisplayName = "PostValidateEmailAsync: When user not found, returns NotFound")]
    public async Task PostValidateEmailAsync_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmail test for user not found.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        var request = new PostValidateEmailRequest { Email = "notexist@example.com", PinToken = 123456 };
        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostValidateEmailAsync.");
        var result = await service.PostValidateEmailAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains("not have an account") || e.Contains("email is incorrect"));
    }

    [Fact(DisplayName = "PostValidateEmailAsync: When user already active, returns BadRequest")]
    public async Task PostValidateEmailAsync_WhenUserAlreadyActive_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmail test for already active user.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.ACTIVE;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var request = new PostValidateEmailRequest { Email = user.Email!, PinToken = 123456 };
        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostValidateEmailAsync.");
        var result = await service.PostValidateEmailAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("already been validated") || e.Contains("blocked"));
    }

    [Fact(DisplayName = "PostValidateEmailAsync: When token not found in cache, returns NotFound")]
    public async Task PostValidateEmailAsync_WhenTokenNotFoundInCache_ReturnsNotFound()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmail test for token not found.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.INVALID;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null); 

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var service = CreateService(origin, replica, cacheService);

        var request = new PostValidateEmailRequest { Email = user.Email!, PinToken = 123456 };

        // Act
        Output.WriteLine("Executing PostValidateEmailAsync.");
        var result = await service.PostValidateEmailAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains("found one token") || e.Contains("not possible"));
    }

    [Fact(DisplayName = "PostValidateEmailAsync: When token mismatch, returns BadRequest")]
    public async Task PostValidateEmailAsync_WhenTokenMismatch_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmail test for token mismatch.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.INVALID;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var correctPin = 654321;
        var cacheData = new { email = user.Email, name = user.Name, pin_token = correctPin, created_at = DateTime.UtcNow };

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(JsonSerializer.Serialize(cacheData));

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var service = CreateService(origin, replica, cacheService);

        var request = new PostValidateEmailRequest { Email = user.Email!, PinToken = 999999 }; 

        // Act
        Output.WriteLine("Executing PostValidateEmailAsync.");
        var result = await service.PostValidateEmailAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("does not match") || e.Contains("try again"));
    }
    #endregion

    #region Success
    [Fact(DisplayName = "PostValidateEmailAsync: With valid token, returns Ok and activates user")]
    public async Task PostValidateEmailAsync_WithValidToken_ReturnsOkAndActivatesUser()
    {
        // Arrange
        Output.WriteLine("Arranging PostValidateEmail test for success.");
        var user = UserEntityFaker.Create().Generate();
        user.StatusId = (int)EnumUserStatus.INVALID;
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var pinToken = 123456;
        var cacheData = new { email = user.Email, name = user.Name, pin_token = pinToken, created_at = DateTime.UtcNow };

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(JsonSerializer.Serialize(cacheData));
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var service = CreateService(origin, replica, cacheService);

        var request = new PostValidateEmailRequest { Email = user.Email!, PinToken = pinToken };

        // Act
        Output.WriteLine("Executing PostValidateEmailAsync.");
        var result = await service.PostValidateEmailAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badReq)
        {
            var err = (ResponseApiError)badReq.Value!;
            throw new Exception($"PostValidateEmail failed with: {string.Join(", ", err.Messages)}");
        }
        if (result is NotFoundObjectResult notFoundRes)
        {
            var err = (ResponseApiError)notFoundRes.Value!;
            throw new Exception($"PostValidateEmail not found: {string.Join(", ", err.Messages)}");
        }

        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var updatedUser = await replica.Users.AsNoTracking().FirstAsync(u => u.Id == user.Id);
        updatedUser.StatusId.Should().Be((int)EnumUserStatus.ACTIVE);
    }
    #endregion
}
