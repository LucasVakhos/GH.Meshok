using System.ComponentModel;
using System.Diagnostics;
namespace GH.Components
{
    public static class FileHelper
    {
        public static void OpenUrl(string link)
        {
            try
            {
                FileHelper.OpenViaProcess(link);
            }
            catch (GH.ComponentsException ex)
            {
                DlgHelper.DlgError("Не возможно открыть url: " + link + Environment.NewLine + ex.Message);
            }
        }
        public static void OpenFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            try
            {
                if (DlgHelper.DlgYesNo($"Желаете открыть {Path.GetFileName(fileName)}?"))
                    FileHelper.OpenViaProcess(fileName);
                else
                    FileHelper.OpenFolder(fileName);
            }
            catch (GH.ComponentsException ex)
            {
                DlgHelper.DlgError("Не возможно открыть файл: " + fileName + Environment.NewLine + ex.Message);
            }
        }
        public static void OpenFolder(string fileName)
        {
            try
            {
                new Process()
                {
                    StartInfo = {
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Normal,
                        FileName = "explorer",
                        WorkingDirectory = Path.GetDirectoryName(fileName),
                        Arguments = @"/n, /select, " + fileName,
                        UseShellExecute = true,
                    }
                }.Start();
            }
            catch (Win32Exception ex)
            {
                throw new GH.ComponentsException(ex.Message);
            }
        }
        public static void OpenViaProcess(string fileName)
        {
            try
            {
                new Process()
                {
                    StartInfo = {
                        FileName = fileName,
                        Verb = "Open",
                        WindowStyle = ProcessWindowStyle.Normal
                    }
                }.Start();
            }
            catch (Win32Exception ex)
            {
                throw new GH.ComponentsException(ex.Message);
            }
        }
        public static bool IsCompressed(string pad)
        {
            return (File.GetAttributes(pad) & FileAttributes.Compressed) == FileAttributes.Compressed;
        }
    }
}
