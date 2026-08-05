using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities;

/// <summary>
/// Class data bank
/// </summary>
[Table("bank", Schema = "accounts")]
public class BankEntity
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    [Key]
    [Column("id", TypeName = "SERIAL")]
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    [Column("name", TypeName = "VARCHAR(150)")]
    public string Name { get; set; }

    /// <summary>
    /// Bank type
    /// </summary>
    [JsonPropertyName("bank_type")]
    [Column("bank_type", TypeName = "INT")]
    public int BankType { get; set; }

    /// <summary>
    /// Active
    /// </summary>
    [JsonPropertyName("active")]
    [Column("active", TypeName = "BOOLEAN")]
    public bool Active { get; set; }

    /// <summary>
    /// Created at
    /// </summary>
    [JsonPropertyName("created_at")]
    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Updated at
    /// </summary>
    [JsonPropertyName("updated_at")]
    [Column("updated_at", TypeName = "TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; }
}
