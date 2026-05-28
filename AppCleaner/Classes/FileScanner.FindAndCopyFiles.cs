using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;
namespace AppCleaner
{
    public partial class FileScanner
    {
        #region Find class and copy to folder

        private void FindAndAddClassToProject(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var findText = _store.FindText.Trim();

            // Раньше тут был полный обход *.cs по всем подпапкам.
            // Теперь: если найден проект, ищем только по файлам проекта.
            var files = GetFilesForOperation(
                    _store.SearchFolder,
                    "*.cs",
                    cancellationToken,
                    preferProjectFiles: true,
                    includeDesignerFiles: false)
                .ToArray();

            _store.SetProgressMaximum(files.Length);

            for (var i = 0; i < files.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    AddToLog($"[Проверка] {files[i]}");

                    if (TryExtractClassToFolder(files[i], findText, _store.PlaceFolder, cancellationToken))
                    {
                        AddToLog($"[Готово] Класс {findText} скопирован из файла: {files[i]}");
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка] {files[i]} - {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(files[i]);
                }
            }

            _store.ProgressValue = files.Length;
            AddToLog($"[Не найдено] Класс с текстом {findText} не найден.");
        }

        private bool TryExtractClassToFolder(string sourceFile, string findText, string placeFolder, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(findText))
                return false;

            var encoding = DetectFileEncoding(sourceFile);
            var source = File.ReadAllText(sourceFile, encoding);

            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree.GetCompilationUnitRoot(cancellationToken);

            var typeNode = root
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls =>
                    cls.ToFullString().Contains(findText, StringComparison.Ordinal));

            if (typeNode is null)
                return false;

            var className = typeNode.Identifier.ValueText;

            var namespaceNode = typeNode
                .Ancestors()
                .OfType<BaseNamespaceDeclarationSyntax>()
                .FirstOrDefault();

            var usingsText = string.Join(Environment.NewLine,
                root.Usings.Select(x => x.ToFullString().TrimEnd()));

            var typeText = typeNode.NormalizeWhitespace().ToFullString();

            var resultText = namespaceNode is null
                ? BuildClassFileText(usingsText, typeText)
                : BuildClassFileText(usingsText, namespaceNode.Name.ToString(), typeText);

            Directory.CreateDirectory(placeFolder);

            var destinationFile = Path.Combine(placeFolder, $"{className}.cs");

            BackupExistingFile(destinationFile);
            File.WriteAllText(destinationFile, resultText, encoding);

            return true;
        }

        private static string BuildClassFileText(string usingsText, string typeText)
        {
            return string.IsNullOrWhiteSpace(usingsText)
                ? $"{typeText}{Environment.NewLine}"
                : $"{usingsText}{Environment.NewLine}{Environment.NewLine}{typeText}{Environment.NewLine}";
        }

        private static string BuildClassFileText(string usingsText, string namespaceName, string typeText)
        {
            var header = string.IsNullOrWhiteSpace(usingsText)
                ? string.Empty
                : $"{usingsText}{Environment.NewLine}{Environment.NewLine}";

            return
                $"{header}" +
                $"namespace {namespaceName}{Environment.NewLine}" +
                $"{{{Environment.NewLine}" +
                $"{IndentText(typeText, 4)}{Environment.NewLine}" +
                $"}}{Environment.NewLine}";
        }

        private void BackupExistingFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            var backupPath = filePath + ".bak";
            File.Copy(filePath, backupPath, overwrite: true);
            AddToLog($"[Бэкап] {backupPath}");
        }

        private static string IndentText(string text, int spaces)
        {
            var indent = new string(' ', spaces);

            return string.Join(Environment.NewLine,
                text.Replace("\r\n", "\n")
                    .Replace('\r', '\n')
                    .Split('\n')
                    .Select(line => string.IsNullOrWhiteSpace(line) ? line : indent + line));
        }

        #endregion

        #region Remove non-project files

        private void RemoveNonProjectFiles(bool dryRun, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectFile = _store.ProjectFile;
            var projectDirectory = Path.GetDirectoryName(projectFile);

            if (string.IsNullOrWhiteSpace(projectDirectory) || !Directory.Exists(projectDirectory))
            {
                AddToLog("Ошибка: Не удалось определить папку проекта.");
                return;
            }

            AddToLog(dryRun
                ? $"[DRY-RUN] Проверка ненужных файлов: {projectDirectory}"
                : $"Удаление ненужных файлов: {projectDirectory}");

            var filesInProject = LoadProjectFiles(projectFile, dryRun, cancellationToken);

            // Удаление лишних файлов разрешено только в папках, где есть файлы проекта.
            // Это защищает обычные папки, которые лежат рядом, но не входят в проект.
            var projectFolders = filesInProject
                .Where(path => !ContainsWildcard(path))
                .Select(Path.GetDirectoryName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(NormalizeRelativePath)
                .Where(folder => !ShouldIgnoreFolder(folder))
                .Distinct(PathComparer)
                .ToArray();

            var candidates = new List<string>();

            foreach (var folder in projectFolders)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var fullFolder = Path.Combine(projectDirectory, folder);

                if (!Directory.Exists(fullFolder))
                    continue;

                // Здесь намеренно TopDirectoryOnly.
                // Вложенные папки попадут отдельно, только если в них есть файлы проекта.
                candidates.AddRange(GetCandidateFiles(fullFolder, cancellationToken));
            }

            var uniqueFiles = candidates
                .Distinct(PathComparer)
                .ToArray();

            _store.SetProgressMaximum(uniqueFiles.Length);
            AddToLog($"Файлов для проверки: {uniqueFiles.Length}");

            var deletedCount = 0;

            foreach (var filePath in uniqueFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var relativePath = NormalizeRelativePath(
                    Path.GetRelativePath(projectDirectory, filePath));

                try
                {
                    if (ShouldIgnoreFile(relativePath))
                        continue;

                    if (filesInProject.Contains(relativePath))
                        continue;

                    if (dryRun)
                    {
                        AddToLog($"[DRY-RUN] Был бы удалён: {relativePath}");
                        continue;
                    }

                    File.Delete(filePath);
                    deletedCount++;

                    AddToLog($"Удалено: {relativePath}");
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка] {relativePath}: {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(filePath);
                }
            }

            AddToLog(dryRun
                ? "[DRY-RUN] Проверка завершена."
                : $"Удаление завершено. Удалено файлов: {deletedCount}");
        }

        private string[] GetCandidateFiles(string directory, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var pattern = _store.SearchPattern == FilePatterns.PatternBak
                    ? _store.SearchPattern
                    : "*";

                return Directory.GetFiles(directory, pattern, SearchOption.TopDirectoryOnly)
                    .Where(file => !ShouldIgnoreFile(NormalizeRelativePath(Path.GetFileName(file))))
                    .ToArray();
            }
            catch (Exception ex)
            {
                AddToLog($"Ошибка обхода файлов: {ex.Message}");
                return Array.Empty<string>();
            }
        }

        private HashSet<string> LoadProjectFiles(string projectFile, bool dryRun, CancellationToken cancellationToken)
        {
            var result = new HashSet<string>(PathComparer);

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var projectDirectory = Path.GetDirectoryName(projectFile)!;
                var document = XDocument.Load(projectFile);

                var isSdkProject = document.Root?.Attribute("Sdk") is not null;

                var enableDefaultCompileItems = document
                    .Descendants("EnableDefaultCompileItems")
                    .Select(x => x.Value.Trim())
                    .FirstOrDefault();

                var defaultCompileEnabled =
                    isSdkProject &&
                    !string.Equals(enableDefaultCompileItems, "false", StringComparison.OrdinalIgnoreCase);

                foreach (var item in document.Descendants()
                             .Where(element =>
                                 element.Attribute("Include") is not null ||
                                 element.Attribute("Update") is not null))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var attr = item.Attribute("Include") ?? item.Attribute("Update");
                    var includeValue = NormalizeRelativePath(attr?.Value ?? string.Empty);

                    if (string.IsNullOrWhiteSpace(includeValue))
                        continue;

                    // Если Include содержит MSBuild glob, раскрываем его в реальные файлы.
                    if (ContainsWildcard(includeValue))
                    {
                        foreach (var file in ExpandProjectGlob(projectDirectory, includeValue, cancellationToken))
                            result.Add(file);

                        continue;
                    }

                    if (!ShouldIgnoreFile(includeValue))
                        result.Add(includeValue);
                }

                // SDK-style проекты по умолчанию включают *.cs файлы.
                // Здесь это единственное место, где допустим обход *.cs,
                // потому что иначе проектные файлы просто не перечислены в .csproj.
                // Обход безопасный: Z.*, bin, obj и прочие папки исключаются.
                if (defaultCompileEnabled)
                {
                    foreach (var file in EnumerateFilesSafe(projectDirectory, "*.cs", cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var relativePath = NormalizeRelativePath(Path.GetRelativePath(projectDirectory, file));

                        if (!ShouldIgnoreFile(relativePath))
                            result.Add(relativePath);
                    }
                }

                AddToLog(dryRun
                    ? $"[DRY-RUN] Найдено файлов проекта: {result.Count}"
                    : $"Найдено файлов проекта: {result.Count}");
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                AddToLog($"Ошибка чтения .csproj: {ex.Message}");
            }

            return result;
        }

        // Раскрывает Include="Folder\**\*.cs" и подобные шаблоны в реальные файлы.
        private IEnumerable<string> ExpandProjectGlob(
            string projectDirectory,
            string includePattern,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var normalized = NormalizeRelativePath(includePattern);

            if (!ContainsWildcard(normalized))
                yield break;

            var firstWildcardIndex = normalized.IndexOfAny(new[] { '*', '?' });
            var prefix = firstWildcardIndex <= 0
                ? string.Empty
                : normalized[..firstWildcardIndex];

            var slashIndex = prefix.LastIndexOf(Path.DirectorySeparatorChar);
            var baseRelativeFolder = slashIndex < 0
                ? string.Empty
                : prefix[..slashIndex];

            if (ShouldIgnoreFolder(baseRelativeFolder))
                yield break;

            var baseFolder = string.IsNullOrWhiteSpace(baseRelativeFolder)
                ? projectDirectory
                : Path.Combine(projectDirectory, baseRelativeFolder);

            if (!Directory.Exists(baseFolder))
                yield break;

            // Упрощённо берём расширение из glob'а.
            // Для **/*.cs будет *.cs, для **/*.resx будет *.resx.
            var filePattern = Path.GetFileName(normalized);

            if (string.IsNullOrWhiteSpace(filePattern) || filePattern.Contains("**"))
                filePattern = "*";

            foreach (var file in EnumerateFilesSafe(baseFolder, filePattern, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var relativePath = NormalizeRelativePath(Path.GetRelativePath(projectDirectory, file));

                if (!ShouldIgnoreFile(relativePath))
                    yield return relativePath;
            }
        }

        #endregion

    }
}
