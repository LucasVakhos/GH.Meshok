namespace AppCleaner
{
    public static class FormIniExtension
    {
        private const string Section = "MainForm";

        public static void SaveState(this Form form, string filePath)
        {
            var ini = new IniFile(filePath);

            var bounds = form.WindowState == FormWindowState.Normal
                ? form.Bounds
                : form.RestoreBounds;

            ini.Write(Section, "X", bounds.X);
            ini.Write(Section, "Y", bounds.Y);
            ini.Write(Section, "Width", bounds.Width);
            ini.Write(Section, "Height", bounds.Height);
            ini.Write(Section, "WindowState", form.WindowState);

            ini.Save();
        }

        public static void LoadState(this Form form, string filePath)
        {
            var ini = new IniFile(filePath);

            if (!int.TryParse(ini.Read(Section, "X"), out var x) ||
                !int.TryParse(ini.Read(Section, "Y"), out var y) ||
                !int.TryParse(ini.Read(Section, "Width"), out var width) ||
                !int.TryParse(ini.Read(Section, "Height"), out var height))
                return;

            form.StartPosition = FormStartPosition.Manual;
            form.Bounds = new Rectangle(x, y, width, height);

            if (Enum.TryParse<FormWindowState>(
                    ini.Read(Section, "WindowState"),
                    out var state))
            {
                form.WindowState = state;
            }
        }
    }
}
