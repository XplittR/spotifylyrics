using PropertyChanged;

namespace SpotifyLyricsDomain.Models {
    [AddINotifyPropertyChangedInterface]
    public class Media {
        public readonly string Artist;
        public readonly string Song;

        public Media(string artist, string song) {
            Artist = artist;
            Song   = song;
        }

        //todo: UrlArtist and UrlSong-get properties?
        public string DisplayTitle { get { return $"{Artist} - {Song}"; } }
        public string Url { get; set; }
        public string Lyrics { get; set; }
        public override string ToString() {
            return $"Media: {Artist} - {Song}";
        }
    }
}