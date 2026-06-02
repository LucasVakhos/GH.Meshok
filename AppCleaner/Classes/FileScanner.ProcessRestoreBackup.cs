using System.Text.RegularExpressions;

namespace AppCleaner
{
    public partial class FileScanner
    {
        private async Task RestoreCSharpFilesFromBakFolderAsync(CancellationToken cancellationToken)
        {
            var backupFiles = Directory
                .EnumerateFiles(_store.SearchFolder, "*.cs.*.bak", SearchOption.AllDirectories)
                .Where(file => !IsDesignerFile(file))
                .Where(IsTimestampedCSharpBackup)
                .GroupBy(GetTargetFilePathFromTimestampedBak)
                .Select(group => group.OrderByDescending(File.GetCreationTimeUtc).First())
                .ToArray();

            _store.SetProgressMaximum(backupFiles.Length);
            AddToLog($"Файлов .cs.*.bak для восстановления: {backupFiles.Length}");

            foreach (var backupFile in backupFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    if (await RestoreCSharpFileFromBakAsync(backupFile, cancellationToken))
                        AddToLog($"[Восстановлен] {backupFile}");
                    else
                        AddToLog($"[Пропуск] {backupFile}");
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка восстановления] {backupFile} - {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(backupFile);
                }
            }

            AddToLog("Восстановление .cs из последнего timestamp .bak завершено.");
        }

        private async Task<bool> RestoreCSharpFileFromBakAsync(string backupFilePath, CancellationToken cancellationToken)
        {
            if (!IsTimestampedCSharpBackup(backupFilePath))
                return false;

            if (!File.Exists(backupFilePath))
                return false;

            var targetFilePath = GetTargetFilePathFromTimestampedBak(backupFilePath);
            var encoding = DetectFileEncoding(backupFilePath);
            var backupSource = await File.ReadAllTextAsync(backupFilePath, encoding, cancellationToken);

            await File.WriteAllTextAsync(targetFilePath, backupSource, encoding, cancellationToken);

            File.Delete(backupFilePath);

            return true;
        }

        private static bool IsTimestampedCSharpBackup(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            return Regex.IsMatch(
                fileName,
                @"^.+\.cs\.\d{8}_\d{6}\.bak$",
                RegexOptions.IgnoreCase);
        }

        private static string GetTargetFilePathFromTimestampedBak(string backupFilePath)
        {
            return Regex.Replace(
                backupFilePath,
                @"\.cs\.\d{8}_\d{6}\.bak$",
                ".cs",
                RegexOptions.IgnoreCase);
        }
    }
}