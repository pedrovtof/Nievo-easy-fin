using System.Text.RegularExpressions;

namespace NievoEasyFin.Application.Helper;

/// <summary>
/// Class to help validate the password
/// </summary>
public class PasswordValidatorHelper
{
    private static Regex REGEX_RULES = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,12}$");

    public PasswordValidatorHelper()
    {

    }

    /// <summary>
    /// Regex method for password
    /// Has minimum 6 characters to max 12 in length. Adjust it by modifying {6,12}
    /// At least one uppercase English letter. You can remove this condition by removing (?=.*?[A-Z])
    /// At least one lowercase English letter.  You can remove this condition by removing (?=.*?[a-z])
    /// At least one digit. You can remove this condition by removing (?=.*?[0-9])
    /// At least one special character,  You can remove this condition by removing (?=.*?[#?!@$%^*-])
    /// </summary>
    /// <param name="password">String senha para ser validado</param>
    /// <returns></returns>
    internal bool ValidatePasswordRegex(string password)
    {
        return REGEX_RULES.IsMatch(password);
    }
}
