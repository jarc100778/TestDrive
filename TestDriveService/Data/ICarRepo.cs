using TestDriveService.Models;

namespace TestDriveService.Data
{
    public interface ICarRepo
    {
        Task SaveChanges();
        Task<Car?> GetCarById(int id);
        Task<IEnumerable<Car>> GetAllCars();
        Task CreateCar(Car car);
        void DeleteCar(Car car);
        Task<Car?> ExternalCarExists(int externalCarId);
    }

}
