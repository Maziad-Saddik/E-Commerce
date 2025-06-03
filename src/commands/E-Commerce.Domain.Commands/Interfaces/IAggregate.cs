using E_Commerce.Domain.Events;

namespace E_Commerce.Domain.Interfaces;

public interface IAggregate
{
    string Id { get; }
    int Sequence { get; }
    IReadOnlyList<Event> GetUncommittedEvents();
    void MarkChangesAsCommitted();
}
