namespace NievoEasyfin.Application.Interfaces.Response;

/// <summary>
/// Class template for API response with sucess
/// </summary>
public class ResponseApiSucess
{
    public bool Success { get; set; } = true;

    public object? Data { get; set; } = new { };

    /// <summary>
    /// Method to create the template
    /// </summary>
    /// <param name="data">any {}</param>
    public ResponseApiSucess(object data)
    {
        Data = data;
    }
}
