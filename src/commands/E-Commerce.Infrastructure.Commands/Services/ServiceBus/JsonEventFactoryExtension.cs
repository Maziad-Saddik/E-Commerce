using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Services.ServiceBus
{
    public static class JsonEventFactoryExtension
    {
        public static Event ToEvent(this JsonDocument json, string subject, ILogger logger)
        {
            try
            {
                var type = typeof(Event).Assembly.ExportedTypes.SingleOrDefault(x => x.Name == subject);

                return type is null
                    ? throw new TypeLoadException("Event type could not be loaded.")
                    : (Event?)json.Deserialize(type, Const.JsonSerializerOptions)
                    ?? throw new InvalidOperationException("Event deserialization failed.");

            }
            catch (Exception e)
            {
                logger.LogError(e, "failed to deserialize event");

                throw;
            }
        }
    }

}


