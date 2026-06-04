//FileScanner
#nullable enable
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System.Diagnostics;
using System.Text;
namespace AppCleaner;

public partial class FileScanner : XtraUserControl
{
    private const int UiUpdateIntervalMs = 500;
    private const int BackupMaxAttempts = 10_000;
    private static readonly StringComparer PathComparer = StringComparer.OrdinalIgnoreCase;
    // Папки, которые нельзя сканировать ни при каких условиях.
    // Они исключаются и при проектном режиме, и при fallback-обходе папок.
    private static readonly HashSet<string> IgnoredFolders = new(PathComparer)
    {
        "bin",
        "obj",
        ".vs",
        ".git",
        "node_modules"
    };
    // Файлы, которые нельзя изменять/удалять.
    private static readonly HashSet<string> IgnoredFiles = new(PathComparer)
    {
        "appsettings.json",
        "web.config"
    };

    private readonly ScannerSetting _store = new();
    private readonly System.Windows.Forms.Timer _uiTimer = new();
    private CancellationTokenSource? _operationCts;
    private bool _suppressFolderEditValueChanged;
    private bool _suppressNetEditValueChanged;
    private readonly List<ComboToDoItems> _todoItems = new();
    private readonly List<ComboNetItems> _netItems = new();
    private ComboToDoItems TodoType => _todoType;
    private ComboToDoItems _todoType;


    public FileScanner()
    {
        InitializeComponent();
        InitializeBindings();
        InitializeComboBoxes();
        InitializeControls();
        InitializeUiTimer();
        SetupLayouts();
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _store.LoadFromIni();
        openFileDlg.InitialDirectory = _store.SearchFolder;
        openFolderDlg.InitialDirectory = _store.SearchFolder;
        SetSelectedTodoFromStore();
        SetSelectedNetFromStore();
        RefreshPathComboBoxes();

        cboSelectToDo.SelectedIndex = _store.SelectedActionIndex;

        _todoType = GetTodoBySelectedIndex();

        UpdatePathFilters(_todoType);

        SetupLayouts();
        SyncPathEditorFromStore();
        _store.RefreshCommandStates();

        RefreshUi();
    }
    private void InitializeBindings()
    {
        bsFileScanner.DataSource = _store;
        txtFind.DataBindings.Add("EditValue", bsFileScanner,
            nameof(ScannerSetting.FindText), true,
            DataSourceUpdateMode.OnPropertyChanged);
        txtReplace.DataBindings.Add("EditValue", bsFileScanner,
            nameof(ScannerSetting.ReplaceText), true,
            DataSourceUpdateMode.OnPropertyChanged);
        cboSearchExt.DataBindings.Add("EditValue", bsFileScanner,
            nameof(ScannerSetting.SearchPattern), true,
            DataSourceUpdateMode.OnPropertyChanged);
        cboDRY_RUN.DataBindings.Add("SelectedIndex", bsFileScanner,
            nameof(ScannerSetting.DryRunIndex), true,
            DataSourceUpdateMode.OnPropertyChanged);
        btnBegin.DataBindings.Add("Enabled", bsFileScanner,
            nameof(ScannerSetting.BeginEnabled), true,
            DataSourceUpdateMode.Never);
        btnCancel.DataBindings.Add("Enabled", bsFileScanner,
            nameof(ScannerSetting.CancelEnabled), true,
            DataSourceUpdateMode.Never);
        btnSave.DataBindings.Add("Enabled", bsFileScanner,
            nameof(ScannerSetting.SaveEnabled), true,
            DataSourceUpdateMode.Never);
        progressBar.Properties.Maximum = Math.Max(1, _store.ProgressMaximum);
        _store.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(ScannerSetting.ProgressMaximum) &&
                !string.IsNullOrEmpty(e.PropertyName))
            {
                return;
            }
            SafeInvoke(UpdateProgressMaximum);
        };
    }
    private void InitializeComboBoxes()
    {
        RefreshPathComboBoxes();
        cboSearchExt.Properties.Items.Clear();
        cboSearchExt.Properties.Items.AddRange(FilePatterns.AllPatterns);
        cboSelectToDo.Properties.Items.Clear();
        _todoItems.Clear();
        foreach (ComboToDoItems item in Enum.GetValues<ComboToDoItems>()
                     //.OrderBy(x => x.GetAttribute<ComboItemAttribute>()?.Name ?? x.ToString())
                     )
        {
            _todoItems.Add(item);
            cboSelectToDo.Properties.Items.Add(
                item.GetAttribute<ComboItemAttribute>()?.Name ?? item.ToString());
        }
        SetSelectedTodoFromStore();
        cboNET.Properties.Items.Clear();
        _netItems.Clear();
        foreach (var item in Enum.GetValues<ComboNetItems>())
        {
            _netItems.Add(item);
            cboNET.Properties.Items.Add(GetDisplayName(item));
        }
        SetSelectedNetFromStore();
    }
    private ComboNetItems GetNetBySelectedIndex()
    {
        int index = cboNET.SelectedIndex;
        return index >= 0 && index < _netItems.Count
            ? _netItems[index]
            : default;
    }
    private void SetSelectedNetFromStore()
    {
        var net = Enum.IsDefined(typeof(ComboNetItems), _store.NETVersion)
            ? _store.NETVersion
            : ComboNetItems.net80;
        int index = _netItems.IndexOf(net);
        if (index < 0)
        {
            index = 0;
            net = _netItems.Count > 0
                ? _netItems[index]
                : ComboNetItems.net80;
        }
        _suppressNetEditValueChanged = true;
        try
        {
            cboNET.SelectedIndex = index;
            cboNET.EditValue = cboNET.Properties.Items[index];
            _store.NETVersion = net;
        }
        finally
        {
            _suppressNetEditValueChanged = false;
        }
    }
    private void RefreshPathComboBoxes()
    {
        var pathes = _store.Pathes.ToArray();
        cboSearchFolder.Properties.Items.Clear();
        cboSearchFolder.Properties.Items.AddRange(pathes);
        cboPlaceFolder.Properties.Items.Clear();
        cboPlaceFolder.Properties.Items.AddRange(pathes);
    }
    private void InitializeControls()
    {
        openFileDlg.InitialDirectory = Application.StartupPath;
        openFolderDlg.InitialDirectory = Application.StartupPath;
        _store.RefreshCommandStates();
    }
    private void InitializeUiTimer()
    {
        _uiTimer.Interval = UiUpdateIntervalMs;
        _uiTimer.Tick += (_, _) => RefreshUi();
    }
    internal async Task StartAsync()
    {
        if (!ValidateSelected())
            return;
        SaveCurrentUiStateToStore();

        _store.SetCurrentActionValues(_todoType, cboSearchFolder.Text, cboPlaceFolder.Text);
        _store.SaveToIni();
        BeginOperation();
        try
        {
            LogOperationHeader();
            await RunSelectedOperationAsync(CurrentToken).ConfigureAwait(true);
            AddToLog("Операция завершена.");
        }
        catch (OperationCanceledException)
        {
            AddToLog("Операция отменена пользователем.");
        }
        catch (Exception ex)
        {
            AddToLog($"[Критическая ошибка] {ex.Message}");
            AddToLog(ex.ToString());
        }
        finally
        {
            EndOperation();
        }
    }
    private void SaveCurrentUiStateToStore()
    {
        UpdatePathsFromEditor();
        _store.AddPathes(cboSearchFolder.EditValue?.ToString());
        _store.AddPathes(cboPlaceFolder.EditValue?.ToString());
    }
    public void Cancel()
    {
        _operationCts?.Cancel();
        _store.RefreshCommandStates();
    }
    private CancellationToken CurrentToken => _operationCts?.Token ?? CancellationToken.None;
    private void BeginOperation()
    {
        RefreshPathComboBoxes();
        SetSelectedTodoFromStore();
        _uiTimer.Start();
        _store.IsWorking = true;
        var newCts = new CancellationTokenSource();
        var oldCts = Interlocked.Exchange(ref _operationCts, newCts);
        oldCts?.Cancel();
        oldCts?.Dispose();
        _store.Reset();
        EnsureDefaultTokenIfNeeded();
        _store.RefreshCommandStates();
        RefreshUi();
    }
    private void EndOperation()
    {
        var oldCts = Interlocked.Exchange(ref _operationCts, null);
        oldCts?.Cancel();
        oldCts?.Dispose();
        _store.IsWorking = false;
        _store.RefreshCommandStates();
        _uiTimer.Stop();
        RefreshUi();
    }
    private Task RunSelectedOperationAsync(CancellationToken cancellationToken)
    {
        return TodoType switch
        {
            ComboToDoItems.DeleteEmpty
                or ComboToDoItems.DeleteRegionRows
                or ComboToDoItems.FindAndReplace
                => Task.Run(() => ScanAndProcessFiles(cancellationToken), cancellationToken),
            ComboToDoItems.FindValueOrClassAddScaveToProject
                => Task.Run(() => FindAndAddClassToProject(cancellationToken), cancellationToken),
            ComboToDoItems.ClearNameSpace
                => Task.Run(() => NormalizeNamespacesInDirectory(_store.DryRun, cancellationToken), cancellationToken),
            ComboToDoItems.CollectAllNameSpaces
                => Task.Run(() => CollectAllNamespaces(cancellationToken), cancellationToken),
            ComboToDoItems.CollectUsingPackages
                => Task.Run(() => CollectRequiredPackagesFromUsings(cancellationToken), cancellationToken),
            ComboToDoItems.DeleteBakFiles
                => Task.Run(() => DeleteBakFiles(cancellationToken), cancellationToken),
            ComboToDoItems.DeleteNonProjectFiles
                => Task.Run(() => RemoveNonProjectFiles(_store.DryRun, cancellationToken), cancellationToken),
            ComboToDoItems.SyncProjectFileWithSample
                => Task.Run(() => SyncProjectFileWithSample(cancellationToken), cancellationToken),
            ComboToDoItems.ConvertOldCsprojToSdkStyle
                => Task.Run(() => ConvertOldCsprojToSdkStyle(
                    _store.ProjectFile,
                    _store.NETVersion), cancellationToken),
            ComboToDoItems.TranslateEnToRu
                => Task.Run(() => TranslateEnToRuFolderAsync(cancellationToken), cancellationToken),
            ComboToDoItems.NormalizeMethodSignatures
                => Task.Run(() => NormalizeMethodSignaturesFolderAsync(cancellationToken), cancellationToken),
            ComboToDoItems.RestoreCSharpFilesFromBak
                => Task.Run(() => RestoreCSharpFilesFromBakFolderAsync(cancellationToken), cancellationToken),
            ComboToDoItems.RestoreMissingUsings
                => Task.Run(() => RecoveryMissingUsings(cancellationToken), cancellationToken),
            ComboToDoItems.AddFilePathCommentToCsFiles =>
                Task.Run(() => AddFilePathCommentToCsFiles(cancellationToken), cancellationToken),
            _ => Task.CompletedTask
        };
    }
    private ComboToDoItems GetSelectedAction()
    {
        return TodoType;
    }
    private ComboToDoItems GetTodoBySelectedIndex()
    {
        int index = cboSelectToDo.SelectedIndex;
        return index >= 0 && index < _todoItems.Count
            ? _todoItems[index]
            : default;
    }
    private void SetSelectedTodoFromStore()
    {
        var todo = Enum.IsDefined(typeof(ComboToDoItems), _store.SelectedActionIndex)
            ? (ComboToDoItems)_store.SelectedActionIndex
            : default;
        int index = _todoItems.IndexOf(todo);
        if (index < 0)
            index = 0;
        cboSelectToDo.SelectedIndex = index;
        _todoType = GetTodoBySelectedIndex();
        _store.SelectedActionIndex = (int)_todoType;
    }
    private void LogOperationHeader()
    {
        switch (TodoType)
        {
            case ComboToDoItems.DeleteNonProjectFiles:
                AddToLog($"Файл проекта: {_store.ProjectFile}");
                break;
            case ComboToDoItems.SyncProjectFileWithSample:
                AddToLog($"Файл проекта: {_store.ProjectFile}");
                AddToLog($"Образец файла проекта: {_store.SampleProjectFile}");
                break;
            default:
                AddToLog($"Папка: {_store.SearchFolder}");
                break;
        }
        if (TodoType != ComboToDoItems.FindValueOrClassAddScaveToProject)
            AddToLog($"Маска файлов: {_store.SearchPattern}");
        if (TodoType == ComboToDoItems.FindValueOrClassAddScaveToProject)
            AddToLog($"Папка назначения: {_store.PlaceFolder}");
    }
    #region UI events
    private void cboSearchExt_EditValueChanged(object sender, EventArgs e)
    {
        _store.SearchPattern = cboSearchExt.EditValue?.ToString() ?? "*.cs";
    }

    #region UpdatePathFilters

    private void cboSelectToDo_SelectedIndexChanged(object sender, EventArgs e)
    {
        _todoType = GetTodoBySelectedIndex();

        _store.SelectedActionIndex = (int)_todoType;

        UpdatePathFilters(_todoType);

        SetupLayouts();
        SyncPathEditorFromStore();
        _store.RefreshCommandStates();
    }

    private void UpdatePathFilters(ComboToDoItems action)
    {
        FillCombo(cboSearchFolder, _store.GetPathes(action, true));
        FillCombo(cboPlaceFolder, _store.GetPathes(action, false));

        cboSearchFolder.Text = _store.GetSearchValue(action) ?? string.Empty;
        cboPlaceFolder.Text = _store.GetPlaceValue(action) ?? string.Empty;
    }
    private static void FillCombo(ComboBoxEdit combo, string[] items)
    {
        combo.Properties.Items.BeginUpdate();

        try
        {
            combo.Properties.Items.Clear();
            combo.Properties.Items.AddRange(items.Cast<object>().ToArray());
        }
        finally
        {
            combo.Properties.Items.EndUpdate();
        }
    }
    private void cboSearchFolder_EditValueChanged(object sender, EventArgs e)
    {
        _store.SetSearchValue(_todoType, cboSearchFolder.Text);
        _store.AddPathes(cboSearchFolder.Text);
        _store.RefreshCommandStates();
    }

    private void cboPlaceFolder_EditValueChanged(object sender, EventArgs e)
    {
        _store.SetPlaceValue(_todoType, cboPlaceFolder.Text);
        _store.AddPathes(cboPlaceFolder.Text);
        _store.RefreshCommandStates();
    }
    #endregion

    private void cboDRY_RUN_EditValueChanged(object sender, EventArgs e)
    {
        _store.DryRunIndex = cboDRY_RUN.SelectedIndex;
    }
    private async void btnBegin_Click(object sender, EventArgs e)
    {
        try
        {
            await StartAsync().ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            AddToLog($"[Критическая ошибка] {ex.Message}");
        }
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    private void maskSymbol_EditValueChanged(object sender, EventArgs e)
    {
        _store.RefreshCommandStates();
    }
    private void txtFolder_EditValueChanged(object sender, EventArgs e)
    {
        UpdatePathsFromEditor();
        _store.RefreshCommandStates();
    }
    private void searchFolder_EditValueChanged(object sender, EventArgs e)
    {
        UpdatePathsFromEditor();
        _store.RefreshCommandStates();
    }
    private void searchFolder_BtnClick(object sender, ButtonPressedEventArgs e)
    {
        if (e.Button.Kind == ButtonPredefines.Combo)
        {
            return;
        }
        SelectPath(sender as ButtonEdit);
    }
    #endregion
    #region UI helpers
    void SelectPath(ButtonEdit selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        string? selectedPath = ShowPathDialog();
        if (string.IsNullOrWhiteSpace(selectedPath))
            return;
        selector.EditValue = selectedPath;
        UpdatePathsFromEditor();
        _store.RefreshCommandStates();
    }
    private string? ShowPathDialog()
    {
        bool useFileDialog =
            TodoType == ComboToDoItems.DeleteNonProjectFiles ||
            TodoType is ComboToDoItems.SyncProjectFileWithSample or ComboToDoItems.RestoreMissingUsings ||
            TodoType == ComboToDoItems.ConvertOldCsprojToSdkStyle;
        if (useFileDialog)
        {
            return openFileDlg.ShowDialog() == DialogResult.OK
                ? openFileDlg.FileName
                : null;
        }
        return openFolderDlg.ShowDialog() == DialogResult.OK
            ? openFolderDlg.SelectedPath
            : null;
    }
    private void SyncPathEditorFromStore()
    {
        _suppressFolderEditValueChanged = true;
        try
        {
            cboSearchFolder.EditValue = TodoType switch
            {
                ComboToDoItems.DeleteNonProjectFiles => _store.ProjectFile,
                ComboToDoItems.SyncProjectFileWithSample => _store.ProjectFile,
                ComboToDoItems.ConvertOldCsprojToSdkStyle => _store.ProjectFile,
                _ => _store.SearchFolder
            };
            cboPlaceFolder.EditValue = TodoType switch
            {
                ComboToDoItems.SyncProjectFileWithSample => _store.SampleProjectFile,
                ComboToDoItems.ConvertOldCsprojToSdkStyle => _store.SampleProjectFile,
                ComboToDoItems.FindValueOrClassAddScaveToProject => _store.PlaceFolder,
                _ => string.Empty
            };
        }
        finally
        {
            _suppressFolderEditValueChanged = false;
        }
    }
    private void UpdatePathsFromEditor()
    {
        if (_suppressFolderEditValueChanged)
            return;
        string searchValue = cboSearchFolder.EditValue?.ToString() ?? string.Empty;
        string placeValue = cboPlaceFolder.EditValue?.ToString() ?? string.Empty;
        switch (TodoType)
        {
            case ComboToDoItems.DeleteNonProjectFiles:
                _store.ProjectFile = searchValue;
                break;
            case ComboToDoItems.SyncProjectFileWithSample:
            case ComboToDoItems.ConvertOldCsprojToSdkStyle:
                _store.ProjectFile = searchValue;
                break;
            case ComboToDoItems.FindValueOrClassAddScaveToProject:
                _store.SearchFolder = searchValue;
                _store.PlaceFolder = placeValue;
                break;
            default:
                _store.SearchFolder = searchValue;
                break;
        }
        _store.RefreshCommandStates();
    }
    private void SetupLayouts()
    {
        var attr = TodoType.GetAttribute<ComboItemAttribute>();
        bool isFindReplace = TodoType == ComboToDoItems.FindAndReplace;
        bool isFindAdd = TodoType == ComboToDoItems.FindValueOrClassAddScaveToProject;
        bool isSync = TodoType is ComboToDoItems.SyncProjectFileWithSample or ComboToDoItems.RestoreMissingUsings;
        bool isConvert = TodoType == ComboToDoItems.ConvertOldCsprojToSdkStyle;
        bool isProjectMode = isFindAdd || isSync || isConvert;
        cboSearchFolder.Properties.NullValuePrompt = isConvert
            ? "Установите старый файл проекта..."
            : isProjectMode
                ? "Установите файл проекта для сравнения..."
                : "Установите папку для сканирования...";
        cboPlaceFolder.Properties.NullValuePrompt = isConvert
            ? "Установите путь нового SDK-style проекта..."
            : isSync
                ? "Установите образец файла проекта..."
                : "Установите папку куда копировать найденное...";
        lcSearchFolder.Text = attr?.SearchLabel ?? "Cканировать папку:";
        lcPlaceFolder.Text = attr?.PlaceLabel ?? "Папка для найденного:";
        SetVisibility(lcNET_Version, isConvert);
        SetVisibility(emptySearchExt, !isProjectMode);
        SetVisibility(lcSearchExt, !isProjectMode);
        SetVisibility(lgFindReplace, isFindReplace || isFindAdd || isConvert);
        SetVisibility(lcDRY_RUN, TodoType is ComboToDoItems.ClearNameSpace or ComboToDoItems.DeleteNonProjectFiles);
        SetVisibility(lcFind, isFindReplace || isFindAdd);
        SetVisibility(lcReplace, isFindReplace);
        SetVisibility(lcPlaceFolder, isFindAdd || isSync);
        cboSearchFolder.Properties.NullValuePrompt = isProjectMode
            ? "Установите файл проекта для сравнения..."
            : "Установите папку для сканирования...";
        cboPlaceFolder.Properties.NullValuePrompt = isSync
            ? "Установите образец файла проекта..."
            : "Установите папку куда копировать найденное...";
    }
    private static void SetVisibility(BaseLayoutItem item, bool visible)
    {
        item.Visibility = visible
            ? LayoutVisibility.Always
            : LayoutVisibility.Never;
    }
    private void UpdateProgressMaximum()
    {
        progressBar.Properties.Maximum = Math.Max(1, _store.ProgressMaximum);
        if (progressBar.Position > progressBar.Properties.Maximum)
            progressBar.Position = progressBar.Properties.Maximum;
    }
    private void SafeInvoke(Action action)
    {
        if (IsDisposed || Disposing)
            return;
        try
        {
            if (InvokeRequired)
            {
                BeginInvoke(action);
                return;
            }
            action();
        }
        catch (InvalidOperationException)
        {
        }
    }
    private void ScrollToEnd()
    {
        if (!logMemo.IsHandleCreated)
            return;
        var text = logMemo.EditValue?.ToString();
        if (string.IsNullOrEmpty(text))
            return;
        logMemo.SelectionStart = text.Length;
        logMemo.ScrollToCaret();
    }
    private void RefreshUi()
    {
        SafeInvoke(() =>
        {
            foundFiles.EditValue = _store.TotalFiles;
            if (!Equals(foundFolders.EditValue, _store.TotalFolders))
                foundFolders.EditValue = _store.TotalFolders;
            progressBar.EditValue = _store.ProgressValue;
            logMemo.EditValue = _store.LogText;
            ScrollToEnd();
        });
    }
    #endregion
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _uiTimer.Stop();
            _uiTimer.Dispose();
            var oldCts = Interlocked.Exchange(ref _operationCts, null);
            oldCts?.Cancel();
            oldCts?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
    private void cboNET_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_suppressNetEditValueChanged)
            return;
        _store.NETVersion = GetNetBySelectedIndex();
    }
    private string GetLogFileName()
    {
        string name = TodoType.ToString();
        foreach (var ch in Path.GetInvalidFileNameChars())
            name = name.Replace(ch, '_');
        return $"{name}.log";
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
        using SaveFileDialog dlg = new()
        {
            InitialDirectory = openFolderDlg.InitialDirectory,
            Title = "Сохранить лог",
            FileName = GetLogFileName(),
            Filter = "Log files (*.log)|*.log",
            DefaultExt = "log",
            AddExtension = true
        };
        if (dlg.ShowDialog() != DialogResult.OK)
            return;
        File.WriteAllText(
            dlg.FileName,
            logMemo.Text,
            Encoding.UTF8);
        Process.Start(new ProcessStartInfo
        {
            FileName = "devenv.exe",
            Arguments = $"/edit \"{dlg.FileName}\"",
            UseShellExecute = true
        });
    }

}
