using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PostLoginUserAsync
/// </summary>
public class PostLoginUserRequest
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
