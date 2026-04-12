using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NievoEasyfin.Application.Configuration;
using System.Security.Claims;
using NievoEasyfin.Application.Extensions.Claims;

namespace NievoEasyfin.Application.Services.Security
{
    /// <summary>
    /// Class service to JWT
    /// </summary>
    public class JsonWebTokenService
    {
        private static JsonWebTokenConfiguration _jsonWebTokenConfiguration;

        public JsonWebTokenService(JsonWebTokenConfiguration jsonWebTokenConfiguration)
        {
            _jsonWebTokenConfiguration = jsonWebTokenConfiguration;
        }

        /// <summary>
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
}
