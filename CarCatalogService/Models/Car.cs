using CarCatalogService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarCatalogService.Models
{
    public class Car
    {
        [Key]
        [Required]
        public int Id { get; set; }
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
    }
}
