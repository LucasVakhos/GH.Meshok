#nullable enable
// ============================================================================
// GH.Components.Refactored.cs
// ----------------------------------------------------------------------------
// Новый вариант ActionList в духе Delphi TActionList, но с архитектурой,
// более подходящей для WinForms + DevExpress:
//
// 1. ActionBase хранит состояние действия и выполняет команду.
// 2. ActionList хранит коллекцию действий и связи Action <-> UI-компонент.
// 3. IActionBinding синхронизирует конкретный UI-компонент с ActionBase.
// 4. Designer автоматически даёт действиям имена Action1, Action2, ...
//
// ВАЖНО:
// - MessageHook больше не нужен.
// - ActionBase больше не знает напрямую про NavBarItem, BarItem, SimpleButton.
// - Runtime reflection убран из основного потока синхронизации.
// - Код специально подробно прокомментирован.
// ============================================================================
using System.ComponentModel;
using System.Drawing.Design;
namespace GH.Components;
// ============================================================================
// ActionBase
// ----------------------------------------------------------------------------
// Базовое действие.
//
// Это аналог TAction из Delphi:
// - Caption      -> текст на кнопке/пункте меню
// - Enabled      -> доступность
// - Visible      -> видимость
// - Checked      -> checked-state
// - CheckOnClick -> переключать Checked при клике
// - ShortcutKeys -> горячая клавиша
// - Execute      -> событие выполнения
// - Update       -> событие обновления состояния
//
// Главное отличие от старого варианта:
// ActionBase НЕ хранит targets и НЕ обновляет контролы напрямую.
// Это делает ActionList через IActionBinding.
// ============================================================================
[StandardAction]
[DefaultEvent(nameof(Execute))]
[ToolboxBitmap(typeof(ActionBase), "Images.Action.bmp")]
[Designer(typeof(ActionBaseDesigner))]
public class ActionBase : Component
{
    private string _caption = string.Empty;
    private string _category = "none";
    private string _toolTipText = string.Empty;
    private bool _enabled = true;
    private bool _visible = true;
    private bool _checked;
    private bool _checkOnClick;
    private Keys _shortcutKeys = Keys.None;
    private Image? _image;
    private Image? _largeImage;
    private object? _tag;
    private ActionList? _actionList;
    public ActionBase()
    {
        // По умолчанию Caption равен имени типа.
        // Если designer назначит имя Action1, Action2, это имя будет именем компонента,
        // а Caption можно будет отдельно менять в PropertyGrid.
        _caption = GetType().Name;
    }
    public ActionBase(IContainer container) : this()
    {
        // Такой конструктор нужен WinForms designer-у.
        // Он позволяет designer-у сериализовать:
        // this.action1 = new SomeAction(this.components);
        container.Add(this);
    }
    // ActionList устанавливает это свойство, когда действие добавляется в коллекцию.
    // Снаружи менять его нельзя, чтобы одно действие не оказалось в двух списках.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ActionList? ActionList
    {
        get => _actionList;
        internal set => _actionList = value;
    }
    // Произвольные пользовательские данные.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object? Tag
    {
        get => _tag;
        set => SetField(ref _tag, value, nameof(Tag));
    }
    // Текст действия. Обычно попадает в Text/Caption UI-компонента.
    [DefaultValue("")]
    [Localizable(true)]
    public string Caption
    {
        get => _caption;
        set => SetField(ref _caption, value ?? string.Empty, nameof(Caption));
    }
    // Категория нужна для группировки действий в designer/editor.
    [DefaultValue("none")]
    [Localizable(true)]
    public string Category
    {
        get => string.IsNullOrWhiteSpace(_category) ? "none" : _category;
        set => SetField(ref _category, string.IsNullOrWhiteSpace(value) ? "none" : value, nameof(Category));
    }
    // Итоговый Enabled учитывает Enabled самого ActionList.
    // Если весь список отключён, все действия считаются недоступными.
    [DefaultValue(true)]
    public bool Enabled
    {
        get => _enabled && (ActionList?.Enabled ?? true);
        set => SetField(ref _enabled, value, nameof(Enabled));
    }
    // Локальное значение Enabled без учёта ActionList.Enabled.
    // Нужно ActionList-у при пересинхронизации.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal bool LocalEnabled => _enabled;
    [DefaultValue(true)]
    public bool Visible
    {
        get => _visible;
        set => SetField(ref _visible, value, nameof(Visible));
    }
    [DefaultValue(false)]
    public bool Checked
    {
        get => _checked;
        set => SetField(ref _checked, value, nameof(Checked));
    }
    [DefaultValue(false)]
    public bool CheckOnClick
    {
        get => _checkOnClick;
        set => SetField(ref _checkOnClick, value, nameof(CheckOnClick));
    }
    [DefaultValue(Keys.None)]
    [Localizable(true)]
    public Keys ShortcutKeys
    {
        get => _shortcutKeys;
        set => SetField(ref _shortcutKeys, value, nameof(ShortcutKeys));
    }
    [DefaultValue(null)]
    [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v25.2", typeof(UITypeEditor))]
    public virtual Image? Image
    {
        get => _image;
        set => SetField(ref _image, value, nameof(Image));
    }
    [DefaultValue(null)]
    [Editor("DevExpress.Utils.Design.DXImageEditor, DevExpress.Design.v25.2", typeof(UITypeEditor))]
    public virtual Image? LargeImage
    {
        get => _largeImage;
        set => SetField(ref _largeImage, value, nameof(LargeImage));
    }
    [DefaultValue("")]
    [Localizable(true)]
    [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
    public string ToolTipText
    {
        get => _toolTipText;
        set => SetField(ref _toolTipText, value ?? string.Empty, nameof(ToolTipText));
    }
    // Событие вызывается при изменении любого свойства действия.
    // Binding-и слушают это событие и обновляют UI.
    public event EventHandler<ActionChangedEventArgs>? Changed;
    // Перед выполнением можно отменить действие.
    public event CancelEventHandler? BeforeExecute;
    // Основное событие выполнения действия.
    public event EventHandler? Execute;
    // После выполнения.
    public event EventHandler? AfterExecute;
    // Событие для пересчёта состояния действия.
    // Например: action.Enabled = grid.SelectedRows.Count > 0.
    public event EventHandler? Update;
    // Явный вызов обновления состояния.
    public void DoUpdate()
    {
        OnUpdate(EventArgs.Empty);
    }
    // Выполнение действия пользователем или shortcut-ом.
    public void DoExecute()
    {
        if (!Enabled)
        {
            return;
        }
        if (CheckOnClick)
        {
            Checked = !Checked;
        }
        var args = new CancelEventArgs();
        OnBeforeExecute(args);
        if (args.Cancel)
        {
            return;
        }
        OnExecute(EventArgs.Empty);
        OnAfterExecute(EventArgs.Empty);
    }
    protected virtual void OnBeforeExecute(CancelEventArgs e)
    {
        BeforeExecute?.Invoke(this, e);
    }
    protected virtual void OnExecute(EventArgs e)
    {
        Execute?.Invoke(this, e);
    }
    protected virtual void OnAfterExecute(EventArgs e)
    {
        AfterExecute?.Invoke(this, e);
    }
    protected virtual void OnUpdate(EventArgs e)
    {
        Update?.Invoke(this, e);
    }
    protected virtual void OnChanged(string propertyName)
    {
        Changed?.Invoke(this, new ActionChangedEventArgs(propertyName));
    }
    // Универсальный helper для setter-ов.
    // Он не даёт генерировать лишние Changed-события.
    protected bool SetField<T>(ref T field, T value, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }
        field = value;
        OnChanged(propertyName);
        return true;
    }
    internal void RaiseEffectiveEnabledChanged()
    {
        // Когда меняется ActionList.Enabled, локальное поле _enabled не меняется,
        // но итоговый Enabled меняется. Поэтому ActionList вызывает этот метод.
        OnChanged(nameof(Enabled));
    }
    internal void ExecuteShortcut()
    {
        if (!Enabled)
            return;
        if (CheckOnClick)
            Checked = !Checked;
        DoExecute();
    }
}
