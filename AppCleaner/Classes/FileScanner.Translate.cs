using GTranslate.Translators;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace AppCleaner
{
    public partial class FileScanner
    {
        private readonly YandexTranslator _translator = new();
        private async Task TranslateEnglishInFolderAsync(CancellationToken cancellationToken)
        {
            var files = Directory
                .EnumerateFiles(_store.SearchFolder, "*.*", SearchOption.AllDirectories)
                .Where(file =>
                    file.EndsWith(".razor", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".html", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                .Where(file => !IsDesignerFile(file))
                .ToArray();
            _store.SetProgressMaximum(files.Length);
            AddToLog($"Файлов для перевода: {files.Length}");
            foreach (var file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    if (await TranslateFileAsync(file, cancellationToken))
                        AddToLog($"[Переведён] {file}");
                    else
                        AddToLog($"[Пропуск] {file}");
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка перевода] {file} - {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(file);
                }
            }
            AddToLog("Перевод завершён.");
        }
        private async Task<bool> TranslateFileAsync(string filePath, CancellationToken cancellationToken)
        {
            var encoding = DetectFileEncoding(filePath);
            var source = await File.ReadAllTextAsync(filePath, encoding, cancellationToken);
            var translated = source;
            translated = await TranslateTagTextAsync(translated, cancellationToken);
            translated = await TranslateRazorAttributesAsync(translated, cancellationToken);
            translated = await TranslateCSharpStringLiteralsAsync(translated, cancellationToken);
            translated = await TranslateSlashCommentsAsync(translated, cancellationToken);
            if (string.Equals(source, translated, StringComparison.Ordinal))
                return false;
            CreateBackup(filePath);
            await File.WriteAllTextAsync(filePath, translated, encoding, cancellationToken);
            return true;
        }
        private async Task<string> TranslateCSharpStringLiteralsAsync(string source, CancellationToken cancellationToken)
        {
            var regex = new Regex(
                @"(?<prefix>(?:ErrorMessage|Name|Message)\s*=\s*""|throw\s+new\s+\w+Exception\s*\(\s*""|message\s*=\s*"")(?<text>(?:\\""|[^""])*[A-Za-z](?:\\""|[^""])*)(?<end>"")",
                RegexOptions.Compiled);
            return await ReplaceMatchesAsync(source, regex, "text", cancellationToken);
        }
        private async Task<string> ReplaceMatchesAsync(string source, Regex regex, string groupName, CancellationToken cancellationToken)
        {
            var result = new StringBuilder();
            var lastIndex = 0;
            foreach (Match match in regex.Matches(source))
            {
                cancellationToken.ThrowIfCancellationRequested();
                result.Append(source, lastIndex, match.Index - lastIndex);
                var text = match.Groups[groupName].Value;
                if (ShouldTranslateText(text))
                {
                    var translated = await TranslateAsync(text.Trim(), cancellationToken);
                    result.Append(match.Value.Replace(text, translated));
                }
                else
                {
                    result.Append(match.Value);
                }
                lastIndex = match.Index + match.Length;
            }
            result.Append(source, lastIndex, source.Length - lastIndex);
            return result.ToString();
        }
        private async Task<string> TranslateRazorAttributesAsync(string source, CancellationToken cancellationToken)
        {
            var regex = new Regex(
                @"(?<name>\b(?:Text|LabelText|Caption|Title|Placeholder|NullText|ErrorMessage|Message)\s*=\s*"")(?<text>[^""@{}<>]*[A-Za-z][^""{}<>]*)(?<end>"")",
                RegexOptions.Compiled);
            return await ReplaceMatchesAsync(source, regex, "text", cancellationToken);
        }
        private async Task<string> TranslateTagTextAsync(string source, CancellationToken cancellationToken)
        {
            var regex = new Regex(
                @"(?<=>)(?<text>[^<>\r\n]+?)(?=<)",
                RegexOptions.Compiled);
            return await ReplaceMatchesAsync(source, regex, "text", cancellationToken);
        }
        private async Task<string> TranslateSlashCommentsAsync(string source, CancellationToken cancellationToken)
        {
            var regex = new Regex(
                @"(?<prefix>//\s*)(?<text>[A-Za-z][^\r\n]*)",
                RegexOptions.Compiled);
            var result = new StringBuilder();
            var lastIndex = 0;
            foreach (Match match in regex.Matches(source))
            {
                cancellationToken.ThrowIfCancellationRequested();
                result.Append(source, lastIndex, match.Index - lastIndex);
                var prefix = match.Groups["prefix"].Value;
                var text = match.Groups["text"].Value;
                if (ShouldTranslateText(text))
                {
                    var translated = await TranslateAsync(text.Trim(), cancellationToken);
                    result.Append(prefix);
                    result.Append(translated);
                }
                else
                {
                    result.Append(match.Value);
                }
                lastIndex = match.Index + match.Length;
            }
            result.Append(source, lastIndex, source.Length - lastIndex);
            return result.ToString();
        }
        private async Task<string> TranslateAsync(string phrase, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(phrase))
                return phrase;
            var result = await _translator.TranslateAsync(phrase, "ru");
            cancellationToken.ThrowIfCancellationRequested();
            return WebUtility.HtmlEncode(result.Translation);
        }
        private static bool ShouldTranslateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
            var value = text.Trim();
            if (value.Length < 2)
                return false;
            if (!Regex.IsMatch(value, @"[A-Za-z]"))
                return false;
            // НЕ переводим одиночные идентификаторы: options, Value, Password, Name
            if (Regex.IsMatch(value, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                return false;
            // НЕ переводим кодовые выражения
            if (value.Contains('@') ||
                value.Contains('{') ||
                value.Contains('}') ||
                value.Contains("=>") ||
                value.Contains("nameof", StringComparison.Ordinal))
                return false;
            // НЕ переводим member access: Model.Name, user.Email
            if (Regex.IsMatch(value, @"\b[A-Za-z_][A-Za-z0-9_]*\.[A-Za-z_][A-Za-z0-9_]*\b"))
                return false;
            // НЕ переводим urls / paths / routes
            if (value.Contains("://", StringComparison.Ordinal))
                return false;
            if (Regex.IsMatch(value, @"^[A-Za-z0-9_./:-]+$") &&
                !Regex.IsMatch(value, @"\s"))
                return false;
            // НЕ переводим имена свойств Grid / Blazor
            if (Regex.IsMatch(value,
                @"^(Date|TemperatureC|TemperatureF|Summary|FieldName|Value|Text|Caption|Name)$",
                RegexOptions.IgnoreCase))
                return false;
            // НЕ переводим route
            if (value.StartsWith("/") || value.StartsWith("~/"))
                return false;
            // НЕ переводим query параметры
            if (value.Contains('=') && !value.Contains(' '))
                return false;
            return true;
        }
    }
}
