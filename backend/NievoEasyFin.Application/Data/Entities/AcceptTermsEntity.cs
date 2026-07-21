using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyFin.Application.Data.Entities;

[Table("accept_terms", Schema = "journey")]
public class AcceptTermsEntity
{
    [Key]
    [Column("id", TypeName = "SERIAL")]
    public int Id { get; set; }

    [Column("code", TypeName = "VARCHAR(50)")]
    public string Code { get; set; }

    [Column("name", TypeName = "VARCHAR(150)")]
    public string? Name { get; set; }

    [Column("description", TypeName = "VARCHAR(250)")]
    public string? Description { get; set; }

    [Column("version", TypeName = "INT")]
    public int Version { get; set; }

    [Column("content", TypeName = "TEXT")]
    public string? Content { get; set; }

    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; }

    [Column("active", TypeName = "BOOLEAN")]
    public bool? Active { get; set; }
}
