using System;
using System.Linq;
using System.Text.RegularExpressions;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Helpers {
    public static class Backend {

        public static Media LoadLyrics(Media media) {

            foreach (var service in OptionsViewModel.Instance.Services.Where(s => s.IsEnabled)) {
                try {
                    media = service.GetLyrics(media);
                    break;
                } catch (LyricsNotFoundException ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                } catch (ServiceNotAvailableException ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return media;
        }
    }
}
