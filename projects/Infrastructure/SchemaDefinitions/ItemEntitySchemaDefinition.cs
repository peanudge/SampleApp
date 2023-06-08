using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SchemaDefinition;

public class ItemEntitySchemaDefinition : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items", CatalogContext.DefaultSchema);
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(p => p.Genre)
            .WithMany(g => g.Items)
            .HasForeignKey(p => p.GenreId);

        builder.HasOne(p => p.Artist)
            .WithMany(a => a.Items)
            .HasForeignKey(p => p.ArtistId);

        builder.Property(p => p.Price).HasConversion(
            p => $"{p.Amount}:{p.Currency}",
            p => new Price
            {
                Amount = Convert.ToDecimal(p.Split(':', StringSplitOptions.None)[0]),
                Currency = p.Split(':', StringSplitOptions.None)[1]
            });
    }
}