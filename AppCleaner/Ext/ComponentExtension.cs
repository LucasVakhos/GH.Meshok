using System.ComponentModel;
using System.Diagnostics;
namespace AppCleaner
{
    public static class ComponentExtension
    {
        // Вычисляем режим дизайна динамически
        public static bool IsDesignMode(this Component component)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;
            var processName = Process.GetCurrentProcess().ProcessName.ToLowerInvariant();
            return new[] { "devenv", "vcsexpress", "vbexpress", "vcexpress", "wdexpress", "sharpdevelop" }
                   .Contains(processName);
        }
    }
}
