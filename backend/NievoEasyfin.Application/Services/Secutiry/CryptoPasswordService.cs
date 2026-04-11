using System.Security.Cryptography;
using System.Text;

namespace NievoEasyfin.Application.Services.Security
{
    /// <summary>
    /// Class model for Crypto password
    /// </summary>
    public class CryptoPasswordService
    {
        private static readonly int Iterations = DotNetEnv.Env.GetInt("PASSWORD_CRYPTO_ITERATIONS");

        private static readonly int KeySize = DotNetEnv.Env.GetInt("PASSWORD_CRYPTO_KEYSIZE");

        private static readonly byte[] Salt = Convert.FromHexString(DotNetEnv.Env.GetString("PASSWORD_CRYPTO_SALT"));

        private static HashAlgorithmName AlgorithmHash = HashAlgorithmName.SHA512;

        /// <summary>
        /// Service constructor
        /// </summary>
        public CryptoPasswordService()
        {

        }

        /// <summary>
        /// Method to create salt
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(KeySize);
        }

        /// <summary>
        /// Method to Hash password user
        /// </summary>
        /// <param name="password">password from request</param>
        /// <returns>Hash string</returns>
        /// <exception cref="ArgumentException">Invalid input</exception>
        public async Task<string> HashPasswordAsync(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("[NievoEasyFin.Application][CryptoPasswordHelper][HashPassword] Value password cannot be null or empty");

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Salt, Iterations, AlgorithmHash, KeySize);

            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Method for validate the password.
        /// </summary>
        /// <param name="password">Input from request</param>
        /// <param name="hash">value in database</param>
        /// <returns>True/False</returns>
        public async Task<bool> HashValidateAsync(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Salt, Iterations, AlgorithmHash, KeySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
