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
// ToolStripItemActionBinding
// ----------------------------------------------------------------------------
// Binding для ToolStripItem / ToolStripMenuItem.
// ============================================================================
public sealed class ToolStripItemActionBinding : ActionBindingBase<ToolStripItem>
{
    public ToolStripItemActionBinding(ActionBase action, ToolStripItem target) : base(action, target)
    {
        TargetComponent.Click += Target_Click;
        if (TargetComponent is ToolStripMenuItem menuItem)
        {
            menuItem.CheckedChanged += MenuItem_CheckedChanged;
        }
        Synchronize();
    }
    protected override void ApplyAll()
    {
        TargetComponent.Text = Action.Caption;
        TargetComponent.Enabled = Action.Enabled;
        TargetComponent.Visible = Action.Visible;
        TargetComponent.Image = Action.Image;
        TargetComponent.ToolTipText = Action.ToolTipText;
        TargetComponent.AutoToolTip = string.IsNullOrWhiteSpace(Action.ToolTipText);
        if (TargetComponent is ToolStripMenuItem menuItem)
        {
            menuItem.Checked = Action.Checked;
        }
    }
    protected override void ApplyProperty(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ActionBase.Caption):
                TargetComponent.Text = Action.Caption;
                break;
            case nameof(ActionBase.Enabled):
                TargetComponent.Enabled = Action.Enabled;
                break;
            case nameof(ActionBase.Visible):
                TargetComponent.Visible = Action.Visible;
                break;
            case nameof(ActionBase.Image):
                TargetComponent.Image = Action.Image;
                break;
            case nameof(ActionBase.ToolTipText):
                TargetComponent.ToolTipText = Action.ToolTipText;
                TargetComponent.AutoToolTip = string.IsNullOrWhiteSpace(Action.ToolTipText);
                break;
            case nameof(ActionBase.Checked):
                if (TargetComponent is ToolStripMenuItem menuItem)
                {
                    menuItem.Checked = Action.Checked;
                }
                break;
            default:
                ApplyAll();
                break;
        }
    }
    private void Target_Click(object? sender, EventArgs e)
    {
        if (!Updating)
        {
            Action.DoExecute();
        }
    }
    private void MenuItem_CheckedChanged(object? sender, EventArgs e)
    {
        if (!Updating && sender is ToolStripMenuItem menuItem)
        {
            Action.Checked = menuItem.Checked;
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TargetComponent.Click -= Target_Click;
            if (TargetComponent is ToolStripMenuItem menuItem)
            {
                menuItem.CheckedChanged -= MenuItem_CheckedChanged;
            }
        }
        base.Dispose(disposing);
    }
}
