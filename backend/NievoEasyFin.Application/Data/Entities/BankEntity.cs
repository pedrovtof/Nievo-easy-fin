using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyFin.Application.Data.Entities;

[Table("bank", Schema = "accounts")]
public class BankEntity
{
    [Key]
    [Column("id", TypeName = "SERIAL")]
    public int Id { get; set; }

    [Column("name", TypeName = "VARCHAR(150)")]
    public string? Name { get; set; }

    [Column("bank_type", TypeName = "INT")]
    public int? BankType { get; set; }

    [Column("active", TypeName = "BOOLEAN")]
    public bool Active { get; set; }

    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; }
}