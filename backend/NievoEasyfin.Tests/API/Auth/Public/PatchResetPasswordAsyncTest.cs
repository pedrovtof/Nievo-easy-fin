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

public class PatchResetPasswordAsyncTest
{
    private readonly Faker _faker = new Faker("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;

    public PatchResetPasswordAsyncTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with success
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá ser feito com sucesso")]
    public async Task FluentAssertions_PatchResetPasswordAsync_SucessAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiSucess(new { Message = EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_200_PASSWORD_CHANGED.GetDescription() });
        var okResult = new OkObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        _testOutputHelper.WriteLine($"\n Validado sucesso ao resetar a senha do usuário com email {requestBuilder.Email} \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with invalid email error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de email inválido")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAILAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithEmail("invalid-email");

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de email inválido. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with empty email error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de email nulo ou vazio")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTYAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithEmail(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de email nulo ou vazio. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with user not found error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de usuário não encontrado")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNNDAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription() });
        var notFoundResult = new NotFoundObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de usuário não encontrado. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with token not found in cache error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de token não encontrado no cache")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHEAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHE.GetDescription() });
        var notFoundResult = new NotFoundObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHE.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de token não encontrado no cache. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with invalid token error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de token inválido")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALIDAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithPinToken("invalid-token");

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALID.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALID.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de token inválido. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with empty password error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de senha nula ou vazia")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULLAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithPassword(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de senha nula ou vazia. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with password wrong length error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de tamanho incorreto da senha")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHTAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithPassword("123");

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de tamanho incorreto da senha. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with password wrong format error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de formato incorreto da senha")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMATAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithPassword("senhasemformato");

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de formato incorreto da senha. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with password is the same error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro de mesma senha")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAMEAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAME.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAME.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de nova senha ser igual à atual. \n");
    }

    /// <summary>
    /// Method to test endpoint PatchResetPasswordAsync with password not updated error
    /// </summary>
    [Fact(DisplayName = "Reset de senha deverá retornar erro interno de não atualização")]
    public async Task FluentAssertions_PatchResetPasswordAsync_PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATEDAsync()
    {
        // Arrange
        var requestBuilder = new PatchResetPasswordRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATED.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PatchResetPasswordAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATED.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de falha ao atualizar a senha no sistema. \n");
    }
}
