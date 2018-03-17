using System;
using System.Linq;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Helpers {
    public static class Backend {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public static Media LoadLyrics(Media media) {

            foreach (var service in OptionsViewModel.Instance.Services.Where(s => s.IsEnabled)) {
                try {
                    media = service.GetLyrics(media);
                    break;
                } catch (LyricsNotFoundException ex) {
                    Log.Error(ex);
                    continue;
                } catch (ServiceNotAvailableException ex) {
                    Log.Error(ex);
                    continue;
                } catch (Exception ex) {
                    Log.Error(ex);
                    throw;
                }
            }

            return media;
        }
    }
}
