using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestDriveService.Data;
using TestDriveService.Dtos;
using TestDriveService.SyncDataServices;

namespace TestDriveService.Controllers
{
    [Route("api/t/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepo _carRepo;
        private readonly IMapper _mapper;
        private readonly ICarDataClient _carDataClient;

        public CarsController(ICarRepo carRepo, IMapper mapper, ICarDataClient carDataClient)
        {
            _carRepo = carRepo;
            _mapper = mapper;
            _carDataClient = carDataClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarReadDto>>> GetCars()
        {
            Console.WriteLine("--> Getting Cars from DB ");

            var cars = await _carRepo.GetAllCars();

            return Ok(_mapper.Map<IEnumerable<CarReadDto>>(cars));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarImportDto>>> ImportCars()
        {
            Console.WriteLine("--> Getting Cars from CarCatalogService ");

            var cars = await _carDataClient.GetAllCars();
            //ToDo в цикле сохранить машины в БД (если нужно).

            return Ok(cars);
        }

        // метод чисто для тестирования наличия прав доступа. Создание машины через сервис CarCatalogService.
        [HttpPost]
        public async Task<ActionResult<CarImportDto>> CreateCarToRemoteDB(CarCreateDto carCreateDto)
        {
            Console.WriteLine("--> Creating Car by CarCatalogService ");

            CarImportDto car = await _carDataClient.CreateCar(carCreateDto);

            return Ok(car);
        }
    }
}
