using AutoMapper;
using TestDriveService.Dtos;
using TestDriveService.Models;

namespace TestDriveService.Profiles
{
    public class TestDriveOrderProfiles: Profile
    {
        public TestDriveOrderProfiles()
        {
            // Source -> Target
            CreateMap<TestDriveOrder, TestDriveOrderReadDto>();
            CreateMap<TestDriveOrderCreateDto, TestDriveOrder>();
            CreateMap<TestDriveOrderUpdateDto, TestDriveOrder>();
            CreateMap<CarPublishedDto, Car>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Car, CarReadDto>();

        }
    }
}
