using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PostValidateEmail
/// </summary>
public class PostValidateEmailRequest
{
    /// <summary>
    /// User email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// User token
    /// </summary>
    [JsonPropertyName("pin_token")]
    public int PinToken { get; set; }
}
