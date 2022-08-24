using AutoMapper;
using CarCatalogService.AsyncDataServices;
using CarCatalogService.Data;
using CarCatalogService.Dtos;
using CarCatalogService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController: ControllerBase
    {
        private readonly ICarRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public CarsController(ICarRepo carRepo, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _repository = carRepo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [Authorize("cars.read")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarReadDto>>> GetCars()
        {
            Console.WriteLine("--> Getting Cars....");

            var cars = await _repository.GetAllCars();

            return Ok(_mapper.Map<IEnumerable<CarReadDto>>(cars));
        }

        [Authorize("cars.read")]
        [HttpGet("{id}", Name = "GetCarById")]
        public async Task<ActionResult<CarReadDto>> GetCarById(int id)
        {
            var car = await _repository.GetCarById(id);
            if (car != null)
            {
                return Ok(_mapper.Map<CarReadDto>(car));
            }

            return NotFound();
        }

        [Authorize("cars.all")]
        [HttpPost]
        public async Task<ActionResult<CarReadDto>> CreateCar(CarCreateDto carCreateDto)
        {
            var carModel = _mapper.Map<Car>(carCreateDto);
            await _repository.CreateCar(carModel);
            await _repository.SaveChanges();

            var carReadDto = _mapper.Map<CarReadDto>(carModel);

            // Send Async Message
            try
            {
                var carPublishedDto = _mapper.Map<CarPublishedDto>(carReadDto);
                carPublishedDto.Event = "Car_Published";
                _messageBusClient.PublishCar(carPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetCarById), new { Id = carReadDto.Id }, carReadDto);
        }

        [Authorize("cars.all")]
        [HttpPut]
        public async Task<ActionResult<CarReadDto>> UpdateCar(int id, CarUpdateDto carUpdateDto)
        {
            var carModel = await _repository.GetCarById(id);
            if (carModel == null)
            {
                return NotFound();
            }

            _mapper.Map(carUpdateDto, carModel);
            await _repository.SaveChanges();

            // Send Async Message
            try
            {
                var carPublishedDto = _mapper.Map<CarPublishedDto>(carModel);
                carPublishedDto.Event = "Car_Updated";
                _messageBusClient.PublishCar(carPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return NoContent();
        }

        [Authorize("cars.all")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            var carModel = await _repository.GetCarById(id);
            if (carModel == null)
            {
                return NotFound();
            }
            _repository.DeleteCar(carModel);
            await _repository.SaveChanges();

            // Send Async Message
            try
            {
                var carPublishedDto = _mapper.Map<CarPublishedDto>(carModel);
                carPublishedDto.Event = "Car_Deleted";
                _messageBusClient.PublishCar(carPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return NoContent();
        }
    }
}
