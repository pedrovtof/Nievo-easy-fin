using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities;

/// <summary>
/// Class data CategoryEntity
/// </summary>
[Table("category", Schema = "goals")]
public class CategoryEntity
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
    /// UserId
    /// </summary>
    [JsonPropertyName("user_id")]
    [Column("user_id", TypeName = "INTEGER")]
    public int? UserId { get; set; }

    /// <summary>
    /// GoalsId
    /// </summary>
    [JsonPropertyName("goal_id")]
    [Column("goal_id", TypeName = "INTEGER")]
    public int? GoalId { get; set; }

    /// <summary>
    /// ParrentCategory
    /// </summary>
    [JsonPropertyName("parrent_category")]
    [Column("parrent_category", TypeName = "INTEGER")]
    public int? ParrentCategory { get; set; }

    /// <summary>
    /// CreatedAt
    /// </summary>
    [JsonPropertyName("created_at")]
    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UpdatedAt
    /// </summary>
    [JsonPropertyName("updated_at")]
    [Column("updated_at", TypeName = "TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; }
}
