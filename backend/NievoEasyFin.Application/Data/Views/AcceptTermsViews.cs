using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views;

/// <summary>
/// View for accept Terms entity
/// </summary>
public class AcceptTermsViews
{
    /// <summary>
    /// Constructor base
    /// </summary>
    /// <param name="entity">AcceptTermsEntity</param>
    public AcceptTermsViews(AcceptTermsEntity entity)
    {
        Content = entity.Content;
        Version = entity.Version;
    }

    /// <summary>
    /// Content of accept terms 
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Version of accept terms
    /// </summary>
    public int Version { get; set; }
}
