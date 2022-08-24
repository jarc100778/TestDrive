using AutoMapper;
using System.Text.Json;
using TestDriveService.Data;
using TestDriveService.Dtos;
using TestDriveService.Enums;
using TestDriveService.Models;

namespace TestDriveService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CarPublished:
                    await AddCar(message);
                    break;
                case EventType.CarUpdated:
                    await UpdateCar(message);
                    break;
                case EventType.CarDeleted:
                    await DeleteCar(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch (eventType.Event)
            {
                case "Car_Published":
                    Console.WriteLine("--> Car Published Event Detected");
                    return EventType.CarPublished;
                case "Car_Updated":
                    Console.WriteLine("--> Car Updated Event Detected");
                    return EventType.CarUpdated;
                case "Car_Deleted":
                    Console.WriteLine("--> Car Deleted Event Detected");
                    return EventType.CarDeleted;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task AddCar(string carPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICarRepo>();

                var carPublishedDto = JsonSerializer.Deserialize<CarPublishedDto>(carPublishedMessage);

                try
                {
                    var car = _mapper.Map<Car>(carPublishedDto);
                    if ( await repo.ExternalCarExists(car.ExternalId) == null )
                    {
                        await repo.CreateCar(car);
                        await repo.SaveChanges();
                        Console.WriteLine("--> Car added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Car already exists...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Car to DB {ex.Message}");
                }
            }
        }

        private async Task UpdateCar(string carPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICarRepo>();
                var carPublishedDto = JsonSerializer.Deserialize<CarPublishedDto>(carPublishedMessage);

                try
                {
                    var carModel = await repo.ExternalCarExists(carPublishedDto.Id);

                    if ( !(carModel == null) )
                    {
                        _mapper.Map(carPublishedDto, carModel);
                        await repo.SaveChanges();
                        Console.WriteLine("--> Car updated!");
                    }
                    else
                    {
                        Console.WriteLine("--> Car not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update Car to DB {ex.Message}");
                }
            }
        }

        private async Task DeleteCar(string carPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICarRepo>();
                var carPublishedDto = JsonSerializer.Deserialize<CarPublishedDto>(carPublishedMessage);

                try
                {
                    var carModel = await repo.ExternalCarExists(carPublishedDto.Id);

                    if (!(carModel == null))
                    {
                        repo.DeleteCar(carModel);
                        await repo.SaveChanges();
                        Console.WriteLine("--> Car deleted!");
                    }
                    else
                    {
                        Console.WriteLine("--> Car not exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not delete Car to DB {ex.Message}");
                }
            }
        }

    }

}
