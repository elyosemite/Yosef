using DotNetEcosystemStudy.Infrastructure.Model;
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

        builder.Property(o => o.OrganizationName)
            .IsRequired();

        builder.Property(o => o.ContributorsCount);

        builder.Property(o => o.Secret);

        builder.HasMany(o => o.Projects)
            .WithOne(p => p.Organization)
            .HasForeignKey(p => p.Identifier)
            .OnDelete(DeleteBehavior.Cascade);
    }
}