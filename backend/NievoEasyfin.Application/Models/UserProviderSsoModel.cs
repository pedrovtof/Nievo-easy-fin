using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using NievoEasyfin.Application.Interfaces.Models;

namespace NievoEasyfin.Application.Models;

/// <summary>
/// Class model to user_provider_sso
/// </summary>
public class UserProviderSsoModel : UserProviderSsoEntity, IUserProviderSsoModel
{
    private readonly AuthOrigin _AuthMainNodeDatabase;

    private readonly AuthReplica? _AuthReplicaNodeDatabase;

    public UserProviderSsoModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
    {
        _AuthMainNodeDatabase = authMainNodeDatabase;
        _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
    }

    public async Task<UserProviderSsoEntity> CreateUserProviderSsoEntityAsync(int provider, int user, string sub)
    {
        UserProviderSsoEntity userProvider = new UserProviderSsoEntity()
        {
            Sub = sub,
            UserId = user,
            SsoProviderId = provider,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _AuthMainNodeDatabase.UserProvider.AddAsync(userProvider);
        await _AuthMainNodeDatabase.SaveChangesAsync();

        return userProvider;
    }

    /// <summary>
    /// Search userProviderSso by Sub and Provider
    /// </summary>
    /// <param name="sub">Unique id</param>
    /// <param name="provider">provider Id</param>
    /// <returns>UserProviderSsoEntity</returns>
    public async Task<UserProviderSsoEntity> GetUserProviderSsoBySubAndProviderAsync(string sub, int provider)
        => await _AuthReplicaNodeDatabase.UserProvider.FirstOrDefaultAsync<UserProviderSsoEntity>(x => x.Sub == sub && x.SsoProviderId == provider);
}
