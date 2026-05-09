namespace NievoEasyfin.Application.Interfaces.Services;

public interface ICryptoPasswordService
{
    Task<string> HashPasswordAsync(string password);
    Task<bool> HashValidateAsync(string password, string hash);
}
