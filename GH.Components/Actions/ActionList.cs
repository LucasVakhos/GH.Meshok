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
using GH.Components;
using System.ComponentModel;
using System.ComponentModel.Design;
namespace GH.Components;
// ============================================================================
// ActionList
// ----------------------------------------------------------------------------
// Аналог Delphi TActionList.
//
// Возможности:
// - хранит Actions;
// - является ExtenderProvider-ом: у поддерживаемых компонентов появляется
//   свойство Action;
// - создаёт binding Action <-> target;
// - обрабатывает shortcuts через ProcessCmdKey/KeyDown владельца;
// - централизованно вызывает DoUpdate.
//
// Как использовать:
// 1. Положить ActionList на форму.
// 2. Добавить Actions в коллекцию.
// 3. У кнопки/BarItem/NavBarItem выбрать свойство Action.
// ============================================================================
[ToolboxItemFilter("System.Windows.Forms")]
[ProvideProperty("Action", typeof(Component))]
[ToolboxBitmap(typeof(ActionList), "Images.ActionList.bmp")]
[DefaultProperty(nameof(Actions))]
public class ActionList : Component, IExtenderProvider, ISupportInitialize
{
    private readonly ActionCollection _actions;
    private readonly Dictionary<Component, ActionBase> _targetToAction = new();
    private readonly Dictionary<Component, IActionBinding> _bindings = new();
    private readonly ToolTip _toolTip = new();
    private bool _enabled = true;
    private bool _initializing;
    private ContainerControl? _owner;
    private bool _listenKeyDown = true;
    public ActionList()
    {
        _actions = new ActionCollection(this);
    }
    public ActionList(IContainer container) : this()
    {
        container.Add(this);
    }
    // Глобальное включение/выключение всех действий списка.
    [DefaultValue(true)]
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled == value)
            {
                return;
            }
            _enabled = value;
            // Изменился effective Enabled у всех действий.
            foreach (var action in Actions)
            {
                action.RaiseEffectiveEnabledChanged();
            }
        }
    }
    // Если true, ActionList слушает KeyDown владельца и выполняет action shortcuts.
    [DefaultValue(true)]
    public bool ListenKeyDown
    {
        get => _listenKeyDown;
        set => _listenKeyDown = value;
    }
    // WinForms ToolTip используется binding-ами для обычных Control/BaseButton.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ToolTip ToolTip => _toolTip;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ActionCollection Actions => _actions;
    // Владелец нужен для shortcut-ов.
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ContainerControl? Owner
    {
        get => _owner;
        set
        {
            if (_owner == value)
            {
                return;
            }
            if (_owner is not null)
            {
                _owner.KeyDown -= Owner_KeyDown;
            }
            _owner = value;
            if (_owner is not null)
            {
                _owner.KeyDown += Owner_KeyDown;
            }
        }
    }
    // Событие общего обновления ActionList.
    public event EventHandler? Update;
    public void BeginInit()
    {
        _initializing = true;
    }
    public void EndInit()
    {
        _initializing = false;
        ValidateBindings();
        DoUpdate();
        SynchronizeAll();
    }
    // Extender property getter: показывает выбранное действие для компонента.
    [DefaultValue(null)]
    [Category("Action")]
    [Description("Действие, связанное с этим компонентом.")]
    public ActionBase? GetAction(Component target)
    {
        ArgumentNullException.ThrowIfNull(target);
        return _targetToAction.TryGetValue(target, out var action) ? action : null;
    }
    // Extender property setter: назначает или снимает действие с компонента.
    public void SetAction(Component target, ActionBase? action)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (!_initializing && action is not null && action.ActionList != this)
        {
            throw new ArgumentException("Action должен принадлежать этому ActionList.", nameof(action));
        }
        RemoveBinding(target);
        if (action is null)
        {
            return;
        }
        if (!ActionBindingFactory.CanBind(target))
        {
            throw new NotSupportedException($"Тип target не поддерживается: {target.GetType().FullName}");
        }
        _targetToAction[target] = action;
        _bindings[target] = ActionBindingFactory.Create(action, target, _toolTip);
    }
    // IExtenderProvider решает, каким компонентам designer покажет свойство Action.
    bool IExtenderProvider.CanExtend(object extendee)
    {
        return extendee is Component component && ActionBindingFactory.CanBind(component);
    }
    // Вызвать Update у списка и у всех действий.
    public void DoUpdate()
    {
        Update?.Invoke(this, EventArgs.Empty);
        foreach (var action in Actions)
        {
            action.DoUpdate();
        }
    }
    // Синхронизировать все UI targets с текущим состоянием actions.
    public void SynchronizeAll()
    {
        foreach (var binding in _bindings.Values)
        {
            binding.Synchronize();
        }
    }
    // Обработка shortcut-ов.
    // Возвращает true, если shortcut найден и выполнен.
    public bool ProcessShortcut(Keys keyData)
    {
        if (!Enabled)
        {
            return false;
        }
        var action = Actions.FirstOrDefault(x => x.ShortcutKeys == keyData && x.Enabled);
        if (action is null)
        {
            return false;
        }
        action.DoExecute();
        return true;
    }
    internal void OnActionAdded(ActionBase action)
    {
        // Место для будущих hooks.
        // Сейчас дополнительных действий не нужно.
    }
    internal void OnActionRemoved(ActionBase action)
    {
        // Место для будущих hooks.
        // Сейчас дополнительных действий не нужно.
    }
    internal void UnbindAction(ActionBase action)
    {
        var targets = _targetToAction
            .Where(pair => ReferenceEquals(pair.Value, action))
            .Select(pair => pair.Key)
            .ToArray();
        foreach (var target in targets)
        {
            RemoveBinding(target);
        }
    }
    private void Owner_KeyDown(object? sender, KeyEventArgs e)
    {
        if (!ListenKeyDown)
        {
            return;
        }
        if (ProcessShortcut(e.KeyData))
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
    private void RemoveBinding(Component target)
    {
        if (_bindings.Remove(target, out var binding))
        {
            binding.Dispose();
        }
        _targetToAction.Remove(target);
    }
    private void ValidateBindings()
    {
        foreach (var action in _targetToAction.Values)
        {
            if (!Actions.Contains(action))
            {
                throw new InvalidOperationException("Target ссылается на Action, которого нет в ActionList.Actions.");
            }
        }
    }
    // Designer сам устанавливает Site.
    // Через Site можно найти root component формы и назначить Owner.
    public override ISite? Site
    {
        get => base.Site;
        set
        {
            base.Site = value;
            if (value?.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost
                && designerHost.RootComponent is ContainerControl container)
            {
                Owner = container;
            }
        }
    }
    [Browsable(false)]
    public bool Active
    {
        get
        {
            if (_owner == null)
                return false;
            return _owner.CanFocus;
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Owner = null;
            foreach (var binding in _bindings.Values.ToArray())
            {
                binding.Dispose();
            }
            _bindings.Clear();
            _targetToAction.Clear();
            _toolTip.Dispose();
        }
        base.Dispose(disposing);
    }
    public void CheckShortcuts(object sender, KeyEventArgs e)
    {
        if (!ListenKeyDown)
            return;
        foreach (ActionBase action in _actions.Where(x => x.ShortcutKeys == e.KeyData))
        {
            action.ExecuteShortcut();
        }
    }
}
