using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace SpotifyLyricsDomain {
    [AddINotifyPropertyChangedInterface]
    public class BackendViewModel {
        public string WindowTitle { get; set; }
        public Media MyMedia { get; set; }

        public void UpdateTitle() {
            var newTitle = Backend.GetWindowTitle();
            if (WindowTitle != newTitle) {
                WindowTitle = newTitle;
                UpdateLyrics();
            }
        }

        private void UpdateLyrics() {
            SetLoadingLyrics();
            MyMedia = Backend.GetLyrics(WindowTitle);
        }

        public void SetLoadingLyrics() {
            MyMedia = new Media("Loading", "Loading") { Lyrics = "Loading..." };
        }
    }
}
