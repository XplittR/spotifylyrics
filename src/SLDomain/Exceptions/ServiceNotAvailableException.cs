using System;
using System.Collections.Generic;
using SpotifyLyricsDomain.Models;

namespace SpotifyLyricsDomain.Exceptions {
    public class ServiceNotAvailableException : Exception {
        public static Dictionary<string, DateTime> ServiceExceptions = new Dictionary<string, DateTime>();
        private ServiceNotAvailableException(string message, Exception innerException) : base(message, innerException) { }

        public static ServiceNotAvailableException Create(string service, Media media, Exception innerException) {
            ServiceExceptions[service] = DateTime.Now; //todo: for not querying this service until a certain time has passed.
            var msg = $"Service unavailable. Service: \"{service}\". Artist: \"{media.Artist}\". Song: \"{media.Song}\".";
            return new ServiceNotAvailableException(msg, innerException);
        }
    }
}