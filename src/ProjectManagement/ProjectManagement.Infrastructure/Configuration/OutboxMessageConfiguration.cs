namespace Yosef.ProjectManagement.Infrastructure.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yosef.ProjectManagement.Domain.Outbox;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(nameof(OutboxMessage));

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Payload).IsRequired();

        // Index for quick lookup
        builder.HasIndex(m => new { m.OccurredOn });
    }
}
