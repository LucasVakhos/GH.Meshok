using System.Text;
namespace AppCleaner
{
    public static class FilePathExtensions
    {
        /// <summary>
        /// Определяет кодировку файла по BOM; если BOM отсутствует — пытается эвристически отличить UTF-8 (без BOM)
        /// от однобайтовых кодировок (по умолчанию CP1251). При желании можно передать fallback.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="fallback">Если не удалось определить — возвращаем эту кодировку (по умолчанию CP1251 в Windows).</param>
        /// <param name="maxBytesForHeuristics">Сколько байт читать для эвристики (по умолчанию 4096).</param>
        public static Encoding DetectEncodingFromBomOrHeuristic(this string filePath, Encoding fallback = null, int maxBytesForHeuristics = 4096)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                return fallback ?? GetDefaultAnsiEncoding();
            // Резерв: если нужен CP1251 в .NET Core/5+, нужно регистрировать CodePagesEncodingProvider один раз в приложении:
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            fallback ??= GetDefaultAnsiEncoding();
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // читаем до 4 байт для BOM-детекции
                var bom = new byte[4];
                int read = fs.Read(bom, 0, bom.Length);
                // BOM checks
                if (read >= 4 && bom[0] == 0x00 && bom[1] == 0x00 && bom[2] == 0xFE && bom[3] == 0xFF)
                    return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
                if (read >= 4 && bom[0] == 0xFF && bom[1] == 0xFE && bom[2] == 0x00 && bom[3] == 0x00)
                    return new UTF32Encoding(bigEndian: false, byteOrderMark: true);
                if (read >= 3 && bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                    return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                if (read >= 2 && bom[0] == 0xFE && bom[1] == 0xFF)
                    return new UnicodeEncoding(bigEndian: true, byteOrderMark: true);
                if (read >= 2 && bom[0] == 0xFF && bom[1] == 0xFE)
                    return new UnicodeEncoding(bigEndian: false, byteOrderMark: true);
                // BOM не найден — делаем эвристику: читаем несколько килобайт и проверяем на валидность UTF-8
                fs.Position = 0;
                int toRead = (int)Math.Min(maxBytesForHeuristics, fs.Length);
                var buffer = new byte[toRead];
                int n = fs.Read(buffer, 0, toRead);
                if (n > 0)
                {
                    int highBytes = 0;
                    for (int i = 0; i < n; i++)
                        if (buffer[i] >= 0x80) highBytes++;
                    bool validUtf8 = IsValidUtf8(buffer, n);
                    // Если есть многобайтовые символы и последовательность валидна -> UTF-8 без BOM
                    if (validUtf8 && highBytes > 0)
                        return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
                    // Если валидно UTF-8 но нет highBytes (только ASCII) — это скорее ASCII; можно вернуть UTF8 или fallback.
                    if (validUtf8 && highBytes == 0)
                        return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
                    // иначе — полагаем, что это однобайтовая кодировка (CP1251 или указанный fallback)
                    return fallback;
                }
            }
            return fallback;
        }
        // Простая проверка валидности UTF-8 (не делает никаких проб и ошибок, только проверяет корректность байт-последовательностей)
        private static bool IsValidUtf8(byte[] bytes, int count)
        {
            int i = 0;
            while (i < count)
            {
                byte b = bytes[i];
                if (b <= 0x7F)
                {
                    // ASCII
                    i++;
                    continue;
                }
                int expectedContinuation = 0;
                if ((b & 0xE0) == 0xC0) expectedContinuation = 1;        // 110xxxxx
                else if ((b & 0xF0) == 0xE0) expectedContinuation = 2;   // 1110xxxx
                else if ((b & 0xF8) == 0xF0) expectedContinuation = 3;   // 11110xxx
                else
                {
                    // Неправильный ведущий байт
                    return false;
                }
                if (i + expectedContinuation >= count)
                    return false; // обрывается последовательность
                for (int j = 1; j <= expectedContinuation; j++)
                {
                    if ((bytes[i + j] & 0xC0) != 0x80) // проверяем 10xxxxxx
                        return false;
                }
                i += expectedContinuation + 1;
            }
            return true;
        }
        private static Encoding GetDefaultAnsiEncoding()
        {
            // В Windows обычно CP1251 для русскоязычных систем; Encoding.Default может отличаться по платформе.
            try
            {
                // Попытаемся вернуть CP1251 (кодовая страница 1251). В .NET Core требуется регистрация поставщика кодировок.
                return Encoding.GetEncoding(1251);
            }
            catch
            {
                return Encoding.Default;
            }
        }
    }
}
