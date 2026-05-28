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
using System.ComponentModel.Design;
namespace GH.Components;
// ============================================================================
// ActionBaseDesigner
// ----------------------------------------------------------------------------
// Designer для ActionBase.
//
// Задача:
// - когда действие создаётся в designer-е и у него нет имени,
//   присвоить имя Action1, Action2, Action3...
//
// Почему именно Site.Name:
// - имя компонента в WinForms designer хранится в component.Site.Name;
// - это имя используется для генерации поля в Form.Designer.cs.
// ============================================================================
public sealed class ActionBaseDesigner : ComponentDesigner
{
    public ActionBaseDesigner()
    {
    }
    public override void Initialize(IComponent component)
    {
        base.Initialize(component);
        if (component.Site is null)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(component.Site.Name))
        {
            return;
        }
        if (component.Site.Container is null)
        {
            return;
        }
        component.Site.Name = CreateUniqueActionName(component.Site.Container);
    }
    private static string CreateUniqueActionName(IContainer container)
    {
        var index = 1;
        while (true)
        {
            var candidate = $"Action{index}";
            if (container.Components[candidate] is null)
            {
                return candidate;
            }
            index++;
        }
    }
}
