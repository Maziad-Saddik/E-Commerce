namespace E_Commerce.Infrastructure.Entities;

public class OutboxMessageEntity
{
    public OutboxMessageEntity()
    { }
    public OutboxMessageEntity(Event @event)
    {
        Event = @event;
    }

    public int Id { get; set; }
    public Event? Event { get; set; }
}
