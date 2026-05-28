using DevExpress.XtraEditors;
namespace GH.Components
{
    public static class DlgHelper
    {
        private static Form mainForm;
        public static Form MainForm
        {
            get
            {
                if (mainForm == null)
                    mainForm = Application.OpenForms[0];
                return mainForm;
            }
        }
        public static DialogResult DialogResult { get; private set; }
        private static void MessageBox(object param)
        {
            XtraMessageBoxArgs args = param as XtraMessageBoxArgs;
            DialogResult = XtraMessageBox.Show(args);
        }
        private static XtraMessageBoxArgs GetArgs(string message, Icon icon, bool yesNo = false)
        {
            XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            args.Caption = RunContext.AppMainForm == null ? RunContext.AppCaption : RunContext.AppMainForm.Text;
            args.Icon = icon;
            args.Text = message;
            if (yesNo)
            {
                args.Buttons = new DialogResult[2];
                args.Buttons[0] = DialogResult.Yes;
                args.Buttons[1] = DialogResult.No;
                args.DefaultButtonIndex = 1;
            }
            else
            {
                args.Buttons = new DialogResult[1];
                args.Buttons[0] = DialogResult.OK;
                args.DefaultButtonIndex = 0;
            }
            args.Owner = RunContext.AppMainForm;
            return args;
        }
        public static bool DlgYesNo(string message)
        {
            XtraMessageBoxArgs args = GetArgs(message, SystemIcons.Question, true);
            RunContext.Send(MessageBox, args);
            return DialogResult == DialogResult.Yes;
        }
        public static void DlgInfo(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Information));
        }
        public static void DlgWarning(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Warning));
        }
        public static void DlgError(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Error));
        }
        private static void VoidDlg(XtraMessageBoxArgs args)
        {
            RunContext.Send(MessageBox, args);
        }
    }
}
