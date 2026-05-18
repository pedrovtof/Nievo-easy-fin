using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NievoEasyFin.Application.Configuration;
using System.Security.Claims;
using NievoEasyFin.Application.Extensions.Claims;

namespace NievoEasyFin.Application.Services.Security;

/// <summary>
/// Service responsible for generating and managing JSON Web Tokens (JWT).
/// Class service to JWT
/// </summary>
public class JsonWebTokenService
{
    private static JsonWebTokenConfiguration _jsonWebTokenConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonWebTokenService"/> class.
    /// </summary>
    /// <param name="jsonWebTokenConfiguration">The configuration settings for JWT generation.</param>
    public JsonWebTokenService(JsonWebTokenConfiguration jsonWebTokenConfiguration)
    {
        _jsonWebTokenConfiguration = jsonWebTokenConfiguration;
    }

    /// <summary>
    /// Generates a JWT token for the specified user email.
    /// </summary>
    /// <param name="email">The email of the user for whom the token is being generated.</param>
    /// <returns>A signed JWT token string.</returns>
    /// Method to generate token jwt
    /// </summary>
    /// <param name="email">email of the user</param>
    /// <returns>token jwt</returns>
    public async Task<string> GenerateTokenAsync(string email)
    {
        var handler = new JwtSecurityTokenHandler();

        var credential = new SigningCredentials(
            await _jsonWebTokenConfiguration.GetSymmetricSecurityKey(),
            await _jsonWebTokenConfiguration.GetAlgorithmTokenSignature()
        );

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = await ClaimsIdentityTokenAsync(
                email
            ),
            SigningCredentials = credential,
            Expires = DateTime.UtcNow.AddHours(12)
        };

        var token = handler.CreateToken(tokenDescription);

        return handler.WriteToken(token);
    }

    /// <summary>
    /// Creates a <see cref="ClaimsIdentity"/> for the user token based on their email.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task representing the asynchronous operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
    /// Method to create subject of the jwt
    /// </summary>
    /// <param name="email">user email</param>
    /// <returns>ClaimsIdentity</returns>
    private async Task<ClaimsIdentity> ClaimsIdentityTokenAsync(string email)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(await ci.AddClaimToATokenAsync("Email", email));

        return ci;
    }
}
