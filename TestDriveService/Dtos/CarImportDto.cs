using TestDriveService.Enums;

namespace TestDriveService.Dtos
{
    /// <summary>
    /// Машина , полученная от сервиса CarCatalogService
    /// </summary>
    public class CarImportDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal EngineVolume { get; set; }
        public KppType KppType { get; set; }
        public FuelType FuelType { get; set; }
    }
}
