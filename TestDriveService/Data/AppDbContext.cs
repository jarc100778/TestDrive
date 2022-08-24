using Microsoft.EntityFrameworkCore;
using TestDriveService.Models;

namespace TestDriveService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<TestDriveOrder> Orders => Set<TestDriveOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId);

            modelBuilder.Entity<TestDriveOrder>()
                .HasOne(x => x.Car) 
                .WithMany(x => x.Orders)    
                .HasForeignKey(x => x.CarId);

            modelBuilder.Entity<Car>()
                .Property<decimal>("EngineVolume")
                .HasColumnType("decimal(2,1)");
        }
    }
}
