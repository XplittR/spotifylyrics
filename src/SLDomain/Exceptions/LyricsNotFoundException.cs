using System;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.Exceptions {
    public class LyricsNotFoundException : Exception {
        private LyricsNotFoundException(string message) : base(message) { }

        public static LyricsNotFoundException Create(string service, Media media) {
            var msg = $"Could not find lyrics. Service: \"{service}\". Artist: \"{media.Artist}\". Song: \"{media.Song}\". Attempted URL: \"{media.Url}\".";
            return new LyricsNotFoundException(msg);
        }
    }
}