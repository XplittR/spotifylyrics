using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Helpers;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.Services {
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
            lyrics       = lyrics.Replace("\\n", Environment.NewLine).Replace("\\", "");
            media.Lyrics = lyrics;
        }
    }
}