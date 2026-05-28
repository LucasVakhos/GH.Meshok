using GH.Utils;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GH.Helpers
{
    public static class ClipboardHelper
    {
        public static bool TextToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            try
            {
                Clipboard.SetText(text);
                return true;
            }
            catch (ExternalException ex)
            {
                DlgHelper.DlgError("Не возможно поместить текст в Clipboard: " + Environment.NewLine + ex.Message);
                return false;
            }
        }
    }

}
