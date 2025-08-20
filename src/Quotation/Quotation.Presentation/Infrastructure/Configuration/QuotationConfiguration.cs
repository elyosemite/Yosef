using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quotation.Presentation.Infrastructure.Model;

namespace Quotation.Presentation.Infrastructure.Configuration;

public class QuotationConfiguration : IEntityTypeConfiguration<QuotationDataModel>
{
    public void Configure(EntityTypeBuilder<QuotationDataModel> builder)
    {
        builder.ToTable(nameof(QuotationDataModel));

        builder.HasKey(o => o.QuotationId);
        builder.Property(o => o.ProductName);
        builder.Property(o => o.CustomerName);
        builder.Property(o => o.Price);
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(o => o.UpdatedAt);
    }
}