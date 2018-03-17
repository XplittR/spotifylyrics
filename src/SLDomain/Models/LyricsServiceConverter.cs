using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsDomain.Models {
    public class LyricsServiceConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            JObject item = JObject.Load(reader);
            var serviceName = item["ServiceName"].Value<string>();
            if (serviceName == "Genius") {
                return item.ToObject<GeniusService>();
            } else if (serviceName == "MusixMatch") {
                return item.ToObject<MusixMatchService>();
            } else {
                return item.ToObject<DeprecatedService>();
            }
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override bool CanConvert(Type objectType) {
            return typeof(LyricsService).IsAssignableFrom(objectType);
        }
    }
}
