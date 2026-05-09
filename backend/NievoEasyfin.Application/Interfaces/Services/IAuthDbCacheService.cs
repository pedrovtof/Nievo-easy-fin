using NievoEasyfin.Application.Data.Cache.Views;
using NievoEasyfin.Application.Data.Entities.Cache;

namespace NievoEasyfin.Application.Interfaces.Services;

public interface IAuthDbCacheService
{
    Task<TokenPasswordResetView?> GetTokenPasswordResetAttempByUserIdAsync(int userId);
    Task<TokenPasswordResetEntity> CreateTokenPasswordResetAttempAsync(int userId, string email);
    Task<bool> ValidateTokenAsync(int requestToken, int cachetoken);
}
