using E_Commerce.Applications.Contracts;
using E_Commerce.Domain.Aggregates;
using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Exceptions;
using MediatR;

namespace E_Commerce.Applications.Services
{
    public class CancelOrderCommandHandler(IEventStore eventStore) : IRequestHandler<CancelOrderCommand, string>
    {
        public async Task<string> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            List<Event> events = await eventStore.GetAllAsync(
                command.OrderId,
                cancellationToken
            );

            if (events.Count == 0)
                throw new AppException(ExceptionStatusCode.FailedPrecondition, "No Events");

            Order order = Order.LoadFromHistory(events);

            order.Cancel(command);

            await eventStore.CommitAsync(order, cancellationToken);

            return order.Id;
        }
    }

}
