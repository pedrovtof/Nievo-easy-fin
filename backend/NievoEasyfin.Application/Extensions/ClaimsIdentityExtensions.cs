using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NievoEasyfin.Application.Extensions.Claims
{
    /// <summary>
    /// Extension for Claims 
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// Add new property in claim
        /// </summary>
        /// <param name="value">this object</param>
        /// <param name="key">String key from dictionary</param>
        /// <param name="content">String value from dictionary</param>
        /// <returns>Claim</returns>
        public static Task<Claim> AddClaimToATokenAsync(this ClaimsIdentity value, string key, string content)
         => Task.FromResult(new Claim(key, content));
    }
}
