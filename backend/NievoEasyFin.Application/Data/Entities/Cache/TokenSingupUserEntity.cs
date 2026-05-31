using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities.Cache;

public class TokenSingupUserEntity
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("pin_token")]
    public int PinToken { get; set; }
}
