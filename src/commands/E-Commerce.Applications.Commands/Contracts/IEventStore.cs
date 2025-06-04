using E_Commerce.Domain.Events;
using E_Commerce.Domain.Interfaces;

namespace E_Commerce.Applications.Contracts;

public interface IEventStore
{
    Task<List<Event>> GetAllAsync(string aggregateId, CancellationToken cancellationToken);

    Task CommitAsync(IAggregate aggregate, CancellationToken cancellationToken);

    Task CommitAsync(Event @event, CancellationToken cancellationToken);

    Task CommitAsync(IReadOnlyList<Event> events, CancellationToken cancellationToken);
}
