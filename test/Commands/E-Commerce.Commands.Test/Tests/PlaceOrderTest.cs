using Calzolari.Grpc.Net.Client.Validation;
using E_Commerce.Commands.Test.Helper;
using E_Commerce.Infrastructure.Persistence;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace E_Commerce.Commands.Test.Tests
{
    public class PlaceOrderTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly DatabaseHelper _databaseHelper;

        private readonly GrpcHelper _grpcHelper;

        private readonly WebApplicationFactory<Program> _factory;

        public PlaceOrderTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.SetUnitTestsIsolatedEnvironment();
            });

            _databaseHelper = new DatabaseHelper(_factory);

            _grpcHelper = new GrpcHelper(_factory);
        }

        [Fact]
        public async Task VerifyDatabaseConnection()
        {
            using var scope = _factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var canConnect = await dbContext.Database.CanConnectAsync();

            Assert.True(canConnect, "Database connection failed.");
        }

        [Theory]
        [InlineData("")]
        public async Task PlaceOrder_InvalidData_ThrowsInvalidArgument(
           string errorPropertyName
        )
        {
            RpcException exception = await Assert.ThrowsAsync<RpcException>
               (() => _grpcHelper.Send(x => x.PlaceOrderAsync(new Protos.PlaceOrderRequest())).ResponseAsync);

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);

            Assert.Contains(
              exception.GetValidationErrors(),
              e => e.PropertyName.EndsWith(errorPropertyName)
            );
        }
    }
}
