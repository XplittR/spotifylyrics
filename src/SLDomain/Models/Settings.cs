using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyLyricsDomain.Services;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsDomain.Models {

    public class Settings {
        public Settings(OptionsViewModel viewModel) {
            Services = viewModel.Services.Select(s => s.CreateSetting()).ToList();
        }

        public List<ServiceSetting> Services;
    }

    public class ServiceSetting {

    }
    public class LyricsServiceConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            //todo
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object) {
                t.WriteTo(writer);
            } else {
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var temp = reader.Value;
            dynamic instance = serializer.Deserialize<dynamic>(reader);
            var name = instance.ServiceName;
            if (name == "Genius") {
                return serializer.Deserialize<GeniusService>(reader);
            }
            return null;
            //throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
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
