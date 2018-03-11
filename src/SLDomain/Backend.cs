using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PropertyChanged;

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

        public static Media GetLyrics(string artistAndSong) {
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
            return LoadLyrics(new Media(artist, song));
        }

        public static void ShuffleServices() {

        }

        public static Media LoadLyrics(Media media) {

            foreach (var service in OptionsViewModel.Instance.Services.Where(s => s.IsEnabled)) {
                try {
                    media = service.GetLyrics(media);
                    break;
                } catch (LyricsNotFoundException ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                } catch (ServiceNotAvailableException ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return media;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class Media {
        public readonly string Artist;
        public readonly string Song;

        public Media(string artist, string song) {
            Artist = artist;
            Song = song;
        }

        //todo: UrlArtist and UrlSong-get properties?
        public string DisplayTitle { get { return $"{Artist} - {Song}"; } }
        public string Url { get; set; }
        public string Lyrics { get; set; }
        public override string ToString() {
            return $"Media: {Artist} - {Song}";
        }
    }

    public class LyricsNotFoundException : Exception {
        private LyricsNotFoundException(string message) : base(message) { }

        public static LyricsNotFoundException Create(string service, Media media, string attemptedUrl) {
            var msg = $"Could not find lyrics. Service: \"{service}\". Artist: \"{media.Artist}\". Song: \"{media.Song}\". Attempted URL: \"{attemptedUrl}\".";
            return new LyricsNotFoundException(msg);
        }
    }

    public class ServiceNotAvailableException : Exception {
        public static Dictionary<string, DateTime> ServiceExceptions = new Dictionary<string, DateTime>();
        private ServiceNotAvailableException(string message, Exception innerException) : base(message, innerException) { }

        public static ServiceNotAvailableException Create(string service, Media media, Exception innerException) {
            ServiceExceptions[service] = DateTime.Now; //todo: for not querying this service until a certain time has passed.
            var msg = $"Service unavailable. Service: \"{service}\". Artist: \"{media.Artist}\". Song: \"{media.Song}\".";
            return new ServiceNotAvailableException(msg, innerException);
        }
    }
}
