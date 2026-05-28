// File: FileScanner.cs
#nullable disable
namespace AppCleaner
{
    public static class FilePatterns
    {
        public const string PatternCs = "*.cs";
        public const string PatternRazor = "*.razor";
        public const string PatternBak = "*.bak";
        public const string PatternAll = "*.*";
        public static readonly string[] AllPatterns = new[] { PatternCs, PatternRazor, PatternBak, PatternAll };
    }
}
