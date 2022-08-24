using System.ComponentModel.DataAnnotations;
using TestDriveService.Enums;

namespace TestDriveService.Dtos
{
    public class CarCreateDto
    {
        [Required]
        public string Make { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        [Required]
        public decimal EngineVolume { get; set; }
        [Required]
        public KppType KppType { get; set; }
        [Required]
        public FuelType FuelType { get; set; }
    }
}
