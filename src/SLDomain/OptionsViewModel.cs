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
            SelectedService = Services.First();
        }

        private void CreateDefaultSettings() {
            Services = new List<LyricsService> {
                new GeniusService(),
                new MusixMatchService(),
            };

        }

        public List<LyricsService> Services { get; set; }
        public LyricsService SelectedService { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public abstract class LyricsService {
        public bool IsEnabled { get; set; } = true;
        protected abstract void GetLyricsInternal(Media media);

        private readonly Dictionary<string, Media> _lyricsCache = new Dictionary<string, Media>();
        public Media GetLyrics(Media media) {
            if (!_lyricsCache.ContainsKey(media.Artist + media.Song)) {
                GetLyricsInternal(media);
                media.Lyrics = media.Lyrics.Replace("&amp;", "&").Replace("`", "'").Trim();
                _lyricsCache.Add(media.Artist + media.Song, media);
            }
            return _lyricsCache[media.Artist + media.Song];
        }

        public string ServiceName {
            get {
                return GetType().Name.TrimEnd("Service");
            }
        }
    }

    public class GeniusService : LyricsService {
        protected override void GetLyricsInternal(Media media) {
            var urlArtist = media.Artist.Replace(' ', '-');
            var urlSong = media.Song.Replace(' ', '-');
            var url = $"http://genius.com/{urlArtist}-{urlSong}-lyrics";
            media.Url = url;
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
            media.Lyrics = lyrics;
            //todo: return url for clickable
        }


    }

    public class MusixMatchService : LyricsService {
        protected override void GetLyricsInternal(Media media) {
            const string userAgent = "curl/7.9.8 (i686-pc-linux-gnu) libcurl 7.9.8 (OpenSSL 0.9.6b) (ipv6 enabled)";
            var artist = media.Artist.Replace(' ', '-');
            var song = media.Song.Replace(' ', '-');
            var url = $"https://www.musixmatch.com/search/{artist}-{song}/tracks";
            media.Url = url;

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
            media.Lyrics = lyrics;
        }
    }

    public static class HttpHelpers {
        public static string GetHtml(string url, string userAgent) {
            //todo: To HttpExtensions, or something?
            //Todo: Extend to allow to set other headers than useragent? Dictionary<HttpRequestHeader,string>?
            string html;
            using (var client = new WebClient()) {
                client.Headers[HttpRequestHeader.UserAgent] = userAgent;
                html = client.DownloadString(url);
            }
            return html;
        }
    }
}
