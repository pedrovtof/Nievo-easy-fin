using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request;

/// <summary>
/// This class is a template for PatchResetPassword
/// </summary>
public class PatchResetPasswordRequest
{
    /// <summary>
    /// User email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    /// Token to reset
    /// </summary>
    [JsonPropertyName("pin_token")]
    public string PinToken { get; set; }

    /// <summary>
    /// New Password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
