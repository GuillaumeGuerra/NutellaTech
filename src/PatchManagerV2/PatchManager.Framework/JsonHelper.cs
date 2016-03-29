using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PatchManager.Framework
{
    public static class JsonHelper<T>
    {
        private static Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(() =>
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }, LazyThreadSafetyMode.PublicationOnly);

        public static string ObjectToJson(T item)
        {
            return JsonConvert.SerializeObject(item, Formatting.Indented, _settings.Value);
        }

        public static T JsonToObject(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings.Value);
        }
    }
}
