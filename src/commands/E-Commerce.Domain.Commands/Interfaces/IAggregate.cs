using Anis.TransactionsDateManagement.Commands.Domain.Events;

namespace E_Commerce.Domain.Interfaces;

public interface IAggregate
{
    TransactionId Id { get; }
    int Sequence { get; }
    IReadOnlyList<Event> GetUncommittedEvents();
    void MarkChangesAsCommitted();
}
