using System.Text;
using Bogus;

namespace NievoEasyfin.Tests.Build.Generators;

/// <summary>
/// Utility class to generate random passwords that meet validation requirements.
/// Extracted to avoid duplication across request builders.
/// </summary>
public static class PasswordGenerator
{
    private static readonly Faker Faker = new Faker("pt_BR");

    private static readonly List<string> Symbols = new()
    {
        "!", "@", "#", "$", "%",
        "^", "&", "*", "(", ")",
        "+", "=", "/", "[", "]",
        "{", "}", "\\", "`", "~",
        "<", ">", ",", "."
    };

    /// <summary>
    /// Generates a random password containing uppercase, lowercase, numeric, and special characters.
    /// </summary>
    /// <returns>A random password string</returns>
    public static string Generate()
    {
        var str = new StringBuilder();

        str.Append(Faker.Hacker.Random.AlphaNumeric(3).ToUpper());
        str.Append(Faker.Random.AlphaNumeric(3));
        str.Append(Faker.Random.Number(3).ToString());
        str.Append(string.Join("", Faker.PickRandom(Symbols, Faker.Random.Number(1, 3))));

        return str.ToString();
    }
}
