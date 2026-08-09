using NievoEasyFin.Tests.Mocks.Helpers;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostCreateUserAsyncTest : UsersServiceTestBase
{
    public PostCreateUserAsyncTest(WireMockFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    #region Success

    [Fact(DisplayName = "PostCreateUserAsync: With valid request returns Created")]
    public async Task PostCreateUserAsync_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        Output.WriteLine("Setting up valid create user request");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var code = Environment.GetEnvironmentVariable("CODE_SINGUP_TERMS") ?? "SINGUP_TERMS_V1";
        var connection = authOrigin.Database.GetDbConnection();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = $"INSERT INTO journey.accept_terms (code, name, version, active, created_at, updated_at) VALUES ('{code}', 'Terms of Service', 1, 1, datetime('now'), datetime('now'));";
            cmd.ExecuteNonQuery();
        }

        var smtpMock = new SmtpModelMock();
        var service = CreateService(authOrigin, authReplica, smtpMock);

        // Act
        var result = await service.PostCreateUserAsync(request);
        Output.WriteLine("Execution of PostCreateUserAsync succeeded");

        // Assert
        if (result is BadRequestObjectResult badReq)
        {
            var err = (ResponseApiError)badReq.Value!;
            throw new Exception($"Create User failed with: {string.Join(", ", err.Messages)}");
        }

        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();

        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(request.Name);
        userInDb.StatusId.Should().Be((int)EnumUserStatus.INVALID);

        smtpMock.WasSingUpUserTokenMailCalled.Should().BeTrue();
        smtpMock.LastEmailSentTo.Should().Be(request.Email);
        
        Output.WriteLine("User successfully verified in database");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "PostCreateUserAsync: When email exists with active status returns BadRequest")]
    public async Task PostCreateUserAsync_WhenEmailExistsWithActiveStatus_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up create user request with existing active email");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.Email;
        existingUser.StatusId = (int)EnumUserStatus.ACTIVE;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("already exists"));
        
        Output.WriteLine("Validation passed: returned BadRequest for existing email");
    }

    [Fact(DisplayName = "PostCreateUserAsync: When email exists with invalid status returns BadRequest")]
    public async Task PostCreateUserAsync_WhenEmailExistsWithInvalidStatus_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up create user request with existing invalid email");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.Email;
        existingUser.StatusId = (int)EnumUserStatus.INVALID;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("not valid") || e.Contains("validate it again"));
        
        Output.WriteLine("Validation passed: returned BadRequest for invalid existing email");
    }

    [Fact(DisplayName = "PostCreateUserAsync: When terms not accepted returns BadRequest")]
    public async Task PostCreateUserAsync_WhenTermsNotAccepted_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up create user request with unaccepted terms");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";
        request.AcceptTerms = false; 

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
        
        Output.WriteLine("Validation passed: terms not accepted returned BadRequest");
    }

    [Fact(DisplayName = "PostCreateUserAsync: When host is empty returns BadRequest")]
    public async Task PostCreateUserAsync_WhenHostIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up create user request with empty host");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";
        request.SetHost(string.Empty);

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        Output.WriteLine("Validation passed: empty host returned BadRequest");
    }

    [Fact(DisplayName = "PostCreateUserAsync: When UserAgent is empty returns BadRequest")]
    public async Task PostCreateUserAsync_WhenUserAgentIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up create user request with empty UserAgent");
        var request = new PostCreateUserRequestBuilder();
        request.Password = "Strong@123";
        request.SetUserAgent(string.Empty);

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        Output.WriteLine("Validation passed: empty UserAgent returned BadRequest");
    }

    #endregion
}
