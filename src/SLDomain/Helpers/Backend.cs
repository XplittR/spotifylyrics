using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.Services;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Helpers {
    public static class Backend {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        private static LyricsService _lastUsedService;
        public static Media LoadLyrics(Media media) {
            return LoadLyrics(media, OptionsViewModel.Instance.Services.Where(s => s.IsEnabled));
        }

        public static Media ChangeLyrics(Media media) {
            _lastUsedService = _lastUsedService ?? OptionsViewModel.Instance.Services.FirstOrDefault(s => s.IsEnabled);
            if (_lastUsedService == null) return media;
            var lastIdx = OptionsViewModel.Instance.Services.IndexOf(_lastUsedService);
            var ouroborosList = OptionsViewModel.Instance.Services.Cycle(lastIdx + 1);
            return LoadLyrics(media, ouroborosList);
        }

        private static Media LoadLyrics(Media media, IEnumerable<LyricsService> services) {
            foreach (var service in services) {
                _lastUsedService = service;
                try {
                    media = service.GetLyrics(media);
                    break;
                } catch (LyricsNotFoundException ex) {
                    Log.Error(ex);
                    continue;
                } catch (ServiceNotAvailableException ex) {
                    Log.Error(ex);
                    continue;
                } catch (Exception ex) when (LogEx(ex)) { }
            }
            return media;
        }

        private static bool LogEx(Exception ex) {
            Log.Error(ex);
            return false;
        }
    }
}
