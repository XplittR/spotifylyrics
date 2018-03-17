using SpotifyLyricsDomain.Exceptions;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.Services {
    public class DeprecatedService : LyricsService {
        protected override void GetLyricsInternal(Media media) {
            throw ServiceNotAvailableException.Create(ServiceName, media, null);
        }
    }
}