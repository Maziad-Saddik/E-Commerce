using E_Commerce.Applications.Contracts;
using E_Commerce.Domain.Aggregates;
using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Events;
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

            Order order = Order.LoadFromHistory(events);

            await eventStore.CommitAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
