using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;

namespace E_Commerce.Infrastructure.Services.Logger;

public class LoggerServiceBuilder
{
    public static ILogger Build()
    {
        var configuration = AppConfiguration.Build();

        var loggerConfiguration = configuration.GetSection("Logger");

        var appName = loggerConfiguration["AppName"];

        var appNamespace = loggerConfiguration["AppNamespace"];

        var writeToConsole = loggerConfiguration.GetValue<bool>("WriteToConsole");

        var seqUrl = loggerConfiguration["SeqUrl"];

        var logger = new LoggerConfiguration()
            .Enrich.WithProperty("name", appName ?? "anis-x")
            .Enrich.WithProperty("namespace", appNamespace ?? "anis-x")
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(configuration);

        if (writeToConsole)
            logger.WriteTo.Console();

        if (!string.IsNullOrEmpty(seqUrl))
            logger.WriteTo.Seq(seqUrl);

        SelfLog.Enable(Console.Error);

        return logger.CreateLogger();
    }
}