using System.Globalization;
using System.Text.RegularExpressions;
namespace GH.Components
{
    public static class StringExtensions
    {
        public static string ToCamalCase(this string original)
        {
            if (string.IsNullOrEmpty(original))
                return original;
            string result = _properNameRx.Replace(original.ToLower(CultureInfo.CurrentCulture), HandleWord);
            return result;
        }
        public static string WordToCamalCase(this string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;
            if (word.Length > 1)
                return Char.ToUpper(word[0], CultureInfo.CurrentCulture) + word.Substring(1);
            return word.ToUpper(CultureInfo.CurrentCulture);
        }
        private static readonly Regex _properNameRx = new Regex(@"\b(\w+)\b");
        private static readonly string[] _prefixes = { "mc" };
        private static string HandleWord(Match m)
        {
            string word = m.Groups[1].Value;
            foreach (string prefix in _prefixes)
            {
                if (word.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
                    return prefix.WordToCamalCase() + word.Substring(prefix.Length).WordToCamalCase();
            }
            return word.WordToCamalCase();
        }
    }
}
