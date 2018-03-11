using System.Collections.Generic;
using System.Linq;
using PropertyChanged;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsDomain.ViewModels {
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
}
