//FileScanner
#nullable enable
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System.Xml.Linq;

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

    private readonly FileScannerStore _store = new();
    private readonly System.Windows.Forms.Timer _uiTimer = new();
    private readonly string _iniFilePath = Path.Combine(Application.StartupPath, "FileScanner.ini");

    private CancellationTokenSource? _operationCts;
    private bool _suppressFolderEditValueChanged;

    private ComboItemsTypes ItemsType => _itemsType;
    private ComboItemsTypes _itemsType;

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

        _store.LoadFromIni(_iniFilePath);
        openFileDlg.InitialDirectory = _store.SearchFolder;
        openFolderDlg.InitialDirectory = _store.SearchFolder;

        _itemsType = _store.SelectedActionIndex < 0
            ? default
            : (ComboItemsTypes)_store.SelectedActionIndex;

        SetupLayouts();
        SyncPathEditorFromStore();
        RefreshUi();
    }

    private void InitializeBindings()
    {
        bsFileScanner.DataSource = _store;

        txtFind.DataBindings.Add("EditValue", bsFileScanner,
            nameof(FileScannerStore.FindText), true,
            DataSourceUpdateMode.OnPropertyChanged);

        txtReplace.DataBindings.Add("EditValue", bsFileScanner,
            nameof(FileScannerStore.ReplaceText), true,
            DataSourceUpdateMode.OnPropertyChanged);

        //txtPlaceFolder.DataBindings.Add("EditValue", bsFileScanner,
        //    nameof(FileScannerStore.PlaceFolder), true,
        //    DataSourceUpdateMode.OnPropertyChanged);

        cboSearchExt.DataBindings.Add("EditValue", bsFileScanner,
            nameof(FileScannerStore.SearchPattern), true,
            DataSourceUpdateMode.OnPropertyChanged);

        cboSelectToDo.DataBindings.Add("SelectedIndex", bsFileScanner,
            nameof(FileScannerStore.SelectedActionIndex), true,
            DataSourceUpdateMode.OnPropertyChanged);

        cboDRY_RUN.DataBindings.Add("SelectedIndex", bsFileScanner,
            nameof(FileScannerStore.DryRunIndex), true,
            DataSourceUpdateMode.OnPropertyChanged);

        btnBegin.DataBindings.Add("Enabled", bsFileScanner,
            nameof(FileScannerStore.BeginEnabled), true,
            DataSourceUpdateMode.Never);

        btnCancel.DataBindings.Add("Enabled", bsFileScanner,
            nameof(FileScannerStore.CancelEnabled), true,
            DataSourceUpdateMode.Never);

        progressBar.Properties.Maximum = Math.Max(1, _store.ProgressMaximum);

        _store.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(FileScannerStore.ProgressMaximum) &&
                !string.IsNullOrEmpty(e.PropertyName))
            {
                return;
            }

            SafeInvoke(UpdateProgressMaximum);
        };
    }

    private void InitializeComboBoxes()
    {
        cboSearchExt.Properties.Items.Clear();
        cboSearchExt.Properties.Items.AddRange(FilePatterns.AllPatterns);

        cboSelectToDo.Properties.Items.Clear();

        foreach (ComboItemsTypes item in Enum.GetValues(typeof(ComboItemsTypes)))
            cboSelectToDo.Properties.Items.Add(GetDisplayName(item));

        cboSelectToDo.SelectedIndex = 0;
        _itemsType = cboSelectToDo.SelectedIndex < 0
            ? default
            : (ComboItemsTypes)cboSelectToDo.SelectedIndex;
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
        _store.SaveToIni(_iniFilePath);

        if (!ValidateSelected())
            return;

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

    public void Cancel()
    {
        _operationCts?.Cancel();
        _store.RefreshCommandStates();
    }

    private CancellationToken CurrentToken => _operationCts?.Token ?? CancellationToken.None;

    private void BeginOperation()
    {
        _itemsType = _store.SelectedActionIndex < 0
            ? default
            : (ComboItemsTypes)_store.SelectedActionIndex;

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
        return ItemsType switch
        {
            ComboItemsTypes.DeleteEmpty
                or ComboItemsTypes.DeleteRegionRows
                or ComboItemsTypes.FindAndReplace
                => Task.Run(() => ScanAndProcessFiles(cancellationToken), cancellationToken),
            ComboItemsTypes.FindValueOrClassAddScaveToProject
                => Task.Run(() => FindAndAddClassToProject(cancellationToken), cancellationToken),
            ComboItemsTypes.ClearNameSpace
                => Task.Run(() => NormalizeNamespacesInDirectory(_store.DryRun, cancellationToken), cancellationToken),
            ComboItemsTypes.CollectAllNameSpaces
                => Task.Run(() => CollectAllNamespaces(cancellationToken), cancellationToken),
            ComboItemsTypes.CollectUsingPackages
                => Task.Run(() => CollectRequiredPackagesFromUsings(cancellationToken), cancellationToken),
            ComboItemsTypes.DeleteBakFiles
                => Task.Run(() => DeleteBakFiles(cancellationToken), cancellationToken),
            ComboItemsTypes.DeleteNonProjectFiles
                => Task.Run(() => RemoveNonProjectFiles(_store.DryRun, cancellationToken), cancellationToken),
            ComboItemsTypes.SyncProjectFileWithSample
                => Task.Run(() => SyncProjectFileWithSample(cancellationToken), cancellationToken),
            ComboItemsTypes.ConvertOldCsprojToSdkStyle
                => Task.Run(() => ConvertOldCsprojToSdkStyle(_store.ProjectFile, _store.SampleProjectFile), cancellationToken),
            ComboItemsTypes.TranslateEnglishToRussian
                => Task.Run(() => TranslateEnglishInFolderAsync(cancellationToken), cancellationToken),
            _ => Task.CompletedTask
        };
    }

    private ComboItemsTypes GetSelectedAction()
    {
        return _store.SelectedActionIndex < 0
            ? default
            : (ComboItemsTypes)_store.SelectedActionIndex;
    }

    private void LogOperationHeader()
    {
        switch (ItemsType)
        {
            case ComboItemsTypes.DeleteNonProjectFiles:
                AddToLog($"Файл проекта: {_store.ProjectFile}");
                break;
            case ComboItemsTypes.SyncProjectFileWithSample:
                AddToLog($"Файл проекта: {_store.ProjectFile}");
                AddToLog($"Образец файла проекта: {_store.SampleProjectFile}");
                break;
            default:
                AddToLog($"Папка: {_store.SearchFolder}");
                break;
        }


        if (ItemsType != ComboItemsTypes.FindValueOrClassAddScaveToProject)
            AddToLog($"Маска файлов: {_store.SearchPattern}");

        if (ItemsType == ComboItemsTypes.FindValueOrClassAddScaveToProject)
            AddToLog($"Папка назначения: {_store.PlaceFolder}");
    }

    #region UI events

    private void cboSearchExt_EditValueChanged(object sender, EventArgs e)
    {
        _store.SearchPattern = cboSearchExt.EditValue?.ToString() ?? "*.cs";
    }

    private void cboSelectToDo_SelectedIndexChanged(object sender, EventArgs e)
    {
        _store.SelectedActionIndex = cboSelectToDo.SelectedIndex;
        _itemsType = cboSelectToDo.SelectedIndex < 0
            ? default
            : (ComboItemsTypes)cboSelectToDo.SelectedIndex;

        SetupLayouts();
        SyncPathEditorFromStore();
        _store.RefreshCommandStates();
    }

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
        SelectPath(sender as ButtonEdit);
    }

    private void searchFolder_Click(object sender, EventArgs e)
    {
        SelectPath(sender as ButtonEdit);
    }

    #endregion

    #region UI helpers

    private void SelectPath(ButtonEdit selector)
    {
        if (selector == null)
            return;

        string? selectedPath = ShowPathDialog();

        if (!string.IsNullOrWhiteSpace(selectedPath))
            selector.EditValue = selectedPath;
    }

    private string? ShowPathDialog()
    {
        bool useFileDialog =
            ItemsType == ComboItemsTypes.DeleteNonProjectFiles ||
            ItemsType == ComboItemsTypes.SyncProjectFileWithSample;

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
            cboSearchFolder.EditValue = ItemsType switch
            {
                ComboItemsTypes.DeleteNonProjectFiles => _store.ProjectFile,
                ComboItemsTypes.SyncProjectFileWithSample => _store.ProjectFile,
                ComboItemsTypes.ConvertOldCsprojToSdkStyle => _store.ProjectFile,
                _ => _store.SearchFolder
            };

            cboPlaceFolder.EditValue = ItemsType switch
            {
                ComboItemsTypes.SyncProjectFileWithSample => _store.SampleProjectFile,
                ComboItemsTypes.ConvertOldCsprojToSdkStyle => _store.SampleProjectFile,
                ComboItemsTypes.FindValueOrClassAddScaveToProject => _store.PlaceFolder,
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

        switch (ItemsType)
        {
            case ComboItemsTypes.DeleteNonProjectFiles:
                _store.ProjectFile = searchValue;
                break;

            case ComboItemsTypes.SyncProjectFileWithSample:
            case ComboItemsTypes.ConvertOldCsprojToSdkStyle:
                _store.ProjectFile = searchValue;
                _store.SampleProjectFile = placeValue;
                break;

            case ComboItemsTypes.FindValueOrClassAddScaveToProject:
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

        var attr = ItemsType.GetAttribute<ComboItemAttribute>();

        bool isFindReplace = ItemsType == ComboItemsTypes.FindAndReplace;
        bool isFindAdd = ItemsType == ComboItemsTypes.FindValueOrClassAddScaveToProject;
        bool isSync = ItemsType == ComboItemsTypes.SyncProjectFileWithSample;
        bool isConvert = ItemsType == ComboItemsTypes.ConvertOldCsprojToSdkStyle;
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

        SetVisibility(emptySearchExt, !isProjectMode);
        SetVisibility(lcSearchExt, !isProjectMode);
        SetVisibility(lgFindReplace, isFindReplace || isFindAdd);
        SetVisibility(lcDRY_RUN, ItemsType is ComboItemsTypes.ClearNameSpace or ComboItemsTypes.DeleteNonProjectFiles);
        SetVisibility(lcFind, isFindReplace || isFindAdd);
        SetVisibility(lcReplace, isFindReplace);
        SetVisibility(lcPlaceFolder, isFindAdd || isSync || isConvert);

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
}

