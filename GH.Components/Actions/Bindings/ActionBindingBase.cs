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
namespace GH.Components;
// ============================================================================
// ActionBindingBase<TTarget>
// ----------------------------------------------------------------------------
// Базовый класс для всех binding-ов.
//
// Он:
// - хранит Action и Target;
// - слушает Action.Changed;
// - вызывает Synchronize(propertyName);
// - предотвращает рекурсивные обновления через _updating.
// ============================================================================
public abstract class ActionBindingBase<TTarget> : IActionBinding where TTarget : Component
{
    private bool _disposed;
    private bool _updating;
    protected ActionBindingBase(ActionBase action, TTarget target)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
        TargetComponent = target ?? throw new ArgumentNullException(nameof(target));
        Action.Changed += Action_Changed;
    }
    public ActionBase Action { get; }
    public TTarget TargetComponent { get; }
    public Component Target => TargetComponent;
    protected bool Updating => _updating;
    public void Synchronize()
    {
        if (_disposed)
        {
            return;
        }
        _updating = true;
        try
        {
            ApplyAll();
        }
        finally
        {
            _updating = false;
        }
    }
    public void Synchronize(string propertyName)
    {
        if (_disposed)
        {
            return;
        }
        _updating = true;
        try
        {
            ApplyProperty(propertyName);
        }
        finally
        {
            _updating = false;
        }
    }
    protected abstract void ApplyAll();
    protected abstract void ApplyProperty(string propertyName);
    private void Action_Changed(object? sender, ActionChangedEventArgs e)
    {
        Synchronize(e.PropertyName);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            Action.Changed -= Action_Changed;
        }
        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
