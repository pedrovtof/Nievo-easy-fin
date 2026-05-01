using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Services;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NievoEasyfin.Auth.Controllers.Public;
using NievoEasyfin.Tests.Build.Request;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.Auth.Public;

public class AuthenticatorTest
{
    private readonly Faker _faker = new Faker("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;

    public AuthenticatorTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with success
    /// </summary>
    [Fact(DisplayName = "Login deverá ser feito com sucesso")]
    public async Task FluentAssertions_PostLoginUserAsync_SucessAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiSucess(new { Token = "mocked-jwt-token" });
        var okResult = new OkObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        _testOutputHelper.WriteLine($"\n Validado sucesso com {requestBuilder.Email} e {requestBuilder.Password} \n");
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with empty email error
    /// </summary>
    [Fact(DisplayName = "Login deverá retornar erro de email vazio")]
    public async Task FluentAssertions_PostLoginUserAsync_POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULLAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithEmail(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro email vazio. \n");
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with empty password error
    /// </summary>
    [Fact(DisplayName = "Login deverá retornar erro de senha vazia")]
    public async Task FluentAssertions_PostLoginUserAsync_POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULLAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithPassword(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro senha vazia. \n");
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with user not found error
    /// </summary>
    [Fact(DisplayName = "Login deverá retornar erro de usuário não encontrado")]
    public async Task FluentAssertions_PostLoginUserAsync_POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUNDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> {
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription(),
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED.GetDescription()
        });
        var notFoundResult = new NotFoundObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro usuário não encontrado. \n");
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with user blocked error
    /// </summary>
    [Fact(DisplayName = "Login deverá retornar erro de usuário bloqueado")]
    public async Task FluentAssertions_PostLoginUserAsync_POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKEDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> {
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription(),
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED.GetDescription()
        });
        var notFoundResult = new NotFoundObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro usuário bloqueado. \n");
    }

    /// <summary>
    /// Method to test endpoint POSTLOGINUSER with wrong password error
    /// </summary>
    [Fact(DisplayName = "Login deverá retornar erro de senha incorreta")]
    public async Task FluentAssertions_PostLoginUserAsync_POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro senha incorreta. \n");
    }
}
