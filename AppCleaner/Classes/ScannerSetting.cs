using System.Collections.Concurrent;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace AppCleaner;
public sealed class ScannerSetting : INotifyPropertyChanged
{
    [Saved]
    public Dictionary<ComboToDoItems, ActionSettings> ActionSettings { get; } = new()
    {
        [ComboToDoItems.DeleteEmpty] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.DeleteRegionRows] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.FindAndReplace] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.FindValueOrClassAddScaveToProject] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Project },
        [ComboToDoItems.ClearNameSpace] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.CollectAllNameSpaces] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.CollectUsingPackages] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.DeleteBakFiles] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.DeleteNonProjectFiles] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.SyncProjectFileWithSample] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Project },
        [ComboToDoItems.ConvertOldCsprojToSdkStyle] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Project },
        [ComboToDoItems.TranslateEnToRu] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.NormalizeMethodSignatures] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.RestoreCSharpFilesFromBak] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },
        [ComboToDoItems.RestoreMissingUsings] = new() { SearchPathType = PathFilterType.Project, PlacePathType = PathFilterType.Project },
        [ComboToDoItems.AddFilePathCommentToCsFiles] = new() { SearchPathType = PathFilterType.Folder, PlacePathType = PathFilterType.Folder },

    };

    public ActionSettings GetActionSettings(ComboToDoItems action)
    {
        return ActionSettings[action];
    }

    public string[] GetPathes(ComboToDoItems action, bool search)
    {
        var settings = GetActionSettings(action);
        var type = search ? settings.SearchPathType : settings.PlacePathType;

        return Pathes
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Where(path =>
            {
                bool isProject = path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase);
                return type == PathFilterType.Project ? isProject : !isProject;
            })
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private string _findText = string.Empty;
    private string _replaceText = string.Empty;
    private string _searchFolder = string.Empty;
    private string _placeFolder = string.Empty;
    private string _projectFile = string.Empty;
    private string _searchPattern = "*.cs";
    private string _logText = string.Empty;
    private int _totalFiles;
    private int _progressValue;
    private int _progressMaximum = 100;
    private int _selectedActionIndex;
    private int _dryRunIndex;
    private bool _beginEnabled;
    private bool _cancelEnabled;
    private bool _isWorking;
    private readonly ConcurrentDictionary<string, int> _totalFolders = new();
    private string _sampleProject = string.Empty;
    private ComboNetItems _netVersion = ComboNetItems.net80;
    private List<string> _pathes = new();
    public ScannerSetting()
    {
        _findText =
            ConfigurationManager.AppSettings["MaskFindSymbolText"]
            ?? Environment.GetEnvironmentVariable("APP_MASK_TOKEN")
            ?? string.Empty;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    [Saved]
    public string FindText
    {
        get => _findText;
        set => SetField(ref _findText, value ?? string.Empty);
    }
    [Saved]
    public string ReplaceText
    {
        get => _replaceText;
        set => SetField(ref _replaceText, value ?? string.Empty);
    }
    [Saved]
    public List<string> Pathes
    {
        get => _pathes;
        set => SetField(ref _pathes, NormalizePathes(value));
    }
    [Saved]
    [Pathes]
    public string SearchFolder
    {
        get => _searchFolder;
        set
        {
            if (SetField(ref _searchFolder, value ?? string.Empty))
                RefreshCommandStates();
        }
    }
    [Saved]
    [Pathes]
    public string PlaceFolder
    {
        get => _placeFolder;
        set
        {
            if (SetField(ref _placeFolder, value ?? string.Empty))
                RefreshCommandStates();
        }
    }
    [Saved]
    [Pathes]
    public string ProjectFile
    {
        get => _projectFile;
        set
        {
            if (SetField(ref _projectFile, value ?? string.Empty))
                RefreshCommandStates();
        }
    }
    [Saved]
    [Pathes]
    public string SampleProjectFile
    {
        get => _sampleProject;
        set
        {
            if (SetField(ref _sampleProject, value ?? string.Empty))
                RefreshCommandStates();
        }
    }
    [Saved]
    public string SearchPattern
    {
        get => _searchPattern;
        set => SetField(ref _searchPattern, value ?? "*.cs");
    }
    [Saved]
    public int SelectedActionIndex
    {
        get => _selectedActionIndex;
        set
        {
            if (SetField(ref _selectedActionIndex, value))
                RefreshCommandStates();
        }
    }
    [Saved]
    public int DryRunIndex
    {
        get => _dryRunIndex;
        set
        {
            if (SetField(ref _dryRunIndex, value))
                OnPropertyChanged(nameof(DryRun));
        }
    }
    [Saved]
    public ComboNetItems NETVersion
    {
        get => _netVersion;
        set => SetField(ref _netVersion, value);
    }
    public bool DryRun => DryRunIndex == 0;
    public string LogText
    {
        get => _logText;
        private set
        {
            if (SetField(ref _logText, value))
                OnPropertyChanged(nameof(SaveEnabled));
        }
    }
    public int TotalFiles
    {
        get => _totalFiles;
        set => SetField(ref _totalFiles, value);
    }
    public int TotalFolders => _totalFolders.Count;
    public int ProgressValue
    {
        get => _progressValue;
        set => SetField(ref _progressValue, Math.Min(Math.Max(0, value), ProgressMaximum));
    }
    public int ProgressMaximum
    {
        get => _progressMaximum;
        set
        {
            value = Math.Max(1, value);
            if (_progressMaximum == value)
                return;
            _progressMaximum = value;
            OnPropertyChanged(nameof(ProgressMaximum));
            if (_progressValue > _progressMaximum)
                ProgressValue = _progressMaximum;
        }
    }
    public bool BeginEnabled
    {
        get => _beginEnabled;
        private set => SetField(ref _beginEnabled, value);
    }
    public bool CancelEnabled
    {
        get => _cancelEnabled;
        private set => SetField(ref _cancelEnabled, value);
    }
    public bool IsWorking
    {
        get => _isWorking;
        set
        {
            if (SetField(ref _isWorking, value))
                RefreshCommandStates();
        }
    }
    public bool SaveEnabled => !string.IsNullOrEmpty(LogText);
    public void SetProgressMaximum(int value)
    {
        ProgressMaximum = value;
        ProgressValue = 0;
    }
    public void AddToLog(string message)
    {
        LogText += $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
    }
    public void RefreshCommandStates()
    {
        var action = SelectedActionIndex < 0
            ? default
            : (ComboToDoItems)SelectedActionIndex;
        BeginEnabled = !IsWorking && action switch
        {
            ComboToDoItems.DeleteNonProjectFiles =>
                !string.IsNullOrWhiteSpace(ProjectFile),
            ComboToDoItems.ConvertOldCsprojToSdkStyle =>
                !string.IsNullOrWhiteSpace(ProjectFile),
            ComboToDoItems.SyncProjectFileWithSample =>
                !string.IsNullOrWhiteSpace(ProjectFile) &&
                !string.IsNullOrWhiteSpace(SampleProjectFile),
            ComboToDoItems.RestoreMissingUsings =>
                !string.IsNullOrWhiteSpace(ProjectFile) &&
                !string.IsNullOrWhiteSpace(SampleProjectFile),
            ComboToDoItems.FindValueOrClassAddScaveToProject =>
                !string.IsNullOrWhiteSpace(SearchFolder) &&
                !string.IsNullOrWhiteSpace(PlaceFolder),
            _ =>
                !string.IsNullOrWhiteSpace(SearchFolder)
        };
        CancelEnabled = IsWorking;
    }
    public void SaveToIni()
    {
        AddPathesFromMarkedProperties();

        var ini = new IniFile();
        ini.SaveObject(this);
    }
    public void LoadFromIni()
    {
        var ini = new IniFile();
        ini.LoadObject(this);

        if (!Enum.IsDefined(typeof(ComboNetItems), NETVersion))
            NETVersion = ComboNetItems.net80;

        RefreshCommandStates();
    }
    public void IncFiles()
    {
        Interlocked.Increment(ref _totalFiles);
        OnPropertyChanged(nameof(TotalFiles));
    }
    public void IncFolders(string folder)
    {
        if (_totalFolders.TryAdd(folder, 0))
            OnPropertyChanged(nameof(TotalFolders));
    }
    public void Reset()
    {
        _logText = string.Empty;
        _progressValue = 0;
        _progressMaximum = 100;
        _totalFiles = 0;
        _totalFolders.Clear();
        OnPropertyChanged(nameof(LogText));
        OnPropertyChanged(nameof(SaveEnabled));
        OnPropertyChanged(nameof(ProgressValue));
        OnPropertyChanged(nameof(ProgressMaximum));
        OnPropertyChanged(nameof(TotalFiles));
        OnPropertyChanged(nameof(TotalFolders));
    }
    internal void IncProgress()
    {
        Interlocked.Increment(ref _progressValue);
        OnPropertyChanged(nameof(ProgressValue));
    }
    internal void AddPathes(string? editValue)
    {
        if (string.IsNullOrWhiteSpace(editValue))
            return;
        editValue = editValue.Trim();
        if (!_pathes.Contains(editValue, StringComparer.OrdinalIgnoreCase))
        {
            _pathes.Add(editValue);
            OnPropertyChanged(nameof(Pathes));
        }
    }
    private void AddPathesFromMarkedProperties()
    {
        foreach (var property in GetPathProperties(GetType()))
        {
            var value = property.GetValue(this)?.ToString();
            if (!string.IsNullOrWhiteSpace(value))
                AddPathes(value);
        }
    }
    private static IEnumerable<PropertyInfo> GetPathProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.CanRead)
            .Where(x => x.GetCustomAttribute<PathesAttribute>() is not null);
    }
    private static List<string> NormalizePathes(IEnumerable<string>? pathes)
    {
        return pathes?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList()
            ?? new List<string>();
    }
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string? GetSearchValue(ComboToDoItems action)
    {
        return GetActionSettings(action).SearchValue;
    }

    public string? GetPlaceValue(ComboToDoItems action)
    {
        return GetActionSettings(action).PlaceValue;
    }

    public void SetSearchValue(ComboToDoItems action, string? value)
    {
        GetActionSettings(action).SearchValue = value;
    }

    public void SetPlaceValue(ComboToDoItems action, string? value)
    {
        GetActionSettings(action).PlaceValue = value;
    }

    public void SetCurrentActionValues(ComboToDoItems action, string? searchValue, string? placeValue)
    {
        var settings = GetActionSettings(action);

        settings.SearchValue = searchValue ?? string.Empty;
        settings.PlaceValue = placeValue ?? string.Empty;

        AddPathes(searchValue);
        AddPathes(placeValue);
    }
    private bool SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName!);
        return true;
    }    
}
