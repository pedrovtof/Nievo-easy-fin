using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PostCreateUser
/// </summary>
public class PostCreateUserRequest
{
    /// <summary>
    /// User name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// User password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }

    /// <summary>
    /// User Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

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
