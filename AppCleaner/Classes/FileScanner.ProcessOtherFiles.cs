using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;
namespace AppCleaner
{
    public partial class FileScanner
    {
        private void ScanAndProcessFiles(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Получаем файлы для операции.
            // Если выбрана папка решения — сначала читаем .slnx/.sln и берём проекты оттуда.
            // Если решения нет — ищем проекты рекурсивно.
            // Если проектов нет — сканируем папку как обычную директорию.
            var files = GetFilesForOperation(
                _store.SearchFolder,
                _store.SearchPattern,
                cancellationToken,
                preferProjectFiles: true);
            AddToLog($"Найдено файлов: {files.Length}");
            _store.SetProgressMaximum(files.Length);
            var changedCount = 0;
            for (var i = 0; i < files.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (ProcessFile(files[i], cancellationToken))
                    changedCount++;
            }
            AddToLog(files.Length == 0
                ? "Файлов для обработки не найдено."
                : "Сканирование завершено.");
            AddToLog($"Итог по операции: найдено и заменено: {changedCount}");
        }
        /// <summary>
        /// Определяет папки, которые нужно сканировать.
        /// Приоритет:
        /// 1. .slnx
        /// 2. .sln
        /// 3. рекурсивный поиск .csproj
        /// 4. сама папка
        /// </summary>
        private List<string> GetScanRoots(string rootFolder, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var slnx = Directory
                .GetFiles(rootFolder, "*.slnx", SearchOption.TopDirectoryOnly)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(slnx))
            {
                AddToLog($"[Решение] Используется файл решения: {slnx}");
                var roots = GetProjectsFromSolutionFile(slnx, cancellationToken);
                if (roots.Count > 0)
                    return roots;
            }
            var sln = Directory
                .GetFiles(rootFolder, "*.sln", SearchOption.TopDirectoryOnly)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(sln))
            {
                AddToLog($"[Решение] Используется файл решения: {sln}");
                var roots = GetProjectsFromSolutionFile(sln, cancellationToken);
                if (roots.Count > 0)
                    return roots;
            }
            AddToLog("[Решение] .slnx/.sln не найдено. Используется обход папок.");
            return GetProjectRootsOrFolders(rootFolder, cancellationToken);
        }
        /// <summary>
        /// Извлекает проекты из .sln или .slnx.
        /// Возвращает папки проектов, а не сами .csproj.
        /// </summary>
        private List<string> GetProjectsFromSolutionFile(string solutionFile, CancellationToken cancellationToken)
        {
            var result = new List<string>();
            if (!File.Exists(solutionFile))
                return result;
            var solutionDir = Path.GetDirectoryName(solutionFile);
            if (string.IsNullOrWhiteSpace(solutionDir))
                return result;
            foreach (var line in File.ReadLines(solutionFile))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!line.Contains(".csproj", StringComparison.OrdinalIgnoreCase))
                    continue;
                var projectPath = ExtractCsprojPath(line);
                if (string.IsNullOrWhiteSpace(projectPath))
                    continue;
                if (IsIgnoredProject(projectPath))
                {
                    AddToLog($"[Пропуск проекта] {projectPath}");
                    continue;
                }
                var fullProjectPath = Path.GetFullPath(
                    Path.Combine(solutionDir, projectPath));
                if (!File.Exists(fullProjectPath))
                    continue;
                var projectDir = Path.GetDirectoryName(fullProjectPath);
                if (string.IsNullOrWhiteSpace(projectDir))
                    continue;
                if (!result.Contains(projectDir, StringComparer.OrdinalIgnoreCase))
                {
                    result.Add(projectDir);
                    AddToLog($"[Проект] Найден проект: {fullProjectPath}");
                }
            }
            return result;
        }
        /// <summary>
        /// Достаёт путь к .csproj из строки .sln/.slnx.
        /// Работает и для классического .sln, и для XML-подобных строк .slnx.
        /// </summary>
        private static string? ExtractCsprojPath(string line)
        {
            var match = Regex.Match(
                line,
                @"[""']?([^""'=<>]+\.csproj)[""']?",
                RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;
            return match.Groups[1].Value.Trim();
        }
        /// <summary>
        /// Если решения нет, ищем проекты по папкам.
        /// Важно: если в папке найден .csproj, внутрь этой папки дальше не идём.
        /// </summary>
        private List<string> GetProjectRootsOrFolders(string rootFolder, CancellationToken cancellationToken)
        {
            var result = new List<string>();
            void Walk(string folder)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (IsIgnoredDirectory(folder))
                    return;
                var projects = Directory
                    .GetFiles(folder, "*.csproj", SearchOption.TopDirectoryOnly)
                    .Where(x => !IsIgnoredProject(x))
                    .ToList();
                if (projects.Count > 0)
                {
                    result.Add(folder);
                    AddToLog($"[Папка проекта] {folder}");
                    // Не идём глубже, потому что эта папка уже является проектом.
                    return;
                }
                foreach (var dir in Directory.GetDirectories(folder))
                    Walk(dir);
            }
            Walk(rootFolder);
            if (result.Count == 0)
                result.Add(rootFolder);
            return result;
        }
        /// <summary>
        /// Исключаем копии и бэкапы проектов,
        /// чтобы файл вроде "NewsMaker — копия.csproj" не перехватывал сканирование.
        /// </summary>
        private static bool IsIgnoredProject(string path)
        {
            var file = Path.GetFileName(path);
            return file.Contains("копия", StringComparison.OrdinalIgnoreCase)
                || file.Contains("copy", StringComparison.OrdinalIgnoreCase)
                || file.EndsWith(".bak.csproj", StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Проверяет, можно ли обрабатывать файл.
        /// Исключаем служебные папки сборки и Git.
        /// </summary>
        private static bool IsAllowedFilePath(string path)
        {
            return !path.Contains(@"\bin\", StringComparison.OrdinalIgnoreCase)
                && !path.Contains(@"\obj\", StringComparison.OrdinalIgnoreCase)
                && !path.Contains(@"\.git\", StringComparison.OrdinalIgnoreCase)
                && !path.Contains(@"\.vs\", StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Проверяет, нужно ли пропустить папку при рекурсивном обходе.
        /// </summary>
        private static bool IsIgnoredDirectory(string folder)
        {
            var name = Path.GetFileName(folder);
            return name.Equals("bin", StringComparison.OrdinalIgnoreCase)
                || name.Equals("obj", StringComparison.OrdinalIgnoreCase)
                || name.Equals(".git", StringComparison.OrdinalIgnoreCase)
                || name.Equals(".vs", StringComparison.OrdinalIgnoreCase);
        }
        private bool ProcessFile(string filePath, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!File.Exists(filePath))
            {
                AddToLog($"[Ошибка] Файл не найден: {filePath}");
                CountProcessedFile(filePath);
                return false;
            }
            try
            {
                var encoding = DetectFileEncoding(filePath);
                var lines = File.ReadAllLines(filePath, encoding).ToList();
                var hasChanges = false;
                for (var i = 0; i < lines.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var result = ProcessLine(lines[i]);
                    if (!result.Changed)
                        continue;
                    lines[i] = result.Line;
                    hasChanges = true;
                }
                if (!hasChanges)
                {
                    AddToLog($"[Пропуск] {Path.GetFileName(filePath)} - ничего не найдено.");
                    return false;
                }
                CreateBackup(filePath);
                if (ItemsType is ComboItemsTypes.DeleteEmpty or ComboItemsTypes.DeleteRegionRows)
                    File.WriteAllLines(filePath, lines.Where(line => line.Length > 0), encoding);
                else
                    File.WriteAllLines(filePath, lines, encoding);
                AddToLog($"[Готово] Файл обновлён: {filePath}");
                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                AddToLog($"[Ошибка] {filePath} - {ex.Message}");
                return false;
            }
            finally
            {
                CountProcessedFile(filePath);
            }
        }
        private LineProcessResult ProcessLine(string? line)
        {
            if (line is null)
                return new LineProcessResult(string.Empty, false);
            return ItemsType switch
            {
                ComboItemsTypes.DeleteEmpty => ProcessDeleteEmpty(line),
                ComboItemsTypes.DeleteRegionRows => ProcessDeleteRegionRows(line),
                ComboItemsTypes.FindAndReplace => ProcessFindAndReplace(line),
                _ => new LineProcessResult(line, false)
            };
        }
        private LineProcessResult ProcessFindAndReplace(string line)
        {
            var find = _store.FindText;
            var replace = _store.ReplaceText;
            if (string.IsNullOrEmpty(find) || !line.Contains(find))
                return new LineProcessResult(line, false);
            var newLine = line.Replace(find, replace);
            return string.Equals(line, newLine, StringComparison.Ordinal)
                ? new LineProcessResult(line, false)
                : new LineProcessResult(newLine, true);
        }
        private static LineProcessResult ProcessDeleteRegionRows(string line)
        {
            return Regex.IsMatch(line, @"^\s*#(region|endregion)\b.*$")
                ? new LineProcessResult(string.Empty, true)
                : new LineProcessResult(line, false);
        }
        private static LineProcessResult ProcessDeleteEmpty(string line)
        {
            return string.IsNullOrWhiteSpace(line) || Regex.IsMatch(line, @"^\s*;+\s*$")
                ? new LineProcessResult(string.Empty, true)
                : new LineProcessResult(line, false);
        }
        private readonly record struct LineProcessResult(string Line, bool Changed);
    }
}
