using Anis.UnregisteredAccountsTransactions.Commands.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Persistence.Configuration;

public class BaseEventConfigurations : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasIndex(e => new { e.AggregateId, e.Sequence }).IsUnique();

        builder.Property<string>("EventType").HasMaxLength(128);

        builder.HasDiscriminator<string>("EventType");
    }
}
