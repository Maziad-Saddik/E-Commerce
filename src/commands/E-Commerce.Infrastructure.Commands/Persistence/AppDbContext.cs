using Anis.UnregisteredAccountsTransactions.Commands.Domain.Events;
using Anis.UnregisteredAccountsTransactions.Commands.Domain.Events.Data;
using Anis.UnregisteredAccountsTransactions.Commands.Domain.Models;
using Anis.UnregisteredAccountsTransactions.Commands.Domain.Models.SnapShots;
using Anis.UnregisteredAccountsTransactions.Commands.Infrastructure.Persistence.Configuration;
using E_Commerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<AccountPhoneNumber> AccountPhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfigurations());

        modelBuilder.ApplyConfiguration(new BaseEventConfigurations());

        modelBuilder.ApplyConfiguration(new BaseSnapshotConfigurations());

        modelBuilder.ApplyConfiguration(new AccountPhoneNumberConfigurations());

        modelBuilder.ApplyConfiguration(new GenericEventConfiguration<RegisterTransactionRequested, RegisterTransactionRequestedData>());

        modelBuilder.ApplyConfiguration(new GenericEventConfiguration<TransactionEdited, TransactionEditedData>());
    }
}
