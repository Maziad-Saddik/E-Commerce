using Microsoft.Extensions.Hosting;

namespace E_Commerce.Infrastructure.Services.ServiceBus
{
    public class PendingMessagesPublisher(IServiceBusPublisher serviceBusPublisher) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            serviceBusPublisher.StartPublishing();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
