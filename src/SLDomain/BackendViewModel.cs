using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace SpotifyLyricsDomain {
    [AddINotifyPropertyChangedInterface]
    public class BackendViewModel  {
        public string WindowTitle { get; set; }
        public string Lyrics { get; set; }

        public void UpdateTitle() {
            var newTitle = Backend.GetWindowTitle();
            if (WindowTitle != newTitle) {
                WindowTitle = newTitle;
                UpdateLyrics();
            }
        }

        private void UpdateLyrics() {
            Lyrics = Backend.GetLyrics(WindowTitle);
        }
    }
}
