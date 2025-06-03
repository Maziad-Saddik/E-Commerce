using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Events;
using E_Commerce.Infrastructure.Entities;
using E_Commerce.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Customer> AccountPhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfigurations());

        modelBuilder.ApplyConfiguration(new BaseEventConfigurations());

        // modelBuilder.ApplyConfiguration(new GenericEventConfiguration<RegisterTransactionRequested, RegisterTransactionRequestedData>());
    }
}
