using NievoEasyfin.Application.Data.Entities;

namespace NievoEasyfin.Application.Interfaces.Models;

public interface IUserProviderSsoModel
{
    Task<UserProviderSsoEntity> CreateUserProviderSsoEntityAsync(int provider, int user, string sub);
    Task<UserProviderSsoEntity> GetUserProviderSsoBySubAndProviderAsync(string sub, int provider);
}
