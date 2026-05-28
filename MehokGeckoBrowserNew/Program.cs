using System;
namespace MeshokBrowser
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode =        DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
            AppContextMB.RunInstance();
        }
    }
}
