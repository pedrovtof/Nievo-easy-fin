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
        PayDate = entity.PayDate;
        Amount = entity.Amount;
        CreatedAt = entity.CreatedAt;
    }

    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

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
    /// Amount
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>
    /// Cron Tab
    /// </summary>
    [JsonPropertyName("cron_tab")]
    public string CronTab { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; }

    /// <summary>
    /// PayDate
    /// </summary>
    [JsonPropertyName("pay_date")]
    public DateTime PayDate { get; set; }

    /// <summary>
    /// UserCardName
    /// </summary>
    [JsonPropertyName("user_card_name")]
    public string UserCardName { get; set; }

    /// <summary>
    /// UserBankNickName
    /// </summary>
    [JsonPropertyName("user_bank_nickname")]
    public string UserBankNickName { get; set; }

    /// <summary>
    /// BankName
    /// </summary>
    [JsonPropertyName("bank_name")]
    public string BankName { get; set; }

    /// <summary>
    /// CreatedAt
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Records
    /// </summary>
    [JsonIgnore]
    public int Records { get; set; }
}
