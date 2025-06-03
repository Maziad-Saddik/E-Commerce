using E_Commerce.Infrastructure.Persistence;
using E_Commerce.Infrastructure.Services.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Extensions
{
    public static class InfraRegistration
    {
        public static void InfrastructureRegister(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                option => option.UseInMemoryDatabase(""));

            //services.AddScoped<IEventStore, EventStore>();

            services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();

            services.AddSingleton<DemoServiceBusPublisher>();

            services.AddHostedService<DatabaseMigrationHostedService>();

            services.AddHostedService<PendingMessagesPublisher>();

            services.AddOptions<ServiceBusOptions>()
                .BindConfiguration(ServiceBusOptions.ServiceBus)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
