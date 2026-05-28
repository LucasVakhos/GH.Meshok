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
// IActionBinding
// ----------------------------------------------------------------------------
// Контракт связи между ActionBase и конкретным UI-компонентом.
//
// Binding отвечает за:
// - подписку на события UI;
// - вызов action.DoExecute();
// - перенос Action-свойств в UI;
// - отписку от событий в Dispose.
// ============================================================================
public interface IActionBinding : IDisposable
{
    ActionBase Action { get; }
    Component Target { get; }
    void Synchronize();
    void Synchronize(string propertyName);
}
