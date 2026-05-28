using System.Collections.Concurrent;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
namespace AppCleaner;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SavedAttribute : Attribute
{
}
public sealed class FileScannerStore : INotifyPropertyChanged
{
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
    private ConcurrentDictionary<string, int> _totalFolders = new ConcurrentDictionary<string, int>();
    private string _sampleProject;

    public FileScannerStore()
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
    public bool DryRun => DryRunIndex == 0;
    public string LogText
    {
        get => _logText;
        private set => SetField(ref _logText, value);
    }
    public int TotalFiles
    {
        get => _totalFiles;
        set => SetField(ref _totalFiles, value);
    }
    public int TotalFolders
    {
        get => _totalFolders.Count;
    }
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
            : (ComboItemsTypes)SelectedActionIndex;
        BeginEnabled = !IsWorking && action switch
        {
            ComboItemsTypes.DeleteNonProjectFiles =>
                !string.IsNullOrWhiteSpace(ProjectFile),
            ComboItemsTypes.SyncProjectFileWithSample =>
                !string.IsNullOrWhiteSpace(ProjectFile) &&
                !string.IsNullOrWhiteSpace(SampleProjectFile),
            ComboItemsTypes.FindValueOrClassAddScaveToProject =>
                !string.IsNullOrWhiteSpace(SearchFolder) &&
                !string.IsNullOrWhiteSpace(PlaceFolder),
            _ =>
                !string.IsNullOrWhiteSpace(SearchFolder)
        };
        CancelEnabled = IsWorking;
    }
    public void SaveToIni(string filePath)
    {
        var ini = new IniFile(filePath);

        foreach (var property in GetSavedProperties())
        {
            var value = property.GetValue(this);
            ini.Write("FileScannerStore", property.Name, value);
        }

        ini.Save();
    }
    public void LoadFromIni(string filePath)
    {
        var ini = new IniFile(filePath);

        foreach (var property in GetSavedProperties())
        {
            var text = ini.Read("FileScannerStore", property.Name);

            if (string.IsNullOrEmpty(text))
                continue;

            try
            {
                var value = ConvertFromString(text, property.PropertyType);
                property.SetValue(this, value);
            }
            catch
            {
            }
        }

        RefreshCommandStates();
    }
    private static IEnumerable<PropertyInfo> GetSavedProperties()
    {
        return typeof(FileScannerStore)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.CanRead && x.CanWrite)
            .Where(x => x.GetCustomAttribute<SavedAttribute>() is not null)
            .Where(x => IsSupportedIniType(x.PropertyType));
    }
    private static bool IsSupportedIniType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        return type == typeof(string)
            || type == typeof(int)
            || type == typeof(bool)
            || type == typeof(long)
            || type == typeof(double)
            || type == typeof(decimal)
            || type.IsEnum;
    }
    private static object? ConvertFromString(string text, Type targetType)
    {
        var nullableType = Nullable.GetUnderlyingType(targetType);
        var realType = nullableType ?? targetType;
        if (nullableType != null && string.IsNullOrWhiteSpace(text))
            return null;
        if (realType == typeof(string))
            return text;
        if (realType.IsEnum)
            return Enum.Parse(realType, text);
        if (realType == typeof(bool))
            return bool.Parse(text);
        return Convert.ChangeType(text, realType, CultureInfo.InvariantCulture);
    }
    private static string EscapeIni(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("=", "\\=");
    }
    private static string UnescapeIni(string value)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i] != '\\' || i == value.Length - 1)
            {
                sb.Append(value[i]);
                continue;
            }
            var next = value[++i];
            sb.Append(next switch
            {
                'r' => '\r',
                'n' => '\n',
                '=' => '=',
                '\\' => '\\',
                _ => next
            });
        }
        return sb.ToString();
    }
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    public void IncFiles()
    {
        Interlocked.Increment(ref _totalFiles);
        OnPropertyChanged(nameof(TotalFiles));
    }
    public void IncFolders(string folder)
    {
        if (_totalFolders.TryAdd(folder, 0))
        {
            OnPropertyChanged(nameof(TotalFolders));
        }
    }
    public void Reset()
    {
        _logText = string.Empty;
        _progressValue = 0;
        _progressMaximum = 100;
        _totalFiles = 0;
        _totalFolders.Clear();
    }
    internal void IncProgress()
    {
        Interlocked.Increment(ref _progressValue);
        OnPropertyChanged(nameof(ProgressValue));
    }

    internal static void LoadMainFormState(MainForm mainForm, string iniFilePath)
    {
        throw new NotImplementedException();
    }

    internal static void SaveMainFormState(MainForm mainForm, string iniFilePath)
    {
        throw new NotImplementedException();
    }
}
