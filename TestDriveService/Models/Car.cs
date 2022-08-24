using System.ComponentModel.DataAnnotations;
using TestDriveService.Enums;

namespace TestDriveService.Models
{
    public class Car
    {

        /// <summary>
        /// ИД машины
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// внешний ИД (от сервиса CarCatalogService) 
        /// </summary>
        [Required]
        public int ExternalId { get; set; }
        /// <summary>
        ///Марка автомобиля: БМВ 
        /// </summary>
        [Required]
        public string Make { get; set; } = string.Empty;
        /// <summary>
        /// Модель автомобиля: X3
        /// </summary>
        [Required]
        public string Model { get; set; } = string.Empty;
        /// <summary>
        /// Объем двигателя
        /// </summary>
        [Required]
        public decimal EngineVolume { get; set; }
        /// <summary>
        /// Тип КПП
        /// </summary>
        [Required]
        public KppType KppType { get; set; }
        /// <summary>
        /// Тип топлива
        /// </summary>
        [Required]
        public FuelType FuelType { get; set; }
        /// <summary>
        /// Заявки на тест-драйв
        /// </summary>
        public ICollection<TestDriveOrder> Orders { get; set; } = new List<TestDriveOrder>();
    }
}
