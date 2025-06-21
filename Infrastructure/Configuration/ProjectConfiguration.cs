using DotNetEcosystemStudy.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetEcosystemStudy.Infrastructure.Configuration;

// modelBuilder.Entity<Project>()
//         .HasOne(p => p.Organization)                // Cada Project tem um Organization
//         .WithMany(o => o.Projects)                  // Cada Organization tem muitos Projects
//         .HasForeignKey(p => p.OrganizationId)       // Chave estrangeira
//         .OnDelete(DeleteBehavior.Cascade);

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(Project));

        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.Organization)
            .WithMany(o => o.Projects)
            .HasForeignKey(p => p.Id)
            .HasPrincipalKey(p => p.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}
