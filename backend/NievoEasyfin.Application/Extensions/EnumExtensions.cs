using System.ComponentModel;
using System.Reflection;

namespace NievoEasyfin.Application.Extensions.Enum;

/// <summary>
/// Extension for Enum
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Returns the [Description] attribute value of an enum member.
    /// Falls back to ToString() if no description is found.
    /// </summary>
    /// <param name="value">this object</param>
    /// <returns>Enum description</returns>
    public static string GetDescription(this System.Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field is null) return value.ToString();

        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}
