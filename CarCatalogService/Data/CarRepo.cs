using CarCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CarCatalogService.Data
{
    public class CarRepo : ICarRepo
    {
        private readonly AppDbContext _context;

        public CarRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Car>> GetAllCars()
        {
            return await _context.Cars.ToListAsync();   
        }

        public async Task<Car?> GetCarById(int id)
        {
            return await _context.Cars.FirstOrDefaultAsync( x => x.Id == id);
        }

        public async Task CreateCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            await _context.AddAsync(car);
        }

        public void DeleteCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car));
            }

            _context.Remove(car);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
