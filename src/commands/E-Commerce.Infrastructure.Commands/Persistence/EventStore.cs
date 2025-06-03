namespace E_Commerce.Infrastructure.Persistence;

//public class EventStore(
//    AppDbContext dbContext,
//    IServiceBusPublisher serviceBusPublisher,
//) : IEventStore
//{
//    private readonly AppDbContext _dbContext = dbContext;

//    private readonly IServiceBusPublisher _serviceBusPublisher = serviceBusPublisher;

//    public async Task<IReadOnlyList<Event>> GetPaginatedAllAsync(
//        int pageNumber,
//        int pageSize,
//        CancellationToken cancellationToken
//    ) => await _dbContext.Events
//                .AsNoTracking()
//                .OrderBy(x => x.Id)
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync(cancellationToken);


//    public async Task<List<Event>> GetByAggregateIdFromSpecifiedSequenceAsync(
//        string aggregateId,
//        CancellationToken cancellationToken
//    )
//    {
//        return await _dbContext.Events
//                .Where(x => x.AggregateId.Equals(aggregateId) && x.Sequence > sequence)
//                .OrderBy(x => x.Id)
//                .ToListAsync(cancellationToken);
//    }

//    public async Task CommitAsync(Transaction aggregate, CancellationToken cancellationToken)
//    {
//        foreach (var @event in aggregate.GetUncommittedEvents())
//        {
//            await _dbContext.Events.AddAsync(@event, cancellationToken);

//            await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(@event), cancellationToken);
//        }

//        await TakeSnapshotAsync(aggregate, cancellationToken);

//        await _dbContext.SaveChangesAsync(cancellationToken);

//        _serviceBusPublisher.StartPublishing();
//    }

//    private async Task TakeSnapshotAsync(Transaction transaction, CancellationToken cancellationToken)
//    {
//        IReadOnlyList<Event> events = transaction.GetUncommittedEvents();

//        if (events.Any(x => x.Sequence % _snapshotEventLimit != 0))
//            return;

//        await _dbContext.Snapshots.AddAsync(Snapshot.Create(transaction), cancellationToken);
//    }

//    public async Task CommitAsync(IReadOnlyList<Event> events, CancellationToken cancellationToken = default)
//    {
//        foreach (var @event in events)
//        {
//            await _dbContext.Events.AddAsync(@event, cancellationToken);

//            await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(@event), cancellationToken);
//        }

//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }
//}
