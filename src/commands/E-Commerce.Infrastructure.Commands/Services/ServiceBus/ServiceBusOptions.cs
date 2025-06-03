using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Infrastructure.Services.ServiceBus;

public class ServiceBusOptions
{

    public const string ServiceBus = "ServiceBus";

    public const string ServiceBusLiveTesting = "LiveTesting:ServiceBus";

    [Required]
    public string TopicName { get; init; } = string.Empty;

    public string SubscriptionName { get; init; } = string.Empty;

    public string DemoConnectionString { get; init; } = string.Empty;

    public void SetLiveTestOptions(IConfiguration configuration)
    {
        configuration.GetSection(ServiceBusLiveTesting).Bind(this);
    }
}
