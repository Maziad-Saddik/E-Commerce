using E_Commerce.Commands.Test.FakerService;
using E_Commerce.Infrastructure.Persistence;
using E_Commerce.Infrastructure.Services.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Commands.Test.Helper
{
    public static class ServiceCollectionExtensions
    {
        public static void SetUnitTestsIsolatedEnvironment(
            this IServiceCollection services
        )
        {
            UseInMemoryTesting(services);

            services.RejectServiceBus();

            services.RemoveDatabaseMigrator();
        }

        public static void SetLiveTestsIsolatedEnvironment(
            this IServiceCollection services
        )
        {
            UseInMemoryTesting(services);

            services.RemoveDatabaseMigrator();
        }

        private static void RejectServiceBus(this IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ImplementationType == typeof(ServiceBusPublisher));

            services.Remove(descriptor);

            services.AddSingleton<IServiceBusPublisher, FakeServiceBusPublisher>();
        }

        private static void RemoveDatabaseMigrator(this IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ImplementationType == typeof(DatabaseMigrationHostedService));

            services.Remove(descriptor);
        }

        private static void UseInMemoryTesting(IServiceCollection services)
        {
            var descriptor = services.Where(
                d => d.ServiceType.FullName is not null
                && d.ServiceType.FullName.Contains(nameof(DbContext))
            ).ToList();

            foreach (var item in descriptor)
            {
                services.Remove(item);
            }

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(dbName));
        }
    }
}