using System.Text.RegularExpressions;
namespace AppCleaner;

public static class StringHelpers
{
    // Компилированный regex для производительности
    private static readonly Regex CollapseSemicolonsRegex =
        new Regex(@";(?:\s*;)+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    /// <summary>
    /// Заменяет последовательности типа ";;" или ";   ;" (и более длинные) на один ";".
    /// </summary>
    public static string CollapseSemicolons(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return CollapseSemicolonsRegex.Replace(input, ";");
    }
    /// <summary>
    /// Расширение для string.
    /// </summary>
    public static string CollapseSemicolonsExt(this string input) => CollapseSemicolons(input);
}
