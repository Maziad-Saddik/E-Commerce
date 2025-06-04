using E_Commerce.Domain.Commands;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Grpc;

namespace Extensions;

public static class CommandExtension
{
    public static PlaceOrderCommand ToCommand(this PlaceOrderRequest request) => new()
    {
        OrderId = request.OrderId,
        UserId = request.UserId,
        Customer = E_Commerce.Domain.Entities.Customer.Add(
           id: Guid.Parse(request.Customer.Id),
           name: request.Customer.Name,
           email: new Email(request.Customer.Email)
        ),
        OrderItems = request.OrderItems.Select(x => E_Commerce.Domain.Entities.OrderItem.Add(
           productRef: x.ProductRefenence,
           quantity: x.Quantity,
           price: new Money((decimal)x.Price, x.Currency))
        ).ToList(),
    };

    public static CancelOrderCommand ToCommand(this CancelOrderRequest request) => new()
    {
        OrderId = request.OrderId,
        UserId = request.UserId
    };
}
