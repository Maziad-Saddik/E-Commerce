using E_Commerce.Commands.Test.Protos;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.ValueObjects;

namespace E_Commerce.Commands.Test.Helper;

public static class AssertEquality
{
    public static void OfOrderPlaced(
        Event orderPlaced,
        PlaceOrderRequest request,
        PlaceOrderResponse response
    )
    {
        OrderPlaced @event = Assert.IsType<OrderPlaced>(orderPlaced);

        Assert.Equal(request.UserId, @event.UserId);

        Assert.Equal(request.OrderId, @event.AggregateId);

        Assert.Equal(response.Id, @event.AggregateId);

        Assert.Equal(request.Customer.Id, @event.Data.Customer.Id.ToString());

        Assert.Equal(request.Customer.Name, @event.Data.Customer.Name);

        Assert.Equal(new Email(request.Customer.Email), @event.Data.Customer.Email);

        foreach (var item in request.OrderItems)
        {
            Domain.Entities.OrderItem? orderItem = @event.Data.OrderItems.FirstOrDefault(x => x.ProductRef == item.ProductRefenence);

            Assert.NotNull(orderItem);

            Assert.Equal(item.Quantity, orderItem.Quantity);

            Assert.Equal(new Money((decimal)item.Price, item.Currency), orderItem.Price);
        }
    }
}
