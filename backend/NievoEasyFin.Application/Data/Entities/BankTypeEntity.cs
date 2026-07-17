using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities;

/// <summary>
/// Bank Type entity
/// </summary>
[Table("bank_type", Schema = "accounts")]
public class BankTypeEntity
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
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    [Column("description", TypeName = "VARCHAR(255)")]
    public string Description { get; set; }

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
