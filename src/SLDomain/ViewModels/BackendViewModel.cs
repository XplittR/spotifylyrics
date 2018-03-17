using PropertyChanged;
using SpotifyLyricsDomain.Helpers;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.ViewModels {
    [AddINotifyPropertyChangedInterface]
    public class BackendViewModel {
        public string WindowTitle { get; set; }
        public Media MyMedia { get; set; }

        public void UpdateTitle() {
            var newTitle = SpotifyHelpers.GetWindowTitle();
            if (WindowTitle != newTitle) {
                WindowTitle = newTitle;
                UpdateLyrics();
            }
        }

        private void UpdateLyrics() {
            SetLoadingLyrics();
            if (string.IsNullOrWhiteSpace(WindowTitle))
                return;
            MyMedia = Media.Create(WindowTitle);
            MyMedia = Backend.LoadLyrics(MyMedia);
        }

        public void ChangeLyrics() {
            SetLoadingLyrics();
            if (string.IsNullOrWhiteSpace(WindowTitle))
                return;
            MyMedia = Media.Create(WindowTitle);
            MyMedia = Backend.ChangeLyrics(MyMedia);
        }

        public void SetLoadingLyrics() {
            MyMedia = new Media("Loading", "Loading") { Lyrics = "Loading..." };
        }
    }
}
