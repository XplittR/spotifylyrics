using System;

namespace SpotifyLyricsDomain.Helpers {
    public static class StringHelpers {
        public static int SubstringCount(this string haystack, string needle) {
            //Counts occurrances of "needle" in "haystack". http://stackoverflow.com/a/35871647/4353819
            return haystack.Split(new[] { needle }, StringSplitOptions.None).Length - 1;
        }

        public static string[] Split(this string str, string separator, StringSplitOptions splitOptions = StringSplitOptions.None) {
            return str.Split(new[] { separator }, splitOptions);
        }

        public static string TrimEnd(this string str, string substring) {
            if (str.EndsWith(substring))
                return str.Substring(0, str.Length - substring.Length);
            return str;
        }

        public static bool EqualsIgnCase(this string orig, string compareTo) {
            return String.Equals(orig, compareTo, StringComparison.OrdinalIgnoreCase);
        }
    }
}
