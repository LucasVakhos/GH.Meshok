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
namespace GH.Components;
// ============================================================================
// BarItemActionBinding
// ----------------------------------------------------------------------------
// Binding для DevExpress BarItem / BarButtonItem / BarSubItem.
// ============================================================================
public sealed class BarItemActionBinding : ActionBindingBase<BarItem>
{
    public BarItemActionBinding(ActionBase action, BarItem target) : base(action, target)
    {
        TargetComponent.ItemClick += Target_ItemClick;
        Synchronize();
    }
    protected override void ApplyAll()
    {
        TargetComponent.Caption = Action.Caption;
        TargetComponent.Enabled = Action.Enabled;
        TargetComponent.Visibility = Action.Visible ? BarItemVisibility.Always : BarItemVisibility.Never;
        TargetComponent.Hint = Action.ToolTipText;
        if (TargetComponent is BarButtonItem buttonItem)
        {
            buttonItem.Glyph = Action.Image;
            buttonItem.LargeGlyph = Action.LargeImage;
            buttonItem.Down = Action.Checked;
        }
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
                TargetComponent.Visibility = Action.Visible ? BarItemVisibility.Always : BarItemVisibility.Never;
                break;
            case nameof(ActionBase.ToolTipText):
                TargetComponent.Hint = Action.ToolTipText;
                break;
            case nameof(ActionBase.Image):
                if (TargetComponent is BarButtonItem buttonItem1)
                {
                    buttonItem1.Glyph = Action.Image;
                }
                break;
            case nameof(ActionBase.LargeImage):
                if (TargetComponent is BarButtonItem buttonItem2)
                {
                    buttonItem2.LargeGlyph = Action.LargeImage;
                }
                break;
            case nameof(ActionBase.Checked):
                if (TargetComponent is BarButtonItem buttonItem3)
                {
                    buttonItem3.Down = Action.Checked;
                }
                break;
            default:
                ApplyAll();
                break;
        }
    }
    private void Target_ItemClick(object sender, ItemClickEventArgs e)
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
            TargetComponent.ItemClick -= Target_ItemClick;
        }
        base.Dispose(disposing);
    }
}
