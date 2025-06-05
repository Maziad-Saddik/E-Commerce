using E_Commerce.Infrastructure.Queries.Persistence;
using E_Commerce.Infrastructure.Queries.Persistence.Repositories;
using E_Commerce.Infrastructure.Queries.Services.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure.Queries.Extensions
{
    public static class InfraContainer
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(" "));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHostedService<DatabaseMigrationHostedService>();

            services.AddServiceBus(configuration);

            return services;
        }
    }
}