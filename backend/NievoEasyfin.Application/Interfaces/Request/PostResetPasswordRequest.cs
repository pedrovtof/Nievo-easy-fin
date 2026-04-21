using System.Text.Json.Serialization;

namespace NievoEasyfin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PostResetPassword
/// </summary>
public class PostResetPasswordRequest
{
    /// <summary>
    /// User email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}
