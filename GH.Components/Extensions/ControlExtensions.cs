using System;
using System.Diagnostics.CodeAnalysis;
    public static class ControlExtensions
{
    /// <summary>
    /// Рекурсивно ищет первый подходящий для фокуса контрол внутри parent.
    /// Критерий: Visible && Enabled && TabStop (можно расширить).
    /// Возвращает null, если ничего не найдено.
    /// </summary>
    public static Control GetFirstFocusableChild(this Control parent)
    {
        if (parent == null) return null;
        // Сначала ищем среди прямых дочерних элементов
        foreach (Control c in parent.Controls)
        {
            if (IsFocusableCandidate(c)) return c;
        }
        // Затем рекурсивно
        foreach (Control c in parent.Controls)
        {
            var nested = c.GetFirstFocusableChild();
            if (nested != null) return nested;
        }
        return null;
    }
    private static bool IsFocusableCandidate(Control c)
    {
        // TabStop покрывает большинство интерактивных контролов; можно добавить дополнительные типы
        return c != null && c.Visible && c.Enabled && c.TabStop;
    }
    /// <summary>
    /// Безопасно ставит фокус на первый найденный фокусируемый дочерний контрол (или на сам sender, если ничего не найдено).
    /// Вызывать из обработчика события: SafeFocusFirstChild(sender as Control);
    /// </summary>
    public static void SafeFocusFirstChild(this Control parent)
    {
        if (parent == null) return;
        // действие, выполняемое в UI-потоке
        Action focusAction = () =>
        {
            try
            {
                var target = parent.GetFirstFocusableChild() ?? parent;
                // Попробуем Focus(); если не сработает, попробуем Select()
                if (!target.Focused)
                {
                    target.Focus();
                    if (!target.Focused)
                    {
                        target.Select();
                    }
                }
            }
            catch
            {
                // не фейлим приложение из-за фокуса
            }
        };
        if (parent.IsHandleCreated && parent.InvokeRequired)
            parent.BeginInvoke(focusAction);
        else
            focusAction();
    }
    public static void SafeSetText(this Control ctrl, string text)
    {
        if (ctrl == null) return;
        if (ctrl.InvokeRequired)
            ctrl.BeginInvoke(new Action(() => ctrl.Text = text));
        else
            ctrl.Text = text;
    }
    [return: MaybeNull]
    public static T GetChildControl<T>(this Control parent, string name) where T : Control
    {
        var result = parent.Controls.Find(name, true).FirstOrDefault() as T;
        if (result == null)
            throw new InvalidOperationException($"Control with name '{name}' not found.");
        return result;
    }
    [return: MaybeNull]
    public static T FindControlInParents<T>(this Control control) where T : Control
    {
        var parent = control.Parent;
        while (parent != null)
        {
            if (parent is T found)
                return found;
            parent = parent.Parent;
        }
        return null;
    }
}
