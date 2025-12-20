using Newtonsoft.Json;
using System.Xml;

namespace LinoVative.Shared.Dto.JsonConverters
{
    public class Iso8601TimeSpanNewtonsoftConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var ts = (TimeSpan)value;
            writer.WriteValue(XmlConvert.ToString(ts)); // writes "PT3H" style
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return objectType == typeof(TimeSpan?) ? (TimeSpan?)null : TimeSpan.Zero;

            var s = reader.Value?.ToString();
            if (string.IsNullOrWhiteSpace(s))
                return objectType == typeof(TimeSpan?) ? (TimeSpan?)null : TimeSpan.Zero;

            // Try ISO8601 first
            try
            {
                return XmlConvert.ToTimeSpan(s);
            }
            catch
            {
                // Fallback: try TimeSpan.Parse for "hh:mm:ss" style
                if (TimeSpan.TryParse(s, out var ts))
                    return ts;

                // Last resort: throw or return default
                throw new JsonSerializationException($"Invalid TimeSpan value: {s}");
            }
        }
    }
}
