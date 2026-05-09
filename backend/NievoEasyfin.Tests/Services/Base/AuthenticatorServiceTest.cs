using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Tests.Build.Request;
using NievoEasyfin.Tests.Services.Base;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.Services.Base;

public class AuthenticatorServiceTest : ServiceTestBase
{
    public AuthenticatorServiceTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "Login deve retornar sucesso quando dados são válidos")]
    public async Task PostLoginUserAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var user = new UserEntity { Email = request.Email, Password = "hashed_password" };
        
        MockUserModel.GetUserByEmailAsync(request.Email).Returns(user);
        MockCrypto.HashValidateAsync(request.Password, user.Password).Returns(true);
        MockJwt.GenerateTokenAsync(user.Email).Returns("fake-jwt-token");

        // Act
        var result = await AuthenticatorService.PostLoginUserAsync(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;
        
        var loginResponse = response.Data.Should().BeOfType<PostLoginUserResponse>().Subject;
        loginResponse.Token.Should().Be("fake-jwt-token");
        
        Output.WriteLine($"Login realizado com sucesso para {request.Email}");
    }

    [Fact(DisplayName = "Login deve retornar NotFound quando usuário não existe")]
    public async Task PostLoginUserAsync_UsuarioNaoExiste_RetornaNotFound()
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        MockUserModel.GetUserByEmailAsync(request.Email).Returns((UserEntity)null);

        // Act
        var result = await AuthenticatorService.PostLoginUserAsync(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        Output.WriteLine("Validado que usuário inexistente retorna 404");
    }

    [Fact(DisplayName = "Login deve retornar BadRequest quando senha está incorreta")]
    public async Task PostLoginUserAsync_SenhaIncorreta_RetornaBadRequest()
    {
        // Arrange
        var request = new PostLoginUserRequestBuilder();
        var user = new UserEntity { Email = request.Email, Password = "hashed_password" };
        
        MockUserModel.GetUserByEmailAsync(request.Email).Returns(user);
        MockCrypto.HashValidateAsync(request.Password, user.Password).Returns(false);

        // Act
        var result = await AuthenticatorService.PostLoginUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        Output.WriteLine("Validado que senha incorreta retorna 400");
    }
}
