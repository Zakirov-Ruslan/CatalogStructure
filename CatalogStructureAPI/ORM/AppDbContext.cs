using Microsoft.EntityFrameworkCore;

namespace CatalogStructureAPI.ORM
{
    public class AppDbContext : DbContext
    {
        public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogItem>()
                .HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
