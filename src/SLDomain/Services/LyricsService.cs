﻿using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;
using SpotifyLyricsDomain.Helpers;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Services {
    [AddINotifyPropertyChangedInterface]
    public abstract class LyricsService {
        public bool IsEnabled { get; set; } = true;
        public int Order { get; set; }

        [JsonIgnore]
        public bool CanOrderUp { get { return Order > 1; } }
        [JsonIgnore]
        public bool CanOrderDown { get { return Order < OptionsViewModel.Instance.Services.Count; } }
        protected abstract void GetLyricsInternal(Media media);

        private readonly Dictionary<string, Media> _lyricsCache = new Dictionary<string, Media>();
        public Media GetLyrics(Media media) {
            if (!_lyricsCache.ContainsKey(media.Artist + media.Song)) {
                GetLyricsInternal(media);
                media.Lyrics = media.Lyrics.Replace("&amp;", "&").Replace("`", "'").Trim();
                _lyricsCache.Add(media.Artist + media.Song, media);
            }
            return _lyricsCache[media.Artist + media.Song];
        }

        public string ServiceName {
            get {
                return GetType().Name.TrimEnd("Service");
            }
        }

        public abstract ServiceSetting CreateSetting();
    }
}