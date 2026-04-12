using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NievoEasyfin.Application.Configuration
{
    /// <summary>
    /// Class to create JWT configuration
    /// </summary>
    public class JsonWebTokenConfiguration
    {
        public static string PrivateKey { get; } = DotNetEnv.Env.GetString("JWT_PRIVATE_CONTRACT_STRING");

        public static string PublicteKey { get; } = DotNetEnv.Env.GetString("JWT_PUBLIC_CONTRACT_STRING");

        private const string AlgorithmToken = SecurityAlgorithms.HmacSha256Signature;

        /// <summary>
        /// Method to returns Symmetric Security Key
        /// </summary>
        /// <returns>SymmetricSecurityKey</returns>
        public async Task<SymmetricSecurityKey> GetSymmetricSecurityKey()
            => new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(PrivateKey)
            );

        /// <summary>
        /// Method to returns Algorithm Token Signature
        /// </summary>
        /// <returns>string</returns>
        public async Task<string> GetAlgorithmTokenSignature()
            => AlgorithmToken;
    }
}
