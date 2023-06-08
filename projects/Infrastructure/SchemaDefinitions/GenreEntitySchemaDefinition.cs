using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.SchemaDefinition;

public class GenreEntitySchemaDefinition : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", CatalogContext.DefaultSchema);
        builder.HasKey(k => k.GenreId);
        builder.Property(p => p.GenreDescription)
            .IsRequired()
            .HasMaxLength(1000);
    }
}