using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpotifyLyricsDomain.Helpers {
    public static class SpotifyHelpers {
        public static string GetWindowTitle() {
            var procs = FindProcesses();
            var title = procs.Select(p => p.MainWindowTitle).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            title = title?.Replace('—', '-').Replace("Spotify - ", "");
            if (string.IsNullOrWhiteSpace(title) || title == "Spotify") return null;
            return title;
        }

        private static IEnumerable<Process> FindProcesses() {
            var possibleProcessNames = GenerateProcNames("Spotify");
            Process[] procs = null;
            int i = 0;
            while (procs == null || procs.Length == 0 || procs[0].PriorityClass == ProcessPriorityClass.Idle) {
                procs = Process.GetProcessesByName(possibleProcessNames[i]);
                i++;
                if (i >= possibleProcessNames.Length)
                    break;
            }
            return procs;
        }

        private static string[] GenerateProcNames(params string[] alternatives) {
            var list = new List<string>();
            foreach (var alternative in alternatives) {
                var altVariations = new[] { alternative, alternative.ToLower(), alternative.ToUpper() };
                foreach (var alt in altVariations) {
                    list.AddRange(new[] { alt, alt + ".exe", alt + ".EXE" });
                }
            }
            return list.ToArray();
        }
    }
}