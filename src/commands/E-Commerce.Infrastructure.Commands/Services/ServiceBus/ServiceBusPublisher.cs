using Azure.Messaging.ServiceBus;
using E_Commerce.Domain.Interfaces;
using E_Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Infrastructure.Services.ServiceBus;

public class ServiceBusPublisher : IServiceBusPublisher
{
    protected readonly ServiceBusSender _sender;
    protected readonly IServiceProvider _provider;
    private readonly ILogger<ServiceBusOptions> _logger;
    private readonly object _lockObject = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private bool IsBusy { get; set; }

    public ServiceBusPublisher(
        IOptions<ServiceBusOptions> options,
        IServiceProvider provider,
        IConfiguration configuration,
        ILogger<ServiceBusOptions> logger
    )
    {
        ServiceBusClient client = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));

        _sender = client.CreateSender(options.Value.TopicName);

        _provider = provider;

        _logger = logger;
    }

    public void StartPublishing()
    {

        Task.Run(() =>
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                lock (_lockObject)
                {
                    PublishEvents().GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Event sender error .");
            }
            finally
            {
                IsBusy = false;
            }
        });
    }

    private async Task PublishEvents()
    {
        using var scope = _provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        while (true)
        {

            var messages = await dbContext.OutboxMessages
                .Include(x => x.Event)
                .OrderBy(x => x.Id)
                .Take(100)
                .ToListAsync();

            if (messages.Count == 0) return;

            foreach (var message in messages)
            {
                if (message.Event is null)
                {
                    throw new InvalidOperationException("Event is null, please include the event in the query");
                }

                IEvent @event = message.Event;

                var json = JsonSerializer.Serialize(@event, _jsonSerializerOptions);

                var serviceBusMessage = new ServiceBusMessage(json)
                {
                    PartitionKey = message.Event.AggregateId,
                    SessionId = message.Event.AggregateId,
                    Subject = message.Event.GetType().Name,
                };

                await _sender.SendMessageAsync(serviceBusMessage);

                dbContext.OutboxMessages.Remove(message);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}