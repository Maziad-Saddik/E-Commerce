using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Infrastructure.Queries.Services.ServiceBus
{
    public class ServiceBusOptions
    {
        public const string ServiceBus = "ServiceBus";
        public const string ServiceBusLiveTesting = "TestSettings:LiveTesting:ServiceBus";

        [Required]
        public required string TopicName { get; init; }

        [Required]
        public required string SubscriptionName { get; init; }

        [Required]
        public required bool EnableDeadLetter { get; init; }

        public void SetLiveTestOptions(IConfiguration configuration)
        {
            configuration.GetSection(ServiceBusLiveTesting).Bind(this);
        }
    }
}

