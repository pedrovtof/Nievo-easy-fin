using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Interfaces.Response;

namespace NievoEasyfin.Application.Interfaces.Infrastructure;

public interface ISSoProviderAuth
{
    Task<SsoProviderEntity> GetProviderByNameAsync(string provider);
    Task<ResponseProvider> ValidateProviderAsync(string provider, string? token = null);
}
