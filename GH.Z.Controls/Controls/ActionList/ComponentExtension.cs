using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GH.Components
{
    public static class ComponentExtension
    {
        private static bool _isDesignMode = new List<string>() {
                "devenv",
                "vcsexpress",
                "vbexpress",
                "vcexpress",
                "wdexpress",
                "sharpdevelop"
                }.Contains(Process.GetCurrentProcess().ProcessName.ToLowerInvariant());

        public static bool IsDesignMode(this Component obj) => _isDesignMode;
    }
}
