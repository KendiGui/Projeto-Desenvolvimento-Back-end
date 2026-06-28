using System.Text.RegularExpressions;

namespace Domain.Helpers
{
    public static class EnumExtensions
    {
        public static string ToApiString(this Enum value)
        {
            var name = value.ToString();
            return Regex.Replace(name, "([a-z0-9])([A-Z])", "$1_$2").ToUpperInvariant();
        }

        public static bool TryParseApi<TEnum>(string? texto, out TEnum value) where TEnum : struct, Enum
        {
            value = default;
            if (string.IsNullOrWhiteSpace(texto)) return false;

            var normalizado = texto.Replace("_", string.Empty).Trim();
            return Enum.TryParse(normalizado, ignoreCase: true, out value)
                && Enum.IsDefined(value);
        }
    }
}
