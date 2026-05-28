using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
namespace AppCleaner.Ext
{
    public static class CountersHelpers
    {
        private static readonly string _logFilePath = Path.Combine(Application.StartupPath, "namespaces_collect.log.txt");
        private static readonly object _logFileLock = new object();
        private static int _foldersProcessed;
        private static int _filesProcessed;
        private static ConcurrentDictionary<string, int> _processedFolders = new ConcurrentDictionary<string, int>();
        public static int FilesProcessed => Volatile.Read(ref _filesProcessed);
        public static int FoldersProcessed => Volatile.Read(ref _foldersProcessed);
        internal static void Reset()
        {
            _foldersProcessed = 0;
            _filesProcessed = 0;
            _processedFolders.Clear();
        }
        internal static void IncFiles()
        {
            Interlocked.Increment(ref _filesProcessed);
        }
        internal static void IncFolders(string folder)
        {
            if (_processedFolders.TryAdd(folder, 0))
            {
                Interlocked.Increment(ref _foldersProcessed);
            }
        }
        internal static void AddToLog(string logLine)
        {
            try
            {
                lock (_logFileLock)
                {
                    File.AppendAllText(_logFilePath,
                        logLine + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch { /* игнорируем */ }
        }
        internal static void ShowLog()
        {
            // По завершении можно открыть лог (опционально)
            try
            {
                if (File.Exists(_logFilePath))
                {
                    Process.Start(new ProcessStartInfo(_logFilePath) { UseShellExecute = true });
                }
            }
            catch { /* игнорируем, если открыть нельзя */ }
        }
    }
}
