using E_Commerce.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Infrastructure.Persistence.Configuration
{
    public class GenericEventConfiguration<TEntity, TData> : IEntityTypeConfiguration<TEntity>
            where TEntity : Event<TData>
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Data).HasConversion(
                 v => JsonSerializer.Serialize(v, JsonSerializerOptions),
                 v => JsonSerializer.Deserialize<TData>(v, JsonSerializerOptions)!
            ).HasColumnName("Data");
        }
    }
}
