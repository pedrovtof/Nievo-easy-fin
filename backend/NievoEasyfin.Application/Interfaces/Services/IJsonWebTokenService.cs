namespace NievoEasyfin.Application.Interfaces.Services;

public interface IJsonWebTokenService
{
    Task<string> GenerateTokenAsync(string email);
}
