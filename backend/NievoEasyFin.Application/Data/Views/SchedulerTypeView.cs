using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for SchedulerType
/// </summary>
public class SchedulerTypeView
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public SchedulerTypeView() { }

    /// <summary>
    /// Constructor from entity
    /// </summary>
    /// <param name="entity">SchedulerTypeEntity</param>
    public SchedulerTypeView(SchedulerTypeEntity entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Description = entity.Description;
        CronTab = entity.CronTab;
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
    /// Cron tab
    /// </summary>
    [JsonPropertyName("cron_tab")]
    public string? CronTab { get; set; }

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
