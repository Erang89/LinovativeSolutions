
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace Linovative.Frontend.Services.Converter
{
    public class Iso8601TimeSpanConverter : JsonConverter<TimeSpan?>
    {
        public override TimeSpan? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException($"Unexpected token parsing TimeSpan. Token: {reader.TokenType}");

            var value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            return XmlConvert.ToTimeSpan(value); // e.g. "PT3H"
        }

        public override void Write(
            Utf8JsonWriter writer,
            TimeSpan? value,
            JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(XmlConvert.ToString(value.Value)); // ISO 8601
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
