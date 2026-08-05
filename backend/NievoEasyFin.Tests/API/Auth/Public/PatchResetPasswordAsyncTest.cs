using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using StackExchange.Redis;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PatchResetPasswordAsyncTest : AuthenticatorServiceTestBase
{
    public PatchResetPasswordAsyncTest(WireMockFixture fixture, ITestOutputHelper output) 
        : base(fixture, output)
    {
    }

    #region Success
    [Fact(DisplayName = "PatchResetPasswordAsync: With valid token, returns Ok")]
    public async Task PatchResetPasswordAsync_WithValidToken_ReturnsOk()
    {
        // Arrange
        Output.WriteLine("Arranging PatchResetPassword test.");
        var user = UserEntityFaker.Create().Generate();
        user.Password = "OldPasswordHash";
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var pinToken = 123456;
        var request = new PatchResetPasswordRequestBuilder();
        request.Email = user.Email!;
        request.PinToken = pinToken;
        request.Password = "Strong@123";

        var dbMock = new Mock<IDatabase>();
        var cacheData = new { user_id = user.Id, email = user.Email, pin_token = pinToken };
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(JsonSerializer.Serialize(cacheData));

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);

        await DbContextMockFactory.SyncToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, cacheService);

        // Act
        Output.WriteLine("Executing PatchResetPasswordAsync.");
        var result = await service.PatchResetPasswordAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badRequest)
        {
            var error = (ResponseApiError)badRequest.Value!;
            throw new Exception($"Patch Password failed with BAD REQUEST: {string.Join(", ", error.Messages)}");
        }
        if (result is NotFoundObjectResult notFound)
        {
            var error = (ResponseApiError)notFound.Value!;
            throw new Exception($"Patch Password failed with NOT FOUND: {string.Join(", ", error.Messages)}");
        }
        result.Should().BeOfType<OkObjectResult>();

        var updatedUser = await replica.Users.AsNoTracking().FirstAsync(u => u.Id == user.Id);
        updatedUser.Password.Should().NotBe("OldPasswordHash");
    }
    #endregion
}
