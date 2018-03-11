using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLyricsDomain.Helpers {
    public static class ListHelpers {
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB) {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
        public static IList<T> Swap<T>(this IList<T> list, T a, T b) {
            return list.Swap(list.IndexOf(a), list.IndexOf(b));
        }
    }
}
