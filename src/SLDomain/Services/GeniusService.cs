using System;
using System.Linq;
using HtmlAgilityPack;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.Services {
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
                throw LyricsNotFoundException.Create(ServiceName, media);
            }
            if (lyricDivs.Count > 1) {
                //todo: Do we need to select one in this case?
                Console.WriteLine("todo..");
            }
            var lyricNode = lyricDivs.First();
            var lyrics = lyricNode.InnerText;
            media.Lyrics = lyrics;
        }
    }
}