using AutoMapper;
using CarCatalogService.Dtos;
using CarCatalogService.Models;

namespace CarCatalogService.Profiles
{
    public class CarsProfile: Profile
    {
        public CarsProfile()
        {
            // Source -> Target
            CreateMap<Car, CarReadDto>();
            CreateMap<CarCreateDto, Car>();
            CreateMap<CarUpdateDto, Car>();
            CreateMap<CarReadDto, CarPublishedDto>();
            CreateMap<Car, CarPublishedDto>();
        }

    }
}
