using Calzolari.Grpc.Net.Client.Validation;
using E_Commerce.Commands.Test.Faker.Requests;
using E_Commerce.Commands.Test.Helper;
using E_Commerce.Commands.Test.Protos;
using E_Commerce.Domain.Events;
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

        [Fact]
        public async Task PlaceOrder_WithValidData_SavesOrderPlaced()
        {
            PlaceOrderRequest request = new PlaceOrderRequestFaker();

            request.OrderItems.Add(new OrderItem
            {
                ProductRefenence = Guid.NewGuid().ToString(),
                Quantity = 1,
                Price = 1000,
                Currency = "USD"
            });

            PlaceOrderResponse response = await _grpcHelper.Send(x => x.PlaceOrderAsync(request).ResponseAsync);

            List<Event> events = await _databaseHelper.GetAllAsync(request.OrderId);

            Assert.Single(events);

            AssertEquality.OfOrderPlaced(orderPlaced: events.Last(), request: request, response);
        }


        [Theory]
        [InlineData(" ", ValidGuid, ValidGuid, "Maziad", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.OrderId))]
        [InlineData(EmptyGuid, ValidGuid, ValidGuid, "Maziad", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.OrderId))]
        [InlineData(ValidGuid, " ", ValidGuid, "Maziad", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.UserId))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.UserId))]
        [InlineData(ValidGuid, EmptyGuid, " ", "Maziad", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.Customer.Id))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, " ", "amns@gmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.Customer.Name))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", " ", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.Customer.Email))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", ValidGuid, 10, 1000, "USD", nameof(PlaceOrderRequest.Customer.Email))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", " ", 10, 1000, "USD", nameof(OrderItem.ProductRefenence))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", EmptyGuid, 10, 1000, "USD", nameof(OrderItem.ProductRefenence))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", ValidGuid, -1, 1000, "USD", nameof(OrderItem.Quantity))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", ValidGuid, 4144, -1, "USD", nameof(OrderItem.Price))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", ValidGuid, 4144, 144, " ", nameof(OrderItem.Currency))]
        [InlineData(ValidGuid, EmptyGuid, ValidGuid, "Maziad", "amnsgmail.com", ValidGuid, 4144, 144, "US", nameof(OrderItem.Currency))]
        public async Task PlaceOrder_InvalidData_ThrowsInvalidArgument(
            string orderId,
            string userId,
            string customerId,
            string name,
            string email,
            string productRef,
            int quantity,
            double price,
            string currency,
           string errorPropertyName
        )
        {
            PlaceOrderRequest request = new PlaceOrderRequestFaker()
                .RuleFor(x => x.OrderId, x => orderId)
                .RuleFor(x => x.UserId, x => userId)
                .RuleFor(x => x.Customer, x => new Customer
                {
                    Id = customerId,
                    Name = name,
                    Email = email,
                });

            request.OrderItems.Add(new OrderItem
            {
                ProductRefenence = productRef,
                Quantity = quantity,
                Price = price,
                Currency = currency,
            });

            RpcException exception = await Assert.ThrowsAsync<RpcException>
               (() => _grpcHelper.Send(x => x.PlaceOrderAsync(request)).ResponseAsync);

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);

            Assert.Contains(
                exception.GetValidationErrors(),
                e => e.PropertyName.EndsWith(errorPropertyName)
            );
        }

        private const string ValidGuid = "4D5AE574-DE1A-41D6-853F-D666EAB4D1DD";
        private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";
    }
}
