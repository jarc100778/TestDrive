using CarCatalogService.Models;

namespace CarCatalogService.Data
{
    public interface ICarRepo
    {
        Task SaveChanges();
        Task<Car?> GetCarById(int id);
        Task<IEnumerable<Car>> GetAllCars();
        Task CreateCar(Car car);
        void DeleteCar(Car car);
    }
}
