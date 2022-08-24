using TestDriveService.Dtos;

namespace TestDriveService.SyncDataServices
{
    public interface ICarDataClient
    {
        Task<IEnumerable<CarImportDto>?> GetAllCars();
        Task<CarImportDto?> CreateCar(CarCreateDto carCreateDto);
    }
}
