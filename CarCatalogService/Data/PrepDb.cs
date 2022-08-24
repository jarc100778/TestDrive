using CarCatalogService.Models;
using CarCatalogService.Models.Enums;

namespace CarCatalogService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(WebApplication app, bool isProd)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (!isProd && !context.Cars.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Cars.AddRange(
                    new Car { Make = "BMW", Model = "X3", EngineVolume = 3, FuelType = FuelType.Diesel, KppType = KppType.Automatic },
                    new Car { Make = "Exeed", Model = "TXL", EngineVolume = 1.6M, FuelType = FuelType.Petrol, KppType = KppType.Robot },
                    new Car { Make = "Geely", Model = "Tugella", EngineVolume = 2, FuelType = FuelType.Petrol, KppType = KppType.Automatic }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
