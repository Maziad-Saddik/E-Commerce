using E_Commerce.Domain.Queries.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Infrastructure.Queries.Services.ServiceBus
{
    public static class JsonEventFactoryExtension
    {
        private static JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static T? Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);

        public static Event ToEvent(this JsonDocument json, string subject, ILogger logger)
        {
            try
            {
                var type = typeof(Event).Assembly.ExportedTypes.SingleOrDefault(x => x.Name == subject);

                return type is null
                    ? throw new TypeLoadException("Event type could not be loaded.")
                    : (Event?)json.Deserialize(type, JsonSerializerOptions) ?? throw new InvalidOperationException("Event deserialization failed.");

            }
            catch (Exception e)
            {
                logger.LogError(e, "failed to deserialize event");
                throw;
            }
        }
    }
}
