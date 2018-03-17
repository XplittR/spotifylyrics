using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> list, int startIndex) {
            var localList = list.ToList();
            return localList.GetRange(startIndex, localList.Count - startIndex)
                .Concat(localList.GetRange(0, startIndex));
        }
    }
}
