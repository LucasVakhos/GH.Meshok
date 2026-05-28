//FileScanner.Utils
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using SysAttr = System.Attribute;
namespace AppCleaner;

public partial class FileScanner
{
    #region Validation

    private bool ValidateSelected()
    {
        return ItemsType switch
        {
            ComboItemsTypes.DeleteNonProjectFiles => ValidateProjectFile(),
            ComboItemsTypes.SyncProjectFileWithSample => ValidateSyncProjectFileWithSample(),
            ComboItemsTypes.ConvertOldCsprojToSdkStyle => ValidateConvertOldCsprojToSdkStyle(),
            ComboItemsTypes.FindValueOrClassAddScaveToProject => ValidateFindAddToProject(),
            ComboItemsTypes.FindAndReplace => ValidateFolder() && ValidateFindText(),
            _ => ValidateFolder()
        };
    }

    private bool ValidateSyncProjectFileWithSample()
    {
        if (string.IsNullOrWhiteSpace(_store.ProjectFile) ||
            !_store.ProjectFile.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(_store.ProjectFile))
        {
            AddToLog("Ошибка: Указанный путь не является существующим файлом проекта (.csproj).");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_store.SampleProjectFile) ||
            !_store.SampleProjectFile.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(_store.SampleProjectFile))
        {
            AddToLog("Ошибка: Указанный образец не является существующим файлом проекта (.csproj).");
            return false;
        }

        var projectDirectory = Path.GetDirectoryName(_store.ProjectFile);
        if (string.IsNullOrWhiteSpace(projectDirectory) || !Directory.Exists(projectDirectory))
        {
            AddToLog("Ошибка: Не удалось определить папку проекта.");
            return false;
        }

        var sampleDirectory = Path.GetDirectoryName(_store.SampleProjectFile);
        if (string.IsNullOrWhiteSpace(sampleDirectory) || !Directory.Exists(sampleDirectory))
        {
            AddToLog("Ошибка: Не удалось определить папку проекта.");
            return false;
        }

        return true;
    }

    private bool ValidateConvertOldCsprojToSdkStyle()
    {
        if (string.IsNullOrWhiteSpace(_store.ProjectFile) ||
            !_store.ProjectFile.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(_store.ProjectFile))
        {
            AddToLog("Ошибка: старый .csproj не найден.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_store.SampleProjectFile) ||
            !_store.SampleProjectFile.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            AddToLog("Ошибка: укажите путь для нового .csproj.");
            return false;
        }

        return true;
    }

    private bool ValidateProjectFile()
    {
        if (string.IsNullOrWhiteSpace(_store.ProjectFile) ||
            !_store.ProjectFile.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(_store.ProjectFile))
        {
            AddToLog("Ошибка: Указанный путь не является существующим файлом проекта (.csproj).");
            return false;
        }

        var projectDirectory = Path.GetDirectoryName(_store.ProjectFile);

        if (!string.IsNullOrWhiteSpace(projectDirectory) &&
            Directory.Exists(projectDirectory))
        {
            return true;
        }

        AddToLog("Ошибка: Не удалось определить папку проекта.");
        return false;
    }

    private bool ValidateFolder()
    {
        if (!string.IsNullOrWhiteSpace(_store.SearchFolder) &&
            Directory.Exists(_store.SearchFolder))
        {
            return true;
        }

        AddToLog($"Папка не найдена: {_store.SearchFolder}");
        return false;
    }

    private bool ValidateFindText()
    {
        if (!string.IsNullOrEmpty(_store.FindText))
            return true;

        AddToLog("Ошибка: не указана строка для поиска.");
        return false;
    }

    private bool ValidateFindAddToProject()
    {
        if (!ValidateFolder())
            return false;

        if (string.IsNullOrWhiteSpace(_store.FindText))
        {
            AddToLog("Ошибка: не указано имя класса для поиска.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_store.PlaceFolder) ||
            !Directory.Exists(_store.PlaceFolder))
        {
            AddToLog($"Ошибка: папка назначения не найдена: {_store.PlaceFolder}");
            return false;
        }

        return true;
    }

    #endregion

    #region Project sync

    private void SyncProjectFileWithSample(CancellationToken cancellationToken)
    {
        var sampleDoc = XDocument.Load(_store.SampleProjectFile, LoadOptions.PreserveWhitespace);
        var projectDoc = XDocument.Load(_store.ProjectFile, LoadOptions.PreserveWhitespace);

        XNamespace sampleNs = sampleDoc.Root?.Name.Namespace ?? XNamespace.None;
        XNamespace projectNs = projectDoc.Root?.Name.Namespace ?? XNamespace.None;

        EnsureSdkCompileInclude(projectDoc, projectNs);

        var sampleItems = GetConcreteCompileItems(sampleDoc, sampleNs)
            .ToDictionary(
                x => NormalizePath(x.Path),
                x => ToCompileUpdate(x.Element, projectNs),
                StringComparer.OrdinalIgnoreCase);

        var projectUpdateItems = projectDoc
            .Descendants(projectNs + "Compile")
            .Where(x => x.Attribute("Update") != null)
            .Select(x => new
            {
                Path = x.Attribute("Update")!.Value,
                Element = x
            })
            .Where(x => IsConcreteCsFile(x.Path))
            .ToList();

        foreach (var item in projectUpdateItems)
        {
            if (!sampleItems.ContainsKey(NormalizePath(item.Path)))
                item.Element.Remove();
        }

        var existingUpdates = projectDoc
            .Descendants(projectNs + "Compile")
            .Where(x => x.Attribute("Update") != null)
            .Select(x => x.Attribute("Update")!.Value)
            .Where(IsConcreteCsFile)
            .Select(NormalizePath)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var targetItemGroup = projectDoc.Root!
            .Elements(projectNs + "ItemGroup")
            .FirstOrDefault(g => g.Elements(projectNs + "Compile")
                .Any(x => x.Attribute("Update") != null));

        if (targetItemGroup == null)
        {
            targetItemGroup = new XElement(projectNs + "ItemGroup");
            projectDoc.Root!.Add(targetItemGroup);
        }

        foreach (var item in sampleItems)
        {
            if (!existingUpdates.Contains(item.Key))
                targetItemGroup.Add(item.Value);
        }

        projectDoc.Save(_store.ProjectFile);
    }

    private static void EnsureSdkCompileInclude(XDocument projectDoc, XNamespace ns)
    {
        var hasCompileInclude = projectDoc
            .Descendants(ns + "Compile")
            .Any(x =>
                x.Attribute("Include")?.Value == @"**\*.cs" ||
                x.Attribute("Include")?.Value == "**/*.cs");

        if (hasCompileInclude)
            return;

        var firstItemGroup = projectDoc.Root!
            .Elements(ns + "ItemGroup")
            .FirstOrDefault();

        if (firstItemGroup == null)
        {
            firstItemGroup = new XElement(ns + "ItemGroup");
            projectDoc.Root!.Add(firstItemGroup);
        }

        firstItemGroup.AddFirst(
            new XElement(ns + "Compile",
                new XAttribute("Include", @"**\*.cs"),
                new XAttribute("Exclude", @"bin\**;obj\**;Actions\**;DataSources\**")));
    }

    #endregion

    #region Project convert

    private static void ConvertOldCsprojToSdkStyle(string oldCsprojPath, string newCsprojPath)
    {
        var oldDoc = XDocument.Load(oldCsprojPath, LoadOptions.PreserveWhitespace);
        XNamespace oldNs = oldDoc.Root?.Name.Namespace ?? XNamespace.None;

        string sdk = GetInstalledSdkName();
        string targetFramework = GetTargetFramework(oldDoc, oldNs);

        var newDoc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("Project",
                new XAttribute("Sdk", sdk),

                new XElement("PropertyGroup",
                    new XElement("OutputType", GetValue(oldDoc, oldNs, "OutputType", "WinExe")),
                    new XElement("TargetFramework", targetFramework),
                    new XElement("Nullable", "enable"),
                    new XElement("UseWindowsForms", "true"),
                    new XElement("ImplicitUsings", "enable"),
                    new XElement("EnableDefaultCompileItems", "false"),
                    new XElement("EnableDefaultEmbeddedResourceItems", "false"),
                    new XElement("EnableDefaultNoneItems", "false")
                ),

                new XElement("ItemGroup",
                    new XElement("Compile",
                        new XAttribute("Include", @"**\*.cs"),
                        new XAttribute("Exclude", @"bin\**;obj\**")),

                    new XElement("EmbeddedResource",
                        new XAttribute("Include", @"**\*.resx"),
                        new XAttribute("Exclude", @"bin\**;obj\**")),

                    new XElement("None",
                        new XAttribute("Include", @"**\*"),
                        new XAttribute("Exclude", @"**\*.cs;**\*.resx;bin\**;obj\**"))
                )
            )
        );

        AddCompileUpdates(oldDoc, newDoc, oldNs);
        AddEmbeddedResourceUpdates(oldDoc, newDoc, oldNs);
        AddPackageReferences(oldDoc, newDoc, oldNs);

        newDoc.Save(newCsprojPath);
    }

    private static string GetInstalledSdkName()
    {
        var sdks = GetInstalledDotnetSdks();

        if (sdks.Any(x => x.StartsWith("8.")))
            return "Microsoft.NET.Sdk";

        if (sdks.Any(x => x.StartsWith("6.") || x.StartsWith("7.") || x.StartsWith("9.")))
            return "Microsoft.NET.Sdk";

        return "Microsoft.NET.Sdk";
    }

    private static List<string> GetInstalledDotnetSdks()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--list-sdks",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null)
                return [];

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output
                .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' ')[0].Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    private static string GetTargetFramework(XDocument doc, XNamespace ns)
    {
        var oldVersion = GetValue(doc, ns, "TargetFrameworkVersion", "v4.8");

        return oldVersion switch
        {
            "v4.8" => "net8.0-windows",
            "v4.7.2" => "net8.0-windows",
            "v4.7.1" => "net8.0-windows",
            "v4.7" => "net8.0-windows",
            _ => "net8.0-windows"
        };
    }

    private static void AddCompileUpdates(XDocument oldDoc, XDocument newDoc, XNamespace oldNs)
    {
        var itemGroup = new XElement("ItemGroup");

        foreach (var compile in oldDoc.Descendants(oldNs + "Compile"))
        {
            var include = compile.Attribute("Include")?.Value;
            if (string.IsNullOrWhiteSpace(include))
                continue;

            if (!include.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            var update = new XElement("Compile",
                new XAttribute("Update", include));

            foreach (var child in compile.Elements())
            {
                update.Add(new XElement(
                    child.Name.LocalName,
                    child.Attributes(),
                    child.Nodes()));
            }

            if (update.HasElements)
                itemGroup.Add(update);
        }

        if (itemGroup.HasElements)
            newDoc.Root!.Add(itemGroup);
    }

    private static void AddEmbeddedResourceUpdates(XDocument oldDoc, XDocument newDoc, XNamespace oldNs)
    {
        var itemGroup = new XElement("ItemGroup");

        foreach (var res in oldDoc.Descendants(oldNs + "EmbeddedResource"))
        {
            var include = res.Attribute("Include")?.Value;
            if (string.IsNullOrWhiteSpace(include))
                continue;

            if (!include.EndsWith(".resx", StringComparison.OrdinalIgnoreCase))
                continue;

            var update = new XElement("EmbeddedResource",
                new XAttribute("Update", include));

            foreach (var child in res.Elements())
            {
                update.Add(new XElement(
                    child.Name.LocalName,
                    child.Attributes(),
                    child.Nodes()));
            }

            if (update.HasElements)
                itemGroup.Add(update);
        }

        if (itemGroup.HasElements)
            newDoc.Root!.Add(itemGroup);
    }

    private static void AddPackageReferences(XDocument oldDoc, XDocument newDoc, XNamespace oldNs)
    {
        var itemGroup = new XElement("ItemGroup");

        foreach (var package in oldDoc.Descendants(oldNs + "PackageReference"))
        {
            itemGroup.Add(new XElement("PackageReference",
                package.Attributes(),
                package.Elements()));
        }

        if (itemGroup.HasElements)
            newDoc.Root!.Add(itemGroup);
    }

    #endregion

    #region Compile helpers

    private static IEnumerable<(string Path, XElement Element)> GetConcreteCompileItems(
        XDocument doc,
        XNamespace ns)
    {
        return doc
            .Descendants(ns + "Compile")
            .Select(x => new
            {
                Path = x.Attribute("Update")?.Value
                    ?? x.Attribute("Include")?.Value,
                Element = x
            })
            .Where(x => IsConcreteCsFile(x.Path))
            .Select(x => (x.Path!, x.Element));
    }

    private static IEnumerable<(string Path, XElement Element)> GetCompileItems(XDocument doc, XNamespace ns)
    {
        return doc
            .Descendants(ns + "Compile")
            .Select(x => new
            {
                Path = x.Attribute("Update")?.Value
                    ?? x.Attribute("Include")?.Value,
                Element = x
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Path))
            .Select(x => (x.Path!, x.Element));
    }

    private static XElement ToCompileUpdate(XElement sourceCompile, XNamespace targetNs)
    {
        var path = sourceCompile.Attribute("Update")?.Value
            ?? sourceCompile.Attribute("Include")?.Value
            ?? string.Empty;

        var result = new XElement(targetNs + "Compile",
            new XAttribute("Update", path));

        foreach (var child in sourceCompile.Elements())
        {
            result.Add(new XElement(
                targetNs + child.Name.LocalName,
                child.Attributes(),
                child.Nodes()));
        }

        return result;
    }

    private static string? GetCompilePath(XElement compile)
    {
        return compile.Attribute("Update")?.Value
            ?? compile.Attribute("Include")?.Value;
    }

    private static bool IsConcreteCsFile(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        path = NormalizePath(path);

        return path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
            && !path.Contains('*')
            && !path.Contains('?');
    }

    #endregion

    #region Common helpers

    private void EnsureDefaultTokenIfNeeded()
    {
        var selectedAction = GetSelectedAction();

        if (!string.IsNullOrWhiteSpace(_store.FindText))
            return;

        var field = typeof(ComboItemsTypes).GetField(selectedAction.ToString());

        if (field is null)
            return;

        var attr = (ComboItemAttribute?)SysAttr.GetCustomAttribute(field, typeof(ComboItemAttribute));

        if (string.IsNullOrWhiteSpace(attr?.Pattern))
            return;

        _store.FindText = attr.Pattern;
        AddToLog($"[Конфиг] MaskFindSymbolText установлен по умолчанию: {_store.FindText}");
    }

    private static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field is null)
            return value.ToString();

        var displayAttr = (DisplayAttribute?)SysAttr.GetCustomAttribute(field, typeof(DisplayAttribute));

        if (!string.IsNullOrWhiteSpace(displayAttr?.GetName()))
            return displayAttr.GetName()!;

        var comboAttr = (ComboItemAttribute?)SysAttr.GetCustomAttribute(field, typeof(ComboItemAttribute));

        return !string.IsNullOrWhiteSpace(comboAttr?.Name)
            ? comboAttr.Name
            : value.ToString();
    }

    private static string NormalizePath(string path)
    {
        return path.Replace('/', '\\').Trim();
    }

    private static string NormalizeRelativePath(string path)
    {
        return (path ?? string.Empty)
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar)
            .Trim();
    }

    private static string GetValue(XDocument doc, XNamespace ns, string name, string defaultValue)
    {
        return doc.Descendants(ns + name).FirstOrDefault()?.Value?.Trim()
            ?? defaultValue;
    }

    #endregion

    #region Ignore rules

    private static bool IsDesignerFile(string filePath)
    {
        return filePath.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ShouldIgnoreFile(string relativePath)
    {
        relativePath = NormalizeRelativePath(relativePath);

        if (string.IsNullOrWhiteSpace(relativePath))
            return true;

        if (IgnoredFiles.Contains(Path.GetFileName(relativePath)))
            return true;

        var folder = Path.GetDirectoryName(relativePath);

        return !string.IsNullOrWhiteSpace(folder) && ShouldIgnoreFolder(folder);
    }

    private static bool ShouldIgnoreFolder(string? relativeFolder)
    {
        if (string.IsNullOrWhiteSpace(relativeFolder))
            return false;

        var parts = NormalizeRelativePath(relativeFolder)
            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Where(x => !string.IsNullOrWhiteSpace(x));

        foreach (var part in parts)
        {
            if (IgnoredFolders.Contains(part))
                return true;

            if (part.StartsWith("Z.", StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    #endregion

    #region File helpers

    private static Encoding DetectFileEncoding(string filePath)
    {
        try
        {
            return filePath.DetectEncodingFromBomOrHeuristic();
        }
        catch
        {
            return Encoding.Default;
        }
    }

    private void AddToLog(string message)
    {
        _store.AddToLog(message);
    }

    private void CountProcessedFile(string filePath)
    {
        _store.IncFiles();
        _store.IncFolders(Path.GetDirectoryName(filePath) ?? string.Empty);
        _store.IncProgress();
    }

    private void CreateBackup(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            AddToLog($"[Ошибка бэкапа] файл не найден: {filePath}");
            return;
        }

        for (var attempt = 0; attempt <= BackupMaxAttempts; attempt++)
        {
            try
            {
                var candidatePath = attempt == 0
                    ? filePath + ".bak"
                    : $"{filePath}.{attempt}.bak";

                using var source = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var destination = new FileStream(candidatePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

                source.CopyTo(destination);
                CopyFileTimestamps(filePath, candidatePath);

                AddToLog($"[Бэкап] {candidatePath}");
                return;
            }
            catch (IOException)
            {
            }
            catch (Exception ex)
            {
                AddToLog($"[Ошибка бэкапа] {filePath} - {ex.Message}");
                return;
            }
        }

        AddToLog($"[Ошибка бэкапа] не удалось создать уникальный бэкап для {filePath}");
    }

    private static void CopyFileTimestamps(string sourcePath, string destinationPath)
    {
        try
        {
            File.SetCreationTimeUtc(destinationPath, File.GetCreationTimeUtc(sourcePath));
            File.SetLastWriteTimeUtc(destinationPath, File.GetLastWriteTimeUtc(sourcePath));
        }
        catch
        {
        }
    }

    #endregion
}