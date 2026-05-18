using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Cache.Views;

public class TokenPasswordResetView
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("pin_token")]
    public int PinToken { get; set; }
}
