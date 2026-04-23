using System.Text.Json.Serialization;

namespace NievoEasyfin.Application.Data.Entities.Cache;

public class TokenPasswordResetEntity
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("pin_token")]
    public int PinToken { get; set; }
}
