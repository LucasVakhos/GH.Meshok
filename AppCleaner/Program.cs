using System.Reflection;
namespace AppCleaner;
static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        //Console.WriteLine("=== Запуск приложения ===");
        //Console.WriteLine($"Рабочая директория: {AppDomain.CurrentDomain.BaseDirectory}");
        //Console.WriteLine($"Имя сборки: {Assembly.GetEntryAssembly()?.GetName().Name}");
        //AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
        //{
        //    try
        //    {
        //        string assemblyName = new AssemblyName(e.Name).Name;
        //        string libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs");
        //        string assemblyPath = Path.Combine(libsPath, $"{assemblyName}.dll");
        //        Console.WriteLine($"=== AssemblyResolve: Поиск сборки '{e.Name}' ===");
        //        Console.WriteLine($"Ожидаемый путь: {assemblyPath}");
        //        if (File.Exists(assemblyPath))
        //        {
        //            Console.WriteLine($"Файл найден. Загрузка...");
        //            var assembly = Assembly.LoadFrom(assemblyPath);
        //            Console.WriteLine($"Сборка успешно загружена: {assembly.FullName}");
        //            return assembly;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Файл не найден в папке Libs.");
        //        }
        //    }
        //    catch (BadImageFormatException bifEx)
        //    {
        //        Console.Error.WriteLine($"Ошибка формата сборки {e.Name}: {bifEx.Message}");
        //    }
        //    catch (FileLoadException flEx)
        //    {
        //        Console.Error.WriteLine($"Ошибка загрузки сборки {e.Name}: {flEx.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine($"Неожиданная ошибка при загрузке {e.Name}: {ex.Message}");
        //    }
        //    return null;
        //};
        // Настройка конфигурации приложения
        //Add the following code to a method that starts the application.

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
