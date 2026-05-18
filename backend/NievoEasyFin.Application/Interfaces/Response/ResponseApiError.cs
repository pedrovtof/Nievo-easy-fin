namespace NievoEasyFin.Application.Interfaces.Response;

/// <summary>
/// Class template for API response with error
/// </summary>
public class ResponseApiError
{
    public bool Error { get; set; } = true;

    public List<string> Messages { get; set; } = new List<string>();

    public int Errors { get; set; } = 0;

    /// <summary>
    /// Method to create the template
    /// </summary>
    /// <param name="errors">list String</param>
    public ResponseApiError(List<string> errors)
    {
        Messages = errors;
        Errors = errors.Count;
    }
}
