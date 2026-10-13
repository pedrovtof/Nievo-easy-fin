using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for Payment Transaction
/// </summary>
public class TransactionView
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public TransactionView() { }

    /// <summary>
    /// Constructor from entity
    /// </summary>
    /// <param name="entity">TransactionEntity</param>
    public TransactionView(TransactionEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        UserId = entity.UserId;
        TransactionStatusId = entity.TransactionStatusId;
        CategorieId = entity.CategorieId;
        PayDate = entity.PayDate;
        Amount = entity.Amount;
        CardId = entity.CardId;
        BankId = entity.BankId;
        TransactionTypeId = entity.TransactionTypeId;
        SchedulerId = entity.SchedulerId;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// User id
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Transaction status id
    /// </summary>
    [JsonPropertyName("transaction_status_id")]
    public int TransactionStatusId { get; set; }

    /// <summary>
    /// Transaction status name
    /// </summary>
    [JsonPropertyName("transaction_status_name")]
    public string? TransactionStatusName { get; set; }

    /// <summary>
    /// Category id
    /// </summary>
    [JsonPropertyName("categorie_id")]
    public int? CategorieId { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    [JsonPropertyName("category_name")]
    public string? CategoryName { get; set; }

    /// <summary>
    /// Pay date
    /// </summary>
    [JsonPropertyName("pay_date")]
    public DateTime PayDate { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>
    /// Card id
    /// </summary>
    [JsonPropertyName("card_id")]
    public int? CardId { get; set; }

    /// <summary>
    /// Card name
    /// </summary>
    [JsonPropertyName("card_name")]
    public string? CardName { get; set; }

    /// <summary>
    /// Bank id
    /// </summary>
    [JsonPropertyName("bank_id")]
    public int? BankId { get; set; }

    /// <summary>
    /// Bank name
    /// </summary>
    [JsonPropertyName("bank_name")]
    public string? BankName { get; set; }

    /// <summary>
    /// Transaction type id
    /// </summary>
    [JsonPropertyName("transaction_type_id")]
    public int TransactionTypeId { get; set; }

    /// <summary>
    /// Transaction type name
    /// </summary>
    [JsonPropertyName("transaction_type_name")]
    public string? TransactionTypeName { get; set; }

    /// <summary>
    /// Scheduler id
    /// </summary>
    [JsonPropertyName("scheduler_id")]
    public int? SchedulerId { get; set; }

    /// <summary>
    /// Scheduler name
    /// </summary>
    [JsonPropertyName("scheduler_name")]
    public string? SchedulerName { get; set; }

    /// <summary>
    /// Created at
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Updated at
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Records
    /// </summary>
    [JsonIgnore]
    public int Records { get; set; }
}
