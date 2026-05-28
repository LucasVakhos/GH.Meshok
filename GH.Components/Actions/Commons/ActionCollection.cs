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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
namespace GH.Components;
// ============================================================================
// ActionCollection
// ----------------------------------------------------------------------------
// Коллекция действий внутри ActionList.
//
// Она гарантирует:
// - действие знает свой ActionList;
// - одно действие не может принадлежать двум ActionList;
// - при удалении связь корректно разрывается.
// ============================================================================
[Editor("GH.Components.Design.ActionCollectionEditor, GH.Components.Design", typeof(UITypeEditor))]
public sealed class ActionCollection : Collection<ActionBase>
{
    private readonly ActionList _owner;
    internal ActionCollection(ActionList owner)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }
    public ActionList Owner => _owner;
    protected override void InsertItem(int index, ActionBase item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureCanOwn(item);
        base.InsertItem(index, item);
        item.ActionList = Owner;
        Owner.OnActionAdded(item);
    }
    protected override void SetItem(int index, ActionBase item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureCanOwn(item);
        var oldItem = this[index];
        Owner.UnbindAction(oldItem);
        oldItem.ActionList = null;
        Owner.OnActionRemoved(oldItem);
        base.SetItem(index, item);
        item.ActionList = Owner;
        Owner.OnActionAdded(item);
    }
    protected override void RemoveItem(int index)
    {
        var oldItem = this[index];
        Owner.UnbindAction(oldItem);
        oldItem.ActionList = null;
        Owner.OnActionRemoved(oldItem);
        base.RemoveItem(index);
    }
    protected override void ClearItems()
    {
        foreach (var action in this.ToArray())
        {
            Owner.UnbindAction(action);
            action.ActionList = null;
            Owner.OnActionRemoved(action);
        }
        base.ClearItems();
    }
    private void EnsureCanOwn(ActionBase action)
    {
        if (action.ActionList is not null && action.ActionList != Owner)
        {
            throw new ArgumentException("Action уже принадлежит другому ActionList.", nameof(action));
        }
    }
}
