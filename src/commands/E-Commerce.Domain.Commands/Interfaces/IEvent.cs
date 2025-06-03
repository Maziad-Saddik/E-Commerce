namespace E_Commerce.Domain.Interfaces
{
    public interface IEvent
    {
        string AggregateId { get; }
        string UserId { get; }
        DateTime DateTime { get; }
        int Sequence { get; }
        int Version { get; }
        dynamic Data { get; }
    }
}
