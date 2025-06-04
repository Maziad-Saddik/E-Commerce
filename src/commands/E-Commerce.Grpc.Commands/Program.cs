using E_Commerce.Applications.Extensions;
using E_Commerce.Infrastructure.Extensions;
using E_Commerce.Infrastructure.Services.Logger;
using Extensions;
using Middleware;
using Serilog;
using Services;

Log.Logger = LoggerServiceBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.InfrastructureRegister(builder.Configuration);

builder.Services.MediatrRegister();

builder.Services.AddGrpcWithValidators();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<CultureInfoManager>();

app.MapGrpcService<OrdersCommandService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }
