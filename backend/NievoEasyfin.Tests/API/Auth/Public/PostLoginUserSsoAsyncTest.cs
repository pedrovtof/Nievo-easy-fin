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

public class PostLoginUserSsoAsyncTest
{
    private readonly Faker _faker = new Faker("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;

    public PostLoginUserSsoAsyncTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with success
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá ser feito com sucesso")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_SucessAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiSucess(new { Token = "mocked-jwt-token" });
        var okResult = new OkObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        _testOutputHelper.WriteLine($"\n Validado sucesso SSO com provedor {requestBuilder.Provider} \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with provider SSO not configured error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de provedor SSO não configurado")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGUREDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de provedor SSO não configurado. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with user blocked error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de usuário bloqueado")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKEDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKED.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKED.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de usuário bloqueado no SSO. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with provider not configured error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de provedor não configurado")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGUREDAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de provedor não configurado. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with provider inactive error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de provedor inativo")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVEAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de provedor inativo. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with provider not 200 response error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de resposta inválida do provedor")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSEAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de resposta inválida do provedor. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with empty provider error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de provedor nulo ou vazio")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTYAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithProvider(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de provedor nulo ou vazio. \n");
    }

    /// <summary>
    /// Method to test endpoint PostLoginUserSsoAsync with empty provider access token error
    /// </summary>
    [Fact(DisplayName = "Login SSO deverá retornar erro de access token do provedor nulo ou vazio")]
    public async Task FluentAssertions_PostLoginUserSsoAsync_POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTYAsync()
    {
        // Arrange
        var requestBuilder = new PostLoginUserSsoRequestBuilder();
        requestBuilder.Default();
        requestBuilder.WithProviderAccessToken(string.Empty);

        var mockService = Substitute.For<IAuthenticatorService>();
        var expectedResponse = new ResponseApiError(new List<string> { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY.GetDescription() });
        var badRequestResult = new BadRequestObjectResult(expectedResponse);

        mockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var controller = new AuthenticatorController(mockService);

        // Act
        var result = await controller.PostLoginUserSsoAsync(requestBuilder);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY.GetDescription());
        _testOutputHelper.WriteLine($"\n Validado erro de access token do provedor nulo ou vazio. \n");
    }
}
