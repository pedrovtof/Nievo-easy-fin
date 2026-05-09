using FluentAssertions;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Tests.Integration.Database;
using Xunit;

namespace NievoEasyfin.Tests.Integration.Database;

public class UserModelTest : DatabaseTestBase
{
    private readonly UserModel _userModel;

    public UserModelTest()
    {
        _userModel = new UserModel(DbOrigin, DbReplica);
    }

    [Fact(DisplayName = "Deve criar e recuperar um usuário pelo email")]
    public async Task CreateUserAsync_DevePersistirNoBanco()
    {
        // Arrange
        var email = "test@example.com";
        var name = "Test User";

        // Act
        await _userModel.CreateUserAsync(name, "hash", email);
        var user = await _userModel.GetUserByEmailAsync(email);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Name.Should().Be(name);
    }

    [Fact(DisplayName = "Deve retornar null para email inexistente")]
    public async Task GetUserByEmailAsync_Inexistente_RetornaNull()
    {
        // Act
        var user = await _userModel.GetUserByEmailAsync("nonexistent@example.com");

        // Assert
        user.Should().BeNull();
    }
}
