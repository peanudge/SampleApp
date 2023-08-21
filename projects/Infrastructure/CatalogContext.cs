using Domain.Models;
using Domain.Repositories;
using Infrastructure.SchemaDefinition;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        public const string DefaultSchema = "catalog";

        // TODO: Replace Item to ItemEntity When apply Hexagon Architecture
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public CatalogContext(DbContextOptions<CatalogContext> options) :
            base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new ArtistEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new GenreEntitySchemaDefinition());
            modelBuilder.ApplyConfiguration(new CategoryEntitySchemaDefinition());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
