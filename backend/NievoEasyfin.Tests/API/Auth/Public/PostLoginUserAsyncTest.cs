using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Tests.Build.Request;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.Auth.Public;

public class PostLoginUserAsyncTest : AuthenticatorTestBase
{
    public PostLoginUserAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Login deverá ser feito com sucesso")]
    public async Task PostLoginUserAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var okResult = BuildOk(new { Token = "mocked-jwt-token" });

        MockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostLoginUserAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado sucesso com {request.Email} e {request.Password} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL, "Email vazio ou nulo" },
        new object[] { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL, "Senha vazia ou nula" },
        new object[] { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD, "Senha incorreta" },
    };

    [Theory(DisplayName = "Login deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostLoginUserAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostLoginUserAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region NotFound Errors

    public static IEnumerable<object[]> NotFoundErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
        new object[] { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED, "Usuário bloqueado" },
    };

    [Theory(DisplayName = "Login deverá retornar NotFound para cenários de erro")]
    [MemberData(nameof(NotFoundErrors))]
    public async Task PostLoginUserAsync_CenarioDeErro_RetornaNotFound(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var notFoundResult = BuildNotFound(
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND,
            EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED
        );

        MockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        // Act
        var result = await Controller.PostLoginUserAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Login deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostLoginUserAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var okResult = BuildOk(new { Token = "mocked-jwt-token" });

        MockService.PostLoginUserAsync(Arg.Any<PostLoginUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostLoginUserAsync(request);

        // Assert — verifica que o service foi chamado exatamente 1 vez
        await MockService.Received(1).PostLoginUserAsync(Arg.Any<PostLoginUserRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
