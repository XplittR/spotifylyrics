using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using PropertyChanged;

namespace SpotifyLyricsDomain {
    [AddINotifyPropertyChangedInterface]
    public class OptionsViewModel {
        public static OptionsViewModel Instance { get; set; }

        public OptionsViewModel() {
            bool readFromFile = false;
            if (readFromFile) {
                //todo: Read settings from file
            } else {
                CreateDefaultSettings();
            }
        }

        private void CreateDefaultSettings() {
            Services = new List<LyricsService> {
                new GeniusService(),
                new MusixMatchService(),
            };
        }

        public List<LyricsService> Services { get; set; } = new List<LyricsService>();
    }

    public abstract class LyricsService {
        public abstract string GetLyrics(Media media);

        public string ServiceName {
            get {
                return GetType().Name.TrimEnd("Service");
            }
        }
    }

    public class GeniusService : LyricsService {
        public override string GetLyrics(Media media) {
            media.Artist = media.Artist.Replace(' ', '-');
            media.Song = media.Song.Replace(' ', '-');
            var url = $"http://genius.com/{media.Artist}-{media.Song}-lyrics";
            HtmlDocument doc;
            try {
                var web = new HtmlWeb();
                doc = web.Load(url);
            } catch (Exception ex) {
                throw ServiceNotAvailableException.Create(ServiceName, media, ex);
            }

            var lyricDivs = doc.DocumentNode.Descendants("div").Where(n => (n as HtmlNode).HasClass("lyrics")).ToList();
            if (!lyricDivs.Any()) {
                throw LyricsNotFoundException.Create(ServiceName, media, url);
            }
            if (lyricDivs.Count > 1) {
                //todo: Do we need to select one in this case?
                Console.WriteLine("todo..");
            }
            var lyricNode = lyricDivs.First();
            var lyrics = lyricNode.InnerText;
            return lyrics;
            //todo: return url for clickable
        }


    }

    public class MusixMatchService : LyricsService {
        public override string GetLyrics(Media media) {
            const string userAgent = "curl/7.9.8 (i686-pc-linux-gnu) libcurl 7.9.8 (OpenSSL 0.9.6b) (ipv6 enabled)";
            media.Artist = media.Artist.Replace(' ', '-');
            media.Song = media.Song.Replace(' ', '-');
            var url = $"https://www.musixmatch.com/search/{media.Artist}-{media.Song}/tracks";

            string html;
            try {
                html = HttpHelpers.GetHtml(url, userAgent);
            } catch (Exception ex) {
                throw ServiceNotAvailableException.Create(ServiceName, media, ex);
            }

            var rgx = new Regex(@"""track_share_url"":""([^""]*)""");
            if (!rgx.IsMatch(html)) {
                throw LyricsNotFoundException.Create(ServiceName, media, url);
            }

            var trackShareUrl = rgx.Match(html).Groups[1].Value;
            trackShareUrl = Regex.Unescape(trackShareUrl);
            HtmlDocument doc = new HtmlDocument();
            try {
                html = HttpHelpers.GetHtml(trackShareUrl, userAgent);
                doc.LoadHtml(html);
            } catch (Exception ex) {
                throw ServiceNotAvailableException.Create(ServiceName, media, ex);
            }
            var notAvailable = doc.DocumentNode.Descendants("div").Any(n => (n as HtmlNode).HasClass("mxm-lyrics-not-available"));
            if (notAvailable) {
                throw LyricsNotFoundException.Create(ServiceName, media, url);
            }

            var lyrics = doc.DocumentNode.OuterHtml.Split(new[] { "\"body\":\"" }, StringSplitOptions.None)[1].Split(new[] { "\",\"language\":\"" }, StringSplitOptions.None)[0];
            lyrics = lyrics.Replace("\\n", Environment.NewLine).Replace("\\", "");
            return lyrics;
        }
    }
}
