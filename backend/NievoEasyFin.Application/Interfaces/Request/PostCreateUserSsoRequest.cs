using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PostCreateUser
/// </summary>
public class PostCreateUserSsoRequest
{
    /// <summary>
    /// Name of the provider (journey.sso_provider)
    /// </summary>
    [JsonPropertyName("provider_name")]
    public string Provider { get; set; }

    /// <summary>
    /// User Id from provider
    /// </summary>
    [JsonPropertyName("provider_access_token")]
    public string ProviderAccessToken { get; set; }

    /// <summary>
    /// User Email
    /// </summary>
    [JsonPropertyName("accept_terms")]
    public bool AcceptTerms { get; set; }

    /// <summary>
    /// Header com agent do usuário
    /// </summary>
    private string UserAgent { get; set; }

    /// <summary>
    /// Header com Host
    /// </summary>
    private string Host { get; set; }

    /// <summary>
    /// Setter Host
    /// </summary>
    /// <param name="host">string</param>
    public void SetHost(string host)
    {
        Host = host;
    }

    /// <summary>
    /// Getter Host
    /// </summary>
    /// <returns>Host</returns>
    public string GetHost()
    {
        return Host;
    }

    /// <summary>
    /// Setter User Agent
    /// </summary>
    /// <param name="userAgent">string</param>
    public void SetUserAgent(string userAgent)
    {
        UserAgent = userAgent;
    }

    /// <summary>
    /// Getter User Agent
    /// </summary>
    /// <returns>UserAgent</returns>
    public string GetUserAgent()
    {
        return UserAgent;
    }
}
