using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for TransactionStatus
/// </summary>
public class TransactionSituationView
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public TransactionSituationView() { }

    /// <summary>
    /// Constructor from entity
    /// </summary>
    /// <param name="entity">TransactionSituationEntity</param>
    public TransactionSituationView(TransactionSituationEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        Active = entity.Active;
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
    /// Active
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Records
    /// </summary>
    [JsonIgnore]
    public int Records { get; set; }
}
