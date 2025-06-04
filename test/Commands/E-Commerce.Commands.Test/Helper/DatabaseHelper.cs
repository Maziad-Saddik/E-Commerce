using E_Commerce.Applications.Contracts;
using E_Commerce.Domain.Events;
using E_Commerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Commands.Test.Helper
{
    public class DatabaseHelper(WebApplicationFactory<Program> factory)
    {
        public async Task<List<Event>> GetAllAsync(string aggregateId)
        {
            using IServiceScope scope = factory.Services.CreateScope();

            IEventStore eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

            return await eventStore.GetAllAsync(aggregateId, default);
        }

        public async Task AddEventAsync(Event @event)
        {
            using IServiceScope scope = factory.Services.CreateScope();

            IEventStore eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

            await eventStore.CommitAsync(@event, default);
        }

        public async Task AddEventsAsync(IReadOnlyList<Event> events)
        {
            using IServiceScope scope = factory.Services.CreateScope();

            IEventStore eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

            await eventStore.CommitAsync(events, default);
        }

        public async Task<List<Event>> GetEventsBySequenceAsync(string aggregateId, int minSequence = 0)
        {
            using IServiceScope scope = factory.Services.CreateScope();

            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await dbContext.Events
                .Where(x => x.Sequence >= minSequence && x.AggregateId.Equals(aggregateId))
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task DeleteEventsAsync()
        {
            using IServiceScope scope = factory.Services.CreateScope();

            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Events.ExecuteDeleteAsync();
        }
    }
}
