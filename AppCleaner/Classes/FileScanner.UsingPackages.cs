using System.Text.RegularExpressions;
namespace AppCleaner;

public partial class FileScanner
{
    private void CollectRequiredPackagesFromUsings(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var files = Directory
            .EnumerateFiles(_store.SearchFolder, "*.cs", SearchOption.AllDirectories)
            .Where(file => !IsIgnoredScanPath(file))
            .ToArray();
        _store.SetProgressMaximum(files.Length);
        var packages = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                foreach (var packageName in ExtractUsingPackages(file))
                    packages.Add(packageName);
            }
            catch (Exception ex)
            {
                AddToLog($"[Ошибка] {file} - {ex.Message}");
            }
            finally
            {
                CountProcessedFile(file);
            }
        }
        AddToLog("Итог по операции: найдены пакеты/usings:");
        foreach (var package in packages)
            AddToLog(package);
        AddToLog($"Всего: {packages.Count}");
    }
    private static IEnumerable<string> ExtractUsingPackages(string filePath)
    {
        var text = File.ReadAllText(filePath);
        var matches = Regex.Matches(
            text,
            @"^\s*using\s+(?!static\s)(?:[\w]+\s*=\s*)?(?<name>[A-Za-z_][\w]*(?:\.[A-Za-z_][\w]*)*)\s*;",
            RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            var usingName = match.Groups["name"].Value.Trim();
            if (string.IsNullOrWhiteSpace(usingName))
                continue;
            yield return NormalizePackageName(usingName);
        }
    }
    private static string NormalizePackageName(string usingName)
    {
        // GH.Components -> GH.Components
        // GH.Configs -> GH.Configs
        // System.Collections.Generic -> System.Collections
        // System.Windows.Forms -> System.Windows.Forms
        var parts = usingName.Split('.');
        if (parts.Length <= 2)
            return usingName;
        if (usingName.StartsWith("System.Windows.Forms", StringComparison.OrdinalIgnoreCase))
            return "System.Windows.Forms";
        if (usingName.StartsWith("System.Drawing", StringComparison.OrdinalIgnoreCase))
            return "System.Drawing";
        return $"{parts[0]}.{parts[1]}";
    }
    private static bool IsIgnoredScanPath(string filePath)
    {
        var parts = filePath.Split(
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar);
        return parts.Any(part =>
            part.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
            part.Equals("obj", StringComparison.OrdinalIgnoreCase) ||
            part.Equals(".vs", StringComparison.OrdinalIgnoreCase) ||
            part.Equals(".git", StringComparison.OrdinalIgnoreCase) ||
            part.StartsWith("Z.", StringComparison.OrdinalIgnoreCase));
    }
}
