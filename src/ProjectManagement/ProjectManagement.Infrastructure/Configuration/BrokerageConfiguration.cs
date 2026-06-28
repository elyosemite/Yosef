using ProjectManagement.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectManagement.Infrastructure.Configuration;

public class BrokerageConfiguration : IEntityTypeConfiguration<BrokerageDataModel>
{
    public void Configure(EntityTypeBuilder<BrokerageDataModel> builder)
    {
        builder.ToTable("OrganizationDataModel");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.Identifier).ValueGeneratedNever();
        builder.Property(o => o.BrokerageName).HasColumnName("OrganizationName");
        builder.Property(o => o.ContributorsCount);
        builder.Property(o => o.Secret);

        builder.HasMany(o => o.Projects)
            .WithOne(p => p.Organization)
            .HasForeignKey(p => p.Id)
            .HasPrincipalKey(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
