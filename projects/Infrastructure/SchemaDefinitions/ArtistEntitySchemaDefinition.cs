using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SchemaDefinition;

public class ArtistEntitySchemaDefinition : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists", CatalogContext.DefaultSchema);
        builder.HasKey(k => k.ArtistId);
        builder.Property(p => p.ArtistName)
            .IsRequired()
            .HasMaxLength(200);
    }
}