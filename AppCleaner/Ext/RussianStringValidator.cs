using System.Text.RegularExpressions;
namespace Extensions
{
    public static class RussianStringValidator
    {
        // Компилируем regex для повторного использования
        private static readonly Regex CyrillicWithSpaces = new Regex(@"^[\p{IsCyrillic}\s]+$", RegexOptions.Compiled);
        private static readonly Regex CyrillicOnly = new Regex(@"^[\p{IsCyrillic}]+$", RegexOptions.Compiled);
        /// <summary>
        /// Возвращает true, если строка содержит только кириллические буквы (и по желанию пробелы),
        /// и при этом не является однострочным комментарием, начинающимся с "//".
        /// </summary>
        /// <param name="line">Исходная строка (может быть null).</param>
        /// <param name="allowSpaces">Разрешать пробелы внутри строки (по умолчанию true).</param>
        /// <param name="treatCommentAsNonCyrillic">Если true (по умолчанию), строки, начинающиеся с "//", считаются невалидными.</param>
        public static bool IsOnlyCyrillic(this string? line, bool allowSpaces = true, bool treatCommentAsNonCyrillic = true)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;
            string trimmed = line.Trim();
            if (treatCommentAsNonCyrillic && trimmed.StartsWith("//", StringComparison.Ordinal))
                return false;
            return (allowSpaces ? CyrillicWithSpaces : CyrillicOnly).IsMatch(trimmed);
        }
    }
}
