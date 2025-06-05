using Microsoft.Extensions.Configuration;

namespace E_Commerce.Infrastructure.Queries.Services.Logger;

public class AppConfiguration
{
    public static IConfiguration Build()
        => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
}
