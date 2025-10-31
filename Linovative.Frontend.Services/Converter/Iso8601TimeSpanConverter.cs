using Newtonsoft.Json;
using System.Xml;

namespace Linovative.Frontend.Services.Converter
{
    public class Iso8601TimeSpanConverter : JsonConverter<TimeSpan?>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(XmlConvert.ToString(value.Value)); // Write as ISO 8601 string
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var isoDuration = reader.Value.ToString();
            return XmlConvert.ToTimeSpan(isoDuration); // Convert ISO 8601 string to TimeSpan
        }
    }
}
