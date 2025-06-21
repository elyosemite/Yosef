using DotNetEcosystemStudy.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetEcosystemStudy.Infrastructure.Configuration;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable(nameof(Organization));

        builder.HasKey(o => o.Identifier);
        
        builder.HasIndex(o => o.Identifier)
            .IsUnique();

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.ContributorsCount);

        builder.Property(o => o.Secret);

        builder.HasMany(o => o.Projects);
    }
}