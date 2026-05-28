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
using DevExpress.XtraNavBar;
namespace GH.Components;
// ============================================================================
// NavBarItemActionBinding
// ----------------------------------------------------------------------------
// Binding для DevExpress NavBarItem.
// ============================================================================
public sealed class NavBarItemActionBinding : ActionBindingBase<NavBarItem>
{
    public NavBarItemActionBinding(ActionBase action, NavBarItem target) : base(action, target)
    {
        TargetComponent.LinkClicked += Target_LinkClicked;
        Synchronize();
    }
    protected override void ApplyAll()
    {
        TargetComponent.Caption = Action.Caption;
        TargetComponent.Enabled = Action.Enabled;
        TargetComponent.Visible = Action.Visible;
        TargetComponent.Hint = Action.ToolTipText;
        TargetComponent.SmallImage = Action.Image;
        TargetComponent.LargeImage = Action.LargeImage;
    }
    protected override void ApplyProperty(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ActionBase.Caption):
                TargetComponent.Caption = Action.Caption;
                break;
            case nameof(ActionBase.Enabled):
                TargetComponent.Enabled = Action.Enabled;
                break;
            case nameof(ActionBase.Visible):
                TargetComponent.Visible = Action.Visible;
                break;
            case nameof(ActionBase.ToolTipText):
                TargetComponent.Hint = Action.ToolTipText;
                break;
            case nameof(ActionBase.Image):
                TargetComponent.SmallImage = Action.Image;
                break;
            case nameof(ActionBase.LargeImage):
                TargetComponent.LargeImage = Action.LargeImage;
                break;
            default:
                ApplyAll();
                break;
        }
    }
    private void Target_LinkClicked(object sender, NavBarLinkEventArgs e)
    {
        if (!Updating)
        {
            Action.DoExecute();
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TargetComponent.LinkClicked -= Target_LinkClicked;
        }
        base.Dispose(disposing);
    }
}
