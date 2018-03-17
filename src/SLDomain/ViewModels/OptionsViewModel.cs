using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PropertyChanged;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsDomain.ViewModels {
    [AddINotifyPropertyChangedInterface]
    public class OptionsViewModel {
        private const string SettingsFilename = "settings.json";
        public static OptionsViewModel Instance { get; set; }

        public static void Create() {
            bool readFromFile = File.Exists(SettingsFilename);
            OptionsViewModel vm;
            if (readFromFile) {
                vm = ReadSettingsFromFile();
            } else {
                vm = new OptionsViewModel();
                vm.CreateDefaultSettings();
            }
            vm.SelectedService = vm.Services.First();
            Instance = vm;
        }

        private void CreateDefaultSettings() {
            int order = 1;
            Services = new List<LyricsService> {
                new GeniusService { Order = order++ },
                new MusixMatchService { Order = order++ },
            };
        }

        public void SaveSettings() {
            var json = JsonConvert.SerializeObject(Instance, Formatting.Indented, new LyricsServiceConverter());
            File.WriteAllText(SettingsFilename, json, Encoding.UTF8);
        }


        private static OptionsViewModel ReadSettingsFromFile() {
            var json = File.ReadAllText(SettingsFilename, Encoding.UTF8);
            var vm = JsonConvert.DeserializeObject<OptionsViewModel>(json, new LyricsServiceConverter());
            return vm;
        }

        public List<LyricsService> Services { get; set; }
        [JsonIgnore]
        public LyricsService SelectedService { get; set; }

        public void OrderUp(LyricsService service) {
            if (!service.CanOrderUp) return;
            var swapWith = Services.First(s => s.Order == (service.Order - 1));
            service.Order--;
            swapWith.Order++;
            Services = Services.OrderBy(s => s.Order).ToList();
        }

        public void OrderDown(LyricsService service) {
            if (!service.CanOrderDown) return;
            var swapWith = Services.First(s => s.Order == (service.Order + 1));
            service.Order++;
            swapWith.Order--;
            Services = Services.OrderBy(s => s.Order).ToList();
        }
    }
}
