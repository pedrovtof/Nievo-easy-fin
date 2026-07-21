using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for Banks
/// </summary>
public class BanksViews
{
    /// <summary>
    /// Constructor base
    /// </summary>
    public BanksViews() { }

    /// <summary>
    /// Number of itens
    /// </summary>
    [JsonPropertyName("records")]
    public int Records { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Bank active
    /// </summary>
    [JsonPropertyName("bank_active")]
    public bool BankActive { get; set; }

    /// <summary>
    /// Bank type
    /// </summary>
    [JsonPropertyName("bank_type")]
    public int BankType { get; set; }

    /// <summary>
    /// Active
    /// </summary>
    [JsonPropertyName("bank_type_active")]
    public bool BankTypeActive { get; set; }

    /// <summary>
    /// Bank type name
    /// </summary>
    [JsonPropertyName("bank_type_name")]
    public string BankTypeName { get; set; }

    /// <summary>
    /// Bank type description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
