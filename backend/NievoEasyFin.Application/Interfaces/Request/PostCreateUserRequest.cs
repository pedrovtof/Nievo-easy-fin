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
}
