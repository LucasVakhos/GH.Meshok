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
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System.ComponentModel;
namespace GH.Components;
// ============================================================================
// ActionBindingFactory
// ----------------------------------------------------------------------------
// Фабрика выбирает нужный binding для конкретного UI-компонента.
//
// Если нужно поддержать новый контрол — добавьте новый binding и одну ветку здесь.
// ============================================================================
public static class ActionBindingFactory
{
    public static IActionBinding Create(ActionBase action, Component target, ToolTip? toolTip)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(target);
        return target switch
        {
            BarItem barItem => new BarItemActionBinding(action, barItem),
            NavBarItem navBarItem => new NavBarItemActionBinding(action, navBarItem),
            BaseButton baseButton => new SimpleButtonActionBinding(action, baseButton, toolTip),
            ToolStripItem toolStripItem => new ToolStripItemActionBinding(action, toolStripItem),
            Control control => new ControlActionBinding(action, control, toolTip),
            _ => throw new NotSupportedException($"Тип target не поддерживается: {target.GetType().FullName}")
        };
    }
    public static bool CanBind(Component target)
    {
        return target is BarItem
            or NavBarItem
            or BaseButton
            or ToolStripItem
            or Control;
    }
}
