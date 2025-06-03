using Microsoft.Extensions.Configuration;

namespace E_Commerce.Infrastructure.Services.Logger;

public static class AppConfiguration
{
    public static IConfiguration Build()
        => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
}
