namespace NievoEasyfin.Application.Interfaces.Response;

public class PostLoginUserResponse
{
    public string Token { get; set; }

    public PostLoginUserResponse(string token)
    {
        Token = token;
    }
}
