//FileScanner.Utils
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SysAttr = System.Attribute;
namespace AppCleaner;
public partial class FileScanner
{
    #region Validation
    private bool ValidateSelected()
    {
        return TodoType switch
        {
            ComboToDoItems.DeleteNonProjectFiles => ValidateProjectFile(),
            ComboToDoItems.SyncProjectFileWithSample => ValidateSyncProjectFileWithSample(),
            ComboToDoItems.ConvertOldCsprojToSdkStyle => ValidateConvertOldCsprojToSdkStyle(),
            ComboToDoItems.FindValueOrClassAddScaveToProject => ValidateFindAddToProject(),
            ComboToDoItems.FindAndReplace => ValidateFolder() && ValidateFindText(),
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
    private void ConvertOldCsprojToSdkStyle(string csprojPath, ComboNetItems netVersion)
    {
        if (string.IsNullOrWhiteSpace(csprojPath))
            throw new ArgumentException("Не указан файл проекта.", nameof(csprojPath));
        if (!File.Exists(csprojPath))
            throw new FileNotFoundException("Файл проекта не найден.", csprojPath);
        string backupPath = GetBackupFilePath(csprojPath);
        File.Copy(csprojPath, backupPath, overwrite: false);
        AddToLog($"Создан backup: {backupPath}");
        var oldDoc = XDocument.Load(backupPath, LoadOptions.PreserveWhitespace);
        XNamespace oldNs = oldDoc.Root?.Name.Namespace ?? XNamespace.None;
        string targetFramework = netVersion.ToTargetFramework();
        AddToLog($"TargetFramework: {targetFramework}");
        bool isSdkStyle = oldDoc.Root?.Attribute("Sdk") != null;
        var newDoc = isSdkStyle
            ? ConvertExistingSdkStyleProject(oldDoc, oldNs, targetFramework)
            : ConvertLegacyProjectToSdkStyle(oldDoc, oldNs, targetFramework);
        EnsureAppConfigWithLibsProbing(csprojPath);
        EnsureAppConfigCopyToOutput(newDoc);
        EnsureAfterBuildTarget(newDoc);
        newDoc.Save(csprojPath);
        AddToLog($"Создан SDK-style проект: {csprojPath}");
    }
    private static XDocument ConvertExistingSdkStyleProject(XDocument oldDoc, XNamespace oldNs, string targetFramework)
    {
        var newDoc = new XDocument(oldDoc);
        var mainGroup = newDoc.Root!
            .Elements(oldNs + "PropertyGroup")
            .FirstOrDefault(x => x.Attribute("Condition") == null);
        if (mainGroup == null)
        {
            mainGroup = new XElement("PropertyGroup");
            newDoc.Root!.AddFirst(mainGroup);
        }
        SetOrAddElement(mainGroup, "TargetFramework", targetFramework);
        SetOrAddElement(mainGroup, "UseWindowsForms", "true");
        SetOrAddElement(mainGroup, "ImplicitUsings", "enable");
        RemoveElement(mainGroup, "EnableDefaultCompileItems");
        RemoveElement(mainGroup, "EnableDefaultEmbeddedResourceItems");
        RemoveElement(mainGroup, "EnableDefaultNoneItems");
        return newDoc;
    }
    private static XDocument ConvertLegacyProjectToSdkStyle(XDocument oldDoc, XNamespace oldNs, string targetFramework)
    {
        var newDoc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("Project",
                new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                CreateLegacyMainPropertyGroup(oldDoc, oldNs, targetFramework),
                CreateConfigurationPropertyGroup(oldDoc, oldNs, "Debug|AnyCPU")
            )
        );
        CopyItemGroups(oldDoc, newDoc, oldNs);
        return newDoc;
    }
    private static XElement CreateLegacyMainPropertyGroup(XDocument oldDoc, XNamespace oldNs, string targetFramework)
    {
        return new XElement("PropertyGroup",
            new XElement("OutputType", GetValue(oldDoc, oldNs, "OutputType", "Library")),
            new XElement("RootNamespace", GetValue(oldDoc, oldNs, "RootNamespace", "")),
            new XElement("AssemblyName", GetValue(oldDoc, oldNs, "AssemblyName", "")),
            new XElement("TargetFramework", targetFramework),
            new XElement("UseWindowsForms", "true"),
            new XElement("ImplicitUsings", "enable"),
            new XElement("LangVersion", "latest"),
            new XElement("EnableDefaultCompileItems", "false"),
            new XElement("EnableDefaultEmbeddedResourceItems", "false"),
            new XElement("EnableDefaultNoneItems", "false")
        );
    }
    private static XElement CreateConfigurationPropertyGroup(XDocument oldDoc, XNamespace oldNs, string configuration)
    {
        var oldGroup = oldDoc.Root!
            .Elements(oldNs + "PropertyGroup")
            .FirstOrDefault(x =>
            {
                var condition = x.Attribute("Condition")?.Value;
                return !string.IsNullOrWhiteSpace(condition)
                    && condition.Contains(configuration, StringComparison.OrdinalIgnoreCase);
            });
        var newGroup = new XElement("PropertyGroup",
            new XAttribute("Condition", $" '$(Configuration)|$(Platform)' == '{configuration}' "));
        if (oldGroup == null)
            return newGroup;
        foreach (var element in oldGroup.Elements())
            newGroup.Add(CloneWithoutNamespace(element));
        return newGroup;
    }
    private static void CopyItemGroups(XDocument oldDoc, XDocument newDoc, XNamespace oldNs)
    {
        foreach (var oldItemGroup in oldDoc.Root!.Elements(oldNs + "ItemGroup"))
        {
            var newItemGroup = new XElement("ItemGroup");
            foreach (var item in oldItemGroup.Elements())
                newItemGroup.Add(CloneWithoutNamespace(item));
            if (newItemGroup.HasElements)
                newDoc.Root!.Add(newItemGroup);
        }
    }
    private static XElement CloneWithoutNamespace(XElement source)
    {
        return new XElement(
            source.Name.LocalName,
            source.Attributes(),
            source.Nodes().Select(x =>
                x is XElement element
                    ? CloneWithoutNamespace(element)
                    : x));
    }
    private static void EnsureAppConfigCopyToOutput(XDocument newDoc)
    {
        bool exists = newDoc.Root!
            .Descendants("None")
            .Any(x => string.Equals(
                x.Attribute("Update")?.Value,
                "App.config",
                StringComparison.OrdinalIgnoreCase));
        if (exists)
            return;
        newDoc.Root!.Add(
            new XElement("ItemGroup",
                new XElement("None",
                    new XAttribute("Update", "App.config"),
                    new XElement("CopyToOutputDirectory", "PreserveNewest"))));
    }
    private static void EnsureAfterBuildTarget(XDocument newDoc)
    {
        newDoc.Root!
            .Elements("Target")
            .Where(x => string.Equals(
                x.Attribute("Name")?.Value,
                "AfterBuild",
                StringComparison.OrdinalIgnoreCase))
            .Remove();
        AddAfterBuildTarget(newDoc);
    }
    private static void AddAfterBuildTarget(XDocument newDoc)
    {
        //newDoc.Root!.Add(
        //    new XElement("Target",
        //        new XAttribute("Name", "MoveDependenciesToLibs"),
        //        new XAttribute("AfterTargets", "Build"),
        //        new XElement("ItemGroup",
        //            new XElement("MoveToLibFolder",
        //                new XAttribute("Include", "$(OutputPath)*.dll;$(OutputPath)*.xml"),
        //                new XAttribute("Exclude",
        //                    "$(TargetPath);" +
        //                    "$(OutputPath)$(AssemblyName).dll;" +
        //                    "$(OutputPath)$(AssemblyName).xml"))),
        //        new XElement("MakeDir",
        //            new XAttribute("Directories", "$(OutputPath)libs")),
        //        new XElement("Move",
        //            new XAttribute("SourceFiles", "@(MoveToLibFolder)"),
        //            new XAttribute("DestinationFolder", "$(OutputPath)libs"),
        //            new XAttribute("OverwriteReadOnlyFiles", "true")),
        //        new XElement("ItemGroup",
        //            new XElement("FilesToDelete",
        //                new XAttribute("Include", "$(OutputPath)**\\*.*"),
        //                new XAttribute("Exclude",
        //                    "$(OutputPath)*.exe;" +
        //                    "$(OutputPath)$(AssemblyName).dll;" +
        //                    "$(OutputPath)$(AssemblyName).pdb;" +
        //                    "$(OutputPath)$(AssemblyName).deps.json;" +
        //                    "$(OutputPath)$(AssemblyName).runtimeconfig.json;" +
        //                    "$(OutputPath)$(AssemblyName).exe.config;" +
        //                    "$(OutputPath)App.config;" +
        //                    "$(OutputPath)libs\\**\\*.*;" +
        //                    "$(OutputPath)ru\\**\\*.*;" +
        //                    "$(OutputPath)b\\**\\*.*;" +
        //                    "$(OutputPath)runtimes\\**\\*.*"))),
        //        new XElement("Delete",
        //            new XAttribute("Files", "@(FilesToDelete)"))
        //    ));

        newDoc.Root!.Add(
            new XElement("Target",
                new XAttribute("Name", "CopyFilesToLibs"),
                new XAttribute("AfterTargets", "Build"),

                new XComment(@"
 временно

 <ItemGroup>
     <DllFiles Include=""$(OutputPath)*.dll"" Exclude=""$(OutputPath)$(AssemblyName).dll"" />
 </ItemGroup>

 <Message Text=""Найденные DLL для перемещения: @(DllFiles)"" Importance=""high"" />

 <MakeDir Directories=""$(OutputPath)Libs"" />

 <Move SourceFiles=""@(DllFiles)"" DestinationFolder=""$(OutputPath)Libs"" />

 <Exec Command=""powershell -Command ""$exclude = @('libs', 'ru', 'runtimes'); Get-ChildItem '$(OutputPath)' -Directory | Where-Object { $exclude -notcontains $_.Name } | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue"""" />
"),

                new XElement("Exec",
                    new XAttribute(
                        "Command",
                        "powershell -Command &quot;$exclude = @('ru', 'runtimes'); Get-ChildItem '$(OutputPath)' -Directory | Where-Object { $exclude -notcontains $_.Name } | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue&quot;"))
            )
        );
    }
    private static void EnsureAppConfigWithLibsProbing(string csprojPath)
    {
        string projectDir = Path.GetDirectoryName(csprojPath)
            ?? throw new InvalidOperationException("Не удалось определить каталог проекта.");
        string appConfigPath = Path.Combine(projectDir, "App.config");
        XDocument doc = File.Exists(appConfigPath)
            ? XDocument.Load(appConfigPath, LoadOptions.PreserveWhitespace)
            : new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("configuration"));
        XElement configuration;
        if (doc.Root == null)
        {
            configuration = new XElement("configuration");
            doc.Add(configuration);
        }
        else if (doc.Root.Name.LocalName == "configuration")
        {
            configuration = doc.Root;
        }
        else
        {
            configuration = new XElement("configuration", doc.Root);
            doc.Root.ReplaceWith(configuration);
        }
        var runtime = configuration.Element("runtime");
        if (runtime == null)
        {
            runtime = new XElement("runtime");
            configuration.Add(runtime);
        }
        XNamespace asmNs = "urn:schemas-microsoft-com:asm.v1";
        var assemblyBinding = runtime.Elements(asmNs + "assemblyBinding").FirstOrDefault();
        if (assemblyBinding == null)
        {
            assemblyBinding = new XElement(asmNs + "assemblyBinding");
            runtime.Add(assemblyBinding);
        }
        var probing = assemblyBinding.Element(asmNs + "probing");
        if (probing == null)
        {
            assemblyBinding.Add(new XElement(
                asmNs + "probing",
                new XAttribute("privatePath", "libs")));
        }
        else
        {
            probing.SetAttributeValue("privatePath", "libs");
        }
        doc.Save(appConfigPath);
    }
    private static void SetOrAddElement(XElement parent, string elementName, string value)
    {
        var element = parent.Element(elementName);
        if (element == null)
        {
            parent.Add(new XElement(elementName, value));
            return;
        }
        element.Value = value;
    }
    private static void RemoveElement(XElement parent, string elementName)
    {
        parent.Elements(elementName).Remove();
    }
    private static string GetBackupFilePath(string filePath)
    {
        string backupPath = filePath + ".bak";
        if (!File.Exists(backupPath))
            return backupPath;
        for (int i = 1; i < 10_000; i++)
        {
            backupPath = $"{filePath}.{i}.bak";
            if (!File.Exists(backupPath))
                return backupPath;
        }
        throw new IOException("Не удалось создать имя backup-файла.");
    }
    private static string GetValue(XDocument doc, XNamespace ns, string elementName, string defaultValue)
    {
        return doc.Descendants(ns + elementName)
                  .FirstOrDefault()
                  ?.Value
                  ?.Trim()
               ?? defaultValue;
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
        var field = typeof(ComboToDoItems).GetField(selectedAction.ToString());
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
        if (string.IsNullOrEmpty(path))
            return string.Empty;
        path = path.Replace('\\', '/');
        if (path.StartsWith("/"))
            path = path.TrimStart('/');
        return path;
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
        if (string.IsNullOrWhiteSpace(filePath))
        {
            AddToLog("[Ошибка бэкапа] путь к файлу пустой.");
            return;
        }

        if (!File.Exists(filePath))
        {
            AddToLog($"[Ошибка бэкапа] файл не найден: {filePath}");
            return;
        }

        try
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFilePath = $"{filePath}.{timestamp}.bak";

            using var source = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var destination = new FileStream(backupFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

            source.CopyTo(destination);

            CopyFileTimestamps(filePath, backupFilePath);

            AddToLog($"[Бэкап создан] {backupFilePath}");
        }
        catch (Exception ex)
        {
            AddToLog($"[Ошибка бэкапа] {filePath} - {ex.Message}");
        }
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
