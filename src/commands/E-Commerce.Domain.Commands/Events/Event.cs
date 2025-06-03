using E_Commerce.Domain.Interfaces;

namespace E_Commerce.Domain.Events
{
    public abstract class Event : IEvent
    {
        public int Id { get; private set; } = default(int);
        public required string AggregateId { get; init; }
        public required int Sequence { get; init; }
        public required DateTime DateTime { get; init; }
        public required int Version { get; init; }
        public required string UserId { get; init; }

        dynamic IEvent.Data => ((dynamic)this).Data;
        public TEvent As<TEvent>() where TEvent : Event => (TEvent)this;
    }

    public abstract class Event<T> : Event
    {
        public required T Data { get; init; }
    }
}
