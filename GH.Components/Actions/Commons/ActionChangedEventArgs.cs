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
namespace GH.Components;
// ============================================================================
// ActionChangedEventArgs
// ----------------------------------------------------------------------------
// Передаёт имя изменившегося свойства.
// Binding может обновлять только нужное свойство UI, а не всё подряд.
// ============================================================================
public sealed class ActionChangedEventArgs : EventArgs
{
    public ActionChangedEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }
    public string PropertyName { get; }
}
