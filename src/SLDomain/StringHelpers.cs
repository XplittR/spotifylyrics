using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLyricsDomain {
    public static class StringHelpers {
        public static int SubstringCount(this string haystack, string needle) {
            //Counts occurrances of "needle" in "haystack". http://stackoverflow.com/a/35871647/4353819
            return haystack.Split(new[] { needle }, StringSplitOptions.None).Length - 1;
        }

        public static string[] Split(this string str, string separator, StringSplitOptions splitOptions = StringSplitOptions.None) {
            return str.Split(new[] { separator }, splitOptions);
        }
    }
}
