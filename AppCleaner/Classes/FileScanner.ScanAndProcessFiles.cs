using System.Text.RegularExpressions;
namespace AppCleaner
{
    public partial class FileScanner
    {
        #region Project-aware file selection

        // Главный метод выбора файлов для всех операций.
        // Логика такая:
        // 1. Если найден .csproj, берём только файлы, которые реально принадлежат проекту.
        // 2. Если .csproj не найден, делаем fallback: безопасно обходим папки рекурсивно.
        // 3. Z.* и служебные папки исключаются всегда.
        private string[] GetFilesForOperation(string rootFolder, string searchPattern, CancellationToken cancellationToken, bool preferProjectFiles = true, bool includeDesignerFiles = true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(rootFolder) || !Directory.Exists(rootFolder))
                return Array.Empty<string>();

            var projectFile = preferProjectFiles
                ? FindProjectFile(rootFolder, cancellationToken)
                : null;

            IEnumerable<string> files;

            if (!string.IsNullOrWhiteSpace(projectFile))
            {
                AddToLog($"[Проект] Используется файл проекта: {projectFile}");
                files = GetPhysicalProjectFiles(projectFile, searchPattern, cancellationToken);
            }
            else
            {
                AddToLog("[Проект] Файл проекта не найден. Используется полный обход папок.");
                files = EnumerateFilesSafe(rootFolder, searchPattern, cancellationToken);
            }

            if (!includeDesignerFiles)
                files = files.Where(file => !IsDesignerFile(file));

            return files
                .Where(File.Exists)
                .Distinct(PathComparer)
                .ToArray();
        }

        // Ищем .csproj сначала в выбранной папке, потом ниже по дереву.
        // При поиске тоже не заходим в Z.*, bin, obj и прочие исключённые папки.
        private string? FindProjectFile(string rootFolder, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var topLevelProject = Directory
                    .GetFiles(rootFolder, "*.csproj", SearchOption.TopDirectoryOnly)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(topLevelProject))
                    return topLevelProject;
            }
            catch
            {
                return null;
            }

            return EnumerateFilesSafe(rootFolder, "*.csproj", cancellationToken)
                .FirstOrDefault();
        }

        // Возвращает физические файлы проекта.
        // Важно: тут не надо blindly сканировать все папки проекта.
        // Мы сначала читаем .csproj и берём только то, что проект реально включает.
        private IEnumerable<string> GetPhysicalProjectFiles(string projectFile, string searchPattern, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectDirectory = Path.GetDirectoryName(projectFile);

            if (string.IsNullOrWhiteSpace(projectDirectory) || !Directory.Exists(projectDirectory))
                yield break;

            var relativeFiles = LoadProjectFiles(projectFile, dryRun: false, cancellationToken);

            foreach (var relativeFile in relativeFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(relativeFile))
                    continue;

                // Если в .csproj попал glob или папка, он будет отфильтрован здесь.
                // Реальные glob'ы раскрываются внутри LoadProjectFiles.
                if (ContainsWildcard(relativeFile))
                    continue;

                if (ShouldIgnoreFile(relativeFile))
                    continue;

                var fullPath = Path.Combine(projectDirectory, relativeFile);

                if (!File.Exists(fullPath))
                    continue;

                if (!FileMatchesPattern(fullPath, searchPattern))
                    continue;

                yield return fullPath;
            }
        }

        // Безопасный fallback-обход папок.
        // Используется только если .csproj не найден либо для операций, где проект намеренно не нужен.
        private IEnumerable<string> EnumerateFilesSafe(string rootFolder, string searchPattern, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(rootFolder) || !Directory.Exists(rootFolder))
                yield break;

            var stack = new Stack<string>();
            stack.Push(rootFolder);

            while (stack.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var folder = stack.Pop();
                var relativeFolder = NormalizeRelativePath(Path.GetRelativePath(rootFolder, folder));

                if (!string.IsNullOrWhiteSpace(relativeFolder) && ShouldIgnoreFolder(relativeFolder))
                    continue;

                string[] files;

                try
                {
                    files = Directory.GetFiles(folder, searchPattern, SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    AddToLog($"[Пропуск папки] {folder} - {ex.Message}");
                    continue;
                }

                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var relativeFile = NormalizeRelativePath(Path.GetRelativePath(rootFolder, file));

                    if (!ShouldIgnoreFile(relativeFile))
                        yield return file;
                }

                string[] subFolders;

                try
                {
                    subFolders = Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly);
                }
                catch
                {
                    continue;
                }

                foreach (var subFolder in subFolders)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var relativeSubFolder = NormalizeRelativePath(Path.GetRelativePath(rootFolder, subFolder));

                    if (!ShouldIgnoreFolder(relativeSubFolder))
                        stack.Push(subFolder);
                }
            }
        }

        // Проверка маски файла без лишнего Directory.GetFiles по всему дереву.
        private static bool FileMatchesPattern(string filePath, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern) || pattern == "*")
                return true;

            var fileName = Path.GetFileName(filePath);

            var regex = "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$";

            return Regex.IsMatch(fileName, regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        private static bool ContainsWildcard(string path)
        {
            return path.Contains('*') || path.Contains('?');
        }

        #endregion    }
        #region Delete bak

        private void DeleteBakFiles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // .bak обычно не входит в .csproj, поэтому тут проектный режим отключён.
            // Но обход всё равно безопасный: Z.*, bin, obj и прочие папки исключены.
            var files = GetFilesForOperation(
                _store.SearchFolder,
                "*.bak",
                cancellationToken,
                preferProjectFiles: false);

            _store.SetProgressMaximum(files.Length);
            AddToLog($"Найдено .bak файлов: {files.Length}");

            var deletedCount = 0;

            for (var i = 0; i < files.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    File.Delete(files[i]);
                    deletedCount++;
                    AddToLog($"[Удалён] {files[i]}");
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка удаления] {files[i]} - {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(files[i]);
                }
            }

            AddToLog($"Удаление .bak завершено. Удалено: {deletedCount}");
        }

        #endregion

    }
}