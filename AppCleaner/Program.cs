using System.Runtime.Loader;

namespace AppCleaner;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Подключаем resolver для загрузки DLL из папки "libs"
        AssemblyLoadContext.Default.Resolving += (context, assemblyName) =>
        {
            try
            {
                var libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs");
                var assemblyPath = Path.Combine(libsPath, $"{assemblyName.Name}.dll");
                if (File.Exists(assemblyPath))
                {
                    return context.LoadFromAssemblyPath(assemblyPath);
                }
            }
            catch
            {
                // Игнорируем ошибки - попробует загрузить стандартным способом
            }
            return null;
        };

        AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
        {
            try
            {
                var assemblyName = new System.Reflection.AssemblyName(e.Name).Name;
                var libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs");
                var assemblyPath = Path.Combine(libsPath, $"{assemblyName}.dll");
                
                if (File.Exists(assemblyPath))
                {
                    return System.Reflection.Assembly.LoadFrom(assemblyPath);
                }
            }
            catch
            {
                // Игнорируем ошибки - попробует загрузить стандартным способом
            }
            return null;
        };

        // Настройка конфигурации приложения

        //Enables trace source. Remove the following line in the Release version of the project.
        DevExpress.Utils.Localization.XtraLocalizer.EnableTraceSource();

        //Uncomment the following line in a Release version.
        //DevExpress.Utils.Localization.XtraLocalizer.UserResourceManager = DXLocalization.ResourceManager;
        ApplicationConfiguration.Initialize();
        //string libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs");
        //Console.WriteLine("=== Содержимое папки Libs ===");
        //if (Directory.Exists(libsPath))
        //{
        //    foreach (var file in Directory.GetFiles(libsPath, "*.dll"))
        //    {
        //        Console.WriteLine($"  - {Path.GetFileName(file)}");
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Папка Libs не существует!");
        //}
        // Запуск основного окна
        Application.Run(new MainForm());
    }
}
