using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Applications.Extensions;

public static class RegisterationExtensions
{
    public static void MediatrRegister(this IServiceCollection services)
    {
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<>());
    }
}

