using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NievoEasyFin.Application.Data.Entities;

/// <summary>
/// Classe accounts.user_bank entity
/// </summary>
[Table("user_bank", Schema = "accounts")]
public class UserBankEntity
{
    [Key]
    [Column("id", TypeName = "SERIAL")]
    public int Id { get; set; }

    [Column("nick_name", TypeName = "VARCHAR(150)")]
    public string? NickName { get; set; }

    [Column("active", TypeName = "BOOLEAN")]
    public bool Active { get; set; }

    [Column("user_id", TypeName = "INT")]
    public int UserId { get; set; }

    [Column("bank_id", TypeName = "INT")]
    public int BankId { get; set; }

    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; }
}
