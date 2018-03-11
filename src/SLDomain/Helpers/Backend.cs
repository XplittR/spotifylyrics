using System;
using System.Linq;
using System.Text.RegularExpressions;
using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Helpers {
    public static class Backend {
        public static Media GetLyrics(string artistAndSong) {
            string artist = string.Empty, song = string.Empty;
            if (artistAndSong.SubstringCount(" - ") == 1) {
                var parts = artistAndSong.Split(" - ");
                artist = parts[0];
                song = parts[1];
            } else if (artistAndSong.SubstringCount(" - ") == 2) {
                var parts = artistAndSong.Split(" - ");
                artist = parts[0];
                song = parts[1];
            }

            if (song.SubstringCount(" / ") > 1) {
                var parts = song.Split(" / ");
                song = parts[0];
            }

            song = Regex.Replace(song, @"\s+", " "); //Replace all whitespace with a single space
            artist = artist.Trim();
            song = song.Trim();
            return LoadLyrics(new Media(artist, song));
        }

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
