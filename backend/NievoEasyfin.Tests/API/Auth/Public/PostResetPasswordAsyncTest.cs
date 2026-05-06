using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Services;
using NievoEasyfin.Auth.Controllers.Public;
using NievoEasyfin.Tests.Build.Request;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.Auth.Public;

public class PostResetPasswordAsyncTest
{
    private readonly Faker _faker = new Faker("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;

    public PostResetPasswordAsyncTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Method to test endpoint PostResetPasswordAsync with success
    /// </summary>
    [Fact(DisplayName = "Solicitação de reset de senha deverá ser feita com sucesso")]
    public async Task FluentAssertions_PostResetPasswordAsync_SucessAsync()
    {
        // Arrange
        var requestBuilder = new PostResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiSucess(new { Message = "Token sent" });
        var okResult = new OkObjectResult(expectedResponse);

        mockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        _testOutputHelper.WriteLine($"\n Validado sucesso ao solicitar reset de senha para o email {requestBuilder.Email} \n");
    }

    /// <summary>
    /// Method to test endpoint PostResetPasswordAsync with invalid email error
    /// </summary>
    [Fact(DisplayName = "Solicitação de reset de senha deverá retornar erro de email inválido")]
    public async Task FluentAssertions_PostResetPasswordAsync_POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAILAsync()
    {
        // Arrange
        var requestBuilder = new PostResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithEmail("invalid-email");

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de email inválido. \n");
    }

    /// <summary>
    /// Method to test endpoint PostResetPasswordAsync with empty email error
    /// </summary>
    [Fact(DisplayName = "Solicitação de reset de senha deverá retornar erro de email nulo ou vazio")]
    public async Task FluentAssertions_PostResetPasswordAsync_POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTYAsync()
    {
        // Arrange
        var requestBuilder = new PostResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithEmail(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de email nulo ou vazio. \n");
    }

    /// <summary>
    /// Method to test endpoint PostResetPasswordAsync with user not found error
    /// </summary>
    [Fact(DisplayName = "Solicitação de reset de senha deverá retornar erro de usuário não encontrado")]
    public async Task FluentAssertions_PostResetPasswordAsync_POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNNDAsync()
    {
        // Arrange
        var requestBuilder = new PostResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription() });
        var notFoundResult = new NotFoundObjectResult(expectedResponse);

        mockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de usuário não encontrado. \n");
    }

    /// <summary>
    /// Method to test endpoint PostResetPasswordAsync with token found in cache error
    /// </summary>
    [Fact(DisplayName = "Solicitação de reset de senha deverá retornar erro de token já gerado")]
    public async Task FluentAssertions_PostResetPasswordAsync_POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHEAsync()
    {
        // Arrange
        var requestBuilder = new PostResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de token já gerado em cache. \n");
    }
}
