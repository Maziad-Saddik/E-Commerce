using E_Commerce.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain
{
    public class EmailJsonConverter : JsonConverter<Email>
    {
        public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return string.IsNullOrEmpty(value) ? default : new Email(value);
        }

        public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
