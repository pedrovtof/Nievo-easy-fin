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

public class PostCreateUserAsyncTest : UsersTestBase
{
    public PostCreateUserAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Criação de usuário deverá ser feita com sucesso")]
    public async Task PostCreateUserAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostCreateUserRequestBuilder();
        var okResult = new StatusCodeResult(201); // Or BuildOk if preferred, but service returns 201

        MockService.PostCreateUserAsync(Arg.Any<PostCreateUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(201);
        Output.WriteLine($"\n Validado sucesso com {request.Email} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_EMPTY_NULL, "Nome vazio ou nulo" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_WITH_WRONG_LENGHT, "Nome com tamanho errado" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL, "Senha vazia ou nula" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT, "Senha com tamanho errado" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT, "Senha com formato errado" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL, "Email vazio ou nulo" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS, "Email já existe" },
        new object[] { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_INVALID, "Email inválido" },
    };

    [Theory(DisplayName = "Criação de usuário deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostCreateUserAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostCreateUserRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostCreateUserAsync(Arg.Any<PostCreateUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostCreateUserAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Criação de usuário deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostCreateUserAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostCreateUserRequestBuilder();
        var okResult = new StatusCodeResult(201);

        MockService.PostCreateUserAsync(Arg.Any<PostCreateUserRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostCreateUserAsync(request);

        // Assert
        await MockService.Received(1).PostCreateUserAsync(Arg.Any<PostCreateUserRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
