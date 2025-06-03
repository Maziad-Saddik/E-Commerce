using E_Commerce.Domain.Events;

namespace E_Commerce.Infrastructure.Entities;

public class OutboxMessage
{
    private OutboxMessage()
    { }

    public OutboxMessage(Event @event)
    {
        Event = @event;
    }

    public int Id { get; private set; }

    public Event? Event { get; private set; }
}
