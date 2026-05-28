using System.Globalization;
using System.Text;

namespace AppCleaner
{
    public sealed class IniFile
    {
        private readonly string _filePath;
        private readonly Dictionary<string, Dictionary<string, string>> _sections =
            new(StringComparer.OrdinalIgnoreCase);

        public IniFile(string filePath)
        {
            _filePath = filePath;
            Load();
        }

        public string Read(string section, string key, string defaultValue = "")
        {
            return _sections.TryGetValue(section, out var values) &&
                   values.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }

        public void Write(string section, string key, object? value)
        {
            if (!_sections.TryGetValue(section, out var values))
            {
                values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                _sections[section] = values;
            }

            values[key] = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public void Save()
        {
            var lines = new List<string>();

            foreach (var section in _sections)
            {
                lines.Add($"[{section.Key}]");

                foreach (var pair in section.Value)
                    lines.Add($"{pair.Key}={Escape(pair.Value)}");

                lines.Add(string.Empty);
            }

            File.WriteAllLines(_filePath, lines, Encoding.UTF8);
        }

        private void Load()
        {
            if (!File.Exists(_filePath))
                return;

            string currentSection = string.Empty;

            foreach (var rawLine in File.ReadAllLines(_filePath, Encoding.UTF8))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line[1..^1];

                    if (!_sections.ContainsKey(currentSection))
                        _sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    continue;
                }

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                if (!_sections.TryGetValue(currentSection, out var values))
                {
                    values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    _sections[currentSection] = values;
                }

                values[parts[0].Trim()] = Unescape(parts[1]);
            }
        }

        public static string Escape(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("=", "\\=");
        }

        public static string Unescape(string value)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != '\\' || i == value.Length - 1)
                {
                    sb.Append(value[i]);
                    continue;
                }

                var next = value[++i];

                sb.Append(next switch
                {
                    'r' => '\r',
                    'n' => '\n',
                    '=' => '=',
                    '\\' => '\\',
                    _ => next
                });
            }

            return sb.ToString();
        }
    }
}
