using CarCatalogService.Models.Enums;

namespace CarCatalogService.Dtos
{
    public class CarReadDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal EngineVolume { get; set; }
        public KppType KppType { get; set; }
        public FuelType FuelType { get; set; }
    }
}
