using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SpotifyLyricsDomain {
    public static class Backend {
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

        public static string GetLyrics(string artistAndSong) {
            string artist = string.Empty, song = string.Empty;
            if (artistAndSong.SubstringCount(" - ") == 1) {
                var parts = artistAndSong.Split(" - ");
                artist = parts[0];
                song = parts[1];
            } else if (artistAndSong.SubstringCount(" - ") == 2) {
                var parts = artistAndSong.Split(" - ");
                artist = parts[0];
                song = parts[1];
            }

            if (song.SubstringCount(" / ") > 1) {
                var parts = song.Split(" / ");
                song = parts[0];
            }

            song = Regex.Replace(song, @"\s+", " "); //Replace all whitespace with a single space
            artist = artist.Trim();
            song = song.Trim();
            return LoadLyrics(new Media { Artist = artist, Song = song });
        }

        public static string LoadLyrics(Media media) {
            var services = new List<Func<Media, string>> {
                Services.Genius,
                Services.Genius2,
            };
            var currentService = services.First();
            //foreach
            var lyrics = currentService(media);
            lyrics = lyrics.Replace("&amp;", "&").Replace("`", "'").Trim();
            return lyrics;
        }
    }

    public class Media {
        public string Artist;
        public string Song;
    }

    public static class Services {
        public static string Genius(Media media) {
            var artist = media.Artist.Replace(' ', '-');
            var song = media.Song.Replace(' ', '-');
            var url = $"http://genius.com/{artist}-{song}-lyrics";

            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            return html;
            //todo: return url for clickable
        }
        public static string Genius2(Media media) {
            return Genius(media);
        }
    }
}
