using System.Security.Cryptography;
using System.Text;

namespace NievoEasyFin.Application.Services.Security;

/// <summary>
/// Service responsible for secure password hashing and validation using PBKDF2.
/// </summary>
public class CryptoPasswordService
{
    private static readonly int Iterations = DotNetEnv.Env.GetInt("PASSWORD_CRYPTO_ITERATIONS");

    private static readonly int KeySize = DotNetEnv.Env.GetInt("PASSWORD_CRYPTO_KEYSIZE");

    private static readonly byte[] Salt = Convert.FromHexString(DotNetEnv.Env.GetString("PASSWORD_CRYPTO_SALT"));

    private static HashAlgorithmName AlgorithmHash = HashAlgorithmName.SHA512;

    /// <summary>
    /// Initializes a new instance of the <see cref="CryptoPasswordService"/> class.
    /// </summary>
    public CryptoPasswordService()
    {

    }

    /// <summary>
    /// Generates a random cryptographic salt.
    /// </summary>
    /// <returns>A byte array containing the generated salt.</returns>
    private byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(KeySize);
    }

    /// <summary>
    /// Hashes a plain-text password using PBKDF2.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The hexadecimal string representation of the hashed password.</returns>
    /// <exception cref="ArgumentException">Thrown when the password is null or empty.</exception>
    public async Task<string> HashPasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("[NievoEasyFin.Application][CryptoPasswordHelper][HashPassword] Value password cannot be null or empty");

        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Salt, Iterations, AlgorithmHash, KeySize);

        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Validates a plain-text password against a stored hash.
    /// </summary>
    /// <param name="password">The plain-text password to validate.</param>
    /// <param name="hash">The stored hexadecimal hash to compare against.</param>
    /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
    public async Task<bool> HashValidateAsync(string password, string hash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            return false;

        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, Salt, Iterations, AlgorithmHash, KeySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}
