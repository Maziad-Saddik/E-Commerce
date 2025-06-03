using Azure.Messaging.ServiceBus;
using E_Commerce.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Infrastructure.Services.ServiceBus
{
    public class DemoServiceBusPublisher
    {
        protected readonly ServiceBusSender _sender;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public DemoServiceBusPublisher(IOptions<ServiceBusOptions> options)
        {
            ServiceBusClient client = new(options.Value.DemoConnectionString);

            _sender = client.CreateSender(options.Value.TopicName);
        }

        public async Task PublishEventAsync(IEvent @event)
        {
            string json = JsonSerializer.Serialize(@event, _jsonSerializerOptions);

            ServiceBusMessage serviceBusMessage = new(json)
            {
                PartitionKey = @event.AggregateId.ToString(),
                SessionId = @event.AggregateId.ToString(),
                Subject = @event.GetType().Name,
            };

            await _sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
