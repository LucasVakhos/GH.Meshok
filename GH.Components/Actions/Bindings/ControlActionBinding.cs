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
// ControlActionBinding
// ----------------------------------------------------------------------------
// Binding для обычных WinForms Control.
// Подходит для Button, CheckBox и большинства обычных контролов.
// ============================================================================
public sealed class ControlActionBinding : ActionBindingBase<Control>
{
    private readonly ToolTip? _toolTip;
    public ControlActionBinding(ActionBase action, Control target, ToolTip? toolTip) : base(action, target)
    {
        _toolTip = toolTip;
        TargetComponent.Click += Target_Click;
        if (TargetComponent is CheckBox checkBox)
        {
            checkBox.CheckedChanged += CheckBox_CheckedChanged;
        }
        Synchronize();
    }
    protected override void ApplyAll()
    {
        TargetComponent.Text = Action.Caption;
        TargetComponent.Enabled = Action.Enabled;
        TargetComponent.Visible = Action.Visible;
        if (TargetComponent is ButtonBase buttonBase)
        {
            buttonBase.Image = Action.Image;
        }
        if (TargetComponent is CheckBox checkBox)
        {
            checkBox.Checked = Action.Checked;
        }
        _toolTip?.SetToolTip(TargetComponent, Action.ToolTipText);
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
                if (TargetComponent is ButtonBase buttonBase)
                {
                    buttonBase.Image = Action.Image;
                }
                break;
            case nameof(ActionBase.Checked):
                if (TargetComponent is CheckBox checkBox)
                {
                    checkBox.Checked = Action.Checked;
                }
                break;
            case nameof(ActionBase.ToolTipText):
                _toolTip?.SetToolTip(TargetComponent, Action.ToolTipText);
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
    private void CheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (!Updating && sender is CheckBox checkBox)
        {
            Action.Checked = checkBox.Checked;
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TargetComponent.Click -= Target_Click;
            if (TargetComponent is CheckBox checkBox)
            {
                checkBox.CheckedChanged -= CheckBox_CheckedChanged;
            }
        }
        base.Dispose(disposing);
    }
}
