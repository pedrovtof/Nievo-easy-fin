using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Entities;

/// <summary>
/// Transaction entity
/// </summary>
[Table("transaction", Schema = "payment")]
public class TransactionEntity
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
    public string? Description { get; set; }

    /// <summary>
    /// User id
    /// </summary>
    [JsonPropertyName("user_id")]
    [Column("user_id", TypeName = "INT")]
    public int UserId { get; set; }

    /// <summary>
    /// Transaction status id
    /// </summary>
    [JsonPropertyName("transaction_status_id")]
    [Column("transaction_status_id", TypeName = "INT")]
    public int TransactionStatusId { get; set; }

    /// <summary>
    /// Category id
    /// </summary>
    [JsonPropertyName("categorie_id")]
    [Column("categorie_id", TypeName = "INT")]
    public int? CategorieId { get; set; }

    /// <summary>
    /// Pay date
    /// </summary>
    [JsonPropertyName("pay_date")]
    [Column("pay_date", TypeName = "TIMESTAMP")]
    public DateTime PayDate { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [JsonPropertyName("amount")]
    [Column("amount", TypeName = "BIGINT")]
    public long Amount { get; set; }

    /// <summary>
    /// Card id
    /// </summary>
    [JsonPropertyName("card_id")]
    [Column("card_id", TypeName = "INT")]
    public int? CardId { get; set; }

    /// <summary>
    /// Bank id
    /// </summary>
    [JsonPropertyName("bank_id")]
    [Column("bank_id", TypeName = "INT")]
    public int? BankId { get; set; }

    /// <summary>
    /// Transaction type id
    /// </summary>
    [JsonPropertyName("transaction_type_id")]
    [Column("transaction_type_id", TypeName = "INT")]
    public int TransactionTypeId { get; set; }

    /// <summary>
    /// Scheduler id
    /// </summary>
    [JsonPropertyName("scheduler_id")]
    [Column("scheduler_id", TypeName = "INT")]
    public int? SchedulerId { get; set; }

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
