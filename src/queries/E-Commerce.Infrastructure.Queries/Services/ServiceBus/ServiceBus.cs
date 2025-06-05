using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Infrastructure.Queries.Services.ServiceBus
{
    public class ServiceBus(IConfiguration configuration)
    {
        public ServiceBusClient Client { get; } = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
    }
}
