using E_Commerce.Applications.Contracts;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Interfaces;
using E_Commerce.Infrastructure.Entities;
using E_Commerce.Infrastructure.Services.ServiceBus;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence;

public class EventStore(AppDbContext dbContext, IServiceBusPublisher serviceBusPublisher) : IEventStore
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IServiceBusPublisher _serviceBusPublisher = serviceBusPublisher;

    public async Task<List<Event>> GetAllAsync(string aggregateId, CancellationToken cancellationToken)
           => await _dbContext.Events
            .AsNoTracking()
            .Where(x => x.AggregateId.Equals(aggregateId))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task CommitAsync(IAggregate aggregate, CancellationToken cancellationToken)
    {
        foreach (var @event in aggregate.GetUncommittedEvents())
        {
            await _dbContext.Events.AddAsync(@event, cancellationToken);

            await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(@event), cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _serviceBusPublisher.StartPublishing();
    }

    public async Task CommitAsync(Event @event, CancellationToken cancellationToken)
        => await CommitAsync([@event], cancellationToken);

    public async Task CommitAsync(IReadOnlyList<Event> events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            await _dbContext.Events.AddAsync(@event, cancellationToken);

            await _dbContext.OutboxMessages.AddAsync(new OutboxMessage(@event), cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
