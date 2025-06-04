using E_Commerce.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain
{
    public class MoneyJsonConverter : JsonConverter<Money>
    {
        public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return string.IsNullOrEmpty(value) ? default : Money.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
