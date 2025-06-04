using Calzolari.Grpc.AspNetCore.Validation;
using Interceptors;
using Validation;

namespace Extensions;

public static class RegisterationExtensions
{
    public static void AddGrpcWithValidators(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.EnableMessageValidation();
            options.Interceptors.Add<HandleErrorInterceptor>();
        });

        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddGrpcValidation();
        services.AddValidator<PlaceOrderRequestValidator>();
    }
}

