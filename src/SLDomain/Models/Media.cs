using System.Text.RegularExpressions;
using PropertyChanged;
using SpotifyLyricsDomain.Helpers;

namespace SpotifyLyricsDomain.Models {
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

        public static Media Create(string artistAndSong) {
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

            //todo: Remove "feat."-parts?

            song = Regex.Replace(song, @"\s+", " "); //Replace all whitespace with a single space
            artist = artist.Trim();
            song = song.Trim();
            return new Media(artist, song);
        }
    }
}