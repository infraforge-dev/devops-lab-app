using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        //TODO: Validate lengths for properties in application layer. Affects performance and indexing.
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.PictureUrl).HasMaxLength(2048);
        builder.Property(x => x.Type).HasMaxLength(50);
        builder.Property(x => x.Brand).HasMaxLength(50);
        builder.Property(x => x.QuantityInStock).HasDefaultValue(0);
    }
}
