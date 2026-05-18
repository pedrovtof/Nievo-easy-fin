using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostLoginUserSsoAsyncTest : AuthenticatorTestBase
{
    public PostLoginUserSsoAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Login SSO deverá ser feito com sucesso")]
    public async Task PostLoginUserSsoAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostLoginUserSsoRequestBuilder();
        var okResult = BuildOk(new { Token = "mocked-jwt-token" });

        MockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostLoginUserSsoAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado sucesso SSO com provedor {request.Provider} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED, "Provedor SSO não configurado" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKED, "Usuário bloqueado" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED, "Provedor não configurado" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE, "Provedor inativo" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE, "Resposta inválida do provedor" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY, "Provedor nulo ou vazio" },
        new object[] { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY, "Access token do provedor nulo ou vazio" },
    };

    [Theory(DisplayName = "Login SSO deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostLoginUserSsoAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostLoginUserSsoRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostLoginUserSsoAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Login SSO deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostLoginUserSsoAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostLoginUserSsoRequestBuilder();
        var okResult = BuildOk(new { Token = "mocked-jwt-token" });

        MockService.PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostLoginUserSsoAsync(request);

        // Assert — verifica que o service foi chamado exatamente 1 vez
        await MockService.Received(1).PostLoginUserSsoAsync(Arg.Any<PostLogiPostLoginUserSsoRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
