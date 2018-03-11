using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using PropertyChanged;
using SpotifyLyricsDomain.Helpers;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsDomain.ViewModels {
    [AddINotifyPropertyChangedInterface]
    public class OptionsViewModel {
        public static OptionsViewModel Instance { get; set; }

        public OptionsViewModel() {
            bool readFromFile = false;//File.Exists("settings.json");
            if (readFromFile) {
                //ReadSettingsFromFile();
                //todo: Read settings from file
            } else {
                CreateDefaultSettings();
            }
            SelectedService = Services.First();
        }

        private void CreateDefaultSettings() {
            int order = 1;
            Services = new List<LyricsService> {
                new GeniusService { Order = order++ },
                new MusixMatchService { Order = order++ },
            };
        }

        public void SaveSettings() {
            /*var settings = new Settings(this);
            var json = JsonConvert.SerializeObject(Instance, new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> {
                    new LyricsServiceConverter(){}
                }
            });
            ReadSettingsFromFile(json);*/
        }
        

        private void ReadSettingsFromFile(string jsonTemp) {
            /*var optVM = JsonConvert.DeserializeObject<OptionsViewModel>(jsonTemp, new LyricsServiceConverter());*/

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
