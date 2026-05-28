using System.Reflection;
namespace AppCleaner;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
        {
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Libs",
                dllName);
            if (File.Exists(path))
                return Assembly.LoadFrom(path);
            return null;
        };
        Application.Run(new MainForm());
    }
}
