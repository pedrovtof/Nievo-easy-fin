using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using NievoEasyfin.Application.Interfaces.Response;
using System.Text.Json;
using NievoEasyfin.Application.Interfaces.Infrastructure;

namespace NievoEasyfin.Application.Infrastructure.Auth;

/// <summary>
/// Class model to validate the provider response
/// </summary>
public class SSoProviderAuth : SsoProviderEntity, ISSoProviderAuth
{
    private static AuthReplica? _AuthReplicaNodeDatabase;

    private static readonly string GOOGLE_API_USER_INFO = DotNetEnv.Env.GetString("GOOGLE_API_USER_INFO");

    private static readonly string GOOGLE_API_TOKEN_INFO = DotNetEnv.Env.GetString("GOOGLE_API_TOKEN_INFO");

    private static readonly string GOOGLE_ID_CLIENT = DotNetEnv.Env.GetString("GOOGLE_ID_CLIENT");

    public SSoProviderAuth(AuthReplica authReplicaNodeDatabase)
    {
        _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
    }

    /// <summary>
    /// Method to search in database the provider
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public async Task<SsoProviderEntity> GetProviderByNameAsync(string provider)
        => await _AuthReplicaNodeDatabase.SsoProvider.FirstOrDefaultAsync<SsoProviderEntity>(x => x.Name == provider && x.Active == true);

    /// <summary>
    /// Method to switch between provider types
    /// </summary>
    /// <param name="provider">SsoProviderEntity</param>
    /// <param name="token">token from sso</param>
    /// <returns>api response</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseProvider> ValidateProviderAsync(string provider, string? token = null)
    {
        if (token == null)
            throw new ArgumentException("The token for ValidateProviderAsync is necessary");

        ResponseProvider result;

        switch (provider)
        {
            case "google":
                result = await ProviderGoogleAsync(provider, token);
                break;

            case "github":
                result = await ProviderGithub(provider, token);
                break;

            default:
                throw new NotImplementedException($"Method Provider->{provider}<-Async didn't implement");
        }

        return result;
    }

    #region Google

    /// <summary>
    /// Method to validate token in google
    /// </summary>
    /// <param name="provider">SsoProviderEntity</param>
    /// <param name="token">Token from sso</param>
    /// <returns>response from api</returns>
    private async Task<ResponseProvider> ProviderGoogleAsync(string provider, string token)
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var validateProjectFromGoogle = await ProviderGoogleAsync(client, token);
        if (!validateProjectFromGoogle)
        {
            var error = new ResponseProvider();
            error.WithError("Invalid project from google or invalid token");
            return error;
        }

        var response = await client.GetAsync(GOOGLE_API_USER_INFO);

        if (!response.IsSuccessStatusCode)
        {
            var error = new ResponseProvider();
            error.WithError("Response was invalid from google");
            return error;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ResponseProvider>(responseContent);
    }

    /// <summary>
    /// Validate the project from GCP
    /// </summary>
    /// <param name="client">Http Client</param>
    /// <param name="token">Token sub gcp</param>
    /// <returns>true or false</returns>
    private async Task<bool> ProviderGoogleAsync(HttpClient client, string token)
    {
        var response = await client.GetAsync(GOOGLE_API_TOKEN_INFO);
        if (!response.IsSuccessStatusCode)
            return false;

        var responseData = JsonSerializer.Deserialize<ResponseProvider>(await response.Content.ReadAsStringAsync());

        if (responseData.Aud != GOOGLE_ID_CLIENT)
            return false;

        return true;
    }

    #endregion Google

    #region GitHub

    /// <summary>
    /// Method to validate token in github
    /// </summary>
    /// <param name="provider">SsoProviderEntity</param>
    /// <param name="token">Token from sso</param>
    /// <returns>response from api</returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ResponseProvider> ProviderGithub(string provider, string? token = null)
    {
        throw new NotImplementedException("Method not yeat implemented");
    }

    #endregion GitHub
}
