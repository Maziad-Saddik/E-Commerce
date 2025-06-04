using E_Commerce.Applications.Contracts;
using E_Commerce.Domain.Aggregates;
using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Exceptions;
using MediatR;

namespace E_Commerce.Applications.Services
{
    public class PlaceOrderCommandHandler(IEventStore eventStore) : IRequestHandler<PlaceOrderCommand, string>
    {
        public async Task<string> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            List<Event> events = await eventStore.GetAllAsync(
                command.OrderId,
                cancellationToken
            );

            if (events.Count > 0)
                throw new AppException(ExceptionStatusCode.AlreadyExists, "Order Already Exists");

            Order order = Order.Place(command);

            await eventStore.CommitAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
