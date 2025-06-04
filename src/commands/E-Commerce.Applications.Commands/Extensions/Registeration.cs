using E_Commerce.Applications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Applications.Extensions;

public static class RegistrationExtensions
{
    public static void MediatrRegister(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PlaceOrderCommandHandler>());
    }
}

