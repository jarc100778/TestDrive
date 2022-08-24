using CarCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {

        }

        public DbSet<Car> Cars => Set<Car>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property<decimal>("EngineVolume")
                .HasColumnType("decimal(2,1)");
        }
    }
}
