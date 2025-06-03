using E_Commerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Persistence.Configuration;

public class OutboxMessageConfigurations : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasOne(x => x.Event)
            .WithOne()
            .HasForeignKey<OutboxMessage>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
