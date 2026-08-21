using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using StackExchange.Redis;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostResetPasswordAsyncTest : AuthenticatorServiceTestBase
{
    public PostResetPasswordAsyncTest(WireMockFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    #region Success
    [Fact(DisplayName = "PostResetPasswordAsync: When user exists, returns Created")]
    public async Task PostResetPasswordAsync_WhenUserExists_ReturnsCreated()
    {
        // Arrange
        Output.WriteLine("Arranging PostResetPassword test.");
        var user = UserEntityFaker.Create().Generate();
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var request = new PostResetPasswordRequestBuilder();
        request.Email = user.Email!;

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);
        var smtpMock = new SmtpModelMock();

        await DbContextMockFactory.SyncToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, cacheService, smtpMock);

        // Act
        Output.WriteLine("Executing PostResetPasswordAsync.");
        var result = await service.PostResetPasswordAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badRequest)
        {
            var error = (ResponseApiError)badRequest.Value!;
            throw new Exception($"PostResetPassword failed with: {string.Join(", ", error.Messages)}");
        }

        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(201);

        smtpMock.WasResetTokenMailCalled.Should().BeTrue();
        smtpMock.LastEmailSentTo.Should().Be(user.Email);
    }
    #endregion
}
