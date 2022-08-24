using CarCatalogService.Dtos;

namespace CarCatalogService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishCar(CarPublishedDto carPublishedDto);
    }
}
