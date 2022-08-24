using TestDriveService.Enums;

namespace TestDriveService.Dtos
{
    /// <summary>
    /// Car-Dto для отправки/получения в Раббит. 
    /// </summary>
    public class CarPublishedDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal EngineVolume { get; set; }
        public KppType KppType { get; set; }
        public FuelType FuelType { get; set; }
        /// <summary>
        /// Описание события (создали , обновили, удалили)   
        /// </summary>
        public string Event { get; set; } = string.Empty;
    }
}
