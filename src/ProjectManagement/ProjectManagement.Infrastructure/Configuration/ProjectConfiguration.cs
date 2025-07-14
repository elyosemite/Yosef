using ProjectManagement.Infrastructure;
using ProjectManagement.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManagement.Infrastructure.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<ProjectDataModel>
{
    public void Configure(EntityTypeBuilder<ProjectDataModel> builder)
    {
        builder.ToTable(nameof(ProjectDataModel));

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
