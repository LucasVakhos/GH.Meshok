// Classes\FileScanner.ProcessFilePathComments.cs
using System.Text;

namespace AppCleaner;

public partial class FileScanner
{
    private void AddFilePathCommentToCsFiles(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var files = GetFilesForOperation(
            _store.SearchFolder,
            "*.cs",
            cancellationToken,
            preferProjectFiles: true,
            includeDesignerFiles: false);

        _store.SetProgressMaximum(files.Length);
        AddToLog($"Найдено .cs файлов проекта: {files.Length}");

        var changedCount = 0;
        var skippedCount = 0;

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var projectRoot = GetProjectRootForFile(file, cancellationToken);
                var relativePath = GetRelativeFilePath(projectRoot, file);
                var comment = $"// {relativePath}";

                var encoding = DetectFileEncoding(file);
                var text = File.ReadAllText(file, encoding);

                if (HasSameHeaderComment(text, comment))
                {
                    skippedCount++;
                    AddToLog($"[Пропуск] Уже есть comment: {relativePath}");
                    continue;
                }

                if (!_store.DryRun)
                {
                    CreateBackup(file);

                    var newText = comment + Environment.NewLine + text;
                    File.WriteAllText(file, newText, encoding);
                }

                changedCount++;
                AddToLog(_store.DryRun
                    ? $"[DRY RUN] Будет добавлен: {comment}"
                    : $"[Обновлён] {comment}");
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

        AddToLog($"Готово. Обновлено: {changedCount}, пропущено: {skippedCount}");
    }

    private string GetProjectRootForFile(string filePath, CancellationToken cancellationToken)
    {
        var folder = Path.GetDirectoryName(filePath);

        while (!string.IsNullOrWhiteSpace(folder))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Directory.GetFiles(folder, "*.csproj", SearchOption.TopDirectoryOnly).Any())
                return folder;

            folder = Directory.GetParent(folder)?.FullName;
        }

        return _store.SearchFolder;
    }

    private static string GetRelativeFilePath(string rootFolder, string filePath)
    {
        var relativePath = Path.GetRelativePath(rootFolder, filePath);
        return relativePath.Replace("/", "\\");
    }

    private static bool HasSameHeaderComment(string text, string comment)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        using var reader = new StringReader(text);
        var firstLine = reader.ReadLine()?.Trim();

        return string.Equals(firstLine, comment, StringComparison.OrdinalIgnoreCase);
    }
}